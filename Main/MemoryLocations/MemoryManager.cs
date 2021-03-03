using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.Processor;
using FoenixIDE.Display;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.Simulator.Devices;

namespace FoenixIDE.MemoryLocations
{
    /// <summary>
    /// Maps an address on the bus to a device or memory. GPU, RAM, and ROM are hard coded. Other I/O devices will be added 
    /// later.
    /// </summary>
    public class MemoryManager : IMappable
    {
        public const int MinAddress = 0x00_0000;
        public const int MaxAddress = 0xff_ffff;

        //public List<IMappable> devices = new List<IMappable>();
        public MemoryRAM RAM = null;
        public MemoryRAM FLASH = null;
        public MemoryRAM VIDEO = null;
        public MemoryRAM VICKY = null;
        public MemoryRAM GABE = null;
        public MathCoproRegister MATH = null;
        public MathFloatRegister FLOAT = null;
        public CodecRAM CODEC = null;
        public KeyboardRegister KEYBOARD = null;
        public SDCardDevice SDCARD = null;
        public InterruptController INTERRUPT = null;
        public UART UART1 = null;
        public UART UART2 = null;
        public OPL2 OPL2 = null;
        public MPU401 MPU401 = null;
        public VDMA VDMA = null;
        public TimerRegister TIMER0 = null;
        public TimerRegister TIMER1 = null;
        public TimerRegister TIMER2 = null;

        public bool VectorPull = false;

        public int StartAddress
        {
            get
            {
                return 0;
            }
        }

        public int Length
        {
            get
            {
                return 0x100_0000;
            }
        }

        public int EndAddress
        {
            get
            {
                return 0xFF_FFFF;
            }
        }

        /// <summary>
        /// Determine whehter the address being read from or written to is an I/O device or a memory cell.
        /// If the location is an I/O device, return that device. Otherwise, return the memory being referenced.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Device"></param>
        /// <param name="DeviceAddress"></param>
        public void GetDeviceAt(int Address, out IMappable Device, out int DeviceAddress)
        {
            if (Address == MemoryMap.CODEC_WR_CTRL_FMX)
            {
                Device = CODEC;
                DeviceAddress = 0;
                return;
            }
            if (Address >= MemoryMap.MATH_START && Address <= MemoryMap.MATH_END)
            {
                Device = MATH;
                DeviceAddress = Address - MATH.StartAddress;
                return;
            }
            if (Address >= MemoryMap.INT_PENDING_REG0 && Address <= MemoryMap.INT_PENDING_REG0 + 3)
            {
                Device = INTERRUPT;
                DeviceAddress = Address - INTERRUPT.StartAddress;
                return;
            }
            if (Address >= MemoryMap.TIMER0_CTRL_REG && Address <= MemoryMap.TIMER0_CTRL_REG + 7)
            {
                Device = TIMER0;
                DeviceAddress = Address - TIMER0.StartAddress;
                return;
            }
            if (Address >= MemoryMap.TIMER1_CTRL_REG && Address <= MemoryMap.TIMER1_CTRL_REG + 7)
            {
                Device = TIMER1;
                DeviceAddress = Address - TIMER1.StartAddress;
                return;
            }
            if (Address >= MemoryMap.TIMER2_CTRL_REG && Address <= MemoryMap.TIMER2_CTRL_REG + 7)
            {
                Device = TIMER2;
                DeviceAddress = Address - TIMER2.StartAddress;
                return;
            }
            if (Address >= RAM.StartAddress && Address <= RAM.StartAddress + RAM.Length - 1)
            {
                Device = RAM;
                DeviceAddress = Address - RAM.StartAddress;
                return;
            }
            
            if (Address >= KEYBOARD.StartAddress && Address <= KEYBOARD.EndAddress)
            {
                Device = KEYBOARD;
                DeviceAddress = Address - KEYBOARD.StartAddress;
                return;
            }
            if (Address >= MemoryMap.UART1_REGISTERS && Address < MemoryMap.UART1_REGISTERS + 8)
            {
                Device = UART1;
                DeviceAddress = Address - MemoryMap.UART1_REGISTERS;
                return;
            }
            if (Address >= MemoryMap.UART2_REGISTERS && Address < MemoryMap.UART2_REGISTERS + 8)
            {
                Device = UART2;
                DeviceAddress = Address - MemoryMap.UART2_REGISTERS;
                return;
            }
            if (Address >= MemoryMap.MPU401_DATA_REG && Address <= MemoryMap.MPU401_STATUS_REG)
            {
                Device = MPU401;
                DeviceAddress = Address - MPU401.StartAddress;
                return;
            }
            if (Address >= SDCARD.StartAddress && Address <= SDCARD.EndAddress)
            {
                Device = SDCARD;
                DeviceAddress = Address - SDCARD.StartAddress;
                return;
            }
            if (Address >= MemoryMap.FLOAT_START && Address <= MemoryMap.FLOAT_END)
            {
                Device = FLOAT;
                DeviceAddress = Address - FLOAT.StartAddress;
                return;
            }
            if (Address >= MemoryMap.VDMA_START && Address < MemoryMap.VDMA_START + MemoryMap.VDMA_SIZE)
            {
                Device = VDMA;
                DeviceAddress = Address - MemoryMap.VDMA_START;
                return;
            }
            if (Address >= MemoryMap.VICKY_START && Address <= MemoryMap.VICKY_END)
            {
                Device = VICKY;
                DeviceAddress = Address - VICKY.StartAddress;
                return;
            }
            if (Address >= MemoryMap.OPL2_S_BASE && Address <= MemoryMap.OPL2_S_BASE + 255)
            {
                Device = OPL2;
                DeviceAddress = Address - OPL2.StartAddress;
                return;
            }
            if (Address >= MemoryMap.GABE_START && Address <= MemoryMap.GABE_END)
            {
                Device = GABE;
                DeviceAddress = Address - GABE.StartAddress;
                return;
            }
            if (Address >= MemoryMap.VIDEO_START && Address < (MemoryMap.VIDEO_START + MemoryMap.VIDEO_SIZE))
            {
                Device = VIDEO;
                DeviceAddress = Address - VIDEO.StartAddress;
                return;
            }
            if (Address >= MemoryMap.FLASH_START && Address <= MemoryMap.FLASH_END)
            {
                Device = FLASH;
                DeviceAddress = Address - FLASH.StartAddress;
                return;
            }

            // oops, we didn't map this to anything. 
            Device = null;
            DeviceAddress = 0;
        }

        public virtual byte this[int Address]
        {
            get { return ReadByte(Address); }
            set { WriteByte(Address, value); ; }
        }

        public virtual byte this[int Bank, int Address]
        {
            get { return ReadByte(Bank * 0xffff + Address & 0xffff); }
            set { WriteByte(Bank * 0xffff + Address & 0xffff, value); }
        }

        /// <summary>
        /// Finds device mapped to 'Address' and calls it 
        /// 'Address' is offset by GetDeviceAt to device internal address range
        /// </summary>
        public virtual byte ReadByte(int Address)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            if (device == null)
                return 0xff;
            return device.ReadByte(deviceAddress);
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int ReadWord(int Address)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            // TODO - if device is null, generate a software interrupt.
            return device.ReadByte(deviceAddress) | (device.ReadByte(deviceAddress + 1) << 8);
        }

        /// <summary>
        /// Reads 3 bytes from memory and builds a 24-bit unsigned integer.
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public int ReadLong(int Address)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            return device.ReadByte(deviceAddress)
                | (device.ReadByte(deviceAddress + 1) << 8)
                | (device.ReadByte(deviceAddress + 2) << 16);
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            device.WriteByte(deviceAddress, Value);
        }

        public void WriteWord(int Address, int Value)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            device.WriteByte(deviceAddress, (byte)(Value & 0xff));
            device.WriteByte(deviceAddress + 1, (byte)(Value >> 8 & 0xff));
        }

        public void WriteLong(int Address, int Value)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            device.WriteByte(deviceAddress, (byte)(Value & 0xff));
            device.WriteByte(deviceAddress + 1, (byte)(Value >> 8 & 0xff));
            device.WriteByte(deviceAddress + 2, (byte)(Value >> 16 & 0xff));
        }

        public int Read(int Address, int Length)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            int addr = deviceAddress;
            int ret = device.ReadByte(addr);
            if (Length >= 2)
                ret += device.ReadByte(addr + 1) << 8;
            if (Length >= 3)
                ret += device.ReadByte(addr + 2) << 16;
            return ret;
        }

        internal void Write(int Address, int Value, int Length)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);
            if (device == null)
                throw new Exception("No device at " + Address.ToString("X6"));
            device.WriteByte(deviceAddress, (byte)(Value & 0xff));
            if (Length >= 2)
                device.WriteByte(deviceAddress + 1, (byte)(Value >> 8 & 0xff));
            if (Length >= 3)
                device.WriteByte(deviceAddress + 2, (byte)(Value >> 16 & 0xff));
        }
    }
}
