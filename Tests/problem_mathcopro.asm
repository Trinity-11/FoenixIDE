.cpu "65816"
.include "macros_inc.asm"
.include "bank00_inc.asm"

* = $1000
IBOOT     CLC           ; clear the carry flag
          XCE
          setaxl
; add two 32-bit integers with the math co-processor
; $5678 1234 + $8765 4321 = DDDD 5555
          LDA #$1234
          STA @lADDER32_A_LL
          LDA #$5678
          STA @lADDER32_A_HL

          LDA #$4321
          STA @lADDER32_B_LL
          LDA #$8765
          STA @lADDER32_B_HL

          ; first word should be $5555
          LDA @lADDER32_R_LL
          CMP #$5555
          BNE addition_failed
          STA BMP_PRSE_DST_PTR

          ; second word should be $DDDD
          LDA @lADDER32_R_HL
          CMP #$DDDD
          BNE addition_failed
          STA BMP_PRSE_DST_PTR+2
          JML multiply

addition_failed
          LDA #$FFFF
          STA BMP_PRSE_DST_PTR
          STA BMP_PRSE_DST_PTR+2
          NOP
          NOP

; multiply two 16-bit integers
; $1234 x $5678 = $626 0060
multiply  LDA #$1234
          STA @lM0_OPERAND_A
          LDA #$5678
          STA @lM0_OPERAND_B

          ; first word should be $0060
          LDA @lM0_RESULT
          CMP #$0060
          BNE multiplication_failed
          STA BMP_PRSE_DST_PTR + 4

          ; second word should be $626
          LDA @lM0_RESULT + 2
          CMP #$626
          BNE multiplication_failed
          STA BMP_PRSE_DST_PTR + 6
          JML divide

multiplication_failed
          LDA #$FFFF
          STA BMP_PRSE_DST_PTR + 4
          STA BMP_PRSE_DST_PTR + 6
          NOP
          NOP

; divide two 16-bit integers
; 33003 / 6000 = 5 with remainder 3003.
; $80EB / $1770 = 5 with remainder $0BBB.
divide
          LDA #33003
          STA @lD0_OPERAND_A
          LDA #6000
          STA @lD0_OPERAND_B

          ; first word should be 5
          LDA @lD0_RESULT
          CMP #5
          BNE division_failed
          STA BMP_PRSE_DST_PTR + 8

          ; second word should be 3
          LDA @lD0_REMAINDER
          CMP #3003
          BNE division_failed
          STA BMP_PRSE_DST_PTR + 10
          JML end

division_failed
          LDA #$FFFF
          STA BMP_PRSE_DST_PTR + 8
          STA BMP_PRSE_DST_PTR + 10
          NOP
          NOP
end       BRK

* = $FF00
          JML IBOOT
* = VECTOR_ERESET      ; HRESET
          .word $FF00
