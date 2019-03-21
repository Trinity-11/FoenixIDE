using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nu256.Editor
{
    public partial class EditBox : UserControl
    {
        List<string> Lines = new List<string>();
        List<char> CurrentLine = null;

        Point CursorPos = new Point(0, 0);
        Point SelStart = new Point(0, 0);
        Point SelEnd = new Point(0, 0);

        Brush LineNumberBackgroundBrush = SystemBrushes.Control;
        Brush LineNumberTextBrush = SystemBrushes.WindowText;
        Pen LineNumberBorderPen = new Pen(SystemColors.WindowText);
        Brush TextBrush = SystemBrushes.WindowText;
        Font LineNumberFont = new Font("Consolas", 10);

        public EditBox()
        {
            InitializeComponent();
        }

        public int TopLine { get; private set; }

        private void EditBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SizeF lnSize = g.MeasureString("00000 ", LineNumberFont);

            g.Clear(this.BackColor);
            g.FillRectangle(LineNumberBackgroundBrush, 0, 0, lnSize.Width, ClientRectangle.Height);
            g.DrawLine(LineNumberBorderPen, lnSize.Width, 0, lnSize.Width, this.ClientRectangle.Height);

            float y = 0, x = 0;
            int row = 0;
            int lineNumber = TopLine;

            for (y = 0; y < this.ClientRectangle.Height; y++)
            {
                g.DrawString(lineNumber.ToString().PadLeft(5), LineNumberFont, LineNumberTextBrush, x, y, StringFormat.GenericTypographic);
                y += lnSize.Height;
                lineNumber++;
                row++;
            }


        }
    }
}
