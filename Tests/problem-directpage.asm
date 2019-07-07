.cpu "65816"
.include "macros_inc.asm"

; This assembly code is to debug the problem with "PEA, PEI and PER" mode in the Foenix Emulator

MCMDADDR = $000250          ; Just a random monitor variable
MCMP_TEXT = $000253         ; A place to write success/fail value

                CLC
                XCE
                setaxl

                setdp <>MCMDADDR
                LDA #$55AA
                STA MCMDADDR

                LDA #0

                LDA @lMCMDADDR
                CMP #$55AA
                BNE fail

                LDA #1              ; Signal success with A = 1
                BRA done

fail            LDA #0              ; Signal failuer with A = 0

done            STA @lMCMP_TEXT     ; Write success or failure

endlessloop     NOP
                BRA endlessloop
