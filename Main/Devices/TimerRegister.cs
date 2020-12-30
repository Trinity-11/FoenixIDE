using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class TimerRegister : MemoryLocations.MemoryRAM
    {
        public TimerRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            // Address 0 is control register
            if (Address == 0)
            {
                
            }
        }
    }
}
