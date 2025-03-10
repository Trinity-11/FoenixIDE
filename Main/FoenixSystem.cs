using System;
using System.Collections.Generic;
using FoenixIDE.Processor;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.Simulator.UI;
using System.IO;
using System.Windows.Forms;

namespace FoenixIDE
{
    public class FoenixSystem
    {
        public MemoryManager MemMgr = null;
        public Processor.CPU CPU = null;

        public ResourceChecker ResCheckerRef;
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
                case BoardVersion.RevJr_6502:
                case BoardVersion.RevF256K_6502:
                    memSize = 1024 * 1024; // Includes both RAM and flash.
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_F256_MMU;
                    clock = 6293000;
                    is6502 = true;
                    break;
                case BoardVersion.RevJr_65816:
                case BoardVersion.RevF256K_65816:
                    memSize = 1024 * 1024;
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_F256_MMU;
                    clock = 6293000;
                    is6502 = false;
                    break;
                case BoardVersion.RevF256K2e:
                    // needs more memory - 2M SRAM, 512K Flash, 128M DDR3 though DDR3 MMU
                    memSize = 0x00200000;
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_F256_FLAT;
                    clock = 6293000;
                    is6502 = false;
                    break;
            }
            if (boardVersion == BoardVersion.RevB)
            {
                codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL, 4);
                sdcard = new CH376SRegister(MemoryMap.SDCARD_DATA, MemoryMap.SDCARD_SIZE);
            }
            else if (BoardVersionHelpers.IsF256(boardVersion))
            {
                if (BoardVersionHelpers.IsF256_MMU(boardVersion))
                {
                    codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL_F256_MMU, 3);  // unlike the FMX, this register is 16-bits in F256Jr
                    sdcard = new F256SDController(MemoryMap.SDCARD_F256_MMU, 2);
                }
                else
                {
                    codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL_F256_FLAT, 3);
                    sdcard = new F256SDController(MemoryMap.SDCARD_F256_FLAT, 2);
                }
            }
            else
            {
                codec = new CodecRAM(MemoryMap.CODEC_START_FMX, 4);
                sdcard = new GabeSDController(MemoryMap.GABE_SDC_CTRL_START, MemoryMap.GABE_SDC_CTRL_SIZE);
            }

            if (!BoardVersionHelpers.IsF256(boardVersion))
            {
                // Create the C256 VDMA
                VDMA vdma = new VDMA(MemoryMap.VDMA_START, MemoryMap.VDMA_SIZE);
                // These are the strictly 65816-based machines
                MemMgr = new MemoryManager
                {
                    RAM = new MemoryRAM(MemoryMap.RAM_START, memSize),                        // RAM: 2MB Rev B & U, 4MB Rev C & U+
                    VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),       // 60K
                    VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE),       // 4MB Video
                    FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE),       // 8MB RAM
                    GABE = new GabeRAM(MemoryMap.GABE_START, MemoryMap.GABE_SIZE),            // 4K 

                    // Special devices
                    MATH = new MathCoproRegister(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1), // 48 bytes
                    PS2KEYBOARD = new PS2KeyboardRegisterSet1(keyboardAddress, 5),
                    SDCARD = sdcard,
                    INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4),
                    UART1 = new UART(MemoryMap.UART1_REGISTERS, 8),
                    UART2 = new UART(MemoryMap.UART2_REGISTERS, 8),
                    OPL2 = new OPL2(MemoryMap.OPL2_S_BASE, 256),
                    FLOAT = new MathFloatRegister(MemoryMap.FLOAT_START, MemoryMap.FLOAT_END - MemoryMap.FLOAT_START + 1),
                    MPU401 = new MPU401(MemoryMap.MPU401_REGISTERS, 2),
                    DMA = vdma,
                    TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG, 8),
                    TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG, 8),
                    TIMER2 = new TimerRegister(MemoryMap.TIMER2_CTRL_REG, 8),
                    RTC = new RTC(MemoryMap.RTC_SEC, 16),
                    CODEC = codec
                };
                vdma.setVideoRam(MemMgr.VIDEO);
                vdma.setSystemRam(MemMgr.RAM);
                vdma.setVickyRam(MemMgr.VICKY);
                MemMgr.GABE.WriteByte(MemoryMap.GABE_SYS_STAT - MemoryMap.GABE_START, SystemStat);
            }
            else
            {
                // see if this a Flat (65c816) Memory space or with MMU
                if (BoardVersionHelpers.IsF256_Flat(boardVersion))
                {
                    // Create the F256 DMA
                    DMA_F256 dma = new DMA_F256(MemoryMap.DMA_START_F256_FLAT, 20);
                    // This is a 65816-based F256 machine with flat memory map
                    MemMgr = new MemoryManagerF256Flat
                    {
                        RAM = new MemoryRAM(MemoryMap.RAM_START, memSize),
                        FLASHF256 = new FlashF256(MemoryMap.FLASH_START_F256_FLAT, 0x08_0000),
                        // vicky will store 4 pages of data
                        VICKY = new MemoryRAM(MemoryMap.VICKY_START_F256_FLAT, 4 * 0x2000),
                        PS2KEYBOARD = new PS2KeyboardRegisterSet2(keyboardAddress, 5),
                        MATH = new MathCoproRegister_JR(MemoryMap.MATH_START_F256_FLAT, MemoryMap.MATH_END_F256_FLAT - MemoryMap.MATH_START_F256_FLAT + 1), // 32 bytes
                        SDCARD = sdcard,
                        // Set to 4 bytes, just to be compatible with old boards and avoid exceptions in BreakOnIRQCheckBox_CheckedChanged
                        INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0_F256_FLAT, 4),
                        UART1 = new UART(MemoryMap.UART_REGISTERS_F256_FLAT, 8),
                        DMA = dma,
                        TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG_F256_FLAT, 8),
                        TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG_F256_FLAT, 8),
                        RTC = new RTC(MemoryMap.RTC_SEC_F256_FLAT, 16),
                        CODEC = codec,
                        RNG = new RNGRegister(MemoryMap.SEEDL_F256_FLAT, 3),
                        SOLRegister = new SOL(MemoryMap.SOL_CTRL_F256_FLAT, 4),
                        // Only the K machines have a matrix keyboard
                        VIAREGISTERS = new VIARegisters(MemoryMap.JOYSTICK_VIA0_PORT_B_F256_FLAT, 4, MemoryMap.MATRIX_KEYBOARD_VIA1_PORT_B_F256_FLAT, 4)
                };
                    dma.setSystemRam(MemMgr.RAM);
                    // Add the SNES Registers
                    MemMgr.SNESController = new SNES(MemoryMap.SNES_CTRL_REG_FLAT, 16); // We are not implementing the SNES controller, so all values will return $FF
                }
                else
                {
                    // Create the F256 DMA
                    DMA_F256 dma = new DMA_F256(MemoryMap.DMA_START_F256_MMU, 20);
                    // This is a 6502 or 65816-based F256 machine; both have the same memory map
                    MemMgr = new MemoryManagerF256
                    {
                        RAM = new MemoryRAM(MemoryMap.RAM_START, memSize),
                        FLASHF256 = new FlashF256(MemoryMap.RAM_START, 0x08_0000),
                        // vicky will store 4 pages of data
                        VICKY = new MemoryRAM(0, 4 * 0x2000),
                        PS2KEYBOARD = new PS2KeyboardRegisterSet2(keyboardAddress, 5),
                        MATH = new MathCoproRegister_JR(MemoryMap.MATH_START_F256_MMU, MemoryMap.MATH_END_F256_MMU - MemoryMap.MATH_START_F256_MMU + 1), // 32 bytes
                        SDCARD = sdcard,
                        // Set to 4 bytes, just to be compatible with old boards and avoid exceptions in BreakOnIRQCheckBox_CheckedChanged
                        INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0_F256_MMU, 4),
                        UART1 = new UART(MemoryMap.UART_REGISTERS_F256_MMU, 8),
                        DMA = dma,
                        TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG_F256_MMU, 8),
                        TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG_F256_MMU, 8),
                        RTC = new RTC(MemoryMap.RTC_SEC_F256_MMU, 16),
                        CODEC = codec,
                        MMU = new MMU_F256(0, 16, false),
                        RNG = new RNGRegister(MemoryMap.SEEDL_F256_MMU, 3),
                        SOLRegister = new SOL(MemoryMap.SOL_CTRL_F256_MMU, 4),
                        VIAREGISTERS = new VIARegisters(MemoryMap.JOYSTICK_VIA0_PORT_B, 4, MemoryMap.MATRIX_KEYBOARD_VIA1_PORT_B, 4),
                        IECRegister = new IEC(MemoryMap.IEC_START, 2)
                    };
                    dma.setSystemRam(MemMgr.RAM);

                    // The F256jr only has one VIA chip
                    if (boardVersion == BoardVersion.RevJr_6502 || boardVersion == BoardVersion.RevJr_65816)
                    {
                        ((MemoryManagerF256)MemMgr).VIAREGISTERS = new VIARegisters(MemoryMap.JOYSTICK_VIA0_PORT_B, 4);
                    }
                    // Add the SNES Registers
                    MemMgr.SNESController = new SNES(MemoryMap.SNES_CTRL_REG, 16); // We are not implementing the SNES controller, so all values will return $FF
                }

            }

            // Assign memory variables used by other processes
            CPU = new CPU(MemMgr, clock, is6502);

            // Load the kernel.hex if present
            ResetCPU(DefaultKernel);

            if (!BoardVersionHelpers.IsF256(boardVersion))
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
                // Set the Machine ID
                if (boardVersion == BoardVersion.RevJr_6502 || boardVersion == BoardVersion.RevJr_65816)
                {
                    MemMgr.WriteByte(MemoryMap.REVOF_F256_MMU, 0x2);
                }
                else if (boardVersion == BoardVersion.RevF256K_6502 || boardVersion == BoardVersion.RevF256K_65816)
                {
                    MemMgr.WriteByte(MemoryMap.REVOF_F256_MMU, 0x12);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(boardVersion == BoardVersion.RevF256K2e);
                    MemMgr.WriteByte(MemoryMap.REVOF_F256_FLAT, 0x13); // ID num?
                }
                // Set the MCR to be text mode
                MemMgr.VICKY.WriteWord(0xD000 - 0xC000, 1);
                // Set the layers??
                MemMgr.VICKY.WriteWord(0xD002 - 0xC000, 0x1540);

                // Write the byte $DF in the joystick registers to mean they are not present
                MemMgr.VICKY.WriteWord(0xDC00 - 0xC000, 0xDFDF);

                // Set the PCB Hardware Version
                MemMgr.VICKY.WriteWord(0xD6A8 - 0xC000, 0x3041);  // C256Jr
                // Set the CHIP Sub-version, Version, Number
                MemMgr.VICKY.WriteWord(0xD6AA - 0xC000, 0x0101);
                MemMgr.VICKY.WriteWord(0xD6AC - 0xC000, 0x1400);
                MemMgr.VICKY.WriteWord(0xD6AE - 0xC000, 0x0);

                string applicationDirectory = System.AppContext.BaseDirectory;
                String micahFontPath = Path.Combine(applicationDirectory, "Resources", "f256jr_font_micah_jan25th.bin");
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
                MemMgr.RTC.AlarmPeriodicInterruptDelegate += RTCAlarmEvents;
            }
        }

        private void TimerEvent0()
        {
            if (!BoardVersionHelpers.IsF256(boardVersion))
            {
                byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
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
                int addr = BoardVersionHelpers.IsF256_MMU(boardVersion) ? MemoryLocations.MemoryMap.INT_MASK_REG0_F256_MMU : MemoryLocations.MemoryMap.INT_MASK_REG0_F256_FLAT;
                byte mask = MemMgr.ReadByte(addr);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0_JR.JR0_INT04_TMR0) == (byte)Register0_JR.JR0_INT04_TMR0))
                {
                    // Set the Timer0 Interrupt
                    addr = BoardVersionHelpers.IsF256_MMU(boardVersion) ? MemoryLocations.MemoryMap.INT_PENDING_REG0_F256_MMU : MemoryLocations.MemoryMap.INT_PENDING_REG0_F256_FLAT;
                    byte IRQ0 = MemMgr.ReadByte(addr);
                    IRQ0 |= (byte)Register0_JR.JR0_INT04_TMR0;
                    MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                    CPU.Pins.IRQ = true;
                }
            }
        }
        private void TimerEvent1()
        {
            if (!BoardVersionHelpers.IsF256(boardVersion))
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
                int addr = BoardVersionHelpers.IsF256_MMU(boardVersion) ? MemoryLocations.MemoryMap.INT_MASK_REG0_F256_MMU : MemoryLocations.MemoryMap.INT_MASK_REG0_F256_FLAT;
                byte mask = MemMgr.ReadByte(addr);
                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register0_JR.JR0_INT05_TMR1) == (byte)Register0_JR.JR0_INT05_TMR1))
                {
                    addr = BoardVersionHelpers.IsF256_MMU(boardVersion) ? MemoryLocations.MemoryMap.INT_PENDING_REG0_F256_MMU : MemoryLocations.MemoryMap.INT_PENDING_REG0_F256_FLAT;
                    byte IRQ0 = MemMgr.ReadByte(addr);
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
            if (!BoardVersionHelpers.IsF256(boardVersion))
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
                int addr = BoardVersionHelpers.IsF256_MMU(boardVersion) ? MemoryLocations.MemoryMap.INT_PENDING_REG0_F256_MMU : MemoryLocations.MemoryMap.INT_PENDING_REG0_F256_FLAT;
                byte mask = MemMgr.ReadByte(addr + 1);

                if (!CPU.DebugPause && !CPU.Flags.IrqDisable && ((~mask & (byte)Register1_JR.JR1_INT04_RTC) == (byte)Register1_JR.JR1_INT04_RTC))
                {
                    // Set the Timer0 Interrupt
                    byte IRQ0 = MemMgr.ReadByte(addr);
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
        public string GetKernelName()
        {
            return LoadedKernel;
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
            // Implement the bulk.CSV feature for the F256 computers
            if (!info.Exists && BoardVersionHelpers.IsF256(boardVersion))
            {
                // check if the directory exists.  If it does, look for a bulk.csv file
                string bulkCSV = Path.Combine(System.AppContext.BaseDirectory, "roms", "F256", "bulk.csv");
                info = new FileInfo(bulkCSV);

                if (info.Exists)
                {
                    LoadedKernel = bulkCSV;
                    // validate the csv file
                    string[] entries = System.IO.File.ReadAllLines(bulkCSV);
                    foreach (string entry in entries)
                    {
                        // Each entry is a block number, and a file name
                        string[] split = entry.Split(',');
                        if (split.Length > 1)
                        {
                            string blockFile = Path.Combine(System.AppContext.BaseDirectory, "roms", "F256", split[1]);
                            // check the file exists
                            FileInfo testFile = new FileInfo(blockFile);
                            if (!testFile.Exists)
                            {
                                Console.WriteLine("File {0} was not found", split[1]);
                                MessageBox.Show("The file " + split[1] + " is specified in bulk.csv, but not present in the file system.", "Bulk.CSV File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                info = testFile;
                                break;
                            }
                        }
                    }
                }
            }
            while (!info.Exists)
            {
                OpenFileDialog f = new OpenFileDialog
                {
                    Title = "Select a kernel file",
                    Filter = "Hex Files|*.hex|PGX Files|*.pgx|PGZ Files|*.pgz|Binary Files|*.bin"
                };
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadedKernel = f.FileName;
                    info = new FileInfo(LoadedKernel);
                }
                else
                {
                    // Exit the loop and exit the function
                    return false;
                }
            }
            string extension = info.Extension.ToUpper();
            if (info.Name.StartsWith("kernel"))
            {
                if (BoardVersionHelpers.IsF256_MMU(boardVersion))
                {
                    ((MemoryManagerF256)MemMgr).MMU.Reset();
                }
            }
            else
            {
                // Ensure the first LUTs are set correctly - but don't overwrite the kernel.
                if (BoardVersionHelpers.IsF256_MMU(boardVersion))
                {
                    ((MemoryManagerF256)MemMgr).MMU.SetActiveLUT(0);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0x8, 0);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0x9, 1);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xA, 2);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xB, 3);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xC, 4);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xD, 5);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xE, 6);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xF, 7);
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0, 0);
                }
            }
            if (extension.Equals(".HEX"))
            {
                if (HexFile.Load(MemMgr.RAM, BoardVersionHelpers.IsF256(boardVersion) ? ((MemoryManagerF256)MemMgr).FLASHF256 : null, LoadedKernel, BasePageAddress, out _, out _) == -1)
                {
                    return false;
                }
            }
            else if (extension.Equals(".PGX"))
            {
                int flen = (int)(info.Length - 8);
                BinaryReader reader = new BinaryReader(info.OpenRead());
                // The first four byte contain PGX,0x1
                byte[] header = reader.ReadBytes(4);
                if (header[0] == 'P' && header[1] == 'G' && header[2] == 'X')
                {
                    // The next four bytes contain the start address
                    int FnxAddressPtr = reader.ReadInt32();
                    // The rest of the file is data
                    byte[] DataBuffer = reader.ReadBytes(flen);
                    MemMgr.CopyBuffer(DataBuffer, 0, FnxAddressPtr, flen);
                    reader.Close();

                    if (!BoardVersionHelpers.IsF256(boardVersion))
                    {
                        // This is pretty messed up... ERESET points to $FF00, which has simple load routine.
                        MemMgr.WriteWord(MemoryMap.VECTOR_ERESET, 0xFF00);
                        MemMgr.WriteLong(0xFF00, 0x78FB18);  // CLC, XCE, SEI
                        MemMgr.WriteByte(0xFF03, 0x5C);      // JML
                        MemMgr.WriteLong(0xFF04, FnxAddressPtr);
                    }
                    else
                    {
                        MemMgr.WriteWord(MemoryMap.VECTOR_ERESET, FnxAddressPtr);
                    }
                }
            }
            else if (extension.Equals(".PGZ"))
            {
                BinaryReader reader = new BinaryReader(info.OpenRead());
                byte header = reader.ReadByte();  // this should be Z for 24-bits and z for 32-bits
                if (header == 'Z' || header == 'z')
                {
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

                            // This code block is only for FMX/U/U+ - the RESET vector for F256 is addressed later
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

                    } while (reader.BaseStream.Position < info.Length);
                    reader.Close();

                    if (!BoardVersionHelpers.IsF256(boardVersion))
                    {
                        // This is pretty messed up... ERESET points to $FF00, which has simple load routine.
                        MemMgr.WriteWord(MemoryMap.VECTOR_ERESET, 0xFF00);
                        MemMgr.WriteLong(0xFF00, 0x78FB18);  // CLC, XCE, SEI
                        MemMgr.WriteByte(0xFF03, 0x5C);      // JML
                        MemMgr.WriteLong(0xFF04, FnxAddressPtr);
                    }
                    else
                    {
                        MemMgr.WriteWord(MemoryMap.VECTOR_ERESET, FnxAddressPtr);
                    }
                }
            }
            else if (extension.Equals(".FNXML"))
            {
                this.ResetMemory();
                FoeniXmlFile fnxml = new FoeniXmlFile(this, ResCheckerRef);
                fnxml.Load(LoadedKernel);
                boardVersion = fnxml.Version;
            }
            else if (extension.Equals(".BIN"))
            {
                int flen = (int)(info.Length);
                BinaryReader reader = new BinaryReader(info.OpenRead());
                byte[] DataBuffer;
                DataBuffer = reader.ReadBytes(flen);
                int DataStartAddress = 0;
                // Ask the user what address to write in the header
                bool isAddressValid = false;
                do
                {
                    InputDialog addressWindow = new InputDialog("Enter the Start Address (Hexadecimal)", "Start Address");
                    DialogResult result = addressWindow.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            DataStartAddress = Convert.ToInt32(addressWindow.GetValue(), 16);
                            isAddressValid = true;
                        }
                        catch
                        {
                            MessageBox.Show("Invalid Start Address", "Bin File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        return false;
                    }

                } while (!isAddressValid);
                // Copy the data into memory
                MemMgr.RAM.CopyBuffer(DataBuffer, 0, DataStartAddress, flen);

                if (BoardVersionHelpers.IsF256(boardVersion))
                {
                    bool binOverlapsFlash = DataStartAddress >= 0x08_0000;
                    if (binOverlapsFlash)
                    {
                        int flashStart = DataStartAddress - 0x08_0000;
                        ((MemoryManagerF256)MemMgr).FLASHF256.CopyBuffer(DataBuffer, 0, flashStart, flen);
                    }
                }
            }
            else if (extension.Equals(".CSV"))
            {
                string[] entries = System.IO.File.ReadAllLines(info.FullName);
                foreach (string entry in entries)
                {
                    // Each entry is a block number, and a file name
                    string[] split = entry.Split(',');
                    if (split.Length > 1)
                    {
                        string blockFile = Path.Combine(System.AppContext.BaseDirectory, "roms", "F256", split[1]);
                        FileInfo blockInfo = new FileInfo(blockFile);
                        int blockNumber = Convert.ToInt32(split[0], 16);
                        int address = blockNumber * 8192;
                        BinaryReader reader = new BinaryReader(blockInfo.OpenRead());
                        byte[] DataBuffer = reader.ReadBytes(8192);
                        // Handle the case when the file is less than 8192 bytes.
                        ((MemoryManagerF256)MemMgr).FLASHF256.CopyBuffer(DataBuffer, 0, address, DataBuffer.Length <= 8192 ? DataBuffer.Length : 8192);

                        reader.Close();
                    }
                }

                if (BoardVersionHelpers.IsF256_MMU(boardVersion))
                    ((MemoryManagerF256)MemMgr).MMU.WriteByte(0xF, 0x7F);
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
            if (lstFile.Lines.Count > 0 && !BoardVersionHelpers.IsF256(boardVersion))
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

            // Reset the keyboards
            MemMgr.PS2KEYBOARD.WriteByte(0, 0);
            MemMgr.PS2KEYBOARD.WriteByte(4, 0);

            if (BoardVersionHelpers.IsF256(boardVersion))
            {
                ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA0.WriteByte(2, 0xFF);  // DDRB
                ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA0.WriteByte(3, 0xFF);  // DDRA
                ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA0.WriteByte(0, 0xFF); // JOYSTICK 2
                ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA0.WriteByte(1, 0xFF); // JOYSTICK 1
                ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA0.WriteByte(2, 0);  // DDRB
                ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA0.WriteByte(3, 0);  // DDRA

                if (boardVersion != BoardVersion.RevJr_6502 && boardVersion != BoardVersion.RevJr_65816)
                {
                    ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA1.WriteByte(0, 0);
                    ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA1.WriteByte(1, 0);
                    ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA1.WriteByte(2, 0);  // DDRB
                    ((MemoryManagerF256)MemMgr).VIAREGISTERS.VIA1.WriteByte(3, 0);  // DDRA
                }
            }

            return true;
        }

        public void ResetMemory()
        {
            MemMgr.RAM.Zero();
            MemMgr.VICKY.Zero();
            MemMgr.DMA.Zero();
        }

        public static int TextAddressToInt(string value)
        {
            return Convert.ToInt32(value.Replace("$", "").Replace(":", ""), 16);
        }
    }
}
