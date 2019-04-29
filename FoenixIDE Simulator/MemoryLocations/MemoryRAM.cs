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
        public delegate void PostWriteFn(int address);
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
            data[Address] = Value;
            postWrite?.Invoke(Address);
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
        public async void OnCodecWait5SecondsAndWrite00(int address)
        {
            await Task.Delay(200);
            data[0] = 0;
        }

        // This is used to simulate the Keyboard Register
        public void OnKeyboardStatusCodeChange(int address)
        {
            // In order to avoid an infinite loop, we write to the device directly
            switch (address)
            {
                case 0:
                    byte command = data[0];
                    switch (command)
                    {
                        case 0xEE: // echo command
                            data[4] = 1;
                            break;
                    }
                    break;
                case 4:
                    byte reg = data[4];
                    switch (reg)
                    {
                        case 0xAA:
                            data[0] = 0x55;
                            data[4] = 1;
                            break;
                        case 0xAB:
                            data[0] = 0;
                            break;
                    }
                    break;
            }
        }

        public void OnSDCARDCommand(int address)
        {
            byte command = data[1];
            switch (command)
            {
                case 0x15:
                    data[0] = 0x51;
                    break;
            }
        }
    }
}
