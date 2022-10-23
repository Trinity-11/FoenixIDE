using FoenixIDE.Basic;
using System;

namespace FoenixIDE.Simulator.Devices
{
    public class KeyboardRegister: MemoryLocations.MemoryRAM
    {
        private bool mouseDevice = false;
        private bool breakKey = false;
        private byte ps2PacketCntr = 0;
        private int packetLength = 0;
        private byte[] ps2packet = new byte[6];
        public delegate void TriggerInterruptDelegate();
        public TriggerInterruptDelegate TriggerKeyboardInterrupt;
        public TriggerInterruptDelegate TriggerMouseInterrupt;

        public KeyboardRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        // This is used to simulate the Keyboard Register
        public override void WriteByte(int Address, byte Value)
        {
            // In order to avoid an infinite loop, we write to the device directly
            data[Address] = Value;
            
            
            switch (Address)
            {
                case 0:
                    switch (Value)
                    {
                        case 0x69:
                            data[4] = 1;
                            break;
                        case 0xEE: // echo command
                            data[4] = 1;
                            break;
                        case 0xF4:
                            data[0] = 0xFA;
                            data[4] = 1;
                            break;
                        case 0xF6:
                            data[4] = 1;
                            break;
                    }
                    break;
                case 4:
                    switch (Value)
                    {
                        case 0x60:

                            break;
                        case 0xA9:
                            data[0] = 0;
                            break;
                        case 0xAA: // self test
                            data[0] = 0x55;
                            data[4] = 1;
                            break;
                        case 0xAB: // keyboard test
                            data[0] = 0;
                            data[4] = 1;
                            break;
                        case 0xAD: // disable keyboard
                            data[0] = 0;
                            data[1] = 0;
                            break;
                        case 0xAE: // re-enabled sending data
                            data[4] = 1;
                            break;
                        case 0xFF:  // reset 
                            data[4] = 0xAA;
                            break;
                        case 0x20:
                            data[4] = 1;
                            break;
                        //case 0x60:
                        //    data[4] = 0;
                        //    break;
                        //case 0xAA:
                        //    data[0] = 0x55;
                        //    data[4] = 1;
                        //    break;
                        //case 0xA8:
                        //    data[4] = 1;
                        //    break;
                        //case 0xA9:
                        //    data[0] = 0;
                        //    data[4] = 1;
                        //    break;
                        //case 0xAB:
                        //    data[0] = 0;
                        //    break;
                        case 0xD4:
                            data[4] = 1;
                            break;

                    }
                    
                    break;
            }
        }

        public override byte ReadByte(int Address)
        {
            // Whenever the buffer is read, set the buffer to empty.
            if (Address == 0)
            {
                if (!mouseDevice && !breakKey)
                {
                    data[4] = 0;
                }
                else if (packetLength != 0)
                {
                    

                    // raise interrupt
                    if (mouseDevice)
                    {
                        // send the next byte in the packet
                        data[4] = ps2packet[ps2PacketCntr++];
                        TriggerMouseInterrupt();
                        if (ps2PacketCntr == packetLength)
                        {
                            ps2PacketCntr = 0;
                            mouseDevice = false;
                            packetLength = 0;
                        }
                    } 
                    else if (breakKey)  // this doesn't work yet
                    {
                        // send the next byte in the packet
                        data[0] = ps2packet[ps2PacketCntr++];
                        data[4] = 0;
                        TriggerKeyboardInterrupt();
                        if (ps2PacketCntr == packetLength)
                        {
                            ps2PacketCntr = 0;
                            breakKey = false;
                            packetLength = 0;
                        }
                    }
                }
                return data[0];
            }
            else if (Address == 5)
            {
                return 0;
            }
            return data[Address];
        }
        public void WriteScanCodeSequence(byte[] codes, int seqLength)
        {
            breakKey = true;
            data[0] = codes[0];
            data[4] = 0;
            ps2PacketCntr = 1;
            packetLength = seqLength;
            Array.Copy(codes, ps2packet, seqLength);

            TriggerKeyboardInterrupt?.Invoke();
        }
        
        public void MousePackets(byte buttons, byte X, byte Y)
        {
            mouseDevice = true;
            data[0] = buttons;
            ps2PacketCntr = 1;
            packetLength = 3;
            ps2packet[0] = buttons;
            ps2packet[1] = X;
            ps2packet[2] = Y;

            TriggerMouseInterrupt?.Invoke();
        }
        
    }
}
