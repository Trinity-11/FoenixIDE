using FoenixIDE.Common;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    public class MemoryRAM : IMappable
    {
        protected byte[] data = null;
        private readonly int startAddress;
        private readonly int length;
        private readonly int endAddress;
        public delegate void PostWriteFn(int address, byte o, byte n);
        public PostWriteFn postWrite = null;

        public int StartAddress
        {
            get
            {
                return this.startAddress;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int EndAddress
        {
            get
            {
                return endAddress;
            }
        }

        public MemoryRAM(int StartAddress, int Length)
        {
            this.startAddress = StartAddress;
            this.length = Length;
            this.endAddress = StartAddress + Length - 1;
            data = new byte[Length];
        }

        public void Zero()
        {
            Array.Clear(data, 0, Length);
        }
        public virtual byte ReadByte(int Address)
        {
            return data[Address];
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int ReadWord(int Address)
        {
            return ReadByte(Address) + (ReadByte(Address + 1) << 8);
        }

        internal void Load(byte[] SourceData, int SrcStart, int DestStart, int length)
        {
            for (int i = 0; i < length; i++)
            {
                this.data[DestStart + i] = SourceData[SrcStart + i];
            }
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            if (postWrite != null)
            {
                byte old = data[Address];
                data[Address] = Value;
                postWrite.Invoke(Address, old, Value);
            }
            else
            {
                data[Address] = Value;
            }
            
        }

        public void WriteWord(int Address, int Value)
        {
            WriteByte(Address, (byte)(Value & 0xff));
            WriteByte(Address + 1, (byte)(Value >> 8 & 0xff));
        }

        internal int ReadLong(int Address)
        {
            return ReadByte(Address) + (ReadByte(Address + 1) << 8) + (ReadByte(Address + 2) << 16); 
        }

        internal void Copy(int SourceAddress, MemoryRAM Destination, int DestAddress, int Length)
        {
            for(int i=0; i<Length; ++i)
            {
                Destination.data[DestAddress + i] = data[SourceAddress + i];
            }
        }

        // When the codec write address is written to, wait 200ms then write a zero to signify that we're finished
        public async void OnCodecWait5SecondsAndWrite00(int address, byte o, byte n)
        {
            await Task.Delay(200);
            data[0] = 0;
        }

        // This is used to simulate the Keyboard Register
        public void OnKeyboardStatusCodeChange(int address, byte o, byte n)
        {
            // In order to avoid an infinite loop, we write to the device directly
            switch (address)
            {
                case 0:
                    byte command = data[0];
                    switch (command)
                    {
                        case 0x69:
                            data[4] = 1;
                            break;
                        case 0xEE: // echo command
                            data[4] = 1;
                            break;
                        case 0xF4:
                            data[0] = 0xFA;
                            data[4] = 1;
                            break;
                        case 0xF6:
                            data[4] = 1;
                            break;
                    }
                    break;
                case 4:
                    byte reg = data[4];
                    switch (reg)
                    {
                        case 0x20:
                            data[4] = 1;
                            break;
                        case 0x60:
                            data[4] = 0;
                            break;
                        case 0xAA:
                            data[0] = 0x55;
                            data[4] = 1;
                            break;
                        case 0xA8:
                            data[4] = 1;
                            break;
                        case 0xA9:
                            data[0] = 0;
                            data[4] = 1;
                            break;
                        case 0xAB:
                            data[0] = 0;
                            break;
                        case 0xD4:
                            data[4] = 1;
                            break;
                    }
                    break;
            }
        }

        public void OnSDCARDCommand(int address, byte o, byte n)
        {
            byte command = data[1];
            switch (command)
            {
                case 0x15:
                    data[0] = 0x51;
                    break;
            }
        }

        public void OnInterruptPending(int address, byte o, byte n)
        {
            if (address >= MemoryLocations.MemoryMap.INT_PENDING_REG0 && address <= MemoryLocations.MemoryMap.INT_PENDING_REG2)
            {
                // If a bit gets set from 0 to 1, leave it.  If a bit gets set a second time, reset to 0.
                byte combo = (byte)(o & n);
                if (combo > 0)
                {
                    data[address] = (byte)(data[address] & (byte)(~combo));
                }
            }
        }
    }
}
