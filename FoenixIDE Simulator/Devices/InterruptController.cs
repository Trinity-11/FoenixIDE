using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class InterruptController: MemoryLocations.MemoryRAM
    {
        public InterruptController(int StartAddress, int Length): base(StartAddress, Length)
        {

        }
        public override void WriteByte(int Address, byte Value)
        {
            // Read the current byte at the address, to detect an edge
            byte old = data[Address];
            // If a bit gets set from 0 to 1, leave it.  If a bit gets set a second time, reset to 0.
            byte combo = (byte)(old & Value);
            if (combo > 0)
            {
                data[Address] = (byte)(data[Address] & (byte)(~combo));
            }
            else
            {
                data[Address] = Value;
            }
        }
    }
}
