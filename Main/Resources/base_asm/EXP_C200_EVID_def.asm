EVID_EXP_CARD_INFO      = $AE0000    ; Read Only (32 Bytes Card ID - READ ONLY)
EVID_ID_NAME_ASCII      = $AE0000    ; 15 Characters + $00
EVID_ID_VENDOR_ID_Lo    = $AE0010    ; Foenix Project Reserved ID: $F0E1
EVID_ID_VENDOR_ID_Hi    = $AE0011
EVID_ID_CARD_ID_Lo      = $AE0012      ; $9236 - C200-EVID 
EVID_ID_CARD_ID_Hi      = $AE0013
EVID_ID_CARD_CLASS_Lo   = $AE0014    ; TBD
EVID_ID_CARD_CLASS_Hi   = $AE0015    ; TBD
EVID_ID_CARD_SUBCLSS_Lo = $AE0016    ; TBD
EVID_ID_CARD_SUBCLSS_Hi = $AE0017    ; TBD
EVID_ID_CARD_UNDEFINED0 = $AE0018    ; TBD
EVID_ID_CARD_UNDEFINED1 = $AE0019    ; TBD
EVID_ID_CARD_HW_Rev     = $AE001A    ; 00 - in Hex
EVID_ID_CARD_FPGA_Rev   = $AE001B    ; 00 - in Hex
EVID_ID_CARD_UNDEFINED2 = $AE001C    ; TBD
EVID_ID_CARD_UNDEFINED3 = $AE001D    ; TBD
EVID_ID_CARD_CHKSUM0    = $AE001E    ; Not Supported Yet
EVID_ID_CARD_CHKSUM1    = $AE001F    ; Not Supported Yet

;EVID_MOUSEPTR_MEM       = $AE0400    ; Not Instantiated
;EVID_MOUSEPTR_REG       = $AE0600    ; Not Instantiated

EVID_FONT_MEM      = $AE1000
EVID_FG_LUT        = $AE1B00
EVID_BG_LUT        = $AE1B40
;Internal EVID Registers and Internal Memory Locations (LUTs)
EVID_MSTR_CTRL_REG_L	= $AE1E00
EVID_Ctrl_Text_Mode_En  = $01       ; Enable the Text Mode

EVID_MSTR_CTRL_REG_H    = $AE1E01
EVID_800x600ModeEnable   = $01       ; 0 - 640x480 (Clock @ 25.175Mhz), 1 - 800x600 (Clock @ 40Mhz)

EVID_Border_Ctrl_Enable = $01
EVID_BORDER_CTRL_REG    = $AE1E04 ; Bit[0] - Enable (1 by default)  Bit[4..6]: X Scroll Offset ( Will scroll Left) (Acceptable Value: 0..7)
EVID_BORDER_COLOR_B     = $AE1E05
EVID_BORDER_COLOR_G     = $AE1E06
EVID_BORDER_COLOR_R     = $AE1E07
EVID_BORDER_X_SIZE      = $AE1E08; X-  Values: 0 - 32 (Default: 32)
EVID_BORDER_Y_SIZE      = $AE1E09; Y- Values 0 -32 (Default: 32)

EVID_TXT_CURSOR_CTRL_REG = $AE1E10   ;[0]  Enable Text Mode
EVID_Cursor_Enable       = $01
EVID_Cursor_Flash_Rate0  = $02       ; 00 - 1/Sec, 01 - 2/Sec, 10 - 4/Sec, 11 - 5/Sec
EVID_Cursor_Flash_Rate1  = $04
EVID_Cursor_FONT_Page0   = $08       ; Pick Font Page 0 or Font Page 1
EVID_Cursor_FONT_Page1   = $10       ; Pick Font Page 0 or Font Page 1
EVID_TXT_CURSOR_CHAR_REG = $AE1E12

EVID_TXT_CURSOR_COLR_REG = $AE1E13
EVID_TXT_CURSOR_X_REG_L  = $AE1E14
EVID_TXT_CURSOR_X_REG_H  = $AE1E15
EVID_TXT_CURSOR_Y_REG_L  = $AE1E16
EVID_TXT_CURSOR_Y_REG_H  = $AE1E17
; When you Read the Register
EVID_INFO_CHIP_NUM_L     = $AE1E1C
EVID_INFO_CHIP_NUM_H     = $AE1E1D
EVID_INFO_CHIP_VER_L     = $AE1E1E
EVID_INFO_CHIP_VER_H     = $AE1E1F

EVID_TEXT_MEM            = $AE2000
EVID_COLOR_MEM           = $AE4000
EVID_ETHERNET_REG        = $AEE000

