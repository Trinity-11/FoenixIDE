using FoenixIDE.MemoryLocations;
using FoenixIDE.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private int selectedTilemap = 0;

        private MemoryManager memory;

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
            Tilemap0Button_Click(Tilemap0Button, null);
            TilesetList.SelectedIndex = 0;
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
            int lut = LutList.SelectedIndex;
            int tilesetAddress = Convert.ToInt32(TilesetAddress.Text, 16) - 0xB0_0000;
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    byte pixel = memory.VIDEO.ReadByte(tilesetAddress + y * 256 + x);
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

        public void Tilemap0Button_Click(object sender, EventArgs e)
        {
            Button selected = (Button)sender;
            // disable the previous button
            Tilemap0Button.BackColor = SystemColors.Control;
            Tilemap1Button.BackColor = SystemColors.Control;
            Tilemap2Button.BackColor = SystemColors.Control;
            Tilemap3Button.BackColor = SystemColors.Control;
            selectedTilemap = Convert.ToInt32(selected.Tag);
            selected.BackColor = SystemColors.ActiveCaption;

            int addrOffset = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            // show if the tilemap is enabled - ignore the LUT, it's not used
            int ControlReg = memory.ReadByte(addrOffset);
            TilemapEnabledCheckbox.Checked = (ControlReg & 1) != 0;
            // address in memory
            int tilemapAddr = memory.ReadLong(addrOffset + 1);
            TilemapAddress.Text = (tilemapAddr + 0xB0_0000).ToString("X6");

            int width = memory.ReadWord(addrOffset + 4);
            int height = memory.ReadWord(addrOffset + 6);
            Width.Text = width.ToString();
            Height.Text = height.ToString();

            int windowX = memory.ReadWord(addrOffset + 8);
            int windowY = memory.ReadWord(addrOffset + 10);
            WindowX.Text = windowX.ToString();
            WindowY.Text = windowY.ToString();
        }

        /**
         * When a tile is clicked in the GPU window, write the selected tile in memory.
         */
        public void TileClicked_Click(Point tile)
        {

            int tilemapAddress = Convert.ToInt32(TilemapAddress.Text, 16);
            int offset = (tile.Y * Convert.ToInt32(Width.Text) + tile.X + 1) * 2;
            if (selectedX != -1 && selectedY != -1)
            {
                byte value = (byte)(selectedY * 16 + selectedX);
                memory.WriteByte(tilemapAddress + offset, value);
            }
        }

        private void TilemapEnabledCheckbox_Click(object sender, EventArgs e)
        {
            int addrOffset = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            byte ControlReg = memory.ReadByte(addrOffset);
            ControlReg = (byte)((ControlReg & 0xF0) + (TilemapEnabledCheckbox.Checked ? 1 : 0));
            memory.WriteByte(addrOffset, ControlReg);
        }

        private void TileEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void ClearTilemapButton_Click(object sender, EventArgs e)
        {
            int tilemapAddress = Convert.ToInt32(TilemapAddress.Text, 16);
            int width = Convert.ToInt32(Width.Text);
            int height = Convert.ToInt32(Height.Text);
            for (int i = 0; i < width * height * 2; i++)
            {
                memory.WriteByte(tilemapAddress + i, 0);
            }
        }

        private void SaveTilemapButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Title = "Save Tilemap File",
                CheckPathExists = true,
                Filter = "Tilemap|*.data"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream dataFile = File.Create(saveDialog.FileName, 0x800, FileOptions.SequentialScan);
                int tilemapAddress = Convert.ToInt32(TilemapAddress.Text, 16);
                int width = Convert.ToInt32(Width.Text);
                int height = Convert.ToInt32(Height.Text);
                for (int i = 0; i < width * height * 2; i++)
                {
                    byte value = memory.ReadByte(tilemapAddress + i);
                    dataFile.WriteByte(value);
                }
                dataFile.Close();
            }
        }

        private void LutList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tilesetBaseAddr = MemoryLocations.MemoryMap.TILESET_BASE_ADDR + TilesetList.SelectedIndex * 4;
            byte ConfigRegister = (byte)(Stride256Checkbox.Checked? 8:0 + LutList.SelectedIndex);
            memory.WriteByte(tilesetBaseAddr + 3, ConfigRegister);
            TilesetViewer.Refresh();
        }

        private void TilesetAddress_TextChanged(object sender, EventArgs e)
        {
            int tilesetBaseAddr = MemoryLocations.MemoryMap.TILESET_BASE_ADDR + TilesetList.SelectedIndex * 4;
            int newAddress = Convert.ToInt32(TilesetAddress.Text.Replace(":", ""), 16);
            int offsetAddress = newAddress - 0xB0_0000;
            if (offsetAddress > -1)
            {
                memory.WriteLong(tilesetBaseAddr, offsetAddress);
            }
        }

        private void TilemapAddress_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            int newAddress = Convert.ToInt32(TilemapAddress.Text.Replace(":", ""), 16);
            int offsetAddress = newAddress - 0xB0_0000;
            if (offsetAddress > -1)
            {
                memory.WriteLong(tilemapBaseAddr + 1, offsetAddress);
            }
        }

        private void Width_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            int newValue = Convert.ToInt32(Width.Text) & 0x3FF;
            memory.WriteWord(tilemapBaseAddr + 4, newValue);
        }

        private void Height_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            int newValue = Convert.ToInt32(Height.Text) & 0x3FF;
            memory.WriteWord(tilemapBaseAddr + 6, newValue);
        }

        private void WindowX_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            int newValue = Convert.ToInt32(WindowX.Text) & 0x3FF;
            memory.WriteWord(tilemapBaseAddr + 8, newValue);
        }

        private void WindowY_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            int newValue = Convert.ToInt32(WindowY.Text) & 0x3FF;
            memory.WriteWord(tilemapBaseAddr + 10, newValue);
        }

        private void TilemapEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR + selectedTilemap * 12;
            memory.WriteByte(tilemapBaseAddr, (byte)(TilemapEnabledCheckbox.Checked ? 1 : 0));
        }

        private void TilesetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tilesetBaseAddr = MemoryLocations.MemoryMap.TILESET_BASE_ADDR + TilesetList.SelectedIndex * 4;
            int tilesetAddr = memory.ReadLong(tilesetBaseAddr);
            TilesetAddress.Text = (tilesetAddr + 0xB0_0000).ToString("X6");
            int cfgReg = memory.ReadByte(tilesetBaseAddr + 3);
            Stride256Checkbox.Checked = (cfgReg & 8) != 0;
            LutList.SelectedIndex = cfgReg & 7;
        }
    }
}
