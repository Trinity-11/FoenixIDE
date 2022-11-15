using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static FoenixIDE.Simulator.FileFormat.ResourceChecker;

namespace FoenixIDE.Simulator.UI
{
    public partial class TileEditor : Form
    {

        private int selectedLeft = -1;
        private int selectedRight = -1;
        private int hoverX = -1;
        private int hoverY = -1;
        private const int TILE_WIDTH = 17;
        private int selectedTilemap = 0;

        private MemoryManager MemMgr;
        private ResourceChecker resCheckerRef;

        private Pen whitePen = new Pen(Color.White);
        private Pen yellowPen = new Pen(Color.Yellow);
        private Pen redPen = new Pen(Color.Red);
        private Brush whiteBrush = new SolidBrush(Color.White);
        private bool isJunior = false;
        private int tileRegistersBaseAddress;
        private int tilesetRegistersBaseAddress;

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
            int x = e.X / TILE_WIDTH;
            if (x < 16)
            {
                hoverX = x;
            }
            int y = e.Y / TILE_WIDTH;
            if (y < 16)
            {
                hoverY = y;
            }
            TilesetViewer.Refresh();
        }

        public void SetMemory(MemoryManager mm)
        {
            MemMgr = mm;
            if (mm.MMU != null)
            {
                isJunior = true;
                tileRegistersBaseAddress = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR_JR - 0xC000;
                tilesetRegistersBaseAddress = MemoryLocations.MemoryMap.TILESET_BASE_ADDR_JR - 0xC000;
            }
            else
            {
                tileRegistersBaseAddress = MemoryLocations.MemoryMap.TILE_CONTROL_REGISTER_ADDR - MemMgr.VICKY.StartAddress;
                tilesetRegistersBaseAddress = MemoryLocations.MemoryMap.TILESET_BASE_ADDR - MemMgr.VICKY.StartAddress;
            }
        }

        public void SetResourceChecker(ResourceChecker rc)
        {
            resCheckerRef = rc;
        }

        private int[] LoadLUT()
        {
            
            int lookupTables = 4;
            int[] result = new int[lookupTables * 256];


            for (int c = 0; c < lookupTables * 256; c++)
            {
                result[c] = MainWindow.Instance.gpu.GetLUTValue((byte)(c / 256), (byte)(c % 256), false);
            }
            return result;
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
            int[] graphicsLUT = LoadLUT();
            int lut = LutList.SelectedIndex;
            int tileSize = checkSmallTiles.Checked ? 8 : 16;
            int tileStride = tileSize * 16;
            int tilesetAddress = Convert.ToInt32(TilesetAddress.Text, 16);
            MemoryRAM memRef = MemMgr.RAM;
            if (!isJunior)
            {
                tilesetAddress = tilesetAddress - 0xB0_0000;
                memRef = MemMgr.VIDEO;
            }
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    byte pixel;
                    int posX = checkSmallTiles.Checked ? x / 2 : x;
                    int posY = checkSmallTiles.Checked ? y / 2 : y;
                    if (Stride256Checkbox.Checked)
                    {
                        pixel = memRef.ReadByte(tilesetAddress + (posY / tileStride) * tileStride * tileStride + (posY % tileStride) * tileStride + (posX / tileStride) * tileStride + posX % tileStride);
                    }
                    else
                    {
                        pixel = memRef.ReadByte(tilesetAddress + (posY / tileSize) * 16 * tileSize * tileSize + (posY % tileSize) * tileSize + (posX % tileSize) + posX / tileSize * tileSize * tileSize);
                    }
                    if (pixel != 0)
                    {
                        int color = graphicsLUT[lut * 256 + pixel];
                        int destX = x / 16 * TILE_WIDTH + x % 16 + 1;
                        int destY = y / 16 * TILE_WIDTH + y % 16 + 1;
                        System.Runtime.InteropServices.Marshal.WriteInt32(p, (destY * stride + destX * 4), color);
                        if (checkSmallTiles.Checked)
                        {
                            System.Runtime.InteropServices.Marshal.WriteInt32(p, (destY * stride + (destX + 1) * 4), color);
                            System.Runtime.InteropServices.Marshal.WriteInt32(p, ((destY + 1) * stride + (destX + 1) * 4), color);
                            System.Runtime.InteropServices.Marshal.WriteInt32(p, ((destY + 1) * stride + (destX) * 4), color);
                        }
                    }
                }
            }
            frameBuffer.UnlockBits(bitmapData);
            e.Graphics.DrawImageUnscaled(frameBuffer, rect);
            frameBuffer.Dispose();
            if (hoverX > -1 && hoverY > -1)
            {
                e.Graphics.DrawRectangle(whitePen, hoverX * TILE_WIDTH - 1, hoverY * TILE_WIDTH - 1, TILE_WIDTH + 2, TILE_WIDTH + 2);
                e.Graphics.DrawRectangle(yellowPen, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH, TILE_WIDTH, TILE_WIDTH);
                e.Graphics.DrawString((hoverY * 16 + hoverX).ToString("X2"), SystemFonts.DefaultFont, whiteBrush, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH + 2);
            }
            if (selectedLeft != -1)
            {
                int X = selectedLeft % 16;
                int Y = selectedLeft / 16;
                Point[] triangle = new Point[3];
                triangle[0] = new Point(X * TILE_WIDTH - 1, Y * TILE_WIDTH - 1);
                triangle[1] = new Point(X * TILE_WIDTH - 1 + TILE_WIDTH / 2, Y * TILE_WIDTH - 1);
                triangle[2] = new Point(X * TILE_WIDTH - 1, Y * TILE_WIDTH - 1 + TILE_WIDTH / 2);
                Brush redBrush = Brushes.Red;
                e.Graphics.FillPolygon(redBrush, triangle);
            }
            if (selectedRight != -1)
            {
                int X = selectedRight % 16;
                int Y = selectedRight / 16;
                Point[] triangle = new Point[3];
                triangle[0] = new Point((X + 1) * TILE_WIDTH + 1, (Y + 1) * TILE_WIDTH + 1);
                triangle[1] = new Point((X + 1) * TILE_WIDTH + 1, Y * TILE_WIDTH + TILE_WIDTH / 2);
                triangle[2] = new Point(X * TILE_WIDTH + TILE_WIDTH / 2, (Y + 1) * TILE_WIDTH + 1);
                Brush redBrush = Brushes.Red;
                e.Graphics.FillPolygon(redBrush, triangle);
            }
        }

        private void leftTile_Paint(object sender, PaintEventArgs e)
        {
            if (selectedLeft != -1)
            {
                int size = checkSmallTiles.Checked ? 8 : 16;
                Rectangle rect = new Rectangle(0, 0, size, size);
                Bitmap frameBuffer = new Bitmap(size, size, PixelFormat.Format32bppArgb);
                BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                IntPtr p = bitmapData.Scan0;
                int stride = bitmapData.Stride;
                int[] graphicsLUT = LoadLUT();
                int lut = LutList.SelectedIndex;
                int tileStride = Stride256Checkbox.Checked ? size * 16 : size;
                int tilesetAddress = Convert.ToInt32(TilesetAddress.Text, 16) + 
                    (selectedLeft / 16) * (Stride256Checkbox.Checked ? tileStride : 16 * size) * size + 
                    (selectedLeft % 16) * (Stride256Checkbox.Checked ? size : size * size);
                MemoryRAM memRef = MemMgr.RAM;
                if (!isJunior)
                {
                    tilesetAddress = tilesetAddress - 0xB0_0000;
                    memRef = MemMgr.VIDEO;
                }
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        byte pixel = memRef.ReadByte(tilesetAddress + (y * tileStride + x));
                        if (pixel != 0)
                        {
                            int color = graphicsLUT[lut * 256  + pixel];
                            System.Runtime.InteropServices.Marshal.WriteInt32(p, (y * stride + x * 4), color);
                        }
                    }
                }
                e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                // Bilinear interpolation has effect very similar to real HW 
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                frameBuffer.UnlockBits(bitmapData);
                //frameBuffer.Save("c:\\temp\\test.bmp");
                e.Graphics.DrawImage(frameBuffer, leftTile.ClientRectangle);
                frameBuffer.Dispose();
            }
        }

        private void RightTile_Paint(object sender, PaintEventArgs e)
        {
            if (selectedRight != -1)
            {
                int size = checkSmallTiles.Checked ? 8 : 16;
                Rectangle rect = new Rectangle(0, 0, size, size);
                Bitmap frameBuffer = new Bitmap(size, size, PixelFormat.Format32bppArgb);
                BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                IntPtr p = bitmapData.Scan0;
                int stride = bitmapData.Stride;
                int[] graphicsLUT = LoadLUT();
                int lut = LutList.SelectedIndex;
                int tileStride = Stride256Checkbox.Checked ? size * 16 : size;
                int tilesetAddress = Convert.ToInt32(TilesetAddress.Text, 16) + 
                    (selectedRight / 16) * (Stride256Checkbox.Checked ? tileStride : 16 * size) * size +
                    (selectedRight % 16) * (Stride256Checkbox.Checked ? size : size * size);
                MemoryRAM memRef = MemMgr.RAM;
                if (!isJunior)
                {
                    tilesetAddress = tilesetAddress - 0xB0_0000;
                    memRef = MemMgr.VIDEO;
                }
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        byte pixel = memRef.ReadByte(tilesetAddress + (y * tileStride + x));
                        if (pixel != 0)
                        {
                            int color = graphicsLUT[lut * 256 + pixel];
                            System.Runtime.InteropServices.Marshal.WriteInt32(p, (y * stride + x * 4), color);
                        }
                    }
                }
                e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                // Bilinear interpolation has effect very similar to real HW 
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                frameBuffer.UnlockBits(bitmapData);
                e.Graphics.DrawImage(frameBuffer, RightTile.ClientRectangle);
                frameBuffer.Dispose();
            }
        }

        private void TilesetViewer_MouseClick(object sender, MouseEventArgs e)
        {
            int X = e.X / TILE_WIDTH;
            int Y = e.Y / TILE_WIDTH;
            int size = checkSmallTiles.Checked ? 8 : 16;
            if (e.Button == MouseButtons.Left)
            {
                selectedLeft = Y * 16 + X;
                LeftTileSelectedLabel.Text = "Left Tile: $" + (selectedLeft).ToString("X2");
                leftTile.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                selectedRight = Y * 16 + X;
                RightTileSelectedLabel.Text = "Right Tile: $" + (selectedRight).ToString("X2");
                RightTile.Refresh();
            }

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

            int addrOffset = tileRegistersBaseAddress + selectedTilemap * 12;
            // show if the tilemap is enabled - ignore the LUT, it's not used
            int ControlReg = MemMgr.VICKY.ReadByte(addrOffset);
            TilemapEnabledCheckbox.Checked = (ControlReg & 1) != 0;
            checkSmallTiles.Checked = (ControlReg & 0x10) != 0;
            // address in memory
            int tilemapAddr = MemMgr.VICKY.ReadLong(addrOffset + 1) & 0x3F_FFFF;
            TilemapAddress.Text = (tilemapAddr + (isJunior?0:0xB0_0000)).ToString("X6");

            int width = MemMgr.VICKY.ReadWord(addrOffset + 4) & 0x3FF;  // max 1024
            int height = MemMgr.VICKY.ReadWord(addrOffset + 6) & 0x3FF; // max 1024
            TilemapWidth.Text = width.ToString();
            TilemapHeight.Text = height.ToString();

            int windowX = MemMgr.VICKY.ReadWord(addrOffset + 8);
            int windowY = MemMgr.VICKY.ReadWord(addrOffset + 10);
            WindowX.Text = windowX.ToString();
            WindowY.Text = windowY.ToString();
        }

        /**
         * When a tile is clicked in the GPU window, write the selected tile in memory.
         */
        public void TileClicked_Click(Point tile, PointF ratios, bool leftButton)
        {

            int tilemapAddress = Convert.ToInt32(TilemapAddress.Text, 16);
            int tileSize = (checkSmallTiles.Checked ? 8 : 16);
            if (isJunior)
            { 
                tileSize *= 2;
            }
            int winX = 0;
            int winY = 0;
            if (WindowX.Text.Length > 0)
            {
                winX = Convert.ToInt32(WindowX.Text);
            }
            if (WindowY.Text.Length > 0)
            {
                winY = Convert.ToInt32(WindowY.Text);
            }
            int offset = ((tile.Y + (int)(winY/ratios.Y)) / tileSize * Convert.ToInt32(TilemapWidth.Text) + (tile.X + (int)(winX/ratios.X)) / tileSize + 1) * 2;

            if ((leftButton ? selectedLeft : selectedRight) != -1)
            {
                // Write the tile value
                byte value = (byte)(leftButton ? selectedLeft : selectedRight);
                MemMgr.WriteByte(tilemapAddress + offset, value);
                // Write the tileset and LUT - this way we can mix tiles from multiple tilesets in a single map
                MemMgr.WriteByte(tilemapAddress + offset + 1, (byte)((LutList.SelectedIndex << 3) + TilesetList.SelectedIndex));
            }
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
            byte selectedTile = (byte)(selectedLeft);
            int width = Convert.ToInt32(TilemapWidth.Text);
            int height = Convert.ToInt32(TilemapHeight.Text);
            byte lut = (byte)LutList.SelectedIndex;
            for (int i = 0; i < width * height * 2 + 1; i += 2)
            {
                MemMgr.WriteByte(tilemapAddress + i, selectedTile);
                MemMgr.WriteByte(tilemapAddress + i + 1, (byte)((lut << 3) + TilesetList.SelectedIndex));
            }
        }

        private void SaveTilemapButton_Click(object sender, EventArgs e)
        {
            // Create a new resource
            int tilemapAddress = Convert.ToInt32(TilemapAddress.Text, 16);
            int width = Convert.ToInt32(TilemapWidth.Text);
            int height = Convert.ToInt32(TilemapHeight.Text);
            Resource resource = new Resource()
            {
                Name = "Tile Editor Map",
                Length = width * height * 2,
                FileType = ResourceType.tilemap,
                StartAddress = tilemapAddress
            };

            // if this resource is already in the list of resource
            Resource oldRes = resCheckerRef.Find(ResourceType.tilemap, tilemapAddress);
            resCheckerRef.Items.Remove(oldRes);
            resCheckerRef.Add(resource);
            AssetWindow.Instance.UpdateAssets();
        }

        private void TilesetAddress_TextChanged(object sender, EventArgs e)
        {
            int tilesetBaseAddr = tilesetRegistersBaseAddress + TilesetList.SelectedIndex * 4;
            int newAddress = Convert.ToInt32(TilesetAddress.Text.Replace(":", ""), 16);
            int offsetAddress = newAddress;
            if (!isJunior)
            {
                offsetAddress = newAddress - 0xB0_0000;
            }
            if (offsetAddress > -1)
            {
                MemMgr.WriteLong(tilesetBaseAddr, offsetAddress);
            }
        }

        private void TilemapAddress_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = tileRegistersBaseAddress + selectedTilemap * 12;
            int newAddress = Convert.ToInt32(TilemapAddress.Text.Replace(":", ""), 16);
            int offsetAddress = newAddress - 0xB0_0000;
            if (offsetAddress > -1)
            {
                MemMgr.VICKY.WriteByte(tilemapBaseAddr + 1, (byte)(offsetAddress & 0xff));
                MemMgr.VICKY.WriteByte(tilemapBaseAddr + 2, (byte)(offsetAddress >> 8 & 0xff));
                MemMgr.VICKY.WriteByte(tilemapBaseAddr + 3, (byte)(offsetAddress >> 16 & 0xff));
            }
        }

        private void Width_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = tileRegistersBaseAddress + selectedTilemap * 12;
            if (TilemapWidth.Text.Length > 0)
            {
                int newValue = Convert.ToInt32(TilemapWidth.Text) & 0x3FF;
                MemMgr.VICKY.WriteWord(tilemapBaseAddr + 4, newValue);
            }
        }

        private void Height_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = tileRegistersBaseAddress + selectedTilemap * 12;
            if (TilemapHeight.Text.Length > 0)
            {
                int newValue = Convert.ToInt32(TilemapHeight.Text) & 0x3FF;
                MemMgr.VICKY.WriteWord(tilemapBaseAddr + 6, newValue);
            }
        }

        private void WindowX_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = tileRegistersBaseAddress + selectedTilemap * 12;
            if (WindowX.Text.Length > 0)
            {
                int newValue = Convert.ToInt32(WindowX.Text) & 0x3FF;
                MemMgr.VICKY.WriteWord(tilemapBaseAddr + 8, newValue);
            }
        }

        private void WindowY_TextChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = tileRegistersBaseAddress + selectedTilemap * 12;
            if (WindowY.Text.Length > 0)
            {
                int newValue = Convert.ToInt32(WindowY.Text) & 0x3FF;
                MemMgr.VICKY.WriteWord(tilemapBaseAddr + 10, newValue);
            }
        }

        private void checkSmallTiles_CheckedChanged(object sender, EventArgs e)
        {
            int tilemapBaseAddr = tileRegistersBaseAddress + selectedTilemap * 12;
            byte value = MemMgr.VICKY.ReadByte(tilemapBaseAddr);
            if (checkSmallTiles.Checked)
            {
                value |= 0x10;
            }
            else
            {
                value &= 0xF;
            }
            MemMgr.VICKY.WriteByte(tilemapBaseAddr, value);
            TilesetViewer.Refresh();
            leftTile.Refresh();
            RightTile.Refresh();
        }

        private void TilemapEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            int addrOffset = tileRegistersBaseAddress + selectedTilemap * 12;
            byte ControlReg = MemMgr.VICKY.ReadByte(addrOffset);
            ControlReg = (byte)((ControlReg & 0xF0) + (TilemapEnabledCheckbox.Checked ? 1 : 0));
            MemMgr.VICKY.WriteByte(addrOffset, ControlReg);
        }

        private void TilesetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tilesetBaseAddr = tilesetRegistersBaseAddress + TilesetList.SelectedIndex * 4;
            int tilesetAddr = MemMgr.VICKY.ReadLong(tilesetBaseAddr) & 0x3F_FFFF;
            TilesetAddress.Text = (tilesetAddr + (isJunior?0:0xB0_0000)).ToString("X6");
            int cfgReg = MemMgr.VICKY.ReadByte(tilesetBaseAddr + 3);
            Stride256Checkbox.Checked = (cfgReg & 8) != 0;
            LutList.SelectedIndex = cfgReg & 7;
            TilesetViewer.Refresh();
        }

        private void LutList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tilesetBaseAddr = tilesetRegistersBaseAddress + TilesetList.SelectedIndex * 4;
            byte ConfigRegister = (byte)((Stride256Checkbox.Checked ? 8 : 0) + LutList.SelectedIndex);
            MemMgr.VICKY.WriteByte(tilesetBaseAddr + 3, ConfigRegister);
            TilesetViewer.Refresh();
        }

        private void btnMemory_Click(object sender, EventArgs e)
        {
            int address = Convert.ToInt32(TilemapAddress.Text, 16);
            MemoryWindow.Instance.GotoAddress(address & 0xFFFF00);
        }
    }
}
