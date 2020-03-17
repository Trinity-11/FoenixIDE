.cpu "65816"
.include "macros_inc.asm"
    CLC
    XCE
    setaxl
    LDA #$800
    TCS
    LDA #$FE23
    LDY #$10
    STA (3,s),Y ; writes $23 at $234 and $FE at $235
    BRK