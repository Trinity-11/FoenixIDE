using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class MPU401 : MemoryLocations.MemoryRAM
    {
        public MPU401(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            // MPU401 only accepts two commands $3f and $ff, when this is received, acknowledge the command
            if (Address == 1)
            {
                data[0] = 0xFE;
                data[1] = 0x80;
            }
        }
    }
}
