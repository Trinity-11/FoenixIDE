.cpu "65816"
.include "macros_inc.asm"

MCMDADDR = $000250          ; Just a random monitor variable
MCMP_TEXT = $000253         ; A place to write success/fail value
STACK_END = $00FEFF

; This assembly code is to test if direct page is working.

* = $001000

START           CLC
                XCE

                setal
                LDA #STACK_END          ; Point the stack to $00FEFF
                TCS

                PEA $1234
                PLA                     ; A should be $1234
                CMP #$1234
                BNE failure

                ; load the direct page register with MCMDADDR $000250
                .dpage MCMDADDR
                LDA #MCMDADDR
                TCD

                LDA #$5678              ; Save $5678 at $000253
                STA @lMCMP_TEXT
                
                PEI $03,d               ; push the address in $000253
                LDA #0
                PLA                     ; A should be $5678
                CMP #$5678
                BNE failure

                ; Test PER
                PER TARGET
                PLA                     ; A should be the lower 16-bits of address of TARGET
                CMP #<>TARGET
                BNE failure

success         LDA #1
                BRA done

failure         LDA #0

done            STA @lMCMDADDR

endlessloop     NOP
                BRA endlessloop

TARGET          NOP

* = $FFFC
                .word $1000
