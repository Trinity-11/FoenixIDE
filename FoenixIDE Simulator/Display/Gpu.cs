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
        public event KeyPressEventHandler KeyPressed;

        private const int REGISTER_BLOCK_SIZE = 256;
        const int MAX_TEXT_COLS = 128;
        const int MAX_TEXT_LINES = 64;
        const int SCREEN_PAGE_SIZE = 128 * 64;
        const int IO_BASE_ADDR = 0xAF_0000;
        const int TILE_CONTROL_REGISTER_ADDR = 0xAF_0100;
        const int BITMAP_CONTROL_REGISTER_ADDR = 0xAF_0140;
        const int SPRITE_CONTROL_REGISTER_ADDR = 0xAF_0200;
        const int GRP_LUT_BASE_ADDR = 0xAF_2000;
        const int charWidth = 8;
        const int charHeight = 8;
        const int tileSize = 16;
        const int spriteSize = 32;

        int[][] graphicsLUT = new int[8][];

        private int length = 128 * 64 * 2; //Text mode uses 16K, 1 page for text, the other for colors.

        public MemoryRAM VRAM = null;
        public MemoryRAM RAM = null;
        public MemoryRAM IO = null;
        public int paintCycle = 0;

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
            this.SetScreenSize(80, 40);
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
            for (int i = 0; i < 4;i++)
            {
                graphicsLUT[i] = new int[256];
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

            if (IO == null)
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
                e.Graphics.DrawString("CodeRAM Not Initialized", this.Font, TextBrush, 0, 0);
                return;
            }
            // Don't forget to dispose the framebuffer.
            Graphics g = Graphics.FromImage(frameBuffer);

            // Determine if we display a border
            int border_register = IO.ReadByte(4);
            bool displayBorder = (border_register & 1) == 1;

            // Default background color to border color
            int borderColor = (int)((UInt32)(IO.ReadLong(5) | 0xFF000000));
            g.Clear(Color.FromArgb(borderColor));

            

            byte MCRegister = IO.ReadByte(0); // Reading address $AF:0000
            // Graphics Mode - I don't know what this is for... we already have bit for tiles, sprints and bitmaps.
            if ((MCRegister & 0x4) == 0x4) { }

            int backgroundColor = (int)((UInt32)IO.ReadLong(8) | 0xFF000000);
            Brush graphBackgroundBrush = new SolidBrush(Color.FromArgb(backgroundColor));
            int colOffset = (80 - ColumnsVisible) / 2 * charWidth;
            int rowOffset = (64 - LinesVisible) / 2 * charWidth;
            g.FillRectangle(graphBackgroundBrush, colOffset, rowOffset, 640 - 2 * colOffset, 480 - 2 * rowOffset);
            

            // Bitmap Mode
            if ((MCRegister & 0x8) == 0x8)
            {
                DrawBitmap(frameBuffer, displayBorder);
            }
            // Load Graphical LUTs
            LoadLUT();

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
            e.Graphics.DrawImage(frameBuffer, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        private void LoadLUT()
        {
            // Read the color lookup tables
            int lutAddress = GRP_LUT_BASE_ADDR - IO_BASE_ADDR;
            for (int i = 0; i < 4; i++)
            {
                for (int c = 0; c < 256; c++)
                {
                    byte blue = IO.ReadByte(lutAddress++);
                    byte green = IO.ReadByte(lutAddress++);
                    byte red = IO.ReadByte(lutAddress++);
                    lutAddress++;
                    graphicsLUT[i][c] = (255 << 24) + (red << 16) + (green << 8) + blue;
                }
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

        int lastWidth = 0;
        private void DrawBitmapText(Bitmap bitmap)
        {
            if (lastWidth != ColumnsVisible
                && ColumnsVisible > 0
                && LinesVisible > 0)
            {
                lastWidth = ColumnsVisible;
            }
            bool overlayBitSet = (IO.ReadByte(0) & 0x02) == 0x02;

            // We're hard-coding this for now.
            LinesVisible = 52;


            int x;
            int y;

            Graphics g = Graphics.FromImage(bitmap);
            
            // Read the color lookup tables
            int fgLUT = MemoryLocations.MemoryMap.FG_CHAR_LUT_PTR - IO.StartAddress;
            int bgLUT = MemoryLocations.MemoryMap.BG_CHAR_LUT_PTR - IO.StartAddress;

            int col = 0, line = 0;

            int colorStart = MemoryLocations.MemoryMap.SCREEN_PAGE1 - IO.StartAddress;
            int colOffset = (80 - ColumnsVisible) / 2 * charWidth;
            int lineStart = MemoryLocations.MemoryMap.SCREEN_PAGE0 - IO.StartAddress;
            int lineOffset = (60 - LinesVisible) / 2 * charHeight;
            int fontBaseAddress = MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START - IO.StartAddress;
            Rectangle rect = new Rectangle(0, 0, 640, 480);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;
            int stride = bitmapData.Stride;

            for (line = 0; line < LinesVisible; line++)
            {
                int textAddr = lineStart;
                int colorAddr = colorStart;
                for (col = 0; col < ColumnsVisible; col++)
                {
                    x = col * charWidth + colOffset;
                    y = line * charHeight + lineOffset;

                    // Each character will have foreground and background colors
                    byte character = IO.ReadByte(textAddr++);
                    byte color = IO.ReadByte(colorAddr++);
                    byte fgColor = (byte)((color & 0xF0) >> 4);
                    byte bgColor = (byte)(color & 0x0F);
                    int fgValue = (int)((UInt32)IO.ReadLong(fgLUT + fgColor * 4) | 0xFF000000);
                    int bgValue = (int)((UInt32)IO.ReadLong(bgLUT + bgColor * 4) | 0xFF000000);
                    
                    // Each character is defined by 8 bytes
                    for (int i = 0; i < 8; i++)
                    {
                        int offset = (x + (y + i )* 640 ) * 4;
                        byte value = IO.ReadByte(fontBaseAddress + character * 8 + i);
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

            if (CursorState && CursorEnabled)
            {
                x = X * charWidth;
                y = Y * charHeight;
                //g.FillRectangle(CursorBrush, x + colOffset, y + lineOffset, charWidth, charHeight);
                //g.DrawString(CharacterData[GetCharPos(Y, X)].ToString(),
                //    TextFont,
                //    InvertedBrush,
                //    x, y,
                //    StringFormat.GenericTypographic);
            }
        }

        private void DrawBitmap(Bitmap bitmap, bool bkgrnd)
        {
            // Bitmap Controller is located at $AF:0140
            int reg = IO.ReadByte(BITMAP_CONTROL_REGISTER_ADDR - IO_BASE_ADDR);
            if ((reg & 0x01) == 00)
            {
                return;
            }
            int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs

            int bitmapAddress = IO.ReadLong(0xAF_0141 - IO_BASE_ADDR);
            int width = IO.ReadWord(0xAF_0144 - IO_BASE_ADDR);
            int height = IO.ReadWord(0xAF_0146 - IO_BASE_ADDR);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0,0,640, 480), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;
            for (int line = 0; line < height; line++)
            {
                for (int col = 0; col < width; col++)
                {
                    int value = (int)graphicsLUT[lutIndex][VRAM.ReadByte(bitmapAddress++)];
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * bitmap.Width + col) * 4, value);
                }
            }
            bitmap.UnlockBits(bitmapData);
        }

        private void DrawTiles(Bitmap bitmap, int layer, bool bkgrnd)
        {
            // There are four possible tilesets to choose from
            int addrTileset = TILE_CONTROL_REGISTER_ADDR + layer * 8;
            int reg = IO.ReadByte(addrTileset - IO_BASE_ADDR);
            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00)
            {
                return;
            }
            LinesVisible = 52;
            int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs
            bool striding = (reg & 0x80) == 0x80;

            int tilesetAddress = IO.ReadLong(addrTileset + 1 - IO_BASE_ADDR);
            int strideX = IO.ReadWord(addrTileset + 4 - IO_BASE_ADDR);
            int strideY = IO.ReadWord(addrTileset + 6 - IO_BASE_ADDR);

            // Now read the tilemap
            int tilemapAddress = 0xAF5000 + 0x800 * layer;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, 16, 16), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;

            int colOffset = bkgrnd ? (80 - ColumnsVisible) / 2 * charWidth / tileSize: 0;
            int lineOffset = bkgrnd ? (60 - LinesVisible) / 2 * charHeight / tileSize : 0;

            for (int tileRow = lineOffset; tileRow < (30 - lineOffset); tileRow++)
            {
                for (int tileCol = colOffset; tileCol < (40 - colOffset); tileCol++)
                {
                    int tile = IO.ReadByte(tilemapAddress + tileCol + tileRow * 64 - IO_BASE_ADDR);
                    
                    // Tiles are 16 x 16
                    for (int line = 0; line < 16; line++)
                    {
                        for (int col = 0; col < 16; col++)
                        {
                            // Lookup the pixel in the tileset
                            int pixelIndex = VRAM.ReadByte(tilesetAddress + ((tile / 16) * 256 * 16 + (tile % 16) * 16) + col + line * strideX);
                            if (pixelIndex != 0)
                            {
                                int value = (int)graphicsLUT[lutIndex][pixelIndex];
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
            LinesVisible = 52;
            int colOffset = bkgrnd ? (80 - ColumnsVisible) / 2 * charWidth / spriteSize : 0;
            int lineOffset = bkgrnd ? (60 - LinesVisible) / 2 * charHeight / spriteSize : 0;

            // There are 32 possible sprites to choose from.
            for (int s = 0; s < 32; s++)
            {
                int addrSprite = SPRITE_CONTROL_REGISTER_ADDR + s * 8;
                int reg = IO.ReadByte(addrSprite - IO_BASE_ADDR);
                // if the set is not enabled, we're done.
                int spriteLayer = (reg & 0x70) >> 4;
                if ((reg & 1) == 1 && layer == spriteLayer)
                {
                    int lutIndex = (reg & 14) >> 1;  // 8 possible LUTs
                    bool striding = (reg & 0x80) == 0x80;

                    int spriteAddress = IO.ReadLong(addrSprite + 1 - IO_BASE_ADDR);
                    int posX = IO.ReadWord(addrSprite + 4 - IO_BASE_ADDR);
                    int posY = IO.ReadWord(addrSprite + 6 - IO_BASE_ADDR);

                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(posX, posY, 32, 32), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    IntPtr p = bitmapData.Scan0;

                    // Sprites are 32 x 32
                    for (int line = 0; line < 32; line++)
                    {
                        for (int col = 0; col < 32; col++)
                        {
                            // Lookup the pixel in the tileset
                            int pixelIndex = VRAM.ReadByte(spriteAddress++);
                            if (pixelIndex != 0)
                            {
                                int value = (int)graphicsLUT[lutIndex][pixelIndex];
                                System.Runtime.InteropServices.Marshal.WriteInt32(p, (line * bitmap.Width + col) * 4, value);
                            }
                        }
                    }

                    bitmap.UnlockBits(bitmapData);
                }
            }
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

        private void FrameBuffer_KeyPress(object sender, KeyPressEventArgs e)
        {
            TerminalKeyEventArgs args = new TerminalKeyEventArgs(e.KeyChar);
            KeyPressed?.Invoke(this, args);
        }

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
            if (IO == null)
                return;
            IO.WriteByte(Address - VRAM.StartAddress, Data);
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
            cs.Load(Filename, Offset, IO, MemoryLocations.MemoryMap.FONT0_MEMORY_BANK_START & 0xffff, CharSize);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }
    }
}
