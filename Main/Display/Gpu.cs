using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Imaging;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.MemoryLocations;
using System.Diagnostics;
using FoenixIDE.Timers;

namespace FoenixIDE.Display
{
    public partial class Gpu : UserControl
    {

        private const int REGISTER_BLOCK_SIZE = 256;
        const int MAX_TEXT_COLS = 128;
        const int MAX_TEXT_LINES = 64;
        const int SCREEN_PAGE_SIZE = 128 * 64;
        
        const int charWidth = 8;
        const int charHeight = 8;
        const int tileSize = 16;
        const int spriteSize = 32;

        public MemoryRAM VRAM = null;
        public MemoryRAM RAM = null;
        public MemoryRAM VICKY = null;
        public int paintCycle = 0;
        private bool tileEditorMode = false;
        public bool MousePointerMode = false;
        public delegate void StartOfFramEvent();
        public StartOfFramEvent StartOfFrame;
        public delegate void StartOfLineEvent();
        public StartOfLineEvent StartOfLine;

        /// <summary>
        /// number of frames to wait to refresh the screen.
        /// One frame = 1/60 second.
        /// </summary>
        public int BlinkingCounter = BLINK_RATE;
        public const int BLINK_RATE = 30;

        // To provide a better contrast when writing on top of bitmaps
        Brush BackgroundTextBrush = new SolidBrush(Color.Black);
        Brush TextBrush = new SolidBrush(Color.LightBlue);
        Brush BorderBrush = new SolidBrush(Color.LightBlue);
        Brush InvertedBrush = new SolidBrush(Color.Blue);
        Brush CursorBrush = new SolidBrush(Color.LightBlue);

        //Timer gpuRefreshTimer = new Timer
        //{
        //    Interval = 15
        //};
        private MultimediaTimer hiresTimer = new MultimediaTimer(16);

        public Gpu()
        {
            InitializeComponent();
            this.Load += new EventHandler(Gpu_Load);
        }

        void Gpu_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(Gpu_Paint);
            //gpuRefreshTimer.Tick += new EventHandler(GpuRefreshTimer_Tick);
            hiresTimer.Elapsed += new MultimediaElapsedEventHandler(GpuRefreshTimer_Tick);

            if (DesignMode)
            {
                //gpuRefreshTimer.Enabled = false;
                hiresTimer.Stop();
            }
            else
            {
                if (ParentForm == null)
                    return;
                int htarget = 480;
                int topmargin = ParentForm.Height - ClientRectangle.Height;
                int sidemargin = ParentForm.Width - ClientRectangle.Width;
                ParentForm.Height = htarget + topmargin;
                ParentForm.Width = (int)Math.Ceiling(htarget * 1.6) + sidemargin;
                //gpuRefreshTimer.Enabled = true;
                hiresTimer.Start();
            }
        }
        void GpuRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (BlinkingCounter-- == 0)
            {
                CursorState = !CursorState;
                BlinkingCounter = BLINK_RATE;
            }
            Invalidate();
        }

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

        /// <summary>
        /// Draw the frame buffer to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Bitmap frameBuffer = new Bitmap(640, 480, PixelFormat.Format32bppArgb);
        private volatile bool drawing = false;
        void Gpu_Paint(object sender, PaintEventArgs e)
        {
            paintCycle++;
            if (DesignMode)
            {
                e.Graphics.DrawString("Design Mode", this.Font, TextBrush, 0, 0);
                return;
            }
            if (VICKY == null)
            {
                e.Graphics.DrawString("IO Memory Not Initialized", this.Font, TextBrush, 0, 0);
                return;
            }
            if (VRAM == null)
            {
                e.Graphics.DrawString("VRAM Not Initialized", this.Font, TextBrush, 0, 0);
                return;
            }
            if (RAM == null)
            {
                e.Graphics.DrawString("RAM Not Initialized", this.Font, TextBrush, 0, 0);
                return;
            }
            // Read the Master Control Register
            byte MCRegister = VICKY.ReadByte(0); // Reading address $AF:0000
            int top = 0; // top gets modified if error messages are displayed
            Graphics g = Graphics.FromImage(frameBuffer);
            
            if (MCRegister == 0 || (MCRegister & 0x80) == 0x80)
            {
                e.Graphics.DrawString("Graphics Mode disabled", this.Font, TextBrush, 0, 0);
                return;
            }
            else if ((MCRegister & 0x1) == 0x1)
            {
                if (ColumnsVisible < 1 || ColumnsVisible > MAX_TEXT_COLS)
                {
                    DrawTextWithBackground("ColumnsVisible invalid:" + ColumnsVisible.ToString(), g, Color.Black, 0, top);
                    top += 12;
                }
                if (LinesVisible < 1 || LinesVisible > MAX_TEXT_LINES)
                {
                    DrawTextWithBackground("LinesVisible invalid:" + LinesVisible.ToString(), g, Color.Black, 0, top);
                    top += 12;
                }
            }


            if (drawing)
            {
                // drop the frame
                return;
            }
            drawing = true;

            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            if (tileEditorMode)
            {
                g.Clear(Color.LightGray);
                DrawTextWithBackground("Tile Editing Mode", g, Color.Black, 240, 10);
                DrawTextWithBackground("Tile Editing Mode", g, Color.Black, 240, 455);
            }

            // Determine if we display a border
            byte border_register = VICKY.ReadByte(MemoryMap.BORDER_CTRL_REG - MemoryMap.VICKY_BASE_ADDR);
            bool displayBorder = (border_register & 1) == 1;

            int borderXSize = VICKY.ReadByte(MemoryMap.BORDER_X_SIZE - MemoryMap.VICKY_BASE_ADDR);
            int borderYSize = VICKY.ReadByte(MemoryMap.BORDER_Y_SIZE - MemoryMap.VICKY_BASE_ADDR);

            Rectangle rect = new Rectangle(0, 0, 640, 480);
            BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // Load the SOL register - a lines
            byte SOLRegister = VICKY.ReadByte(MemoryMap.VKY_LINE_IRQ_CTRL_REG - MemoryMap.VICKY_BASE_ADDR);
            int SOLLine0 = VICKY.ReadWord(MemoryMap.VKY_LINE0_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR);
            int SOLLine1 = VICKY.ReadWord(MemoryMap.VKY_LINE1_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR);

            for (int line = 0; line < 480; line++)
            {
                // Handle SOL interrupts
                if (StartOfLine != null && (SOLRegister & 1) == 1 && line == SOLLine0)
                {
                    StartOfLine.Invoke();
                }
                if (StartOfLine != null && (SOLRegister & 2) == 2 && line == SOLLine1)
                {
                    StartOfLine.Invoke();
                }

                // implement a LUT cache - this must be done every time a line is drawn
                // Load Graphical LUTs
                int[] graphicsLUT = LoadLUT(VICKY);

                byte[] gammaCorrection = null;
                // Apply gamma correct
                if ((MCRegister & 0x40) == 0x40)
                {
                    gammaCorrection = LoadGammaCorrection(VICKY);
                }

                // Default background color to border color
                // In Text mode, the border color is stored at $AF:0005.
                byte borderRed = VICKY.ReadByte(5);
                byte borderGreen = VICKY.ReadByte(6);
                byte borderBlue = VICKY.ReadByte(7);
                if (gammaCorrection != null)
                {
                    borderRed = gammaCorrection[0x200 + borderRed];
                    borderGreen = gammaCorrection[0x100 + borderGreen];
                    borderBlue = gammaCorrection[borderBlue];
                }
                int borderColor = (int)(0xFF000000 + (borderBlue << 16) + (borderGreen << 8) + borderRed);

                if (tileEditorMode)
                {
                    borderColor = Color.LightGray.ToArgb();
                }
                int offset = line * 640;
                IntPtr p = bitmapData.Scan0;
                if (line < borderYSize || line > 480 - borderYSize)
                {
                    for (int x = 0; x < 640; x++)
                    {
                        System.Runtime.InteropServices.Marshal.WriteInt32(p, (offset + x) * 4, borderColor);
                    }
                }
                else
                {
                    // Graphics Mode
                    int backgroundColor = unchecked((int)0xFF000000);
                    if ((MCRegister & 0x4) == 0x4)
                    {
                        byte backRed = VICKY.ReadByte(MemoryMap.BACKGROUND_COLOR_B - MemoryMap.VICKY_BASE_ADDR);
                        byte backGreen = VICKY.ReadByte(MemoryMap.BACKGROUND_COLOR_G - MemoryMap.VICKY_BASE_ADDR);
                        byte backBlue = VICKY.ReadByte(MemoryMap.BACKGROUND_COLOR_R - MemoryMap.VICKY_BASE_ADDR);
                        if (gammaCorrection != null)
                        {
                            backRed = gammaCorrection[0x200 + backRed];
                            backGreen = gammaCorrection[0x100 + backGreen];
                            backBlue = gammaCorrection[backBlue];
                        }
                        backgroundColor = (int)(0xFF000000 + (backBlue << 16) + (backGreen << 8) + backRed);
                    }

                    for (int x = 0; x < 640; x++)
                    {
                        int resetValue = x < borderXSize || x > 640 - borderXSize ? borderColor : backgroundColor;
                        System.Runtime.InteropServices.Marshal.WriteInt32(p, (offset + x) * 4, resetValue);
                    }

                    // Bitmap Mode
                    if ((MCRegister & 0x4) == 0x4 || tileEditorMode)
                    {
                        if ((MCRegister & 0x8) == 0x8)
                        {
                            DrawBitmap(ref bitmapData, ref graphicsLUT, ref gammaCorrection, displayBorder, backgroundColor, borderXSize, borderYSize, line);
                        }

                        for (int layer = 4; layer > 0; --layer)
                        {
                            if ((MCRegister & 0x10) == 0x10)
                            {
                                DrawTiles(ref bitmapData, ref graphicsLUT, ref gammaCorrection, layer - 1, displayBorder, borderXSize, borderYSize, line);
                            }
                            if ((MCRegister & 0x20) == 0x20)
                            {
                                DrawSprites(ref bitmapData, ref graphicsLUT, ref gammaCorrection, layer - 1, displayBorder, borderXSize, borderYSize, line);
                            }
                        }
                    }
                    if ((MCRegister & 7) == 0x1 || (MCRegister & 7) == 3 || (MCRegister & 7) == 7)
                    {
                        if (top == 0)
                        {
                            DrawBitmapText(ref bitmapData, ref gammaCorrection, borderXSize, borderYSize, line);
                        }
                    }
                }
                if (!TileEditorMode)
                {
                    DrawMouse(ref bitmapData, ref graphicsLUT, ref gammaCorrection, line);
                }
                
            }
            frameBuffer.UnlockBits(bitmapData);
            e.Graphics.DrawImage(frameBuffer, ClientRectangle);
            drawing = false;

            // Check if SOF is enabled
            if (MCRegister != 0 && MCRegister != 0x80)
            {
                StartOfFrame?.Invoke();
            }
        }

        public static byte[] LoadGammaCorrection(MemoryRAM VKY)
        {
            // Read the color lookup tables
            int gammaAddress = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
            // 
            byte[] result = new byte[3*256];
            VKY.CopyIntoBuffer(gammaAddress, result, 0, 3 * 256);
            return result;
        }

        public static int[] LoadLUT(MemoryRAM VKY)
        {
            // Read the color lookup tables
            int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
            int lookupTables = 4;
            int[] result = new int[lookupTables * 256];


            for (int c = 0; c < lookupTables * 256; c++)
            {
                byte blue = VKY.ReadByte(lutAddress++);
                byte green = VKY.ReadByte(lutAddress++);
                byte red = VKY.ReadByte(lutAddress++);
                lutAddress++; // skip the alpha channel
                result[c] = (0xFF << 24) + (red << 16) + (green << 8) + blue;
            }
            return result;
        }

        int lastWidth = 0;
        private void DrawBitmapText(ref BitmapData bd, ref byte[] gammaCorrection, int colOffset, int rowOffset, int line)
        {
            if (lastWidth != ColumnsVisible
                && ColumnsVisible > 0
                && LinesVisible > 0)
            {
                lastWidth = ColumnsVisible;
            }
            bool overlayBitSet = (VICKY.ReadByte(0) & 0x02) == 0x02;

            // We're hard-coding this for now.
            int lines = LinesVisible;
            
            // Read the color lookup tables
            int fgLUT = MemoryLocations.MemoryMap.FG_CHAR_LUT_PTR - VICKY.StartAddress;
            int bgLUT = MemoryLocations.MemoryMap.BG_CHAR_LUT_PTR - VICKY.StartAddress;

            int colorStart = MemoryLocations.MemoryMap.SCREEN_PAGE1 - VICKY.StartAddress;
            int lineStart = MemoryLocations.MemoryMap.SCREEN_PAGE0 - VICKY.StartAddress;
            int fontBaseAddress = MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START - VICKY.StartAddress;
            IntPtr p = bd.Scan0;
            int stride = bd.Stride;

            // Find which line of characters to display
            int txtline = (line - rowOffset) / charHeight;
            
            
            for (int col = 0; col < ColumnsVisible; col++)
            {
                int x = col * charWidth + colOffset;
                if (x > 639 - colOffset)
                {
                    continue;
                }

                int textAddr = lineStart + COLS_PER_LINE * txtline + col;
                int colorAddr = colorStart + COLS_PER_LINE * txtline + col;

                // Each character will have foreground and background colors
                byte character = VICKY.ReadByte(textAddr);
                if (CursorX == col && CursorY == txtline && CursorState && CursorEnabled)
                {
                    character = VICKY.ReadByte(MemoryLocations.MemoryMap.VKY_TXT_CURSOR_CHAR_REG - VICKY.StartAddress);
                }
                byte color = VICKY.ReadByte(colorAddr);
                byte fgColor = (byte)((color & 0xF0) >> 4);
                byte bgColor = (byte)(color & 0x0F);

                // In order to reduce the load of applying Gamma correction, load single bytes
                byte fgValueRed = VICKY.ReadByte(fgLUT + fgColor * 4);
                byte fgValueGreen = VICKY.ReadByte(fgLUT + fgColor * 4 + 1);
                byte fgValueBlue = VICKY.ReadByte(fgLUT + fgColor * 4 + 2);

                byte bgValueRed = VICKY.ReadByte(bgLUT + bgColor * 4);
                byte bgValueGreen = VICKY.ReadByte(bgLUT + bgColor * 4 + 1);
                byte bgValueBlue = VICKY.ReadByte(bgLUT + bgColor * 4 + 2);

                if (gammaCorrection != null)
                {
                    fgValueBlue = gammaCorrection[fgValueBlue];
                    fgValueGreen = gammaCorrection[0x100 + fgValueGreen];
                    fgValueRed = gammaCorrection[0x200 + fgValueRed];

                    bgValueBlue = gammaCorrection[bgValueBlue];
                    bgValueGreen = gammaCorrection[0x100 + bgValueGreen];
                    bgValueRed = gammaCorrection[0x200 + bgValueRed];
                }
                int fgValue = (int)((fgValueBlue << 16) + (fgValueGreen << 8) + fgValueRed + 0xFF000000);
                int bgValue = (int)((bgValueBlue << 16) + (bgValueGreen << 8) + bgValueRed + 0xFF000000);

                // Each character is defined by 8 bytes
                int fontLine = (line - rowOffset) % charHeight;
                    
                int offset = (x + line* 640 ) * 4;
                byte value = VICKY.ReadByte(fontBaseAddress + character * 8 + fontLine);
                // TODO: this is pretty bad - rewrite it
                if ((value & 0x80) == 0x80)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset, bgValue);
                }
                if ((value & 0x40) == 0x40)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 4, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 4, bgValue);
                }
                if ((value & 0x20) == 0x20)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 8, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 8, bgValue);
                }
                if ((value & 0x10) == 0x10)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 12, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 12, bgValue);
                }

                // Low nibble
                if ((value & 0x8) == 0x8)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 16, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 16, bgValue);
                }
                if ((value & 0x4) == 0x4)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 20, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 20, bgValue);
                }
                if ((value & 0x2) == 0x2)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 24, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 24, bgValue);
                }
                if ((value & 0x1) == 0x1)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 28, fgValue);
                }
                else if (!overlayBitSet)
                {
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, offset + 28, bgValue);
                }
            }
        }

        private void DrawBitmap(ref BitmapData bd, ref int[] graphicsLUT, ref byte[] gammaCorrection, bool bkgrnd, int bgndColor, int borderXSize, int borderYSize, int line)
        {
            // Bitmap Controller is located at $AF:0140
            int reg = VICKY.ReadByte(MemoryMap.BITMAP_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_BASE_ADDR);
            if ((reg & 0x01) == 00)
            {
                return;
            }
            int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs

            int bitmapAddress = VICKY.ReadLong(0xAF_0141 - MemoryMap.VICKY_BASE_ADDR);
            int width = VICKY.ReadWord(0xAF_0144 - MemoryMap.VICKY_BASE_ADDR);
            int height = VICKY.ReadWord(0xAF_0146 - MemoryMap.VICKY_BASE_ADDR);

            IntPtr p = bd.Scan0;
            int value = 0;
            int offset = bitmapAddress + line * 640;
            for (int col = borderXSize; col < (width - borderXSize); col++)
            {
                value = VRAM.ReadByte(offset + col);
                if (value == 0)
                {
                    value = bgndColor;
                }
                else
                {
                    value = graphicsLUT[lutIndex * 256 + value];
                }
                if (gammaCorrection != null )
                {
                    value = (int)((gammaCorrection[(value & 0x00FF0000) >> 0x10] << 0x10) +
                                    (gammaCorrection[0x100 + ((value & 0x0000FF00) >> 0x08)] << 0x08) +
                                    (gammaCorrection[0x200 + (value & 0x000000FF)]) + 0xFF000000);
                }

                System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * width + col) * 4, value);
            }
        }

        private void DrawTiles(ref BitmapData bd, ref int[] graphicsLUT, ref byte[] gammaCorrection, int layer, bool bkgrnd, int borderXSize, int borderYSize, int line)
        {
            // There are four possible tilesets to choose from
            int addrTileset = MemoryMap.TILE_CONTROL_REGISTER_ADDR + layer * 8;
            int reg = VICKY.ReadByte(addrTileset - MemoryMap.VICKY_BASE_ADDR);
            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00)
            {
                return;
            }
            // This is hard coded for now
            int lines = 52;
            int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs
            bool striding = (reg & 0x80) == 0x80;

            int tilesetAddress = VICKY.ReadLong(addrTileset + 1 - MemoryMap.VICKY_BASE_ADDR);
            int strideX = striding ? 256 : VICKY.ReadWord(addrTileset + 4 - MemoryMap.VICKY_BASE_ADDR);
            int strideY = VICKY.ReadWord(addrTileset + 6 - MemoryMap.VICKY_BASE_ADDR);

            // Now read the tilemap
            int tilemapAddress = 0xAF5000 + 0x800 * layer;
            IntPtr p = bd.Scan0;

            int colOffset = bkgrnd ? (80 - ColumnsVisible) / 2 * charWidth / tileSize: 0;
            int lineOffset = bkgrnd ? (60 - lines) / 2 * charHeight / tileSize : 0;

            for (int tileRow = lineOffset; tileRow < (30 - lineOffset); tileRow++)
            {
                if (tileRow * 16 < borderYSize || tileRow * 16 > (480 - borderYSize)) continue;
                for (int tileCol = colOffset; tileCol < (40 - colOffset); tileCol++)
                {
                    if (tileCol * 16 < borderXSize || (tileCol + 1) * 16 > (640 - borderXSize)) continue;
                    int tile = VICKY.ReadByte(tilemapAddress + tileCol + tileRow * 64 - MemoryMap.VICKY_BASE_ADDR);
                    int pixelIndex = 0;
                    int value = 0;

                    // Tiles are 16 x 16
                    for (int tline = 0; tline < 16; tline++)
                    {
                        int offset = tilesetAddress + ((tile / 16) * 256 * 16 + (tile % 16) * 16) + tline * strideX;
                        for (int col = 0; col < 16; col++)
                        {
                            // Lookup the pixel in the tileset
                            pixelIndex = VRAM.ReadByte(offset + col );
                            if (pixelIndex != 0)
                            {
                                value = (int)graphicsLUT[lutIndex * 256 + pixelIndex];
                                if (gammaCorrection != null)
                                {
                                    //value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                                    value = (int)((gammaCorrection[(value & 0x00FF0000) >> 0x10] << 0x10) +
                                                  (gammaCorrection[0x100 + ((value & 0x0000FF00) >> 0x08)] << 0x08) +
                                                  (gammaCorrection[0x200 + (value & 0x000000FF)]) + 0xFF000000);
                                }
                                
                                System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * 640 + col + tileCol * 16 + tileRow * 16 * 640) * 4, value);
                            }
                        }
                    }
                }
            }
        }

        private void DrawSprites(ref BitmapData bd, ref int[] graphicsLUT, ref byte[] gammaCorrection, int layer, bool bkgrnd, int borderXSize, int borderYSize, int line)
        {
            // There are 32 possible sprites to choose from.
            for (int s = 0; s < 32; s++)
            {
                int addrSprite = MemoryMap.SPRITE_CONTROL_REGISTER_ADDR + s * 8;
                int reg = VICKY.ReadByte(addrSprite - MemoryMap.VICKY_BASE_ADDR);
                // if the set is not enabled, we're done.
                int spriteLayer = (reg & 0x70) >> 4;
                int posY = VICKY.ReadWord(addrSprite + 6 - MemoryMap.VICKY_BASE_ADDR);
                if ((reg & 1) == 1 && layer == spriteLayer && (line >= posY && line < posY + 32))
                {

                    int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs
                    bool striding = (reg & 0x80) == 0x80;

                    int spriteAddress = VICKY.ReadLong(addrSprite + 1 - MemoryMap.VICKY_BASE_ADDR);
                    int posX = VICKY.ReadWord(addrSprite + 4 - MemoryMap.VICKY_BASE_ADDR);
                    
                    
                    if (posX >= (640 - borderXSize) || posY >= (480 - borderYSize) || posX < 0 || posY < 0)
                    {
                        continue;
                    }
                    int spriteWidth = 32;
                    int xOffset = 0;
                    // Check for sprite bleeding on the left-hand-side
                    if (posX < borderXSize)
                    {
                        xOffset = borderXSize - posX;
                        posX = borderXSize;
                        spriteWidth = 32 - xOffset;
                        if (spriteWidth == 0)
                        {
                            continue;
                        }
                    }
                    // Check for sprite bleeding on the right-hand side
                    if (posX + 32 > 640 - borderXSize)
                    {
                        spriteWidth = 640 - borderXSize - posX;
                        if (spriteWidth == 0)
                        {
                            continue;
                        }
                    }

                    IntPtr p = bd.Scan0;

                    int value = 0;
                    int pixelIndex = 0;

                    // Sprites are 32 x 32
                    int sline = line - posY;
                    {
                        int lineOffset = (sline + posY) * 640;
                        for (int col = xOffset; col < xOffset + spriteWidth; col++)
                        {
                            // Lookup the pixel in the tileset
                            pixelIndex = VRAM.ReadByte(spriteAddress + col + sline * 32);
                            if (pixelIndex != 0)
                            {
                                value = (int)graphicsLUT[(lutIndex + 1) * 256 + pixelIndex];
                                if (gammaCorrection != null)
                                {
                                    //value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                                    value = (int)((gammaCorrection[(value & 0x00FF0000) >> 0x10] << 0x10) +
                                                  (gammaCorrection[0x100 + ((value & 0x0000FF00) >> 0x08)] << 0x08) +
                                                  (gammaCorrection[0x200 + (value & 0x000000FF)]) + 0xFF000000);
                                }

                                System.Runtime.InteropServices.Marshal.WriteInt32(p, (lineOffset + (col-xOffset + posX)) * 4, value);
                            }
                        }
                    }
                }
            }
        }

        private void DrawMouse(ref BitmapData bd, ref int[] graphicsLUT, ref byte[] gammaCorrection, int line)
        {
            int X = VICKY.ReadWord(0x702);
            int PosY = VICKY.ReadWord(0x704);

            byte mouseReg = VICKY.ReadByte(0x700);
            bool MousePointerEnabled = (mouseReg & 1) == 1;
            if (MousePointerEnabled && line >= PosY && line < PosY + 16)
            {

                int pointerAddress = 0xAF_0500 - VICKY.StartAddress;
                if ((mouseReg & 2) == 2)
                {
                    pointerAddress += 0x100;
                }
                // Mouse pointer is a 16x16 icon
                int colsToDraw = X < 640 - 16 ? 16 : 640 - X;

                IntPtr p = bd.Scan0;
                int mline = line - PosY;
                for (int col = 0; col < colsToDraw; col++)
                {
                    // Values are 0: transparent, 1:black, 255: white (gray scales)
                    byte pixelIndexR = VICKY.ReadByte(pointerAddress + mline * 16 + col);
                    byte pixelIndexG = pixelIndexR;
                    byte pixelIndexB = pixelIndexR;
                    if (pixelIndexR != 0)
                    {
                        if (gammaCorrection != null)
                        {
                            pixelIndexB = gammaCorrection[pixelIndexR];
                            pixelIndexG = gammaCorrection[0x100 + pixelIndexR];
                            pixelIndexR = gammaCorrection[0x200 + pixelIndexR];
                        }
                        int value = (int)((pixelIndexB << 16) + (pixelIndexG << 8) + pixelIndexR + 0xFF000000);
                        System.Runtime.InteropServices.Marshal.WriteInt32(p, ((mline + PosY) * 640 + col + X) * 4, value);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a font set into RAM and adds it to the character set table.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Filename"></param>
        public void LoadFontSet(string Name, string Filename, int Offset, CharacterSet.CharTypeCodes CharType, CharacterSet.SizeCodes CharSize)
        {
            CharacterSet cs = new CharacterSet();
            // Load the data from the file into the  IO buffer - starting at address $AF8000
            cs.Load(Filename, Offset, VICKY, MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START & 0xffff, CharSize);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        bool CursorState = true;
        private bool CursorEnabled
        {
            get
            {
                if (RAM == null)
                    return false;

                return (VICKY.ReadByte(MemoryMap.VKY_TXT_CURSOR_CTRL_REG - MemoryMap.VICKY_START) & 1) == 1;
            }
        }

        public bool TileEditorMode
        {
            get
            {
                return tileEditorMode;
            }
            set
            {
                tileEditorMode = value;
            }
        }

        private int GetCharPos(int row, int col)
        {
            if (RAM == null)
                return 0;
            int baseAddress = RAM.ReadLong(MemoryMap.SCREENBEGIN);
            return baseAddress + row * COLS_PER_LINE + col;
        }

        /// <summary>
        /// Column of the cursor position. 0 is left edge
        /// </summary>
        [Browsable(false)]
        public int CursorX
        {
            get
            {
                if (RAM == null)
                    return 0;

                return RAM.ReadByte(MemoryMap.CURSORX);
            }
        }

        /// <summary>
        /// Row of cursor position. 0 is top of the screen
        /// </summary>
        [Browsable(false)]
        public int CursorY
        {
            get
            {
                if (RAM == null)
                    return 0;

                return RAM.ReadByte(MemoryMap.CURSORY);
            }
        }

        [Browsable(false)]
        public int ColumnsVisible
        {
            get
            {
                if (RAM == null)
                    return 0;

                return RAM.ReadByte(MemoryMap.COLS_VISIBLE);
            }
        }

        [Browsable(false)]
        public int LinesVisible
        {
            get
            {
                if (RAM == null)
                    return 0;
                return RAM.ReadByte(MemoryMap.LINES_VISIBLE);
            }
        }

        public int COLS_PER_LINE
        {
            get
            {
                if (RAM == null)
                    return 0;

                return RAM.ReadByte(MemoryMap.COLS_PER_LINE);
            }
        }

        /// <summary>
        /// Memory offset of the cursor position on the screen. The top-left corner is the first memory location
        /// of the screen. 
        /// </summary>
        [Browsable(false)]
        public int CursorPos
        {
            get
            {
                if (RAM == null)
                    return 0;
                return RAM.ReadWord(MemoryMap.CURSORPOS);
            }
        }
    }
}
