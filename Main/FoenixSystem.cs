using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.Processor;
using FoenixIDE.Display;
using System.Threading;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.UI;
using System.IO;
using System.Windows.Forms;

namespace FoenixIDE
{
    public class FoenixSystem
    {
        public MemoryManager MemMgr = null;
        public Processor.CPU CPU = null;

        public ResourceChecker ResCheckerRef;
        public Processor.Breakpoints Breakpoints = new Processor.Breakpoints();
        public ListFile lstFile;
        private BoardVersion boardVersion;
        public SortedList<string, WatchedMemory> WatchList = new SortedList<string, WatchedMemory>();
        private string LoadedKernel;

        public FoenixSystem(BoardVersion version, string DefaultKernel)
        {
            boardVersion = version;

            int memSize = MemoryMap.RAM_SIZE;
            CodecRAM codec = null;
            SDCardDevice sdcard = null;
            byte SystemStat = 0; // FMX
            int keyboardAddress = MemoryMap.KBD_DATA_BUF_FMX; // FMX
            int clock = 14318000;
            bool is6502 = false;

            switch (boardVersion)
            {
                case BoardVersion.RevB:
                    break;
                case BoardVersion.RevC:
                    memSize *= 2;
                    break;
                case BoardVersion.RevU:
                    SystemStat = 1;
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_U;
                    break;
                case BoardVersion.RevUPlus:
                    memSize *= 2;
                    SystemStat = 5;
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_U;
                    break;
                case BoardVersion.RevJr:
                    memSize = 1024*1024;
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_JR;
                    clock = 6293000;
                    is6502 = true;
                    break;
            }
            if (boardVersion == BoardVersion.RevB)
            {
                codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL, 4);
                sdcard = new CH376SRegister(MemoryMap.SDCARD_DATA, MemoryMap.SDCARD_SIZE);
            }
            else if (boardVersion == BoardVersion.RevJr)
            {
                codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL_JR, 3);  // unlike the FMX, this register is 16-bits in F256Jr
                sdcard = new GabeSDController(MemoryMap.SDCARD_JR, MemoryMap.SDCARD_SIZE);   // TODO: write yet a new SD controller.
            }
            else
            {
                codec = new CodecRAM(MemoryMap.CODEC_START_FMX, 4);
                sdcard = new GabeSDController(MemoryMap.GABE_SDC_CTRL_START, MemoryMap.GABE_SDC_CTRL_SIZE);
            }

            if (boardVersion != BoardVersion.RevJr)
            {
                // These are the 65816-based machines
                MemMgr = new MemoryManager
                {
                    RAM = new MemoryRAM(MemoryMap.RAM_START, memSize),                        // RAM: 2MB Rev B & U, 4MB Rev C & U+
                    VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),       // 60K
                    VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE),       // 4MB Video
                    FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE),       // 8MB RAM
                    GABE = new GabeRAM(MemoryMap.GABE_START, MemoryMap.GABE_SIZE),            // 4K 

                    // Special devices
                    MATH = new MathCoproRegister(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1), // 48 bytes
                    KEYBOARD = new KeyboardRegister(keyboardAddress, 5),
                    SDCARD = sdcard,
                    INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4),
                    UART1 = new UART(MemoryMap.UART1_REGISTERS, 8),
                    UART2 = new UART(MemoryMap.UART2_REGISTERS, 8),
                    OPL2 = new OPL2(MemoryMap.OPL2_S_BASE, 256),
                    FLOAT = new MathFloatRegister(MemoryMap.FLOAT_START, MemoryMap.FLOAT_END - MemoryMap.FLOAT_START + 1),
                    MPU401 = new MPU401(MemoryMap.MPU401_REGISTERS, 2),
                    VDMA = new VDMA(MemoryMap.VDMA_START, MemoryMap.VDMA_SIZE),
                    TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG, 8),
                    TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG, 8),
                    TIMER2 = new TimerRegister(MemoryMap.TIMER2_CTRL_REG, 8),
                    RTC = new RTC(MemoryMap.RTC_SEC, 16),
                    CODEC = codec,
                    MMU = null
                };
                MemMgr.VDMA.setVideoRam(MemMgr.VIDEO);
                MemMgr.VDMA.setSystemRam(MemMgr.RAM);
                MemMgr.VDMA.setVickyRam(MemMgr.VICKY);
                MemMgr.GABE.WriteByte(MemoryMap.GABE_SYS_STAT - MemoryMap.GABE_START, SystemStat);
            }
            else
            {
                // This is a 6502-based machine
                MemMgr = new MemoryManager
                {
                    RAM = new MemoryRAM(MemoryMap.RAM_START, memSize),
                    // vicky will store 4 pages of data
                    VICKY = new MemoryRAM(0, 4 * 0x2000),
                    KEYBOARD = new KeyboardRegister(keyboardAddress, 5),
                    MATH = new MathCoproRegister(MemoryMap.MATH_START_JR, MemoryMap.MATH_END_JR - MemoryMap.MATH_START_JR + 1), // 32 bytes
                    SDCARD = sdcard,
                    INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0_JR, 2),
                    UART1 = new UART(MemoryMap.UART_REGISTERS_JR, 8),
                    TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG_JR, 8),
                    TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG_JR, 8),
                    RTC = new RTC(MemoryMap.RTC_SEC_JR, 16),
                    CODEC = codec,
                    MMU = new MMU_JR(0,16)
                };
            }

            // Assign memory variables used by other processes
            CPU = new CPU(MemMgr, clock, is6502);

            // Load the kernel.hex if present
            ResetCPU(DefaultKernel);

            if (boardVersion != BoardVersion.RevJr)
            {
                // Write bytes $9F in the joystick registers to mean that they are not installed.
                MemMgr.WriteWord(0xAFE800, 0x9F9F);
                MemMgr.WriteWord(0xAFE802, 0x9F9F);

                if (MemMgr.TIMER2.TimerInterruptDelegate == null)
                {
                    MemMgr.TIMER2.TimerInterruptDelegate += TimerEvent2;
                }

                // Set the Vicky rev and subrev
                MemMgr.VICKY.WriteWord(0x1C, 0x7654);
                MemMgr.VICKY.WriteWord(0x1E, 0x3456);
                MemMgr.VICKY.WriteByte(MemoryMap.GAMMA_CTRL_REG - MemoryMap.VICKY_BASE_ADDR, 0x11); // Gamma and hi-res are off
                // set the date
                MemMgr.VICKY.WriteByte(MemoryMap.FPGA_DOR - MemoryMap.VICKY_BASE_ADDR, 0x1);
                MemMgr.VICKY.WriteByte(MemoryMap.FPGA_MOR - MemoryMap.VICKY_BASE_ADDR, 0x2);
                MemMgr.VICKY.WriteByte(MemoryMap.FPGA_YOR - MemoryMap.VICKY_BASE_ADDR, 0x21);

                // Set board revision
                MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_C - MemoryMap.GABE_START, (byte)'E');
                MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_4 - MemoryMap.GABE_START, (byte)'M');
                switch (boardVersion)
                {
                    case BoardVersion.RevB:
                        MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_A - MemoryMap.GABE_START, (byte)'B');
                        break;
                    case BoardVersion.RevC:
                        MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_A - MemoryMap.GABE_START, (byte)'C');
                        break;
                    case BoardVersion.RevU:
                        MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_A - MemoryMap.GABE_START, (byte)'U');
                        break;
                    case BoardVersion.RevUPlus:
                        MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_A - MemoryMap.GABE_START, (byte)'+');
                        break;
                }
            }
            else
            {
                MemMgr.WriteByte(MemoryMap.REVOFJR, 0x2);
                MemMgr.VICKY.WriteWord(0xD000 - 0xC000, 1);
                MemMgr.VICKY.WriteWord(0xD002 - 0xC000, 0x1540);

                String micahFontPath = @"Resources\f256jr_font_micah_jan25th.bin";
                if (System.IO.File.Exists(micahFontPath))
                {
                    byte[] fontBuffer = global::System.IO.File.ReadAllBytes(micahFontPath);
                    MemMgr.VICKY.CopyBuffer(fontBuffer, 0, 0x2000, 2048);
                }
            }

            if (MemMgr.TIMER0.TimerInterruptDelegate == null)
            {
                MemMgr.TIMER0.TimerInterruptDelegate += TimerEvent0;
            }
            if (MemMgr.TIMER1.TimerInterruptDelegate == null)
            {
                MemMgr.TIMER1.TimerInterruptDelegate += TimerEvent1;
            }
            if (MemMgr.RTC.AlarmPeriodicInterruptDelegate == null)
            {
                MemMgr.RTC.AlarmPeriodicInterruptDelegate  += RTCAlarmEvents;
            }
        }

        private void TimerEvent0()
        {
            if (boardVersion != BoardVersion.RevJr)
            {
                byte mask =  MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT02_TMR0) == (byte)Register0.FNX0_INT02_TMR0))
                {
                    // Set the Timer0 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                    IRQ0 |= (byte)Register0.FNX0_INT02_TMR0;
                    MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
            else
            {
                byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0_JR - 0xC000);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0_JR.JR0_INT04_TMR0) == (byte)Register0_JR.JR0_INT04_TMR0))
                {
                    // Set the Timer0 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR - 0xC000);
                    IRQ0 |= (byte)Register0_JR.JR0_INT04_TMR0;
                    MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
        }
        private void TimerEvent1()
        {
            if (boardVersion != BoardVersion.RevJr)
            {
                byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT03_TMR1) == (byte)Register0.FNX0_INT03_TMR1))
                {
                    // Set the Timer1 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                    IRQ0 |= (byte)Register0.FNX0_INT03_TMR1;
                    MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
            else
            {
                byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0_JR - 0xC000);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0_JR.JR0_INT05_TMR1) == (byte)Register0_JR.JR0_INT05_TMR1))
                {
                    // Set the Timer0 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR - 0xC000);
                    IRQ0 |= (byte)Register0_JR.JR0_INT05_TMR1;
                    MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
        }

        // This event does not exist in the F256Jr
        private void TimerEvent2()
        {
            byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
            if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT04_TMR2) == (byte)Register0.FNX0_INT04_TMR2))
            {
                // Set the Timer2 Interrupt
                byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                IRQ0 |= (byte)Register0.FNX0_INT04_TMR2;
                MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                CPU.Pins.IRQ = true;
            }
        }

        private void RTCAlarmEvents()
        {
            if (boardVersion != BoardVersion.RevJr)
            {
                byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT05_RTC) == (byte)Register0.FNX0_INT05_RTC))
                {
                    // Set the Timer1 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                    IRQ0 |= (byte)Register0.FNX0_INT05_RTC;
                    MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
            else
            {
                byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR + 1 - 0xC000);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register1_JR.JR1_INT04_RTC) == (byte)Register1_JR.JR1_INT04_RTC))
                {
                    // Set the Timer0 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR + 1 - 0xC000);
                    IRQ0 |= (byte)Register1_JR.JR1_INT04_RTC;
                    MemMgr.INTERRUPT.WriteFromGabe(1, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
        }

        public BoardVersion GetVersion()
        {
            return boardVersion;
        }
        public void SetVersion(BoardVersion rev)
        {
            boardVersion = rev;
        }
        // return true if the CPU was reset and the program was loaded
        public bool ResetCPU(string filename)
        {
            if (CPU != null)
            {
                CPU.DebugPause = true;
                //CPU.Halt();
            }

            if (filename != null)
            {
                LoadedKernel = filename;
            }

            // If the reset vector is not set in Bank 0, but it is set in Bank 18, the copy bank 18 into bank 0.
            int BasePageAddress = 0x18_0000;
            if (boardVersion == BoardVersion.RevC || boardVersion == BoardVersion.RevUPlus)
            {
                BasePageAddress = 0x38_0000;
            }

            FileInfo info = new FileInfo(LoadedKernel);
            if (!info.Exists)
            {
                OpenFileDialog f = new OpenFileDialog
                {
                    Title = "Select a kernel file",
                    Filter = "Hex Files|*.hex|PGX Files|*.pgx|PGZ Files|*.pgz"
                };
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadedKernel = f.FileName;
                    info = new FileInfo(LoadedKernel);
                }
            }
            string extension = info.Extension.ToUpper();
            if (extension.Equals(".HEX"))
            {
                if (!HexFile.Load(MemMgr.RAM, LoadedKernel, BasePageAddress, out _, out _))
                {
                    return false;
                }
                if (boardVersion == BoardVersion.RevJr)
                {
                    //byte[] tempBuffer = new byte[0x1_0000];
                    //MemMgr.RAM.CopyIntoBuffer(0, 0x1_0000, tempBuffer);
                    //MemMgr.RAM.Zero();
                    //MemMgr.RAM.CopyBuffer(tempBuffer, 0, 0xF_0000, 0x1_0000);
                }
            }
            else if (extension.Equals(".PGX"))
            {
                FileInfo f = new FileInfo(LoadedKernel);
                int flen = (int)(f.Length - 8);
                BinaryReader reader = new BinaryReader(f.OpenRead());
                // The first four byte contain PGX,0x1
                byte[] header = reader.ReadBytes(4);
                // The next four bytes contain the start address
                int FnxAddressPtr = reader.ReadInt32();
                // The rest of the file is data
                byte[] DataBuffer = reader.ReadBytes(flen);
                reader.Close();

                // This is pretty messed up... ERESET points to $FF00, which has simple load routine.
                MemMgr.WriteWord(MemoryMap.VECTOR_ERESET, 0xFF00);
                MemMgr.WriteLong(0xFF00, 0x78FB18);  // CLC, XCE, SEI
                MemMgr.WriteByte(0xFF03, 0x5C);      // JML
                MemMgr.WriteLong(0xFF04, FnxAddressPtr);
            }
            else if (extension.Equals(".PGZ"))
            {
                FileInfo f = new FileInfo(LoadedKernel);
                BinaryReader reader = new BinaryReader(f.OpenRead());
                byte header = reader.ReadByte();  // this should be Z for 24-bits and z for 32-bits
                int size = header == 'z' ? 4 : 3;
                int FnxAddressPtr = -1;

                do
                {
                    byte[] bufAddr = reader.ReadBytes(size);
                    byte[] bufLength = reader.ReadBytes(size);
                    int address = bufAddr[0] + bufAddr[1] * 0x100 + bufAddr[2] * 0x10000 + (size == 4 ? bufAddr[3] * 0x1000000 : 0);
                    int blockLength = bufLength[0] + bufLength[1] * 0x100 + bufLength[2] * 0x10000 + (size == 4 ? bufLength[3] * 0x1000000 : 0);
                    if (blockLength == 0)
                    {
                        FnxAddressPtr = address;
                    }
                    else
                    {
                        byte[] DataBuffer = reader.ReadBytes(blockLength);
                        MemMgr.CopyBuffer(DataBuffer, 0, address, blockLength);

                        // TODO - make this backward compatible
                        if (address >= (BasePageAddress + 0xFF00) && (address < (BasePageAddress + 0xFFFF)))
                        {
                            int pageFFLen = blockLength - ((address + blockLength) - (BasePageAddress + 0x1_0000));
                            if (pageFFLen > blockLength)
                            {
                                pageFFLen = blockLength;
                            }
                            MemMgr.CopyBuffer(DataBuffer, 0, address - BasePageAddress, pageFFLen);
                        }
                    }

                } while (reader.BaseStream.Position < f.Length);
                reader.Close();

                // This is pretty messed up... ERESET points to $FF00, which has simple load routine.
                MemMgr.WriteWord(MemoryMap.VECTOR_ERESET, 0xFF00);
                MemMgr.WriteLong(0xFF00, 0x78FB18);  // CLC, XCE, SEI
                MemMgr.WriteByte(0xFF03, 0x5C);      // JML
                MemMgr.WriteLong(0xFF04, FnxAddressPtr);
            }
            else if (extension.Equals(".FNXML"))
            {
                this.ResetMemory();
                FoeniXmlFile fnxml = new FoeniXmlFile(this, ResCheckerRef);
                fnxml.Load(LoadedKernel);
                boardVersion = fnxml.Version;
            }

            // Load the .LST file if it exists
            if (lstFile == null)
            {
                lstFile = new ListFile(LoadedKernel);
            }
            else
            {
                // TODO: This results in lines of code to be shown in incorrect order - Fix
                ListFile tempList = new ListFile(LoadedKernel);
                foreach (DebugLine line in tempList.Lines.Values)
                {
                    if (lstFile.Lines.ContainsKey(line.PC))
                    {
                        lstFile.Lines.Remove(line.PC);
                    }
                    lstFile.Lines.Add(line.PC, line);
                    for (int i = 1; i < line.commandLength; i++)
                    {
                        if (lstFile.Lines.ContainsKey(line.PC + i))
                        {
                            lstFile.Lines.Remove(line.PC + i);
                        }
                    }
                }
            }

            // See if lines of code exist in the 0x18_0000 to 0x18_FFFF block for RevB/RevU or 0x38_0000 to 0x38_FFFF block for RevC/RevU+
            List<DebugLine> copiedLines = new List<DebugLine>();
            if (lstFile.Lines.Count > 0)
            {
                foreach (DebugLine line in lstFile.Lines.Values)
                {
                    if (line != null && line.PC >= BasePageAddress && line.PC < BasePageAddress + 0x1_0000)
                    {
                        DebugLine dl = (DebugLine)line.Clone();
                        dl.PC -= BasePageAddress;
                        copiedLines.Add(dl);
                    }
                }
            }
            if (copiedLines.Count > 0)
            {
                foreach (DebugLine line in copiedLines)
                {
                    if (lstFile.Lines.ContainsKey(line.PC))
                    {
                        lstFile.Lines.Remove(line.PC);
                    }
                    lstFile.Lines.Add(line.PC, line);
                }
            }
            CPU.Reset();

            // Reset the keyboard
            MemMgr.KEYBOARD.WriteByte(0, 0);
            MemMgr.KEYBOARD.WriteByte(4, 0);

            return true;
        }

        public void ResetMemory()
        {
            MemMgr.RAM.Zero();
            MemMgr.VICKY.Zero();
            MemMgr.VDMA.Zero();
        }
    }
}
