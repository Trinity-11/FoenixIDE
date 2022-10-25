using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    /**
     * This class allows the F256 Junior to access 1MB of RAM/FLASH and provision 8k pages for the 6502 CPU
     */
    public class MMU_JR : MemoryLocations.MemoryRAM
    {
        private byte activeLUT = 0;
        byte[] LUTs = { 0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F };

        public MMU_JR(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override byte ReadByte(int Address)
        {
            if (Address >= 8 && Address <= 0xF)
            {
                return LUTs[activeLUT * 8 + Address - 8];
            }
            else
            {
                return base.ReadByte(Address);
            }
        }

        public override void WriteByte(int Address, byte Value)
        {
            if (Address >= 8 && Address <= 0xF)
            {
                LUTs[activeLUT * 8 + Address - 8] = Value;
            }
            else
            {
                base.WriteByte(Address, Value);
            }
        }

        public byte GetPage(byte LUT, byte segment)
        {

            return LUTs[LUT * 8 + segment];
        }

        public void SetPage(byte LUT, byte segment, byte value)
        {
            LUTs[LUT * 8 + segment] = value;
        }
        public void SetActiveLUT(byte LUT)
        {
            activeLUT = LUT;
        }

        public void Reset()
        {
            data[0] = 0;
            data[1] = 0;
             LUTs = new byte[]{ 0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F };
        }
    }
}
