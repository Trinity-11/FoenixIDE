using FoenixIDE.MemoryLocations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class JoystickForm : Form
    {
        public MemoryRAM beatrix = null;
        public JoystickForm()
        {
            InitializeComponent();
        }

        private void JoystickForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void AllButtonsUp(object sender, MouseEventArgs e)
        {
            if (beatrix != null)
            {
                beatrix.WriteByte(MemoryLocations.MemoryMap.JOYSTICK0 - MemoryLocations.MemoryMap.BEATRIX_START, 0x9F);
            }
        }

        private void AllButtonsDown(object sender, MouseEventArgs e)
        {
            int buttonPressed = int.Parse((string)((Control)sender).Tag);
            byte value = (byte)(0x9F & ~buttonPressed);
            if (beatrix != null)
            {
                beatrix.WriteByte(MemoryLocations.MemoryMap.JOYSTICK0 - MemoryLocations.MemoryMap.BEATRIX_START, value);
            }
        }

        private void JoystickForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // this cancels the close event.
            this.Hide();
        }
    }
}
