using FoenixIDE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    /// <summary>
    ///  base class for I/O device. 
    /// </summary>
    public class IODevice : IMappable
    {
        private int startAddress;
        private int length;
        private int endAddress;

        private byte[] OutputData = null;
        private byte[] InputData = null;

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

        public IODevice(int StartAddress, int Length)
        {
            this.startAddress = StartAddress;
            this.length = Length;
            this.endAddress = StartAddress + Length - 1;
            this.OutputData = new byte[length];
            this.InputData = new byte[length];
        }

        public byte ReadByte(int Address)
        {
            return InputData[Address];
        }

        public void WriteByte(int Address, byte Data)
        {
            OutputData[Address] = Data;
        }
    }
}
