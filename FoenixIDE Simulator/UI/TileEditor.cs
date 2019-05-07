using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private Pen yellowPen = new Pen(Color.Yellow);
        private Pen redPen = new Pen(Color.Red);
        private Brush whiteBrush = new SolidBrush(Color.White);

        public TileEditor()
        {
            InitializeComponent();
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

        /**
         * Draw the tileset with clear lines separating the images 16x16.
         */
        private void TilesetViewer_Paint(object sender, PaintEventArgs e)
        {
            if (hoverX > -1 && hoverY > -1)
            {
                e.Graphics.DrawRectangle(yellowPen, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH, TILE_WIDTH + 1, TILE_WIDTH + 1);
                e.Graphics.DrawString("$" + (hoverY * 16 + hoverX).ToString("X2"), SystemFonts.DefaultFont, whiteBrush, hoverX * TILE_WIDTH, hoverY * TILE_WIDTH);
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
        }
    }
}
