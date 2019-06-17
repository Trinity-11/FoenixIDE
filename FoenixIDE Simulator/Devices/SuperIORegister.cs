using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class SuperIORegister: MemoryRAM
    {
        public SuperIORegister(int StartAddress, int Length): base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            byte command = data[1];
            switch (command)
            {
                case 0x15:
                    data[0] = 0x51;
                    break;
            }
        }
    }
}
