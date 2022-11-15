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
        public const int GAMMA_CTRL_REG = 0xAF_0002;
        public const int BORDER_CTRL_REG = 0xAF_0004; // border is enabled if bit 0 is 1.

        public const int BORDER_X_SIZE = 0xAF_0008; // X-  Values: 0 - 32 (Default: 32)
        public const int BORDER_Y_SIZE = 0xAF_0009; // Y- Values 0 -32 (Default: 32)


        public const int BACKGROUND_COLOR_B = 0xAF_000D; // When in Graphic Mode, if a pixel is "0" then the Background pixel is chosen
        public const int BACKGROUND_COLOR_G = 0xAF_000E;
        public const int BACKGROUND_COLOR_R = 0xAF_000F;

        public const int VKY_TXT_CURSOR_CTRL_REG = 0xAF_0010;
        public const int VKY_TXT_CURSOR_CHAR_REG = 0xAF_0012;
        public const int VKY_TXT_CURSOR_X_REG = 0xAF_0014;
        public const int VKY_TXT_CURSOR_Y_REG = 0xAF_0016;
        public const int VKY_TXT_CURSOR_CTRL_REG_JR = 0xD010;
        public const int VKY_TXT_CURSOR_CHAR_REG_JR = 0xD012;
        public const int VKY_TXT_CURSOR_X_REG_JR = 0xD014;
        public const int VKY_TXT_CURSOR_Y_REG_JR = 0xD016;

        // Line Interrupt Registers
        public const int VKY_LINE_IRQ_CTRL_REG =  0xAF001B; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE0_CMP_VALUE_LO = 0xAF001C; // Write Only[7:0]
        public const int VKY_LINE1_CMP_VALUE_LO = 0xAF001E; // Write Only[7:0]
        public const int VKY_LINE_IRQ_CTRL_REG_JR =   0xD018; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE_CMP_VALUE_JR = 0xD019; // Write Only[7:0]


        public const int BITMAP_CONTROL_REGISTER_ADDR = 0xAF_0100; // 2 layers - 8 bytes
        public const int TILE_CONTROL_REGISTER_ADDR = 0xAF_0200; // 12 bytes for each tile layer
        public const int TILE_CONTROL_REGISTER_ADDR_JR = 0xD200;
        public const int SPRITE_CONTROL_REGISTER_ADDR = 0xAF_0C00; // 8 bytes for each sprite

        public const int VDMA_START = 0xAF_0400;
        public const int VDMA_SIZE = 0x31; // from $af:0400 to $af:0430

        public const int MOUSE_PTR_GRAP0 = 0xAF_0500; // image for pointer 0
        public const int MOUSE_PTR_GRAP1 = 0xAF_0600; // image for pointer 1

        public const int MOUSE_PTR_REG = 0xAF_0700;

        public const int C256F_MODEL_MAJOR =    0xAF_070B;
        public const int C256F_MODEL_MINOR =    0xAF_070C ;
        public const int FPGA_DOR =             0xAF_070D ;
        public const int FPGA_MOR =             0xAF_070E ;
        public const int FPGA_YOR =             0xAF_070F ;

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

        // Junior RTC
        public const int RTC_SEC_JR = 0x00_D690; // Seconds Register

        public const int SUPERIO_START = 0xAF_1000;
        public const int SUPERIO_END = 0xAF_13FF;
        public const int KBD_DATA_BUF_FMX = 0xAF_1060;     // FMX Keyboard input, output buffer
        public const int KBD_STATUS_PORT_FMX = 0xAF_1064;  // FMX keyboard status port

        public const int KBD_DATA_BUF_U = 0xAF_1803;       // U Keyboard input, output buffer
        public const int KBD_STATUS_PORT_U = 0xAF_1807;    // U keyboard status port

        public const int KBD_DATA_BUF_JR = 0x00_D640;       // JR Keyboard input, output buffer
        public const int KBD_STATUS_PORT_JR = 0x00_D644;    // JR keyboard status port

        // FDC  - $AF:13F0
        // LPT1 - $AF:1378
        public const int UART1_REGISTERS = 0xAF_13F8;
        public const int UART2_REGISTERS = 0xAF_12F8;
        public const int UART_REGISTERS_JR = 0x_D630;
        // KBD  - $AF:1060
        // GAME - $AF:1200 - Not Connected
        // MPU  - $AF:1330

        public const int MPU401_REGISTERS = 0xAF_1330;  // 2 bytes
        public const int MPU401_DATA_REG = 0xAF_1330;
        public const int MPU401_STATUS_REG = 0xAF_1331;

        public const int FG_CHAR_LUT_PTR = 0xAF_1F40; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR = 0xAF_1F80; // 15 color lookup table
        public const int FG_CHAR_LUT_PTR_JR = 0xD800; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR_JR = 0xD840; // 15 color lookup table

        public const int GRP_LUT_BASE_ADDR = 0xAF_2000;  // room for 8 LUTs at 1024 bytes each (256 * 4 bytes per colour)
        public const int GAMMA_BASE_ADDR = 0xAF_4000;    // each 256 byte for B, G, R
        public const int GAMMA_BASE_ADDR_JR = 0x00_C000;
        public const int GRP_LUT_BASE_ADDR_JR = 0xD000;

        public const int TILESET_BASE_ADDR = 0xAF_0280; // 8 tileset addresses, 4 bytes - 3 bytes address of tileset, 1 byte configuration
        public const int TILESET_BASE_ADDR_JR = 0xD280;

        public const int FONT0_MEMORY_BANK_START = 0xAF_8000;
        public const int FONT1_MEMORY_BANK_START = 0xAF_8800;
        public const int FONT_MEMORY_BANK_START_JR = 0xC000;

        public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE_JR = 0xC000;

        public const int REVOFPCB_C = 0xAF_E805;
        public const int REVOFPCB_4 = 0xAF_E806;
        public const int REVOFPCB_A = 0xAF_E807;
        public const int REVOFJR    = 0x00_D6A7;
        #endregion
    }
}
