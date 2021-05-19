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
        public SortedList<int, WatchedMemory> WatchList = new SortedList<int, WatchedMemory>();
        private string LoadedKernel;

        public FoenixSystem(BoardVersion version, string DefaultKernel)
        {
            boardVersion = version;

            int memSize = MemoryMap.RAM_SIZE;
            CodecRAM codec = null;
            SDCardDevice sdcard = null;
            byte SystemStat = 0; // FMX
            int keyboardAddress = MemoryMap.KBD_DATA_BUF_FMX; // FMX

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
            }
            if (boardVersion == BoardVersion.RevB)
            {
                codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL, 2);  // This register is only a single byte but we allow writing a word
                sdcard = new CH376SRegister(MemoryMap.SDCARD_DATA, MemoryMap.SDCARD_SIZE);
            }
            else
            {
                codec = new CodecRAM(MemoryMap.CODEC_WR_CTRL_FMX, 2);  // This register is only a single byte but we allow writing a word
                sdcard = new GabeSDController(MemoryMap.GABE_SDC_CTRL_START, MemoryMap.GABE_SDC_CTRL_SIZE);
            }

            MemMgr = new MemoryManager
            {
                RAM = new MemoryRAM(MemoryMap.RAM_START, memSize),                        // RAM: 2MB Rev B & U, 4MB Rev C & U+
                VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),       // 60K
                VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE),       // 4MB Video
                FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE),       // 8MB RAM
                GABE = new GabeRAM(MemoryMap.GABE_START, MemoryMap.GABE_SIZE),   // 4K 

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
                TIMER2 = new TimerRegister(MemoryMap.TIMER2_CTRL_REG, 8)
            };
            MemMgr.CODEC = codec;
            MemMgr.KEYBOARD.SetKernel(this);

            // Assign memory variables used by other processes
            CPU = new CPU(MemMgr);
            
            MemMgr.VDMA.setVideoRam(MemMgr.VIDEO);
            MemMgr.VDMA.setSystemRam(MemMgr.RAM);
            MemMgr.GABE.WriteByte(MemoryMap.GABE_SYS_STAT - MemoryMap.GABE_START, SystemStat);

            // Load the kernel.hex if present
            ResetCPU(DefaultKernel);

            // Write bytes $9F in the joystick registers to mean that they are not installed.
            MemMgr.WriteWord(0xAFE800, 0x9F9F);
            MemMgr.WriteWord(0xAFE802, 0x9F9F);
            MemMgr.TIMER0.TimerInterruptDelegate += TimerEvent0;
            MemMgr.TIMER1.TimerInterruptDelegate += TimerEvent1;
            MemMgr.TIMER2.TimerInterruptDelegate += TimerEvent2;
        }

        private void TimerEvent0()
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
        private void TimerEvent1()
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

        public BoardVersion GetVersion()
        {
            return boardVersion;
        }
        public void SetVersion(BoardVersion rev)
        {
            boardVersion = rev;
        }
        // return true if the CPU was reset and the program was loaded
        public bool ResetCPU(string kernelFilename)
        {
            if (CPU != null)
            {
                CPU.DebugPause = true;
                //CPU.Halt();
            }

            if (kernelFilename != null)
            {
                LoadedKernel = kernelFilename;
            }

            // If the reset vector is not set in Bank 0, but it is set in Bank 18, the copy bank 18 into bank 0.
            int BasePageAddress = 0x18_0000;
            if (boardVersion == BoardVersion.RevC || boardVersion == BoardVersion.RevUPlus)
            {
                BasePageAddress = 0x38_0000;
            }

            if (LoadedKernel.EndsWith(".fnxml", true, null))
            {
                this.ResetMemory();
                FoeniXmlFile fnxml = new FoeniXmlFile(this, ResCheckerRef);
                fnxml.Load(LoadedKernel);
                boardVersion = fnxml.Version;
            }
            else
            {
                LoadedKernel = HexFile.Load(MemMgr.RAM, LoadedKernel, BasePageAddress, out _, out _);
                if (LoadedKernel != null)
                {
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
                }
                else
                {
                    return false;
                }
            }

            // See if lines of code exist in the 0x18_0000 to 0x18_FFFF block for RevB or 0x38_0000 to 0x38_FFFF block for Rev C
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
