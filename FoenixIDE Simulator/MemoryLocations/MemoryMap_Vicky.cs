using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Vicky Memory Map

        public const int VICKY_BASE_ADDR = 0xAF_0000;
        public const int VKY_TXT_CURSOR_CHAR_REG = 0xAF_0012;
        public const int VKY_TXT_CURSOR_CTRL_REG = 0xAF_0010;

        
        public const int TILE_CONTROL_REGISTER_ADDR = 0xAF_0100;
        public const int BITMAP_CONTROL_REGISTER_ADDR = 0xAF_0140;
        public const int SPRITE_CONTROL_REGISTER_ADDR = 0xAF_0200;
        
        public const int RTC_SEC = 0xAF_0800; // Seconds Register
        public const int RTC_SEC_ALARM = 0xAF_0801; // Seconds Alarm Register
        public const int RTC_MIN = 0xAF_0802; // Minutes Register
        public const int RTC_MIN_ALARM = 0xAF_0803; // Minutes Alarm Register
        public const int RTC_HRS = 0xAF_0804; // Hours Register
        public const int RTC_HRS_ALARM = 0xAF_0805; // Hours Alarm Register
        public const int RTC_DAY = 0xAF_0806; // Day Register
        public const int RTC_DAY_ALARM = 0xAF_0807; // Day Alarm Register
        public const int RTC_DOW = 0xAF_0808; // Day of Week Register
        public const int RTC_MONTH = 0xAF_0809; // Month Register
        public const int RTC_YEAR = 0xAF_080A; // Year Register
        public const int RTC_RATES = 0xAF_080B; // Rates Register
        public const int RTC_ENABLE = 0xAF_080C; // Enables Register
        public const int RTC_FLAGS = 0xAF_080D; // Flags Register
        public const int RTC_CTRL = 0xAF_080E; // Control Register
        public const int RTC_CENTURY = 0xAF_080F; // Century Register

        public const int SUPERIO_START = 0xAF_1000;
        public const int SUPERIO_END = 0xAF_13FF;
        public const int KBD_DATA_BUF = 0xAF_1060; // Keyboard input, output buffer
        public const int KBD_STATUS_PORT = 0xAF_1064;  // keyboard status port

        // FDC  - $AF:13F0
        // LPT1 - $AF:1378
        public const int UART1_REGISTERS = 0xAF_13F8;
        public const int UART2_REGISTERS = 0xAF_12F8;
        // KBD  - $AF:1060
        // GAME - $AF:1200 - Not Connected
        // MPU  - $AF:1330

        public const int MPU401_REGISTERS = 0xAF_1330;  // 2 bytes
        public const int MPU401_DATA_REG = 0xAF_1330;
        public const int MPU401_STATUS_REG = 0xAF_1331;

        public const int FG_CHAR_LUT_PTR = 0xAF_1F40; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR = 0xAF_1F80; // 15 color lookup table

        public const int GRP_LUT_BASE_ADDR = 0xAF_2000;
        public const int GAMMA_BASE_ADDR = 0xAF_4000;

        public const int FONT0_MEMORY_BANK_START = 0xAF_8000;
        public const int FONT1_MEMORY_BANK_START = 0xAF_8800;

        public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 

        #endregion
    }
}
