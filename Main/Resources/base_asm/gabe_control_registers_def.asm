;GABE Control Registers

;$AFE880..$AFE887

GABE_MSTR_CTRL      = $AFE880
GABE_CTRL_PWR_LED   = $01     ; Controls the LED in the Front of the case (Next to the reset button)
GABE_CTRL_SDC_LED   = $02     ; Controls the LED in the Front of the Case (Next to SDCard)
GABE_CTRL_BUZZER    = $10     ; Controls the Buzzer
GABE_CTRL_WRM_RST   = $80     ; Warm Reset (needs to Setup other registers)

GABE_NOTUSED        = $AFE881 ; Reserved for future use
GABE_RST_AUTH0      = $AFE882 ; Must Contain the BYTE $AD for Reset to Activate
GABE_RST_AUTH1      = $AFE883 ; Must Contain the BYTE $DE for Reset to Activate

; READ
GABE_RNG_DAT_LO     = $AFE884 ; Low Part of 16Bit RNG Generator
GABE_RNG_DAT_HI     = $AFE885 ; Hi Part of 16Bit RNG Generator

; WRITE
GABE_RNG_SEED_LO    = $AFE884 ; Low Part of 16Bit RNG Generator
GABE_RNG_SEED_HI    = $AFE885 ; Hi Part of 16Bit RNG Generator

; READ
GABE_RNG_STAT       = $AFE886 ;
GABE_RNG_LFSR_DONE  = $80     ; indicates that Output = SEED Database

; WRITE
GABE_RNG_CTRL       = $AFE886 ;
GABE_RNG_CTRL_EN    = $01     ; Enable the LFSR BLOCK_LEN
GABE_RNG_CTRL_DV    = $02     ; After Setting the Seed Value, Toggle that Bit for it be registered

GABE_SYS_STAT       = $AFE887 ;
GABE_SYS_STAT_MID0  = $01     ; Machine ID -- LSB
GABE_SYS_STAT_MID1  = $02     ; Machine ID -- MSB
GABE_SYS_STAT_MID2  = $04     ; Machine ID -- MSB

GABE_SYS_STAT_EXP   = $08     ; if Zero, there is an Expansion Card Preset
GABE_SYS_STAT_CPUA  = $40     ; Indicates the (8bit/16bit) Size of the Accumulator
GABE_SYS_STAT_CPUX  = $80     ; Indicates the (8bit/16bit) Size of the Accumulator


;Bit 2, Bit 1, Bit 0
;$000: FMX
;$100: FMX (Future C5A)
;$001: U 2Meg
;$101: U+ 4Meg U+
;$010: TBD (Reserved)
;$110: TBD (Reserved)
;$011: A2560 Dev
;$111: A2560 Keyboard