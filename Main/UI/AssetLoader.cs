using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.FileFormat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using static FoenixIDE.Simulator.FileFormat.ResourceChecker;

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
            // Add items to the combo box                 //index
            FileTypesCombo.Items.Add("Bitmap");           //0
            FileTypesCombo.Items.Add("Tileset 16x16");    //1
            FileTypesCombo.Items.Add("Tileset   8x8");    //2
            FileTypesCombo.Items.Add("Sprite");           //3
            FileTypesCombo.Items.Add("Tilemap");          //4
            FileTypesCombo.Items.Add("Palette");          //5
            FileTypesCombo.Items.Add("Binary");           //6
            LUTCombo.Items.Add("LUT");
            FileTypesCombo.SelectedIndex = 0;
            LUTCombo.SelectedIndex = 0;
        }

        /**
         * The LUT list box is only selected when loading bitmaps, sprites and tiles
         */
        private void FileTypesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool LUTSelected = FileTypesCombo.SelectedIndex > 3;
            LUTCombo.Enabled = !LUTSelected;
            checkOverwriteLUT.Enabled = !LUTSelected;
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
                Filter = "Asset Files (*.bmp *.png *.bin *.data *.pal *.tlm *.aseprite)|*.bmp;*.png;*.bin;*.data;*.pal;*.tlm;*.aseprite|Binary Files (*.bin)|*.bin|Palette Files (*.pal)|*.pal|Bitmap Files (*.bmp *.png)|*.bmp;*.png|Data Files|*.data|Tilemap Files (*.tlm)|*.tlm|Any File|*.*"
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
                if (".tlm".Equals(ExtLabel.Text.ToLower()))
                {
                    FileTypesCombo.SelectedIndex = 4;
                }
                else if (".pal".Equals(ExtLabel.Text.ToLower()))
                {
                    FileTypesCombo.SelectedIndex = 5;
                } 
                else if (".bin".Equals(ExtLabel.Text.ToLower()))
                {
                    FileTypesCombo.SelectedIndex = 6;
                }
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
            switch (FileTypesCombo.SelectedIndex)
            {
                case 0:  // bitmaps
                    operationType = ResourceType.bitmap;
                    conversionStride = screenResX;
                    break;
                case 1:  // tilesets 16 x 16
                    operationType = ResourceType.tileset;
                    conversionStride = 256;
                    maxHeight = 256;
                    break;
                case 2:  // tilesets 8 x 8
                    operationType = ResourceType.tileset;
                    conversionStride = 128;
                    maxHeight = 128;
                    break;
                case 3:  // sprites
                    operationType = ResourceType.sprite;
                    conversionStride = 32;
                    maxHeight = 256;
                    break;
                case 4:  // tilemaps
                    operationType = ResourceType.tilemap;
                    ExtLabel.Text = ".bin";
                    break;
                case 5:  // palettes
                    operationType = ResourceType.lut;
                    ExtLabel.Text = ".pal";
                    break;
                case 6: // others
                    operationType = ResourceType.raw;
                    break;
            }

            ResourceChecker.Resource res = new ResourceChecker.Resource
            {
                StartAddress = destAddress,
                SourceFile = FileNameTextBox.Text,
                Name = Path.GetFileNameWithoutExtension(FileNameTextBox.Text),
                FileType = operationType
            };


            switch (ExtLabel.Text.ToLower())
            {
                case ".png":
                    Bitmap png = new Bitmap(FileNameTextBox.Text, false);
                    if (FileTypesCombo.SelectedIndex == 1)
                    {
                        if (png.Width == 16)
                        {
                            conversionStride = 16;
                            maxHeight = 256 * 16;
                        }
                    }
                    else if(FileTypesCombo.SelectedIndex == 2)
                    {
                        if (png.Width == 8)
                        {
                            conversionStride = 8;
                            maxHeight = 256 * 8;
                        }
                    }

                    ConvertBitmapToRaw(png, res, (byte)LUTCombo.SelectedIndex, conversionStride, maxHeight);
                    break;
                case ".bmp":
                    Bitmap bmp = new Bitmap(FileNameTextBox.Text, false);
                    if (FileTypesCombo.SelectedIndex == 1)
                    {
                        if (bmp.Width == 16)
                        {
                            conversionStride = 16;
                            maxHeight = 256 * 16;
                        }
                    }
                    else if (FileTypesCombo.SelectedIndex == 2)
                    {
                        if (bmp.Width == 8)
                        {
                            conversionStride = 8;
                            maxHeight = 256 * 8;
                        }
                    }
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
            StoreButton.Enabled = res.Length > 0;
            if (AssetWindow.Instance != null)
            {
                AssetWindow.Instance.UpdateAssets();
                Close();
            }
        }

        /*
         * Convert an image file into 'bitmap', 'tileset' or 'sprites'
         * Bitmaps are converted as-is.
         * Tileset have a maximum width of 256 pixels.
         * Sprites are 32x32: the image may contain an array of sprites that need to be "de-interlaced".
         */
        private unsafe void ConvertBitmapToRaw(Bitmap bitmap, ResourceChecker.Resource resource, byte lutIndex, int stride, int maxHeight)
        {             
            if (ResChecker.Add(resource))
            {
                

                // Load LUT from memory - ignore indexes 0 and 1
                int lutBaseAddress = MemoryLocations.MemoryMap.GRP_LUT_BASE_ADDR + lutIndex * 0x400 - MemoryLocations.MemoryMap.VICKY_BASE_ADDR;

                // Limit how much data is imported based on the type of image
                int importedLines = maxHeight < bitmap.Height ? maxHeight : bitmap.Height;
                int importedCols = ((bitmap.Width / stride) > 0) ? (bitmap.Width / stride) * stride : bitmap.Width;
                
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                byte* bitmapPointer = (byte*)bitmapData.Scan0.ToPointer();
                int bytesPerPixel = bitmapData.Stride / bitmap.Width;

                byte[] data = new byte[importedCols * importedLines]; // the bitmap is based on resolution of the machine
                resource.Length = importedCols * importedLines;  // one byte per pixel - palette is separate

                bool tooManyColours = false;
                bool done = false;
                byte mask = 0xFF;
                List<int> lut = null;

                
                while (!done && mask != 0x80)
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
                                    // TODO - try this approximation
                                    //
                                    break;
                                case 4:
                                    b = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel] & mask);
                                    g = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 1] & mask);
                                    r = (byte)(bitmapPointer[line * bitmapData.Stride + col * bytesPerPixel + 2] & mask);
                                    //alpha is ignored

                                    break;
                            }
                            int rgb = rgb = b + g * 256 + r * 256 * 256;
                            if (rgb != 0 && bytesPerPixel == 3 && mask == 0xc0)
                            {
                                rgb = (r * 7 / 255) << 5 + (g * 7 / 255) << 2 + (b * 3 / 255);
                            }
                            
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
                                data[line * importedCols + col] = (byte)index;
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
                bitmap.UnlockBits(bitmapData);

                if (mask != 0xc0)
                {
                    int videoAddress = resource.StartAddress - 0xB0_0000;

                    // Reorganize the bitmap for sprites
                    if (stride < bitmap.Width)
                    {
                        byte[] deinterlaced = new byte[data.Length];  // same lenght as data

                        // Create a new bitmap to de-interlace the sprite sheet
                        int lineWidthInBytes = stride;
                        int stridelineCount = (bitmap.Height / stride) * (bitmap.Width / stride) * stride;
                        int lineWidth = bitmap.Width;

                        for (int y = 0; y < stridelineCount; y++)
                        {
                            for (int x = 0; x < lineWidthInBytes; x++)
                            {
                                deinterlaced[x + y * lineWidthInBytes] = data[x +
                                    (y / stride) * lineWidthInBytes +
                                    (y % lineWidthInBytes) * lineWidth +
                                    (y / bitmap.Width) * bitmap.Width * (stride -1)
                                    ];
                            }
                        }
                        MemMgrRef.VIDEO.CopyBuffer(deinterlaced, 0, videoAddress, deinterlaced.Length);
                    }
                    else
                    {
                        MemMgrRef.VIDEO.CopyBuffer(data, 0, videoAddress, data.Length);
                    }

                    if (lut != null)
                    {
                        for (int i = 0; i < lut.Count; i++)
                        {
                            int rbg = lut[i];
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i, LowByte(rbg));
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 1, MidByte(rbg));
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 2, HighByte(rbg));
                            MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 3, 0);
                        }
                        // if the Overwrite checkbox is checked, then complete with zeros
                        if (checkOverwriteLUT.Checked)
                        {
                            for (int i= lut.Count; i < 256; i++)
                            {
                                MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i, 0);
                                MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 1, 0);
                                MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 2, 0);
                                MemMgrRef.VICKY.WriteByte(lutBaseAddress + 4 * i + 3, 0);
                            }
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
