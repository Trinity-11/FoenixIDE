using FoenixIDE.Simulator.Devices;

namespace FoenixIDE.MemoryLocations
{
    /// <summary>
    /// Maps an address on the bus to a device or memory. GPU, RAM, and ROM are hard coded. Other I/O devices will be added 
    /// later.
    /// </summary>
    public class MemoryManagerF256Flat : MemoryManagerF256
    {
        /// <summary>
        /// Determine whehter the address being read from or written to is an I/O device or a memory cell.
        /// If the location is an I/O device, return that device. Otherwise, return the memory being referenced.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Device"></param>
        /// <param name="DeviceAddress"></param>
        public override void GetDeviceAt(int Address, out IMappable Device, out int DeviceAddress)
        {
            // this is F256 Flat mode
            // Special case for the Memory Window again...
            //                    if (Address > 0xFFFF)
            //                    {
            //                       Device = RAM;
            //                        DeviceAddress = Address & 0xF_FFFF;
            //                        return;
            //                    }

            // Map Flash into upper 256 bytes of 1st 64K
            if (Address >= 0x00_FF00 && Address <= 0x00_FFFF)
            {
                Address |= 0xFF_0000;
            }

            if (Address >= CODEC.StartAddress && Address <= CODEC.EndAddress)
            {
                Device = CODEC;
                DeviceAddress = Address - CODEC.StartAddress;
                return;
            }
            if (Address >= DMA.StartAddress && Address <= DMA.EndAddress)
            {
                Device = DMA;
                DeviceAddress = Address - DMA.StartAddress;
                return;
            }
            if (Address >= MATH.StartAddress && Address <= MATH.EndAddress)
            {
                Device = MATH;
                DeviceAddress = Address - MATH.StartAddress;
                return;
            }
            if (Address >= INTERRUPT.StartAddress && Address <= INTERRUPT.EndAddress)
            {
                Device = INTERRUPT;
                DeviceAddress = Address - INTERRUPT.StartAddress;
                return;
            }
            if (Address >= TIMER0.StartAddress && Address <= TIMER0.EndAddress)
            {
                Device = TIMER0;
                DeviceAddress = Address - TIMER0.StartAddress;
                return;
            }
            if (Address >= TIMER1.StartAddress && Address <= TIMER1.EndAddress)
            {
                Device = TIMER1;
                DeviceAddress = Address - TIMER1.StartAddress;
                return;
            }
            if (Address >= RTC.StartAddress && Address <= RTC.EndAddress)
            {
                Device = RTC;
                DeviceAddress = Address - RTC.StartAddress;
                return;
            }
            if (Address >= PS2KEYBOARD.StartAddress && Address <= PS2KEYBOARD.EndAddress)
            {
                Device = PS2KEYBOARD;
                DeviceAddress = Address - PS2KEYBOARD.StartAddress;
                return;
            }
            if (VIAREGISTERS != null)
            {
                if (Address >= VIAREGISTERS.VIA0.StartAddress && Address <= VIAREGISTERS.VIA0.EndAddress)
                {
                    Device = VIAREGISTERS.VIA0;
                    DeviceAddress = Address - VIAREGISTERS.VIA0.StartAddress;
                    return;
                }
                if (VIAREGISTERS.VIA1 != null && Address >= VIAREGISTERS.VIA1.StartAddress && Address <= VIAREGISTERS.VIA1.EndAddress)
                {
                    Device = VIAREGISTERS.VIA1;
                    DeviceAddress = Address - VIAREGISTERS.VIA1.StartAddress;
                    return;
                }
            }
            if (SNESController != null && Address >= SNESController.StartAddress && Address <= SNESController.EndAddress)
            {
                Device = SNESController;
                DeviceAddress = Address - SNESController.StartAddress;
                return;
            }
            if (RNG != null)
            {
                if (Address >= RNG.StartAddress && Address <= RNG.EndAddress)
                {
                    Device = RNG;
                    DeviceAddress = Address - RNG.StartAddress;
                    return;
                }
            }
            if (Address >= UART1.StartAddress && Address <= UART1.EndAddress)
            {
                Device = UART1;
                DeviceAddress = Address - UART1.StartAddress;
                return;
            }
            if (Address >= SDCARD.StartAddress && Address <= SDCARD.EndAddress)
            {
                Device = SDCARD;
                DeviceAddress = Address - SDCARD.StartAddress;
                return;
            }
            if (Address >= SOLRegister.StartAddress && Address < SOLRegister.EndAddress)
            {
                Device = SOLRegister;
                DeviceAddress = Address - SOLRegister.StartAddress;
                return;
            }

            if (Address >= VICKY.StartAddress && Address <= VICKY.EndAddress)
            {
                Device = VICKY;
                DeviceAddress = Address - VICKY.StartAddress;
                return;
            }

            if (Address >= FLASHF256.StartAddress && Address <= FLASHF256.EndAddress)
            {
                Device = FLASHF256;
                DeviceAddress = Address - FLASHF256.StartAddress;
                return;
            }

            if (Address >= RAM.StartAddress && Address <= RAM.StartAddress + RAM.Length - 1)
            {
                Device = RAM;
                DeviceAddress = Address - RAM.StartAddress;
                return;
            }

            // oops, we didn't map this to anything. 
            Device = null;
            DeviceAddress = 0;
        }
    }
}
