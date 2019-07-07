.cpu "65816"

          SEC  ; set carry
          LDA #$05 ; A = 5
          SBC #$04 ; subsctract 4
          CMP #$01
          BNE is_fail
          LDA #1
          ; is success
          BRK
is_fail   LDA #0
          BRK
