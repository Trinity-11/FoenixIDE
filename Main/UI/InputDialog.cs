using System;
using System.Drawing;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }
		public InputDialog(String prompt, String title = "", String defaultResponse = "", int XPos = -1, int YPos = -1 )
        {
            InitializeComponent();
            if (title.Length > 0)
            {
                this.Text = title;
            }
            LabelMessage.Text = prompt;

            if ( (XPos >= 0) && (YPos >= 0)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(XPos, YPos);
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            TextValue.Text = defaultResponse;
        }

        public String GetValue()
        {
            return TextValue.Text;
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(LabelMessage.Text, LabelMessage.Font, 320);
                TextValue.Height = (int)(size.Height + .5);
            }
        }
    }
}
