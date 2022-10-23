using FoenixIDE.Simulator.FileFormat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class CodecRAM: MemoryLocations.MemoryRAM
    {
        public CodecRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override async void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            data[2] = 1;
            await Task.Delay(200);
            data[2] = 0;
        }
    }
}
