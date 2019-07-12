.cpu "65816"

.include "macros_inc.asm"

START
        CLC
        XCE
        setal
        lda #$1234
        xba
DONE    NOP
        BRA DONE
