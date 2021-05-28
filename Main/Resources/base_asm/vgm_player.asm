; ****************************************************************************
; * Video Game Music Player
; * Author Daniel Tremblay
; * Code written for the C256 Foenix retro computer
; * Permission is granted to reuse this code to create your own games
; * for the C256 Foenix.
; * Copyright Daniel Tremblay 2020
; * This code is provided without warranty.
; * Please attribute credits to Daniel Tremblay if you reuse.
; ****************************************************************************
; *   To play VGM files in your games, include this file first.
; *   Next, in your interrupt handler, enable timer0.
; *   In the TIMER0 interrupt handler, call the VGM_WRITE_REGISTER subroutine.
; *   In your game code, set the SONG_START to the beginning of your VGM file.
; *   Call VGM_SET_SONG_POINTERS, this will initialize the other register.
; *   Finally, call VGM_INIT_TIMERS to initialize TIMER0 and TIMER1.
; *   Chips supported at this time are:
; *     - SN76489 (PSG)
; *     - YM2612 (OPN2)
; *     - YM2151 (OPM)
; *     - YM262 (OPL3)
; *     - YM3812 (OPL2)
; ****************************************************************************

; Important offsets
VGM_VERSION       = $8  ; 32-bits
SN_CLOCK          = $C  ; 32-bits
LOOP_OFFSET       = $1C ; 32-bits
YM_OFFSET         = $2C ; 32-bits
OPM_CLOCK         = $30 ; 32-bits
VGM_OFFSET        = $34 ; 32-bits

; VGM Registers
COMMAND           = $7F ; 1 byte

; VGM Registers
SONG_START        = $84 ; 4 bytes
CURRENT_POSITION  = $88 ; 4 bytes
WAIT_CNTR         = $8C ; 2 bytes
LOOP_OFFSET_REG   = $8E ; 2 bytes
PCM_OFFSET        = $8A ; 4 bytes

AY_3_8910_A       = $90 ; 2 bytes
AY_3_8910_B       = $92 ; 2 bytes
AY_3_8910_C       = $94 ; 2 bytes
AY_3_8910_N       = $96 ; 2 bytes

DATA_STREAM_CNT   = $7D ; 2 byte
DATA_STREAM_TBL   = $8000 ; each entry is 4 bytes

OPM_BASE_ADDRESS  = $AFF000
PSG_BASE_ADDRESS  = $AFF100
OPN2_BASE_ADDRESS = $AFF200
OPL3_BASE_ADRESS  = $AFE600

increment_long_addr .macro
            setal
            INC \1
            BNE increment_done
            INC \1 + 2
    increment_done
            setas
                    .endm
; *******************************************************************
; * Interrupt driven sub-routine.
; *******************************************************************
VGM_WRITE_REGISTER
            .as
            LDX WAIT_CNTR
            CPX #0
            BEQ READ_COMMAND
            
            DEX
            STX WAIT_CNTR
            
            RTL
            
    READ_COMMAND
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            STA COMMAND
            increment_long_addr CURRENT_POSITION
            
            AND #$F0
            LSR A
            LSR A
            LSR A
            TAX
            JMP (VGM_COMMAND_TABLE,X)
            
    VGM_LOOP_DONE
            RTL
            
; *******************************************************************
; * Command Table
; *******************************************************************
VGM_COMMAND_TABLE
            .word <>INVALID_COMMAND ;0
            .word <>INVALID_COMMAND ;1
            .word <>INVALID_COMMAND ;2
            .word <>SKIP_BYTE_CMD   ;3 - reserved - not implemented
            .word <>SKIP_BYTE_CMD   ;4 - not implemented
            .word <>WRITE_YM_CMD    ;5 - YM*
            .word <>WAIT_COMMANDS   ;6
            .word <>WAIT_N_1        ;7
            .word <>YM2612_SAMPLE   ;8
            .word <>DAC_STREAM      ;9
            .word <>AY8910          ;A - AY8910
            .word <>SKIP_TWO_BYTES  ;B - not implemented
            .word <>SKIP_THREE_BYTES;C - not implemented
            .word <>SKIP_THREE_BYTES;D - not implemented
            .word <>SEEK_OFFSET     ;E - not implemented
            .word <>SKIP_FOUR_BYTES ;F - not implemented
            
INVALID_COMMAND
            .as
            JMP VGM_WRITE_REGISTER

SKIP_BYTE_CMD
            .as
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER

SKIP_TWO_BYTES
            .as
            setal
            INC CURRENT_POSITION
            BNE s2_1
            INC CURRENT_POSITION + 2
    s2_1
            INC CURRENT_POSITION
            BNE s2_2
            INC CURRENT_POSITION + 2
    s2_2
            setas
            JMP VGM_WRITE_REGISTER

SKIP_THREE_BYTES
            .as
            setal
            INC CURRENT_POSITION
            BNE s3_1
            INC CURRENT_POSITION + 2
    s3_1
            INC CURRENT_POSITION
            BNE s3_2
            INC CURRENT_POSITION + 2
    s3_2
            INC CURRENT_POSITION
            BNE s3_3
            INC CURRENT_POSITION + 2
    s3_3
            setas
            JMP VGM_WRITE_REGISTER

SEEK_OFFSET
            .as
            LDA COMMAND
            CMP #$E0
            BNE SKIP_FOUR_BYTES
            
            ; read 4 bytes, add them to the databank 0 offset
            ; and store in the PCM_OFFSET
            setal
            LDA [CURRENT_POSITION]
            STA ADDER_A
            increment_long_addr CURRENT_POSITION
            increment_long_addr CURRENT_POSITION
            setal
            LDA [CURRENT_POSITION]
            STA ADDER_A + 2
            increment_long_addr CURRENT_POSITION
            increment_long_addr CURRENT_POSITION
            setal
            LDA DATA_STREAM_TBL
            STA ADDER_B
            LDA DATA_STREAM_TBL + 2
            STA ADDER_B + 2
            
            LDA ADDER_R
            STA PCM_OFFSET
            LDA ADDER_R + 2
            STA PCM_OFFSET + 2
            setas
            
            JMP VGM_LOOP_DONE
            
SKIP_FOUR_BYTES
            .as
            setal
            INC CURRENT_POSITION
            BNE s4_1
            INC CURRENT_POSITION + 2
    s4_1
            INC CURRENT_POSITION
            BNE s4_2
            INC CURRENT_POSITION + 2
    s4_2
            INC CURRENT_POSITION
            BNE s4_3
            INC CURRENT_POSITION + 2
    s4_3
            INC CURRENT_POSITION
            BNE s4_4
            INC CURRENT_POSITION + 2
    s4_4
            setas
            JMP VGM_WRITE_REGISTER

; we need to combine R1 and R0 together before we send
; the data to the SN76489
AY8910 
            .as
            LDA COMMAND
            CMP #$A0
            BEQ AY_COMMAND
            
            JMP SKIP_TWO_BYTES
            
    AY_COMMAND
            ; the second byte is the register
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            CMP #0 ; Register 0 fine
            BNE AY_R1
            
            LDA AY_3_8910_A
            CMP #8
            BLT R0_FINE
            
            LDA #$87
            STA PSG_BASE_ADDRESS
            LDA #$3F
            STA PSG_BASE_ADDRESS
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        R0_FINE
            XBA
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            
            setal
            LSR A ; drop the LSB
            setas
            PHA
            AND #$F
            ORA #$80
            STA PSG_BASE_ADDRESS
            
            PLA
            setal
            LSR A
            LSR A
            LSR A
            LSR A
            setas
            
            AND #$3F ; 6 bits

            STA PSG_BASE_ADDRESS
            JMP VGM_WRITE_REGISTER
            
            
    AY_R1   CMP #1
            BNE AY_R2
            
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            AND #$F
            STA AY_3_8910_A
            
            JMP VGM_WRITE_REGISTER
            
    AY_R2   CMP #2
            BNE AY_R3
            
            LDA AY_3_8910_B
            CMP #8
            BLT R1_FINE
            
            LDA #$A7
            STA PSG_BASE_ADDRESS
            LDA #$3F
            STA PSG_BASE_ADDRESS
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        R1_FINE
            XBA
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            
            setal
            LSR A ; drop the LSB
            setas
            
            PHA
            AND #$F
            ORA #$A0
            STA PSG_BASE_ADDRESS
            
            PLA
            setal
            LSR A
            LSR A
            LSR A
            LSR A
            setas
            AND #$3F ; 6 bits

            STA PSG_BASE_ADDRESS
            JMP VGM_WRITE_REGISTER
            
    AY_R3   CMP #3
            BNE AY_R4
            
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            AND #$F
            STA AY_3_8910_B
            
            JMP VGM_WRITE_REGISTER
            
    AY_R4   CMP #4
            BNE AY_R5
            
            LDA AY_3_8910_C
            CMP #8
            BLT R2_FINE
            
            LDA #$C7
            STA PSG_BASE_ADDRESS
            LDA #$3F
            STA PSG_BASE_ADDRESS
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        R2_FINE
            XBA
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            
            setal
            LSR A ; drop the LSB
            setas
            
            PHA
            AND #$F
            ORA #$C0
            STA PSG_BASE_ADDRESS
            
            PLA
            setal
            LSR A
            LSR A
            LSR A
            LSR A
            setas
            AND #$3F ; 6 bits

            STA PSG_BASE_ADDRESS
            JMP VGM_WRITE_REGISTER
            
    AY_R5   CMP #5
            BNE AY_R10
            
            LDA [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            AND #$F
            STA AY_3_8910_C
            
            JMP VGM_WRITE_REGISTER
    
    AY_R10
            CMP #8
            BNE AY_R11
            
            LDA #$F
            SEC
            SBC [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            AND #$F
            ORA #$90
            STA PSG_BASE_ADDRESS
            JMP VGM_WRITE_REGISTER
            
    AY_R11
            CMP #9
            BNE AY_R12
            
            LDA #$F
            SEC
            SBC [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            
            AND #$F
            ORA #$B0
            STA PSG_BASE_ADDRESS
            JMP VGM_WRITE_REGISTER
            
    AY_R12
            CMP #10
            BNE AY_R15
            
            LDA #$F
            SEC
            SBC [CURRENT_POSITION]
            increment_long_addr CURRENT_POSITION
            AND #$F
            ORA #$D0
            STA PSG_BASE_ADDRESS
            JMP VGM_WRITE_REGISTER
            
    AY_R15
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER

; *******************************************************************
; * YM Commands
; *******************************************************************
WRITE_YM_CMD
            .as
            LDA COMMAND

            CMP #$50
            BNE CHK_YM2413
            
            LDA [CURRENT_POSITION]
            STA PSG_BASE_ADDRESS
            increment_long_addr CURRENT_POSITION
            JMP VGM_LOOP_DONE ; for some reason, this chip needs more time between writes
            
        CHK_YM2413
            CMP #$51
            BNE CHK_YM2612_P0
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPL3_BASE_ADRESS,X
            ;STA @lOPN2_BASE_ADDRESS,X  ; this probably won't work
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM2612_P0
            CMP #$52
            BNE CHK_YM2612_P1
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPN2_BASE_ADDRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_LOOP_DONE ; for some reason, this chip needs more time between writes
            
        CHK_YM2612_P1
            CMP #$53
            BNE CHK_YM2151
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPN2_BASE_ADDRESS + $100,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_LOOP_DONE ; for some reason, this chip needs more time between writes
            
        CHK_YM2151
            CMP #$54
            BNE CHK_YM2203
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPM_BASE_ADDRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM2203
            CMP #$55
            BNE CHK_YM2608_P0
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            ;STA @lOPM_BASE_ADDRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM2608_P0
            CMP #$56
            BNE CHK_YM2608_P1
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            CMP #$10  ; if the register is 0 to $1F, process as SSG
            BGE YM2608_FM
            JMP AY8910

        YM2608_FM
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPN2_BASE_ADDRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM2608_P1
            CMP #$57
            BNE CHK_YM2610_P0
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPN2_BASE_ADDRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM2610_P0
            CMP #$58
            BNE CHK_YM2610_P1
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            CMP #$10  ; if the register is 0 to $1F, process as SSG
            BGE YM2610_FM
            JMP AY8910
            
        YM2610_FM
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPN2_BASE_ADDRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM2610_P1
            CMP #$59
            BNE CHK_YM3812
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPN2_BASE_ADDRESS + $100,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM3812
            CMP #$5A
            BNE CHK_YM262_P0
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPL3_BASE_ADRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
        
        CHK_YM262_P0
            CMP #$5E
            BNE CHK_YM262_P1
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPL3_BASE_ADRESS,X
            increment_long_addr CURRENT_POSITION
            JMP VGM_WRITE_REGISTER
            
        CHK_YM262_P1
            CMP #$5F
            BNE YM_DONE
            
            ; the second byte is the register
            LDA #0
            XBA
            LDA [CURRENT_POSITION]
            TAX
            increment_long_addr CURRENT_POSITION
            
            ; the third byte is the value to write in the register
            LDA [CURRENT_POSITION]
            STA @lOPL3_BASE_ADRESS+ $100,X
            increment_long_addr CURRENT_POSITION
    YM_DONE
            JMP VGM_WRITE_REGISTER

; *******************************************************************
; * Wait Commands
; *******************************************************************
WAIT_COMMANDS
            .as
            LDA COMMAND
            CMP #$61
            BNE CHK_WAIT_60th
            setal
            LDA [CURRENT_POSITION]
            TAX
            STX WAIT_CNTR
            setas
            increment_long_addr CURRENT_POSITION
            increment_long_addr CURRENT_POSITION
            JMP VGM_LOOP_DONE
            
        CHK_WAIT_60th
            CMP #$62
            BNE CHK_WAIT_50th
            
            LDX #$2df
            STX WAIT_CNTR
            JMP VGM_LOOP_DONE
            
        CHK_WAIT_50th
            CMP #$63
            BNE CHK_END_SONG
            
            LDX #$372
            STX WAIT_CNTR
            JMP VGM_LOOP_DONE

        CHK_END_SONG
            CMP #$66 ; end of song
            BNE CHK_DATA_BLOCK
            
            JSR VGM_SET_LOOP_POINTERS
            JMP VGM_LOOP_DONE
            
        CHK_DATA_BLOCK
            CMP #$67
            BNE DONE_WAIT
            
            JSR READ_DATA_BLOCK
    DONE_WAIT
            JMP VGM_LOOP_DONE

; *******************************************************************
; * Wait N+1 Commands
; *******************************************************************
WAIT_N_1
            .as
            LDA #0
            XBA
            LDA COMMAND
            AND #$F
            TAX
            INX ; $7n where we wait n+1
            STX WAIT_CNTR
            JMP VGM_LOOP_DONE
            
; *******************************************************************
; * Play Samples and wait N
; *******************************************************************
YM2612_SAMPLE
            .as
            
            ; write directly to YM2612 DAC then wait n
            ; load a value from database
            LDA [PCM_OFFSET]
            STA OPN2_BASE_ADDRESS + $2A
            
            ; increment PCM_OFFSET
            setal
            LDA PCM_OFFSET
            INC A
            STA PCM_OFFSET
            BCC YMS_WAIT
            LDA PCM_OFFSET + 2
            INC A
            STA PCM_OFFSET + 2
            
    YMS_WAIT
            setas
            LDA #0
            XBA
            LDA COMMAND
            ; this is the wait part
            AND #$F
            TAX
            STX WAIT_CNTR
            ;CPX #0
            ;BNE YMS_NOT_ZERO
            
            ;RTS
            
    YMS_NOT_ZERO
            JMP VGM_WRITE_REGISTER
            
; *******************************************************************
; * Don't know yet
; *******************************************************************
DAC_STREAM
            .as

            ;JMP VGM_LOOP_DONE
            JMP VGM_WRITE_REGISTER


VGM_SET_SONG_POINTERS
            .as
            
            ; add the start offset
            setal
            LDA #0
            STA WAIT_CNTR
            LDA SONG_START + 2
            STA CURRENT_POSITION + 2
            CLC
            LDY #VGM_OFFSET
            LDA [SONG_START],Y
            ADC #VGM_OFFSET
            ADC SONG_START
            STA CURRENT_POSITION
            BCC VSP_DONE
            
            INC CURRENT_POSITION + 2
    VSP_DONE
            
            setas
            
            RTL

VGM_SET_LOOP_POINTERS
            .as
            
            ; add the start offset
            setal
            LDA #0
            STA WAIT_CNTR
            
            
            CLC
            LDY #LOOP_OFFSET
            LDA [SONG_START],Y
            BEQ NO_LOOP_INFO ; if this is zero, assume that the upper word is also 0
            
            ADC #LOOP_OFFSET ; add the current position
            STA ADDER_A
            INY
            INY
            LDA [SONG_START],Y
            STA ADDER_A + 2
            
            LDA SONG_START
            STA ADDER_B
            LDA SONG_START + 2
            STA ADDER_B + 2
            LDA ADDER_R
            STA CURRENT_POSITION
            LDA ADDER_R + 2
            STA CURRENT_POSITION + 2
            
            BRA VSL_DONE
            
    NO_LOOP_INFO
            
            LDY #VGM_OFFSET
            LDA [SONG_START],Y
            ADC #VGM_OFFSET

            ADC SONG_START
            STA CURRENT_POSITION
            LDA SONG_START + 2
            STA CURRENT_POSITION + 2
            
            BCC VSL_DONE
            INC CURRENT_POSITION + 2
    VSL_DONE
            setas
            
            RTS

            
VGM_INIT_TIMERS
            .as
            
            LDA #64
            STA TIMER0_CMP_L
            LDA #1
            STA TIMER0_CMP_M
            LDA #0
            STA TIMER0_CMP_H
            
            LDA #0    ; set timer0 charge to 0
            STA TIMER0_CHARGE_L
            STA TIMER0_CHARGE_M
            STA TIMER0_CHARGE_H
            
            LDA #TMR0_CMP_RECLR  ; count up from "CHARGE" value to TIMER_CMP
            STA TIMER0_CMP_REG
            
            LDA #(TMR0_EN | TMR0_UPDWN | TMR0_SCLR)
            STA TIMER0_CTRL_REG

            RTL
            
; *******************************************************************************
; * Read a data block   - 67 66 tt ss ss ss ss
; *******************************************************************************
READ_DATA_BLOCK
            .as
            LDA [CURRENT_POSITION] ; should be 66
            ;CMP #$66 ; what happens if it's not 66?
            increment_long_addr CURRENT_POSITION
            LDA  [CURRENT_POSITION] ; should be the type - I expect $C0
            PHA
            increment_long_addr CURRENT_POSITION
            
            ; read the size of the data stream - and compute the end of stream position
            setal
            LDA [CURRENT_POSITION]
            STA ADDER_A
            increment_long_addr CURRENT_POSITION
            increment_long_addr CURRENT_POSITION
            setal
            LDA [CURRENT_POSITION]
            STA ADDER_A + 2
            increment_long_addr CURRENT_POSITION
            increment_long_addr CURRENT_POSITION
            setal
            LDA CURRENT_POSITION
            STA ADDER_B
            LDA CURRENT_POSITION + 2
            STA ADDER_B + 2
            
            ; continue reading the file here
            LDA ADDER_R
            STA CURRENT_POSITION
            LDA ADDER_R + 2
            STA CURRENT_POSITION + 2
            
            setas
            PLA
            BEQ UNCOMPRESSED
            CMP #$C0
            BNE UNKNOWN_DATA_BLOCK
            
    UNCOMPRESSED
            setal
            LDA DATA_STREAM_CNT ; multiply by 4
            ASL A
            ASL A
            TAX
            
            LDA ADDER_B
            STA DATA_STREAM_TBL,X
            LDA ADDER_B + 2
            STA DATA_STREAM_TBL,X + 2

            INC DATA_STREAM_CNT
            setas
            
    UNKNOWN_DATA_BLOCK
            RTS
