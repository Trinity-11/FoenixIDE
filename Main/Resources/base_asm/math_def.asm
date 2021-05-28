;;;
;;; Register address definitions for the math coprocessor
;;;

; Unsigned Multiplier In A (16Bits), In B (16Bits), Answer (32Bits)
UNSIGNED_MULT_A     = $000100 ;2 Bytes Operand A (ie: A x B)
UNSIGNED_MULT_B     = $000102 ;2 Bytes Operand B (ie: A x B)
UNSIGNED_MULT_RESULT= $000104 ;4 Bytes Result of A x B

; Signed Multiplier In A (16Bits), In B (16Bits), Answer (32Bits)
SIGNED_MULT_A       = $000108 ;2 Bytes Operand A (ie: A x B)
SIGNED_MULT_B       = $00010A ;2 Bytes Operand B (ie: A x B)
SIGNED_MULT_RESULT  = $00010C ;4 Bytes Result of A x B

; Unsigned Divide Denominator A (16Bits), Numerator B (16Bits),
; Quotient (16Bits), Remainder (16Bits)
DIVIDER_0        = $000110 ;0 Byte  Signed divider
D0_OPERAND_A     = $000110 ;2 Bytes Divider 1 Dividend ex: A in  B/A
D0_OPERAND_B     = $000112 ;2 Bytes Divider 1 Divisor ex B in B/A
D0_RESULT        = $000114 ;2 Bytes Signed quotient result of B/A ex: 7/2 = 3 r 1
D0_REMAINDER     = $000116 ;2 Bytes Signed remainder of B/A ex: 1 in 7/2=3 r 1

;signed Divide Denominator A (16Bits), Numerator B (16Bits),
; Quotient (16Bits), Remainder (16Bits)
DIVIDER_1        = $000118 ;0 Byte  Unsigned divider
D1_OPERAND_A     = $000118 ;2 Bytes Divider 0 Dividend ex: A in  A/B
D1_OPERAND_B     = $00011A ;2 Bytes Divider 0 Divisor ex B in A/B
D1_RESULT        = $00011C ;2 Bytes Quotient result of A/B ex: 7/2 = 3 r 1
D1_REMAINDER     = $00011E ;2 Bytes Remainder of A/B ex: 1 in 7/2=3 r 1

; 32Bit Adder
ADDER_A          = $000120 ; 4 bytes (32 bit) Accumulator A
ADDER_B          = $000124 ; 4 bytes (32 bit) Accumulator B
ADDER_R          = $000128 ; 4 bytes (32 bit) Result
