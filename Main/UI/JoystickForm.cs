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
        private byte memory = 0xDF;
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
            Text = port == 0 ? "Joystick A" : "Joystick B";
            NO_BUTTON = 0xDF;
        }

        public void SetMatrix(VIARegisters device, int address, int port)
        {
            gabe = null;
            matrix = device;
            portAddress = address;
            this.port = port;
            Text = port == 0 ? "Joystick A" : "Joystick B";
            NO_BUTTON = 0x3F;
        }

        private void SendJoystickValue()
        {
            if (gabe != null)
            {
                gabe.WriteByte(portAddress + port, memory);
            }
            if (matrix != null)
            {
                matrix.JoystickCode((byte)port, memory);
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
            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.Left:
                    memory = (byte)(memory & (NO_BUTTON ^ 4)); // 0xDB;
                    LeftButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.S:
                case Keys.Down:
                    memory = (byte)(memory & (NO_BUTTON ^ 2)); // 0xDD;
                    DownButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.D:
                case Keys.Right:
                    memory = (byte)(memory & (NO_BUTTON ^ 8)); //  0xD7;
                    RightButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.W:
                case Keys.Up:
                    memory = (byte)(memory & (NO_BUTTON ^ 1)); // 0xDE;
                    UpButton.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.Q:
                    memory = (byte)(memory & (NO_BUTTON ^ 0x10)); // 0xCF;
                    Fire1Button.BackColor = SystemColors.ControlDark;
                    break;
                case Keys.E:
                    memory = (byte)(memory & (NO_BUTTON ^ 0x20)); // 0x5F;
                    Fire2Button.BackColor = SystemColors.ControlDark;
                    break;
            }
            SendJoystickValue();
        }
        private void JoystickForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.Left:
                    memory |= 4;
                    LeftButton.BackColor = SystemColors.Control;
                    break;
                case Keys.S:
                case Keys.Down:
                    memory |= 2;
                    DownButton.BackColor = SystemColors.Control;
                    break;
                case Keys.D:
                case Keys.Right:
                    memory |= 8;
                    RightButton.BackColor = SystemColors.Control;
                    break;
                case Keys.W:
                case Keys.Up:
                    memory |= 1;
                    UpButton.BackColor = SystemColors.Control;
                    break;
                case Keys.Q:
                    memory |= 0x10;
                    Fire1Button.BackColor = SystemColors.Control;
                    break;
                case Keys.E:
                    memory |= 0x20;
                    Fire2Button.BackColor = SystemColors.Control;
                    break;
            }
            SendJoystickValue();
        }

        /*
         * All buttons use this event.
         */
        private void AllButtonsUp(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                int buttonPressed = int.Parse((string)(ctrl.Tag));
                memory = (byte)(memory | buttonPressed);
                SendJoystickValue();
            }
        }
        private void AllButtonsDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                int buttonPressed = int.Parse((string)(ctrl.Tag));
                memory = (byte)(memory & ~buttonPressed);
                SendJoystickValue();
            }
        }
    }
}
