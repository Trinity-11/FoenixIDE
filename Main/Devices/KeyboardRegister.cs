using FoenixIDE.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class KeyboardRegister: MemoryLocations.MemoryRAM
    {
        private bool mouseDevice = false;
        private byte ps2PacketCntr = 0;
        private byte[] ps2packet = new byte[3];
        private FoenixSystem kernel;

        public KeyboardRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void SetKernel(FoenixSystem kernel)
        {
            this.kernel = kernel;
        }

        // This is used to simulate the Keyboard Register
        public override void WriteByte(int Address, byte Value)
        {
            // In order to avoid an infinite loop, we write to the device directly
            data[Address] = Value;
            switch (Address)
            {
                case 0:
                    byte command = data[0];
                    switch (command)
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
                    byte reg = data[4];
                    switch (reg)
                    {
                        case 0x20:
                            data[4] = 1;
                            break;
                        case 0x60:
                            data[4] = 0;
                            break;
                        case 0xAA:
                            data[0] = 0x55;
                            data[4] = 1;
                            break;
                        case 0xA8:
                            data[4] = 1;
                            break;
                        case 0xA9:
                            data[0] = 0;
                            data[4] = 1;
                            break;
                        case 0xAB:
                            data[0] = 0;
                            break;
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
                if (!mouseDevice)
                {
                    data[4] = 0;
                }
                else if (ps2PacketCntr != 0)
                {
                    // send the next byte in the packet
                    data[4] = ps2packet[ps2PacketCntr++];
                    if (ps2PacketCntr == 3)
                    {
                        ps2PacketCntr = 0;
                    }
                    // raise interrupt
                    TriggerMouseInterrupt();
                }
            }
            return base.ReadByte(Address);
        }
        public void WriteKey(ScanCode key)
        {
            // Check if the Keyboard interrupt is allowed
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG1);
            if ((~mask & (byte)Register1.FNX1_INT00_KBD) == (byte)Register1.FNX1_INT00_KBD)
            {
                kernel.MemMgr.KEYBOARD.WriteByte(0, (byte)key);
                kernel.MemMgr.KEYBOARD.WriteByte(4, 0);
                // Set the Keyboard Interrupt
                byte IRQ1 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG1);
                IRQ1 |= (byte)Register1.FNX1_INT00_KBD;
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.INT_PENDING_REG1, IRQ1);
                kernel.CPU.Pins.IRQ = true;
            }
        }

        
        public void MousePackets(byte buttons, byte X, byte Y)
        {
            mouseDevice = true;
            data[0] = buttons;
            ps2packet[0] = buttons;
            ps2packet[1] = X;
            ps2packet[2] = Y;
            ps2PacketCntr = 1;

            TriggerMouseInterrupt();
        }
        private void TriggerMouseInterrupt()
        {
            // Set the Mouse Interrupt
            byte IRQ0 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
            IRQ0 |= (byte)Register0.FNX0_INT07_MOUSE;
            kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.INT_PENDING_REG0, IRQ0);
            kernel.CPU.Pins.IRQ = true;
        }
    }
}
