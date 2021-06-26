using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.MemoryLocations;
using System;

namespace FoenixIDE.MemoryLocations
{
    public class MemoryRAM : IMappable
    {
        protected byte[] data = null;
        private readonly int startAddress;
        private readonly int length;
        private readonly int endAddress;

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

        private MemoryRAM()
        {
        }

        /// <summary>
        /// Clear all the bytes in the memory array.
        /// </summary>
        public void Zero()
        {
            Array.Clear(data, 0, Length);
        }

        /// <summary>
        /// Reads a byte from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public virtual byte ReadByte(int Address)
        {
            var d = data;
            if (Address >= 0 && Address < Length)
                return d[Address];
            else
                return 0x40;
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int ReadWord(int Address)
        {
            var d = data;
            //return ReadByte(Address) + (ReadByte(Address + 1) << 8);
            return BitConverter.ToUInt16(d, Address);
        }

        /// <summary>
        /// Read a 24-bit long from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        internal int ReadLong(int Address)
        {
            var d = data;
            //return ReadByte(Address) + (ReadByte(Address + 1) << 8) + (ReadByte(Address + 2) << 16);
            return (int)BitConverter.ToInt32(d, Address) & 0xFF_FFFF;
        }

        internal void Load(byte[] SourceData, int SrcStart, int DestStart, int copyLength)
        {
            var d = data;
            for (int i = 0; i < copyLength; i++)
            {
                d[DestStart + i] = SourceData[SrcStart + i];
            }
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            var d = data;
            d[Address] = Value;
        }

        public void WriteWord(int Address, int Value)
        {
            WriteByte(Address, (byte)(Value & 0xff));
            WriteByte(Address + 1, (byte)(Value >> 8 & 0xff));
        }

        // Duplicate a memory block
        internal void Duplicate(int SourceAddress, int DestAddress, int Length)
        {
            var d = data;
            System.Array.Copy(d, SourceAddress, d, DestAddress, Length);
        }

        public void CopyIntoBuffer(int srcAddress, int srcLength, byte[] buffer)
        {
            var d = data;
            System.Array.Copy(d, srcAddress, buffer, 0, srcLength);
        }

        // Copy data from a buffer to RAM
        public void CopyBuffer(byte[] src, int srcAddress, int destAddress, int length)
        {
            var d = data;
            System.Array.Copy(src, srcAddress, d, destAddress, length);
        }
    }
}
