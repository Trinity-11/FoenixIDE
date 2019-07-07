.cpu "65816"
.include "macros_inc.asm"
; This assembly code is to debug the problem with "post-indexing long" mode in the Foenix Emulator

MCMP_TEXT       = $C000
                CLC
                XCE
                setal
                LDA #$55AA        ; Store 55AA at 0A0000
                STA $0A0000

                setal
                LDA #$0000        ; MCMP_TEXT := 0A0000
                STA MCMP_TEXT
                LDA #$000A
                STA MCMP_TEXT+2

                setdp <>MCMP_TEXT
                setas
                setxl

                LDY #1
PROBLEM         LDA [MCMP_TEXT],Y ; Attempt to load character at 0A0001
                CMP #$55          ; It should be 55
                BNE not_match

                LDA #'Y'          ; So we should print Y
                JSL IPUTC

LOCK            JMP LOCK

not_match       LDA #'N'          ; Print N if it's not as expected
                JSL IPUTC
                BRA LOCK

IPUTC           NOP
                RTS

* = $C000
MYADDRESS       .long $0A0000
