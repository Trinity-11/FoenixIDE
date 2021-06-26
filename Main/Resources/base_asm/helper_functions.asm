; *************************************************************************
; * COPY data from SRAM to VRAM
; * The addresses at SRAM_ADDR, VRAM_ADDR, SRAM_SIZE and VRAM_SIZE must be set before calling this subroutine.
; *************************************************************************
COPY_TO_VRAM
        ;; This is the Source Address of the Data inside the SRAM Mem (this CPU offset)
        .as
        ; SDMA Master Control
        ; Source
        LDA #( SDMA_CTRL0_Enable | SDMA_CTRL0_SysRAM_Src )
        STA @l SDMA_CTRL_REG0
        ; VDMA Master Control
        ; Destination
        LDA #( VDMA_CTRL_Enable |  VDMA_CTRL_SysRAM_Src )
        STA @l VDMA_CONTROL_REG

        LDA #0
        STA @l SDMA_SIZE_H+1
        STA @l VDMA_SIZE_H+1

        ; Begin Transfer
        ; Start the VDMA Controller First
        LDA VDMA_CONTROL_REG
        ORA #VDMA_CTRL_Start_TRF
        STA @l VDMA_CONTROL_REG
        ; Then, Start the SDMA Controller Second (Since the SDMA will lock the CPU while it is doing its job)
        LDA SDMA_CTRL_REG0
        ORA #SDMA_CTRL0_Start_TRF
        STA @l SDMA_CTRL_REG0
        NOP ; When the transfer is started the CPU will be put on Hold (RDYn)...
        NOP ; Before it actually gets to stop it will execute a couple more instructions
        NOP ; From that point on, the CPU is halted (keep that in mind) No IRQ will be processed either during that time
        NOP
        NOP

COPY_TO_VRAM_LOOPA:
        LDA @l VDMA_STATUS_REG
        AND #$80
        CMP #$80  ; Check if bit $80 is cleared to indicate that the VDMA is done.
        BEQ COPY_TO_VRAM_LOOPA

        ; finish by clearing data
        LDA #0
        STA @l SDMA_CTRL_REG0
        STA @l VDMA_CONTROL_REG
        RTL


; *************************************************************************
; * COPY data from SRAM to SRAM
; * The addresses at SRAM__SRC_ADDR, SRAM__DEST_ADDR and SRAM_SIZE must be set before calling this subroutine.
; *************************************************************************
COPY_TO_SRAM
        ;; This is the Source Address of the Data inside the SRAM Mem (this CPU offset)
        .as
        ; SDMA Master Control
        ; Source
        LDA #( SDMA_CTRL0_Enable )
        STA @l SDMA_CTRL_REG0

        ; Begin Transfer
        LDA SDMA_CTRL_REG0
        ORA #SDMA_CTRL0_Start_TRF
        STA @l SDMA_CTRL_REG0
        NOP ; When the transfer is started the CPU will be put on Hold (RDYn)...
        NOP ; Before it actually gets to stop it will execute a couple more instructions
        NOP ; From that point on, the CPU is halted (keep that in mind) No IRQ will be processed either during that time
        NOP
        NOP
        ; finish by clearing data
        LDA #0
        STA @l SDMA_CTRL_REG0
        RTL


; *************************************************************************
; * FILL data with a single byte
; * The addresses at VDMA_BYTE_2_WRITE, VRAM_ADDR, SRAM_SIZE and VRAM_SIZE must be set before calling this subroutine.
; *************************************************************************
FILL_VRAM
        ;; This is the Source Address of the Data inside the SRAM Mem (this CPU offset)
        .as
        ; VDMA Master Control
        ; Destination
        LDA #( VDMA_CTRL_Enable |  VDMA_CTRL_TRF_Fill )
        STA @l VDMA_CONTROL_REG

        LDA #0
        STA @l VDMA_SIZE_H+1

        ; Begin Transfer
        ; Start the VDMA Controller First
        LDA VDMA_CONTROL_REG
        ORA #VDMA_CTRL_Start_TRF
        STA @l VDMA_CONTROL_REG
        
        NOP ; When the transfer is started the CPU will be put on Hold (RDYn)...
        NOP ; Before it actually gets to stop it will execute a couple more instructions
        NOP ; From that point on, the CPU is halted (keep that in mind) No IRQ will be processed either during that time
        NOP

FILL_VRAM_LOOPA:
        LDA @l VDMA_STATUS_REG
        AND #$80
        CMP #$80  ; Check if bit $80 is cleared to indicate that the VDMA is done.
        BEQ FILL_VRAM_LOOPA

        ; finish by clearing data
        LDA #0
        STA @l VDMA_CONTROL_REG
        RTL

LOG_EVID_MSG:
        ; get the current cursor position
        ; write the text
        ; move to next line
        ; if at last line, move the screen up one line
        RTL