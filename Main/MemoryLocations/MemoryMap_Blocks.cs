using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Memory Blocks
        // c# Memory Blocks

        public const int RAM_START = 0x00_0000; // Beginning of 2MB RAM
        public const int RAM_SIZE =  0x20_0000; // 2MB RAM
        public const int PAGE_SIZE = 0x1_0000;  // 64KB

        // Beginning of Vicky Address Space
        public const int VICKY_START = VICKY_BASE_ADDR;            // Beginning of I/O Space
        public const int VICKY_START_JR = 0xD000;                  // IO Page 0
        public const int VICKY_END = 0xAF_DFFF;                    // End of I/O Space
        public const int VICKY_SIZE = VICKY_END - VICKY_START + 1; // 64KB

        public const int GABE_START = 0xAF_E000;
        public const int GABE_END = 0xAF_FFFF;
        public const int GABE_SIZE = GABE_END - GABE_START + 1;

        public const int VIDEO_START = 0xB0_0000;
        public const int VIDEO_SIZE = 0x40_0000;  // 4MB Video RAM

        public const int FLASH_START = 0xF0_0000; // Beginning of FLASH
        public const int FLASH_USER_START = 0xF8_0000;
        public const int FLASH_END = 0xFF_FFFF; // End of 1MB FLASH 
        public const int FLASH_SIZE = 0x10_0000; // 1MB between the two FLASHES

        #endregion
    }
}
