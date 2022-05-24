﻿using System;
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
using System.Threading.Tasks;

namespace FoenixIDE.Display
{
    public unsafe partial class Gpu : UserControl
    {

        private const int REGISTER_BLOCK_SIZE = 256;
        const int MAX_TEXT_COLS = 100;
        const int MAX_TEXT_LINES = 75;
        const int SCREEN_PAGE_SIZE = 128 * 64;

        const int CHAR_WIDTH = 8;
        const int CHAR_HEIGHT = 8;
        const int SPRITE_SIZE = 32;

        public MemoryRAM VRAM = null;
        public MemoryRAM RAM = null;
        public MemoryRAM VICKY = null;
        public int paintCycle = 0;
        private bool tileEditorMode = false;

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
        // In debug mode, draw the screen twice per second, instead of 60 times.
        private MultimediaTimer hiresTimer = new MultimediaTimer(500);
        private static readonly int[] vs = new int[256 * 8];
        private int[] lutCache = vs;

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

        public void setRefreshPeriod(uint time)
        {
            hiresTimer.Interval = time;
            hiresTimer.Start();
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

        public Point GetScreenSize()
        {
            byte MCRHigh = (byte)(VICKY.ReadByte(1) & 3); // Reading address $AF:0001

            Point p = new Point(640, 480);
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
            return p;
        }

        const int STRIDE = 800;
        Bitmap frameBuffer = new Bitmap(STRIDE, 600, PixelFormat.Format32bppArgb);
        private bool drawing = false;
        byte[] pixVals = null;

        /// <summary>
        /// Draw the frame buffer to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        unsafe void Gpu_Paint(object sender, PaintEventArgs e)
        {
            paintCycle++;
            if (DesignMode)
            {
                e.Graphics.DrawString("Design Mode", this.Font, TextBrush, 0, 0);
                return;
            }

            // Read the Master Control Register
            byte MCRegister = VICKY.ReadByte(0); // Reading address $AF:0000
            byte MCRHigh = (byte)(VICKY.ReadByte(1) & 3); // Reading address $AF:0001

            int resX = 640;
            int resY = 480;
            switch (MCRHigh)
            {
                case 1:
                    resX = 800;
                    resY = 600;
                    break;
                case 2:
                    resX = 320;
                    resY = 240;
                    break;
                case 3:
                    resX = 400;
                    resY = 300;
                    break;
            }

            pixVals = new byte[resX];
            int top = 0; // top gets modified if error messages are displayed
            //Graphics g = Graphics.FromImage(frameBuffer);
            Graphics g = e.Graphics;
            byte ColumnsVisible = (byte)(resX / CHAR_WIDTH); //byte ColumnsVisible = RAM.ReadByte(MemoryMap.COLS_VISIBLE);
            byte LinesVisible = (byte)(resY / CHAR_HEIGHT); //byte LinesVisible = RAM.ReadByte(MemoryMap.LINES_VISIBLE);

            if (MCRegister == 0 || (MCRegister & 0x80) == 0x80)
            {
                g.DrawString("Graphics Mode disabled", this.Font, TextBrush, 0, 0);
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

            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            // Bilinear interpolation has effect very similar to real HW 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            // Determine if we display a border
            byte border_register = VICKY.ReadByte(MemoryMap.BORDER_CTRL_REG - MemoryMap.VICKY_BASE_ADDR);
            bool displayBorder = (border_register & 1) != 0;

            int borderXSize = displayBorder ? VICKY.ReadByte(MemoryMap.BORDER_X_SIZE - MemoryMap.VICKY_BASE_ADDR) : 0;
            int borderYSize = displayBorder ? VICKY.ReadByte(MemoryMap.BORDER_Y_SIZE - MemoryMap.VICKY_BASE_ADDR) : 0;
            //this may get corrected in Vicky in the near future.
            // if (isPixelDoubled)
            //{
            //    borderXSize >>= 1; // divide by 2
            //    borderYSize >>= 1; // divide by 2
            //}

            //Rectangle rect = new Rectangle(0, 0, resX, resY);
            Rectangle rect = new Rectangle(0, 0, resX-1, resY-1);
            BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bitmapPointer = (int*)bitmapData.Scan0.ToPointer();

            // Load the SOL register - a lines
            int SOLRegAddr = MemoryMap.VKY_LINE_IRQ_CTRL_REG - MemoryMap.VICKY_BASE_ADDR;
            int SOLLine0Addr = MemoryMap.VKY_LINE0_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR;
            int SOLLine1Addr = MemoryMap.VKY_LINE1_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR;

            // Reset LUT Cache
            //lutCache = new int[256 * 8]; // 8 LUTs
            Array.Clear(lutCache, 0, 256 * 8);


            for (int line = 0; line < resY; line++)
            {
                // Handle SOL interrupts
                byte SOLRegister = VICKY.ReadByte(SOLRegAddr);
                if ((SOLRegister & 1) != 0)
                {
                    int SOLLine0 = VICKY.ReadWord(SOLLine0Addr);
                    if (line == SOLLine0)
                    {
                        StartOfLine?.Invoke();
                    }
                }
                if ((SOLRegister & 2) != 0)
                {
                    int SOLLine1 = VICKY.ReadWord(SOLLine1Addr);
                    if (line == SOLLine1)
                    {
                        StartOfLine?.Invoke();
                    }
                }

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
                int* ptr = bitmapPointer + line * STRIDE;
                if (line < borderYSize || line >= resY - borderYSize)
                {
                    for (int x = 0; x < resX; x++)
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

                    for (int x = 0; x < resX; x++)
                    {
                        int resetValue = x < borderXSize || x >= resX - borderXSize ? borderColor : backgroundColor;
                        ptr[x] = resetValue;
                    }

                    // Bitmap Mode - draw the layers in revers order from back to front
                    if ((MCRegister & 0x4) == 0x4 || tileEditorMode)
                    {
                        
                        // Layer 12 - sprite layer 6
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 6, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 11 - bitmap 1
                        if ((MCRegister & 0x8) == 0x8)
                        {
                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 10 - sprite layer 5
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 5, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 9 - tilemap layer 3
                        if ((MCRegister & 0x10) == 0x10)
                        {
                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 3, displayBorder, borderXSize, line, resX);
                        }
                        // Layer 8 - sprite layer 4
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 4, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 7 - tilemap layer 2
                        if ((MCRegister & 0x10) == 0x10)
                        {
                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 2, displayBorder, borderXSize, line, resX);
                        }
                        // Layer 6 - sprite layer 3
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 3, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 5 - tilemap layer 1
                        if ((MCRegister & 0x10) == 0x10)
                        {
                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 1, displayBorder, borderXSize, line, resX);
                        }
                        // Layer 4 - sprite layer 2
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 2, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 3 - tilemap layer 0
                        if ((MCRegister & 0x10) == 0x10)
                        {
                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 0, displayBorder, borderXSize, line, resX);
                        }
                        // Layer 2 - sprite layer 1
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 1, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 1 - bitmap layer 0
                        if ((MCRegister & 0x8) == 0x8)
                        {
                            DrawBitmap(bitmapPointer, gammaCorrection, 0, displayBorder, backgroundColor, borderXSize, borderYSize, line, resX, resY);
                        }
                        // Layer 0 - sprite layer 0
                        if ((MCRegister & 0x20) != 0)
                        {
                            DrawSprites(bitmapPointer, gammaCorrection, 0, displayBorder, borderXSize, borderYSize, line, resX, resY);
                        }
                    }
                    // Draw the text
                    if ((MCRegister & 7) == 0x1 || (MCRegister & 7) == 3 || (MCRegister & 7) == 7)
                    {
                        if (top == 0)
                        {
                            DrawBitmapText(bitmapPointer, MCRegister, gammaCorrection, ColumnsVisible, LinesVisible, borderXSize, borderYSize, line, resX, resY);
                        }
                    }
                }
                if (!TileEditorMode)
                {
                    DrawMouse(bitmapPointer, gammaCorrection, line, resX, resY);
                }

            }
            frameBuffer.UnlockBits(bitmapData);
            g.DrawImage(frameBuffer, ClientRectangle, rect, GraphicsUnit.Pixel);
            //e.Graphics.DrawImageUnscaled(frameBuffer, rect);  // Use this to debug
            drawing = false;
        }

        // We only cache items that are requested, instead of precomputing all 1024 colors.
        private int GetLUTValue(in byte lutIndex, in byte color, in bool gamma)
        {
            //int offset = lutIndex * 256 + color;
            var lc = lutCache;
            int value = lc[lutIndex * 256 + color];

            if (value == 0)
            {

                int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + (lutIndex * 256 + color) * 4;
                byte red = VICKY.ReadByte(lutAddress);
                byte green = VICKY.ReadByte(lutAddress + 1);
                byte blue = VICKY.ReadByte(lutAddress + 2);
                if (gamma)
                {
                    int baseAddr = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
                    blue = VICKY.ReadByte(baseAddr + blue);           // gammaCorrection[fgValueBlue];
                    green = VICKY.ReadByte(baseAddr + 0x100 + green); // gammaCorrection[0x100 + fgValueGreen];
                    red = VICKY.ReadByte(baseAddr + 0x200 + red);     // gammaCorrection[0x200 + fgValueRed];
                }
                value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                lc[lutIndex * 256 + color] = value;
            }
            return value;
        }

        int[] FGTextLUT;
        int[] BGTextLUT;
        private int[] GetTextLUT(byte fg, byte bg, bool gamma)
        {
            int[] values = new int[2];
            var fgt = FGTextLUT;
            if (fgt[fg] == 0)
            {
                // Read the color lookup tables
                int fgLUTAddress = MemoryLocations.MemoryMap.FG_CHAR_LUT_PTR - VICKY.StartAddress;

                // In order to reduce the load of applying Gamma correction, load single bytes
                byte fgValueRed = VICKY.ReadByte(fgLUTAddress + fg * 4);
                byte fgValueGreen = VICKY.ReadByte(fgLUTAddress + fg * 4 + 1);
                byte fgValueBlue = VICKY.ReadByte(fgLUTAddress + fg * 4 + 2);

                if (gamma)
                {
                    int baseAddr = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
                    fgValueBlue = VICKY.ReadByte(baseAddr + fgValueBlue); //gammaCorrection[fgValueBlue];
                    fgValueGreen = VICKY.ReadByte(baseAddr + 0x100 + fgValueGreen);//gammaCorrection[0x100 + fgValueGreen];
                    fgValueRed = VICKY.ReadByte(baseAddr + 0x200 + fgValueRed);//gammaCorrection[0x200 + fgValueRed];
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
                int bgLUTAddress = MemoryLocations.MemoryMap.BG_CHAR_LUT_PTR - VICKY.StartAddress;

                byte bgValueRed = VICKY.ReadByte(bgLUTAddress + bg * 4);
                byte bgValueGreen = VICKY.ReadByte(bgLUTAddress + bg * 4 + 1);
                byte bgValueBlue = VICKY.ReadByte(bgLUTAddress + bg * 4 + 2);

                if (gamma)
                {
                    int baseAddr = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
                    bgValueBlue = VICKY.ReadByte(baseAddr + bgValueBlue); //gammaCorrection[bgValueBlue];
                    bgValueGreen = VICKY.ReadByte(baseAddr + 0x100 + bgValueGreen); //gammaCorrection[0x100 + bgValueGreen];
                    bgValueRed = VICKY.ReadByte(baseAddr + 0x200 + bgValueRed); //gammaCorrection[0x200 + bgValueRed];
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

        private unsafe void DrawBitmapText(int* p, int MCR, bool gammaCorrection, byte TextColumns, byte TextRows, int colOffset, int rowOffset, int line, int width, int height)
        {
            bool overlayBitSet = (MCR & 0x02) == 0x02;

            int lineStartAddress = MemoryLocations.MemoryMap.SCREEN_PAGE0 - VICKY.StartAddress;
            int colorStartAddress = MemoryLocations.MemoryMap.SCREEN_PAGE1 - VICKY.StartAddress;
            int fontBaseAddress = MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START - VICKY.StartAddress;

            // Find which line of characters to display
            int txtline = (line - rowOffset) / CHAR_HEIGHT;
            //if (txtline + 1 > RAM.ReadByte(MemoryMap.LINES_MAX)) return;
            //byte COLS_PER_LINE = RAM.ReadByte(MemoryMap.COLS_PER_LINE);

            // Initialize the LUTs
            FGTextLUT = new int[16];
            BGTextLUT = new int[16];

            // Cursor Values
            byte CursorY = RAM.ReadByte(MemoryMap.CURSORY);
            byte CursorX = RAM.ReadByte(MemoryMap.CURSORX);
            bool CursorEnabled = (VICKY.ReadByte(MemoryMap.VKY_TXT_CURSOR_CTRL_REG - MemoryMap.VICKY_START) & 1) != 0;

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
                if (CursorX == col && CursorY == txtline && CursorState && CursorEnabled)
                {
                    character = VICKY.ReadByte(MemoryLocations.MemoryMap.VKY_TXT_CURSOR_CHAR_REG - VICKY.StartAddress);
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
        
        private unsafe void DrawBitmap(int* p, bool gammaCorrection, int layer, bool bkgrnd, int bgndColor, int borderXSize, int borderYSize, int line, int width, int height)
        {

            // Bitmap Controller is located at $AF:0100 and $AF:0108
            int regAddr = MemoryMap.BITMAP_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_BASE_ADDR + layer * 8;
            byte reg = VICKY.ReadByte(regAddr);
            if ((reg & 0x01) == 00)
            {
                return;
            }
            byte lutIndex = (byte)((reg >> 1) & 7);  // 8 possible LUTs

            int bitmapAddress = VICKY.ReadLong(regAddr + 1) & 0x3F_FFFF;
            int xOffset = VICKY.ReadWord(regAddr + 4);
            int yOffset = VICKY.ReadWord(regAddr + 6);

            int clrVal = 0;
            int offsetAddress = bitmapAddress + line * width;
            int pixelOffset = line * STRIDE;
            int* ptr = p + pixelOffset;
            //int col = borderXSize;
            byte pixVal = 0;
            VRAM.CopyIntoBuffer(offsetAddress, width, pixVals);

            int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + lutIndex * 1024;

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

        private unsafe void DrawTiles(int* p, bool gammaCorrection, byte TextColumns, int layer, bool bkgrnd, in int borderXSize, in int line, in int width)
        {
            // There are four possible tilemaps to choose from
            int addrTileCtrlReg = MemoryMap.TILE_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_BASE_ADDR + layer * 12;
            int reg = VICKY.ReadByte(addrTileCtrlReg);
            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00)
            {
                return;
            }
            bool smallTiles = (reg & 8) > 0;
            
            int tileSize = (smallTiles ? 8 : 16);
            int strideLine = tileSize * 16;

            int tilemapWidth = VICKY.ReadWord(addrTileCtrlReg + 4) & 0x3FF;   // 10 bits
            //int tilemapHeight = VICKY.ReadWord(addrTileCtrlReg + 6) & 0x3FF;  // 10 bits
            int tilemapAddress = VICKY.ReadLong(addrTileCtrlReg + 1 ) & 0x3F_FFFF;
            
            int tilemapWindowX = VICKY.ReadWord(addrTileCtrlReg + 8);
            bool dirUp = (tilemapWindowX & 0x4000) != 0;
            byte scrollX = (byte)(tilemapWindowX & 0x3C00 >> 10);
            tilemapWindowX &= 0x3FF;
            int tileXOffset = tilemapWindowX % tileSize;

            int tilemapWindowY = VICKY.ReadWord(addrTileCtrlReg + 10);
            bool dirRight = (tilemapWindowY & 0x4000) != 0;
            byte scrollY = (byte)(tilemapWindowY & 0x3C00 >> 10);
            tilemapWindowY &= 0x3FF;

            int tileRow = ( line + tilemapWindowY ) / tileSize;
            int tileYOffset = (line + tilemapWindowY) % tileSize;
            int maxX = width - borderXSize;

            // we always read tiles 0 to width/TILE_SIZE + 1 - this is to ensure we can display partial tiles, with X,Y offsets
            int tilemapItemCount = width / tileSize + 1;
            byte[] tiles = new byte[tilemapItemCount * 2];
            int[] tilesetOffsets = new int[tilemapItemCount];
            VRAM.CopyIntoBuffer(tilemapAddress + (1 + tilemapWindowX / tileSize) * 2 + (tileRow + 0) * tilemapWidth * 2, tilemapItemCount * 2, tiles);

            // cache of tilesetPointers
            int[] tilesetPointers = new int[8];
            int[] strides = new int[8];
            for (int i=0;i<8;i++)
            {
                tilesetPointers[i] = VICKY.ReadLong(MemoryMap.TILESET_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + i * 4) & 0x3F_FFFF;
                byte tilesetConfig = VICKY.ReadByte(MemoryMap.TILESET_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + i * 4 + 3);
                strides[i] = (tilesetConfig & 8) != 0 ? strideLine : tileSize;
            }
            for (int i = 0; i< tilemapItemCount; i++)
            {
                byte tile = tiles[i*2];
                byte tilesetReg = tiles[i*2 + 1];
                byte tileset = (byte)(tilesetReg & 7);
                //byte tileLUT = (byte)((tilesetReg & 0x38) >> 3);

                // tileset
                int tilesetPointer = tilesetPointers[tileset];
                int strideX = strides[tileset];
                if (strideX == tileSize)
                {
                    tilesetOffsets[i] = tilesetPointer + (tile % 16 ) * strideLine + (tile / 16) * strideLine * 16 + tileYOffset * tileSize;
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
            int endTileX = (width - borderXSize + tileXOffset) / tileSize + 1;
            int startOffset = (borderXSize + tilemapWindowX) % tileSize;
            int x = borderXSize;
            byte[] tilepix = new byte[tileSize];
            int clrVal = 0;
            for (int t = startTileX; t < endTileX; t++)
            {
                byte tilesetReg = tiles[t * 2 + 1];
                byte lutIndex = (byte)((tilesetReg & 0x38) >> 3);
                //int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + lutIndex * 1024;
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
                } while (startOffset != tileSize && x < maxX);
                startOffset = 0;
                if (x == maxX)
                {
                    break;
                }
            }
        }

        private unsafe void DrawSprites(int* p, bool gammaCorrection, byte layer, bool bkgrnd, int borderXSize, int borderYSize, int line, int width, int height)
        {
            // There are 32 possible sprites to choose from.
            for (int s = 63; s > -1; s--)
            {
                int addrSprite = MemoryMap.SPRITE_CONTROL_REGISTER_ADDR + s * 8 - MemoryMap.VICKY_BASE_ADDR;
                byte reg = VICKY.ReadByte(addrSprite);
                // if the set is not enabled, we're done.
                byte spriteLayer = (byte)((reg & 0x70) >> 4);
                if ((reg & 1) != 0 && layer == spriteLayer)
                {
                    int posY = VICKY.ReadWord(addrSprite + 6) - 32;
                    if ((line >= posY && line < posY + 32))
                    {
                        // TODO Fix this when Vicky II fixes the LUT issue
                        byte lutIndex = (byte)(((reg & 14) >> 1));  // 8 possible LUTs
                        int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + lutIndex * 1024;
                        bool striding = (reg & 0x80) == 0x80;

                        int spriteAddress = VICKY.ReadLong(addrSprite + 1) & 0x3F_FFFF;
                        int posX = VICKY.ReadWord(addrSprite + 4) - 32;


                        if (posX >= (width - borderXSize) || posY >= (height - borderYSize) || (posX +32) < 0 || (posY + 32) < 0)
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
                        if (posX + 32 > width - borderXSize)
                        {
                            spriteWidth = width - borderXSize - posX;
                            if (spriteWidth == 0)
                            {
                                continue;
                            }
                        }

                        int clrVal = 0;
                        byte pixVal = 0;

                        // Sprites are 32 x 32
                        int sline = line - posY;
                        int lineOffset = line * STRIDE;
                        int* ptr = p + lineOffset;
                        for (int col = xOffset; col < xOffset + spriteWidth; col++)
                        {
                            // Lookup the pixel in the tileset - if the value is 0, it's transparent
                            pixVal = VRAM.ReadByte(spriteAddress + col + sline * 32);
                            if (pixVal != 0)
                            {
                                clrVal = GetLUTValue(lutIndex, pixVal, gammaCorrection);
                                //System.Runtime.InteropServices.Marshal.WriteInt32(p, (lineOffset + (col-xOffset + posX)) * 4, value);
                                ptr[col - xOffset + posX] = clrVal;
                            }
                        }
                    }

                }
            }
        }

        private unsafe void DrawMouse(int* p, bool gammaCorrection, int line, int width, int height)
        {
            byte mouseReg = VICKY.ReadByte(MemoryMap.MOUSE_PTR_REG - MemoryMap.VICKY_BASE_ADDR);
            bool MousePointerEnabled = (mouseReg & 1) == 1;

            if (MousePointerEnabled)
            {
                int PosX = VICKY.ReadWord(0x702);
                int PosY = VICKY.ReadWord(0x704);
                if (line >= PosY && line < PosY + 16)
                {
                    int pointerAddress = (mouseReg & 2) == 0 ? MemoryMap.MOUSE_PTR_GRAP0 - MemoryMap.VICKY_BASE_ADDR : MemoryMap.MOUSE_PTR_GRAP1 - MemoryMap.VICKY_BASE_ADDR;

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
                                pixelIndexB = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + pixelIndexR); // gammaCorrection[pixelIndexR];
                                pixelIndexG = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + pixelIndexR); //gammaCorrection[0x100 + pixelIndexR];
                                pixelIndexR = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + pixelIndexR); //gammaCorrection[0x200 + pixelIndexR];
                            }
                            int value = (int)((pixelIndexB << 16) + (pixelIndexG << 8) + pixelIndexR + 0xFF000000);
                            ptr[col + PosX] = value;
                        }
                    }
                }
                
            }
        }

        public bool IsMousePointerVisible()
        {
            // Read the mouse pointer register
            byte mouseReg = VICKY.ReadByte(0x700);
            return (mouseReg & 1) != 0;
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
