using FoenixIDE.Simulator.Devices;

namespace FoenixIDE.MemoryLocations
{
    /// <summary>
    /// Maps an address on the bus to a device or memory. GPU, RAM, and ROM are hard coded. Other I/O devices will be added 
    /// later.
    /// </summary>
    public class MemoryManagerF256 : MemoryManager
    {
        public FlashF256 FLASHF256 = null;
        public MMU_F256 MMU = null;
        public VIARegisters VIAREGISTERS = null;
        public IEC IECRegister = null;

        /// <summary>
        /// Determine whehter the address being read from or written to is an I/O device or a memory cell.
        /// If the location is an I/O device, return that device. Otherwise, return the memory being referenced.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Device"></param>
        /// <param name="DeviceAddress"></param>
        public override void GetDeviceAt(int Address, out IMappable Device, out int DeviceAddress)
        {
            
            // Special case for the Memory Window again...
            if (Address > 0xFFFF)
            {
                Device = RAM;
                DeviceAddress = Address & 0xF_FFFF;
                return;
            }

            if (Address < 2)
            {
                Device = MMU;
                DeviceAddress = Address;
                return;
            }

            byte MMURegister = MMU.ReadByte(0);
            byte IOPageRegister = MMU.ReadByte(1);
            int offset;
            if (IOPageRegister == 0)
            {
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
                if (Address >= 0xD018 && Address < 0xD01C)
                {
                    Device = SOLRegister;
                    DeviceAddress = Address - 0xD018;
                    return;
                }
                if (Address >= 0xD680 && Address < 0xD682)
                {
                    Device = IECRegister;
                    DeviceAddress = Address - 0xD680;
                    return;
                }
                // These addresses are hard-coded - this is done to store all text and LUT data in vicky
                if (Address >= 0xC000 && Address <= 0xDFFF)
                {
                    Device = VICKY;
                    DeviceAddress = Address - 0xC000;
                    return;
                }
            }
            else
            {
                // For now, vicky stores this data
                if (IOPageRegister < 4 && Address >= 0xC000 && Address <= 0xDFFF)
                {
                    Device = VICKY;
                    offset = IOPageRegister * 8192;  // Page IO 0 is store at the beginning
                    DeviceAddress = offset + Address - 0xC000;
                    return;
                }
            }

            // bits 4 and 5 of MMURegister determine which LUT is being edited
            if ((MMURegister & 0x80) != 0 && Address >= 8 && Address <= 0xF)
            {
                Device = MMU;
                MMU.SetActiveLUT((byte)((MMURegister & 0x30) >> 4));
                DeviceAddress = Address;
                return;
            }

            byte segment = (byte)(Address >> 13);  // take the top 3 bits
            byte page = MMU.GetPage((byte)(MMURegister & 3), segment);

            if (page >= 0x40 && page <= 0x7F) // MMU entry points to FLASH
            {
                Device = FLASHF256;
                offset = (page - 0x40) * 8192;
                DeviceAddress = offset + (Address & 0x1FFF);
                return;
            }

            Device = RAM;
            offset = page * 8192;
            DeviceAddress = offset + (Address & 0x1FFF);
            return;
        }
    }
}
