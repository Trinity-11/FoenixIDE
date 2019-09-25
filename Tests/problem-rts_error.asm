;;;
;;; Example of issue with JSR/RTS
;;;

.cpu "65816"

FK_PUTC             = $190018 ; Print a character to the currently selected channel

;
; Bank 0 Code Segment, including bootup and variables
;

* = $001000

START       CLC
            XCE

            REP #$30
            .al
            .xl

            LDA #<>PROC
            PHA
            RTS

lock        NOP
            BRA lock

PROC        BRK                 ; Emulator will execute this BRK
            
            SEP #$20            ; Hardware will skip the BRK and start here
            .as
            LDA #'X'
            JSL FK_PUTC         ; ... and print an "X"

            BRK


* = $00FFFC 

VRESET      .word <>START