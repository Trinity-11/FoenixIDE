.cpu "65816"

.include "macros_inc.asm"

 MARG1            = $005010

; Demonstrates issue with CMP direct-page-indirect-indexed-Y

                CLC
                XCE

                setdp <>MARG1
                setal

                LDA #$AA55        ; [$A00000] := $55, [$A00001] := $AA
                STA $0A0000

                LDA #$0000        ; [MARG1] := $A00000
                STA MARG1
                setas
                LDA #$0A
                STA MARG1+2

                setas
                LDY #0            ; Is [MARG1] == [MARG1]?
                LDA [MARG1],Y
                CMP [MARG1],Y
                BEQ IS_OK

                LDA #'N'          ; This shouldn't be executed
                BRK
                BRA WAIT

IS_OK           LDA #'Y'          ; This is the expected branch
                BRK

WAIT            JMP WAIT
