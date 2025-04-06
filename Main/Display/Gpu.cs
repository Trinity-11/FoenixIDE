using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using FoenixIDE.MemoryLocations;
using KGySoft.CoreLibraries;
using FoenixIDE.Simulator.Devices;

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
        public SOL F256SOLReg = null;
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


        //NativeHeight and NativeWidth have been adjusted for C#'s window size conventions
        public const int NativeHeight = 480;
        public const int NativeWidth = 640;

        private int MarginHeight;
        private int MarginWidth;

        public Gpu()
        {
            InitializeComponent();
            this.Load += new EventHandler(Gpu_Load);
            hiresTimer = new HiResTimer(200);
            hiresTimer.Elapsed += GpuRefreshTimer_Tick;
        }

        public void KillTimer()
        {
            hiresTimer.Kill();
            hiresTimer = null;
        }

        void Gpu_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(Gpu_Paint);
            BlinkingCounter = BLINK_RATE;

            this.DoubleBuffered = true;

            if (DesignMode)
            {
                hiresTimer.Enabled = false;
            }
            else
            {
                if (ParentForm == null)
                    return;

                // Calculate the margin width,height
                MarginWidth = ParentForm.Width - ClientRectangle.Width;
                MarginHeight = ParentForm.Height - ClientRectangle.Height;

                hiresTimer.Start();
            }
        }

        public void SetViewSize(int width, int height)
        {
            // Resize the window to what the user had selected
            this.Size = new Size(width, height);
            ParentForm.Size = new Size(width + MarginWidth, height + MarginHeight);
        }
        public void SetViewScaling(float scaling, int requiredWidth, int requiredHeight)
        {
            int viewWidth = (int)((float)requiredWidth * scaling);
            int viewHeight = (int)((float)requiredHeight * scaling);
            this.Size = new Size(viewWidth, viewHeight);
            ParentForm.Size = new Size(viewWidth + MarginWidth, viewHeight + MarginHeight);
        }

        public int GetViewWidth()
        {
            return ParentForm.Width - MarginWidth;
        }

        public int GetViewHeight()
        {
            return ParentForm.Height - MarginHeight;
        }

        /// <summary>
        /// number of frames to wait to refresh the screen.
        /// One frame = 1/60 second.
        /// </summary>
        private int BLINK_RATE = 30;
        private int BlinkingCounter;
        private bool MonoRuntime = Type.GetType("Mono.Runtime") != null;
        void GpuRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (BlinkingCounter-- == 0)
            {
                CursorState = !CursorState;
                BlinkingCounter = BLINK_RATE;
            }

            if (MonoRuntime)
            {
                Refresh();
            }
            else
            {
                Invalidate();
            }

            if (BlinkingCounter == 0)
            {
                GpuUpdated?.Invoke();
            }
        }

        public void StopTimer()
        {
            hiresTimer.Enabled = false;
            hiresTimer.Interval = 200;
            hiresTimer.Elapsed -= GpuRefreshTimer_Tick;
        }

        public void ResetGPU(bool started)
        {
            int time = 500;
            if (started)
            {
                time = 17; // 60 Hz
                if (mode == 1)
                {
                    // F256 has a 70Hz mode
                    byte MCRHigh = (byte)(VICKY.ReadByte(MCRAddress + 1) & 7); // Reading address $D001

                    if ((MCRHigh & 1) != 0)
                    {
                        time = 14; // 70 Hz
                    }

                }
                BLINK_RATE = (int)(500 / time);
            }
            else
            {
                BLINK_RATE = 100;
            }
            hiresTimer.Interval = time;
            BlinkingCounter = BLINK_RATE;
            hiresTimer.Start();
        }

        private int mode = 0; // Mode 0 is FAT Vicky, 1 is Tiny Vicky. TODO: This should be refactored
        private int GammaOffset = 0x100; // Gamma Offset is 0x400 for F256K
        public void SetMode(int mode)
        {
            this.mode = mode;
            GammaOffset = mode == 0 ? 0x100 : 0x400;
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
                res = GetScreenSize_F256();
            }
            if (VICKY != null)
            {
                byte MCRegister = VICKY.ReadByte(MCRAddress); // Reading address $AF:0000

                if (pixVals == null || pixVals.Length != res.X)
                {
                    pixVals = new byte[res.X];
                }
                int top = 0; // top gets modified if error messages are displayed
                //Graphics g = Graphics.FromImage(frameBuffer);
                Graphics g = e.Graphics;
                byte ColumnsVisible = (byte)(res.X / CHAR_WIDTH); //byte ColumnsVisible = RAM.ReadByte(MemoryMap.COLS_VISIBLE);
                byte LinesVisible = (byte)(res.Y / CHAR_HEIGHT); //byte LinesVisible = RAM.ReadByte(MemoryMap.LINES_VISIBLE);

                if (MCRegister == 0 || (MCRegister == 0x80))
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
                    System.Console.WriteLine("GPU Skipped Frame");
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

                Rectangle rect = new Rectangle(0, 0, res.X, res.Y);
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
                    if (mode == 0)
                    {
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
                    }
                    else
                    {
                        F256SOLReg.SetRasterRow(line);
                        if (F256SOLReg.IsInterruptEnabled())
                        {
                            if (line == F256SOLReg.GetSOLLineNumber())
                            {
                                StartOfLine?.Invoke();
                            }
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
                        borderRed = VICKY.ReadByte(GammaBaseAddress + GammaOffset * 2 + borderRed); //gammaCorrection[0x200 + borderGreen];
                        borderGreen = VICKY.ReadByte(GammaBaseAddress + GammaOffset + borderGreen); //gammaCorrection[0x100 + borderGreen];
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
                                backRed = VICKY.ReadByte(GammaBaseAddress + GammaOffset * 2 + backRed); // gammaCorrection[0x200 + backRed];
                                backGreen = VICKY.ReadByte(GammaBaseAddress + GammaOffset + backGreen); //gammaCorrection[0x100 + backGreen];
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
                                    DrawTiles(bitmapPointer, gammaCorrection, 3, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 8 - sprite layer 4
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 4, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 7 - tilemap layer 2
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, 2, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 6 - sprite layer 3
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 3, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 5 - tilemap layer 1
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, 1, displayBorder, borderXSize, line, res.X, false, false);
                                }
                                // Layer 4 - sprite layer 2
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 2, displayBorder, borderXSize, borderYSize, line, res.X, res.Y, false, false);
                                }
                                // Layer 3 - tilemap layer 0
                                if ((MCRegister & 0x10) == 0x10)
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, 0, displayBorder, borderXSize, line, res.X, false, false);
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
                                
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 3, displayBorder, borderXSize, borderYSize, line, res.X, res.Y / 2, true, true);
                                }
                                if ((MCRegister & 0x8) != 0 && LayerMgr2 < 3)
                                {
                                    DrawBitmap(bitmapPointer, gammaCorrection, LayerMgr2, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, true, true);
                                }
                                if ((MCRegister & 0x10) != 0 && (LayerMgr2 > 3 && LayerMgr2 < 7))
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, LayerMgr2 & 3, displayBorder, borderXSize, line, res.X, true, true);
                                }
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 2, displayBorder, borderXSize, borderYSize, line, res.X, res.Y/2 , true, true);
                                }
                                if ((MCRegister & 0x8) != 0 && LayerMgr1 < 3)
                                {
                                    DrawBitmap(bitmapPointer, gammaCorrection, LayerMgr1, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, true, true);
                                }
                                if ((MCRegister & 0x10) != 0 && (LayerMgr1 > 3 && LayerMgr1 <7))
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, LayerMgr1 & 3, displayBorder, borderXSize, line, res.X, true, true);
                                }
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 1, displayBorder, borderXSize, borderYSize, line, res.X, res.Y / 2, true, true);
                                }
                                if ((MCRegister & 0x8) != 0 && LayerMgr0 < 3)
                                {
                                    DrawBitmap(bitmapPointer, gammaCorrection, LayerMgr0, displayBorder, backgroundColor, borderXSize, borderYSize, line, res.X, true, true);
                                }
                                if ((MCRegister & 0x10) != 0 && (LayerMgr0 > 3 && LayerMgr0 < 7))
                                {
                                    DrawTiles(bitmapPointer, gammaCorrection, LayerMgr0 & 3, displayBorder, borderXSize, line, res.X, true, true);
                                }
                                if ((MCRegister & 0x20) != 0)
                                {
                                    DrawSprites(bitmapPointer, gammaCorrection, 0, displayBorder, borderXSize, borderYSize, line, res.X, res.Y / 2, true, true);
                                }
                            }
                        }
                        // Draw the text
                        if ((MCRegister & 7) == 0x1 || (MCRegister & 7) == 3 || (MCRegister & 7) == 7)
                        {
                            if (top == 0)
                            {
                                DrawText(bitmapPointer, MCRegister, gammaCorrection, ColumnsVisible, LinesVisible, borderXSize, borderYSize, line,res.X, res.Y);
                            }
                        }
                    }
                    if (!TileEditorMode)
                    {
                        DrawMouse(bitmapPointer, gammaCorrection, line, res.X, res.Y);
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
                return (mouseReg & 3) != 0;
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
