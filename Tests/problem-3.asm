.cpu "65816"
.include "macros_inc.asm"
; This assembly code is to debug the problem with long indirect indexed compare
IND_ADDR        = $5280
DP_LNG_ADDR     = $0A3344

                CLC
                XCE
; store a test value for comparison
                setal
                LDA #$1234
                STA DP_LNG_ADDR + $42

; store the array base address in memory
                LDA #$3344
                STA IND_ADDR ;
                LDA #$000A
                STA IND_ADDR + $2

                setdp $5200
; compare with the long accumulator
                LDA #$1234
                LDY #$42
                CMP [IND_ADDR],Y
                BEQ PASS_LONG
                LDA #$FFFF
                BRK

; compare with a short accumulator
PASS_LONG       setas
                LDA #$12
                LDY #$43
                CMP [IND_ADDR],Y
                BEQ PASS_SHORT
                LDA #$FF
                BRK

PASS_SHORT      LDA #$34
                LDY #$42
                CMP [IND_ADDR],Y
                BEQ FINAL
                LDA #$FF
                BRK

FINAL           LDA #$11
                BRK
