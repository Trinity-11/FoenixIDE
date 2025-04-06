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
        public const int VKY_TXT_CURSOR_CTRL_F256_MMU = 0xD010;
        public const int VKY_TXT_CURSOR_CHAR_F256_MMU = 0xD012;
        public const int VKY_TXT_CURSOR_X_F256_MMU = 0xD014;
        public const int VKY_TXT_CURSOR_Y_F256_MMU = 0xD016;

        public const int VKY_TXT_CURSOR_CTRL_F256_FLAT = 0xF0_1010;
        public const int VKY_TXT_CURSOR_CHAR_F256_FLAT = 0xF0_1012;
        public const int VKY_TXT_CURSOR_X_F256_FLAT = 0xF0_1014;
        public const int VKY_TXT_CURSOR_Y_F256_FLAT = 0xF0_1016;

        // Line Interrupt Registers
        public const int VKY_LINE_IRQ_CTRL_REG =  0xAF001B; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE0_CMP_VALUE_LO = 0xAF001C; // Write Only[7:0]
        public const int VKY_LINE1_CMP_VALUE_LO = 0xAF001E; // Write Only[7:0]
        public const int VKY_LINE_IRQ_CTRL_F256_MMU = 0xD018; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE_CMP_VALUE_F256_MMU = 0xD019; // Write Only[7:0]
        public const int VKY_LINE_IRQ_CTRL_F256_FLAT = 0xF0_1018; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE_CMP_VALUE_F256_FLAT = 0xF0_1019; // Write Only[7:0]


        public const int BITMAP_CONTROL_REGISTER_ADDR = 0xAF_0100; // 2 layers - 8 bytes
        public const int BITMAP_CONTROL_REGISTER_ADDR_F256_MMU = 0xD100;
        public const int BITMAP_CONTROL_REGISTER_ADDR_F256_FLAT = 0xF0_1100;

        public const int TILE_CONTROL_REGISTER_ADDR = 0xAF_0200; // 12 bytes for each tile layer
        public const int TILE_CONTROL_REGISTER_ADDR_F256_MMU = 0xD200;
        public const int TILE_CONTROL_REGISTER_ADDR_F256_FLAT = 0xF0_1200;
        public const int SPRITE_CONTROL_REGISTER_ADDR = 0xAF_0C00; // 8 bytes for each sprite
        public const int SPRITE_CONTROL_REGISTER_ADDR_F256_MMU = 0xD900; 
        public const int SPRITE_CONTROL_REGISTER_ADDR_F256_FLAT = 0xF0_1900;

        public const int VDMA_START = 0xAF_0400;
        public const int VDMA_SIZE = 0x31; // from $af:0400 to $af:0430
        public const int DMA_START_F256_MMU = 0xDF00;
        public const int DMA_START_F256_FLAT = 0xF0_1F00;

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

        // IEC Register
        public const int IEC_START = 0xD680;

        // Mouse Pointer
        public const int MOUSE_POINTER_F256_MMU = 0xD6E0; 
        public const int MOUSE_POINTER_F256_FLAT = 0xF0_16E0;

        // Junior RTC
        public const int RTC_SEC_F256_MMU = 0x00_D690; // Seconds Register
        public const int RTC_SEC_F256_FLAT = 0xF0_1690; // Seconds Register

        public const int SUPERIO_START = 0xAF_1000;
        public const int SUPERIO_END = 0xAF_13FF;
        public const int KBD_DATA_BUF_FMX = 0xAF_1060;     // FMX Keyboard input, output buffer
        public const int KBD_STATUS_PORT_FMX = 0xAF_1064;  // FMX keyboard status port

        public const int KBD_DATA_BUF_U = 0xAF_1803;       // U Keyboard input, output buffer
        public const int KBD_STATUS_PORT_U = 0xAF_1807;    // U keyboard status port

        public const int KBD_DATA_BUF_F256_MMU = 0x00_D640;       // JR Keyboard input, output buffer
        public const int KBD_STATUS_PORT_F256_MMU = 0x00_D644;    // JR keyboard status port
        public const int KBD_DATA_BUF_F256_FLAT = 0xF0_1640;       // JR Keyboard input, output buffer
        public const int KBD_STATUS_PORT_F256_FLAT = 0xF0_1644;    // JR keyboard status port

        // FDC  - $AF:13F0
        // LPT1 - $AF:1378
        public const int UART1_REGISTERS = 0xAF_13F8;
        public const int UART2_REGISTERS = 0xAF_12F8;
        public const int UART_REGISTERS_F256_MMU = 0x_D630;
        public const int UART_REGISTERS_F256_FLAT = 0xF0_1630;

        // KBD  - $AF:1060
        // GAME - $AF:1200 - Not Connected
        // MPU  - $AF:1330

        public const int MPU401_REGISTERS = 0xAF_1330;  // 2 bytes
        public const int MPU401_DATA_REG = 0xAF_1330;
        public const int MPU401_STATUS_REG = 0xAF_1331;

        public const int FG_CHAR_LUT_PTR = 0xAF_1F40; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR = 0xAF_1F80; // 15 color lookup table
        public const int FG_CHAR_LUT_PTR_F256_MMU = 0xD800; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR_F256_MMU = 0xD840; // 15 color lookup table
        public const int FG_CHAR_LUT_PTR_F256_FLAT = 0xF0_1800; // 15 color lookup table
        public const int BG_CHAR_LUT_PTR_F256_FLAT = 0xF0_1840; // 15 color lookup table

        public const int GRP_LUT_BASE_ADDR = 0xAF_2000;  // room for 8 LUTs at 1024 bytes each (256 * 4 bytes per colour)
        public const int GAMMA_BASE_ADDR = 0xAF_4000;    // each 256 byte for B, G, R
        public const int GAMMA_BASE_ADDR_F256_MMU = 0x00_C000;
        public const int GAMMA_BASE_ADDR_F256_FLAT = 0xF0_0000;
        public const int GRP_LUT_BASE_ADDR_F256_MMU = 0xD000;
        public const int GRP_LUT_BASE_ADDR_F256_FLAT = 0xF0_3000;

        public const int TILESET_BASE_ADDR = 0xAF_0280; // 8 tileset addresses, 4 bytes - 3 bytes address of tileset, 1 byte configuration
        public const int TILESET_BASE_ADDR_F256_MMU = 0xD280;
        public const int TILESET_BASE_ADDR_F256_FLAT = 0xF0_1280;

        public const int FONT0_MEMORY_BANK_START = 0xAF_8000;
        public const int FONT1_MEMORY_BANK_START = 0xAF_8800;
        public const int FONT_MEMORY_BANK_START_F256_MMU = 0xC000;
        public const int FONT_MEMORY_BANK_START_F256_FLAT = 0xF0_2000;

        public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE_F256_MMU = 0xC000;
        public const int SCREEN_PAGE_TEXT_F256_FLAT = 0xF0_4000;
        public const int SCREEN_PAGE_COLOR_F256_FLAT = 0xF0_6000;

        public const int REVOFPCB_C = 0xAF_E805;
        public const int REVOFPCB_4 = 0xAF_E806;
        public const int REVOFPCB_A = 0xAF_E807;
        public const int REVOF_F256_MMU  = 0x00_D6A7;
        public const int REVOF_F256_FLAT = 0xF0_16A7;
        public static byte[] GAMMA_1_8 =
        {
            0x00, 0x0b, 0x11, 0x15, 0x19, 0x1c, 0x1f, 0x22, 0x25, 0x27, 0x2a, 0x2c, 0x2e, 0x30, 0x32, 0x34,
            0x36, 0x38, 0x3a, 0x3c, 0x3d, 0x3f, 0x41, 0x43, 0x44, 0x46, 0x47, 0x49, 0x4a, 0x4c, 0x4d, 0x4f,
            0x50, 0x51, 0x53, 0x54, 0x55, 0x57, 0x58, 0x59, 0x5b, 0x5c, 0x5d, 0x5e, 0x60, 0x61, 0x62, 0x63,
            0x64, 0x65, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75,
            0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x84,
            0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8e, 0x8f, 0x90, 0x91, 0x92, 0x93,
            0x94, 0x95, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f, 0x9f, 0xa0,
            0xa1, 0xa2, 0xa3, 0xa3, 0xa4, 0xa5, 0xa6, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xaa, 0xab, 0xac, 0xad,
            0xad, 0xae, 0xaf, 0xb0, 0xb0, 0xb1, 0xb2, 0xb3, 0xb3, 0xb4, 0xb5, 0xb6, 0xb6, 0xb7, 0xb8, 0xb8,
            0xb9, 0xba, 0xbb, 0xbb, 0xbc, 0xbd, 0xbd, 0xbe, 0xbf, 0xbf, 0xc0, 0xc1, 0xc2, 0xc2, 0xc3, 0xc4,
            0xc4, 0xc5, 0xc6, 0xc6, 0xc7, 0xc8, 0xc8, 0xc9, 0xca, 0xca, 0xcb, 0xcc, 0xcc, 0xcd, 0xce, 0xce,
            0xcf, 0xd0, 0xd0, 0xd1, 0xd2, 0xd2, 0xd3, 0xd4, 0xd4, 0xd5, 0xd6, 0xd6, 0xd7, 0xd7, 0xd8, 0xd9,
            0xd9, 0xda, 0xdb, 0xdb, 0xdc, 0xdc, 0xdd, 0xde, 0xde, 0xdf, 0xe0, 0xe0, 0xe1, 0xe1, 0xe2, 0xe3,
            0xe3, 0xe4, 0xe4, 0xe5, 0xe6, 0xe6, 0xe7, 0xe7, 0xe8, 0xe9, 0xe9, 0xea, 0xea, 0xeb, 0xec, 0xec,
            0xed, 0xed, 0xee, 0xef, 0xef, 0xf0, 0xf0, 0xf1, 0xf1, 0xf2, 0xf3, 0xf3, 0xf4, 0xf4, 0xf5, 0xf5,
            0xf6, 0xf7, 0xf7, 0xf8, 0xf8, 0xf9, 0xf9, 0xfa, 0xfb, 0xfb, 0xfc, 0xfc, 0xfd, 0xfd, 0xfe, 0xff
        };
        #endregion
    }
}
