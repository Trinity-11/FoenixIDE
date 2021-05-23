; SDMA Controller $AF0420 - $AF042F
SDMA_CTRL_REG0           = $AF0420
; Bit Field Definition
SDMA_CTRL0_Enable        = $01
SDMA_CTRL0_1D_2D         = $02     ; 0 - 1D (Linear) Transfer , 1 - 2D (Block) Transfer
SDMA_CTRL0_TRF_Fill      = $04     ; 0 - Transfer Src -> Dst, 1 - Fill Destination with "Byte2Write"
SDMA_CTRL0_Int_Enable    = $08     ; Set to 1 to Enable the Generation of Interrupt when the Transfer is over.
SDMA_CTRL0_SysRAM_Src    = $10     ; Set to 1 to Indicate that the Source is the System Ram Memory
SDMA_CTRL0_SysRAM_Dst    = $20     ; Set to 1 to Indicate that the Destination is the System Ram Memory

; SDMA_CONTROL_REG0[5:4]
;                   00: SRAM to SRAM Transfer
;                   01: SRAM to VRAM Transfer (SDMA & VDMA needs to be initialized for transfer to work)
;                   10: VRAM to SRAM Transfer (SDMA & VDMA needs to be initialized for transfer to work)
;                   11: IO Transfer from or to SRAM (refer to SDMA_CONTROL_REG1 for config and direction )

SDMA_XFER_SRAM2SRAM = $00           ; SRAM -> SRAM transfer
SDMA_XFER_SRAM2VRAM = $10           ; SRAM -> VRAM transfer
SDMA_XFER_VRAM2SRAM = $20           ; VRAM -> SRAM transfer
SDMA_XFER_SRAMIO = $30              ; SRAM <-> IO transfer (see: SDMA_CONTROL_REG1 for config and direction)

SDMA_CTLR0_RSVD          = $40      ; Reserved
SDMA_CTRL0_Start_TRF     = $80      ; Set to 1 To Begin Process, Need to Cleared before, you can start another

; THIS SECTION HAS NOT BEEN IMPLEMENTED YET, SO IGNORE FOR NOW
; Control Register to manage the IO Transfer from and to SRAM
SDMA_CTRL_REG1           = $AF0421  ; Write Only - Byte to Write in the Fill Function
SDMA_CTRL1_IO_Src		 = $01		; 1 = Source is an IO Address (ADC, SuperIO, IDE)
SDMA_CTRL1_IO_Src16		 = $02		; 0 = Src 8Bits Transfer / 1= 16Bits Transfer
SDMA_CTRL1_IO_Dst		 = $04		; 1 = Destination is an IO Address (DAC, SuperIO, IDE)
SDMA_CTRL1_IO_Dst16      = $08      ; 0 = Dst 8bits Transfer / 1= 16bits

                                    ; Let me repeat, don't Access the Video Memory then there is a VDMA in progress!

SDMA_SRC_ADDY_L         = $AF0422   ; Pointer to the Source of the Data to be stransfered
SDMA_SRC_ADDY_M         = $AF0423   ; This needs to be within CPU's system RAM range ($00_0000 - $3F_FFFF)
SDMA_SRC_ADDY_H         = $AF0424

SDMA_DST_ADDY_L         = $AF0425   ; Destination Pointer within CPU's video memory Range
SDMA_DST_ADDY_M         = $AF0426   ; This needs to be within CPU's system RAM range ($00_0000 - $3F_FFFF)
SDMA_DST_ADDY_H         = $AF0427

; In 1D Transfer Mode
SDMA_SIZE_L             = $AF0428   ; Maximum Value: $40:0000 (4Megs)
SDMA_SIZE_M             = $AF0429
SDMA_SIZE_H             = $AF042A
SDMA_IGNORED            = $AF042B

; In 2D Transfer Mode
SDMA_X_SIZE_L           = $AF0428   ; Maximum Value: 65535
SDMA_X_SIZE_H           = $AF0429
SDMA_Y_SIZE_L           = $AF042A   ; Maximum Value: 65535
SDMA_Y_SIZE_H           = $AF042B

SDMA_SRC_STRIDE_L       = $AF042C   ; Always use an Even Number ( The Engine uses Even Ver of that value)
SDMA_SRC_STRIDE_H       = $AF042D   ;
SDMA_DST_STRIDE_L       = $AF042E   ; Always use an Even Number ( The Engine uses Even Ver of that value)
SDMA_DST_STRIDE_H       = $AF042F   ;

SDMA_BYTE_2_WRITE       = $AF0430   ; Write Only - Byte to Write in the Fill Function
SDMA_STATUS_REG         = $AF0430   ; Read only

;Status Bit Field Definition
SDMA_STAT_Size_Err       = $01      ; If Set to 1, Overall Size is Invalid
SDMA_STAT_Dst_Add_Err    = $02      ; If Set to 1, Destination Address Invalid
SDMA_STAT_Src_Add_Err    = $04      ; If Set to 1, Source Address Invalid
SDMA_STAT_TimeOut_Err    = $08      ; will be set to 1 if a Timeout occur when transfering between data from and to VRAM
