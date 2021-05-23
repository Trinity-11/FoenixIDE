BM_Enable             = $01

BM_LUT0               = $00 ;
BM_LUT1               = $02 ;
BM_LUT2               = $04 ;
BM_LUT3               = $06 ;
BM_LUT4               = $08 ;
BM_LUT5               = $0A ;
BM_LUT6               = $0C ;
BM_LUT7               = $0E ;

; First BitMap Plane
BM0_CONTROL_REG     = $AF0100
BM0_START_ADDY_L    = $AF0101
BM0_START_ADDY_M    = $AF0102
BM0_START_ADDY_H    = $AF0103
BM0_X_OFFSET        = $AF0104   ; Not Implemented
BM0_Y_OFFSET        = $AF0105   ; Not Implemented
BM0_RESERVED_6      = $AF0106
BM0_RESERVED_7      = $AF0107
; Second BitMap Plane
BM1_CONTROL_REG     = $AF0108
BM1_START_ADDY_L    = $AF0109
BM1_START_ADDY_M    = $AF010A
BM1_START_ADDY_H    = $AF010B
BM1_X_OFFSET        = $AF010C   ; Not Implemented
BM1_Y_OFFSET        = $AF010D   ; Not Implemented
BM1_RESERVED_6      = $AF010E
BM1_RESERVED_7      = $AF010F
