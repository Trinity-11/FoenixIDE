; Joystick Ports
JOYSTICK0     = $AFE800  ;(R) Joystick 0 - J7 (Next to Buzzer)
JOYSTICK1     = $AFE801  ;(R) Joystick 1 - J8
JOYSTICK2     = $AFE802  ;(R) Joystick 2 - J9
JOYSTICK3     = $AFE803  ;(R) Joystick 3 - J10 (next to SD Card)
; Dip switch Ports
DIPSWITCH     = $AFE804  ;(R) $AFE804...$AFE807
; SD Card CH376S Port
SDCARD_DATA   = $AFE808  ;(R/W) SDCARD (CH376S) Data PORT_A (A0 = 0)
SDCARD_CMD    = $AFE809  ;(R/W) SDCARD (CH376S) CMD/STATUS Port (A0 = 1)
; SD Card Card Presence / Write Protect Status Reg
SDCARD_STAT   = $AFE810  ;(R) SDCARD (Bit[0] = CD, Bit[1] = WP)
; Audio WM8776 CODEC Control Interface (Write Only)
CODEC_DATA_LO = $AFE820  ;(W) LSB of Add/Data Reg to Control CODEC See WM8776 Spec
CODEC_DATA_HI = $AFE821  ;(W) MSB od Add/Data Reg to Control CODEC See WM8776 Spec
CODEC_WR_CTRL = $AFE822  ;(W) Bit[0] = 1 -> Start Writing the CODEC Control Register
