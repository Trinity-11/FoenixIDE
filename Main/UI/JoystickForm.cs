using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using System.Drawing;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class JoystickForm : Form
    {
        private MemoryRAM gabe = null;
        private VIARegisters matrix = null;
        private int portAddress = 0;
        private int port = 0;
        private byte NO_BUTTON = 0xDF;
        public JoystickForm()
        {
            InitializeComponent();
        }

        public void SetGabe(MemoryRAM device, int address, int port)
        {
            gabe = device;
            matrix = null;
            portAddress = address;
            this.port = port;
            NO_BUTTON = 0xDF;
        }

        public void SetMatrix(VIARegisters device, int address, int port)
        {
            gabe = null;
            matrix = device;
            portAddress = address;
            this.port = port;
            NO_BUTTON = 0x3F;
        }

        private void SendJoystickValue(byte value)
        {
            if (gabe != null)
            {
                gabe.WriteByte(portAddress + port, value);
            }
            if (matrix != null)
            {
                matrix.JoystickCode((byte)port, value);
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
                    value = (byte)(NO_BUTTON ^ 4); // 0xDB;
                    LeftButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.S:
                    value = (byte)(NO_BUTTON ^ 2); // 0xDD;
                    DownButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.D:
                    value = (byte)(NO_BUTTON ^ 8); //  0xD7;
                    RightButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.W:
                    value = (byte)(NO_BUTTON ^ 1); // 0xDE;
                    UpButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.Q:
                    value = (byte)(NO_BUTTON ^ 0x10); // 0xCF;
                    Fire1Button.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.E:
                    value = (byte)(NO_BUTTON ^ 0x20); // 0x5F;
                    Fire2Button.BackColor = SystemColors.ControlDark;
                    break;
            }
            if (value != 0)
            {
                SendJoystickValue(value);
            }
        }
        private void JoystickForm_KeyUp(object sender, KeyEventArgs e)
        {
            SendJoystickValue(NO_BUTTON);
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
            SendJoystickValue(NO_BUTTON);
        }
        private void AllButtonsDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                int buttonPressed = int.Parse((string)(ctrl.Tag));
                byte value = (byte)(0xFF & ~buttonPressed);
                SendJoystickValue(value);
            }
        }
    }
}
