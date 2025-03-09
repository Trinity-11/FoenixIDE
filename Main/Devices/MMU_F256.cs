using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    /**
     * This class allows the F256 Junior to access 1MB of RAM/FLASH and provision 8k pages for the 6502 CPU
     */
    public class MMU_F256 : MemoryLocations.MemoryRAM
    {
        private byte editLUT = 0;
        byte[] LUTs = { 0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F,
                        0, 1, 2, 3, 4, 5, 6, 0x7F };

        public bool flatMode;

        //
        // TODO: refactor this class - the "FlatMode" should not be exist and the F256Ke doesn't have an MMU.
        //
        // added a mode to the MMU - sorry about that!
        // so, that the handling of addresses for the F256K2e can be similar to the F256K/c
        // otherwise, it seems difficult to identify which mode we're in
        public MMU_F256(int StartAddress, int Length, bool FlatMode) : base(StartAddress, Length)
        {
            flatMode = FlatMode;
        }

        public override byte ReadByte(int Address)
        {
            if (Address >= 8 && Address <= 0xF)
            {
                return LUTs[editLUT * 8 + Address - 8];
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
                LUTs[editLUT * 8 + Address - 8] = Value;
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

        public void SetActiveLUT(byte LUT)
        {
            editLUT = LUT;
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
