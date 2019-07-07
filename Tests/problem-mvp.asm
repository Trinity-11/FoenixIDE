.cpu "65816"
.include "macros_inc.asm"

; This assembly code is to debug the problem with "MVN and MVP"


                CLC
                XCE
                setaxl     ; set everybody to 16-bites

                LDA #$0    ; this should get one byte moved
                LDX #$557  ; bottom address of move
                LDY #$700  ; bottom address of move

                MVP #$0, #$0

                CMP #$FFFF  ; Check that A is $FFFF
                BNE fail

                TXA         ; check that X if $556
                CMP #$556
                BNE fail
                TYA
                CMP #$6FF   ; check that Y is $701
                BNE fail

                LDA $700
                CMP #'P'
                BNE fail

success         LDA #$1
                NOP
                BRA success


fail            LDA #$0
                NOP
                BRA fail

* = $555
                  .text 'MVP'
