using System;

namespace FoenixIDE.Simulator.Devices
{
    // This is a temporary implementation of the SNES controller
    // We should assume 16 bytes - although there are gaps.

    public class IEC : MemoryLocations.MemoryRAM
    {
        private byte SRQ_o = 0;
        private byte RST_o = 0;
        private byte NMI_EN = 0;
        private byte ATN_o = 0;
        private byte CLK_o = 0;
        private byte DAT_o = 0;
        private byte iec_out = 0;
        private byte iec_in = 0;
        public IEC(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }
        public override byte ReadByte(int Address)
        {
            Console.Write("Reading IEC reg {0:X}", Address);
            switch (Address)
            {
                case 0:
                    Console.WriteLine(", data {0:X}", iec_in);
                    return iec_in;
                case 1:
                    Console.WriteLine(", data {0:X}", iec_out);
                    return iec_out;
                default:
                    return 0;
            }
        }
        public override void WriteByte(int Address, byte Value)
        {
            // Address 0 is not writable
            Console.WriteLine("Writing to IEC reg {0:X} {1:X}", Address, Value);
            if (Address == 1)
            {
                iec_out = Value;
                SRQ_o = (byte)((Value & 0x80) >> 7);
                RST_o = (byte)((Value & 0x40) >> 6);
                NMI_EN = (byte)((Value & 0x20) >> 5);
                ATN_o = (byte)((Value & 0x10) >> 4);
                CLK_o = (byte)((Value & 0x2) >> 1);
                DAT_o = (byte)(Value & 0x1);
                iec_in &= 0xFE;
                iec_in |= DAT_o;
            }
        }
    }
}
