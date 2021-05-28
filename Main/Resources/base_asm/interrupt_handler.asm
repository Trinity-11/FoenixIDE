;////////////////////////////////////////////////////////////////////////////
;////////////////////////////////////////////////////////////////////////////
;////////////////////////////////////////////////////////////////////////////
; Interrupt Handler
;////////////////////////////////////////////////////////////////////////////
;////////////////////////////////////////////////////////////////////////////
;////////////////////////////////////////////////////////////////////////////

check_irq_bit  .macro
                LDA \1
                AND #\2
                CMP #\2
                BNE END_CHECK
                STA \1
                JSR \3
                
END_CHECK
                .endm
                
IRQ_HANDLER
; First Block of 8 Interrupts
                .as
                setdp 0
                
                .as
                LDA #0  ; set the data bank register to 0
                PHA
                PLB
                setas

                LDA INT_PENDING_REG0
                BEQ CHECK_PENDING_REG1

; Start of Frame (display), timer 0 (music), mouse (ignored)
                check_irq_bit INT_PENDING_REG0, FNX0_INT00_SOF, SOF_INTERRUPT
                check_irq_bit INT_PENDING_REG0, FNX0_INT01_SOL, SOL_INTERRUPT
                check_irq_bit INT_PENDING_REG0, FNX0_INT02_TMR0, TIMER0_INTERRUPT
                check_irq_bit INT_PENDING_REG0, FNX0_INT03_TMR1, TIMER1_INTERRUPT
                check_irq_bit INT_PENDING_REG0, FNX0_INT04_TMR2, TIMER2_INTERRUPT
                check_irq_bit INT_PENDING_REG0, FNX0_INT07_MOUSE, MOUSE_INTERRUPT

; Second Block of 8 Interrupts
CHECK_PENDING_REG1
                setas
                
                LDA INT_PENDING_REG1
                BEQ CHECK_PENDING_REG2   ; BEQ EXIT_IRQ_HANDLE
; Keyboard Interrupt
                check_irq_bit INT_PENDING_REG1, FNX1_INT00_KBD, KEYBOARD_INTERRUPT
                check_irq_bit INT_PENDING_REG1, FNX1_INT01_SC0, STS_COLLISION_INTERRUPT
                check_irq_bit INT_PENDING_REG1, FNX1_INT02_SC1, STT_COLLISION_INTERRUPT

; Third Block of 8 Interrupts
CHECK_PENDING_REG2
                setas
                LDA INT_PENDING_REG2
                BEQ EXIT_IRQ_HANDLE
                
EXIT_IRQ_HANDLE
                
                RTL
SOF_INTERRUPT
                .as
.include "../_sof_handler.asm"
                RTS

SOL_INTERRUPT
                .as
.include "../_sol_handler.asm"
                RTS

TIMER0_INTERRUPT
                .as
.include "../_timer0_handler.asm"
                RTS

TIMER1_INTERRUPT
                .as
.include "../_timer1_handler.asm"
                RTS

TIMER2_INTERRUPT
                .as
.include "../_timer2_handler.asm"
                RTS

MOUSE_INTERRUPT
                .as
.include "../_mouse_handler.asm"
                RTS

KEYBOARD_INTERRUPT
                .as
.include "../_keyboard_handler.asm"
                RTS

STS_COLLISION_INTERRUPT
                .as
.include "../_collision0_handler.asm"
                RTS

STT_COLLISION_INTERRUPT
                .as
.include "../_collision1_handler.asm"
                RTS