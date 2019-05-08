using FoenixIDE.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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

        private Pen yellowPen = new Pen(Color.Yellow);
        private Pen redPen = new Pen(Color.Red);
        private Brush whiteBrush = new SolidBrush(Color.White);
        private int[][] graphicsLUT;

        public TileEditor()
        {
            InitializeComponent();
        }

        private void TileEditor_Load(object sender, EventArgs e)
        {
            Layer0Button_Click(Layer0Button, null);
            graphicsLUT = Display.Gpu.LoadLUT(memory.IO);
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
            int[] LUT = graphicsLUT[Int32.Parse(LUTDomain.Text)];
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    byte pixel = memory.VIDEO.ReadByte(LayersetAddress + y * 256 + x);
                    int color = LUT[pixel];
                    int destX = x / 16 * TILE_WIDTH + x % 16 + 1;
                    int destY = y / 16 * TILE_WIDTH + y % 16 + 1;
                    System.Runtime.InteropServices.Marshal.WriteInt32(p, (destY * stride + destX * 4) , color);
                }
            }
            frameBuffer.UnlockBits(bitmapData);
            e.Graphics.DrawImage(frameBuffer, rect);
            frameBuffer.Dispose();
            if (hoverX > -1 && hoverY > -1)
            {
                e.Graphics.DrawRectangle(yellowPen, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH, TILE_WIDTH + 1, TILE_WIDTH + 1);
                e.Graphics.DrawString("$" + (hoverY * 16 + hoverX).ToString("X2"), SystemFonts.DefaultFont, whiteBrush, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH + 2);
            }
            if (selectedX > -1 && selectedY > -1)
            {
                e.Graphics.DrawRectangle(redPen, selectedX * TILE_WIDTH, selectedY * TILE_WIDTH, TILE_WIDTH + 1, TILE_WIDTH + 1);
            }
        }

        private void TilesetViewer_MouseClick(object sender, MouseEventArgs e)
        {
            selectedX = e.X / TILE_WIDTH;
            selectedY = e.Y / TILE_WIDTH;
            TileSelectedLabel.Text = "Tile Selected: $" + (selectedY * 16 + selectedX).ToString("X2");
            TilesetViewer.Refresh();
        }

        private void Layer0Button_Click(object sender, EventArgs e)
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
                memory.IO.WriteByte(tilemapAddress - memory.IO.StartAddress, value);
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
            TilesetViewer.Refresh();
        }
    }
}
