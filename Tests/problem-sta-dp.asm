.cpu "65816"

.include "macros_inc.asm"

START
        CLC
        XCE
        setaxl
        LDA #$1234  ; set the direct page
        TCD
        
        LDA #$2233
        LDX #$7777
        STA $45,X
        
DONE    NOP
        BRA DONE
