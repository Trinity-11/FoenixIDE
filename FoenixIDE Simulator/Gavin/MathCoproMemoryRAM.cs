using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE
{
    /**
     * This class will automatically call other methods immediately after writes
     */
    public class MathCoproMemoryRAM : MemoryRAM
    {
        private int startAddress;
        private int length;

        public MathCoproMemoryRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
            this.startAddress = StartAddress;
            this.length = Length;
            data = new byte[Length];
        }

        public override byte ReadByte(int Address)
        {
            return data[Address];
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            int block = Address >> 3;
            switch (block)
            {
                case 0:
                    mathCoproUnsignedMultiplier(0);
                    break;
                case 1:
                    mathCoproSignedMultiplier(8);
                    break;
                case 2:
                    mathCoproUnsignedDivider(0x10);
                    break;
                case 3:
                    mathCoproSignedDivider(0x18);
                    break;
                case 4:
                    mathCoproSignedAdder(0x20);
                    break;
                case 5:
                    mathCoproSignedAdder(0x20);
                    break;
            }
        }

        void mathCoproUnsignedMultiplier(int baseAddr)
        {
            uint acc1 = (uint)((data[baseAddr + 1] << 8) + data[baseAddr]);
            uint acc2 = (uint)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            uint result = acc1 * acc2;
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 7] = (byte)(result >> 24 & 0xFF);
        }

        void mathCoproSignedMultiplier(int baseAddr)
        {
            int acc1 = (data[baseAddr + 1] << 8) + data[baseAddr];
            int acc2 = (data[baseAddr + 3] << 8) + data[baseAddr + 2];
            int result = acc1 * acc2;
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 7] = (byte)(result >> 24 & 0xFF);
        }

        void mathCoproUnsignedDivider(int baseAddr)
        {
            uint acc1 = (uint)((data[baseAddr + 1] << 8) + data[baseAddr]);
            uint acc2 = (uint)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            uint result = 0;
            uint remainder = 0;
            if (acc2 != 0)
            {
                result = acc1 / acc2;
                remainder = acc1 % acc2;
            }
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(remainder & 0xFF);
            data[baseAddr + 7] = (byte)(remainder >> 8 & 0xFF);
        }

        void mathCoproSignedDivider(int baseAddr)
        {
            int acc1 = (data[baseAddr + 1] << 8) + data[baseAddr];
            int acc2 = (data[baseAddr + 3] << 8) + data[baseAddr + 2];
            int result = 0;
            int remainder = 0;
            if (acc2 != 0)
            {
                result = acc1 / acc2;
                remainder = acc1 % acc2;
            }
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(remainder & 0xFF);
            data[baseAddr + 7] = (byte)(remainder >> 8 & 0xFF);
        }

        // This function gets called whenever we write to the Math Coprocessor address space
        void mathCoproSignedAdder(int baseAddr)
        {
            int acc1 = (data[baseAddr + 3] << 24) + (data[baseAddr + 2] << 16) + (data[baseAddr + 1] << 8) + data[baseAddr];
            int acc2 = (data[baseAddr + 7] << 24) + (data[baseAddr + 6] << 16) + (data[baseAddr + 5] << 8) + data[baseAddr + 4];
            int result = acc1 + acc2;
            data[baseAddr + 8] = (byte)(result & 0xFF);
            data[baseAddr + 9] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 10] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 11] = (byte)(result >> 24 & 0xFF);
        }
    }
}
