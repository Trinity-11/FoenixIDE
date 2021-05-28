; DMA Controller $AF0400 - $AF04FF
VDMA_CONTROL_REG        = $AF0400
; Bit Field Definition
VDMA_CTRL_Enable        = $01
VDMA_CTRL_1D_2D         = $02       ; 0 - 1D (Linear) Transfer , 1 - 2D (Block) Transfer
VDMA_CTRL_TRF_Fill      = $04       ; 0 - Transfer Src -> Dst, 1 - Fill Destination with "Byte2Write"
VDMA_CTRL_Int_Enable    = $08       ; Set to 1 to Enable the Generation of Interrupt when the Transfer is over.
VDMA_CTRL_SysRAM_Src    = $10       ; Set to 1 to Indicate that the Source is the System Ram Memory
VDMA_CTRL_SysRAM_Dst    = $20       ; Set to 1 to Indicate that the Destination is the System Ram Memory
VDMA_CTRL_Start_TRF     = $80       ; Set to 1 To Begin Process, Need to Cleared before, you can start another

VDMA_XFER_VRAM2VRAM = $00           ; VRAM -> VRAM transfer
VDMA_XFER_SRAM2VRAM = $01           ; SRAM -> VRAM transfer
VDMA_XFER_VRAM2SRAM = $02           ; VRAM -> SRAM transfer

VDMA_BYTE_2_WRITE       = $AF0401   ; Write Only - Byte to Write in the Fill Function

VDMA_STATUS_REG         = $AF0401   ; Read only
;Status Bit Field Definition
VDMA_STAT_Size_Err      = $01       ; If Set to 1, Overall Size is Invalid
VDMA_STAT_Dst_Add_Err   = $02       ; If Set to 1, Destination Address Invalid
VDMA_STAT_Src_Add_Err   = $04       ; If Set to 1, Source Address Invalid
VDMA_STAT_VDMA_IPS      = $80       ; If Set to 1, VDMA Transfer in Progress (this Inhibit CPU Access to Mem)

                                    ; Let me repeat, don't Access the Video Memory then there is a VDMA in progress!

VDMA_SRC_ADDY_L         = $AF0402   ; Pointer to the Source of the Data to be stransfered
VDMA_SRC_ADDY_M         = $AF0403   ; This needs to be within Vicky's Range ($00_0000 - $3F_0000)
VDMA_SRC_ADDY_H         = $AF0404

VDMA_DST_ADDY_L         = $AF0405   ; Destination Pointer within Vicky's video memory Range
VDMA_DST_ADDY_M         = $AF0406   ; ($00_0000 - $3F_0000)
VDMA_DST_ADDY_H         = $AF0407

; In 1D Transfer Mode
VDMA_SIZE_L             = $AF0408   ; Maximum Value: $40:0000 (4Megs)
VDMA_SIZE_M             = $AF0409
VDMA_SIZE_H             = $AF040A
VDMA_IGNORED            = $AF040B

; In 2D Transfer Mode
VDMA_X_SIZE_L           = $AF0408   ; Maximum Value: 65535
VDMA_X_SIZE_H           = $AF0409
VDMA_Y_SIZE_L           = $AF040A   ; Maximum Value: 65535
VDMA_Y_SIZE_H           = $AF040B

VDMA_SRC_STRIDE_L       = $AF040C   ; Always use an Even Number ( The Engine uses Even Ver of that value)
VDMA_SRC_STRIDE_H       = $AF040D   ;
VDMA_DST_STRIDE_L       = $AF040E   ; Always use an Even Number ( The Engine uses Even Ver of that value)
VDMA_DST_STRIDE_H       = $AF040F   ;
