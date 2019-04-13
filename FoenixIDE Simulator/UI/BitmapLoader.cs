using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class BitmapLoader : Form
    {
        public Common.IMappable Memory = null;

        int controlRegisterAddress = 0;
        int pointerAddress = 0;
        int strideXAddress = 0;
        int strideYAddress = 0;
        int strideX = 0;
        int strideY = 0;
        Bitmap bitmap = null;
        byte controlByte = 0;

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


        private String formatAddress(int address)
        {
            String size = (address).ToString("X");
            return "$" + size.Substring(0, 2) + ":" + size.Substring(2);
        }
        /*
         * Let the user select a file from the file system and display it in a text box.
         */
        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = ".bin";
            openFileDlg.Filter = "Bitmap Files (.bmp)|*.bmp";

            // Set initial directory    
            //openFileDlg.InitialDirectory = @"C:\Temp\";

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
            bitmap = new Bitmap(bmpFilename);
            BitmapSizeValueLabel.Text = bitmap.Width + " x " + bitmap.Height;
            FileSizeResultLabel.Text = formatAddress(bitmap.Width * bitmap.Height);
            PixelDepthValueLabel.Text = (((int)bitmap.PixelFormat) >> 8 & 0xFF).ToString();

            switch (bitmap.Width)
            {
                case 640:
                    BitmapTypesCombo.SelectedItem = "Bitmap";
                    break;
                case 16:
                    BitmapTypesCombo.SelectedItem = "Tile Layer 0";
                    break;
                case 32:
                    BitmapTypesCombo.SelectedItem = "Sprite 0";
                    break;
            }
        }

        private void LoadAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            string item = LoadAddressTextBox.Text.Replace(":","");
            int n = 0;
            if (!int.TryParse(item, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.CurrentInfo, out n) &&
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
                    break;
                default: // sprites 0..31
                    int offsetSP = ((BitmapTypesCombo.SelectedIndex - 5) * 8);
                    controlRegisterAddress = 0xAF_0200; // 1 byte
                    pointerAddress = 0XAF_0201; // 3 bytes
                    strideXAddress = 0xAF_0204; // 2 bytes --> X Position
                    strideYAddress = 0xAF_0206; // 2 bytes --> Y Position
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

            // The addresses in Vicky a offset by $B0:0000
            videoAddress = videoAddress - 0xB0_0000;
            Memory.WriteByte(pointerAddress, lowByte(videoAddress));
            Memory.WriteByte(pointerAddress + 1, midByte(videoAddress));
            Memory.WriteByte(pointerAddress + 2, highByte(videoAddress));

            // Store the strides in the strideX and strideY
            Memory.WriteByte(strideXAddress, lowByte(strideX));
            Memory.WriteByte(strideXAddress + 1, midByte(strideX));
            Memory.WriteByte(strideYAddress, lowByte(strideY));
            Memory.WriteByte(strideYAddress + 1, midByte(strideY));

            // Store the bitmap at the user's determined address
            // The method below simply takes the file and writes it in memory.
            // What we want is the actual pixels.
            ImageConverter converter = new ImageConverter();
            byte[] data = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            int startOffset = BitConverter.ToInt32(data, 10);
            int fileLength = BitConverter.ToInt32(data, 2);

            int numberOfColors = BitConverter.ToInt32(data, 46);
            if (numberOfColors == 0)
            {
                // we need to create a LUT - each LUT only accepts 256 entries - 0 is black

            }
            else
            {
                int lutOffset = 0xAF_2000 + LUTCombo.SelectedIndex * 1024; 
                for (int offset = 54; offset < 1024 + 54; offset = offset + 4)
                { 
                    int color = BitConverter.ToInt32(data, offset);
                    Memory.WriteByte(lutOffset, lowByte(color));
                    Memory.WriteByte(lutOffset + 1, midByte(color));
                    Memory.WriteByte(lutOffset + 2, highByte(color));
                    Memory.WriteByte(lutOffset + 3, 0xFF); // Alpha
                    lutOffset = lutOffset + 4;
                }
            }
            for (int line = 0; line < bitmap.Height; line++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    Memory.WriteByte(writeVideoAddress + (bitmap.Height-line)*bitmap.Width + i, data[startOffset + line*bitmap.Width + i]);
                }
            }
            MessageBox.Show("Transfer successful!", "Bitmap Storage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private byte highByte(int value)
        {
            return ((byte)(value >> 16));
        }

        private byte midByte(int value)
        {
            return ((byte)((value >> 8) & 0xFF));
        }
        private byte lowByte(int value)
        {
            return ((byte)(value & 0xFF));
        }
    }
}
