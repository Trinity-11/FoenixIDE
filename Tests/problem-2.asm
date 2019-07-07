.cpu "65816"
.include "macros_inc.asm"
; This assembly code is to debug the problem with stack-indexing mode in the Foenix Emulator
  CLC
  XCE
  
  setal
  LDA #$1111 ; address where to read the data
  PHA ; push the address to the stack
  
  LDA #$1234
  STA $1113 ; store #$1234 at @ 0x1113
  
  ; using this method does not modify the stack
  ; using LDA 1,S is the same as PLA, without affecting the stack
  LDA 1,S ; the value returned into A is $1111
  
  ; now use stack relative indirect indexed addressing
  LDY #2
  LDA (1,S), Y; this should return #$1234