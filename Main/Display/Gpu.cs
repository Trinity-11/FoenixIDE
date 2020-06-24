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

        public delegate void GpuUpdateFunction();
        public GpuUpdateFunction GpuUpdated;

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
        private int[] lutCache = new int[256 * 4]; // 4 LUTs

        public Gpu()
        {
            InitializeComponent();
            this.Load += new EventHandler(Gpu_Load);
        }

        void Gpu_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(Gpu_Paint);
            this.DoubleBuffered = true;
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
            if (BlinkingCounter == 0)
            {
                GpuUpdated?.Invoke();
            }
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
        unsafe void Gpu_Paint(object sender, PaintEventArgs e)
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
            //Graphics g = Graphics.FromImage(frameBuffer);
            Graphics g = e.Graphics;
            byte ColumnsVisible = RAM.ReadByte(MemoryMap.COLS_VISIBLE);
            byte LinesVisible = RAM.ReadByte(MemoryMap.LINES_VISIBLE);
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
                System.Console.WriteLine("Skipped Frame");
                return;
            }
            drawing = true;

            // Check if SOF is enabled
            if (MCRegister != 0 && MCRegister != 0x80)
            {
                StartOfFrame?.Invoke();
            }

            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            // Determine if we display a border
            byte border_register = VICKY.ReadByte(MemoryMap.BORDER_CTRL_REG - MemoryMap.VICKY_BASE_ADDR);
            bool displayBorder = (border_register & 1) == 1;

            int borderXSize = VICKY.ReadByte(MemoryMap.BORDER_X_SIZE - MemoryMap.VICKY_BASE_ADDR);
            int borderYSize = VICKY.ReadByte(MemoryMap.BORDER_Y_SIZE - MemoryMap.VICKY_BASE_ADDR);

            Rectangle rect = new Rectangle(0, 0, 640, 480);
            BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bitmapPointer = (int*)bitmapData.Scan0.ToPointer();

            // Load the SOL register - a lines
            byte SOLRegister = VICKY.ReadByte(MemoryMap.VKY_LINE_IRQ_CTRL_REG - MemoryMap.VICKY_BASE_ADDR);
            int SOLLine0 = VICKY.ReadWord(MemoryMap.VKY_LINE0_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR);
            int SOLLine1 = VICKY.ReadWord(MemoryMap.VKY_LINE1_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR);

            for (int line = 0; line < 480; line++)
            {
                // Handle SOL interrupts
                if ( ((SOLRegister & 1) == 1) && line == SOLLine0)
                {
                    StartOfLine?.Invoke();
                }
                if ( ((SOLRegister & 2) == 2) && line == SOLLine1)
                {
                    StartOfLine?.Invoke();
                }

                // Reset LUT Cache
                lutCache.Initialize();
                
                bool gammaCorrection = (MCRegister & 0x40) == 0x40;

                // Default background color to border color
                // In Text mode, the border color is stored at $AF:0005.
                byte borderRed = VICKY.ReadByte(5);
                byte borderGreen = VICKY.ReadByte(6);
                byte borderBlue = VICKY.ReadByte(7);
                if (gammaCorrection)
                {
                    borderRed = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + borderRed); //gammaCorrection[0x200 + borderGreen];
                    borderGreen = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + borderGreen); //gammaCorrection[0x100 + borderGreen];
                    borderBlue = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + borderBlue); // gammaCorrection[borderBlue];
                }
                int borderColor = (int)(0xFF000000 + (borderBlue << 16) + (borderGreen << 8) + borderRed);

                if (tileEditorMode)
                {
                    borderColor = Color.LightGray.ToArgb();
                }
                //int offset = line * 640;
                int* ptr = bitmapPointer + line * 640;
                if (line < borderYSize || line > 480 - borderYSize)
                {
                    for (int x = 0; x < 640; x++)
                    {
                        //System.Runtime.InteropServices.Marshal.WriteInt32(bitmapPointer, (offset + x) * 4, borderColor);
                        ptr[x] = borderColor;
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
                        if (gammaCorrection)
                        {
                            backRed = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + backRed); // gammaCorrection[0x200 + backRed];
                            backGreen = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + backGreen); //gammaCorrection[0x100 + backGreen];
                            backBlue = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + backBlue); //gammaCorrection[backBlue];
                        }
                        backgroundColor = (int)(0xFF000000 + (backBlue << 16) + (backGreen << 8) + backRed);
                    }

                    for (int x = 0; x < 640; x++)
                    {
                        int resetValue = x < borderXSize || x > 640 - borderXSize ? borderColor : backgroundColor;
                        //System.Runtime.InteropServices.Marshal.WriteInt32(bitmapPointer, (offset + x) * 4, resetValue);
                        ptr[x] = resetValue;
                    }

                    // Bitmap Mode
                    if ((MCRegister & 0x4) == 0x4 || tileEditorMode)
                    {
                        if ((MCRegister & 0x8) == 0x8)
                        {
                            DrawBitmap(bitmapPointer, gammaCorrection, displayBorder, backgroundColor, borderXSize, borderYSize, line);
                        }

                        for (int layer = 4; layer > 0; --layer)
                        {
                            if ((MCRegister & 0x10) == 0x10)
                            {
                                DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, layer - 1, displayBorder, borderXSize, borderYSize, line);
                            }
                            if ((MCRegister & 0x20) == 0x20)
                            {
                                DrawSprites(bitmapPointer, gammaCorrection, layer - 1, displayBorder, borderXSize, borderYSize, line);
                            }
                        }
                    }
                    if ((MCRegister & 7) == 0x1 || (MCRegister & 7) == 3 || (MCRegister & 7) == 7)
                    {
                        if (top == 0)
                        {
                            DrawBitmapText(bitmapPointer, gammaCorrection, ColumnsVisible, LinesVisible, borderXSize, borderYSize, line);
                        }
                    }
                }
                if (!TileEditorMode)
                {
                    DrawMouse(bitmapPointer, gammaCorrection, line);
                }
                
            }
            frameBuffer.UnlockBits(bitmapData);
            e.Graphics.DrawImage(frameBuffer, ClientRectangle);
            drawing = false;
        }

        //public static byte[] LoadGammaCorrection_DontUse(MemoryRAM VKY)
        //{
        //    // Read the color lookup tables
        //    int gammaAddress = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
        //    // 
        //    byte[] result = new byte[3*256];
        //    VKY.CopyIntoBuffer(gammaAddress, result, 0, 3 * 256);
        //    return result;
        //}

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
                result[c] = (int)(0xFF000000 + (red << 16) + (green << 8) + blue);
            }
            return result;
        }

        // We only cache items that are requested, instead of precomputing all 1024 colors.
        private int getLUTValue(byte lutIndex, byte color)
        {
            int offset = lutIndex * 256 + color;
            if (lutCache[offset] == 0)
            {
                lutCache[offset] = (int)(0xFF000000 + VICKY.ReadLong(MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + offset * 4));
            }
            return lutCache[offset];
        }

        private unsafe void DrawBitmapText(int* p, bool gammaCorrection, byte TextColumns, byte TextRows, int colOffset, int rowOffset, int line)
        {
            bool overlayBitSet = (VICKY.ReadByte(0) & 0x02) == 0x02;

            int lines = TextRows;
            
            // Read the color lookup tables
            int fgLUTAddress = MemoryLocations.MemoryMap.FG_CHAR_LUT_PTR - VICKY.StartAddress;
            int bgLUTAddress = MemoryLocations.MemoryMap.BG_CHAR_LUT_PTR - VICKY.StartAddress;

            int colorStartAddress = MemoryLocations.MemoryMap.SCREEN_PAGE1 - VICKY.StartAddress;
            int lineStartAddress = MemoryLocations.MemoryMap.SCREEN_PAGE0 - VICKY.StartAddress;
            int fontBaseAddress = MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START - VICKY.StartAddress;

            // Find which line of characters to display
            int txtline = (line - rowOffset) / charHeight;
            byte COLS_PER_LINE = RAM.ReadByte(MemoryMap.COLS_PER_LINE);

            // Cursor Values
            byte CursorY = RAM.ReadByte(MemoryMap.CURSORY);
            byte CursorX = RAM.ReadByte(MemoryMap.CURSORX);
            bool CursorEnabled = (VICKY.ReadByte(MemoryMap.VKY_TXT_CURSOR_CTRL_REG - MemoryMap.VICKY_START) & 1) != 0;

            // Each character is defined by 8 bytes
            int fontLine = (line - rowOffset) % charHeight;
            int* ptr = p + line * 640 + colOffset;
            for (int col = 0; col < TextColumns; col++)
            {
                int x = col * charWidth;
                if (x + colOffset > 639 - colOffset)
                {
                    continue;
                }
                
                int textAddr = lineStartAddress + COLS_PER_LINE * txtline + col;
                int colorAddr = colorStartAddress + COLS_PER_LINE * txtline + col;

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
                byte fgValueRed = VICKY.ReadByte(fgLUTAddress + fgColor * 4);
                byte fgValueGreen = VICKY.ReadByte(fgLUTAddress + fgColor * 4 + 1);
                byte fgValueBlue = VICKY.ReadByte(fgLUTAddress + fgColor * 4 + 2);

                byte bgValueRed = VICKY.ReadByte(bgLUTAddress + bgColor * 4);
                byte bgValueGreen = VICKY.ReadByte(bgLUTAddress + bgColor * 4 + 1);
                byte bgValueBlue = VICKY.ReadByte(bgLUTAddress + bgColor * 4 + 2);

                if (gammaCorrection)
                {
                    fgValueBlue = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + fgValueBlue); //gammaCorrection[fgValueBlue];
                    fgValueGreen = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + fgValueGreen);//gammaCorrection[0x100 + fgValueGreen];
                    fgValueRed = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + fgValueRed);//gammaCorrection[0x200 + fgValueRed];

                    bgValueBlue = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + bgValueBlue); //gammaCorrection[bgValueBlue];
                    bgValueGreen = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + bgValueGreen); //gammaCorrection[0x100 + bgValueGreen];
                    bgValueRed = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + bgValueRed); //gammaCorrection[0x200 + bgValueRed];
                }
                int fgValue = (int)((fgValueBlue << 16) + (fgValueGreen << 8) + fgValueRed + 0xFF000000);
                int bgValue = (int)((bgValueBlue << 16) + (bgValueGreen << 8) + bgValueRed + 0xFF000000);

                byte value = VICKY.ReadByte(fontBaseAddress + character * 8 + fontLine);
                //int offset = (x + line * 640) * 4;

                // For each bit in the font, set the foreground color - if the bit is 0 and overlay is set, skip it (keep the background)
                for (int b = 0x80; b >0; b = b >> 1)
                {
                    if ((value & b) != 0)
                    {
                        //System.Runtime.InteropServices.Marshal.WriteInt32(p, offset, fgValue);
                        ptr[0] = fgValue;
                    }
                    else if (!overlayBitSet)
                    {
                        //System.Runtime.InteropServices.Marshal.WriteInt32(p, offset, bgValue);
                        ptr[0] = bgValue;
                    }
                    ptr++;
                    //offset += 4;
                }
            }
        }

        private unsafe void DrawBitmap(int* p, bool gammaCorrection, bool bkgrnd, int bgndColor, int borderXSize, int borderYSize, int line)
        {
            
            // Bitmap Controller is located at $AF:0140
            byte reg = VICKY.ReadByte(MemoryMap.BITMAP_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_BASE_ADDR);
            if ((reg & 0x01) == 00)
            {
                return;
            }
            byte lutIndex = (byte)((reg & 14) >> 1);  // 8 possible LUTs

            int bitmapAddress = VICKY.ReadLong(0xAF_0141 - MemoryMap.VICKY_BASE_ADDR);
            int width = VICKY.ReadWord(0xAF_0144 - MemoryMap.VICKY_BASE_ADDR);
            int height = VICKY.ReadWord(0xAF_0146 - MemoryMap.VICKY_BASE_ADDR);

            int colorVal = 0;
            byte pixVal = 0;
            int offsetAddress = bitmapAddress + line * 640;
            int pixelOffset = line * width;
            int* ptr = p + pixelOffset;
            for (int col = borderXSize; col < (width - borderXSize); col++)
            {
                pixVal = VRAM.ReadByte(offsetAddress + col);
                colorVal = pixVal == 0 ? bgndColor : getLUTValue(lutIndex, pixVal);
                if (gammaCorrection)
                {
                    colorVal = (int)((VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + (colorVal & 0x00FF0000) >> 0x10) << 0x10) +
                                    (VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + ((colorVal & 0x0000FF00) >> 0x08)) << 0x08) +
                                    (VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + (colorVal & 0x000000FF))) + 0xFF000000);
                }

                //System.Runtime.InteropServices.Marshal.WriteInt32(p, (pixelOffset + col) * 4, colorVal);
                ptr[col] = colorVal;
            }
        }

        private unsafe void DrawTiles(int* p, bool gammaCorrection, byte TextColumns, int layer, bool bkgrnd, int borderXSize, int borderYSize, int line)
        {
            // There are four possible tilesets to choose from
            int addrTileset = MemoryMap.TILE_CONTROL_REGISTER_ADDR + layer * 8 - MemoryMap.VICKY_BASE_ADDR;
            int reg = VICKY.ReadByte(addrTileset);
            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00 || line < borderYSize || line > 480 - borderYSize)
            {
                return;
            }
            // This is hard coded for now
            int lines = 52;
            byte lutIndex = (byte)((reg & 14) >> 1);  // 8 possible LUTs
            bool striding = (reg & 0x80) == 0x80;

            int tilesetAddress = VICKY.ReadLong(addrTileset + 1 );
            int strideX = striding ? 256 : VICKY.ReadWord(addrTileset + 4);
            int strideY = VICKY.ReadWord(addrTileset + 6);

            int colOffset = bkgrnd ? (80 - TextColumns) / 2 * charWidth / tileSize: 0;
            int lineOffset = bkgrnd ? (60 - lines) / 2 * charHeight / tileSize : 0;

            int tileRow = line / 16;
            int tilemapAddress = 0xAF5000 + 0x800 * layer + tileRow * 64 - MemoryMap.VICKY_BASE_ADDR;


            //if (tileRow * 16 < borderYSize || tileRow * 16 > (480 - borderYSize)) continue;
            for (int tileCol = colOffset; tileCol < (40 - colOffset); tileCol++)
            {
                if (tileCol * 16 < borderXSize || (tileCol + 1) * 16 > (640 - borderXSize)) continue;
                int tile = VICKY.ReadByte(tilemapAddress + tileCol);
                byte pixelIndex = 0;
                int value = 0;

                // Tiles are 16 x 16
                int tline = line % 16;

                int offsetAddress = tilesetAddress + ((tile / 16) * 256 * 16 + (tile % 16) * 16) + tline * strideX;
                int pixelOffset = tline * 640 + tileCol * 16 + tileRow * 16 * 640;
                int* ptr = p + pixelOffset;
                for (int col = 0; col < 16; col++)
                {
                    // Lookup the pixel in the tileset - if the value is 0, then it's transparent
                    pixelIndex = VRAM.ReadByte(offsetAddress + col );
                    if (pixelIndex != 0)
                    {
                        //value = (int)graphicsLUT[lutIndex * 256 + pixelIndex];
                        value = getLUTValue(lutIndex, pixelIndex);
                        if (gammaCorrection)
                        {
                            value = (int)((VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + (value & 0x00FF0000) >> 0x10) << 0x10) +
                                            (VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + ((value & 0x0000FF00) >> 0x08)) << 0x08) +
                                            (VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + (value & 0x000000FF))) + 0xFF000000);
                        }

                        //System.Runtime.InteropServices.Marshal.WriteInt32(p, (pixelOffset + col) * 4, value);
                        ptr[col] = value;
                    }
                }
            }
        }

        private unsafe void DrawSprites(int* p, bool gammaCorrection, int layer, bool bkgrnd, int borderXSize, int borderYSize, int line)
        {
            // There are 32 possible sprites to choose from.
            for (int s = 0; s < 32; s++)
            {
                int addrSprite = MemoryMap.SPRITE_CONTROL_REGISTER_ADDR + s * 8 - MemoryMap.VICKY_BASE_ADDR;
                int reg = VICKY.ReadByte(addrSprite);
                // if the set is not enabled, we're done.
                int spriteLayer = (reg & 0x70) >> 4;
                int posY = VICKY.ReadWord(addrSprite + 6);
                if ((reg & 1) == 1 && layer == spriteLayer && (line >= posY && line < posY + 32))
                {
                    // TODO Fix this when Vicky II fixes the LUT issue
                    byte lutIndex = (byte)(((reg & 14) >> 1) + 1);  // 8 possible LUTs  -- sprite LUT is off by 1
                    bool striding = (reg & 0x80) == 0x80;

                    int spriteAddress = VICKY.ReadLong(addrSprite + 1);
                    int posX = VICKY.ReadWord(addrSprite + 4);


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

                    int value = 0;
                    byte pixelIndex = 0;

                    // Sprites are 32 x 32
                    int sline = line - posY;
                    int lineOffset = line * 640;
                    int* ptr = p + lineOffset;
                    for (int col = xOffset; col < xOffset + spriteWidth; col++)
                    {
                        // Lookup the pixel in the tileset - if the value is 0, it's transparent
                        pixelIndex = VRAM.ReadByte(spriteAddress + col + sline * 32);
                        if (pixelIndex != 0)
                        {
                            value = getLUTValue(lutIndex, pixelIndex);
                            if (gammaCorrection)
                            {
                                value = (int)((VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + (value & 0x00FF0000) >> 0x10) << 0x10) +
                                                (VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + ((value & 0x0000FF00) >> 0x08)) << 0x08) +
                                                (VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + (value & 0x000000FF))) + 0xFF000000);
                            }

                            //System.Runtime.InteropServices.Marshal.WriteInt32(p, (lineOffset + (col-xOffset + posX)) * 4, value);
                            ptr[col - xOffset + posX] = value;
                        }
                    }
                }
            }
        }

        private unsafe void DrawMouse(int* p, bool gammaCorrection, int line)
        {
            int PosX = VICKY.ReadWord(0x702);
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
                int colsToDraw = PosX < 640 - 16 ? 16 : 640 - PosX;

                int mline = line - PosY;
                int* ptr = p + line * 640;
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
                            pixelIndexB = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + pixelIndexR); // gammaCorrection[pixelIndexR];
                            pixelIndexG = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + pixelIndexR); //gammaCorrection[0x100 + pixelIndexR];
                            pixelIndexR = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + pixelIndexR); //gammaCorrection[0x200 + pixelIndexR];
                        }
                        int value = (int)((pixelIndexB << 16) + (pixelIndexG << 8) + pixelIndexR + 0xFF000000);
                        //System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * 640 + col + X) * 4, value);
                        ptr[col + PosX] = value;
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
    }
}
