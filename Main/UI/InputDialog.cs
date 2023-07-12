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
    public partial class InputDialog : Form
    {
        private String title = "Enter Address";
        private String prompt = "Please enter the new start address in Hexadecimal:";

        public InputDialog()
        {
            InitializeComponent();
        }

        public InputDialog(String prompt, String title = "", String defaultResponse = "", int XPos = -1, int YPos = -1 )
        {
            this.prompt = prompt;
            this.title = title;

            if ( (XPos >= 0) && (YPos >= 0)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(XPos, YPos);
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            InitializeComponent();
            this.txtAddress.Text = defaultResponse;

        }

        public String Value
        {
            get { return this.txtAddress.Text; }
            set { this.txtAddress.Text = value; }
        }
    }
    
}
