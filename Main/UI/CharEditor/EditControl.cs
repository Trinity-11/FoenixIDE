using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.CharEditor
{
    public partial class EditControl : UserControl
    {
        byte[] reloadData = null;
        byte[] clipData = null;
        byte[] FontData = new byte[8 * 256];
        byte[] characterData = new byte[16];
        bool[,] grid = new bool[8, 8];
        int CharIndex = 0;
        int Columns = 8;
        int Rows = 8;
        Color Color0 = Color.Black;
        Color Color1 = Color.LightGreen;
        MouseButtons MouseHeld = MouseButtons.None;
        bool ColorHeld = false;

        public event EventHandler CharacterSaved;

        Brush textBrush = new SolidBrush(Color.LightGreen);
        Pen borderPen = new Pen(Color.DarkGray);

        public EditControl()
        {
            InitializeComponent();
        }

        internal void LoadCharacter(byte[] FontData, int selectedIndex, int bytesPerCharacter)
        {
            this.FontData = FontData;
            this.CharIndex = selectedIndex;
            Rows = bytesPerCharacter;

            LoadCharacter();
        }

        private void LoadCharacter()
        {
            characterData = new byte[Rows];
            int pos = CharIndex * Rows;
            if (pos < 0 || pos >= FontData.Length)
                return;
            for (int i = 0; i < Rows; i++)
            {
                characterData[i] = FontData[pos + i];
            }
            reloadData = new byte[Rows];
            characterData.CopyTo(reloadData, 0);
            LoadPixels();
            Refresh();
        }

        private void LoadPixels()
        {
            for (int y = 0; y < Rows; y++)
            {
                int row = 0;
                for (int x = 0; x < Columns; x++)
                {
                    int bit = (int)Math.Pow(2, (Columns - x - 1));
                    grid[x, y] = (characterData[y] & bit) == bit;
                }
                characterData[y] = (byte)row;
            }
        }

        private void P_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseHeld == MouseButtons.Left)
            {
                Point p = GetPixel(e.Location);
                if (p.X < 0 || p.X >= Columns || p.Y < 0 || p.Y >= Rows)
                    return;
                bool i = grid[p.X, p.Y];
                if (i != ColorHeld)
                {
                    grid[p.X, p.Y] = ColorHeld;
                    characterPanel.Refresh();
                }
            }
        }

        private Point GetPixel(Point location)
        {
            Rectangle pixel = new Rectangle(
                0,
                0,
                characterPanel.ClientRectangle.Width / Columns,
                characterPanel.ClientRectangle.Height / Rows);

            Point p = new Point
            {
                X = location.X / pixel.Width,
                Y = location.Y / pixel.Height
            };

            return p;
        }

        private void P_MouseUp(object sender, MouseEventArgs e)
        {
            MouseHeld = MouseButtons.None;
            Redraw();
        }

        private void P_MouseDown(object sender, MouseEventArgs e)
        {
            MouseHeld = e.Button;
            if (e.Button == MouseButtons.Left)
            {
                Point p = GetPixel(e.Location);
                if (p.X < 0 || p.X >= Columns || p.Y < 0 || p.Y >= Rows)
                    return;
                ColorHeld = !grid[p.X, p.Y];
                grid[p.X, p.Y] = ColorHeld;
                characterPanel.Refresh();
            }
        }

        private void P_Click(object sender, EventArgs e)
        {
            if (sender is Panel p)
            {
                if (p.BackColor == Color0)
                    p.BackColor = characterPanel.ForeColor;
                else
                    p.BackColor = Color0;
            }
            

            
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveCharacter();
        }

        private void SaveCharacter()
        {
            SavePixels();

            int j = CharIndex * Rows;
            for (int i = 0; i < Rows; i++)
            {
                FontData[j + i] = characterData[i];
            }

            CharacterSaved?.Invoke(this, new EventArgs());
        }

        private void SavePixels()
        {
            for (int y = 0; y < Rows; y++)
            {
                int row = 0;
                for (int x = 0; x < Columns; x++)
                {
                    int bit = (int)Math.Pow(2, (Columns - x - 1));
                    row |= (grid[x, y] ? bit : 0);
                }
                characterData[y] = (byte)row;
            }
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = Columns - 1; x > 0; x--)
                {
                    grid[x, y] = grid[x - 1, y];
                }
            }
            Redraw();
        }

        private void Redraw()
        {
            characterPanel.Refresh();
            SaveCharacter();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            if (reloadData == null)
                return;

            reloadData.CopyTo(characterData, 0);
            LoadPixels();
            Redraw();
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns - 1; x++)
                {
                    grid[x, y] = grid[x + 1, y];
                }
            }
            Redraw();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            SavePixels();
            clipData = new byte[Rows];
            characterData.CopyTo(clipData, 0);
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            if (clipData == null)
                return;

            clipData.CopyTo(characterData, 0);
            LoadPixels();
            Redraw();
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Rows - 1; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    grid[x, y] = grid[x, y + 1];
                }
            }
            for (int x = 0; x < Columns; x++)
            {
                grid[x, Rows - 1] = false;
            }
            Redraw();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            for (int y = Rows - 1; y > 0; y--)
            {
                for (int x = 0; x < Columns; x++)
                {
                    grid[x, y] = grid[x, y - 1];
                }
            }
            for (int x = 0; x < Columns; x++)
            {
                grid[x, 0] = false;
            }
            Redraw();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    grid[x, y] = false;
                }
            }
            Redraw();
        }

        private void CharacterPanel_Paint(object sender, PaintEventArgs e)
        {
            Rectangle pixel = new Rectangle(
                0,
                0,
                characterPanel.ClientRectangle.Width / Columns,
                characterPanel.ClientRectangle.Height / Rows);

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    pixel.Location = new Point(x * pixel.Width, y * pixel.Height);
                    if (grid[x, y])
                        e.Graphics.FillRectangle(textBrush, pixel);
                    e.Graphics.DrawRectangle(borderPen, pixel);
                }
            }

        }
    }
}
