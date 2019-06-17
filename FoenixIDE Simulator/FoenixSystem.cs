using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.Processor;
using FoenixIDE.Display;
using FoenixIDE.Common;
using System.Threading;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.UI;

namespace FoenixIDE
{
    public class FoenixSystem
    {
        public MemoryManager Memory = null;
        public Processor.CPU CPU = null;
        public Gpu gpu = null;

        public DeviceEnum InputDevice = DeviceEnum.Keyboard;
        public DeviceEnum OutputDevice = DeviceEnum.Screen;

        public Thread CPUThread = null;
        private String defaultKernel = @"ROMs\kernel.hex";

        public ResourceChecker Resources;
        public Processor.Breakpoints Breakpoints;

        public FoenixSystem(Gpu gpu)
        {
            Memory = new MemoryManager
            {
                RAM = new MemoryRAM(MemoryMap.RAM_START, MemoryMap.RAM_SIZE), // 2MB RAM
                VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),   // 64K IO space
                VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE), // 4MB Video
                FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE), // 8MB RAM
                BEATRIX = new MemoryRAM(MemoryMap.BEATRIX_START, MemoryMap.BEATRIX_SIZE),

                // Special devices
                MATH = new MathCoproRegisters(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1), // 48 bytes
                CODEC = new CodecRAM(MemoryMap.CODEC_WR_CTRL, 2),  // This register is only a single byte but we allow writing a word
                KEYBOARD = new KeyboardRegister(MemoryMap.KBD_DATA_BUF, 5),
                SDCARD = new SDCardRegister(MemoryMap.SDCARD_DATA, 2),
                INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 3),
                UART1 = new UART(MemoryMap.UART1_REGISTERS, 8),
                UART2 = new UART(MemoryMap.UART2_REGISTERS, 8)
            };

            this.CPU = new CPU(Memory);
            this.CPU.SimulatorCommand += CPU_SimulatorCommand;
            this.gpu = gpu;
            gpu.VRAM = Memory.VIDEO;
            gpu.RAM = Memory.RAM;
            gpu.VICKY = Memory.VICKY;
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

        public void ResetCPU()
        {
            CPU.Halt();

            gpu.Refresh();
            // Clear out Vicky's memory
            Memory.VICKY.Zero();
            // This fontset is loaded just in case the kernel doesn't provide one.
            gpu.LoadFontSet("Foenix", @"Resources\Bm437_PhoenixEGA_8x8.bin", 0, CharacterSet.CharTypeCodes.ASCII_PET, CharacterSet.SizeCodes.Size8x8);

            if (defaultKernel.EndsWith(".fnxml", true, null))
            {
                FoeniXmlFile fnxml = new FoeniXmlFile(Memory, Resources, CPUWindow.Instance.breakpoints);
                fnxml.Load(defaultKernel);
            }
            else
            {
                defaultKernel = HexFile.Load(Memory, defaultKernel);
            }

            // If the reset vector is not set in Bank 0, but it is set in Bank 18, the copy bank 18 into bank 0.
            if (Memory.ReadLong(0xFFE0) == 0 && Memory.ReadLong(0x18_FFE0) != 0)
            {
                Memory.RAM.Copy(0x180000, Memory.RAM, 0, MemoryMap.PAGE_SIZE);
            }
            CPU.Reset();
        }

        public void SetKernel(String value)
        {
            defaultKernel = value;
        }
    }
}
