using FoenixIDE.MemoryLocations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class GabeRAM : MemoryLocations.MemoryRAM
    {
        private Random rng = new Random();

        public GabeRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
            
        }
        override public byte ReadByte(int Address)
        {
            if (Address == (MemoryMap.GABE_RNG_SEED_LO - MemoryMap.GABE_START))
            {
                return (byte)rng.Next(255);
            }
            if (Address == (MemoryMap.GABE_RNG_SEED_HI - MemoryMap.GABE_START))
            {
                return (byte)rng.Next(255);
            }
            return data[Address];
        }
    }
}
