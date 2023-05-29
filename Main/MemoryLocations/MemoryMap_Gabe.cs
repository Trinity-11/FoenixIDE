using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Gabe Memory Map
        public const int FLOAT_START = 0xAF_E200;
        public const int FLOAT_END = 0xAF_E20F;

        public const int OPL2_S_BASE = 0xAF_E700;   // Start of OPL2 Stereo range

        // Joystick Ports
        public const int JOYSTICK0 = 0xAF_E800; // (R) Joystick 0 - J7(Next to Buzzer)
        public const int JOYSTICK1 = 0xAF_E801; // (R) Joystick 1 - J8
        public const int JOYSTICK2 = 0xAF_E802; // (R) Joystick 2 - J9
        public const int JOYSTICK3 = 0xAF_E803; // (R) Joystick 3 - J10(next to SD Card)

        // Dip switch Ports
        public const int DIPSWITCH = 0xAF_E804; // (R) $AFE804...$AFE807
        public const int DIPSWITCH_JR = 0XD670;

        public const int SDCARD_START = 0xAF_E808;  // Start of SDCARD memory range

        public const int DIP_USER_MODE = 0xAF_E80D;
        public const int DIP_BOOT_MODE = 0xAF_E80E;  

        public const int SDCARD_END = 0xAF_E81F;    // End of SDCARD memory range
        public const int SDCARD_SIZE = 0x09;        // Size of SD Card memory range
        public const int SDCARD_DATA = 0xAF_E808;
        public const int SDCARD_CMD = 0xAF_E809;
        public const int SDCARD_STAT = 0xAF_E810;

        // F256JR addresses
        public const int SDCARD_JR = 0x00_DD00;
        public const int MATH_JR = 0x00_DE00;

        // Handling code in CODEC_RAM
        public const int CODEC_START = 0xAF_E820;     // Start of CODEC memory range
        public const int CODEC_SIZE = 0x04;           // Size of CODEC memory range
        public const int CODEC_WR_CTRL = 0xAF_E822;   // codec write address
        public const int CODEC_WR_CTRL_JR = 0x00_D620;   // codec write address

        // GABE
        /* 
            GABE_CTRL_PWR_LED   = $01     ; Controls the LED in the Front of the case (Next to the reset button)
            GABE_CTRL_SDC_LED   = $02     ; Controls the LED in the Front of the Case (Next to SDCard)
            GABE_CTRL_BUZZER    = $10     ; Controls the Buzzer
            GABE_CTRL_WRM_RST   = $80     ; Warm Reset (needs to Setup other registers)
        */
        public const int GABE_MSTR_CTRL = 0xAF_E880;
        public const int GABE_NOTUSED = 0xAF_E881;  // Reserved for future use
        public const int GABE_RST_AUTH0 = 0xAF_E882; // Must Contain the BYTE $AD for Reset to Activate
        public const int GABE_RST_AUTH1 = 0xAF_E883; // Must Contain the BYTE $DE for Reset to Activate

        // READ
        public const int GABE_RNG_DAT_LO = 0xAF_E884 ; // Low Part of 16Bit RNG Generator
        public const int GABE_RNG_DAT_HI = 0xAF_E885 ; // High Part of 16Bit RNG Generator

        // WRITE
        public const int GABE_RNG_SEED_LO = 0xAF_E884 ; // Low Part of 16Bit RNG Generator
        public const int GABE_RNG_SEED_HI = 0xAF_E885 ; // High Part of 16Bit RNG Generator

        // READ
        //GABE_RNG_LFSR_DONE  = $80     ; indicates that Output = SEED Database
        public const int GABE_RNG_STAT = 0xAF_E886 ;


        // WRITE
        /*
          GABE_RNG_CTRL_EN    = $01     ; Enable the LFSR BLOCK_LEN
          GABE_RNG_CTRL_DV    = $02     ; After Setting the Seed Value, Toggle that Bit for it be registered
        */
        public const int GABE_RNG_CTRL = 0xAF_E886 ;

        /*
            GABE_SYS_STAT_MID0  = $01     ; Machine ID -- LSB
            GABE_SYS_STAT_MID1 = $02     ; Machine ID -- MSB
            GABE_SYS_STAT_EXP = $08     ; if Zero, there is an Expansion Card Preset
            GABE_SYS_STAT_CPUA  = $40     ; Indicates the(8bit/16bit) Size of the Accumulator
            GABE_SYS_STAT_CPUX  = $80     ; Indicates the(8bit/16bit) Size of the Accumulator
        */
        public const int GABE_SYS_STAT = 0xAF_E887 ;

        public const int CODEC_START_FMX = 0xAF_E900;    // Start of CODEC for FMX
        public const int CODEC_WR_CTRL_FMX = 0xAF_E902;  // codec write address for FMX

        public const int GABE_SDC_CTRL_START = 0xAF_EA00;
        public const int GABE_SDC_CTRL_SIZE = 0x28;

        public const int GABE_SDC_VERSION_REG = GABE_SDC_CTRL_START;
        public const int GABE_SDC_CONTROL_REG = GABE_SDC_CTRL_START + 1;
        public const int GABE_SDC_TRANS_TYPE_REG = GABE_SDC_CTRL_START + 2;
        public const int GABE_SDC_TRANS_CONTROL_REG = GABE_SDC_CTRL_START + 3;
        public const int GABE_SDC_TRANS_STATUS_REG = GABE_SDC_CTRL_START + 4;
        public const int GABE_SDC_TRANS_ERROR_REG = GABE_SDC_CTRL_START + 5;

        // Read/write offsets
        public const int GABE_SDC_SD_ADDR_7_0_REG = GABE_SDC_CTRL_START + 7;
        public const int GABE_SDC_SD_ADDR_15_8_REG = GABE_SDC_CTRL_START + 8;
        public const int GABE_SDC_SD_ADDR_23_16_REG = GABE_SDC_CTRL_START + 9;
        public const int GABE_SDC_SD_ADDR_31_24_REG = GABE_SDC_CTRL_START + 0xA;

        public const int GABE_SDC_SPI_CLK_DEL_REG = GABE_SDC_CTRL_START + 0xB;

        public const int GABE_SDC_RX_FIFO_DATA_REG = GABE_SDC_CTRL_START + 0x10; // Data from the Block Read
        public const int GABE_SDC_RX_FIFO_DATA_CNT_HI = GABE_SDC_CTRL_START + 0x12; // How many Bytes in the FIFO HI
        public const int GABE_SDC_RX_FIFO_DATA_CNT_LO = GABE_SDC_CTRL_START + 0x13; // How many Bytes in the FIFO LO
        public const int GABE_SDC_RX_FIFO_CTRL_REG = GABE_SDC_CTRL_START + 0x14; // Bit0 Force Empty - Set to 1 to clear FIFO, self clearing(the bit)

        public const int GABE_SDC_TX_FIFO_DATA_REG = GABE_SDC_CTRL_START + 0x20; // Write Data Block here
        public const int GABE_SDC_TX_FIFO_CTRL_REG = GABE_SDC_CTRL_START + 0x24; // Bit0 Force Empty - Set to 1 to clear FIFO, self clearing(the bit)

        #endregion
    }
}
