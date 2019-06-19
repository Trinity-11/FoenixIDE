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
    public partial class SerialTerminal : Form
    {
        public static SerialTerminal Instance = null;

        public SerialTerminal()
        {
            InitializeComponent();
            Instance = this;
        }

        private void SerialTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
