using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using FoenixIDE.MemoryLocations;
using KGySoft.CoreLibraries;

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

        public MemoryRAM VRAM = null;
        public MemoryRAM VICKY = null;
        public int paintCycle = 0;
        private bool tileEditorMode = false;

        public delegate void StartOfFramEvent();
        public StartOfFramEvent StartOfFrame;
        public delegate void StartOfLineEvent();
        public StartOfLineEvent StartOfLine;

        public delegate void GpuUpdateFunction();
        public GpuUpdateFunction GpuUpdated;

        private HiResTimer hiresTimer;
        
        private static readonly int[] vs = new int[256 * 8];
        private int[] lutCache = vs;

        public Gpu()
        {
            InitializeComponent();
            this.Load += new EventHandler(Gpu_Load);
            hiresTimer = new HiResTimer(500);
            hiresTimer.Elapsed += GpuRefreshTimer_Tick;
        }

        void Gpu_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(Gpu_Paint);
            BlinkingCounter = BLINK_RATE;

            this.DoubleBuffered = true;

            if (DesignMode)
            {
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
                hiresTimer.Start();
            }
        }

        /// <summary>
        /// number of frames to wait to refresh the screen.
        /// One frame = 1/60 second.
        /// </summary>
        private int BLINK_RATE = 30;
        private int BlinkingCounter;
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

        public void StopTimer()
        {
            hiresTimer.Stop();
            hiresTimer.Interval = 1000;
            hiresTimer.Elapsed -= GpuRefreshTimer_Tick;
            
        }

        public void SetRefreshPeriod(uint time)
        {
            BLINK_RATE = (int)(1000 / time /2);
            BlinkingCounter = BLINK_RATE;
            hiresTimer.Interval = time;
            hiresTimer.Start();
        }

        private int mode = 0; // Mode 0 is FAT Vicky, 1 is Tiny Vicky.
        public void SetMode(int mode)
        {
            this.mode = mode;
        }

        const int STRIDE = 800;
        Bitmap frameBuffer = new Bitmap(STRIDE, 600, PixelFormat.Format32bppArgb);
        private bool drawing = false;
        byte[] pixVals = null;

        int MCRAddress;
        public void SetMCRAddress(int val)
        {
            MCRAddress = val;
        }
        int MousePointerRegister;
        public void SetMousePointerRegister(int val)
        {
            MousePointerRegister = val;
        }
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
            Point res;
            if (mode == 0)
            {
                res = GetScreenSize();
            }
            else
            {
                res = GetScreenSize_JR();
            }
            if (VICKY != null)
            {
                byte MCRegister = VICKY.ReadByte(MCRAddress); // Reading address $AF:0000
                byte MCRHigh = (byte)(VICKY.ReadByte(MCRAddress + 1) & 3); // Reading address $AF:0001

                pixVals = new byte[res.X];
                int top = 0; // top gets modified if error messages are displayed
                //Graphics g = Graphics.FromImage(frameBuffer);
                Graphics g = e.Graphics;
                byte ColumnsVisible = (byte)(res.X / CHAR_WIDTH); //byte ColumnsVisible = RAM.ReadByte(MemoryMap.COLS_VISIBLE);
                byte LinesVisible = (byte)(res.Y / CHAR_HEIGHT); //byte LinesVisible = RAM.ReadByte(MemoryMap.LINES_VISIBLE);

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

                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                // Bilinear interpolation has effect very similar to real HW 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                // Check if SOF is enabled
                if (MCRegister != 0 && MCRegister != 0x80)
                {
                    StartOfFrame?.Invoke();
                }

                // Determine if we display a border
                byte border_register = VICKY.ReadByte(MCRAddress + 4);
                bool displayBorder = (border_register & 1) != 0;

                int borderXSize = displayBorder ? VICKY.ReadByte(MCRAddress + 8) : 0;
                int borderYSize = displayBorder ? VICKY.ReadByte(MCRAddress + 9) : 0;
                //this may get corrected in Vicky in the near future.
                // if (isPixelDoubled)
                //{
                //    borderXSize >>= 1; // divide by 2
                //    borderYSize >>= 1; // divide by 2
                //}

                Rectangle rect = new Rectangle(0, 0, res.X - 1, res.Y - 1);
                BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                int* bitmapPointer = (int*)bitmapData.Scan0.ToPointer();

                // Load the SOL register - a lines
                int SOLRegAddr = LineIRQRegister - VICKY.StartAddress;
                int SOLLine0Addr = SOL0Address - VICKY.StartAddress;
                int SOLLine1Addr = SOL1Address - VICKY.StartAddress;

                // Reset LUT Cache
                //lutCache = new int[256 * 8]; // 8 LUTs
                Array.Clear(lutCache, 0, 256 * 8);

                for (int line = 0; line < res.Y; line++)
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
                    byte borderRed = VICKY.ReadByte(MCRAddress + 5);
                    byte borderGreen = VICKY.ReadByte(MCRAddress + 6);
                    byte borderBlue = VICKY.ReadByte(MCRAddress + 7);
                    if (gammaCorrection)
                    {
                        borderRed = VICKY.ReadByte(GammaBaseAddress + 0x200 + borderRed); //gammaCorrection[0x200 + borderGreen];
                        borderGreen = VICKY.ReadByte(GammaBaseAddress + 0x100 + borderGreen); //gammaCorrection[0x100 + borderGreen];
                        borderBlue = VICKY.ReadByte(GammaBaseAddress + borderBlue); // gammaCorrection[borderBlue];
                    }
                    int borderColor = (int)(0xFF000000 + (borderBlue << 16) + (borderGreen << 8) + borderRed);

                    if (tileEditorMode)
                    {
                        borderColor = Color.LightGray.ToArgb();
                    }
                    //int offset = line * 640;
                    int* ptr = bitmapPointer + line * STRIDE;
                    if (line < borderYSize || line >= res.Y - borderYSize)
                    {
                        for (int x = 0; x < res.X; x++)
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
                            byte backRed = VICKY.ReadByte(MCRAddress + 0x0D);
                            byte backGreen = VICKY.ReadByte(MCRAddress + 0x0E);
                            byte backBlue = VICKY.ReadByte(MCRAddress + 0x0F);
                            if (gammaCorrection)
                            {
                                backRed = VICKY.ReadByte(GammaBaseAddress + 0x200 + backRed); // gammaCorrection[0x200 + backRed];
                                backGreen = VICKY.ReadByte(GammaBaseAddress + 0x100 + backGreen); //gammaCorrection[0x100 + backGreen];
                                backBlue = VICKY.ReadByte(GammaBaseAddress + backBlue); //gammaCorrection[backBlue];
                            }
                            backgroundColor = (int)(0xFF000000 + (backBlue << 16) + (backGreen << 8) + backRed);
                        }

                        for (int x = 0; x < res.X; x++)
                        {
                            int resetValue = x < borderXSize || x >= res.X - borderXSize ? borderColor : backgroundColor;
                            ptr[x] = resetValue;
                        }

                        // Bitmap Mode - draw the layers in revers order from back to front
                        if ((MCRegister & 0x4) == 0x4 || tileEditorMode)
                        {
                            if (mode == 0)
                            {
                                // Layer 12 - sprite layer 6
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 6, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 11 - bitmap 1
                                if ((MCRegister & 0x8) == 0x8)
                                {
                                    DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, false, false);
                                }
                                // Layer 10 - sprite layer 5
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 5, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 9 - tilemap layer 3
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 3, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 8 - sprite layer 4
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 4, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 7 - tilemap layer 2
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 2, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 6 - sprite layer 3
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 3, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 5 - tilemap layer 1
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 1, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 4 - sprite layer 2
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 2, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 3 - tilemap layer 0
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 0, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 2 - sprite layer 1
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 1, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 1 - bitmap layer 0
                                if ((MCRegister & 0x8) == 0x8)
                                {
                                    DrawBitmap(bitmapPointer, gammaCorrection, 0, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, false, false);
                                }
                                // Layer 0 - sprite layer 0
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 0, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                            }
                            else
                            {
                                // Tiny Vicky Layers for Bitmaps, Tilemaps and sprites
                                byte LayerMgr0 = (byte)(VICKY.ReadByte(0xD002 - 0xC000) & 0x7);
                                byte LayerMgr1 = (byte)(VICKY.ReadByte(0xD002 - 0xC000) >> 4);
                                byte LayerMgr2 = (byte)(VICKY.ReadByte(0xD003 - 0xC000) & 0x7);
                                bool doubleY = (MCRHigh & 4) == 0;
                                bool doubleX = (MCRHigh & 2) == 0;
                                int BitmapY = (MCRHigh & 4) != 0 ? res.Y : res.Y >> 1;
                                
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 0, displayBorder, borderXSize, borderYSize, line, res.X, BitmapY, doubleX, doubleY);
                                }
                                if ((MCRegister & 0x8) != 0 || (MCRegister & 0x10) != 0)
                                {
                                    switch (LayerMgr0)
                                    {
                                        case 0:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 0, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 1:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 2:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 4:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 0, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 5:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 1, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 6:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 2, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                    }
                                    
                                }
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 1, displayBorder, borderXSize, borderYSize, line, res.X, BitmapY, doubleX, doubleY);
                                }
                                if ((MCRegister & 0x8) != 0 || (MCRegister & 0x10) != 0)
                                {
                                    switch (LayerMgr1)
                                    {
                                        case 0:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 0, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 1:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 2:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 4:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 0, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 5:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 1, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 6:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 2, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                    }

                                }
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 2, displayBorder, borderXSize, borderYSize, line, res.X, BitmapY, doubleX, doubleY);
                                }
                                if ((MCRegister & 0x8) != 0 || (MCRegister & 0x10) != 0)
                                {
                                    switch (LayerMgr2)
                                    {
                                        case 0:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 0, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 1:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 2:
                                            DrawBitmap(bitmapPointer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 4:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 0, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 5:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 1, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                        case 6:
                                            DrawTiles(bitmapPointer, gammaCorrection, ColumnsVisible, 2, displayBorder, borderXSize, line, res.X, doubleX, doubleY);
                                            break;
                                    }

                                }
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 3, displayBorder, borderXSize, borderYSize, line, res.X, BitmapY, doubleX, doubleY);
                                }
                            }
                        }
                        // Draw the text
                        if ((MCRegister & 7) == 0x1 || (MCRegister & 7) == 3 || (MCRegister & 7) == 7)
                        {
                            if (top == 0)
                            {
                                DrawBitmapText(bitmapPointer, MCRegister, gammaCorrection, ColumnsVisible, LinesVisible, borderXSize, borderYSize, line, res.X, res.Y);
                            }
                        }
                    }
                    if (!TileEditorMode)
                    {
                        if (mode == 0)
                        {
                            DrawMouse(bitmapPointer, gammaCorrection, line, res.X, res.Y);
                        }
                    }

                }
                frameBuffer.UnlockBits(bitmapData);
                g.DrawImage(frameBuffer, ClientRectangle, rect, GraphicsUnit.Pixel);
                //e.Graphics.DrawImageUnscaled(frameBuffer, rect);  // Use this to debug
                drawing = false;
            }
        }

        public bool IsMousePointerVisible()
        {
            if (VICKY != null)
            {
                // Read the mouse pointer register
                byte mouseReg = VICKY.ReadByte(MousePointerRegister);
                return (mouseReg & 1) != 0;
            }
            else
            {
                return false;
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
