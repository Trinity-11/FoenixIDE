.cpu "65816"
.include "macros_inc.asm"
    CLC
    XCE
    setaxl
    LDA #$234
    TCS
    LDA #$123
    STA $237
    LDA #$678
    STA $239
    
    LDA #$FE23
    LDY #$10
    STA (3,s),Y ; writes $23 at $133 and $FE at $134
    BRK