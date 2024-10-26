using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    // This is a temporary implementation of the SNES controller
    // We should assume 16 bytes - although there are gaps.

    public class SNES : MemoryLocations.MemoryRAM
    {
        public SNES(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }
        public override byte ReadByte(int Address)
        {
            if (Address >= 4 && Address <= 0xB)
            {
                return (byte)0xFF;
            }
            else
            {
                return data[Address];
            }
        }
    }
}
