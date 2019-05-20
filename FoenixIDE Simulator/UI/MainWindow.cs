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
        private TileEditor tileEditor;

        private byte previousGraphicMode;
        private delegate void TileClickEvent(Point tile);
        public delegate void TileLoadedEvent(int layer);
        private TileClickEvent TileClicked;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BasicWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            lastKeyPressed.Text = "$" + ((UInt16)e.KeyChar).ToString("X2");
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
                kernel.CPU.DebugPause = true;
                debugWindow = new UI.CPUWindow
                {
                    Top = Screen.PrimaryScreen.WorkingArea.Top,
                };
                debugWindow.Left = Screen.PrimaryScreen.WorkingArea.Width - debugWindow.Width;
                debugWindow.SetKernel(kernel);
                debugWindow.Show();
            } 
            else
            {
                debugWindow.BringToFront();
            }
        }

        private void ShowMemoryWindow()
        {
            if (memoryWindow == null || memoryWindow.IsDisposed)
            {
                memoryWindow = new MemoryWindow
                {
                    Memory = kernel.CPU.Memory,
                    Left = debugWindow.Left,
                    Top = debugWindow.Top + debugWindow.Height
                };
                memoryWindow.Show();
            }
            else
            {
                memoryWindow.BringToFront();
            }
            memoryWindow.UpdateMCRButtons();
        }

        void ShowUploaderWindow()
        {
            if (uploaderWindow == null || uploaderWindow.IsDisposed)
            {
                uploaderWindow = new UploaderWindow();
                int left = this.Left + (this.Width - uploaderWindow.Width) / 2;
                int top =  this.Top + (this.Height - uploaderWindow.Height) / 2;
                uploaderWindow.Location = new Point(left, top);
                uploaderWindow.Memory = kernel.CPU.Memory;
                uploaderWindow.Show();
            }
            else
            {
                uploaderWindow.BringToFront();
            }
        }

        private void NewTileLoaded(int layer)
        {
            tileEditor.SelectLayer(layer);
        }
        /*
         * Loading image into memory requires the user to specify what kind of image (tile, bitmap, sprite).
         * What address location in video RAM.
         */
        private void LoadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BitmapLoader loader = new BitmapLoader
            {
                StartPosition = FormStartPosition.CenterParent,
                Memory = kernel.CPU.Memory
            };
            loader.OnTileLoaded += NewTileLoaded;
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugWindow.Close();
            memoryWindow.Close();
            this.Close();
        }

        int previousCounter = 0;
        int previousFrame = 0;
        DateTime previousTime = DateTime.Now;
        private void PerformanceTimer_Tick(object sender, EventArgs e)
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

        private void GPU_VisibleChanged(object sender, EventArgs e)
        {
            BootTimer.Enabled = gpu.Visible;
        }

        private void CPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDebugWindow();
        }

        private void MemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMemoryWindow();
        }

        private void UploaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUploaderWindow();
        }

        /**
         * Restart the CPU
         */
        private void RestartMenuItemClick(object sender, EventArgs e)
        {
            debugWindow.PauseButton_Click(null, null);
            debugWindow.ClearTrace();
            previousCounter = 0;
            kernel.Reset();
            memoryWindow.UpdateMCRButtons();
            kernel.Run();
            debugWindow.RunButton_Click(null, null);
        }
        
        /** 
         * Reset the system and go to step mode.
         */
        private void DebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kernel.CPU.DebugPause = true;
            debugWindow.ClearTrace();
            previousCounter = 0;
            kernel.Reset();
            memoryWindow.UpdateMCRButtons();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ModeText.Text = "Shutting down CPU thread";
            kernel.CPU.DebugPause = true;
            if (kernel.CPU.CPUThread != null)
            {
                kernel.CPU.CPUThread.Abort();
                kernel.CPU.CPUThread.Join(1000);
            }
        }

        private void LoadHexFile(bool ResetMemory)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Hex Filed|*.hex",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                debugWindow.Close();
                memoryWindow.Close();
                if (ResetMemory)
                {
                    kernel = new FoenixSystem(this.gpu);
                }
                kernel.SetKernel(dialog.FileName);
                kernel.Reset();
                ShowDebugWindow();
                ShowMemoryWindow();
                if (tileEditor != null && tileEditor.Visible)
                {
                    tileEditor.SetMemory(kernel.Memory);
                }
            }
        }
        private void MenuOpenHexFile_Click(object sender, EventArgs e)
        {
            LoadHexFile(true);
        }

        private void OpenHexFileWoZeroingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadHexFile(false);
        }

        /*
         * Read a Foenix XML file
         */
        private void LoadFNXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Foenix XML File|*.fnxml",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                debugWindow.Close();
                memoryWindow.Close();
                kernel = new FoenixSystem(this.gpu);
                kernel.SetKernel(dialog.FileName);
                kernel.Reset();
                ShowDebugWindow();
                ShowMemoryWindow();
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutFrom about = new AboutFrom();
            about.ShowDialog();
        }

        // When the editor window is closed, exit the TileEditorMode
        private void EditorWindowClosed(object sender, FormClosedEventArgs e)
        {
            gpu.TileEditorMode = false;
            // Restore the previous graphics mode
            kernel.Memory.IO.WriteByte(0, previousGraphicMode);
            tileEditor.Dispose();
            tileEditor = null;
        }
        
        private void TileEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileEditor == null)
            {
                tileEditor = new TileEditor();
                tileEditor.SetMemory(kernel.Memory);
                gpu.TileEditorMode = true;
                gpu.ColumnsVisible = 72;
                gpu.LinesVisible = 52;
                // Set Vicky into Tile mode
                previousGraphicMode = kernel.Memory.IO.ReadByte(0);
                kernel.Memory.IO.WriteByte(0, 0x10);
                // Enable borders
                kernel.Memory.IO.WriteByte(4, 1);
                tileEditor.Show();
                tileEditor.FormClosed += new FormClosedEventHandler(EditorWindowClosed);

                // coordinate between the tile editor window and the GPU canvas
                this.TileClicked += new TileClickEvent(tileEditor.TileClicked_Click);
            }
            else
            {
                tileEditor.BringToFront();
            }
    }

        private void Gpu_MouseMove(object sender, MouseEventArgs e)
        {
            double ratioW = gpu.Width / 640d;
            double ratioH = gpu.Height / 480d;
            if (gpu.TileEditorMode)
            {
                if ((e.X / ratioW > 32 && e.X / ratioW < 608) && (e.Y / ratioH > 32 && e.Y / ratioH < 448))
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.No;
                }
            }
            else
            {
                // Read the mouse pointer register
                byte mouseReg = kernel.Memory.IO.ReadByte(0x700);
                if ((mouseReg & 1) == 1)
                {
                    int X = (int)(e.X / ratioW);
                    int Y = (int)(e.Y / ratioH);
                    kernel.Memory.IO.WriteWord(0x702, X);
                    kernel.Memory.IO.WriteWord(0x704, Y);
                    
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void Gpu_MouseLeave(object sender, EventArgs e)
        {
            if (gpu.MousePointerMode || gpu.TileEditorMode)
            {
                Cursor.Show();
            }
            this.Cursor = Cursors.Default;
        }

        private void Gpu_MouseClick(object sender, MouseEventArgs e)
        {
            if (gpu.TileEditorMode && gpu.Cursor != Cursors.No)
            {
                double ratioW = gpu.Width / 640d;
                double ratioH = gpu.Height / 480d;
                TileClicked?.Invoke(new Point((int)(e.X / ratioW / 16), (int)(e.Y / ratioH / 16)));
            }
        }


        private void Gpu_MouseEnter(object sender, EventArgs e)
        {
            if (gpu.MousePointerMode && !gpu.TileEditorMode)
            {
                Cursor.Hide();
            }
        }

        private void ToggleGammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte gammaSetting = kernel.Memory.IO.ReadByte(0);
            gammaSetting ^= 0x40;
            kernel.Memory.IO.WriteByte(0,gammaSetting);
        }
    }
}

