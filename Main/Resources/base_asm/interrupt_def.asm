; Pending Interrupt (Read and Write Back to Clear)
INT_PENDING_REG0 = $000140 ;
INT_PENDING_REG1 = $000141 ;
INT_PENDING_REG2 = $000142 ;
INT_PENDING_REG3 = $000143 ; FMX Model

; Polarity Set
INT_POL_REG0     = $000144 ;
INT_POL_REG1     = $000145 ;
INT_POL_REG2     = $000146 ;
INT_POL_REG3     = $000147 ; FMX Model

; Edge Detection Enable
INT_EDGE_REG0    = $000148 ;
INT_EDGE_REG1    = $000149 ;
INT_EDGE_REG2    = $00014A ;
INT_EDGE_REG3    = $00014B ; FMX Model

; Mask
INT_MASK_REG0    = $00014C ;
INT_MASK_REG1    = $00014D ;
INT_MASK_REG2    = $00014E ;
INT_MASK_REG3    = $00014F ; FMX Model

; Interrupt Bit Definition
; Register Block 0
FNX0_INT00_SOF    = $01  ;Start of Frame @ 60FPS
FNX0_INT01_SOL    = $02  ;Start of Line (Programmable)
FNX0_INT02_TMR0   = $04  ;Timer 0 Interrupt
FNX0_INT03_TMR1   = $08  ;Timer 1 Interrupt
FNX0_INT04_TMR2   = $10  ;Timer 2 Interrupt
FNX0_INT05_RTC    = $20  ;Real-Time Clock Interrupt
FNX0_INT06_FDC    = $40  ;Floppy Disk Controller
FNX0_INT07_MOUSE  = $80  ; Mouse Interrupt (INT12 in SuperIO IOspace)

; Register Block 1
FNX1_INT00_KBD    = $01  ;Keyboard Interrupt
FNX1_INT01_SC0        = $02  ;VICKY_II (INT2) Sprite 2 Sprite Collision
FNX1_INT02_SC1        = $04  ;VICKY_II (INT3) Sprite 2 Tiles Collision
FNX1_INT03_COM2   = $08  ;Serial Port 2
FNX1_INT04_COM1   = $10  ;Serial Port 1
FNX1_INT05_MPU401 = $20  ;Midi Controller Interrupt
FNX1_INT06_LPT    = $40  ;Parallel Port
FNX1_INT07_SDCARD     = $80  ;SD Card Controller Interrupt (CH376S)

; Register Block 2
FNX2_INT00_OPL3       = $01  ;OPl3
FNX2_INT01_GABE_INT0  = $02  ;GABE (INT0) - TBD
FNX2_INT02_GABE_INT1  = $04  ;GABE (INT1) - TBD
FNX2_INT03_SDMA       = $08  ;VICKY_II (INT4)
FNX2_INT04_VDMA       = $10  ;VICKY_II (INT5)
FNX2_INT05_GABE_INT2  = $20  ;GABE (INT2) - TBD
FNX2_INT06_EXT    = $40  ;External Expansion
FNX2_INT07_SDCARD_INS = $80  ; SDCARD Insertion

; Register Block 3 (FMX Expansion)
FNX3_INT00_OPN2       = $01  ;OPN2
FNX3_INT01_OPM        = $02  ;OPM
FNX3_INT02_IDE        = $04  ;HDD IDE INTERRUPT
FNX3_INT03_TBD        = $08  ;TBD
FNX3_INT04_TBD        = $10  ;TBD
FNX3_INT05_TBD        = $20  ;GABE (INT2) - TBD
FNX3_INT06_TBD        = $40  ;External Expansion
FNX3_INT07_TBD        = $80  ; SDCARD Insertion
