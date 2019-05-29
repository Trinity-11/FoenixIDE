using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Direct page
        // c# Direct page Addresses

        public const int VKY_TXT_CURSOR_CHAR_REG = 0xAF_0012;

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

        // The SuperIO chip is in VICKY address range but wired to GAVIN (code under Gavin folder)
        //public const int KBD_DATA_BUF = 0xAF_1060; // Keyboard input, output buffer
        //public const int KBD_STATUS_PORT = 0xAF_1064;  // keyboard status port

        public const int SDCARD_START = 0xAF_E808; // Start of SDCARD memory range
        public const int SDCARD_END = 0xAF_E80F;   // End of SDCARD memory range
        public const int SDCARD_DATA = 0xAF_E808;
        public const int SDCARD_CMD = 0xAF_E809;

        public const int CODEC_WR_CTRL = 0xAF_E822; // codec write address
        public const int CODEC_START = 0xAF_E810;   // Start of CODEC memory range
        public const int CODEC_END = 0xAF_E823;     // End of CODEC memory range

        #endregion
    }
}
