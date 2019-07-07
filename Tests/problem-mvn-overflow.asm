.cpu "65816"
.include "macros_inc.asm"

; This assembly code is to debug the problem with "MVN and MVP"
; Obviously the wrapping around the banks might yield strange results


                CLC
                XCE
                setaxl     ; set everybody to 16-bites

                LDA #$1000    ; this should get one byte moved
                LDX #$FB00  ; bottom address of move
                LDY #$F999  ; bottom address of move

                MVN #$1, #$3  ; move $1001 bytes from $01FB00 to $02F999
                              ; this should wrap around to the start of $010000 and $020000

                CMP #$FFFF  ; Check that A is $FFFF
                BNE fail

                TXA         ; check that X if $B01
                CMP #$0B01
                BNE fail
                TYA
                CMP #$099A   ; check that Y is $99A
                BNE fail

                LDA $03f999  ; test the boundaries
                CMP #$2222
                BNE fail

                LDA $03FE98
                CMP #$1122
                BNE fail

                LDA $040000
                CMP #$0000
                BNE fail

                LDA $030000
                CMP #$1111
                BNE fail

                LDA $030999
                CMP #$11
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
