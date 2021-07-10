using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.Controls
{
    class ColorCheckBox: CheckBox
    {
        private bool active;
        public bool IsActive
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                Invalidate();
            }
        }

        public ColorCheckBox()
        {
            Appearance = System.Windows.Forms.Appearance.Normal;
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            TextAlign = ContentAlignment.MiddleRight;
            FlatAppearance.BorderSize = 0;
            AutoSize = false;
            Height = 16;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(BackColor);
            Size box = new Size(Size.Width - 2, Size.Height - 2);
            SolidBrush foreBrush = new SolidBrush(ForeColor);
            if (Text.Length > 0)
            {
                
                pevent.Graphics.DrawString(Text, Font, foreBrush, 27, 0);
            }
                
            Rectangle rect = new Rectangle(new Point(0,0), box);

            pevent.Graphics.FillRectangle(active ? Brushes.Crimson : Brushes.White, rect);

            if (Checked)
            {
                using (Font wing = new Font("Wingdings", 10f))
                    pevent.Graphics.DrawString("ü", wing, foreBrush, -1, 0);
            }
            pevent.Graphics.DrawRectangle(Pens.DarkSlateBlue, rect);

            Rectangle fRect = ClientRectangle;

            //if (Focused)
            //{
            //    fRect.Inflate(-1, -1);
            //    using (Pen pen = new Pen(Brushes.Gray) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
            //        pevent.Graphics.DrawRectangle(pen, fRect);
            //}
        }
    }

    
}
