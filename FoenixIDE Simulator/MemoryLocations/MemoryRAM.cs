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
        private int startAddress;
        private int length;
        private int endAddress;

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
    }
}
