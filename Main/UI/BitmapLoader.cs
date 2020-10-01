using FoenixIDE.MemoryLocations;
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
        public IMappable Memory = null;
        public ResourceChecker ResChecker;

        public BitmapLoader()
        {
            InitializeComponent();
        }

        private void BitmapLoader_Load(object sender, EventArgs e)
        {
            // Add items to the combo box
            // Tiles Registers: $AF:0100 to $AF:013F
            FileTypesCombo.Items.Add("Raw");

            for (int i = 0; i < 4; i++)
            {
                FileTypesCombo.Items.Add("Tilemap " + i);
            }

            for (int i = 0; i < 8; i++)
            {
                FileTypesCombo.Items.Add("Tileset " + i);
            }

            for (int i = 0; i < 64; i++)
            {
                FileTypesCombo.Items.Add("Sprite " + i);
            }
            FileTypesCombo.SelectedItem = 0;

            for (int i = 0; i < 4; i++)
            {
                LUTCombo.Items.Add("LUT " + i);
            }
            LUTCombo.SelectedIndex = 0;
        }

        private void FileTypesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LUTCombo.Enabled = FileTypesCombo.SelectedIndex != 0;
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
                DefaultExt = ".bin",
                Filter = "Images Files (*.bmp *.bin *.data *.pal)|*.bmp;*.bin;*.data;*.pal|Binary Files|*.bin|Palette Files|*.pal|Bitmap Files|*.bmp|Data Files|*.data|Any File|*.*"
            };

            // Load content of file in a TextBlock
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = openFileDlg.FileName;
                FileInfo info = new FileInfo(FileNameTextBox.Text);
                FileSizeResultLabel.Text = FormatAddress((int)info.Length);
                StoreButton.Enabled = true;
            }
        }

        private void StoreButton_Click(object sender, EventArgs e)
        {
            StoreButton.Enabled = false;

            // Store the address in the pointer address - little endian - 24 bits
            int destAddress = Convert.ToInt32(LoadAddressTextBox.Text.Replace(":", ""), 16);

            byte[] data = File.ReadAllBytes(FileNameTextBox.Text);
            for (int i = 0; i < data.Length; i++)
            {
                Memory.WriteByte(destAddress + i, data[i]);
            }

            // Determine which addresses to store the bitmap into.
            if (FileTypesCombo.SelectedIndex == 0)
            {
                // Raw
            }
            else if (FileTypesCombo.SelectedIndex < 5)
            {
                // Tilemaps 4
                int tilemapIndex = FileTypesCombo.SelectedIndex - 1;
                int baseAddress = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + tilemapIndex * 12;
                byte lutValue = (byte)LUTCombo.SelectedIndex;
                // enable the tilemap
                Memory.WriteByte(baseAddress, (byte)(1 + (lutValue << 1)));
                // write address offset by bank $b0
                int offsetAddress = destAddress - 0xB0_0000;
                Memory.WriteByte(baseAddress + 1, (byte)(offsetAddress & 0xFF));
                Memory.WriteByte(baseAddress + 2, (byte)((offsetAddress & 0xFF00) >> 8));
                Memory.WriteByte(baseAddress + 3, (byte)((offsetAddress & 0xFF00) >> 16));
                // TODO: Need to write the size of the tilemap
            }
            else if (FileTypesCombo.SelectedIndex < 13)
            {
                // Tilesets 8
                int tilesetIndex = FileTypesCombo.SelectedIndex - 5;
                int baseAddress = MemoryLocations.MemoryMap.TILESET_BASE_ADDR + tilesetIndex * 4;
                byte lutValue = (byte)LUTCombo.SelectedIndex;

                // write address offset by bank $b0
                int offsetAddress = destAddress - 0xB0_0000;
                Memory.WriteByte(baseAddress, (byte)(offsetAddress & 0xFF));
                Memory.WriteByte(baseAddress + 1, (byte)((offsetAddress & 0xFF00) >> 8));
                Memory.WriteByte(baseAddress + 2, (byte)((offsetAddress & 0xFF00) >> 16));
                Memory.WriteByte(baseAddress + 3, lutValue);  // TODO: Add the stride 256 bit 3.

            }
            else
            {
                // Sprites 64
                int spriteIndex = FileTypesCombo.SelectedIndex - 13;
                int baseAddress = MemoryLocations.MemoryMap.SPRITE_CONTROL_REGISTER_ADDR + spriteIndex * 8;
                byte lutValue = (byte)LUTCombo.SelectedIndex;

                // enable the tilemap
                Memory.WriteByte(baseAddress, (byte)(1 + (lutValue << 1)));  // TODO: Add sprite depth

                // write address offset by bank $b0
                int offsetAddress = destAddress - 0xB0_0000;
                Memory.WriteByte(baseAddress + 1, (byte)(offsetAddress & 0xFF));
                Memory.WriteByte(baseAddress + 2, (byte)((offsetAddress & 0xFF00) >> 8));
                Memory.WriteByte(baseAddress + 3, (byte)((offsetAddress & 0xFF00) >> 16));
                // TODO: set the position of the sprite
            }
            
            ResourceChecker.Resource res = new ResourceChecker.Resource
            {
                StartAddress = destAddress,
                SourceFile = FileNameTextBox.Text,
                Name = Path.GetFileNameWithoutExtension(FileNameTextBox.Text),
                FileType = FileTypesCombo.SelectedIndex
            };

            StoreButton.Enabled = true;

            //// Store the bitmap at the user's determined address
            //// The method below simply takes the file and writes it in memory.
            //// What we want is the actual pixels.
            //ImageConverter converter = new ImageConverter();
            //byte[] data = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            //int startOffset = BitConverter.ToInt32(data, 10);
            //int fileLength = BitConverter.ToInt32(data, 2);
            //res.Length = bitmap.Height * bitmap.Width;
            //if (ResChecker.Add(res))
            //{

            //    // The addresses in Vicky a offset by $B0:0000
            //    videoAddress = videoAddress - 0xB0_0000;
            //    Memory.WriteByte(pointerAddress, LowByte(videoAddress));
            //    Memory.WriteByte(pointerAddress + 1, MidByte(videoAddress));
            //    Memory.WriteByte(pointerAddress + 2, HighByte(videoAddress));

            //    // Store the strides in the strideX and strideY
            //    Memory.WriteByte(strideXAddress, LowByte(strideX));
            //    Memory.WriteByte(strideXAddress + 1, MidByte(strideX));
            //    Memory.WriteByte(strideYAddress, LowByte(strideY));
            //    Memory.WriteByte(strideYAddress + 1, MidByte(strideY));



            //    int numberOfColors = BitConverter.ToInt32(data, 46);
            //    int lutOffset = 0xAF_2000 + LUTCombo.SelectedIndex * 1024;
            //    if (numberOfColors == 0)
            //    {
            //        // we need to create a LUT - each LUT only accepts 256 entries - 0 is black
            //        TransformBitmap(data, startOffset, Int32.Parse(PixelDepthValueLabel.Text), lutOffset, writeVideoAddress, bitmap.Width, bitmap.Height);
            //    }
            //    else
            //    {
            //        for (int offset = 54; offset < 1024 + 54; offset = offset + 4)
            //        {
            //            int color = BitConverter.ToInt32(data, offset);
            //            Memory.WriteByte(lutOffset, LowByte(color));
            //            Memory.WriteByte(lutOffset + 1, MidByte(color));
            //            Memory.WriteByte(lutOffset + 2, HighByte(color));
            //            Memory.WriteByte(lutOffset + 3, 0xFF); // Alpha
            //            lutOffset = lutOffset + 4;
            //        }
            //        for (int line = 0; line < bitmap.Height; line++)
            //        {
            //            for (int i = 0; i < bitmap.Width; i++)
            //            {
            //                Memory.WriteByte(writeVideoAddress + (bitmap.Height - line + 1) * bitmap.Width + i, data[startOffset + line * bitmap.Width + i]);
            //            }
            //        }
            //    }
            //    if (FileTypesCombo.SelectedIndex > 0 && FileTypesCombo.SelectedIndex < 5)
            //    {
            //        int layer = FileTypesCombo.SelectedIndex - 1;
            //        OnTileLoaded?.Invoke(layer);
            //    }
            //    MessageBox.Show("Transfer successful!", "Bitmap Storage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.Close();
            //}
            //else
            //{
            //    StoreButton.Enabled = true;
            //}
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

        private void BitmapLoader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
