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
        public Gpu gpu = null;

        //public Thread CPUThread = null;
        private String defaultKernel = @"ROMs\kernel.hex";

        public ResourceChecker Resources;
        public Processor.Breakpoints Breakpoints;
        public ListFile lstFile;
        private BoardVersion boardVersion;

        public FoenixSystem(Gpu gpu, BoardVersion version)
        {
            this.gpu = gpu;
            boardVersion = version;
            // This fontset is loaded just in case the kernel doesn't provide one.
            gpu.LoadFontSet("Foenix", @"Resources\Bm437_PhoenixEGA_8x8.bin", 0, CharacterSet.CharTypeCodes.ASCII_PET, CharacterSet.SizeCodes.Size8x8);
        }

        private void CPU_SimulatorCommand(int EventID)
        {
            switch (EventID)
            {
                case SimulatorCommands.RefreshDisplay:
                    gpu.RefreshTimer = 0;
                    break;
                default:
                    break;
            }
        }
       
        // return true if the CPU was reset and the program was loaded
        public bool ResetCPU(bool ResetMemory, string kernelFilename)
        {
            if (CPU != null)
            {
                CPU.Halt();
            }

            gpu.Refresh();

            if (kernelFilename != null)
            {
                defaultKernel = kernelFilename;
            }

            if (defaultKernel.EndsWith(".fnxml", true, null))
            {
                FoeniXmlFile fnxml = new FoeniXmlFile(Resources, CPUWindow.Instance.breakpoints);
                fnxml.Load(defaultKernel);
                boardVersion = fnxml.Version;
                ConfigureMemoryManager(fnxml.buffer);
            }
            else
            {
                HexFile hexFileLoader = new HexFile();
                defaultKernel = hexFileLoader.Load(defaultKernel);
                if (defaultKernel != null)
                {
                    if (ResetMemory)
                    {
                        lstFile = new ListFile(defaultKernel);
                    }
                    else
                    {
                        // TODO: We should really ensure that there are no duplicated PC in the list
                        ListFile tempList = new ListFile(defaultKernel);
                        lstFile.Lines.InsertRange(0, tempList.Lines);
                    }
                    ConfigureMemoryManager(hexFileLoader.buffer);
                }
                else
                {
                    return false;
                }
            }


            // If the reset vector is not set in Bank 0, but it is set in Bank 18, the copy bank 18 into bank 0.
            int BasePageAddress = 0x18_0000;
            if (boardVersion == BoardVersion.RevC)
            {
                BasePageAddress = 0x38_0000;
            }
            if (MemMgr.ReadLong(MemoryMap.VECTORS_BEGIN) == 0 && MemMgr.ReadLong(BasePageAddress + 0xFFE0) != 0 
                ||
                MemMgr.ReadLong(MemoryMap.VECTOR_ERESET) == 0 && MemMgr.ReadLong(BasePageAddress + 0xFFFC) != 0
                )
            {
                MemMgr.RAM.Duplicate(BasePageAddress, 0, MemoryMap.PAGE_SIZE);
                // See if lines of code exist in the 0x18_0000 to 0x18_FFFF block
                List<DebugLine> copiedLines = new List<DebugLine>();
                if (lstFile.Lines.Count > 0)
                {
                    List<DebugLine> tempLines = new List<DebugLine>();
                    foreach (DebugLine line in lstFile.Lines)
                    {
                        if (line.PC >= BasePageAddress && line.PC < BasePageAddress + 0x1_0000)
                        {
                            DebugLine dl = (DebugLine)line.Clone();
                            dl.PC -= BasePageAddress;
                            copiedLines.Add(dl);
                        }
                    }
                }
                if (copiedLines.Count > 0)
                {
                    lstFile.Lines.InsertRange(0, copiedLines);
                }
            }
            CPU.Reset();
            return true;
        }

        public void ConfigureMemoryManager(byte[] buffer)
        {
            if (boardVersion == BoardVersion.RevB)
            {
                MemMgr = new MemoryManager
                {
                    RAM = new MemoryRAM(MemoryMap.RAM_START, MemoryMap.RAM_SIZE),             // 2MB RAM - extensible to 4MB
                    VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),       // 60K
                    VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE),       // 4MB Video
                    FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE),       // 8MB RAM
                    BEATRIX = new MemoryRAM(MemoryMap.BEATRIX_START, MemoryMap.BEATRIX_SIZE), // 4K 

                    // Special devices
                    MATH = new MathCoproRegisters(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1), // 48 bytes
                    CODEC = new CodecRAM(MemoryMap.CODEC_WR_CTRL, 2),  // This register is only a single byte but we allow writing a word
                    KEYBOARD = new KeyboardRegister(MemoryMap.KBD_DATA_BUF, 5),
                    SDCARD = new SDCardRegister(MemoryMap.SDCARD_DATA, MemoryMap.SDCARD_SIZE),
                    INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4),
                    UART1 = new UART(MemoryMap.UART1_REGISTERS, 8),
                    UART2 = new UART(MemoryMap.UART2_REGISTERS, 8),
                    OPL2 = new OPL2(MemoryMap.OPL2_S_BASE, 256),
                    MPU401 = new MPU401(MemoryMap.MPU401_REGISTERS, 2),
                    VDMA = new VDMA(MemoryMap.VDMA_START, MemoryMap.VDMA_SIZE)
                };
            } 
            else
            {
                MemMgr = new MemoryManager
                {
                    RAM = new MemoryRAM(MemoryMap.RAM_START, 2 * MemoryMap.RAM_SIZE),         // 4MB RAM
                    VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),       // 60K
                    VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE),       // 4MB Video
                    FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE),       // 8MB RAM
                    BEATRIX = new MemoryRAM(MemoryMap.BEATRIX_START, MemoryMap.BEATRIX_SIZE), // 4K 

                    // Special devices
                    MATH = new MathCoproRegisters(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1), // 48 bytes
                    CODEC = new CodecRAM(MemoryMap.CODEC_WR_CTRL_FMX, 2),  // This register is only a single byte but we allow writing a word
                    KEYBOARD = new KeyboardRegister(MemoryMap.KBD_DATA_BUF, 5),
                    SDCARD = new SDCardRegister(MemoryMap.SDCARD_DATA, MemoryMap.SDCARD_SIZE),
                    INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4),
                    UART1 = new UART(MemoryMap.UART1_REGISTERS, 8),
                    UART2 = new UART(MemoryMap.UART2_REGISTERS, 8),
                    OPL2 = new OPL2(MemoryMap.OPL2_S_BASE, 256),
                    MPU401 = new MPU401(MemoryMap.MPU401_REGISTERS, 2),
                    VDMA = new VDMA(MemoryMap.VDMA_START, MemoryMap.VDMA_SIZE)
                };
            }
            MemMgr.RAM.CopyBuffer(buffer, 0, 0, MemMgr.RAM.Length);
            this.CPU = new CPU(MemMgr);
            this.CPU.SimulatorCommand += CPU_SimulatorCommand;

            gpu.VRAM = MemMgr.VIDEO;
            gpu.RAM = MemMgr.RAM;
            gpu.VICKY = MemMgr.VICKY;
            MemMgr.VDMA.setVideoRam(MemMgr.VIDEO);

            // Write bytes $9F in the joystick registers to mean that they are not installed.
            MemMgr.WriteWord(0xAFE800, 0x9F9F);
            MemMgr.WriteWord(0xAFE802, 0x9F9F);
        }
    }
}
