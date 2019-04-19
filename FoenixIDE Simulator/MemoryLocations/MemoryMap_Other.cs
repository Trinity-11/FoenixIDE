using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region GPU Memory
        // c# Memory Map

        public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 

        public const int FONT0_MEMORY_BANK_START = 0xAF_8000;
        public const int FONT1_MEMORY_BANK_START = 0xAF_8800;
        public const int FONT_MEMORY_BANK_END = 0xAF_BFFF;

        public const int FG_CHAR_LUT_PTR = 0xAF_1F40; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR = 0xAF_1F80; // 15 color lookup table

        #endregion
    }
}
