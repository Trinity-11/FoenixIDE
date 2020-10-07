.cpu "65816"
.include "macros_inc.asm"
.include "Math_Float_def.asm"

* = $20000
FP_TEST
            PHP
            
            ; try a simple multiplication
            LDA #FP_MATH_CTRL0_INPUT0_MUX + FP_MATH_CTRL0_INPUT1_MUX ; user passes fixed values, no addition
            STA FP_MATH_CTRL0
            LDA #0 ; multiplication output
            STA FP_MATH_CTRL1
            
            setal
            LDA five
            STA FP_MATH_INPUT0_LL
            LDA five + 2
            STA FP_MATH_INPUT0_LL + 2
            LDA six
            STA FP_MATH_INPUT1_LL
            LDA six + 2
            STA FP_MATH_INPUT1_LL + 2
            
            ; copy the results to a memory location
            LDA FP_MATH_OUTPUT_FP_LL
            STA res_fp
            LDA FP_MATH_OUTPUT_FP_LL + 2
            STA res_fp + 2
            
            LDA FP_MATH_OUTPUT_FIXED_LL
            STA res_fixed
            LDA FP_MATH_OUTPUT_FIXED_LL + 2
            STA res_fixed + 2
            
            setas
            
            PLP
            RTL

.align 16
five        .dint 5 * 1p12
six         .dint 6 * 1p12
res_fp      .dint 0
res_fixed   .dint 0