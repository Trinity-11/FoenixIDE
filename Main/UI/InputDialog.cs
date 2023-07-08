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

        public InputDialog(String title, String prompt)
        {
            this.title = title;
            this.prompt = prompt;
            InitializeComponent();
        }

        public String Value
        {
            get { return this.txtAddress.Text; }
            set { this.txtAddress.Text = value; }
        }
    }
    
}
