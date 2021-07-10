; CS_TileMAP0_Registers $AF:0200 - $AF:027F   - TileMap

; A TileMAP is composed of 16bits Tile Number
;Active_Tile_Data[7:0] -> Tile Number
;Active_Tile_Data[10:8] -> Tile Attributes // Tile Set
;Active_Tile_Data[13:11] -> Undefined
;Active_Tile_Data[14] -> Undefined
;Active_Tile_Data[15] -> undefined

; Bit Field Definition for the Control Register
TILE_Enable             = $01
TILE_Collision_On       = $40           ; Enable       
;
;Tile MAP Layer 0 Registers
TL0_CONTROL_REG         = $AF0200       ; Bit[0] - Enable, Bit[3:1] - LUT Select, Bit[6] - Collision On
TL0_START_ADDY_L        = $AF0201       ; Not USed right now - Starting Address to where is the MAP
TL0_START_ADDY_M        = $AF0202
TL0_START_ADDY_H        = $AF0203
TL0_TOTAL_X_SIZE_L      = $AF0204       ; Size of the Map in X Tile Count [9:0] (1024 Max)
TL0_TOTAL_X_SIZE_H      = $AF0205
TL0_TOTAL_Y_SIZE_L      = $AF0206       ; Size of the Map in Y Tile Count [9:0]
TL0_TOTAL_Y_SIZE_H      = $AF0207
TL0_WINDOW_X_POS_L      = $AF0208       ; Top Left Corner Position of the TileMAp Window in X + Scroll
TL0_WINDOW_X_POS_H      = $AF0209       ; Direction: [14] Pos: [13-4] Scroll: [3:0] in X
TL0_WINDOW_Y_POS_L      = $AF020A       ; Top Left Corner Position of the TileMAp Window in Y
TL0_WINDOW_Y_POS_H      = $AF020B       ; Direction: [14] Pos: [13:4] Scroll: [3:0] in Y
;Tile MAP Layer 1 Registers
TL1_CONTROL_REG         = $AF020C       ; Bit[0] - Enable, Bit[3:1] - LUT Select,
TL1_START_ADDY_L        = $AF020D       ; Not USed right now - Starting Address to where is the MAP
TL1_START_ADDY_M        = $AF020E
TL1_START_ADDY_H        = $AF020F
TL1_TOTAL_X_SIZE_L      = $AF0210       ; Size of the Map in X Tile Count [9:0] (1024 Max)
TL1_TOTAL_X_SIZE_H      = $AF0211
TL1_TOTAL_Y_SIZE_L      = $AF0212       ; Size of the Map in Y Tile Count [9:0]
TL1_TOTAL_Y_SIZE_H      = $AF0213
TL1_WINDOW_X_POS_L      = $AF0214       ; Top Left Corner Position of the TileMAp Window in X + Scroll
TL1_WINDOW_X_POS_H      = $AF0215       ;  Direction: [14] Pos: [13-4] Scroll: [3:0] in X
TL1_WINDOW_Y_POS_L      = $AF0216       ; Top Left Corner Position of the TileMAp Window in Y
TL1_WINDOW_Y_POS_H      = $AF0217       ; Direction: [14] Pos: [13:4] Scroll: [3:0] in Y
;Tile MAP Layer 2 Registers
TL2_CONTROL_REG         = $AF0218       ; Bit[0] - Enable, Bit[3:1] - LUT Select,
TL2_START_ADDY_L        = $AF0219       ; Not USed right now - Starting Address to where is the MAP
TL2_START_ADDY_M        = $AF021A
TL2_START_ADDY_H        = $AF021B
TL2_TOTAL_X_SIZE_L      = $AF021C       ; Size of the Map in X Tile Count [9:0] (1024 Max)
TL2_TOTAL_X_SIZE_H      = $AF021D
TL2_TOTAL_Y_SIZE_L      = $AF021E       ; Size of the Map in Y Tile Count [9:0]
TL2_TOTAL_Y_SIZE_H      = $AF021F
TL2_WINDOW_X_POS_L      = $AF0220       ; Top Left Corner Position of the TileMAp Window in X + Scroll
TL2_WINDOW_X_POS_H      = $AF0221       ;  Direction: [14] Pos: [13-4] Scroll: [3:0] in X
TL2_WINDOW_Y_POS_L      = $AF0222       ; Top Left Corner Position of the TileMAp Window in Y
TL2_WINDOW_Y_POS_H      = $AF0223       ; Direction: [14] Pos: [13:4] Scroll: [3:0] in Y
;Tile MAP Layer 3 Registers
TL3_CONTROL_REG         = $AF0224       ; Bit[0] - Enable, Bit[3:1] - LUT Select,
TL3_START_ADDY_L        = $AF0225       ; Not USed right now - Starting Address to where is the MAP
TL3_START_ADDY_M        = $AF0226
TL3_START_ADDY_H        = $AF0227
TL3_TOTAL_X_SIZE_L      = $AF0228       ; Size of the Map in X Tile Count [9:0] (1024 Max)
TL3_TOTAL_X_SIZE_H      = $AF0229
TL3_TOTAL_Y_SIZE_L      = $AF022A       ; Size of the Map in Y Tile Count [9:0]
TL3_TOTAL_Y_SIZE_H      = $AF022B
TL3_WINDOW_X_POS_L      = $AF022C       ; Top Left Corner Position of the TileMAp Window in X + Scroll
TL3_WINDOW_X_POS_H      = $AF022D       ;  Direction: [14] Pos: [13-4] Scroll: [3:0] in X
TL3_WINDOW_Y_POS_L      = $AF022E       ; Top Left Corner Position of the TileMAp Window in Y
TL3_WINDOW_Y_POS_H      = $AF022F       ; Direction: [14] Pos: [13:4] Scroll: [3:0] in Y

; CS_TileMAP1_Registers $AF:0280 - $AF:02FF   - TileData
; Tile Set 0 Location info
TILESET0_ADDY_L         = $AF0280   ; Pointer to Tileset 0 [21:0]
TILESET0_ADDY_M         = $AF0281
TILESET0_ADDY_H         = $AF0282
TILESET0_ADDY_CFG       = $AF0283   ; [3] - TileStride256x256
; Tile Set 0 Location info
TILESET1_ADDY_L         = $AF0284
TILESET1_ADDY_M         = $AF0285
TILESET1_ADDY_H         = $AF0286
TILESET1_ADDY_CFG       = $AF0287
; Tile Set 0 Location info
TILESET2_ADDY_L         = $AF0288
TILESET2_ADDY_M         = $AF0289
TILESET2_ADDY_H         = $AF028A
TILESET2_ADDY_CFG       = $AF028B
; Tile Set 0 Location info
TILESET3_ADDY_L         = $AF028C
TILESET3_ADDY_M         = $AF028D
TILESET3_ADDY_H         = $AF028E
TILESET3_ADDY_CFG       = $AF028F
; Tile Set 0 Location info
TILESET4_ADDY_L         = $AF0290
TILESET4_ADDY_M         = $AF0291
TILESET4_ADDY_H         = $AF0292
TILESET4_ADDY_CFG       = $AF0293
; Tile Set 0 Location info
TILESET5_ADDY_L         = $AF0294
TILESET5_ADDY_M         = $AF0295
TILESET5_ADDY_H         = $AF0296
TILESET5_ADDY_CFG       = $AF0297
; Tile Set 0 Location info
TILESET6_ADDY_L         = $AF0298
TILESET6_ADDY_M         = $AF0299
TILESET6_ADDY_H         = $AF029A
TILESET6_ADDY_CFG       = $AF029B
; Tile Set 0 Location info
TILESET7_ADDY_L         = $AF029C
TILESET7_ADDY_M         = $AF029D
TILESET7_ADDY_H         = $AF029E
TILESET7_ADDY_CFG       = $AF029F
