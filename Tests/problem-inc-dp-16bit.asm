.cpu "65816"
.include "macros_inc.asm"
z_L   = $001416
z_H   = $001418
z_HLU = $00141a

* = 0
            clc
            xce
            
            setaxl
            LDA #$1400
            TCD
            .dpage $1400
            
            LDY #$FFFF
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