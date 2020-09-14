using FoenixIDE.Basic;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.Devices.SDCard;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.Simulator.UI;
using FoenixIDE.CharEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoenixIDE.Timers;
using System.IO;

namespace FoenixIDE.UI
{
    public partial class MainWindow : Form
    {
        public FoenixSystem kernel;

        //  Windows
        public UI.CPUWindow debugWindow;
        public MemoryWindow memoryWindow;
        public UploaderWindow uploaderWindow;
        private WatchForm watchWindow = new WatchForm();
        private SDCardWindow sdCardWindow = new SDCardWindow();
        private TileEditor tileEditor;
        private CharEditorWindow charEditor;
        public SerialTerminal terminal;
        private JoystickForm joystickWindow = new JoystickForm();

        // Local variables and events
        private byte previousGraphicMode;
        private delegate void TileClickEvent(Point tile);
        public delegate void TileLoadedEvent(int layer);
        private TileClickEvent TileClicked;
        private ResourceChecker ResChecker = new ResourceChecker();
        private delegate void TransmitByteFunction(byte Value);
        private delegate void ShowFormFunction();
        private String defaultKernel = @"roms\kernel.hex";
        private int jumpStartAddress;
        private bool disabledIRQs = false;
        private bool autoRun = false;
        private BoardVersion version = BoardVersion.RevC;
        public static MainWindow Instance = null;
        private delegate void WriteCPSFPSFunction(string CPS, string FPS);

        public MainWindow(string[] programArgs)
        {
            if (programArgs.Length >0)
            {
                DecodeProgramArguments(programArgs);
            }
            InitializeComponent();
            Instance = this;
        }

        private void DecodeProgramArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch(args[i].Trim())
                {
                    // the hex file to load is specified
                    case "-h":
                    case "--hex":
                        // a kernel file must be specified
                        if (args[i+1].Trim().StartsWith("-") || !args[i + 1].Trim().EndsWith("hex"))
                        {
                            throw new Exception("You must specify a hex file.");
                        }
                        defaultKernel = args[i+1];
                        i++; // skip the next argument
                        break;
                    case "-j":
                    case "--jump":
                        // An address must be specified
                        int value = Convert.ToInt32(args[i+1].Replace("$:", ""), 16);
                        if (value != 0)
                        {
                            jumpStartAddress = value;
                            i++; // skip the next argument
                        } else
                        {
                            throw new Exception("Invalid address specified: " + args[i + 1]);
                        }
                        break;
                    // Autorun - a value is not expected for this one
                    case "-r":
                    case "--run":
                        autoRun = true;
                        break;
                    // Disable IRQs - a value is not expected for this one
                    case "-i":
                    case "--irq":
                        disabledIRQs = true;
                        break;
                    // Board Version B or C
                    case "-b":
                    case "--board":
                        string verArg = args[i + 1];
                        switch (verArg.ToLower())
                        {
                            case "b":
                                version = BoardVersion.RevB;
                                break;
                            case "c":
                                version = BoardVersion.RevC;
                                break;
                        }
                        break;
                    default:
                        throw new Exception("Unknown switch used:" + args[i].Trim());
                }
            }
        }

        private void BasicWindow_Load(object sender, EventArgs e)
        {
            kernel = new FoenixSystem(this.gpu, version, defaultKernel);
            terminal = new SerialTerminal();
            ShowDebugWindow();
            ShowMemoryWindow();
            gpu.StartOfFrame += SOF;
            gpu.StartOfLine += SOL;
            gpu.GpuUpdated += Gpu_Update_Cps_Fps;

            joystickWindow.beatrix = kernel.MemMgr.BEATRIX;

            if (disabledIRQs)
            {
                debugWindow.DisableIRQs(true);
            }

            if (autoRun)
            {   
                debugWindow.RunButton_Click(null, null);
            }

            this.Top = 0;
            this.Left = 0;
            //this.Width = debugWindow.Left;
            if (this.Width > 1200)
            {
                this.Width = 1200;
            }
            this.Height = Convert.ToInt32(this.Width * 0.75);

            SetDipSwitchMemory();
            // Code is tightly coupled with memory manager
            kernel.MemMgr.UART1.TransmitByte += SerialTransmitByte;
            kernel.MemMgr.UART2.TransmitByte += SerialTransmitByte;
            kernel.MemMgr.SDCARD.sdCardIRQMethod += SDCardInterrupt;

            int left = this.Left + (this.Width - watchWindow.Width) / 2;
            int top = this.Top + (this.Height - watchWindow.Height) / 2;
            watchWindow.Location = new Point(left, top);
            watchWindow.SetKernel(kernel);

            DisplayBoardVersion();
            EnableMenuItems();
            ResetSDCard();
        }

        private void LoadHexFile(string Filename, bool ResetMemory)
        {
            debugWindow.Pause();
            kernel.SetVersion(version);
            if (kernel.ResetCPU(ResetMemory, Filename))
            {
                if (kernel.lstFile != null)
                {
                    ShowDebugWindow();
                    ShowMemoryWindow();
                }
                ResetSDCard();
                debugWindow.ClearTrace();
            }
        }

        private void ShowDebugWindow()
        {
            cPUToolStripMenuItem.Enabled = true;
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
                debugWindow.SetKernel(kernel);
                debugWindow.BringToFront();
            }
        }

        private void ShowMemoryWindow()
        {
            memoryToolStripMenuItem.Enabled = true;
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
            memoryWindow.SetGamma += UpdateGamma;
            memoryWindow.SetHiRes += UpdateHiRes;
        }

        public void SerialTransmitByte(byte Value)
        {
            if (terminal.textBox1.InvokeRequired)
            {
                Invoke(new TransmitByteFunction(SerialTransmitByte), Value);
            }
            else
            {
                terminal.textBox1.Text += Convert.ToChar(Value);
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
                uploaderWindow.kernel = kernel;
                uploaderWindow.SetBoardVersion(version);
                uploaderWindow.Show();
            }
            else
            {
                uploaderWindow.BringToFront();
            }
        }

        public void WatchListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!watchWindow.Visible)
            {
                watchWindow.Show();
            }
            else
            {
                watchWindow.BringToFront();
            }
        }

        private void NewTileLoaded(int layer)
        {
            tileEditor?.SelectLayer(layer);
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
                Memory = kernel.CPU.Memory,
                ResChecker = ResChecker
            };
            loader.OnTileLoaded += NewTileLoaded;
            loader.ShowDialog(this);
        }

        DateTime pSof;
        public void SOF()
        {
            // Check if the interrupt is enabled
            DateTime currentDT = DateTime.Now;
            TimeSpan ts = currentDT - pSof;
            //System.Console.WriteLine(ts.TotalMilliseconds);
            pSof = currentDT;
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
            if (!kernel.CPU.DebugPause && !kernel.CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT00_SOF) == (byte)Register0.FNX0_INT00_SOF))
            {
                // Set the SOF Interrupt
                byte IRQ0 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                IRQ0 |= (byte)Register0.FNX0_INT00_SOF;
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.INT_PENDING_REG0, IRQ0);
                kernel.CPU.Pins.IRQ = true;
            }
        }

        public void SOL()
        {
            // Check if the interrupt is enabled
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
            if (!kernel.CPU.DebugPause && !kernel.CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT01_SOL) == (byte)Register0.FNX0_INT01_SOL))
            {
                // Set the SOL Interrupt
                byte IRQ0 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                IRQ0 |= (byte)Register0.FNX0_INT01_SOL;
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.INT_PENDING_REG0, IRQ0);
                kernel.CPU.Pins.IRQ = true;
            }
        }

        public void SDCardInterrupt(CH376SInterrupt irq)
        {
            // Check if the Keyboard interrupt is allowed
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG1);
            if (!kernel.CPU.DebugPause && (~mask & (byte)Register1.FNX1_INT07_SDCARD) == (byte)Register1.FNX1_INT07_SDCARD)
            {
                // Set the SD Card Interrupt
                byte IRQ1 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG1);
                IRQ1 |= (byte)Register1.FNX1_INT07_SDCARD;
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.INT_PENDING_REG1, IRQ1);
                kernel.CPU.Pins.IRQ = true;

                // Write the interrupt result
                kernel.MemMgr.SDCARD.WriteByte(0, (byte)irq);
            }
        }
        private void BasicWindow_KeyDown(object sender, KeyEventArgs e)
        {
            ScanCode scanCode = ScanCodes.GetScanCode(e.KeyCode);
            if (scanCode != ScanCode.sc_null)
            {
                e.Handled = true;
                lastKeyPressed.Text = "$" + ((byte)scanCode).ToString("X2");
                if (kernel.MemMgr != null && !kernel.CPU.DebugPause)
                {
                    kernel.MemMgr.KEYBOARD.WriteKey(kernel, scanCode);
                }
            }
            else
            {
                lastKeyPressed.Text = "";
            }
        }

        private void BasicWindow_KeyUp(object sender, KeyEventArgs e)
        {
            ScanCode scanCode = ScanCodes.GetScanCode(e.KeyCode);
            if (scanCode != ScanCode.sc_null)
            {
                scanCode += 0x80;
                lastKeyPressed.Text = "$" + ((byte)scanCode).ToString("X2");
                if (kernel.MemMgr != null && !kernel.CPU.DebugPause)
                {
                    kernel.MemMgr.KEYBOARD.WriteKey(kernel, scanCode);
                }
            }
            else
            {
                lastKeyPressed.Text = "";
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gpu.StartOfFrame = null;
            gpu.StartOfLine = null;
            if (debugWindow != null)
            {
                debugWindow.Close();
            }
            if (memoryWindow != null)
            {
                memoryWindow.Close();
            }
            this.Close();
        }

        private void Write_CPS_FPS_Safe(string CPS, string FPS)
        {
            if (statusStrip1.InvokeRequired)
            {
                var d = new WriteCPSFPSFunction(Write_CPS_FPS_Safe);
                statusStrip1.Invoke(d, new object[] { CPS, FPS});
            }
            else
            {
                cpsPerf.Text = CPS;
                fpsPerf.Text = FPS;
            }
        }
        int previousCounter = 0;
        int previousFrame = 0;
        DateTime previousTime = DateTime.Now;
        private void Gpu_Update_Cps_Fps()
        {
            if (kernel != null  && kernel.CPU != null)
            {
                DateTime currentTime = DateTime.Now;

                if (!kernel.CPU.DebugPause)
                {
                    
                    TimeSpan s = currentTime - previousTime;
                    int currentCounter = kernel.CPU.CycleCounter;
                    int currentFrame = gpu.paintCycle;
                    double cps = (currentCounter - previousCounter) / s.TotalSeconds;
                    double fps = (currentFrame - previousFrame) / s.TotalSeconds;

                    previousCounter = currentCounter;
                    previousTime = currentTime;
                    previousFrame = currentFrame;
                    Write_CPS_FPS_Safe("CPS: " + cps.ToString("N0"), "FPS: " + fps.ToString("N0"));
                }
                // write the time to memory - values are BCD
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Second));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_MIN - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Minute));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_HRS - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Hour));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_DAY - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Day));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_MONTH - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Month));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_YEAR - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Year % 100));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_CENTURY - kernel.MemMgr.VICKY.StartAddress, BCD(currentTime.Year / 100));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_DOW - kernel.MemMgr.VICKY.StartAddress, (byte)(currentTime.DayOfWeek + 1));
            }
            else
            {
                cpsPerf.Text = "CPS: 0";
                fpsPerf.Text = "FPS: 0";
            }
        }

        private byte BCD(int val)
        {
            return (byte)(val / 10 * 0x10 + val % 10);
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
        public void RestartMenuItemClick(object sender, EventArgs e)
        {           
            previousCounter = 0;
            debugWindow.Pause();
            if (kernel.ResetCPU(false, null))
            {
                debugWindow.SetKernel(kernel);
                debugWindow.ClearTrace();
                SetDipSwitchMemory();
                memoryWindow.Memory = kernel.CPU.Memory;
                memoryWindow.UpdateMCRButtons();

                // Restart the CPU
                debugWindow.RunButton_Click(null, null);
            }
        }
        
        /** 
         * Reset the system and go to step mode.
         */
        private void DebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            previousCounter = 0;
            debugWindow.Pause();
            if (kernel.ResetCPU(false, null))
            {
                debugWindow.SetKernel(kernel);
                debugWindow.ClearTrace();
                SetDipSwitchMemory();
                memoryWindow.Memory = kernel.CPU.Memory;
                memoryWindow.UpdateMCRButtons();
                debugWindow.Refresh();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            gpu.StartOfFrame = null;
            gpu.StartOfLine = null;
            ModeText.Text = "Shutting down CPU thread";
            if (kernel.CPU != null)
            {
                kernel.CPU.DebugPause = true;
                if (kernel.CPU.CPUThread != null)
                {
                    kernel.CPU.CPUThread.Abort();
                    kernel.CPU.CPUThread.Join(1000);
                }
            }
        }

        private void MenuOpenHexFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Hex Filed|*.hex",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadHexFile(dialog.FileName, sender.Equals(menuOpenHexFile));
            }
            
        }

        /*
         * Read a Foenix XML file
         */
        private void LoadFNXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Load Project File",
                Filter = "Foenix Project File|*.fnxml",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // TODO - this code is so coupled - we need to set the version in the XML file too.
                kernel.Resources = ResChecker;
                if (kernel.ResetCPU(true, dialog.FileName))
                {
                    SetDipSwitchMemory();
                    ShowDebugWindow();
                    ShowMemoryWindow();
                    EnableMenuItems();
                }
            }
        }

        /*
         * Export all memory content to an XML file.
         */
        private void SaveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pick the file to create
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Save Project File",
                CheckPathExists = true,
                Filter = "Foenix Project File| *.fnxml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FoeniXmlFile fnxml = new FoeniXmlFile(kernel, ResChecker);
                fnxml.Write(dialog.FileName, true);
            }
        }

        private void saveWatchListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pick the file to create
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Save Watch List File",
                CheckPathExists = true,
                Filter = "Foenix Watch List File| *.wlxml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FoeniXmlFile xmlFile = new FoeniXmlFile(kernel, null);
                xmlFile.WriteWatches(dialog.FileName);
            }
        }

        private void loadWatchListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Load Watch List File",
                Filter = "Foenix Watch List File|*.wlxml",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FoeniXmlFile xmlFile = new FoeniXmlFile(kernel, null);
                xmlFile.ReadWatches(dialog.FileName);
                watchWindow.SetKernel(kernel);
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
            kernel.MemMgr.VICKY.WriteByte(0, previousGraphicMode);
            tileEditor.Dispose();
            tileEditor = null;
        }
        
        private void TileEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileEditor == null)
            {
                tileEditor = new TileEditor();
                tileEditor.SetMemory(kernel.MemMgr);
                gpu.TileEditorMode = true;
                // Set Vicky into Tile mode
                previousGraphicMode = kernel.MemMgr.VICKY.ReadByte(0);
                kernel.MemMgr.VICKY.WriteByte(0, 0x10);
                // Enable borders
                kernel.MemMgr.VICKY.WriteByte(4, 1);
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


        private void CharacterEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (charEditor == null)
            {
                charEditor = new CharEditorWindow();
            }
            charEditor.Show();
        }

        private void Gpu_MouseMove(object sender, MouseEventArgs e)
        {
            Point size = gpu.GetScreenSize();
            double ratioW = gpu.Width / (double)size.X;
            double ratioH = gpu.Height / (double)size.Y;
            if (gpu.TileEditorMode)
            {
                if ((e.X / ratioW > 32 && e.X / ratioW < size.X -32) && (e.Y / ratioH > 32 && e.Y / ratioH < size.Y -32))
                {
                    this.Cursor = Cursors.Hand;
                    if (e.Button == MouseButtons.Left)
                    {
                        TileClicked?.Invoke(new Point((int)(e.X / ratioW / 16), (int)(e.Y / ratioH / 16)));
                    }
                }
                else
                {
                    this.Cursor = Cursors.No;
                }
            }
            else if (kernel.MemMgr != null)
            {
                // Read the mouse pointer register
                byte mouseReg = kernel.MemMgr.VICKY.ReadByte(0x700);
                if ((mouseReg & 1) == 1)
                {
                    int X = (int)(e.X / ratioW);
                    int Y = (int)(e.Y / ratioH);
                    kernel.MemMgr.VICKY.WriteWord(0x702, X);
                    kernel.MemMgr.VICKY.WriteWord(0x704, Y);
                    
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

        private void gpu_MouseDown(object sender, MouseEventArgs e)
        {
            if (gpu.TileEditorMode && gpu.Cursor != Cursors.No)
            {
                Point size = gpu.GetScreenSize();
                double ratioW = gpu.Width / size.X;
                double ratioH = gpu.Height / size.Y;
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

        private void TerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            terminal.Show();
        }

        private void SDCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kernel.MemMgr != null)
            {
                sdCardWindow.SetPath(kernel.MemMgr.SDCARD.GetSDCardPath());
                sdCardWindow.SetCapacity(kernel.MemMgr.SDCARD.GetCapacity());
                sdCardWindow.ShowDialog(this);
                ResetSDCard();
            }
        }

        private void ResetSDCard()
        {
            string path = sdCardWindow.GetPath();
            int capacity = sdCardWindow.GetCapacity();
            kernel.MemMgr.SDCARD.SetSDCardPath(path);
            byte sdCardStat = 0;
            if (path == null || path.Length == 0)
            {
                SDCardPath.Text = "SD Card Disabled";
                kernel.MemMgr.SDCARD.isPresent = false;
            }
            else
            {
                SDCardPath.Text = "SDC: " + path;
                sdCardStat = 1;
                kernel.MemMgr.SDCARD.isPresent = true;
                kernel.MemMgr.SDCARD.SetCapacity(capacity);
            }
            if (typeof(CH376SRegister) == kernel.MemMgr.SDCARD.GetType())
            {
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.SDCARD_STAT, sdCardStat);
            }
        }

        private void DisplayBoardVersion()
        {
            if (version == BoardVersion.RevB)
            {
                toolStripRevision.Text = "Rev B";
            }
            else
            {
                toolStripRevision.Text = "Rev C";
            }
            // force repaint
            statusStrip1.Invalidate();
        }
        private void ToolStripRevision_Click(object sender, EventArgs e)
        {
            if (version == BoardVersion.RevB)
            {
                version = BoardVersion.RevC;
            }
            else
            {
                version = BoardVersion.RevB;
            }
            kernel.SetVersion(version);
            if (uploaderWindow != null)
            {
                uploaderWindow.SetBoardVersion(version);
            }
            DisplayBoardVersion();
            // TODO - Reset the memory and reload the program?
        }

        private void EnableMenuItems()
        {
            RestartMenuItem.Enabled = true;
            DebugMenuItem.Enabled = true;
        }

        /*
         * DIP SWITCH Definition:
            DIP1 - BOOT MODE0 - b0 : $AF:E80E
            DIP2 - BOOT MODE1 - b1 : $AF:E80E
            DIP3 - USER DEFINED2
            DIP4 - USER DEFINED1
            DIP5 - USER DEFINED0
            DIP6 - HIGH-RES @ Boot (800 x 600) (when it is instantiated in Vicky II)
            DIP7 - GAMMA Correction ON/OFF
            DIP8- HDD INSTALLED
        */
        private bool[] switches = new bool[8];
        private void DipSwitch_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            if (version == BoardVersion.RevC)
            {
                int textOffset = 24;
                ToolStripStatusLabel label = (ToolStripStatusLabel)sender;
                int bankHeight = label.Height;
                int switchWidth = (label.Width - textOffset) / 8;
                int dipHeight = (bankHeight - 5) / 2;

                int offset = 2;
                int width = ((ToolStripLabel)sender).Width;
                int height = ((ToolStripLabel)sender).Height;
                e.Graphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, width, height));
                e.Graphics.DrawString("OFF", SystemFonts.SmallCaptionFont, Brushes.White, 0, 9);
                e.Graphics.DrawString("ON", SystemFonts.SmallCaptionFont, Brushes.White, 2, -2);

                for (int i = 0; i < 8; i++)
                {
                    // Draw the switch slide
                    e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(textOffset + (i * switchWidth), offset, switchWidth - offset, bankHeight - offset * 2));
                    e.Graphics.DrawRectangle(Pens.DarkGray, new Rectangle(textOffset + (i * switchWidth), offset, switchWidth - offset, bankHeight - offset * 2));
                    int top = (switches[i]) ? offset + 1 : offset + dipHeight;
                    e.Graphics.FillEllipse(Brushes.DarkSlateGray, new Rectangle(textOffset + (i * switchWidth) + 1, top, switchWidth - offset * 2, dipHeight));
                }
            }
        }

        private void DipSwitch_MouseDown(object sender, MouseEventArgs e)
        {
            ToolStripStatusLabel label = (ToolStripStatusLabel)sender;
            int textOffset = 24;
            int switchWidth = (label.Width - textOffset) / 8;
            int switchID = (e.X - 24) / switchWidth;
           
            if (switchID < 8)
            {
                // get current status and toggle it
                switches[switchID] = !switches[switchID];
                label.Invalidate();
                SetDipSwitchMemory();
            }
        }

        private void SetDipSwitchMemory()
        {
            // if kernel memory is available, set the memory
            byte bootMode = (byte)((switches[0] ? 0 : 1) + (switches[1] ? 0 : 2));
            byte userMode = (byte)((switches[2] ? 0 : 1) + (switches[3] ? 0 : 2) + (switches[4] ? 0 : 4));
            if (kernel.MemMgr != null)
            {
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.DIP_BOOT_MODE, bootMode);
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.DIP_USER_MODE, userMode);

                // switch 5 - high-res mode
                byte hiRes = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.VICKY_BASE_ADDR + 1);
                if (switches[4])
                {
                    hiRes |= 1;
                }
                else
                {
                    hiRes &= 0xFE;
                }
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.VICKY_BASE_ADDR + 1, hiRes);

                // switch 6 - Gamma
                byte MCR = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.VICKY_BASE_ADDR);
                if (switches[6])
                {
                    MCR |= 0x40;
                }
                else
                {
                    MCR &= 0b1011_1111;
                }
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.VICKY_BASE_ADDR, MCR);
            }
        }

        public void UpdateGamma(bool gamma)
        {
            switches[6] = gamma;
            dipSwitch.Invalidate();
        }

        public void UpdateHiRes(bool hires)
        {
            switches[4] = hires;
            dipSwitch.Invalidate();
        }

        private void JoystickSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            joystickWindow.Show();
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            // Allow if the file is Hex
            string[] obj = (string[])e.Data.GetData("FileName");
            if (obj != null && obj.Length > 0)
            {
                FileInfo info = new FileInfo(obj[0]);
                if (info.Extension.ToUpper().Equals(".HEX"))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            string[] obj = (string[])e.Data.GetData("FileName");
            if (obj != null && obj.Length > 0)
            {
                FileInfo info = new FileInfo(obj[0]);
                if (info.Extension.ToUpper().Equals(".HEX"))
                {
                    LoadHexFile(obj[0], false);
                }
            }
        }
    }
}

