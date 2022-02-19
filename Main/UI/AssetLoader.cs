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
using static FoenixIDE.Simulator.FileFormat.ResourceChecker;
using static FoenixIDE.UI.MainWindow;

namespace FoenixIDE.UI
{
    public partial class AssetLoader : Form
    {
        public MemoryManager MemMgrRef = null;
        public ResourceChecker ResChecker;
        public int topLeftPixelColor = 0;

        public AssetLoader()
        {
            InitializeComponent();
        }

        private void AssetLoader_Load(object sender, EventArgs e)
        {
            // Add items to the combo box
            // Tiles Registers: $AF:0100 to $AF:013F
            FileTypesCombo.Items.Add("Bitmap Layer 0");
            FileTypesCombo.Items.Add("Bitmap Layer 1");

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
            FileTypesCombo.SelectedItem = 0; // Bitmap layer 0
            for (int i = 0; i < 4; i++)
            {
                FileTypesCombo.Items.Add("LUT " + i);
            }

            for (int i = 0; i < 4; i++)
            {
                LUTCombo.Items.Add("LUT " + i);
            }
            FileTypesCombo.SelectedIndex = 0;
            LUTCombo.SelectedIndex = 0;
        }

        /**
         * The LUT list box is only selected when loading bitmaps, sprites and tiles
         */
        private void FileTypesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool LUTSelected = FileTypesCombo.SelectedItem.ToString().StartsWith("LUT");
            LUTCombo.Enabled = !LUTSelected;
            LoadAddressTextBox.Enabled = !LUTSelected;
            if (FileTypesCombo.SelectedItem.ToString().StartsWith("LUT"))
            {
                int lut = Convert.ToInt32(FileTypesCombo.SelectedItem.ToString().Substring(4));
                LoadAddressTextBox.Enabled = false;
                LoadAddressTextBox.Text = (MemoryLocations.MemoryMap.GRP_LUT_BASE_ADDR + lut * 1024).ToString("X6");
            }
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
                Filter = "Asset Files (*.bmp *.png *.bin *.data *.pal *.tls *.aseprite)|*.bmp;*.png;*.bin;*.data;*.pal;*.tls;*.aseprite|Binary Files|*.bin|Palette Files|*.pal|Bitmap Files|*.bmp;*.png|Data Files|*.data|Tilemap Files|*.tls|Any File|*.*"
            };

            // Load content of file in a TextBlock
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = openFileDlg.FileName;
                FileInfo info = new FileInfo(FileNameTextBox.Text);
                ExtLabel.Text = info.Extension;
                if (".png".Equals(ExtLabel.Text.ToLower()) || ".bmp".Equals(ExtLabel.Text.ToLower()))
                {
                    GetTopLeftPixelColor(FileNameTextBox.Text);
                }
                FileSizeResultLabel.Text = FormatAddress((int)info.Length);
                StoreButton.Enabled = true;
            }
        }

        private void GetTopLeftPixelColor(string filename)
        {
            Bitmap png = new Bitmap(filename, false);
            Color clr = png.GetPixel(0, 0);
            topLeftPixelColor = clr.R * 256 * 256 + clr.G * 256 + clr.B;
            if (radioTopLeftColor.Checked)
            {
                textTransparentColor.Text = topLeftPixelColor.ToString("X6");
            }
        }

        private void StoreButton_Click(object sender, EventArgs e)
        {
            StoreButton.Enabled = false;

            // Store the address in the pointer address - little endian - 24 bits
            int destAddress = Convert.ToInt32(LoadAddressTextBox.Text.Replace(":", ""), 16);
            FileInfo info = new FileInfo(FileNameTextBox.Text);
            byte MCRHigh = (byte)(MemMgrRef.VICKY.ReadByte(1) & 3);
            int screenResX = 640;
            int screenResY = 480;
            switch(MCRHigh)
            {
                case 1:
                    screenResX = 800;
                    screenResY = 600;
                    break;
                case 2:
                    screenResX = 320;
                    screenResY = 240;
                    break;
                case 3:
                    screenResX = 400;
                    screenResY = 300;
                    break;
            }

            ResourceType operationType = ResourceType.raw;
            int conversionStride = 0;
            int maxHeight = screenResY;
            if (FileTypesCombo.SelectedIndex < 2)
            {
                operationType = ResourceType.bitmap;
                conversionStride = screenResX;
            }
            else if (FileTypesCombo.SelectedIndex < 6)
            { 
                operationType = ResourceType.tilemap;
                ExtLabel.Text = ".data";
            }
            else if (FileTypesCombo.SelectedIndex < 14)
            {
                operationType = ResourceType.tileset;
                conversionStride = 256;
                maxHeight = 256;
            }
            else if (FileTypesCombo.SelectedIndex < 78)
            {
                operationType = ResourceType.sprite;
                conversionStride = 32;
                maxHeight = 32;
            }
            else
            {
                operationType = ResourceType.lut;
                ExtLabel.Text = ".data";
            }

            ResourceChecker.Resource res = new ResourceChecker.Resource
            {
                StartAddress = destAddress,
                SourceFile = FileNameTextBox.Text,
                Name = Path.GetFileNameWithoutExtension(FileNameTextBox.Text),
                FileType = operationType,
            };


            switch (ExtLabel.Text.ToLower())
            {
                case ".png":
                    Bitmap png = new Bitmap(FileNameTextBox.Text, false);
                    ConvertBitmapToRaw(png, res, (byte)LUTCombo.SelectedIndex, conversionStride, maxHeight);
                    break;
                case ".bmp":
                    Bitmap bmp = new Bitmap(FileNameTextBox.Text, false);
                    ConvertBitmapToRaw(bmp, res, (byte)LUTCombo.SelectedIndex, conversionStride, maxHeight);
                    break;
                default:
                    // Read the file as raw
                    byte[] data = File.ReadAllBytes(FileNameTextBox.Text);
                    // Check if there's a resource conflict
                    res.Length = data.Length;
                    if (ResChecker.Add(res))
                    {
                        MemMgrRef.CopyBuffer(data, 0, destAddress, data.Length);
                    }
                    else
                    {
                        res.Length = -1;
                    }
                    break;
            }

            if (res.Length > 0)
            {
                // write address offset by bank $b0
                int imageAddress = destAddress - 0xB0_0000;
                int regAddress = -1;
                byte lutValue = (byte)LUTCombo.SelectedIndex;

                // Determine which addresses to store the bitmap into.
                if (FileTypesCombo.SelectedIndex < 2)
                {
                    // Bitmaps
                    regAddress = MemoryLocations.MemoryMap.BITMAP_CONTROL_REGISTER_ADDR + FileTypesCombo.SelectedIndex * 8;
                    // enable the bitmap - TODO add the LUT
                    MemMgrRef.WriteByte(regAddress,(byte)(1 + lutValue * 2));
                    
                }
                else if (FileTypesCombo.SelectedIndex < 6)
                {
                    // Tilemaps 4
                    int tilemapIndex = FileTypesCombo.SelectedIndex - 1;
                    regAddress = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + tilemapIndex * 12;
                    
                    // enable the tilemap
                    MemMgrRef.WriteByte(regAddress, (byte)(1 + (lutValue << 1)));

                    // TODO: Need to write the size of the tilemap
                }
                else if (FileTypesCombo.SelectedIndex < 14)
                {
                    // Tilesets 8
                    int tilesetIndex = FileTypesCombo.SelectedIndex - 5;
                    regAddress = MemoryLocations.MemoryMap.TILESET_BASE_ADDR + tilesetIndex * 4;

                    MemMgrRef.WriteByte(regAddress + 3, lutValue);  // TODO: Add the stride 256 bit 3.
                }
                else
                {
                    // Sprites 64
                    int spriteIndex = FileTypesCombo.SelectedIndex - 14;
                    regAddress = MemoryLocations.MemoryMap.SPRITE_CONTROL_REGISTER_ADDR + spriteIndex * 8;

                    // enable the tilemap
                    MemMgrRef.WriteByte(regAddress, (byte)(1 + (lutValue << 1)));  // TODO: Add sprite depth
                                                                                   // write address offset by bank $b0
                    // Set the sprite at (32,32)
                    MemMgrRef.WriteWord(regAddress + 4, 32);
                    MemMgrRef.WriteWord(regAddress + 6, 32);
                }
                // write address offset by bank $b0
                MemMgrRef.WriteByte(regAddress + 1, LowByte(imageAddress));
                MemMgrRef.WriteByte(regAddress + 2, MidByte(imageAddress));
                MemMgrRef.WriteByte(regAddress + 3, HighByte(imageAddress));

                StoreButton.Enabled = true;
            }
            if (res.Length != -1)
            {
                this.DialogResult = DialogResult.OK;
                if (FileTypesCombo.SelectedIndex > 1 && FileTypesCombo.SelectedIndex < 6)
                {
                    int layer = FileTypesCombo.SelectedIndex - 2;
                    //OnTileLoaded?.Invoke(layer);
                }
                Close();
            }
            else
            {
                // Keep the Asset Loader open
                StoreButton.Enabled = true;
            }
            
        }

        private unsafe void ConvertBitmapToRaw(Bitmap bitmap, ResourceChecker.Resource resource, byte lutIndex, int stride, int maxHeight)
        {             
            if (ResChecker.Add(resource))
            {
                

                // Load LUT from memory - ignore indexes 0 and 1
                int lutBaseAddress = MemoryLocations.MemoryMap.GRP_LUT_BASE_ADDR + lutIndex * 0x400 - MemoryLocations.MemoryMap.VICKY_BASE_ADDR;

                // Limit how much data is imported based on the type of image
                int importedLines = maxHeight < bitmap.Height ? maxHeight : bitmap.Height;
                int importedCols = stride < bitmap.Width ? stride : bitmap.Width;

                byte[] data = new byte[stride * importedLines]; // the bitmap is based on resolution of the machine
                resource.Length = stride * bitmap.Height;  // one byte per pixel - palette is separate

                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int bytesPerPixel = bitmapData.Stride / bitmap.Width;
                byte* bitmapPointer = (byte*)bitmapData.Scan0.ToPointer();
                bool tooManyColours = false;
                bool done = false;
                byte mask = 0xFF;
                List<int> lut = null;
                while (!done && mask != 0xc0)
                {
                    int transparentColor = 0;
                    try
                    {
                        transparentColor = Convert.ToInt32(textTransparentColor.Text, 16) & ((mask << 16) + (mask << 8) + mask);
                    }
                    finally
                    { }

                    done = true;
                    // Reset the Lookup Table
                    lut = new List<int>(256)
                    {
                        // Always add black (transparent) and white
                        0,
                        0xFFFFFF
                    };

                    // The user may decide to overwrite the palette from this bitmap
                    if (!checkOverwriteLUT.Checked)
                    {
                        for (int i = 2; i < 256; i++)
                        {
                            int value = MemMgrRef.VICKY.ReadLong(lutBaseAddress + 4 * i);
                            if (value != 0)
                            {
                                lut.Add(value);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    for (int line = 0; line < importedLines; line++)
                    {
                        for (int col = 0; col < importedCols; col++)
                        {
                            byte b = 0;
                            byte r = 0;
                            byte g = 0;

                            switch (bytesPerPixel)
                            {
                                case 1:
                                    byte palIndex = bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel];
                                    System.Drawing.Color palValue = bitmap.Palette.Entries[palIndex];
                                    b = (byte)(palValue.B & mask);
                                    g = (byte)(palValue.G & mask);
                                    r = (byte)(palValue.R & mask);
                                    break;
                                case 2:
                                    ushort wordValue = (ushort)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel] + bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 1] * 256);
                                    b = (byte)(wordValue & 0x1F);  //  5bits
                                    g = (byte)((wordValue >> 5) & 0x3F); // 6 bits
                                    r = (byte)(wordValue >> 11); // 5 bits
                                    break;
                                case 3:
                                    b = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel] & mask);
                                    g = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 1] & mask);
                                    r = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 2] & mask);
                                    break;
                                case 4:
                                    b = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel] & mask);
                                    g = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 1] & mask);
                                    r = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 2] & mask);
                                    //alpha is ignored

                                    break;
                            }
                            int rgb = b + g * 256 + r * 256 * 256;
                            // Check if the RBG matches the transparent color
                            int index = 0;
                            if (rgb != transparentColor )
                            {
                                // if not, look in the LUT
                                index = lut.IndexOf(rgb);
                            }
                            // If the index is undefined, add a new entry
                            if (index == -1)
                            {
                                if (lut.Count < 256)
                                {
                                    lut.Add(rgb);
                                    index = (byte)lut.IndexOf(rgb);
                                }
                                else
                                {
                                    tooManyColours = true;
                                    break;
                                }
                            }
                            if (index != -1)
                            {
                                data[line * stride + col] = (byte)index;
                            }
                        }
                        if (tooManyColours)
                        {
                            // TODO should use a colour histogram to count how many times a colour is used and then decimate here, based on low usage.
                            done = false;
                            tooManyColours = false;
                            mask <<= 1;
                            break;
                        }
                    }
                }

                if (mask != 0xc0)
                {
                    int videoAddress = resource.StartAddress - 0xB0_0000;

                    MemMgrRef.VIDEO.CopyBuffer(data, 0, videoAddress, data.Length);

                    if (lut != null)
                    {
                        for (int i = 0; i < lut.Count; i++)
                        {
                            int rbg = lut[i];
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i, LowByte(rbg));
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 1, MidByte(rbg));
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 2, HighByte(rbg));
                        }
                    }

                    // Check if a LUT matching our index is present in the Resources, if so don't do anything.
                    Resource resLut = ResChecker.Find(ResourceType.lut, lutBaseAddress + MemoryLocations.MemoryMap.VICKY_BASE_ADDR);
                    if (resLut == null)
                    {
                        Resource lutPlaceholder = new Resource
                        {
                            Length = 0x400,
                            FileType = ResourceType.lut,
                            Name = "Generated LUT",
                            StartAddress = lutBaseAddress + MemoryLocations.MemoryMap.VICKY_BASE_ADDR
                        };
                        ResChecker.Add(lutPlaceholder);
                    }
                }
                else
                {
                    MessageBox.Show("An error occured converting the image colors to LUT.\n" +
                        "You can try loading the image with a different LUT or\n" +
                        "Zero one of the LUTs or\n" +
                        "Check the Overwrite Existing LUT checkbox");
                    
                    ResChecker.Items.Remove(resource);
                    resource.Length = -1;
                }

            }
            else
            {
                resource.Length = -1;
            }
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
                            MemMgrRef.WriteByte(value * 4 + lutPointer, data[pointer]);
                            MemMgrRef.WriteByte(value * 4 + 1 + lutPointer, data[pointer + 1]);
                            MemMgrRef.WriteByte(value * 4 + 2 + lutPointer, data[pointer + 2]);
                            MemMgrRef.WriteByte(value * 4 + 3 + lutPointer, 0xFF);
                        }
                        MemMgrRef.WriteByte(videoPointer++, value);
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

        private void radioCustomColor_CheckedChanged(object sender, EventArgs e)
        {
            textTransparentColor.ReadOnly = false;
        }

        private void radioBlack_CheckedChanged(object sender, EventArgs e)
        {
            textTransparentColor.ReadOnly = true;
            textTransparentColor.Text = "000000";
        }

        private void radioTopLeftColor_CheckedChanged(object sender, EventArgs e)
        {
            textTransparentColor.ReadOnly = true;
            textTransparentColor.Text = topLeftPixelColor.ToString("X6");
        }
    }
}
