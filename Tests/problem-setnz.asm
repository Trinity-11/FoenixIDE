.cpu "65816"
.include "macros_inc.asm"

* = 0
            clc
            xce
            setaxl
            lda #%1000_0000_0000_0000        ; N = 0, should be 1
            ldx #%1000_0000_0000_0000        ; N = 0, should be 1
            ldy #%1000_0000_0000_0000        ; same as above
            
            WAI