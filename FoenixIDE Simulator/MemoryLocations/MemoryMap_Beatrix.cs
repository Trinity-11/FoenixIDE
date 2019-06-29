using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Beatrix Memory Map

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

        public const int OPL2_S_BASE = 0xAF_E700;   // Start of OPL2 Stereo range

        #endregion
    }
}
