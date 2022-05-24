using FoenixIDE.MemoryLocations;
using System.Drawing;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class JoystickForm : Form
    {
        public MemoryRAM gabe = null;
        public JoystickForm()
        {
            InitializeComponent();
        }

        private void SendJoystickValue(int joystick, byte value)
        {
            if (gabe != null)
            {
                gabe.WriteByte(MemoryLocations.MemoryMap.JOYSTICK0 - MemoryLocations.MemoryMap.GABE_START + joystick, value);
            }
        }

        /*
         * We're catching the form close event.  This allows us to reuse the form.
         */
        private void JoystickForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // this cancels the close event.
            this.Hide();
        }
        /*
         * Keyboard Down - the form receives key events
         */
        private void JoystickForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            byte value = 0;
            switch (e.KeyCode)
            {
                case Keys.A:
                    value = 0xDB;
                    LeftButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.S:
                    value = 0xDD;
                    DownButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.D:
                    value = 0xD7;
                    RightButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.W:
                    value = 0xDE;
                    UpButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.Q:
                    value = 0xCF;
                    Fire1Button.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.E:
                    value = 0x5F;
                    Fire2Button.BackColor = SystemColors.ControlDark;
                    break;
            }
            if (value != 0)
            {
                SendJoystickValue(0, value);
            }
        }
        private void JoystickForm_KeyUp(object sender, KeyEventArgs e)
        {
            SendJoystickValue(0, 0xDF);
            switch (e.KeyCode)
            {
                case Keys.A:
                    LeftButton.BackColor = SystemColors.Control;
                    break;
                case Keys.S:
                    DownButton.BackColor = SystemColors.Control;
                    break;
                case Keys.D:
                    RightButton.BackColor = SystemColors.Control;
                    break;
                case Keys.W:
                    UpButton.BackColor = SystemColors.Control;
                    break;
                case Keys.Q:
                    Fire1Button.BackColor = SystemColors.Control;
                    break;
                case Keys.E:
                    Fire2Button.BackColor = SystemColors.Control;
                    break;
            }
        }

        /*
         * All buttons use this event.
         */
        private void AllButtonsUp(object sender, MouseEventArgs e)
        {
            SendJoystickValue(0, 0xDF);
        }
        private void AllButtonsDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                int buttonPressed = int.Parse((string)(ctrl.Tag));
                byte value = (byte)(0xDF & ~buttonPressed);
                SendJoystickValue(0, value);
            }
        }
    }
}
