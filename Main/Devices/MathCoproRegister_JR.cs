using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    /**
     * This class will automatically call other methods immediately after writes
     */
    public class MathCoproRegister_JR : MemoryLocations.MemoryRAM
    {
        public MathCoproRegister_JR(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            int block = Address >> 2;
            // If the address is below 0x10, then write the value in the register.
            if (block < 4)
            {
                data[Address] = Value;
            }
            switch (block)
            {
                case 0:
                    MathCoproUnsignedMultiplier(0);
                    break;
                case 1:
                    MathCoproUnsignedDivider(0x4);
                    break;
                case 2:
                case 3:
                    MathCoproUnsignedAdder(0x8);
                    break;
            }
        }

        void MathCoproUnsignedMultiplier(int baseAddr)
        {
            ushort acc1 = (ushort)((data[baseAddr + 1] << 8) + data[baseAddr]);
            ushort acc2 = (ushort)((data[baseAddr + 3] << 8) + data[baseAddr + 2]);
            uint result = (uint)acc1 * acc2;
            data[baseAddr + 0x10] = (byte)(result & 0xFF);
            data[baseAddr + 0x11] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 0x12] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 0x13] = (byte)(result >> 24 & 0xFF);
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
            data[baseAddr + 0x10] = (byte)(result & 0xFF);
            data[baseAddr + 0x11] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 0x12] = (byte)(remainder & 0xFF);
            data[baseAddr + 0x13] = (byte)(remainder >> 8 & 0xFF);
        }

        void MathCoproUnsignedAdder(int baseAddr)
        {
            int acc1 = (data[baseAddr + 3] << 24) + (data[baseAddr + 2] << 16) + (data[baseAddr + 1] << 8) + data[baseAddr];
            int acc2 = (data[baseAddr + 7] << 24) + (data[baseAddr + 6] << 16) + (data[baseAddr + 5] << 8) + data[baseAddr + 4];
            int result = acc1 + acc2;
            data[baseAddr + 0x10] = (byte)(result & 0xFF);
            data[baseAddr + 0x11] = (byte)(result >> 8 & 0xFF);
            data[baseAddr + 0x12] = (byte)(result >> 16 & 0xFF);
            data[baseAddr + 0x13] = (byte)(result >> 24 & 0xFF);
        }
    }
}
