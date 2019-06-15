using FoenixIDE.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class TileEditor : Form
    {
        private int selectedX = -1;
        private int selectedY = -1;
        private int hoverX = -1;
        private int hoverY = -1;
        private const int TILE_WIDTH = 17;
        private int layer = 0;
        private MemoryManager memory;
        int LayersetAddress = 0;

        private Pen whitePen = new Pen(Color.White);
        private Pen yellowPen = new Pen(Color.Yellow);
        private Pen redPen = new Pen(Color.Red);
        private Brush whiteBrush = new SolidBrush(Color.White);

        public TileEditor()
        {
            InitializeComponent();
        }

        private void TileEditor_Load(object sender, EventArgs e)
        {
            Layer0Button_Click(Layer0Button, null);
        }

        /**
         * When the user moves the mouse, highlight the border in yellow and print the number.
         */
        private void TilesetViewer_MouseMove(object sender, MouseEventArgs e)
        {
            hoverX = e.X / TILE_WIDTH;
            hoverY = e.Y / TILE_WIDTH;
            TilesetViewer.Refresh();
        }

        public void SetMemory(MemoryManager memMgr)
        {
            memory = memMgr;
        }

        /**
         * Draw the tileset with clear lines separating the images 16x16.
         */
        private void TilesetViewer_Paint(object sender, PaintEventArgs e)
        {
            // Read the memory and display the tiles
            Rectangle rect = new Rectangle(0, 0, 16 * 17 + 1, 16 * 17 + 1);
            Bitmap frameBuffer = new Bitmap(16 * 17 + 1, 16 * 17 + 1, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr p = bitmapData.Scan0;
            int stride = bitmapData.Stride;
            int[] graphicsLUT = Display.Gpu.LoadLUT(memory.VICKY);
            int lut = Int32.Parse(LUTDomain.Text);
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    byte pixel = memory.VIDEO.ReadByte(LayersetAddress + y * 256 + x);
                    if (pixel != 0)
                    {
                        int color = graphicsLUT[lut * 256 + pixel];
                        int destX = x / 16 * TILE_WIDTH + x % 16 + 1;
                        int destY = y / 16 * TILE_WIDTH + y % 16 + 1;
                        System.Runtime.InteropServices.Marshal.WriteInt32(p, (destY * stride + destX * 4), color);
                    }
                }
            }
            frameBuffer.UnlockBits(bitmapData);
            e.Graphics.DrawImageUnscaled(frameBuffer, rect);
            frameBuffer.Dispose();
            if (hoverX > -1 && hoverY > -1)
            {
                e.Graphics.DrawRectangle(whitePen, hoverX * TILE_WIDTH-1, hoverY * TILE_WIDTH-1, TILE_WIDTH+2, TILE_WIDTH+2);
                e.Graphics.DrawRectangle(yellowPen, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH, TILE_WIDTH, TILE_WIDTH);
                e.Graphics.DrawString((hoverY * 16 + hoverX).ToString("X2"), SystemFonts.DefaultFont, whiteBrush, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH + 2);
            }
            if (selectedX > -1 && selectedY > -1)
            {
                e.Graphics.DrawRectangle(redPen, selectedX * TILE_WIDTH - 1, selectedY * TILE_WIDTH - 1, TILE_WIDTH + 2, TILE_WIDTH + 2);
                e.Graphics.DrawRectangle(redPen, selectedX * TILE_WIDTH, selectedY * TILE_WIDTH, TILE_WIDTH, TILE_WIDTH);
            }
        }

        private void TilesetViewer_MouseClick(object sender, MouseEventArgs e)
        {
            selectedX = e.X / TILE_WIDTH;
            selectedY = e.Y / TILE_WIDTH;
            TileSelectedLabel.Text = "Tile Selected: $" + (selectedY * 16 + selectedX).ToString("X2");
            TilesetViewer.Refresh();
        }

        public void SelectLayer(int layer)
        {
            Button selectedButton = null;
            switch (layer)
            {
                case 0:
                    selectedButton = Layer0Button;
                    break;
                case 1:
                    selectedButton = Layer1Button;
                    break;
                case 2:
                    selectedButton = Layer2Button;
                    break;
                case 3:
                    selectedButton = Layer3Button;
                    break;
            }
            Layer0Button_Click(selectedButton, null);
        }

        public void Layer0Button_Click(object sender, EventArgs e)
        {
            Button selected = (Button)sender;
            // disable the previous button
            Layer0Button.BackColor = SystemColors.Control;
            Layer1Button.BackColor = SystemColors.Control;
            Layer2Button.BackColor = SystemColors.Control;
            Layer3Button.BackColor = SystemColors.Control;
            layer = Convert.ToInt32(selected.Tag);
            selected.BackColor = SystemColors.ActiveCaption;

            int addrOffset = 0xAF_0100 + layer * 8;
            int ControlReg = memory.ReadByte(addrOffset);
            LayersetAddress = memory.ReadLong(addrOffset + 1);
            int LUT = (ControlReg >> 1) & 3;
            LUTDomain.Text = LUT.ToString();

            TilesetAddressText.Text = (LayersetAddress + 0xB0_0000).ToString("X6");

            int StrideX = memory.ReadWord(addrOffset + 4);
            int StrideY = memory.ReadWord(addrOffset + 6);
            LayerEnabledCheckbox.Checked = (ControlReg & 1) == 1;
            StrideXText.Text = StrideX.ToString("X4");
            StrideYText.Text = StrideY.ToString("X4");
            TilesetViewer.Refresh();
        }

        /**
         * When a tile is clicked in the GPU window, write the selected tile in memory.
         */
        public void TileClicked_Click(Point tile)
        {
            int tilemapAddress = 0xAF5000 + 0x800 * layer + tile.Y * 64 + tile.X;
            if (selectedX != -1 && selectedY != -1)
            {
                byte value = (byte)(selectedY * 16 + selectedX);
                memory.VICKY.WriteByte(tilemapAddress - memory.VICKY.StartAddress, value);
            }
        }

        private void LayerEnabledCheckbox_Click(object sender, EventArgs e)
        {
            int addrOffset = 0xAF_0100 + layer * 8;
            byte ControlReg = memory.ReadByte(addrOffset);
            ControlReg ^= 1;
            memory.WriteByte(addrOffset, ControlReg);
        }

        private void LUTDomain_SelectedItemChanged(object sender, EventArgs e)
        {
            if (sender is DomainUpDown)
            {
                int addrOffset = 0xAF_0100 + layer * 8;
                byte ControlReg = memory.ReadByte(addrOffset);
                byte lut = Convert.ToByte(LUTDomain.Text);
                ControlReg = (byte)(ControlReg | (lut << 1));
                memory.WriteByte(addrOffset, ControlReg);
            }
            TilesetViewer.Refresh();
        }

        private void TilesetAddressText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tilesetAddress = Convert.ToInt32(TilesetAddressText.Text, 16) - 0xB0_0000;
                if (tilesetAddress >= 0)
                {
                    
                    int addrOffset = 0xAF_0100 + layer * 8;
                    memory.WriteLong(addrOffset + 1, tilesetAddress);
                }
            }
            catch
            {

            }
        }

        private void StrideXText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int strideValue = Convert.ToInt16(TilesetAddressText.Text, 16);
                if (strideValue >= 0)
                {
                    int addrOffset = 0xAF_0100 + layer * 8;
                    memory.WriteWord(addrOffset + 4, strideValue);
                }
            }
            catch
            {

            }
        }

        private void StrideYText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int strideValue = Convert.ToInt16(TilesetAddressText.Text, 16);
                if (strideValue >= 0)
                {
                    int addrOffset = 0xAF_0100 + layer * 8;
                    memory.WriteWord(addrOffset + 6, strideValue);
                }
            }
            catch
            {

            }
        }
    }
}
