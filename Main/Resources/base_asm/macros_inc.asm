; Set 8-bit accumulator
setaxs          .macro
                SEP #$30        ; set A&X short
                .as
                .xs 
                .endm
                
; Set 16-bit accumulator
setaxl          .macro
                REP #$30        ; set A&X long 
                .al
                .xl
                .endm

; Set 8-bit accumulator
setas           .macro
                SEP #$20        ; set A short 
                .as
                .endm
                
; Set 16-bit accumulator
setal           .macro
                REP #$20        ; set A long 
                .al
                .endm

; Set 8 bit index registers               
setxs           .macro
                SEP #$10        ; set X short 
                .xs
                .endm
                
; Set 16-bit index registers
setxl           .macro
                REP #$10        ; set X long 
                .xl
                .endm

; Set the direct page. 
; Note: This uses the accumulator and leaves A set to 16 bits. 
setdp           .macro                
                PHA             ; begin setdp macro 
                PHP
                setal
                LDA #\1         ; set DP to page 0
                TCD             
                .dpage \1
                PLP
                PLA             ; end setdp macro 
                .endm 

setdbr          .macro          ; Set the B (Data bank) register 
                PHA             ; begin setdbr macro 
                PHP
                setas
                LDA #\1
                PHA
                PLB
                .databank \1
                PLP
                PLA             ; end setdbr macro 
                .endm 

TURN_ON_SD_LED  .macro
                ; turn on the LED
                LDA GABE_MSTR_CTRL
                AND #~GABE_CTRL_SDC_LED
                ORA #GABE_CTRL_SDC_LED
                STA GABE_MSTR_CTRL
                .endm
                
TURN_OFF_SD_LED .macro
                ; turn off the LED
                LDA GABE_MSTR_CTRL
                AND #~GABE_CTRL_SDC_LED
                STA GABE_MSTR_CTRL
                .endm
                
LOAD_KBD_STATUS_PORT .macro
                LDA @lGABE_SYS_STAT
                AND #$7
                BNE LKSP_U
                LDA @lSTATUS_PORT_FMX
                BRA +
        LKSP_U
                LDA @lSTATUS_PORT_U
        +       .endm
                
LOAD_KBD_INPT_BUF .macro
                LDA @lGABE_SYS_STAT
                AND #$7
                BNE LKIB_U
                LDA @lKBD_INPT_BUF_FMX
                BRA +
        LKIB_U
                LDA @lKBD_INPT_BUF_U
        +       .endm