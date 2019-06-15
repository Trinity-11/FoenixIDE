using FoenixIDE.Simulator.FileFormat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FoenixIDE.UI.MainWindow;

namespace FoenixIDE.UI
{
    public partial class BitmapLoader : Form
    {
        public Common.IMappable Memory = null;
        public ResourceChecker ResChecker;

        int controlRegisterAddress = 0;
        int pointerAddress = 0;
        int strideXAddress = 0;
        int strideYAddress = 0;
        int strideX = 0;
        int strideY = 0;
        Bitmap bitmap = null;
        byte controlByte = 0;
        public TileLoadedEvent OnTileLoaded;

        public BitmapLoader()
        {
            InitializeComponent();
        }

        private void BitmapLoader_Load(object sender, EventArgs e)
        {
            // Add items to the combo box
            // Tiles Registers: $AF:0100 to $AF:013F
            BitmapTypesCombo.Items.Add("Bitmap");

            BitmapTypesCombo.Items.Add("Tile Layer 0");
            BitmapTypesCombo.Items.Add("Tile Layer 1");
            BitmapTypesCombo.Items.Add("Tile Layer 2");
            BitmapTypesCombo.Items.Add("Tile Layer 3");

            BitmapTypesCombo.Items.Add("Sprite 0");
            BitmapTypesCombo.Items.Add("Sprite 1");
            BitmapTypesCombo.Items.Add("Sprite 2");
            BitmapTypesCombo.Items.Add("Sprite 3");
            BitmapTypesCombo.Items.Add("Sprite 4");
            BitmapTypesCombo.Items.Add("Sprite 5");
            BitmapTypesCombo.Items.Add("Sprite 6");
            BitmapTypesCombo.Items.Add("Sprite 7");
            BitmapTypesCombo.Items.Add("Sprite 8");
            BitmapTypesCombo.Items.Add("Sprite 9");
            BitmapTypesCombo.Items.Add("Sprite 10");
            BitmapTypesCombo.Items.Add("Sprite 11");
            BitmapTypesCombo.Items.Add("Sprite 12");
            BitmapTypesCombo.Items.Add("Sprite 13");
            BitmapTypesCombo.Items.Add("Sprite 14");
            BitmapTypesCombo.Items.Add("Sprite 15");
            BitmapTypesCombo.Items.Add("Sprite 16");
            BitmapTypesCombo.Items.Add("Sprite 17");
            BitmapTypesCombo.Items.Add("Sprite 18");
            BitmapTypesCombo.Items.Add("Sprite 19");
            BitmapTypesCombo.Items.Add("Sprite 20");
            BitmapTypesCombo.Items.Add("Sprite 21");
            BitmapTypesCombo.Items.Add("Sprite 22");
            BitmapTypesCombo.Items.Add("Sprite 23");
            BitmapTypesCombo.Items.Add("Sprite 24");
            BitmapTypesCombo.Items.Add("Sprite 25");
            BitmapTypesCombo.Items.Add("Sprite 26");
            BitmapTypesCombo.Items.Add("Sprite 27");
            BitmapTypesCombo.Items.Add("Sprite 28");
            BitmapTypesCombo.Items.Add("Sprite 29");
            BitmapTypesCombo.Items.Add("Sprite 30");
            BitmapTypesCombo.Items.Add("Sprite 31");

            LUTCombo.Items.Add("LUT 0");
            LUTCombo.Items.Add("LUT 1");
            LUTCombo.Items.Add("LUT 2");
            LUTCombo.Items.Add("LUT 3");
            LUTCombo.SelectedIndex = 0;
        }


        private String FormatAddress(int address)
        {
            String size = (address).ToString("X6");
            return "$" + size.Substring(0, 2) + ":" + size.Substring(2);
        }
        /*
         * Let the user select a file from the file system and display it in a text box.
         */
        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                Title = "Load Bitmap",
                DefaultExt = ".bmp",
                Filter = "Bitmap Files|*.bmp|Binary Files|*.bin|Any File|*.bmp;*.png"
            };

            // Load content of file in a TextBlock
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = openFileDlg.FileName;
                GetBitmapAttributes(openFileDlg.FileName);
                StoreButton.Enabled = true;
            }
        }

        private void GetBitmapAttributes(String bmpFilename)
        {
            if (Path.GetExtension(bmpFilename).Equals(".bmp"))
            {
                bitmap = new Bitmap(bmpFilename);
                BitmapSizeValueLabel.Text = bitmap.Width + " x " + bitmap.Height;
                FileSizeResultLabel.Text = FormatAddress(bitmap.Width * bitmap.Height);
                PixelDepthValueLabel.Text = (((int)bitmap.PixelFormat) >> 8 & 0xFF).ToString();
                switch (bitmap.Width)
                {
                    case 640:
                        BitmapTypesCombo.SelectedItem = "Bitmap";
                        break;
                    case 16:
                    case 256:
                        BitmapTypesCombo.SelectedItem = "Tile Layer 0";
                        break;
                    case 32:
                        BitmapTypesCombo.SelectedItem = "Sprite 0";
                        break;
                }
            }
            else
            {
                FileInfo info = new FileInfo(bmpFilename);
                FileSizeResultLabel.Text = FormatAddress((int)info.Length);
                BitmapSizeValueLabel.Text = "Unspecified";
                PixelDepthValueLabel.Text = "8";
            }
        }

        private void LoadAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            string item = LoadAddressTextBox.Text.Replace(":","");
            if (!int.TryParse(item, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.CurrentInfo, out int n) &&
              item != String.Empty)
            {
                LoadAddressTextBox.Text = item.Remove(item.Length - 1, 1);
                LoadAddressTextBox.SelectionStart = LoadAddressTextBox.Text.Length;
            }
        }

        private void BitmapTypesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Determine which addresses to store the bitmap into.
            switch (BitmapTypesCombo.SelectedIndex)
            {
                case 0: //bitmap
                    controlRegisterAddress = 0xAF_0140; // 1 byte
                    pointerAddress = 0XAF_0141; // 3 bytes
                    strideXAddress = 0xAF_0144; // 2 bytes --> X Size
                    strideYAddress = 0xAF_0146; // 2 bytes --> Y Size
                    strideX = bitmap.Width;
                    strideY = bitmap.Height;
                    break;
                case 1: //tile layer 0..3
                case 2:
                case 3:
                case 4:
                    int offsetTL = ((BitmapTypesCombo.SelectedIndex - 1) * 8);
                    controlRegisterAddress = 0xAF_0100 + offsetTL; // 1 byte
                    pointerAddress = 0XAF_0101 + offsetTL; // 3 bytes
                    strideXAddress = 0xAF_0104 + offsetTL; // 2 bytes
                    strideYAddress = 0xAF_0106 + offsetTL; // 2 bytes
                    strideX = bitmap.Width;
                    strideY = bitmap.Height;
                    break;
                default: // sprites 0..31
                    int offsetSP = ((BitmapTypesCombo.SelectedIndex - 5) * 8);
                    controlRegisterAddress = 0xAF_0200; // 1 byte
                    pointerAddress = 0XAF_0201; // 3 bytes
                    strideXAddress = 0xAF_0204; // 2 bytes --> X Position
                    strideYAddress = 0xAF_0206; // 2 bytes --> Y Position
                    strideX = bitmap.Width;
                    strideY = bitmap.Height;
                    break;
            }
        }

        private void StoreButton_Click(object sender, EventArgs e)
        {
            StoreButton.Enabled = false;

            // TODO: determine what to do with the control register
            controlByte = (byte)((LUTCombo.SelectedIndex << 1) + 1);
            Memory.WriteByte(controlRegisterAddress, controlByte); // enable

            // Store the address in the pointer address - little endian - 24 bits
            string strAddress = LoadAddressTextBox.Text.Replace(":", "");
            int videoAddress = 0;
            if (strAddress != String.Empty)
            {
                int.TryParse(strAddress, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.CurrentInfo, out videoAddress);
            }
            int writeVideoAddress = videoAddress;
            ResourceChecker.Resource res = new ResourceChecker.Resource
            {
                StartAddress = videoAddress,
                SourceFile = FileNameTextBox.Text,
                Name = Path.GetFileNameWithoutExtension(FileNameTextBox.Text)
            };

            // Store the bitmap at the user's determined address
            // The method below simply takes the file and writes it in memory.
            // What we want is the actual pixels.
            ImageConverter converter = new ImageConverter();
            byte[] data = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            int startOffset = BitConverter.ToInt32(data, 10);
            int fileLength = BitConverter.ToInt32(data, 2);
            res.Length = strideX * strideY;
            if (ResChecker.Add(res))
            {

                // The addresses in Vicky a offset by $B0:0000
                videoAddress = videoAddress - 0xB0_0000;
                Memory.WriteByte(pointerAddress, LowByte(videoAddress));
                Memory.WriteByte(pointerAddress + 1, MidByte(videoAddress));
                Memory.WriteByte(pointerAddress + 2, HighByte(videoAddress));

                // Store the strides in the strideX and strideY
                Memory.WriteByte(strideXAddress, LowByte(strideX));
                Memory.WriteByte(strideXAddress + 1, MidByte(strideX));
                Memory.WriteByte(strideYAddress, LowByte(strideY));
                Memory.WriteByte(strideYAddress + 1, MidByte(strideY));



                int numberOfColors = BitConverter.ToInt32(data, 46);
                int lutOffset = 0xAF_2000 + LUTCombo.SelectedIndex * 1024;
                if (numberOfColors == 0)
                {
                    // we need to create a LUT - each LUT only accepts 256 entries - 0 is black
                    TransformBitmap(data, startOffset, Int32.Parse(PixelDepthValueLabel.Text), lutOffset, writeVideoAddress, bitmap.Width, bitmap.Height);
                }
                else
                {
                    for (int offset = 54; offset < 1024 + 54; offset = offset + 4)
                    {
                        int color = BitConverter.ToInt32(data, offset);
                        Memory.WriteByte(lutOffset, LowByte(color));
                        Memory.WriteByte(lutOffset + 1, MidByte(color));
                        Memory.WriteByte(lutOffset + 2, HighByte(color));
                        Memory.WriteByte(lutOffset + 3, 0xFF); // Alpha
                        lutOffset = lutOffset + 4;
                    }
                    for (int line = 0; line < bitmap.Height; line++)
                    {
                        for (int i = 0; i < bitmap.Width; i++)
                        {
                            Memory.WriteByte(writeVideoAddress + (bitmap.Height - line + 1) * bitmap.Width + i, data[startOffset + line * bitmap.Width + i]);
                        }
                    }
                }
                if (BitmapTypesCombo.SelectedIndex > 0 && BitmapTypesCombo.SelectedIndex < 5)
                {
                    int layer = BitmapTypesCombo.SelectedIndex - 1;
                    OnTileLoaded?.Invoke(layer);
                }
                MessageBox.Show("Transfer successful!", "Bitmap Storage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                StoreButton.Enabled = true;
            }
        }

        private byte HighByte(int value)
        {
            return ((byte)(value >> 16));
        }

        private byte MidByte(int value)
        {
            return ((byte)((value >> 8) & 0xFF));
        }
        private byte LowByte(int value)
        {
            return ((byte)(value & 0xFF));
        }

        /*
         * Convert a bitmap with no palette to a bytes with a color lookup table.
         */
        private void TransformBitmap(byte[] data, int startOffset, int pixelDepth, int lutPointer, int videoPointer, int width, int height)
        {
            List<int> lut = new List<int>(256)
            {
                // Always add black and white
                0,
                0xFFFFFF
            };
            // Read every pixel into a color table
            int bytes = 1;
            switch (pixelDepth)
            {
                case 16:
                    bytes = 2;
                    break;
                case 24:
                    bytes = 3;
                    break;
            }
            // Now read the bitmap
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pointer = startOffset + ((height - y - 1) * width + x) * bytes;
                    int rgb = -1;
                    switch (pixelDepth)
                    {
                        case 16:
                            rgb = (data[pointer] & 0x1F) + ((((data[pointer] & 0xE0) >> 5) + (data[pointer + 1] & 0x3) << 3) << 8) + ((data[pointer + 1] & 0x7C) << 14);
                            break;
                        case 24:
                            rgb = data[pointer] + (data[pointer + 1] << 8) + (data[pointer + 2] << 16);
                            break;
                    }
                    if (rgb != -1)
                    {
                        int index = lut.IndexOf(rgb);
                        byte value = (byte)index;
                        if (index == -1 && lut.Count < 256)
                        {
                            lut.Add(rgb);
                            value = (byte)(lut.Count - 1);
                            // Write the value to the LUT
                            Memory.WriteByte(value * 4 + lutPointer, data[pointer]);
                            Memory.WriteByte(value * 4 + 1 + lutPointer, data[pointer + 1]);
                            Memory.WriteByte(value * 4 + 2 + lutPointer, data[pointer + 2]);
                            Memory.WriteByte(value * 4 + 3 + lutPointer, 0xFF);
                        }
                        Memory.WriteByte(videoPointer++, value);
                    }
                }
            }
        }
    }
}
