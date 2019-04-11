using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace FoenixIDE.Display
{
    public class CharacterSet
    {
        public string Name = "";
        public const int SlotSize = 8192;

        /// <summary>
        /// Character types. Ths is used to pick the "slot" the character set lives in.
        /// </summary>
        public enum CharTypeCodes
        {
            ASCII_PET = 0,
            ASCII_IBM = 1,
            PETSCII_GRAPHICS = 2,
            PETSCII_TEXT = 3,
            Custom1 = 4,
            Custom2 = 5,
            Custom3 = 6,
            Custom4 = 7,
        }

        /// <summary>
        /// size of character bitmaps.
        /// </summary>
        public enum SizeCodes
        {
            Unknown = 0,
            Size8x8 = 8,
            Size8x16 = 16,
        }

        public CharTypeCodes CharType = CharTypeCodes.ASCII_PET;
        public SizeCodes CharSize = SizeCodes.Unknown;

        public Bitmap[] Bitmaps;

        public int StartAddress;
        public int Length;
        public MemoryRAM CharacterData;
        private int charWidth = 8;
        private int charHeight = 8;

        /// <summary>
        /// Returns a single row (byte) in the character data. 
        /// </summary>
        /// <param name="CharacterCode">Character code</param>
        /// <param name="Row">Row in glpyh</param>
        /// <returns></returns>
        public byte Read(int CharacterCode, int Row)
        {
            if (CharacterData == null)
                return 0;
            int addr = StartAddress + CharacterCode * charHeight + Row;
            return CharacterData.ReadByte(addr);
        }

        /// <summary>
        /// Load a character set from disk
        /// </summary>
        /// <param name="Filename">data file with character glyphs</param>
        /// <param name="Vram">array to store glyph data</param>
        /// <param name="StartAddress">starting address in array</param>
        /// <param name="newCharSize">Size of glyhphs (8x8 or 8x16)</param>
        public void Load(string Filename, int Offset, MemoryRAM Vram, int StartAddress, SizeCodes newCharSize)
        {
            this.StartAddress = StartAddress;
            this.CharSize = newCharSize;
            this.CharacterData = Vram;

            try
            {
                byte[] d = global::System.IO.File.ReadAllBytes(Filename);
                Vram.Load(d, Offset, StartAddress, d.Length - Offset);
            }
            catch (Exception ex)
            {
                SystemLog.WriteLine(SystemLog.SeverityCodes.Recoverable, "Error in CharacteSet.Load\r\n" + ex.Message + "Filename:" + Filename);
            }

            GenerateBitmaps();

        }

        SolidBrush brush = new SolidBrush(Color.White);
        private void GenerateBitmaps()
        {
            Bitmaps = new Bitmap[256];
            switch (CharSize)
            {
                case SizeCodes.Unknown:
                    break;
                case SizeCodes.Size8x8:
                    charHeight = 8;
                    break;
                case SizeCodes.Size8x16:
                    charHeight = 16;
                    break;
                default:
                    break;
            }

            for (int charCode = 0; charCode < 256; charCode++)
            {
                Bitmap bmp = new Bitmap(charWidth, charHeight, PixelFormat.Format1bppIndexed);
                Bitmaps[charCode] = bmp;

                BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, charWidth, charHeight), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
                IntPtr p = bitmapData.Scan0;
                for (int y = 0; y < charHeight; y++)
                {
                    System.Runtime.InteropServices.Marshal.WriteByte(p, y * bitmapData.Stride, Read(charCode, y));
                }
                bmp.UnlockBits(bitmapData);
            }
        }
    }
}
