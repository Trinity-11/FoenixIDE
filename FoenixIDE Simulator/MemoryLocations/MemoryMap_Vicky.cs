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

        public const int KBD_DATA_BUF = 0xAF_1060; // Keyboard input, output buffer
        public const int KBD_STATUS_PORT = 0xAF_1064;  // keyboard status port

        public const int FG_CHAR_LUT_PTR = 0xAF_1F40; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR = 0xAF_1F80; // 15 color lookup table

        public const int GRP_LUT_BASE_ADDR = 0xAF_2000;
        public const int GAMMA_BASE_ADDR = 0xAF_4000;

        public const int FONT0_MEMORY_BANK_START = 0xAF_8000;
        public const int FONT1_MEMORY_BANK_START = 0xAF_8800;

        public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 

        #endregion

        #region Beatrix Memory Map

        // These items are actually in BEATRIX
        // Joystick Ports
        public const int JOYSTICK0 = 0xAF_E800; // (R) Joystick 0 - J7(Next to Buzzer)
        public const int JOYSTICK1 = 0xAF_E801; // (R) Joystick 1 - J8
        public const int JOYSTICK2 = 0xAF_E802; // (R) Joystick 2 - J9
        public const int JOYSTICK3 = 0xAF_E803; // (R) Joystick 3 - J10(next to SD Card)

        // Dip switch Ports
        public const int DIPSWITCH = 0xAF_E804; // (R) $AFE804...$AFE807

        public const int SDCARD_START = 0xAF_E808;  // Start of SDCARD memory range
        public const int SDCARD_END = 0xAF_E81F;    // End of SDCARD memory range
        public const int SDCARD_SIZE = 0x07;        // Size of SD Card memory range
        public const int SDCARD_DATA = 0xAF_E808;
        public const int SDCARD_CMD = 0xAF_E809;

        // Handling code in CODEC_RAM
        public const int CODEC_START = 0xAF_E820;   // Start of CODEC memory range
        public const int CODEC_END = 0xAF_E823;     // End of CODEC memory range
        public const int CODEC_SIZE = 0x04;         // Size of CODEC memory range
        public const int CODEC_WR_CTRL = 0xAF_E822; // codec write address

        #endregion
    }
}
