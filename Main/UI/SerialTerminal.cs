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
            e.Cancel = true; // this cancels the close event.
            this.Hide();
        }
    }
}
