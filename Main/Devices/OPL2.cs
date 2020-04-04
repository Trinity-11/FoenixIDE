using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class OPL2 : MemoryLocations.MemoryRAM
    {
        public OPL2(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }
    }
}
