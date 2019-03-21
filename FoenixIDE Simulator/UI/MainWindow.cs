using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class MainWindow : Form
    {
        public FoenixSystem kernel;
        public UI.CPUWindow DebugWindow;
        public Timer BootTimer = new Timer();
        public int CyclesPerTick = 35000;
        public MemoryWindow MemoryWindow;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void BasicWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            //lastKeyPressed.Text = "$" + ((UInt16)e.KeyChar).ToString("X2");
            kernel.KeyboardBuffer.Write(e.KeyChar, 2);
        }

        private void BasicWindow_Load(object sender, EventArgs e)
        {
            kernel = new FoenixSystem(this.gpu);

            ShowDebugWindow();
            ShowMemoryWindow();

            this.Top = 0;
            this.Left = 0;
            this.Width = DebugWindow.Left;
            this.Height = Convert.ToInt32(this.Width * 0.75);

            BootTimer.Interval = 100;
            BootTimer.Tick += BootTimer_Tick;
            //kernel.READY();
        }

        private void ShowDebugWindow()
        {
            DebugWindow = new UI.CPUWindow();
            DebugWindow.CPU = kernel.CPU;
            kernel.CPU.DebugPause = true;
            DebugWindow.Kernel = kernel;
            DebugWindow.Left = Screen.PrimaryScreen.WorkingArea.Width - DebugWindow.Width;
            DebugWindow.Top = Screen.PrimaryScreen.WorkingArea.Top;
            DebugWindow.Show();
        }

        void ShowMemoryWindow()
        {
            MemoryWindow = new MemoryWindow();
            MemoryWindow.Memory = kernel.CPU.Memory;
            MemoryWindow.Left = DebugWindow.Left;
            MemoryWindow.Top = DebugWindow.Top + DebugWindow.Height;
            MemoryWindow.Show();
        }

        private void BootTimer_Tick(object sender, EventArgs e)
        {
            BootTimer.Enabled = false;
            kernel.Reset();
        }

        private void BasicWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    kernel.KeyboardBuffer.Write(KeyboardMap.KEY_Up);
                    break;
                case Keys.Down:
                    kernel.KeyboardBuffer.Write(KeyboardMap.KEY_Down);
                    break;
                case Keys.Left:
                    kernel.KeyboardBuffer.Write(KeyboardMap.KEY_Left);
                    break;
                case Keys.Right:
                    kernel.KeyboardBuffer.Write(KeyboardMap.KEY_Right);
                    break;
                case Keys.Home:
                    kernel.KeyboardBuffer.Write(KeyboardMap.KEY_Home);
                    break;
                default:
                    global::System.Diagnostics.Debug.WriteLine("KeyDown: " + e.KeyCode.ToString());
                    break;
            }
        }

        private void BasicWindow_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        double cps;
        private void timer1_Tick(object sender, EventArgs e)
        {
            kernel.CPU.ExecuteCycles(CyclesPerTick);

            TimeSpan s = kernel.CPU.CycleTime;
            int c = kernel.CPU.CycleCounter;

            cps = c / s.TotalSeconds;
        }

        private void performanceTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan s = DateTime.Now - kernel.CPU.StartTime;
            int c = kernel.CPU.CycleCounter;
            cps = c / s.TotalSeconds;

            timerStatus.Text = cps.ToString("N0") + " CPS";
        }

        private void gpu_VisibleChanged(object sender, EventArgs e)
        {
            if (gpu.Visible)
            {
                BootTimer.Enabled = true;
            }
        }

        private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDebugWindow();
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMemoryWindow();
        }

        private void resetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DebugWindow.ClearTrace();
            kernel.CPU.DebugPause = false;
            kernel.Reset();
            kernel.Run();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugWindow.ClearTrace();
            kernel.CPU.DebugPause = true;
            kernel.Reset();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ModeText.Text = "Shutting down CPU thread";

            if (kernel.CPU.CPUThread != null)
            {
                kernel.CPU.CPUThread.Abort();
                kernel.CPU.CPUThread.Join(1000);
            }
        }
    }
}

