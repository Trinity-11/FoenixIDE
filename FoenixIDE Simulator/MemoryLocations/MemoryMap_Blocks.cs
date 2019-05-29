﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Memory Blocks
        // c# Memory Blocks

        public const int RAM_START = 0x00_0000; // Beginning of 2MB RAM
        public const int RAM_END = 0x1F_FFFF; // End of 2MB RAM
        public const int RAM_SIZE = 0x20_0000; // 2MB RAM
        public const int PAGE_SIZE = 0x1_0000; // 64KB

        // Beginning of Vicky IO Register Address Space
        public const int IO_START = 0xAF_0000; // Beginning of I/O Space
        public const int IO_END = 0xAF_FFFF; // End of I/O Space
        public const int IO_SIZE = 0x00_FFFF; // 64KB

        public const int VIDEO_START = 0xB0_0000;
        public const int VIDEO_SIZE = 0x40_0000;  // 4MB Video RAM

        public const int FLASH_START = 0xF0_0000; // Beginning of FLASH
        public const int FLASH_USER_START = 0xF8_0000;
        public const int FLASH_END = 0xFF_FFFF; // End of 1MB FLASH 
        public const int FLASH_SIZE = 0x10_0000; // 1MB between the two FLASHES

        #endregion
    }
}
