using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    // Used by F256.
    public class RNGRegister : MemoryLocations.MemoryRAM
    {
        int seed;
        bool enabled;
        byte lastHigh;
        byte lastLow;
        Random random;

        public RNGRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
            System.Diagnostics.Debug.Assert(Length == 3);
            random = new Random(0);
        }

        byte NextRandomByte()
        {
            byte[] buf = new byte[1];
            random.NextBytes(buf);
            return buf[0];
        }

        public override byte ReadByte(int Address)
        {
            if (Address == 0) // rndl
            {
                if (enabled)
                {
                    lastLow = NextRandomByte();
                }
                return lastLow;
            }

            if (Address == 1) // rndh
            {
                if (enabled)
                {
                    lastHigh = NextRandomByte();
                }
                return lastHigh;
            }

            if (Address == 2) // rnd_stat
            {
                return 0xFF;
            }

            byte read = data[Address];
            return read;
        }

        public override void WriteByte(int Address, byte Value)
        {
            if (Address == 0) // seedl
            {
                seed &= 0xFF00;
                seed |= Value;
            }
            else if (Address == 1) // seedh
            {
                seed &= 0x00FF;
                seed |= Value << 8;
            }
            else if (Address == 2) // rnd_ctrl
            {
                bool seedld = (Value & 0x2) != 0;
                if (seedld)
                {
                    random = new Random(seed);
                }

                bool enable = (Value & 0x1) != 0;
                enabled = enable;
            }
            data[Address] = Value;
        }
    }
}
