

;$AFE200..$AFE20F <- Registers
;Read/Write
FP_MATH_CTRL0 = $AFE200
FP_MATH_CTRL0_INPUT0_MUX = $01  ; 0: UserInput0 - 1: Converter Fixed (20.12) to FP
FP_MATH_CTRL0_INPUT1_MUX = $02  ; 0: UserInput1 - 1: Converter Fixed (20.12) to FP

FP_MATH_CTRL0_ADD_SUB    = $08  ; 0: Substraction - 1: Addition
FP_MATH_CTRL0_ADD_IN0_MUX0 = $10 ; 00: Input Mux0, 01: Input Mux1
FP_MATH_CTRL0_ADD_IN0_MUX1 = $20 ; 10: Mult Out, 11: Div Out
FP_MATH_CTRL0_ADD_IN1_MUX0 = $40 ; 00: Input Mux0, 01: Input Mux1
FP_MATH_CTRL0_ADD_IN1_MUX1 = $80 ; 10: Mult Out, 11: Div Out

FP_MATH_CTRL1 = $AFE201
FP_MATH_CTRL1_OUTPUT_MUX0 = $01 ; 00: Mult Output, 01: Div Output
FP_MATH_CTRL1_OUTPUT_MUX1 = $02 ; 10: Add/Substract Output, 11: '1'

FP_MATH_CTRL2 = $AFE202     ; Not Used - Reserved
FP_MATH_CTRL3 = $AFE203     ; Not Used - Reserved
;Read Only
FP_MATH_MULT_STAT = $AFE204
FP_MULT_STAT_NAN  = $01      ; (NAN) Not a Number Status
FP_MULT_STAT_OVF  = $02      ; Overflow
FP_MULT_STAT_UDF  = $04      ; Underflow
FP_MULT_STAT_ZERO = $08      ; Zero
FP_MATH_DIV_STAT  = $AFE205
FP_DIV_STAT_NAN  = $01      ; Not a number Status
FP_DIV_STAT_OVF  = $02      ; Overflow
FP_DIV_STAT_UDF  = $04      ; Underflow
FP_DIV_STAT_ZERO = $08      ; Zero
FP_DIV_STAT_DIVBYZERO = $10  ; Division by Zero
FP_MATH_ADD_STAT  = $AFE206
FP_ADD_STAT_NAN  = $01      ; Not a number Status
FP_ADD_STAT_OVF  = $02      ; Overflow
FP_ADD_STAT_UDF  = $04      ; Underflow
FP_ADD_STAT_ZERO = $08      ; Zero
FP_MATH_CONV_STAT = $AFE207
FP_CONV_STAT_NAN  = $01      ; Not a number Status
FP_CONV_STAT_OVF  = $02      ; Overflow
FP_CONV_STAT_UDF  = $04      ; Underflow
;Write Input0 (either FP or Fixed (20.12)) - See MUX Setting
FP_MATH_INPUT0_LL = $AFE208
FP_MATH_INPUT0_LH = $AFE209
FP_MATH_INPUT0_HL = $AFE20A
FP_MATH_INPUT0_HH = $AFE20B
;Write Input1 (either FP or Fixed (20.12)) - See MUX Setting
FP_MATH_INPUT1_LL = $AFE20C
FP_MATH_INPUT1_LH = $AFE20D
FP_MATH_INPUT1_HL = $AFE20E
FP_MATH_INPUT1_HH = $AFE20F
;Read Output
FP_MATH_OUTPUT_FP_LL = $AFE208
FP_MATH_OUTPUT_FP_LH = $AFE209
FP_MATH_OUTPUT_FP_HL = $AFE20A
FP_MATH_OUTPUT_FP_HH = $AFE20B
;Read FIXED Output (20.12) Format
FP_MATH_OUTPUT_FIXED_LL = $AFE20C
FP_MATH_OUTPUT_FIXED_LH = $AFE20D
FP_MATH_OUTPUT_FIXED_HL = $AFE20E
FP_MATH_OUTPUT_FIXED_HH = $AFE20F

;DATAA[]  DATAB[]   SIGN BIT  RESULT[]  Overflow Underflow Zero NaN
;Normal   Normal    0         Zero      0       0           1   0
;Normal   Normal    0/1       Normal    0       0           0   0
;Normal   Normal    0/1       Denormal  0       1           1   0
;Normal   Normal    0/1       Infinity  1       0           0   0
;Normal   Denormal  0/1       Normal    0       0           0   0
;Normal   Zero      0/1       Normal    0       0           0   0
;Normal   Infinity  0/1       Infinity  1       0           0   0
;Normal   NaN       X         NaN       0       0           0   1
;Denormal Normal    0/1       Normal    0       0           0   0
;Denormal Denormal  0/1       Normal    0       0           0   0
;Denormal Zero      0/1       Zero      0       0           1   0
;Denormal Infinity  0/1       Infinity  1       0           0   0
;Denormal NaN       X         NaN       0       0           0   1
;Zero     Normal    0/1       Normal    0       0           0   0
;Zero     Denormal  0/1       Zero      0       0           1   0
;Zero     Zero      0/1       Zero      0       0           1   0
;Zero     Infinity  0/1       Infinity  1       0           0   0
;Zero     NaN       X         NaN       0       0           0   1
;Infinity Normal    0/1       Infinity  1       0           0   0
;Infinity Denormal  0/1       Infinity  1       0           0   0
;Infinity Zero      0/1       Infinity  1       0           0   0
;Infinity Infinity  0/1       Infinity  1       0           0   0
;Infinity NaN       X         NaN       0       0           0   1
;NaN      Normal    X         NaN       0       0           0   1
;NaN      Denormal  X         NaN       0       0           0   1
;NaN      Zero      X         NaN       0       0           0   1
;NaN      Infinity  X         NaN       0       0           0   1
;NaN      NaN       X         NaN       0       0           0   1
