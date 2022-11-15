using FoenixIDE.MemoryLocations;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoenixIDE.Display
{
    public unsafe partial class Gpu : UserControl
    {
        // To provide a better contrast when writing on top of bitmaps
        Brush BackgroundTextBrush = new SolidBrush(Color.Black);
        Brush TextBrush = new SolidBrush(Color.LightBlue);
        Brush BorderBrush = new SolidBrush(Color.LightBlue);
        Brush InvertedBrush = new SolidBrush(Color.Blue);
        Brush CursorBrush = new SolidBrush(Color.LightBlue);

        /*
        * Display the text with a colored background. This should make the text more visible against bitmaps.
        */
        private void DrawTextWithBackground(String text, Graphics g, Color backgroundColor, int x, int y)
        {
            g.DrawString(text, this.Font, BackgroundTextBrush, x, y);
            g.DrawString(text, this.Font, BackgroundTextBrush, x + 2, y);
            g.DrawString(text, this.Font, BackgroundTextBrush, x, y + 2);
            g.DrawString(text, this.Font, BackgroundTextBrush, x + 2, y + 2);
            g.DrawString(text, this.Font, TextBrush, x + 1, y + 1);
        }

        public Point GetScreenSize()
        {
            Point p = new Point(640, 480);
            if (VICKY != null)
            {
                byte MCRHigh = (byte)(VICKY.ReadByte(MCRAddress + 1) & 3); // Reading address $AF:0001


                switch (MCRHigh)
                {
                    case 1:
                        p.X = 800;
                        p.Y = 600;
                        break;
                    case 2:
                        p.X = 320;
                        p.Y = 240;
                        break;
                    case 3:
                        p.X = 400;
                        p.Y = 300;
                        break;
                }
            }
            return p;
        }

        public Point GetScreenSize_JR()
        {
            Point p = new Point(640, 480);
            if (VICKY != null)
            {
                byte MCRHigh = (byte)(VICKY.ReadByte(MCRAddress + 1) & 7); // Reading address $D001

                if ((MCRHigh & 1) != 0)
                {
                    // @ 70Hz - Text 640x400, Graphics 320x200
                    p.X = 640;
                    p.Y = 400;
                }
                if ((MCRHigh & 2) != 0)
                {
                    p.X = 320;
                }
                if ((MCRHigh & 4) != 0)
                {
                    p.Y = (MCRHigh & 1) != 0? 200 : 240;
                }
            }
            return p;
        }

        // Text Lookup Tables
        int[] FGTextLUT;
        int[] BGTextLUT;
        int GammaBaseAddress;
        public void SetGammaBaseAddress(int val)
        {
            GammaBaseAddress = val;
        }
        int SOL0Address;
        public void SetSOL0Address(int val)
        {
            SOL0Address = val;
        }
        int SOL1Address;
        public void SetSOL1Address(int val)
        {
            SOL1Address = val;
        }
        int LineIRQRegister;
        public void SetLineIRQRegister(int val)
        {
            LineIRQRegister = val;
        }

        int fgLUTAddress;
        public void SetFGLUTAddress(int val)
        {
            fgLUTAddress = val;
        }
        int bgLUTAddress;
        public void SetBGLUTAddress(int val)
        {
            bgLUTAddress = val;
        }
        int lutBaseAddress;
        public void SetLUTBaseAddress(int val)
        {
            lutBaseAddress = val;
        }
        int fontBaseAddress;
        public void SetFontBaseAddress(int val)
        {
            fontBaseAddress = val;
        }
        int lineStartAddress;
        public void SetTextStartAddress(int val)
        {
            lineStartAddress = val;
        }
        int colorStartAddress;
        public void SetTextColorStartAddress(int val)
        {
            colorStartAddress = val;
        }
        int CursorCtrlRegister;
        public void SetCursorCtrlRegister(int val)
        {
            CursorCtrlRegister = val;
        }
        int CursorCharacterAddress;
        public void SetCursorCharacterAddress(int val)
        {
            CursorCharacterAddress = val;
        }
        int CursorXAddress;
        public void SetCursorXAddress(int val)
        {
            CursorXAddress = val;
        }
        int TileMapBaseAddress;
        public void SetTileMapBaseAddress(int val)
        {
            TileMapBaseAddress = val;
        }
        int TilesetBaseAddress;
        public void SetTilesetBaseAddress(int val)
        {
            TilesetBaseAddress = val;
        }

        int SpriteBaseAddress;
        public void SetSpriteBaseAddress(int val)
        {
            SpriteBaseAddress = val;
        }
        private int[] GetTextLUT(byte fg, byte bg, bool gamma)
        {
            int[] values = new int[2];
            var fgt = FGTextLUT;
            if (fgt[fg] == 0)
            {
                // In order to reduce the load of applying Gamma correction, load single bytes
                byte fgValueRed = VICKY.ReadByte(fgLUTAddress + fg * 4);
                byte fgValueGreen = VICKY.ReadByte(fgLUTAddress + fg * 4 + 1);
                byte fgValueBlue = VICKY.ReadByte(fgLUTAddress + fg * 4 + 2);

                if (gamma)
                {
                    fgValueBlue = VICKY.ReadByte(GammaBaseAddress + fgValueBlue); //gammaCorrection[fgValueBlue];
                    fgValueGreen = VICKY.ReadByte(GammaBaseAddress + 0x100 + fgValueGreen);//gammaCorrection[0x100 + fgValueGreen];
                    fgValueRed = VICKY.ReadByte(GammaBaseAddress + 0x200 + fgValueRed);//gammaCorrection[0x200 + fgValueRed];
                }

                values[0] = (int)((fgValueBlue << 16) + (fgValueGreen << 8) + fgValueRed + 0xFF000000);
                fgt[fg] = values[0];
            }
            else
            {
                values[0] = fgt[fg];
            }
            var bgt = BGTextLUT;
            if (bgt[bg] == 0)
            {
                // Read the color lookup tables
                byte bgValueRed = VICKY.ReadByte(bgLUTAddress + bg * 4);
                byte bgValueGreen = VICKY.ReadByte(bgLUTAddress + bg * 4 + 1);
                byte bgValueBlue = VICKY.ReadByte(bgLUTAddress + bg * 4 + 2);

                if (gamma)
                {
                    bgValueBlue = VICKY.ReadByte(GammaBaseAddress + bgValueBlue); //gammaCorrection[bgValueBlue];
                    bgValueGreen = VICKY.ReadByte(GammaBaseAddress + 0x100 + bgValueGreen); //gammaCorrection[0x100 + bgValueGreen];
                    bgValueRed = VICKY.ReadByte(GammaBaseAddress + 0x200 + bgValueRed); //gammaCorrection[0x200 + bgValueRed];
                }

                values[1] = (int)((bgValueBlue << 16) + (bgValueGreen << 8) + bgValueRed + 0xFF000000);
                bgt[bg] = values[1];
            }
            else
            {
                values[1] = bgt[bg];
            }
            return values;
        }

        // We only cache items that are requested, instead of precomputing all 1024 colors.
        public int GetLUTValue(in byte lutIndex, in byte color, in bool gamma)
        {
            //int offset = lutIndex * 256 + color;
            var lc = lutCache;
            int value = lc[lutIndex * 256 + color];

            if (value == 0)
            {
                int lutAddress = lutBaseAddress + (lutIndex * 256 + color) * 4;
                byte red = VICKY.ReadByte(lutAddress);
                byte green = VICKY.ReadByte(lutAddress + 1);
                byte blue = VICKY.ReadByte(lutAddress + 2);
                if (gamma)
                {
                    blue = VICKY.ReadByte(GammaBaseAddress + blue);           // gammaCorrection[fgValueBlue];
                    green = VICKY.ReadByte(GammaBaseAddress + 0x100 + green); // gammaCorrection[0x100 + fgValueGreen];
                    red = VICKY.ReadByte(GammaBaseAddress + 0x200 + red);     // gammaCorrection[0x200 + fgValueRed];
                }
                value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                lc[lutIndex * 256 + color] = value;
            }
            return value;
        }
        private unsafe void DrawBitmapText(int* p, int MCR, bool gammaCorrection, byte TextColumns, byte TextRows, int colOffset, int rowOffset, int line, int width, int height)
        {
            bool overlayBitSet = (MCR & 0x02) == 0x02;

            // Find which line of characters to display
            int txtline = (line - rowOffset) / CHAR_HEIGHT;
            //if (txtline + 1 > RAM.ReadByte(MemoryMap.LINES_MAX)) return;
            //byte COLS_PER_LINE = RAM.ReadByte(MemoryMap.COLS_PER_LINE);

            // Initialize the LUTs
            FGTextLUT = new int[16];
            BGTextLUT = new int[16];

            // Cursor Values
            
            int curPosY = VICKY.ReadWord(CursorXAddress + 2);
            int curPosX = VICKY.ReadWord(CursorXAddress);

            bool CursorEnabled = (VICKY.ReadByte(CursorCtrlRegister) & 1) != 0;

            // Each character is defined by 8 bytes
            int fontLine = (line - rowOffset) % CHAR_HEIGHT;
            int* ptr = p + line * STRIDE + colOffset;
            for (int col = 0; col < width / CHAR_WIDTH; col++)
            {
                int x = col * CHAR_WIDTH;
                if (x + colOffset > width - 1 - colOffset)
                {
                    continue;
                }
                int offset = 0;
                if (col < TextColumns)
                {
                    offset = TextColumns * txtline + col;
                }
                int textAddr = lineStartAddress + offset;
                int colorAddr = colorStartAddress + offset;
                // Each character will have foreground and background colors
                byte character = VICKY.ReadByte(textAddr);
                byte color = VICKY.ReadByte(colorAddr);

                // Display the cursor
                if (curPosX == col && curPosY == txtline && CursorState && CursorEnabled)
                {
                    character = VICKY.ReadByte(CursorCharacterAddress);
                }

                byte fgColor = (byte)((color & 0xF0) >> 4);
                byte bgColor = (byte)(color & 0x0F);

                int[] textColors = GetTextLUT(fgColor, bgColor, gammaCorrection);

                byte value = VICKY.ReadByte(fontBaseAddress + character * 8 + fontLine);
                //int offset = (x + line * 640) * 4;

                // For each bit in the font, set the foreground color - if the bit is 0 and overlay is set, skip it (keep the background)
                for (int b = 0x80; b > 0; b >>= 1)
                {
                    if ((value & b) != 0)
                    {
                        //System.Runtime.InteropServices.Marshal.WriteInt32(p, offset, fgValue);
                        ptr[0] = textColors[0];
                    }
                    else if (!overlayBitSet)
                    {
                        //System.Runtime.InteropServices.Marshal.WriteInt32(p, offset, bgValue);
                        ptr[0] = textColors[1];
                    }
                    ptr++;
                    //offset += 4;
                }
            }
        }

        int BitmapControlRegister;
        public void SetBitmapControlRegister(int val)
        {
            BitmapControlRegister = val;
        }
        /**
         *
         * dX => double the horizontal pixels
         * dY => double the vertical pixels
         */
        private unsafe void DrawBitmap(int* p, bool gammaCorrection, int layer, bool bkgrnd, int bgndColor, int borderXSize, int borderYSize, int line, int width, bool dX, bool dY)
        {

            // Bitmap Controller is located at $AF:0100 and $AF:0108
            int regAddr = BitmapControlRegister + layer * 8;
            byte reg = VICKY.ReadByte(regAddr);
            if ((reg & 0x01) == 00)
            {
                return;
            }
            byte lutIndex = (byte)((reg >> 1) & 7);  // 8 possible LUTs

            int bitmapAddress = VICKY.ReadLong(regAddr + 1) & 0x3F_FFFF;
            int xOffset = 0;
            int yOffset = 0;
            if (mode == 0)
            {
                xOffset = VICKY.ReadWord(regAddr + 4);
                yOffset = VICKY.ReadWord(regAddr + 6);
            }

            int clrVal = 0;
            int offsetAddress;
            if (!dY)
            {
                offsetAddress = bitmapAddress + line * width;
            }
            else
            {
                offsetAddress = bitmapAddress + (line/2) * (dX ? width/2 : width);
            }
            
            int pixelOffset = line * STRIDE;
            int* ptr = p + pixelOffset;
            //int col = borderXSize;
            byte pixVal = 0;
            if (!dX)
            {
                VRAM.CopyIntoBuffer(offsetAddress, width, pixVals);
            }
            else
            {
                for (int i=0;i < width/2; i++)
                {
                    pixVals[i*2] = VRAM.ReadByte(offsetAddress + i);
                    pixVals[i*2+1] = pixVals[i * 2];
                }
            }

            //while (col < width - borderXSize)
            for (int col = borderXSize; col < width - borderXSize; col++)
            {
                clrVal = bgndColor;
                pixVal = pixVals[col];
                if (pixVal != 0)
                {
                    clrVal = GetLUTValue(lutIndex, pixVal, gammaCorrection);
                    ptr[col] = clrVal;
                }
            }
        }

        private unsafe void DrawTiles(int* p, bool gammaCorrection, byte TextColumns, int layer, bool bkgrnd, in int borderXSize, in int line, in int width, bool dX, bool dY)
        {
            // There are four possible tilemaps to choose from
            int addrTileCtrlReg = TileMapBaseAddress + layer * 12;
            int reg = VICKY.ReadByte(addrTileCtrlReg);
            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00)
            {
                return;
            }
            bool smallTiles = (reg & 0x10) > 0;

            int tileSize = (smallTiles ? 8 : 16);
            int strideLine = tileSize * 16;
            byte scrollMask = (byte)(smallTiles ? 0xE : 0xF);

            int tilemapWidth = VICKY.ReadWord(addrTileCtrlReg + 4) & 0x3FF;   // 10 bits
            //int tilemapHeight = VICKY.ReadWord(addrTileCtrlReg + 6) & 0x3FF;  // 10 bits
            int tilemapAddress = VICKY.ReadLong(addrTileCtrlReg + 1) & 0x3F_FFFF;

            // the tilemapWindowX is 10 bits and the scrollX is the lower 4 bits.  The IDE combines them.
            // direction is bit 15.
            int tilemapWindowX = VICKY.ReadWord(addrTileCtrlReg + 8);
            bool dirLeft = (tilemapWindowX & 0x8000) != 0;
            byte scrollX = (byte)(((tilemapWindowX & scrollMask) * (dirLeft ? -1 : 1)) & scrollMask);
            if (smallTiles)
            {
                tilemapWindowX = ((tilemapWindowX & 0x3FF0 + scrollX) >> 1) ;
            }
            else
            {
                tilemapWindowX = (tilemapWindowX & 0x3FF0) + scrollX;
            }
            int tileXOffset = tilemapWindowX % tileSize;

            // the tilemapWindowY is 10 bits and the scrollY is the lower 4 bits.  The IDE combines them.
            // direction is bit 15.
            int tilemapWindowY = VICKY.ReadWord(addrTileCtrlReg + 10);
            bool dirUp = (tilemapWindowY & 0x8000) != 0;
            byte scrollY = (byte)(((tilemapWindowY & scrollMask) * (dirUp ? -1 : 1)) & scrollMask);
            if (smallTiles)
            {
                tilemapWindowY = ((tilemapWindowY & 0x3FF0 + scrollY) >> 1);
            }
            else
            {
                tilemapWindowY = (tilemapWindowY & 0x3FF0) + scrollY;
            }

            int tileRow = ((dY ? line / 2: line) + tilemapWindowY) / tileSize;
            int tileYOffset = ((dY ? line / 2 : line) + tilemapWindowY) % tileSize;
            int maxX = width - borderXSize;

            // we always read tiles 0 to width/TILE_SIZE + 1 - this is to ensure we can display partial tiles, with X,Y offsets
            int tilemapItemCount = (dX ? width / 2 : width) / tileSize + 1;
            byte[] tiles = new byte[tilemapItemCount * 2];
            int[] tilesetOffsets = new int[tilemapItemCount];
            VRAM.CopyIntoBuffer(tilemapAddress + (1 + tilemapWindowX / tileSize) * 2 + (tileRow + 0) * tilemapWidth * 2, tilemapItemCount * 2, tiles);

            // cache of tilesetPointers
            int[] tilesetPointers = new int[8];
            int[] strides = new int[8];
            for (int i = 0; i < 8; i++)
            {
                tilesetPointers[i] = VICKY.ReadLong(TilesetBaseAddress + i * 4) & 0x3F_FFFF;
                byte tilesetConfig = VICKY.ReadByte(TilesetBaseAddress + i * 4 + 3);
                strides[i] = (tilesetConfig & 8) != 0 ? strideLine : tileSize;
            }
            for (int i = 0; i < tilemapItemCount; i++)
            {
                byte tile = (byte)(tiles[i * 2]);
                byte tilesetReg = tiles[i * 2 + 1];
                byte tileset = (byte)(tilesetReg & 7);
                //byte tileLUT = (byte)((tilesetReg & 0x38) >> 3);

                // tileset
                int tilesetPointer = tilesetPointers[tileset];
                int strideX = strides[tileset];
                if (strideX == tileSize)
                {
                    tilesetOffsets[i] = tilesetPointer + (tile % 16) * tileSize * tileSize + (tile / 16) * tileSize * tileSize * 16 + tileYOffset * tileSize;
                }
                else
                {
                    tilesetOffsets[i] = tilesetPointer + ((tile / 16) * strideX * tileSize + (tile % 16) * tileSize) + tileYOffset * strideX;
                }
            }

            int* ptr = p + line * STRIDE;

            // Ensure that only one line gets drawn, this avoid incorrect wrapping
            //int endX = width - borderXSize;
            //for (int x = borderXSize; x < endX; x++)
            //{
            //    int tileIndex = (x + tileXOffset) / TILE_SIZE;
            //    byte tilesetReg = tiles[tileIndex * 2 + 1];
            //    byte tileLUT = (byte)((tilesetReg & 0x38) >> 3);

            //    int tilesetOffsetAddress = tilesetOffsets[tileIndex] + (x + tilemapWindowX) % TILE_SIZE;
            //    byte pixelIndex = VRAM.ReadByte(tilesetOffsetAddress);
            //    if (pixelIndex > 0)
            //    {
            //        int clrVal = GetLUTValue(tileLUT, pixelIndex, gammaCorrection);
            //        ptr[x] = clrVal;
            //    }
            //}
            // alternate display style - avoids repeating the loop so often
            int startTileX = (borderXSize + tileXOffset) / tileSize;
            int endTileX = ((dX ? width/2 : width) - borderXSize + tileXOffset) / tileSize + 1;
            int startOffset = (borderXSize + tilemapWindowX) % tileSize;
            int x = borderXSize;
            byte[] tilepix = new byte[tileSize];
            int clrVal = 0;
            for (int t = startTileX; t < endTileX; t++)
            {
                byte tilesetReg = tiles[t * 2 + 1];
                byte lutIndex = (byte)((tilesetReg & 0x38) >> 3);
                //int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - VICKY.StartAddress + lutIndex * 1024;
                int tilesetOffsetAddress = tilesetOffsets[t];  // + startOffset

                VRAM.CopyIntoBuffer(tilesetOffsetAddress, tileSize, tilepix);
                do
                {
                    byte pixVal = tilepix[startOffset];
                    if (pixVal > 0)
                    {
                        clrVal = GetLUTValue(lutIndex, pixVal, gammaCorrection);
                        ptr[x] = clrVal;
                    }
                    x++;
                    startOffset++;
                    tilesetOffsetAddress++;
                    if (dX)
                    {
                        if (pixVal >0)
                        {
                            ptr[x] = clrVal;
                        }
                        x++;
                    }
                } while (startOffset != tileSize && x < maxX);
                startOffset = 0;
                if (x == maxX)
                {
                    break;
                }
            }
        }
        const int screenOffset = 32;
        private unsafe void DrawSprites(int* p, bool gammaCorrection, byte layer, bool bkgrnd, int borderXSize, int borderYSize, int line, int width, int height, bool dX, bool dY)
        {
            // There are 32 possible sprites to choose from.
            for (int s = 63; s > -1; s--)
            {
                int addrSprite = SpriteBaseAddress + s * 8;
                byte reg = VICKY.ReadByte(addrSprite);
                // if the set is not enabled, we're done.
                byte spriteLayer = (mode == 0) ? ((byte)((reg & 0x70) >> 4)) : ((byte)((reg & 0x18) >> 3));
                // if the sprite is enabled and the layer matches, then check the line
                if ((reg & 1) != 0 && layer == spriteLayer)
                {
                    byte spriteSize = 32;
                    if (mode == 1)
                    {
                        switch ((reg & 0x60) >> 5)
                        {
                            case 1:
                                spriteSize = 24;
                                break;
                            case 2:
                                spriteSize = 16;
                                break;
                            case 3:
                                spriteSize = 8;
                                break;
                        }
                    }
                    int posY = VICKY.ReadWord(addrSprite + 6) - screenOffset;
                    int actualLine = dY ? line / 2 : line;
                    if ((actualLine >= posY && actualLine < posY + spriteSize))
                    {
                        // TODO Fix this when Vicky II fixes the LUT issue
                        byte lutIndex = (mode == 0) ? (byte)(((reg & 0xC) >> 1)) : (byte)(((reg & 6) >> 1));
            
                        int lutAddress = lutBaseAddress + lutIndex * 1024;
                        bool striding = (reg & 0x80) == 0x80;

                        int spriteAddress = VICKY.ReadLong(addrSprite + 1) & 0x3F_FFFF;
                        int posX = VICKY.ReadWord(addrSprite + 4) - screenOffset;
                        if (dX)
                        {
                            posX *= 2;
                        }


                        if (posX >= (width - borderXSize) || posY >= (height - spriteSize) || (posX + spriteSize) < 0 || (posY + spriteSize) < 0)
                        {
                            continue;
                        }
                        int spriteWidth = spriteSize;
                        int xOffset = 0;
                        // Check for sprite bleeding on the left-hand-side
                        if (posX < borderXSize)
                        {
                            xOffset = borderXSize - posX;
                            posX = borderXSize;
                            spriteWidth = spriteSize - xOffset;
                            if (spriteWidth == 0)
                            {
                                continue;
                            }
                        }
                        // Check for sprite bleeding on the right-hand side
                        if (posX + spriteSize > width - borderXSize)
                        {
                            spriteWidth = width - borderXSize - posX;
                            if (spriteWidth == 0)
                            {
                                continue;
                            }
                        }

                        int clrVal = 0;
                        byte pixVal = 0;

                        int sline = actualLine - posY;
                        int lineOffset = line * STRIDE;
                        int* ptr = p + lineOffset;
                        int cols = spriteSize;
                        if (posX + (dX ? spriteSize*2:spriteSize) >= width - borderXSize)
                        {
                            cols = width - borderXSize - posX;
                            if (dX)
                            {
                                cols /= 2;
                            }
                        }
                        for (int col = xOffset; col < xOffset + cols; col++)
                        {
                            // Lookup the pixel in the tileset - if the value is 0, it's transparent
                            pixVal = VRAM.ReadByte(spriteAddress + col + sline * spriteSize);
                            if (pixVal != 0)
                            {
                                clrVal = GetLUTValue(lutIndex, pixVal, gammaCorrection);
                                ptr[(dX ? col * 2: col) - xOffset + posX] = clrVal;
                                if (dX)
                                {
                                    ptr[col * 2 +1 - xOffset + posX] = clrVal;
                                }
                            }
                        }
                    }

                }
            }
        }
        private unsafe void DrawMouse(int* p, bool gammaCorrection, int line, int width, int height)
        {
            byte mouseReg = VICKY.ReadByte(MemoryMap.MOUSE_PTR_REG - VICKY.StartAddress);
            bool MousePointerEnabled = (mouseReg & 1) == 1;

            if (MousePointerEnabled)
            {
                int PosX = VICKY.ReadWord(MousePointerRegister + 2);
                int PosY = VICKY.ReadWord(MousePointerRegister + 4);
                if (line >= PosY && line < PosY + 16)
                {
                    int pointerAddress = (mouseReg & 2) == 0 ? MemoryMap.MOUSE_PTR_GRAP0 - VICKY.StartAddress : MemoryMap.MOUSE_PTR_GRAP1 - VICKY.StartAddress;

                    // Mouse pointer is a 16x16 icon
                    int colsToDraw = PosX < width - 16 ? 16 : width - PosX;

                    int mline = line - PosY;
                    int* ptr = p + line * STRIDE;
                    for (int col = 0; col < colsToDraw; col++)
                    {
                        // Values are 0: transparent, 1:black, 255: white (gray scales)
                        byte pixelIndexR = VICKY.ReadByte(pointerAddress + mline * 16 + col);
                        byte pixelIndexG = pixelIndexR;
                        byte pixelIndexB = pixelIndexR;
                        if (pixelIndexR != 0)
                        {
                            if (gammaCorrection)
                            {
                                pixelIndexB = VICKY.ReadByte(GammaBaseAddress + pixelIndexR); // gammaCorrection[pixelIndexR];
                                pixelIndexG = VICKY.ReadByte(GammaBaseAddress + 0x100 + pixelIndexR); //gammaCorrection[0x100 + pixelIndexR];
                                pixelIndexR = VICKY.ReadByte(GammaBaseAddress + 0x200 + pixelIndexR); //gammaCorrection[0x200 + pixelIndexR];
                            }
                            int value = (int)((pixelIndexB << 16) + (pixelIndexG << 8) + pixelIndexR + 0xFF000000);
                            ptr[col + PosX] = value;
                        }
                    }
                }

            }
        }
    }
}
