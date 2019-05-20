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

        public const int KBD_DATA_BUF = 0xAF_1060; // Keyboard input, output buffer
        public const int KBD_STATUS_PORT = 0xAF_1064;  // keyboard status port
        public const int SDCARD_DATA = 0xAF_E808;
        public const int SDCARD_CMD = 0xAF_E809;
        public const int CODEC_WR_CTRL = 0xAF_E822; // codec write address

        #endregion
    }
}
