using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    /**
     * This class will automatically call other methods immediately after writes
     */
    public class MathCoproRegister : MemoryLocations.MemoryRAM
    {
        public MathCoproRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            int block = Address >> 3;
            switch (block)
            {
                case 0:
                    MathCoproUnsignedMultiplier(0);
                    break;
                case 1:
                    MathCoproSignedMultiplier(8);
                    break;
                case 2:
                    MathCoproUnsignedDivider(0x10);
                    break;
                case 3:
                    MathCoproSignedDivider(0x18);
                    break;
                case 4:
                    MathCoproSignedAdder(0x20);
                    break;
                case 5:
                    MathCoproSignedAdder(0x20);
                    break;
            }
        }

        void MathCoproUnsignedMultiplier(int baseAddr)
        {
            ushort acc1 = (ushort)((data[baseAddr + 1] << 8) + data[baseAddr]);
            ushort acc2 = (ushort)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            uint result = (uint)acc1 * acc2;
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 7] = (byte)(result >> 24 & 0xFF);
        }

        void MathCoproSignedMultiplier(int baseAddr)
        {
            short acc1 = (short)((data[baseAddr + 1] << 8) + data[baseAddr]);
            short acc2 = (short)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            int result = acc1 * acc2;
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 7] = (byte)(result >> 24 & 0xFF);
        }

        void MathCoproUnsignedDivider(int baseAddr)
        {
            ushort acc1 = (ushort)((data[baseAddr + 1] << 8) + data[baseAddr]);
            ushort acc2 = (ushort)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            uint result = 0;
            uint remainder = 0;
            if (acc1 != 0)
            {
                result = (uint)acc2 / acc1;
                remainder = (uint)acc2 % acc1;
            }
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(remainder & 0xFF);
            data[baseAddr + 7] = (byte)(remainder >> 8 & 0xFF);
        }

        void MathCoproSignedDivider(int baseAddr)
        {
            short acc1 = (short)((data[baseAddr + 1] << 8) + data[baseAddr]);
            short acc2 = (short)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            int result = 0;
            int remainder = 0;
            if (acc1 != 0)
            {
                result = acc2 / acc1;
                remainder = acc2 % acc1;
            }
            data[baseAddr + 4] = (byte)(result & 0xFF);
            data[baseAddr + 5] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 6] = (byte)(remainder & 0xFF);
            data[baseAddr + 7] = (byte)(remainder >> 8 & 0xFF);
        }

        // This function gets called whenever we write to the Math Coprocessor address space
        void MathCoproSignedAdder(int baseAddr)
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
