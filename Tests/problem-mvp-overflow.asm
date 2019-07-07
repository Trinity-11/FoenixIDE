.cpu "65816"
.include "macros_inc.asm"

; This assembly code is to debug the problem with "MVN and MVP"
; Obviously the wrapping around the banks might yield strange results


                CLC
                XCE
                setaxl     ; set everybody to 16-bites

                LDA #$1000    ; this should get one byte moved
                LDX #$200  ; bottom address of move
                LDY #$300  ; bottom address of move

                MVP #$1, #$3  ; move $1001 bytes from $01:0200 to $03:0300
                              ; this should wrap around to the start of $010000 and $030000

                CMP #$FFFF  ; Check that A is $FFFF
                BNE fail

                TXA         ; check that X if $B01
                CMP #$F1FF
                BNE fail
                TYA
                CMP #$F2FF   ; check that Y is $99A
                BNE fail

                LDA $030300  ; test the boundaries
                CMP #$0011
                BNE fail

                LDA $0300FF
                CMP #$1122
                BNE fail

                LDA $040000
                CMP #$0000
                BNE fail

                LDA $02FFFF
                CMP #$2200
                BNE fail

                LDA $03FFFF
                CMP #$0022
                BNE fail

                LDA $03FC00
                CMP #$2222
                BNE fail


success         LDA #$1
                NOP
                BRA success


fail            LDA #$0
                NOP
                BRA fail

* = $010000
                .fill $1000, $11

* = $01FB00
                .fill $1000, $22
