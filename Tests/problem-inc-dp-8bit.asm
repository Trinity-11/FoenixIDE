.cpu "65816"
.include "macros_inc.asm"
z_L   = $001416
z_H   = $001417
z_HLU = $001418

* = 0
            clc
            xce
            
            setaxl
            LDA #$1400
            TCD
            .dpage $1400
            
            setas
            LDY #$FF
            LDX #$FFFF
LOOP
            JSR IncHLU
            DEX
            BNE LOOP
            DEY
            BNE LOOP
            
            WAI
            
IncHLU
            INC z_L
            BNE IncHL_Done
            INC z_H
            BNE IncHL_Done
            INC z_HLU
IncHL_Done
            RTS