.cpu "65816"			
.include "macros_inc.asm"

* = $FF00
          JML IBOOT
* = $00FFFC      ; HRESET
          .word $FF00
          
* = $010000

IBOOT    	CLC					; clear the carry flag
			XCE					; move carry to emulation flag.
			setaxl
			
            LDA #$F2FF
            TCS
            
            pea #$1234          ; first argument
			tsc                 ; pointer to first argument 
			inc a
			pha                 ; address of first pointer to stack
			pea #$4567          ; second argument
			pea #$89ab          ; third argument
			jsr test           
			
		
 
 test		phd                 ; presere Direct on stack / FoenixIDE: 0000 rdaum/c256emu: pointer
			tsc                 ; current stack position
			inc a               
			tcd                 ; stack position to direct register

			; D points to one past our return address, so we may now refer
			; to our inputs via direct page references.

			; at this moment stack on FoenixIDE looks like (c256emu is almost the same)
			; FEFF 12
			; FEFE 34 - first value
			; FEFD FE  
			; FEFC FE - stack pointer
			; FEFB 45
			; FEFA 67 - second value
			; FEF9 89
			; FEF8 AB - third value
			; FEF7 00
			; FEF6 13 - return addr 
			; FEF5 00
			; FEF4 00 - Direct Reg preserve ($fef4 on c256emu)
			; Direct Register = $FEF4
			; DBR             = $00
			
            ; expect and got on rdaum/c256emu | GOT val on FoenixIDE:
            lda 10      ; expect $1234 | GOT $1234                         
			lda 8       ; expect stack pointer (example $fef2) | GOT $fefe
			lda 6       ; expect $4567 | GOT $0013 - return pointer 
			lda 4       ; expect $89ab | GOT $0000 ??? 
			lda 2       ; expect return pointer (example $0012) | GOT $0013
			lda 0       ; expect Direct reg ($fef4) | GOT $0000 - but see below

					
			lda #$9988
			sta 10
			lda #$7766
			sta 8
			lda #$5544
			sta 6
			lda #$3322
			sta 4
			lda #$11aa
			sta 2
			lda #$bbcc
			sta 0

			; at this moment stack on FoenixIDE looks like (
                        ; c256emu was consistent with lda/sta sequence)
			; look at fefb, fefa, fef9, fef8 - 
			; FEFF 99
			; FEFE 99
			; FEFD 77
			; FEFC 66
			; FEFB 45 - should be 55 like in c256emu?
			; FEFA 67 - should be 44 ... ?
			; FEF9 89 - should be 33 ... ?
			; FEF8 AB - should be 22 ... ?
			; FEF7 11
			; FEF6 aa
			; FEF5 bb
			; FEF4 cc
			; Direct Register = $FEF4
			; DBR             = $00


                        lda 10              ; expect $9988 - ok
			lda 8               ; expect $7766 - ok
			lda 6               ; expect $5544 GOT $11aa in FoenixIDE
			lda 4               ; expect $3322 GOT $bbcc  ...
			lda 2               ; expect $11aa GOT $11aa  ...
			lda 0               ; expect $bbcc GOT $bbcc  ...
			
            brk