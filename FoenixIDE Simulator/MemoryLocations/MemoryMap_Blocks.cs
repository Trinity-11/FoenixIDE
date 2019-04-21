using System;
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

        public const int RAM_START = 0x000000; // Beginning of 2MB RAM
        public const int RAM_END = 0x1FFFFF; // End of 2MB RAM
        public const int RAM_SIZE = 0x200000; // 2MB RAM
        public const int PAGE_SIZE = 0x10000; // 64KB


        public const int IO_START = 0xAF0000; // Beginning of I/O Space
        public const int IO_END = 0xAFFFFF; // End of I/O Space
        public const int IO_SIZE = 0x00FFFF; // 64KB

        public const int VIDEO_START = 0xB00000;
        public const int VIDEO_SIZE = 0x400000;  // 4MB Video RAM

        public const int FLASH_START = 0xF00000; // Beginning of FLASH
        public const int FLASH_SYSTEM_START = 0xF00000;
        public const int FLASH_USER_START = 0xF80000;
        public const int FLASH_END = 0xFFFFFF; // End of 1MB FLASH 
        public const int FLASH_SIZE = 0x100000; // 1MB between the two FLASHES

        public const int MATH_START = 0x00_0100;
        public const int MATH_END = 0x00_012F;

        public const int CODEC_WR_CTRL = 0xAF_E822; // codec write address

        #endregion
    }
}
