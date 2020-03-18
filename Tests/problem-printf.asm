.cpu "65816"
.include "macros_inc.asm"

L1          = $0200
L327        = $0080
L328        = $0220
out_char    = $0300

                CLC
                XCE
  
; pushing -3 on the stack
;        printf("signed %d = unsigned %u = hex %x\n", -3, -3, -3);
                pea    #<>$fffffffd
                pea    #<>$fffffffd
                pea    #<>$fffffffd
                pea    #<>L1+128
                pea    #10
                jsl    printf_
        loop    bra loop
        
        
printf_
                setaxl
                tsc
                sec
                sbc #L327
                tcs
                phd
                tcd
format_0	=	6
;  va_list va;
;  char buffer[1];
;  int ret;
;  va_start(va, format);
va_1	    =	0
buffer_1	=	4
ret_1	    =	5
                clc
                tdc
                adc #<>L327+format_0+4
                sta <>L328+va_1
                lda #$0
                sta <>L328+va_1+2
;  ret = _vsnprintf(_out_char, buffer, (size_t)-1, format, va);
                pei <L328+va_1+2
                pei <L328+va_1
                pei <L327+format_0+2
                pei <L327+format_0
                pea #<>$ffffffff
                pea #0
                clc
                tdc
                adc #<>L328+buffer_1
                pha
                pea #`out_char
                pea #<>out_char
                jsl vsnprintf
                sta <>L328+ret_1
;  va_end(va);
;  return ret;
                lda <>L328+ret_1
L329:
                tay
                phx
                ldx <>L327+4
                lda <>L327+2
                sta <>L327+2,X
                lda <>L327+1
                sta <>L327+1,X
                txa
                plx
                pld
                pha
                tsc
                clc
                adc #L327+2
                adc #<1,s
                tcs
                tya
                rtl
                
vsnprintf
                rtl