; ***************************************************************************
; Pending Interrupt (Read and Write Back to Clear)
; TIMER0
TIMER0_CTRL_REG   = $000160 ; (Write - Control, Read Status)
; Control Register Definitions
TMR0_EN     = $01
TMR0_SCLR   = $02
TMR0_SLOAD  = $04 ; Use SLOAD is
TMR0_UPDWN  = $08

TIMER0_CHARGE_L   = $000161 ; Use if you want to Precharge and countdown
TIMER0_CHARGE_M   = $000162 ;
TIMER0_CHARGE_H   = $000163 ;
; Compare Block
TIMER0_CMP_REG    = $000164 ;
TMR0_CMP_RECLR     = $01 ; set to one for it to cycle when Counting up
TMR0_CMP_RELOAD    = $02 ; Set to one for it to reload when Counting Down

TIMER0_CMP_L      = $000165 ; Load this Value for Countup
TIMER0_CMP_M      = $000166 ;
TIMER0_CMP_H      = $000167 ;

; ***************************************************************************
; Pending Interrupt (Read and Write Back to Clear)
; TIMER1
TIMER1_CTRL_REG   = $000168 ;
TMR1_EN     = $01
TMR1_SCLR   = $02
TMR1_SLOAD  = $04
TMR1_UPDWN  = $08 ; 1 = Up, 0 = Down
TIMER1_CHARGE_L   = $000169 ; Use if you want to Precharge and countdown
TIMER1_CHARGE_M   = $00016A ;
TIMER1_CHARGE_H   = $00016B ;
; Compare Block
TIMER1_CMP_REG    = $00016C ;
TMR1_CMP_RECLR     = $01 ; set to one for it to cycle when Counting up
TMR1_CMP_RELOAD    = $02 ; Set to one for it to reload when Counting Down
TIMER1_CMP_L      = $00016D ;
TIMER1_CMP_M      = $00016E ;
TIMER1_CMP_H      = $00016F ;