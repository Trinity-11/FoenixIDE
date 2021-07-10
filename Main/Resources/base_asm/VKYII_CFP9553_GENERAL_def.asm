;Internal VICKY Registers and Internal Memory Locations (LUTs)
MASTER_CTRL_REG_L	      = $AF0000
;Control Bits Fields
Mstr_Ctrl_Text_Mode_En  = $01       ; Enable the Text Mode
Mstr_Ctrl_Text_Overlay  = $02       ; Enable the Overlay of the text mode on top of Graphic Mode (the Background Color is ignored)
Mstr_Ctrl_Graph_Mode_En = $04       ; Enable the Graphic Mode
Mstr_Ctrl_Bitmap_En     = $08       ; Enable the Bitmap Module In Vicky
Mstr_Ctrl_TileMap_En    = $10       ; Enable the Tile Module in Vicky
Mstr_Ctrl_Sprite_En     = $20       ; Enable the Sprite Module in Vicky
Mstr_Ctrl_GAMMA_En      = $40       ; this Enable the GAMMA correction - The Analog and DVI have different color value, the GAMMA is great to correct the difference
Mstr_Ctrl_Disable_Vid   = $80       ; This will disable the Scanning of the Video hence giving 100% bandwith to the CPU

MASTER_CTRL_REG_H       = $AF0001
Mstr_Ctrl_Video_Mode0   = $01       ; 0 - 640x480 (Clock @ 25.175Mhz), 1 - 800x600 (Clock @ 40Mhz)
Mstr_Ctrl_Video_Mode1   = $02       ; 0 - No Pixel Doubling, 1- Pixel Doubling (Reduce the Pixel Resolution by 2)

; New - GAMMA Control/HI-Res DipSwitch Value;
GAMMA_CTRL_REG          = $AF0002
GAMMA_Ctrl_Input        = $01 ; 0 = DipSwitch Chooses GAMMA on/off , 1- Software Control
GAMMA_Ctrl_Soft         = $02 ; 0 = GAMMA Table is not Applied, 1 = GAMMA Table is Applied
SYNC_Turn_Off           = $04 ; 0 = Normal Operation, 1 = Turn Off Sync to get sleep mode from Monitor
GAMMA_DP_SW_VAL         = $08 ; READ ONLY - Actual DIP Switch Value
HIRES_DP_SW_VAL         = $10 ; READ ONLY - 0 = Hi-Res on BOOT ON, 1 = Hi-Res on BOOT OFF



VKY_RESERVED_01         = $AF0003

Border_Ctrl_Enable      = $01
BORDER_CTRL_REG         = $AF0004 ; Bit[0] - Enable (1 by default)  Bit[4..6]: X Scroll Offset ( Will scroll Left) (Acceptable Value: 0..7)
BORDER_COLOR_B          = $AF0005
BORDER_COLOR_G          = $AF0006
BORDER_COLOR_R          = $AF0007
BORDER_X_SIZE           = $AF0008; X-  Values: 0 - 32 (Default: 32)
BORDER_Y_SIZE           = $AF0009; Y- Values 0 -32 (Default: 32)

; AF000A
; AF000B
; AF000C

BACKGROUND_COLOR_B      = $AF000D ; When in Graphic Mode, if a pixel is "0" then the Background pixel is chosen
BACKGROUND_COLOR_G      = $AF000E
BACKGROUND_COLOR_R      = $AF000F ;

VKY_TXT_CURSOR_CTRL_REG = $AF0010   ;[0]  Enable Text Mode
Vky_Cursor_Enable       = $01
Vky_Cursor_Flash_Rate0  = $02       ; 00 - 1/Sec, 01 - 2/Sec, 10 - 4/Sec, 11 - 5/Sec
Vky_Cursor_Flash_Rate1  = $04
Vky_Cursor_FONT_Page0   = $08       ; Pick Font Page 0 or Font Page 1
Vky_Cursor_FONT_Page1   = $10       ; Pick Font Page 0 or Font Page 1
VKY_TXT_START_ADD_PTR   = $AF0011   ; This is an offset to change the Starting address of the Text Mode Buffer (in x)
VKY_TXT_CURSOR_CHAR_REG = $AF0012

VKY_TXT_CURSOR_COLR_REG = $AF0013
VKY_TXT_CURSOR_X_REG_L  = $AF0014
VKY_TXT_CURSOR_X_REG_H  = $AF0015
VKY_TXT_CURSOR_Y_REG_L  = $AF0016
VKY_TXT_CURSOR_Y_REG_H  = $AF0017

; AF0018
; AF0019
; AF001A

; Line Interrupt Registers
VKY_LINE_IRQ_CTRL_REG   = $AF001B ;[0] - Enable Line 0, [1] -Enable Line 1
VKY_LINE0_CMP_VALUE_LO  = $AF001C ;Write Only [7:0]
VKY_LINE0_CMP_VALUE_HI  = $AF001D ;Write Only [3:0]
VKY_LINE1_CMP_VALUE_LO  = $AF001E ;Write Only [7:0]
VKY_LINE1_CMP_VALUE_HI  = $AF001F ;Write Only [3:0]

; When you Read the Register
VKY_INFO_CHIP_NUM_L     = $AF001C   ; Version
VKY_INFO_CHIP_NUM_H     = $AF001D   ; Version
VKY_INFO_CHIP_VER_L     = $AF001E   ; Sub-Version
VKY_INFO_CHIP_VER_H     = $AF001F   ; Sub-Version

; Mouse Pointer Graphic Memory
MOUSE_PTR_GRAP0_START    = $AF0500 ; 16 x 16 = 256 Pixels (Grey Scale) 0 = Transparent, 1 = Black , 255 = White
MOUSE_PTR_GRAP0_END      = $AF05FF ; Pointer 0
MOUSE_PTR_GRAP1_START    = $AF0600 ;
MOUSE_PTR_GRAP1_END      = $AF06FF ; Pointer 1

MOUSE_PTR_CTRL_REG_L    = $AF0700 ; Bit[0] Enable, Bit[1] = 0  ( 0 = Pointer0, 1 = Pointer1)
MOUSE_PTR_CTRL_REG_H    = $AF0701 ;
MOUSE_PTR_X_POS_L       = $AF0702 ; X Position (0 - 639) (Can only read now) Writing will have no effect
MOUSE_PTR_X_POS_H       = $AF0703 ;
MOUSE_PTR_Y_POS_L       = $AF0704 ; Y Position (0 - 479) (Can only read now) Writing will have no effect
MOUSE_PTR_Y_POS_H       = $AF0705 ;
MOUSE_PTR_BYTE0         = $AF0706 ; Byte 0 of Mouse Packet (you must write 3 Bytes)
MOUSE_PTR_BYTE1         = $AF0707 ; Byte 1 of Mouse Packet (if you don't, then )
MOUSE_PTR_BYTE2         = $AF0708 ; Byte 2 of Mouse Packet (state Machine will be jammed in 1 state)
                                  ; (And the mouse won't work)
C256F_MODEL_MAJOR       = $AF070B ;
C256F_MODEL_MINOR       = $AF070C ;
FPGA_DOR                = $AF070D ;
FPGA_MOR                = $AF070E ;
FPGA_YOR                = $AF070F ;
;                       = $AF0800 ; the RTC is Here

; VMEM 2 CPU Read Registers
VMEM2CPU_CTRL_REG      = $AF0900  ; 
VMEM2CPU_Clear_FIFO    = $01        ; Setting this to 1 will flush (Reset) the FIFO, clear it after to return to normal function
                                    ; Probably a good idea to reset this before starting to fetch data from VMEM in your application
VMEM2CPU_Intterupt     = $02        ; This is not implemented because there is not enough Interrupt line on the FMX 
                                    ; It could be implemented in the U... Right now it is not plugged anywere.
VMEM2CPU_Data_Port     = $AF0901    ; This is a Byte Port of the data that you have requested. (Output of the FIFO)
VMEM2CPU_Fifo_Count_LO = $AF0902    ; FIFO Count Lo (number of byte in the FIFO) FIFO Count is 10bits, Count_Bit0 to Count_Bit7 of Count
VMEM2CPU_Fifo_Count_HI = $AF0903    ; FIFO Count Hi, Bit 0 = Count_Bit8, Bit 1 = Count_Bit9
VMEM2CPU_Fifo_Empty    = $80        ; Bit 7 of VMEM2CPU_Fifo_Count_HI, when 1, the FIFO is empty.


;                       = $AF1000 ; The SuperIO Start is Here
;                       = $AF13FF ; The SuperIO Start is Here

FG_CHAR_LUT_PTR         = $AF1F40
BG_CHAR_LUT_PTR		    = $AF1F80

GRPH_LUT0_PTR		    = $AF2000
GRPH_LUT1_PTR		    = $AF2400
GRPH_LUT2_PTR		    = $AF2800
GRPH_LUT3_PTR		    = $AF2C00
GRPH_LUT4_PTR		    = $AF3000
GRPH_LUT5_PTR		    = $AF3400
GRPH_LUT6_PTR		    = $AF3800
GRPH_LUT7_PTR		    = $AF3C00

GAMMA_B_LUT_PTR		    = $AF4000
GAMMA_G_LUT_PTR		    = $AF4100
GAMMA_R_LUT_PTR		    = $AF4200

FONT_MEMORY_BANK0       = $AF8000     ;$AF8000 - $AF87FF
CS_TEXT_MEM_PTR         = $AFA000
CS_COLOR_MEM_PTR        = $AFC000
