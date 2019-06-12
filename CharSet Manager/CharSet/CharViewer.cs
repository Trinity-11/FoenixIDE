using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CharSet
{
    public partial class CharViewer : UserControl
    {
        Brush brush = null;  //new SolidBrush(SystemColors.WindowText);
        
        public int BytesPerCharacter = 8;

        public CharViewer()
        {
            InitializeComponent();
        }

        public byte[] InputData = null;

        public byte[] OutputData = new byte[1024 * 8];

        public byte[] LoadBin(string Filename)
        {
            if (!System.IO.File.Exists(Filename))
                return new byte[4096];

            byte[] data = new byte[4096];
            data = System.IO.File.ReadAllBytes(Filename);
            return data;
        }

        public void SaveBin(string Filename, byte[] data)
        {
            System.IO.File.WriteAllBytes(Filename, data);
        }

        public byte[] LoadPNG(string Filename)
        {
            if (!System.IO.File.Exists(Filename))
                return new byte[4096];

            byte[] data = new byte[4096];
            Bitmap img = Image.FromFile(Filename) as Bitmap;
            if (img == null)
                throw new Exception("Not a bitmap file");

            int pos = 0;
            int x = 0;
            int y = 32;
            int bit = 128;
            int row = 0;

            for (y = 0; y < img.Height; y += BytesPerCharacter)
            {
                for (x = 0; x < img.Width; x += 8)
                {
                    for (int cy = y; cy < y + BytesPerCharacter; cy++)
                    {
                        row = 0;
                        bit = 128;
                        for (int cx = x; cx < x + 8; cx++)
                        {

                            var pixel = img.GetPixel(cx, cy);
                            if (pixel.R > 0)
                                row = row | bit;
                            bit = (byte)(bit >> 1);
                        }
                        data[pos++] = (byte)row;
                    }
                }
            }

            return data;
        }

        // Copy all of the characters from the input to the output 
        // useful for converting BIN to PNG or PNG to BIN
        public void CopyAll()
        {
            CopyBlock(InputData, 0, 0, 256);
            Refresh();
        }

        public void CopyNonPET()
        {
            // control characters (0-31)
            CopyBlock(InputData, 0x0, 0x0, 32);
            // 32 (space) to 63 (?)
            //CopyBlock(InputData, 32, 32, 32);
            // upper case letters
            //CopyBlock(InputData, 0, 64, 32);

            // grave (`)
            CopyBlock(InputData, 0x140, 0x60, 1);
            // lower case letters
            //CopyBlock(InputData, 0x101, 0x61, 26);
            // {|}~ and 127
            CopyBlock(InputData, 0x15b, 0x7b, 5);
            //solid block
            CopyBlock(InputData, 0xe0, 0xa0, 1);
            // C= PET symbols
            //CopyBlock(InputData, 0x61, 0xa1, 31);
            // Shifted PET symbols
            //CopyBlock(InputData, 0x40, 0xc0, 32);
            // new custom glyphs (last two rows)
            CopyBlock(InputData, 0xe0, 0xe0, 32);
            Refresh();

        }

        private void CharViewer_Load(object sender, EventArgs e)
        {
            brush = new SolidBrush(this.ForeColor);
        }

        private void CharViewer_Paint(object sender, PaintEventArgs e)
        {
            DrawCharSet(InputData, e.Graphics, 0, 0);
            //DrawCharSet(CustomData, e.Graphics, 400, 0);
            DrawCharSet(OutputData, e.Graphics, 400, 0);
        }

        internal void Clear()
        {
            OutputData = new byte[4096];
            Refresh();
        }

        private void DrawCharSet(byte[] data, Graphics g, int StartX, int StartY)
        {
            if (data == null)
                return;

            if (brush == null)
                brush = new SolidBrush(this.ForeColor);

            int characters = data.Length / BytesPerCharacter;
            int x0 = StartX;
            int x = x0;
            int y0 = StartY;
            int y = y0;
            int bitWidth = 2;
            int bitHeight = 2;
            int charWidth = bitWidth * 8 + 4;
            int charHeight = bitHeight * BytesPerCharacter + 4;
            int[] cols = { StartX, StartX + 32, StartX + 28 };
            int[] rows = { StartY, StartY + 24, StartY + 28 };

            x = cols[2];
            y = rows[0];
            for (int i = 0; i < 16; i++)
            {
                g.DrawString(" " + i.ToString("X"), this.Font, brush, x, y);
                x += charWidth;
            }

            x = cols[0];
            y = rows[2];
            for (int i = 0; i < 32; i++)
            {
                g.DrawString(i.ToString("X") + "0", this.Font, brush, x, y);
                y += charHeight;
            }

            x = cols[2];
            y = rows[2];
            for (int i = 0; i < characters; i++)
            {
                x0 = x;
                y0 = y;
                for (int charRow = 0; charRow < BytesPerCharacter; charRow++)
                {
                    int pos = i * BytesPerCharacter + charRow;
                    if (pos < 0 || pos >= data.Length)
                        return;

                    byte b = data[pos];
                    for (int bit = 128; bit > 0;)
                    {
                        if ((b & bit) > 0)
                            g.FillRectangle(brush, x, y, bitWidth, bitHeight);
                        x += bitWidth;
                        bit = bit >> 1;
                    }
                    x = x0;
                    y = y + bitHeight;
                }
                x = cols[2] + ((i + 1) % 16 * charWidth);
                y = rows[2] + ((int)(i + 1) / 16) * charHeight;

            }
        }

        /// <summary>
        /// Re-orders the loaded character set, placing the characters in ASCII order. 
        /// <para>Upper case letters start at 64</para>
        /// <para>Lower case letters start at 97</para>
        /// <para>Shifted symbols start at 192 (letter + 128)
        /// <para>C= PET symbols start at 160</para>
        /// <para>New symbols start at 224</para>
        /// <para>0-31 and 128-159 are control characters and not used</para>
        /// </summary>
        public void ConvertPETtoASCII()
        {
            //CopyBlock(CustomData, 0x0, 0x0, 32);
            // 32 (space) to 63 (?)
            CopyBlock(InputData, 32, 32, 32);
            // upper case letters
            CopyBlock(InputData, 0, 64, 32);

            // grave (`)
            // CopyBlock(InputData, 0x140, 0x60, 1);
            // lower case letters
            CopyBlock(InputData, 0x101, 0x61, 26);
            // {|}~ and 127
            //CopyBlock(InputData, 0x15b, 0x7b, 5);
            //solid block
            CopyBlock(InputData, 0xe0, 0xa0, 1);
            // C= PET symbols
            CopyBlock(InputData, 0x61, 0xa1, 31);
            // Shifted PET symbols
            CopyBlock(InputData, 0x40, 0xc0, 32);
            // new custom glyphs (last two rows)
            //CopyBlock(CustomData, 0xe0, 0xe0, 32);
            Refresh();

            //SaveBin("FOENIX-CHARACTER-ASCII.bin", OutputData);
        }

        private void CopyBlock(byte[] source, int sourceIndex, int destIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                CopyCharacter(source, sourceIndex + i, destIndex + i);
            }
        }

        private void CopyCharacter(byte[] source, int sourceIndex, int destIndex)
        {
            int sp = sourceIndex * BytesPerCharacter;
            int dp = destIndex * BytesPerCharacter;
            for (int i = 0; i < BytesPerCharacter; i++)
            {
                OutputData[dp + i] = source[sp + i];
            }
        }

        private void CharViewer_SizeChanged(object sender, EventArgs e)
        {
        }

        private void CharViewer_Resize(object sender, EventArgs e)
        {
        }

        private void CharViewer_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
        }

    }
}
