using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Imaging;
using FoenixIDE.Common;
using FoenixIDE.MemoryLocations;

namespace FoenixIDE.Display
{
    public partial class Gpu : UserControl, IMappable
    {
        //public event KeyPressEventHandler KeyPressed;

        private const int REGISTER_BLOCK_SIZE = 256;
        const int MAX_TEXT_COLS = 128;
        const int MAX_TEXT_LINES = 64;
        const int SCREEN_PAGE_SIZE = 128 * 64;
        //const int IO_BASE_ADDR = 0xAF_0000;
        const int TILE_CONTROL_REGISTER_ADDR = 0xAF_0100;
        const int BITMAP_CONTROL_REGISTER_ADDR = 0xAF_0140;
        const int SPRITE_CONTROL_REGISTER_ADDR = 0xAF_0200;
        //const int GRP_LUT_BASE_ADDR = 0xAF_2000;
        //const int GAMMA_BASE_ADDR = 0xAF_4000;
        const int charWidth = 8;
        const int charHeight = 8;
        const int tileSize = 16;
        const int spriteSize = 32;

        int[,] graphicsLUT = new int[8, 256];//null;
        byte[,] gammaCorrection = new byte[256, 3]; //null;

        private int length = 128 * 64 * 2; //Text mode uses 16K, 1 page for text, the other for colors.

        public MemoryRAM VRAM = null;
        public MemoryRAM RAM = null;
        //public MemoryRAM IO = null;
        public MemoryRAM VICKY = null;

        public int paintCycle = 0;
        private bool tileEditorMode = false;
        public bool MousePointerMode = false;
        public delegate void StartOfFramEvent();
        public StartOfFramEvent StartOfFrame;

        public int StartAddress
        {
            get
            {
                return MemoryLocations.MemoryMap.SCREEN_PAGE0;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int EndAddress
        {
            get
            {
                return StartAddress + length - 1;
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

        //private int colorMatrixStart = 6096;
        //private int attributeStart = 8096;

        //public List<CharacterSet> CharacterSetSlots = new List<CharacterSet>();

        /// <summary>
        /// number of frames to wait to refresh the screen.
        /// One frame = 1/60 second.
        /// </summary>
        public int RefreshTimer = 0;
        public int BlinkRate = 10;

        public ColorCodes CurrentColor = ColorCodes.White;

        // To provide a better contrast when writing on top of bitmaps
        Brush BackgroundTextBrush = new SolidBrush(Color.Black);
        Brush TextBrush = new SolidBrush(Color.LightBlue);
        Brush BorderBrush = new SolidBrush(Color.LightBlue);
        Brush InvertedBrush = new SolidBrush(Color.Blue);
        Brush CursorBrush = new SolidBrush(Color.LightBlue);

        static string MEASURE_STRING = new string('W', 80);

        Timer timer = new Timer();
        bool CursorEnabled = true;
        bool CursorState = true;

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
        public int X
        {
            get
            {
                if (RAM == null)
                    return 0;

                return RAM.ReadByte(MemoryMap.CURSORX);
            }
            set
            {
                int x = value;
                if (x < 0)
                    x = 0;
                if (x >= ColumnsVisible)
                    x = ColumnsVisible - 1;
                if (RAM != null)
                    RAM.WriteByte(MemoryMap.CURSORX, (byte)x);
                ResetDrawTimer();
                CursorPos = GetCharPos(Y, x);
            }
        }

        /// <summary>
        /// Row of cursor position. 0 is top of the screen
        /// </summary>
        [Browsable(false)]
        public int Y
        {
            get
            {
                if (RAM == null)
                    return 0;

                return RAM.ReadByte(MemoryMap.CURSORY);
            }
            set
            {
                int y = value;
                if (y < 0)
                    y = 0;
                if (y >= LinesVisible)
                    y = LinesVisible - 1;
                if (RAM != null)
                    RAM.WriteByte(MemoryMap.CURSORY, (byte)y);
                ResetDrawTimer();
                CursorPos = GetCharPos(y, X);
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
            set
            {
                if (RAM == null)
                    return;

                int i = value;
                if (i < 0)
                    i = 0;
                if (i > MAX_TEXT_COLS)
                    i = MAX_TEXT_COLS;
                RAM.WriteWord(MemoryMap.COLS_VISIBLE, i);
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
            set
            {
                if (RAM == null)
                    return;

                int i = value;
                if (i < 0)
                    i = 0;
                if (i > MAX_TEXT_LINES)
                    i = MAX_TEXT_LINES;
                RAM.WriteWord(MemoryMap.LINES_VISIBLE, i);
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
            set
            {
                if (RAM == null)
                    return;

                int i = value;
                if (i < 0)
                    i = 0;
                if (i > MAX_TEXT_COLS)
                    i = MAX_TEXT_COLS;
                RAM.WriteWord(MemoryMap.COLS_PER_LINE, i);
            }
        }


        public Gpu()
        {
            InitializeComponent();
            this.Load += new EventHandler(Gpu_Load);
        }

        void Gpu_Load(object sender, EventArgs e)
        {
            this.SetScreenSize(80, 60);
            this.Paint += new PaintEventHandler(Gpu_Paint);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 1000 / 60;
            this.VisibleChanged += new EventHandler(FrameBufferControl_VisibleChanged);
            this.DoubleBuffered = true;

            X = 0;
            Y = 0;

            if (DesignMode)
            {
                timer.Enabled = false;
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
            }
        }

        
        public void ResetDrawTimer()
        {
            RefreshTimer = 0;
            CursorState = true;
        }


        public int BufferSize
        {
            get
            {
                return MAX_TEXT_COLS * MAX_TEXT_LINES;
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

            set
            {
                if (RAM == null)
                    return;
                RAM.WriteWord(MemoryMap.CURSORPOS, value);
            }
        }

        /// <summary>
        /// Draw the frame buffer to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Bitmap frameBuffer = new Bitmap(640, 480, PixelFormat.Format32bppArgb);
        void Gpu_Paint(object sender, PaintEventArgs e)
        {
            paintCycle++;

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
            if (MCRegister == 0 || (MCRegister & 0x80) == 0x80)
            {
                e.Graphics.DrawString("Graphics Mode disabled", this.Font, TextBrush, 0, 0);
                return;
            }
            if (MCRegister != 0 && MCRegister != 0x80)
            {
                StartOfFrame?.Invoke();
            }
            Graphics g = Graphics.FromImage(frameBuffer);

            // Determine if we display a border
            int border_register = VICKY.ReadByte(4);
            bool displayBorder = (border_register & 1) == 1;

            int colOffset = (80 - ColumnsVisible) / 2 * charWidth;
            int rowOffset = (60 - LinesVisible) / 2 * charWidth;

            // Load Graphical LUTs
            //graphicsLUT = LoadLUT(IO);
            LoadLUT(VICKY, graphicsLUT);

            // Apply gamma correct
            if ((MCRegister & 0x40) == 0x40)
            {
                //gammaCorrection = LoadGammaCorrection(IO);
                LoadGammaCorrection(VICKY, gammaCorrection);
            }
            //else
            //{
            //    gammaCorrection = null;
            //}

            // Default background color to border color
            // In Text mode, the border color is stored at $AF:0005.
            byte borderRed = VICKY.ReadByte(5);
            byte borderGreen = VICKY.ReadByte(6);
            byte borderBlue = VICKY.ReadByte(7);
            if (((MCRegister & 0x40) == 0x40)/*gammaCorrection != null*/)
            {
                borderRed = gammaCorrection[borderRed, 2];
                borderGreen = gammaCorrection[ borderGreen, 1];
                borderBlue = gammaCorrection[borderBlue, 0];
            }
            int borderColor = (int)(0xFF000000 + (borderBlue << 16) + (borderGreen << 8) + borderRed);

            if (tileEditorMode)
            {
                g.Clear(Color.LightGray);
                DrawTextWithBackground("Tile Editing Mode", g, Color.Black, 240, 10);
                DrawTextWithBackground("Tile Editing Mode", g, Color.Black, 240, 455);
            }
            else
            {
                g.Clear(Color.FromArgb(borderColor));
            }

            // Graphics Mode
            if ((MCRegister & 0x4) == 0x4)
            {
                byte backRed = VICKY.ReadByte(0xD);
                byte backGreen = VICKY.ReadByte(0XE);
                byte backBlue = VICKY.ReadByte(0xF);
                if (((MCRegister & 0x40) == 0x40)/*gammaCorrection != null*/)
                {
                    backRed = gammaCorrection[backRed, 2];
                    backGreen = gammaCorrection[backGreen, 1];
                    backBlue = gammaCorrection[backBlue, 0];
                }
                int backgroundColor = (int)(0xFF000000 + (backBlue << 16) + (backGreen << 8) + backRed);
                Brush graphBackgroundBrush = new SolidBrush(Color.FromArgb(backgroundColor));
                g.FillRectangle(graphBackgroundBrush, colOffset, rowOffset, 640 - 2 * colOffset, 480 - 2 * rowOffset);
            }
       

            // Bitmap Mode
            if ((MCRegister & 0x8) == 0x8)
            {
                DrawBitmap(frameBuffer, displayBorder);
            }
            

            for (int layer = 4; layer > 0; --layer)
            {
                if ((MCRegister & 0x10) == 0x10)
                {
                    DrawTiles(frameBuffer, layer - 1, displayBorder);
                }
                if ((MCRegister & 0x20) == 0x20)
                {
                    DrawSprites(frameBuffer, layer - 1, displayBorder);
                }
            }

            if ((MCRegister & 0x1) == 0x1)
            {
                int top = 0;
                if (ColumnsVisible < 1 || ColumnsVisible > 128)
                {
                    Graphics graphics = Graphics.FromImage(frameBuffer);
                    DrawTextWithBackground("ColumnsVisible invalid:" + ColumnsVisible.ToString(), graphics, Color.Black, 0, top);
                    top += 12;
                }
                if (LinesVisible < 1)
                {
                    Graphics graphics = Graphics.FromImage(frameBuffer);
                    DrawTextWithBackground("LinesVisible invalid:" + LinesVisible.ToString(), graphics, Color.Black, 0, top);
                    top += 12;
                }
                if (top == 0)
                {
                    DrawBitmapText(frameBuffer);
                }
            }
            // Overlay Mode - no need for this, the Text drawing method takes care of this
            if ((MCRegister & 0x2) == 0x2)
            {

            }
            byte mouseReg = VICKY.ReadByte(0x700);
            MousePointerMode = (mouseReg & 1) == 1;
            if (MousePointerMode && !TileEditorMode)
            {
                DrawMouse(frameBuffer);

            }
            e.Graphics.DrawImage(frameBuffer, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        public static void /*byte[,]*/ LoadGammaCorrection(MemoryRAM VKY, byte[,] result)
        {
            // Read the color lookup tables
            int gamAddress = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_START;
            //byte[,] result = new byte[256,3];
            for (int c = 0; c < 256; c++)
            {
                byte blue = VKY.ReadByte(gamAddress);
                byte green = VKY.ReadByte(0x100 + gamAddress);
                byte red = VKY.ReadByte(0x200 + gamAddress);
                gamAddress++;
                result[c, 0] = blue;
                result[c, 1] = green;
                result[c, 2] = red;
            }
            //return result;
        }

        public static void /*int[,]*/ LoadLUT(MemoryRAM VKY, int[,] result)
        {
            // Read the color lookup tables
            int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_START;
            //int[,] result = new int[8,256];
            for (int i = 0; i < 4; i++)
            {
                for (int c = 0; c < 256; c++)
                {
                    byte blue = VKY.ReadByte(lutAddress++);
                    byte green = VKY.ReadByte(lutAddress++);
                    byte red = VKY.ReadByte(lutAddress++);
                    lutAddress++;
                    result[i,c] = (255 << 24) + (red << 16) + (green << 8) + blue;
                }
            }
            //return result;
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

        int lastWidth = 0;
        private void DrawBitmapText(Bitmap bitmap)
        {
            if (lastWidth != ColumnsVisible
                && ColumnsVisible > 0
                && LinesVisible > 0)
            {
                lastWidth = ColumnsVisible;
            }
            bool overlayBitSet = (VICKY.ReadByte(0) & 0x02) == 0x02;

            // We're hard-coding this for now.
            int lines = 52;

            int x;
            int y;

            Graphics gr = Graphics.FromImage(bitmap);

            // Read the color lookup tables
            int fgLUT = MemoryLocations.MemoryMap.FG_CHAR_LUT_PTR - VICKY.StartAddress;
            int bgLUT = MemoryLocations.MemoryMap.BG_CHAR_LUT_PTR - VICKY.StartAddress;

            int col = 0, line = 0;

            int colOffset = (80 - ColumnsVisible) / 2 * charWidth;
            int colorStart = MemoryLocations.MemoryMap.SCREEN_PAGE1 - VICKY.StartAddress;
            int lineStart = MemoryLocations.MemoryMap.SCREEN_PAGE0 - VICKY.StartAddress;
            int lineOffset = (60 - lines) / 2 * charHeight;
            int fontBaseAddress = MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START - VICKY.StartAddress;
            Rectangle rect = new Rectangle(0, 0, 640, 480);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;
            int stride = bitmapData.Stride;

            for (line = 0; line < lines; line++)
            {
                int textAddr = lineStart;
                int colorAddr = colorStart;

                for (col = 0; col < ColumnsVisible; col++)
                {
                    x = col * charWidth + colOffset;
                    y = line * charHeight + lineOffset;

                    // Each character will have foreground and background colors
                    byte character = VICKY.ReadByte(textAddr++);
                    //character = IO.ReadByte(textAddr++);
                    if (X == col && Y == line && CursorState && CursorEnabled)
                    {
                        character = VICKY.ReadByte(MemoryLocations.MemoryMap.VKY_TXT_CURSOR_CHAR_REG - VICKY.StartAddress);
                    }
                    byte color = VICKY.ReadByte(colorAddr++);
                    //byte fgColor = (byte)((color & 0xF0) >> 4);
                    //byte bgColor = (byte)(color & 0x0F);

                    int fgColor = ((color & 0xF0) >> 4) * 4;
                    int bgColor = (color & 0x0F) * 4;

                    // In order to reduce the load of applying Gamma correction, load single bytes
                    //byte fgValueRed = IO.ReadByte(fgLUT + fgColor * 4);
                    //byte fgValueGreen = IO.ReadByte(fgLUT + fgColor * 4 + 1);
                    //byte fgValueBlue = IO.ReadByte(fgLUT + fgColor * 4 + 2);

                    //byte bgValueRed = IO.ReadByte(bgLUT + bgColor * 4);
                    //byte bgValueGreen = IO.ReadByte(bgLUT + bgColor * 4 + 1);
                    //byte bgValueBlue = IO.ReadByte(bgLUT + bgColor * 4 + 2);

                    byte fgValueRed = VICKY.ReadByte(fgLUT + fgColor);
                    byte fgValueGreen = VICKY.ReadByte(fgLUT + fgColor + 1);
                    byte fgValueBlue = VICKY.ReadByte(fgLUT + fgColor + 2);

                    byte bgValueRed = VICKY.ReadByte(bgLUT + bgColor);
                    byte bgValueGreen = VICKY.ReadByte(bgLUT + bgColor + 1);
                    byte bgValueBlue = VICKY.ReadByte(bgLUT + bgColor + 2);

                    if (((VICKY.ReadByte(0) & 0x40) == 0x40)/*gammaCorrection != null*/)
                    {
                        fgValueBlue = gammaCorrection[fgValueBlue, 0];
                        fgValueGreen = gammaCorrection[fgValueGreen, 1];
                        fgValueRed = gammaCorrection[fgValueRed, 2];

                        bgValueBlue = gammaCorrection[bgValueBlue, 0];
                        bgValueGreen = gammaCorrection[bgValueGreen, 1];
                        bgValueRed = gammaCorrection[bgValueRed, 2];
                    }
                    int fgValue = (int)((fgValueBlue << 16) + (fgValueGreen << 8) + fgValueRed + 0xFF000000);
                    int bgValue = (int)((bgValueBlue << 16) + (bgValueGreen << 8) + bgValueRed + 0xFF000000);
                    
                    // Each character is defined by 8 bytes
                    for (int i = 0; i < 8; i++)
                    {
                        int offset = (x + (y + i) * 640) * 4;
                        byte value = VICKY.ReadByte(fontBaseAddress + character * 8 + i);

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
                lineStart += COLS_PER_LINE;
                colorStart += COLS_PER_LINE;
            }
            bitmap.UnlockBits(bitmapData);
        }

        private void DrawBitmap(Bitmap bitmap, bool bkgrnd)
        {
            // Bitmap Controller is located at $AF:0140
            //int reg = IO.ReadByte(BITMAP_CONTROL_REGISTER_ADDR - IO_BASE_ADDR);
            int reg = VICKY.ReadByte(BITMAP_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_START);
            if ((reg & 0x01) == 00)
            {
                return;
            }
            int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs

            int bitmapAddress = VICKY.ReadLong(0xAF_0141 - MemoryMap.VICKY_START);
            int width = VICKY.ReadWord(0xAF_0144 - MemoryMap.VICKY_START);
            int height = VICKY.ReadWord(0xAF_0146 - MemoryMap.VICKY_START);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0,0,640, 480), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;
            int value = 0;

            for (int line = 0; line < height; line++)
            {
                for (int col = 0; col < width; col++)
                {
                    value = (int)graphicsLUT[lutIndex, VRAM.ReadByte(bitmapAddress++)];
                    if (((VICKY.ReadByte(0) & 0x40) == 0x40)/*gammaCorrection != null*/ )
                    {
                        //value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                        value = (int)((gammaCorrection[(value & 0x00FF0000) >> 0x10, 0] << 0x10) +
                                      (gammaCorrection[(value & 0x0000FF00) >> 0x08, 1] << 0x08) +
                                      (gammaCorrection[(value & 0x000000FF), 2]) + 0xFF000000);
                    }

                    System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * bitmap.Width + col) * 4, value);
                }
            }
            bitmap.UnlockBits(bitmapData);
        }

        private void DrawTiles(Bitmap bitmap, int layer, bool bkgrnd)
        {
            // There are four possible tilesets to choose from
            int addrTileset = TILE_CONTROL_REGISTER_ADDR + layer * 8;
            int reg = VICKY.ReadByte(addrTileset - MemoryMap.VICKY_START);
            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00)
            {
                return;
            }
            // This is hard coded for now
            int lines = 52;
            int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs
            bool striding = (reg & 0x80) == 0x80;

            int tilesetAddress = VICKY.ReadLong(addrTileset + 1 - MemoryMap.VICKY_START);
            int strideX = VICKY.ReadWord(addrTileset + 4 - MemoryMap.VICKY_START);
            int strideY = VICKY.ReadWord(addrTileset + 6 - MemoryMap.VICKY_START);

            // Now read the tilemap
            int tilemapAddress = 0xAF5000 + 0x800 * layer;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, 16, 16), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;

            int colOffset = bkgrnd ? (80 - ColumnsVisible) / 2 * charWidth / tileSize: 0;
            int lineOffset = bkgrnd ? (60 - lines) / 2 * charHeight / tileSize : 0;

            for (int tileRow = lineOffset; tileRow < (30 - lineOffset); tileRow++)
            {
                for (int tileCol = colOffset; tileCol < (40 - colOffset); tileCol++)
                {
                    int tile = VICKY.ReadByte(tilemapAddress + tileCol + tileRow * 64 - MemoryMap.VICKY_START);
                    int pixelIndex = 0;
                    int value = 0;

                    // Tiles are 16 x 16
                    for (int line = 0; line < 16; line++)
                    {
                        for (int col = 0; col < 16; col++)
                        {
                            // Lookup the pixel in the tileset
                            pixelIndex = VRAM.ReadByte(tilesetAddress + ((tile / 16) * 256 * 16 + (tile % 16) * 16) + col + line * strideX);
                            if (pixelIndex != 0)
                            {
                                value = (int)graphicsLUT[lutIndex, pixelIndex];
                                if (((VICKY.ReadByte(0) & 0x40) == 0x40)/*gammaCorrection != null*/)
                                {
                                    //value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                                    value = (int)((gammaCorrection[(value & 0x00FF0000) >> 0x10, 0] << 0x10) +
                                                  (gammaCorrection[(value & 0x0000FF00) >> 0x08, 1] << 0x08) +
                                                  (gammaCorrection[(value & 0x000000FF), 2]) + 0xFF000000);
                                }
                                System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * bitmap.Width + col + tileCol * 16 + tileRow * 16 * 640) * 4, value);
                            }
                        }
                    }
                }
            }
            bitmap.UnlockBits(bitmapData);
        }

        private void DrawSprites(Bitmap bitmap, int layer, bool bkgrnd)
        {
            int lines = 52;
            int colOffset = bkgrnd ? (80 - ColumnsVisible) / 2 * charWidth / spriteSize : 0;
            int lineOffset = bkgrnd ? (60 - lines) / 2 * charHeight / spriteSize : 0;

            // There are 32 possible sprites to choose from.
            for (int s = 0; s < 32; s++)
            {
                int addrSprite = SPRITE_CONTROL_REGISTER_ADDR + s * 8;
                int reg = VICKY.ReadByte(addrSprite - MemoryMap.VICKY_START);
                // if the set is not enabled, we're done.
                int spriteLayer = (reg & 0x70) >> 4;
                if ((reg & 1) == 1 && layer == spriteLayer)
                {
                    int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs
                    bool striding = (reg & 0x80) == 0x80;

                    int spriteAddress = VICKY.ReadLong(addrSprite + 1 - MemoryMap.VICKY_START);
                    int posX = VICKY.ReadWord(addrSprite + 4 - MemoryMap.VICKY_START);
                    int posY = VICKY.ReadWord(addrSprite + 6 - MemoryMap.VICKY_START);

                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(posX, posY, 32, 32), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    IntPtr p = bitmapData.Scan0;

                    int value = 0;
                    int pixelIndex = 0;

                    // Sprites are 32 x 32
                    for (int line = 0; line < 32; line++)
                    {
                        for (int col = 0; col < 32; col++)
                        {
                            // Lookup the pixel in the tileset
                            pixelIndex = VRAM.ReadByte(spriteAddress++);
                            if (pixelIndex != 0)
                            {
                                value = (int)graphicsLUT[lutIndex, pixelIndex];
                                if (((VICKY.ReadByte(0) & 0x40) == 0x40)/*gammaCorrection != null*/)
                                {
                                    //value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                                    value = (int)((gammaCorrection[(value & 0x00FF0000) >> 0x10, 0] << 0x10) +
                                                  (gammaCorrection[(value & 0x0000FF00) >> 0x08, 1] << 0x08) +
                                                  (gammaCorrection[(value & 0x000000FF), 2]) + 0xFF000000);
                                }

                                System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * bitmap.Width + col) * 4, value);
                            }
                        }
                    }

                    bitmap.UnlockBits(bitmapData);
                }
            }
        }

        private void DrawMouse(Bitmap bitmap)
        {
            int X = VICKY.ReadWord(0x702);
            int Y = VICKY.ReadWord(0x704);

            byte mouseReg = VICKY.ReadByte(0x700);
            int pointerAddress = 0xAF_0500  - VICKY.StartAddress;
            if ((mouseReg & 2) == 2)
            {
                pointerAddress += 0x100;
            }
            // Mouse pointer is a 16x16 icon
            int colsToDraw = X < 640 - 16 ? 16 : 640 - X;
            int linesToDraw = Y < 480 - 16 ? 16 : 480 - Y;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(X, Y, colsToDraw, linesToDraw), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;
            for (int line = 0; line < linesToDraw; line++)
            {
                for (int col = 0; col < colsToDraw; col++)
                {
                    // Values are 0: transparent, 1:black, 255: white (gray scales)
                    byte pixelIndexR = VICKY.ReadByte(pointerAddress+line *  16 + col);
                    byte pixelIndexG = pixelIndexR;
                    byte pixelIndexB = pixelIndexR;
                    if (pixelIndexR != 0)
                    {
                        if (((VICKY.ReadByte(0) & 0x40) == 0x40)/*gammaCorrection != null*/)
                        {
                            pixelIndexB = gammaCorrection[pixelIndexR, 0];
                            pixelIndexG = gammaCorrection[pixelIndexR, 1];
                            pixelIndexR = gammaCorrection[pixelIndexR, 2];
                        }
                        int value = (int)((pixelIndexB << 16) + (pixelIndexG << 8) + pixelIndexR + 0xFF000000);
                        System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * bitmap.Width + col) * 4, value);
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

        private SizeF MeasureFont(Font font, Graphics g)
        {
            return g.MeasureString(MEASURE_STRING, font, int.MaxValue, StringFormat.GenericTypographic);
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (RefreshTimer-- > 0)
            {
                if (this.Visible)
                    Refresh();
                return;
            }

            this.Refresh();
            CursorState = !CursorState;
            RefreshTimer = BlinkRate;
        }

        void FrameBufferControl_VisibleChanged(object sender, EventArgs e)
        {
            timer.Enabled = this.Visible;
        }

        //private void FrameBuffer_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    TerminalKeyEventArgs args = new TerminalKeyEventArgs(e.KeyChar);
        //    KeyPressed?.Invoke(this, args);
        //}

        public byte ReadByte()
        {
            return 0;
        }

        public virtual void SetScreenSize(int Columns, int Lines)
        {
            this.ColumnsVisible = Columns;
            this.LinesVisible = Lines;
        }

        public byte ReadByte(int Address)
        {
            if (VRAM == null)
                return 0;
            return VRAM.ReadByte(Address - VRAM.StartAddress);
        }

        /// return the GPU registers: start of text page, start of color page, start of character data, 
        /// number of columns, number of LINES, graphics mode, etc. 
        /// </summary>
        /// <param name="Address">Address to read</param>
        /// <returns></returns>
        public byte ReadGPURegister(int Address)
        {
            return 0;
        }

        public void WriteByte(int Address, byte Data)
        {
            if (VICKY == null)
                return;
            VICKY.WriteByte(Address - VRAM.StartAddress, Data);
            //else if (Address >= characterMatrixStart && Address < (characterMatrixStart + CharacterData.Length))
            //{
            //    CharacterData[Address - characterMatrixStart] = (char)Data;
            //}
            //else if (Address >= colorMatrixStart && Address < (colorMatrixStart + ColorData.Length))
            //{
            //    ColorData[Address - colorMatrixStart] = (ColorCodes)Data;
            //}
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
    }
}
