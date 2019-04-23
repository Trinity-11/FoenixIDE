using FoenixIDE.Simulator.UI;
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
        public Timer BootTimer = new Timer();
        public int CyclesPerTick = 35000;

        public UI.CPUWindow debugWindow;
        public MemoryWindow memoryWindow;
        public UploaderWindow uploaderWindow;


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
            this.Width = debugWindow.Left;
            if (this.Width > 1200)
            {
                this.Width = 1200;
            }
            this.Height = Convert.ToInt32(this.Width * 0.75);

            BootTimer.Interval = 100;
            BootTimer.Tick += BootTimer_Tick;
            //kernel.READY();
        }

        private void ShowDebugWindow()
        {
            if (debugWindow == null || debugWindow.IsDisposed)
            {
                debugWindow = new UI.CPUWindow();
                debugWindow.CPU = kernel.CPU;
                kernel.CPU.DebugPause = true;
                debugWindow.Kernel = kernel;
                debugWindow.Left = Screen.PrimaryScreen.WorkingArea.Width - debugWindow.Width;
                debugWindow.Top = Screen.PrimaryScreen.WorkingArea.Top;
                debugWindow.Show();
            } 
            else
            {
                debugWindow.BringToFront();
            }
        }

        void ShowMemoryWindow()
        {
            if (memoryWindow == null || memoryWindow.IsDisposed)
            {
                memoryWindow = new MemoryWindow();
                memoryWindow.Memory = kernel.CPU.Memory;
                memoryWindow.Left = debugWindow.Left;
                memoryWindow.Top = debugWindow.Top + debugWindow.Height;
                memoryWindow.Show();
                memoryWindow.UpdateMCRButtons();
            }
            else
            {
                memoryWindow.BringToFront();
            }
        }

        void ShowUploaderWindow()
        {
            if (uploaderWindow == null || uploaderWindow.IsDisposed)
            {
                uploaderWindow = new UploaderWindow();
                int left = this.Left + (this.Width - uploaderWindow.Width) / 2;
                int top =  this.Top + (this.Height - uploaderWindow.Height) / 2;
                uploaderWindow.Location = new Point(left, top);
                uploaderWindow.Show();
            }
            else
            {
                uploaderWindow.BringToFront();
            }
        }

        /*
         * Loading image into memory requires the user to specify what kind of image (tile, bitmap, sprite).
         * What address location in video RAM.
         */
        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BitmapLoader loader = new BitmapLoader();
            loader.StartPosition = FormStartPosition.CenterParent;
            loader.Memory = kernel.CPU.Memory;
            loader.ShowDialog(this);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            kernel.CPU.ExecuteCycles(CyclesPerTick);
        }

        int previousCounter = 0;
        int previousFrame = 0;
        DateTime previousTime = DateTime.Now;
        private void performanceTimer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan s = currentTime - previousTime;
            int currentCounter = kernel.CPU.CycleCounter;
            int currentFrame = gpu.paintCycle;
            double cps = (currentCounter - previousCounter) / s.TotalSeconds;
            double fps = (currentFrame - previousFrame) / s.TotalSeconds;

            previousCounter = currentCounter;
            previousTime = currentTime;
            previousFrame = currentFrame;
            cpsPerf.Text = "CPS: " + cps.ToString("N0");
            fpsPerf.Text = "FPS: " + fps.ToString("N0");

        }

        private void gpu_VisibleChanged(object sender, EventArgs e)
        {
            BootTimer.Enabled = gpu.Visible;
        }

        private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDebugWindow();
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMemoryWindow();
        }

        private void uploaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUploaderWindow();
        }

        /**
         * Restart the CPU
         */
        private void restartMenuItemClick(object sender, EventArgs e)
        {
            debugWindow.PauseButton_Click(null, null);
            debugWindow.ClearTrace();
            kernel.Reset();
            memoryWindow.UpdateMCRButtons();
            kernel.Run();
            debugWindow.RunButton_Click(null, null);
        }
        
        /** 
         * Reset the system and go to step mode.
         */
        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kernel.CPU.DebugPause = true;
            debugWindow.ClearTrace();
            kernel.Reset();
            memoryWindow.UpdateMCRButtons();
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

        private void loadHexFile(bool ResetMemory)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Hex Filed|*.hex";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                debugWindow.Close();
                memoryWindow.Close();
                if (ResetMemory)
                {
                    kernel = new FoenixSystem(this.gpu);
                }
                kernel.setKernel(dialog.FileName);
                kernel.Reset();
                ShowDebugWindow();
                ShowMemoryWindow();
            }
        }
        private void menuOpenHexFile_Click(object sender, EventArgs e)
        {
            loadHexFile(true);
        }

        private void openHexFileWoZeroingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadHexFile(false);
        }

        /*
         * Read a Foenix XML file
         */
        private void loadFNXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Foenix XML File|*.fnxml";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                debugWindow.Close();
                memoryWindow.Close();
                kernel = new FoenixSystem(this.gpu);
                kernel.setKernel(dialog.FileName);
                kernel.Reset();
                ShowDebugWindow();
                ShowMemoryWindow();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutFrom about = new AboutFrom();
            about.ShowDialog();
        }
    }
}

