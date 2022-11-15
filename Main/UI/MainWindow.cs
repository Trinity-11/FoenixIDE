using FoenixIDE.Basic;
using FoenixIDE.CharEditor;
using FoenixIDE.Display;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.Devices.SDCard;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.Simulator.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
        private AssetWindow assetWindow = new AssetWindow();
        private SDCardWindow sdCardWindow = new SDCardWindow();
        private TileEditor tileEditor;
        private CharEditorWindow charEditor;
        public SerialTerminal terminal;
        private JoystickForm joystickWindow = new JoystickForm();
        private GameGeneratorForm GGF = new GameGeneratorForm();

        // Local variables and events
        private byte previousGraphicMode;
        private delegate void TileClickEvent(Point tile, PointF ratios, bool leftButton);
        private TileClickEvent TileClicked;
        private ResourceChecker ResChecker = new ResourceChecker();
        private delegate void TransmitByteFunction(byte Value);
        private delegate void ShowFormFunction();
        private String defaultKernel;
        private int jumpStartAddress;
        private bool disabledIRQs = false;
        private bool autoRun = true;
        private BoardVersion version = BoardVersion.RevC;
        public static MainWindow Instance = null;
        private delegate void WriteCPSFPSFunction(string CPS, string FPS);
        private bool fullScreen = false;

        public MainWindow(Dictionary<string, string> context)
        {
            bool autoRunCommandLineSpecified = false;
            bool boardVersionCommandLineSpecified = false;

            if (context != null)
            {
                if (context.ContainsKey("jumpStartAddress"))
                {
                    jumpStartAddress = int.Parse(context["jumpStartAddress"]);
                }
                if (context.ContainsKey("defaultKernel"))
                {
                    defaultKernel = context["defaultKernel"];
                }
                if (context.ContainsKey("autoRun"))
                {
                    autoRun = "true".Equals(context["autoRun"]);
                    autoRunCommandLineSpecified = true;
                }
                if (context.ContainsKey("disabledIRQs"))
                {
                    disabledIRQs = "true".Equals(context["disabledIRQs"]);
                }
                if (context.ContainsKey("version"))
                {
                    if (context["version"] == "RevB")
                    {
                        version = BoardVersion.RevB;
                    }
                    else if (context["version"] == "RevC")
                    {
                        version = BoardVersion.RevC;
                    }
                    else if (context["version"] == "RevU")
                    {
                        version = BoardVersion.RevU;
                    }
                    else if (context["version"] == "RevU+")
                    {
                        version = BoardVersion.RevUPlus;
                    }
                    else if (context["version"] == "RevJr")
                    {
                        version = BoardVersion.RevJr;
                    }
                    boardVersionCommandLineSpecified = true;
                }
            }
            // If the user didn't specify context switches, read the ini setting
            if (!autoRunCommandLineSpecified)
            {
                autoRun = Simulator.Properties.Settings.Default.Autorun;
            }
            if (!boardVersionCommandLineSpecified)
            {
                switch (Simulator.Properties.Settings.Default.BoardRevision)
                {
                    case "B":
                        version = BoardVersion.RevB;
                        break;
                    case "C":
                        version = BoardVersion.RevC;
                        break;
                    case "U":
                        version = BoardVersion.RevU;
                        break;
                    case "U+":
                        version = BoardVersion.RevUPlus;
                        break;
                    case "Jr":
                        version = BoardVersion.RevJr;
                        break;
                }
            }
            if (defaultKernel == null)
            {
                switch (version)
                {
                    case BoardVersion.RevB:
                        defaultKernel = @"roms\\kernel_B.hex";
                        break;
                    case BoardVersion.RevC:
                        defaultKernel = @"roms\\kernel_FMX.hex";
                        break;
                    case BoardVersion.RevU:
                        defaultKernel = @"roms\\kernel_U.hex";
                        break;
                    case BoardVersion.RevUPlus:
                        defaultKernel = @"roms\\kernel_U_Plus.hex";
                        break;
                    case BoardVersion.RevJr:
                        defaultKernel = @"roms\\kernel_F256Jr.hex";
                        break;
                }
            }
            if (context == null || "true".Equals(context["Continue"]))
            {
                InitializeComponent();
                Instance = this;
            }
        }

        private void BasicWindow_Load(object sender, EventArgs e)
        {
            kernel = new FoenixSystem(version, defaultKernel);
            terminal = new SerialTerminal();
            ShowDebugWindow();
            ShowMemoryWindow();

            // Now that the kernel is initialized, allocate variables to the GPU
            if (gpu.StartOfFrame == null)
            { 
                gpu.StartOfFrame += SOFRoutine;
            }
            if (gpu.StartOfLine == null)
            {
                gpu.StartOfLine += SOLRoutine;
            }
            if (gpu.GpuUpdated == null)
            {
                gpu.GpuUpdated += Gpu_Update_Cps_Fps;
            }
            gpu.VICKY = kernel.MemMgr.VICKY;
            if (version != BoardVersion.RevJr)
            {
                gpu.SetMode(0);
                gpu.VRAM = kernel.MemMgr.VIDEO;
                
                // This fontset is loaded just in case the kernel doesn't provide one.
                gpu.LoadFontSet("Foenix", @"Resources\Bm437_PhoenixEGA_8x8.bin", 0, CharacterSet.CharTypeCodes.ASCII_PET, CharacterSet.SizeCodes.Size8x8);

                joystickWindow.gabe = kernel.MemMgr.GABE;

                gpu.SetMCRAddress(0);
                gpu.SetFGLUTAddress(MemoryMap.FG_CHAR_LUT_PTR - gpu.VICKY.StartAddress);
                gpu.SetBGLUTAddress(MemoryMap.BG_CHAR_LUT_PTR - gpu.VICKY.StartAddress);
                gpu.SetTextStartAddress(MemoryMap.SCREEN_PAGE0 - gpu.VICKY.StartAddress);
                gpu.SetTextColorStartAddress(MemoryMap.SCREEN_PAGE1 - gpu.VICKY.StartAddress);
                gpu.SetCursorCtrlRegister(MemoryMap.VKY_TXT_CURSOR_CTRL_REG - gpu.VICKY.StartAddress);
                gpu.SetCursorCharacterAddress(MemoryMap.VKY_TXT_CURSOR_CHAR_REG - gpu.VICKY.StartAddress);
                gpu.SetCursorXAddress(MemoryMap.VKY_TXT_CURSOR_X_REG - gpu.VICKY.StartAddress);

                gpu.SetFontBaseAddress(MemoryMap.FONT0_MEMORY_BANK_START - gpu.VICKY.StartAddress);
                gpu.SetLUTBaseAddress(MemoryMap.GRP_LUT_BASE_ADDR - gpu.VICKY.StartAddress);
                gpu.SetGammaBaseAddress(MemoryMap.GAMMA_BASE_ADDR - gpu.VICKY.StartAddress);
                gpu.SetLineIRQRegister(MemoryMap.VKY_LINE_IRQ_CTRL_REG);
                gpu.SetSOL0Address(MemoryMap.VKY_LINE0_CMP_VALUE_LO);
                gpu.SetSOL1Address(MemoryMap.VKY_LINE1_CMP_VALUE_LO);
                gpu.SetMousePointerRegister(0x700);

                gpu.SetBitmapControlRegister(MemoryMap.BITMAP_CONTROL_REGISTER_ADDR - gpu.VICKY.StartAddress);
                gpu.SetTileMapBaseAddress(MemoryMap.TILE_CONTROL_REGISTER_ADDR - gpu.VICKY.StartAddress);
                gpu.SetTilesetBaseAddress(MemoryMap.TILESET_BASE_ADDR - gpu.VICKY.StartAddress);

                gpu.SetSpriteBaseAddress(MemoryMap.SPRITE_CONTROL_REGISTER_ADDR - gpu.VICKY.StartAddress);
            }
            else
            {
                gpu.SetMode(1);
                gpu.VRAM = kernel.MemMgr.RAM;

                // Addresses for VICKY in Junior are zero-based
                gpu.SetMCRAddress(0x1000);
                gpu.SetFGLUTAddress(MemoryMap.FG_CHAR_LUT_PTR_JR - 0xC000);  // IO Page 0
                gpu.SetBGLUTAddress(MemoryMap.BG_CHAR_LUT_PTR_JR - 0xC000);  // IO Page 0
                gpu.SetTextStartAddress(MemoryMap.SCREEN_PAGE_JR + 0x4000 - 0xC000);  // IO Page 2
                gpu.SetTextColorStartAddress(MemoryMap.SCREEN_PAGE_JR + 0x6000 - 0xC000);  // IO Page 3
                gpu.SetCursorCtrlRegister(MemoryMap.VKY_TXT_CURSOR_CTRL_REG_JR - 0xC000);  // IO Page 0
                gpu.SetCursorCharacterAddress(MemoryMap.VKY_TXT_CURSOR_CHAR_REG_JR - 0xC000); // IO Page 0
                gpu.SetCursorXAddress(MemoryMap.VKY_TXT_CURSOR_X_REG_JR - 0xC000); // IO Page 0

                gpu.SetFontBaseAddress(MemoryMap.FONT_MEMORY_BANK_START_JR + 0x2000 - 0xC000);  // IO Page 1
                gpu.SetLUTBaseAddress(MemoryMap.GRP_LUT_BASE_ADDR_JR + 0x2000 - 0xC000);  // IO Page 1
                gpu.SetGammaBaseAddress(MemoryMap.GAMMA_BASE_ADDR_JR - 0xC000);  // IO Page 0
                gpu.SetLineIRQRegister(MemoryMap.VKY_LINE_IRQ_CTRL_REG_JR - 0xC000);  // IO Page 0
                gpu.SetSOL0Address(MemoryMap.VKY_LINE_CMP_VALUE_JR - 0xC000);  // IO Page 0
                gpu.SetMousePointerRegister(0x1000);  // IO Page 0

                gpu.SetBitmapControlRegister(0xD100 - 0xC000);  // IO Page 0
                gpu.SetTileMapBaseAddress(MemoryMap.TILE_CONTROL_REGISTER_ADDR_JR - 0xC000);
                gpu.SetTilesetBaseAddress(MemoryMap.TILESET_BASE_ADDR_JR - 0xC000);
                gpu.SetSpriteBaseAddress(0xD900 - 0xC000);
            }
           
            if (disabledIRQs)
            {
                debugWindow.DisableIRQs(true);
            }

            if (sender != null)
            {
                this.Top = 0;
                this.Left = 0;

                //this.Width = debugWindow.Left;
                if (this.Width > 1200)
                {
                    this.Width = 1200;
                }
                this.Height = Convert.ToInt32(this.Width * 0.75);
            }

            SetDipSwitchMemory();
            if (version != BoardVersion.RevJr)
            {
                // Code is tightly coupled with memory manager
                kernel.MemMgr.UART1.TransmitByte += SerialTransmitByte;
                kernel.MemMgr.UART2.TransmitByte += SerialTransmitByte;
            }
            kernel.MemMgr.SDCARD.sdCardIRQMethod += SDCardInterrupt;
            kernel.MemMgr.KEYBOARD.TriggerMouseInterrupt += TriggerMouseInterrupt;
            kernel.MemMgr.KEYBOARD.TriggerKeyboardInterrupt += TriggerKeyboardInterrupt;

            kernel.ResCheckerRef = ResChecker;

            watchWindow.SetKernel(kernel);
            assetWindow.SetKernel(kernel);

            DisplayBoardVersion();
            EnableMenuItems();
            ResetSDCard();

            if (autoRun)
            {
                debugWindow.RunButton_Click(null, null);
            }
            autorunEmulatorToolStripMenuItem.Checked = autoRun;
        }

        private void CenterForm(Form form)
        {
            int left = this.Left + (this.Width - form.Width) / 2;
            int top = this.Top + (this.Height - form.Height) / 2;
            form.Location = new Point(left, top);
        }

        private void LoadExecutableFile(string Filename)
        {
            debugWindow.Pause();
            kernel.SetVersion(version);
            if (kernel.ResetCPU(Filename))
            {
                gpu.Refresh();
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
                    Memory = kernel.CPU.MemMgr,
                    Left = debugWindow.Left,
                    Top = debugWindow.Top + debugWindow.Height
                };
                memoryWindow.Show();
            }
            else
            {
                memoryWindow.Memory = kernel.CPU.MemMgr;
                memoryWindow.BringToFront();
            }
            if (memoryWindow.WriteMCRBytes == null)
            {
                memoryWindow.WriteMCRBytes += WriteMCRBytesToVicky;
            }
            if (memoryWindow.ReadMCRBytes == null)
            {
                memoryWindow.ReadMCRBytes += ReadMCRBytesFromVicky;
            }
            memoryWindow.UpdateMCRButtons();
            if (memoryWindow.SetGamma == null)
            {
                memoryWindow.SetGamma += UpdateGamma;
            }
            if (memoryWindow.SetHiRes == null)
            {
                memoryWindow.SetHiRes += UpdateHiRes;
            }
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
                int top = this.Top + (this.Height - uploaderWindow.Height) / 2;
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
                CenterForm(watchWindow);
                watchWindow.Show();
            }
            else
            {
                watchWindow.BringToFront();
            }
            watchWindow.UpdateList();
        }

        /*
         * Loading image into memory requires the user to specify what kind of image (tile, bitmap, sprite).
         * What address location in video RAM.
         */
        public void LoadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssetLoader loader = new AssetLoader
            {
                StartPosition = FormStartPosition.CenterParent,
                MemMgrRef = kernel.CPU.MemMgr,
                ResChecker = ResChecker
            };
            IWin32Window parent = this;
            if (!(sender is ToolStripMenuItem))
            {
                parent = (IWin32Window)sender;
            }
            if (loader.ShowDialog(parent) == DialogResult.OK)
            {
                AssetWindow.Instance.UpdateAssets();
            }
        }

        public void SOFRoutine()
        {
            // Check if the interrupt is enabled
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
            if (version == BoardVersion.RevJr)
            {
                // we need to this to avoid using the MMU IO Paging function
                mask = kernel.MemMgr.VICKY.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0_JR - 0xC000);
            }
            
            if (!kernel.CPU.DebugPause)
            {
                // Set the SOF Interrupt
                byte IRQ0 = kernel.MemMgr.INTERRUPT.ReadByte(0);
                if (version == BoardVersion.RevJr)
                {
                    // we need to this to avoid using the MMU IO Paging function
                    IRQ0 = kernel.MemMgr.VICKY.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR - 0xC000);
                }
                IRQ0 |= (byte)Register0.FNX0_INT00_SOF;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                if ((~mask & (byte)Register0.FNX0_INT00_SOF) == (byte)Register0.FNX0_INT00_SOF)
                {
                    kernel.CPU.Pins.IRQ = true;
                }
            }
        }

        public void SOLRoutine()
        {
            // Check if the interrupt is enabled
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
            if (version == BoardVersion.RevJr)
            {
                mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0_JR);
            }
            // if (!kernel.CPU.DebugPause && !kernel.CPU.Flags.IrqDisable && ((~mask & (byte)Register0.FNX0_INT01_SOL) == (byte)Register0.FNX0_INT01_SOL))
            if (!kernel.CPU.DebugPause && ((~mask & (byte)Register0.FNX0_INT01_SOL) == (byte)Register0.FNX0_INT01_SOL))
            {
                // Set the SOL Interrupt
                byte IRQ0 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                if (version == BoardVersion.RevJr)
                {
                    IRQ0 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR);
                }
                IRQ0 |= (byte)Register0.FNX0_INT01_SOL;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                kernel.CPU.Pins.IRQ = true;
            }
        }

        public void setGpuPeriod(uint time)
        {
            gpu.SetRefreshPeriod(time);
        }

        public void SDCardInterrupt(CH376SInterrupt irq)
        {
            // Check if the SD Card interrupt is allowed
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG1);
            if (version == BoardVersion.RevJr)
            {
                mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0_JR + 1);
            }
            if (!kernel.CPU.DebugPause && (~mask & (byte)Register1.FNX1_INT07_SDCARD) == (byte)Register1.FNX1_INT07_SDCARD)
            {
                // Set the SD Card Interrupt
                byte IRQ1 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG1);
                if (version == BoardVersion.RevJr)
                {
                    IRQ1 = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0_JR + 1);
                }
                IRQ1 |= (byte)Register1.FNX1_INT07_SDCARD;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(1, IRQ1);
                kernel.CPU.Pins.IRQ = true;

                // Write the interrupt result
                kernel.MemMgr.SDCARD.WriteByte(0, (byte)irq);
            }
        }

        private void FullScreenToggle()
        {
            if (this.fullScreen == false)
            {
                this.fullScreen = true;

                this.menuStrip1.Visible = false;
                this.statusStrip1.Visible = false;
                this.debugWindow.Visible = false;               // not sure that is needed
                this.memoryWindow.Visible = false;              // maybe maximizing GPU Window is enough?

                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.fullScreen = false;

                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;

                this.menuStrip1.Visible = true;
                this.statusStrip1.Visible = true;
                this.debugWindow.Visible = true;
                this.debugWindow.Show();
                this.debugWindow.Refresh();
                this.memoryWindow.Visible = true;
                this.memoryWindow.Show();
                this.memoryWindow.Refresh();

                this.Focus();
                this.Refresh();
            }
        }

        private void BasicWindow_KeyDown(object sender, KeyEventArgs e)
        {

            // we take over Shift+F11 and Shift+F5
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.F11:
                        FullScreenToggle();
                        break;
                    case Keys.F5:
                        this.debugWindow.RunButton_Click(sender, null);
                        break;
                }
            }

            ScanCode scanCode = ScanCodes.GetScanCode(e.KeyCode);
            if (scanCode != ScanCode.sc_null)
            {
                e.Handled = true;
                lastKeyPressed.Text = "$" + ((byte)scanCode).ToString("X2");
                if (kernel.MemMgr != null && !kernel.CPU.DebugPause)
                {
                   WriteKeyboardCode(scanCode);
                }
            }
            else if (e.KeyCode == Keys.Pause)
            {
                e.Handled = true;
                if (kernel.MemMgr != null && !kernel.CPU.DebugPause)
                {
                    kernel.MemMgr.KEYBOARD.WriteScanCodeSequence(new byte[] { 0xe1, 0x1d, 0x45, 0xe1, 0x9d, 0xc5 }, 6);
                }
                lastKeyPressed.Text = "Break";
            }
            else
            {
                lastKeyPressed.Text = "";
            }
        }

        private void WriteKeyboardCode(ScanCode sc)
        {
            if (version != BoardVersion.RevJr)
            {
                // Check if the Keyboard interrupt is allowed
                byte mask = kernel.MemMgr.ReadByte(MemoryMap.INT_MASK_REG1);
                if ((~mask & (byte)Register1.FNX1_INT00_KBD) != 0)
                {
                    kernel.MemMgr.KEYBOARD.WriteByte(0, (byte)sc);
                    kernel.MemMgr.KEYBOARD.WriteByte(4, 0);

                    TriggerKeyboardInterrupt();
                }
            }
            else
            {
                // Check if the Keyboard interrupt is allowed
                byte mask = kernel.MemMgr.VICKY.ReadByte(MemoryMap.INT_MASK_REG0_JR - 0xC000);
                if ((~mask & (byte)Register0_JR.JR0_INT02_KBD) != 0)
                {
                    kernel.MemMgr.KEYBOARD.WriteByte(0, (byte)sc);
                    kernel.MemMgr.KEYBOARD.WriteByte(4, 0);

                    TriggerKeyboardInterrupt();
                }
            }
        }
        private void TriggerKeyboardInterrupt()
        {
            if (version != BoardVersion.RevJr)
            {
                // Set the Keyboard Interrupt
                byte IrqVal = kernel.MemMgr.INTERRUPT.ReadByte(1);
                IrqVal |= (byte)Register1.FNX1_INT00_KBD;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(1, IrqVal);
                kernel.CPU.Pins.IRQ = true;
            }
            else
            {
                // Set the Keyboard Interrupt
                byte IrqVal = kernel.MemMgr.INTERRUPT.ReadByte(0);
                IrqVal |= (byte)Register0_JR.JR0_INT02_KBD;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, IrqVal);
                kernel.CPU.Pins.IRQ = true;
            }
        }

        private void TriggerMouseInterrupt()
        {
            if (version != BoardVersion.RevJr)
            {
                // Set the Mouse Interrupt
                byte IRQ0 = kernel.MemMgr.INTERRUPT.ReadByte(0);
                IRQ0 |= (byte)Register0.FNX0_INT07_MOUSE;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                kernel.CPU.Pins.IRQ = true;
            }
            else
            {
                // Set the Mouse Interrupt
                byte IRQ0 = kernel.MemMgr.INTERRUPT.ReadByte(0);
                IRQ0 |= (byte)Register0_JR.JR0_INT03_MOUSE;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
                kernel.CPU.Pins.IRQ = true;
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
                    WriteKeyboardCode(scanCode);
                }
            }
            else
            {
                lastKeyPressed.Text = "";
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Write_CPS_FPS_Safe(string CPS, string FPS)
        {
            if (statusStrip1.InvokeRequired)
            {
                var d = new WriteCPSFPSFunction(Write_CPS_FPS_Safe);
                statusStrip1.Invoke(d, new object[] { CPS, FPS });

            }
            else
            {
                cpsPerf.Text = CPS;
                fpsPerf.Text = FPS;
                statusStrip1.Update();
            }
        }

        int previousCounter = 0;
        int previousFrame = 0;
        DateTime previousTime = DateTime.Now;
        private void Gpu_Update_Cps_Fps()
        {
            if (kernel != null && kernel.CPU != null)
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
                else
                {
                    Write_CPS_FPS_Safe("CPS: Stopped", "FPS: N/A");
                }
                WriteRTCTime(currentTime);
            }
            else
            {
                cpsPerf.Text = "CPS: 0";
                fpsPerf.Text = "FPS: 0";
            }
        }

        private void WriteRTCTime(DateTime currentTime)
        {
            if (version != BoardVersion.RevJr)
            {
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
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR - 0xC000, BCD(currentTime.Second));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 2 - 0xC000, BCD(currentTime.Minute));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 4 - 0xC000, BCD(currentTime.Hour));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 6 - 0xC000, BCD(currentTime.Day));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 8 - 0xC000, BCD(currentTime.Month));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 10 - 0xC000, BCD(currentTime.Year % 100));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 12 - 0xC000, BCD(currentTime.Year / 100));
                kernel.MemMgr.VICKY.WriteByte(MemoryLocations.MemoryMap.RTC_SEC_JR + 14 - 0xC000, (byte)(currentTime.DayOfWeek + 1));
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
            if (kernel.ResetCPU(null))
            {
                gpu.Refresh();
                debugWindow.SetKernel(kernel);
                debugWindow.ClearTrace();
                SetDipSwitchMemory();
                memoryWindow.Memory = kernel.CPU.MemMgr;
                if (version == BoardVersion.RevJr)
                {
                    // Now update other registers
                    kernel.MemMgr.MMU.Reset();
                    kernel.MemMgr.VICKY.WriteWord(0xD000 - 0xC000, 1);
                    kernel.MemMgr.VICKY.WriteWord(0xD002 - 0xC000, 0x1540);
                }
                memoryWindow.UpdateMCRButtons();
                ResetSDCard();

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
            if (kernel.ResetCPU(null))
            {
                debugWindow.SetKernel(kernel);
                debugWindow.ClearTrace();
                SetDipSwitchMemory();
                memoryWindow.Memory = kernel.CPU.MemMgr;
                if (version == BoardVersion.RevJr)
                {
                    // Now update other registers
                    kernel.MemMgr.MMU.Reset();
                    kernel.MemMgr.VICKY.WriteWord(0xD000 - 0xC000, 1);
                    kernel.MemMgr.VICKY.WriteWord(0xD002 - 0xC000, 0x1540);
                }
                memoryWindow.UpdateMCRButtons();
                ResetSDCard();

                debugWindow.Refresh();
            }
        }

        private void DefaultKernelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            previousCounter = 0;
            debugWindow.Pause();
            if (kernel.ResetCPU(defaultKernel))
            {
                gpu.Refresh();
                debugWindow.SetKernel(kernel);
                debugWindow.ClearTrace();
                SetDipSwitchMemory();
                memoryWindow.Memory = kernel.CPU.MemMgr;
                if (version == BoardVersion.RevJr)
                {
                    // Now update other registers
                    kernel.MemMgr.MMU.Reset();
                    kernel.MemMgr.VICKY.WriteWord(0xD000 - 0xC000, 1);
                    kernel.MemMgr.VICKY.WriteWord(0xD002 - 0xC000, 0x1540);
                }
                memoryWindow.UpdateMCRButtons();
                ResetSDCard();

                // Restart the CPU
                debugWindow.RunButton_Click(null, null);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            gpu.StopTimer();
            kernel.CPU.DebugPause = true;
            gpu.GpuUpdated -= Gpu_Update_Cps_Fps;
            gpu.StartOfFrame = null;
            gpu.StartOfLine = null;
            
            ModeText.Text = "Shutting down CPU thread";
            if (kernel.CPU != null)
            {
                if (kernel.CPU.CPUThread != null)
                {
                    kernel.CPU.CPUThread.Abort();
                    kernel.CPU.CPUThread.Join(1000);
                }
            }

            if (debugWindow != null)
            {
                debugWindow.Close();
            }
            if (memoryWindow != null)
            {
                memoryWindow.Close();
            }
            if (GGF != null)
            {
                GGF.Close();
            }
        }

        private void MenuOpenExecutableFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Hex Files|*.hex|PGX Files|*.pgx|PGZ Files|*.pgz",
                Title = "Select an Executable File",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadExecutableFile(dialog.FileName);
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
                kernel.ResCheckerRef = ResChecker;
                if (kernel.ResetCPU(dialog.FileName))
                {
                    gpu.Refresh();
                    debugWindow.Pause();
                    SetDipSwitchMemory();
                    ShowDebugWindow();
                    if (version == BoardVersion.RevJr)
                    {
                        // Now update other registers
                        kernel.MemMgr.MMU.Reset();
                        kernel.MemMgr.VICKY.WriteWord(0xD000 - 0xC000, 1);
                        kernel.MemMgr.VICKY.WriteWord(0xD002 - 0xC000, 0x1540);
                    }
                    ShowMemoryWindow();
                    EnableMenuItems();
                    assetWindow.UpdateAssets();
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

        private void SaveWatchListToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void LoadWatchListToolStripMenuItem_Click(object sender, EventArgs e)
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
                if (!watchWindow.Visible)
                {
                    watchWindow.Show();
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        // When the editor window is closed, exit the TileEditorMode
        private void EditorWindowClosed(object sender, FormClosedEventArgs e)
        {
            gpu.TileEditorMode = false;
            // Restore the previous graphics mode
            if (version != BoardVersion.RevJr)
            {
                kernel.MemMgr.VICKY.WriteByte(0, previousGraphicMode);
            }
            else
            {
                kernel.MemMgr.VICKY.WriteByte(0x1000, previousGraphicMode);
            }
            tileEditor.Dispose();
            tileEditor = null;
            TileClicked = null;
        }

        private void TileEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileEditor == null)
            {
                tileEditor = new TileEditor();
                tileEditor.SetMemory(kernel.MemMgr);
                tileEditor.SetResourceChecker(kernel.ResCheckerRef);
                gpu.TileEditorMode = true;
                // Set Vicky into Tile mode
                if (version != BoardVersion.RevJr)
                {
                    previousGraphicMode = kernel.MemMgr.VICKY.ReadByte(0);
                    kernel.MemMgr.VICKY.WriteByte(0, 0x10);
                    // Enable borders
                    kernel.MemMgr.VICKY.WriteByte(4, 1);
                }
                else
                {
                    previousGraphicMode = kernel.MemMgr.VICKY.ReadByte(0xD000 - 0xC000);
                    kernel.MemMgr.VICKY.WriteByte(0x1000, 0x10);
                    // Enable borders
                    kernel.MemMgr.VICKY.WriteByte(0x1004, 1);
                }
                CenterForm(tileEditor);
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
            if (version == BoardVersion.RevJr)
            {
                size = gpu.GetScreenSize_JR();
            }
            float ratioW = gpu.Width / (float)size.X;
            float ratioH = gpu.Height / (float)size.Y;
            if (version != BoardVersion.RevJr)
            {
                bool borderEnabled = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.BORDER_CTRL_REG) == 1;
                double borderWidth = borderEnabled ? kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.BORDER_X_SIZE) : 0;
                double borderHeight = borderEnabled ? kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.BORDER_Y_SIZE) : 0;
                if (gpu.TileEditorMode && e.Button != MouseButtons.None)
                {
                    if ((e.X / ratioW > borderWidth && e.X / ratioW < size.X - borderWidth) && (e.Y / ratioH > borderHeight && e.Y / ratioH < size.Y - borderHeight))
                    {
                        this.Cursor = Cursors.Hand;
                        bool leftButton = e.Button == MouseButtons.Left;
                        TileClicked?.Invoke(new Point((int)(e.X / ratioW), (int)(e.Y / ratioH)), new PointF(ratioW, ratioH), leftButton);
                    }
                    else
                    {
                        this.Cursor = Cursors.No;
                    }
                }
                else if (kernel.MemMgr != null)
                {
                    GenerateMouseInterrupt(e);
                }
            }
        }

        private void Gpu_MouseDown(object sender, MouseEventArgs e)
        {
            Point size = gpu.GetScreenSize();
            if (version == BoardVersion.RevJr)
            {
                size = gpu.GetScreenSize_JR();
            }
            float ratioW = gpu.Width / (float)size.X;
            float ratioH = gpu.Height / (float)size.Y;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    left = true;
                    break;
                case MouseButtons.Right:
                    right = true;
                    break;
                case MouseButtons.Middle:
                    middle = true;
                    break;
            }
            if (gpu.TileEditorMode && gpu.Cursor != Cursors.No)
            {
                bool leftButton = e.Button == MouseButtons.Left;
                TileClicked?.Invoke(new Point((int)(e.X / ratioW), (int)(e.Y / ratioH)), new PointF(ratioW, ratioH), leftButton);
            }
            else
            {
                GenerateMouseInterrupt(e);
            }
        }


        private void Gpu_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    left = false;
                    break;
                case MouseButtons.Right:
                    right = false;
                    break;
                case MouseButtons.Middle:
                    middle = false;
                    break;
            }
            if (!gpu.TileEditorMode)
            {
                GenerateMouseInterrupt(e);
            }
        }

        // Remember the state of the mouse buttons
        bool left = false;
        bool right = false;
        bool middle = false;

        private void GenerateMouseInterrupt(MouseEventArgs e)
        {
            Point size = gpu.GetScreenSize();
            if (version == BoardVersion.RevJr)
            {
                gpu.GetScreenSize_JR();
            }
            double ratioW = gpu.Width / (double)size.X;
            double ratioH = gpu.Height / (double)size.Y;
            int X = (int)(e.X / ratioW);
            int Y = (int)(e.Y / ratioH);

            byte buttons = (byte)((left ? 1 : 0) + (right ? 2 : 0) + (middle ? 4 : 0));

            kernel.MemMgr.VICKY.WriteWord(0x702, X);
            kernel.MemMgr.VICKY.WriteWord(0x704, Y);
            kernel.MemMgr.VICKY.WriteByte(0x706, buttons);

            // Generate three interrupts - to emulate how the PS/2 controller works
            byte mask = kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);
            // The PS/2 packet is byte0, xm, ym
            if ((~mask & (byte)Register0.FNX0_INT07_MOUSE) == (byte)Register0.FNX0_INT07_MOUSE)
            {
                kernel.MemMgr.KEYBOARD.MousePackets((byte)(8 + (middle ? 4 : 0) + (right ? 2 : 0) + (left ? 1 : 0)), (byte)(X & 0xFF), (byte)(Y & 0xFF));
            }
        }

        private void Gpu_MouseLeave(object sender, EventArgs e)
        {
            left = false;
            right = false;
            middle = false;
            if (version != BoardVersion.RevJr && gpu.IsMousePointerVisible() || gpu.TileEditorMode)
            {
                Cursor.Show();
            }
            this.Cursor = Cursors.Default;
        }

        private void Gpu_MouseEnter(object sender, EventArgs e)
        {
            if (version != BoardVersion.RevJr && gpu.IsMousePointerVisible() && !gpu.TileEditorMode)
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
                sdCardWindow.SetClusterSize(kernel.MemMgr.SDCARD.GetClusterSize());
                sdCardWindow.SetFSType(kernel.MemMgr.SDCARD.GetFSType().ToString());
                sdCardWindow.ShowDialog(this);
                ResetSDCard();
            }
        }

        private void ResetSDCard()
        {
            string path = sdCardWindow.GetPath();
            int capacity = sdCardWindow.GetCapacity();
            int clusterSize = sdCardWindow.GetClusterSize();
            string fsType = sdCardWindow.GetFSType();
            bool ISOMode = sdCardWindow.GetISOMode();

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
                kernel.MemMgr.SDCARD.isPresent = true;
                kernel.MemMgr.SDCARD.SetISOMode(ISOMode);
                sdCardStat = 1;

                kernel.MemMgr.SDCARD.SetCapacity(capacity);
                kernel.MemMgr.SDCARD.SetClusterSize(clusterSize);

                if ("FAT12".Equals(fsType))
                {
                    kernel.MemMgr.SDCARD.SetFSType(FSType.FAT12);
                }
                else if ("FAT16".Equals(fsType))
                {
                    kernel.MemMgr.SDCARD.SetFSType(FSType.FAT16);
                }
                else if ("FAT32".Equals(fsType))
                {
                    kernel.MemMgr.SDCARD.SetFSType(FSType.FAT32);
                }
                kernel.MemMgr.SDCARD.ResetMbrBootSector();
            }
            if (typeof(CH376SRegister) == kernel.MemMgr.SDCARD.GetType())
            {
                kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.SDCARD_STAT, sdCardStat);
            }
        }

        private void DisplayBoardVersion()
        {
            string shortVersion = "C";
            if (version == BoardVersion.RevB)
            {
                toolStripRevision.Text = "Rev B";
                shortVersion = "B";
            }
            else if (version == BoardVersion.RevC)
            {
                toolStripRevision.Text = "Rev C";
            }
            else if (version == BoardVersion.RevU)
            {
                toolStripRevision.Text = "Rev U";
                shortVersion = "U";
            }
            else if (version == BoardVersion.RevUPlus)
            {
                toolStripRevision.Text = "Rev U+";
                shortVersion = "U+";
            }
            else
            {
                toolStripRevision.Text = "Rev F256Jr";
                shortVersion = "Jr";
            }
            // force repaint
            statusStrip1.Invalidate();
            Simulator.Properties.Settings.Default.BoardRevision = shortVersion;
            Simulator.Properties.Settings.Default.Save();
        }

        // The user clicks on the tool strip for a different board version: B, FMX, U or U+
        private void ToolStripRevision_Click(object sender, EventArgs e)
        {
            if (version == BoardVersion.RevB)
            {
                version = BoardVersion.RevC;
                defaultKernel = @"roms\\kernel_FMX.hex";
            }
            else if (version == BoardVersion.RevC)
            {
                version = BoardVersion.RevU;
                defaultKernel = @"roms\\kernel_U.hex";
            }
            else if (version == BoardVersion.RevU)
            {
                version = BoardVersion.RevUPlus;
                defaultKernel = @"roms\\kernel_U_Plus.hex";
            }
            else if (version == BoardVersion.RevUPlus)
            {
                version = BoardVersion.RevJr;
                defaultKernel = @"roms\\kernel_F256Jr.hex";
            }
            else
            {
                version = BoardVersion.RevC;
                defaultKernel = @"roms\\kernel_FMX.hex";
            }

            kernel.SetVersion(version);
            if (uploaderWindow != null)
            {
                uploaderWindow.SetBoardVersion(version);
            }
            DisplayBoardVersion();
            // Reset the memory, keyboard, GABE and reload the program?
            debugWindow.Pause();
            kernel.lstFile.Lines.Clear();
            BasicWindow_Load(null, null);
        }

        private void EnableMenuItems()
        {
            RestartMenuItem.Enabled = true;
            DebugMenuItem.Enabled = true;
            DefaultKernelToolStripMenuItem.Enabled = true;
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

            if (version != BoardVersion.RevB)
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
                Font smallFont = new Font(FontFamily.GenericMonospace, 7.0f);
                e.Graphics.DrawString("ON", smallFont, Brushes.White, 2, -1);  // ON above
                e.Graphics.DrawString("OFF", smallFont, Brushes.White, 0, 7);  // OFF below

                for (int i = 0; i < 8; i++)
                {
                    // Draw the switch slide
                    e.Graphics.FillRectangle(Brushes.DarkSlateGray, new Rectangle(textOffset + (i * switchWidth), offset, switchWidth - offset, bankHeight - offset * 2));
                    e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(textOffset + (i * switchWidth), offset, switchWidth - offset, bankHeight - offset * 2));
                    int top = (switches[i]) ? offset + 1 : offset + dipHeight;
                    e.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(textOffset + (i * switchWidth) + 1, top, switchWidth - offset * 2, dipHeight));
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
            if (version != BoardVersion.RevJr)
            {
                // if kernel memory is available, set the memory
                byte bootMode = (byte)((switches[0] ? 0 : 1) + (switches[1] ? 0 : 2));
                byte userMode = (byte)((switches[2] ? 0 : 1) + (switches[3] ? 0 : 2) + (switches[4] ? 0 : 4));
                if (kernel.MemMgr != null)
                {
                    kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.DIP_BOOT_MODE, bootMode);
                    kernel.MemMgr.WriteByte(MemoryLocations.MemoryMap.DIP_USER_MODE, userMode);

                    // switch 5 - high-res mode
                    byte hiRes = kernel.MemMgr.ReadByte(kernel.MemMgr.VICKY.StartAddress + 1);
                    if (switches[4])
                    {
                        hiRes |= 1;
                    }
                    else
                    {
                        hiRes &= 0xFE;
                    }
                    kernel.MemMgr.WriteByte(kernel.MemMgr.VICKY.StartAddress + 1, hiRes);

                    // switch 6 - Gamma
                    byte MCR = kernel.MemMgr.ReadByte(kernel.MemMgr.VICKY.StartAddress);
                    if (switches[6])
                    {
                        MCR |= 0x40;
                    }
                    else
                    {
                        MCR &= 0b1011_1111;
                    }
                    kernel.MemMgr.WriteByte(kernel.MemMgr.VICKY.StartAddress, MCR);
                }
            }
            else
            { 
                byte bootMode = (byte)((switches[0] ? 0 : 1) + (switches[1] ? 0 : 2) + (switches[2] ? 0 : 4) + (switches[3] ? 0 : 8));
                byte userMode = (byte)((switches[4] ? 0 : 1) + (switches[5] ? 0 : 2) + (switches[6] ? 0 : 4));
                if (kernel.MemMgr != null && kernel.MemMgr.VICKY != null)
                {
                    // switch 6 - Gamma
                    byte MCR = kernel.MemMgr.VICKY.ReadByte(MemoryMap.VICKY_START_JR - 0xC000);
                    if (switches[7])
                    {
                        MCR |= 0x40;
                    }
                    else
                    {
                        MCR &= 0b1011_1111;
                    }
                    kernel.MemMgr.VICKY.WriteByte(MemoryMap.VICKY_START_JR - 0xC000, MCR);
                }
            }
        }

        public void WriteMCRBytesToVicky(byte low, byte high)
        {
            int baseAddr = version == BoardVersion.RevJr ? 0xD000 - 0xC000 : 0;

            kernel.MemMgr.VICKY.WriteByte(baseAddr, low);
            kernel.MemMgr.VICKY.WriteByte(baseAddr + 1, high);
        }

        public ushort ReadMCRBytesFromVicky()
        {
            return (ushort)kernel.MemMgr.VICKY.ReadWord(version == BoardVersion.RevJr ? 0xD000 - 0xC000 : 0);
        }

        public void UpdateGamma(bool gamma)
        {
            if (version != BoardVersion.RevJr)
            {
                switches[6] = gamma;
                dipSwitch.Invalidate();
            }
            else
            {
                switches[7] = gamma;
                dipSwitch.Invalidate();
            }
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
                string extension = info.Extension.ToUpper();
                if (extension.Equals(".HEX") || extension.Equals(".PGX") || extension.Equals(".PGZ"))
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
                string extension = info.Extension.ToUpper();
                if (extension.Equals(".HEX") || extension.Equals(".PGX") || extension.Equals(".PGZ"))
                {
                    LoadExecutableFile(obj[0]);
                }
            }
        }

        // Convert a Hex file to PGX
        // Header is 4 bytes: PGX,$1, 4 byte: jump address
        private void ConvertHexToPGXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Hex Files|*.hex",
                Title = "Select a Hex File",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MemoryRAM temporaryRAM = new MemoryRAM(0, 4 * 1024 * 1024);
                HexFile.Load(temporaryRAM, dialog.FileName, 0, out List<int> DataStartAddress, out List<int> DataLength);

                if (DataStartAddress.Count > 1)
                {
                    MessageBox.Show("The Hex file has multiple segments, use a PGZ instead.", "Convert to PGX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // write the file
                    string outputFileName = Path.ChangeExtension(dialog.FileName, "PGX");

                    byte[] buffer = new byte[DataLength[0]];
                    temporaryRAM.CopyIntoBuffer(DataStartAddress[0], DataLength[0], buffer);
                    using (BinaryWriter writer = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
                    {
                        // 8 byte header
                        writer.Write((byte)'P');
                        writer.Write((byte)'G');
                        writer.Write((byte)'X');
                        writer.Write((byte)1);
                        writer.Write(DataStartAddress[0]);
                        writer.Write(buffer);
                    }
                }
            }
        }

        private void ConvertHexToPGZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Hex Files|*.hex",
                Title = "Select a Hex File",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MemoryRAM temporaryRAM = new MemoryRAM(0, 4 * 1024 * 1024);
                HexFile.Load(temporaryRAM, dialog.FileName, 0, out List<int> DataStartAddress, out List<int> DataLength);
                // write the file
                string outputFileName = Path.ChangeExtension(dialog.FileName, "PGZ");

                int startAddress = temporaryRAM.ReadLong(0xFFE1);
                // If the start address 0, then check at $38:FFE1, the FMX, U+ executable location.
                if (startAddress == 0)
                {
                    startAddress = temporaryRAM.ReadLong(0x38FFE1);
                }
                // If the start address is 0, then check at $18:FFE1, for a U executable
                if (startAddress == 0)
                {
                    startAddress = temporaryRAM.ReadLong(0x18FFE1);
                }
                // The last effort is to use the address of the first segment
                if (startAddress == 0)
                {
                    startAddress = DataStartAddress[0];
                }
                using (BinaryWriter writer = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
                {
                    // Header
                    writer.Write((byte)'Z');
                    byte[] buffer;
                    for (int i = 0; i < DataStartAddress.Count; i++)
                    {
                        buffer = PrepareHeader(DataStartAddress[i], DataLength[i]);
                        writer.Write(buffer);
                        buffer = new byte[DataLength[i]];
                        temporaryRAM.CopyIntoBuffer(DataStartAddress[i], DataLength[i], buffer);
                        writer.Write(buffer);
                    }

                    // the last 6 bytes are the start address and 0 length
                    buffer = PrepareHeader(startAddress, 0);
                    writer.Write(buffer);
                }
            }
        }

        private byte[] PrepareHeader(int start, int length)
        {
            byte[] buf = new byte[6];
            buf[0] = (byte)(start & 0xFF);
            buf[1] = (byte)((start >> 8) & 0xFF);
            buf[2] = (byte)((start >> 16) & 0xFF);
            buf[3] = (byte)(length & 0xFF);
            buf[4] = (byte)((length >> 8) & 0xFF);
            buf[5] = (byte)((length >> 16) & 0xFF);
            return buf;
        }
        private void ConvertBinToPGXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Bin Files|*.bin",
                Title = "Select a Bin File",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Ask the user what address to write in the header
                string StrAddress = Microsoft.VisualBasic.Interaction.InputBox("Enter the PGX Start Address (Hexadecimal)", "PGX Start Address", "0");
                if (!StrAddress.Equals("0"))
                {
                    byte[] buffer = File.ReadAllBytes(dialog.FileName);
                    // write the file
                    int DataStartAddress = Convert.ToInt32(StrAddress, 16);
                    string outputFileName = Path.ChangeExtension(dialog.FileName, "PGX");
                    using (BinaryWriter writer = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
                    {
                        // 8 byte header
                        writer.Write((byte)'P');
                        writer.Write((byte)'G');
                        writer.Write((byte)'X');
                        writer.Write((byte)1);
                        writer.Write(DataStartAddress);
                        writer.Write(buffer);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Start Address", "PGX File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int VersionValue(string value)
        {
            string[] parts = value.Split('.');
            int intValue = 0;
            foreach (string part in parts)
            {
                try
                {
                    int partialValue = int.Parse(part);
                    intValue = (intValue + partialValue) * 100;
                }
                finally
                {

                }
            }
            return intValue;
        }

        /**
         * Call the GitHub REST API with / repos / Trinity-11 / FoenixIDE / releases.
         * From the returned JSON, check which one is the latest and if it matches ours.
         * 
         */
        private void CheckForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string URL = "https://api.github.com/repos/Trinity-11/FoenixIDE/releases";
            HttpClient client = new HttpClient();

            String version = AboutForm.AppVersion();
            int appVersion = VersionValue(version);

            // Add an Accept header for JSON format
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("user-agent", "Foenix IDE");
            bool done = false;

            // List data response.
            HttpResponseMessage response = client.GetAsync(URL).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                string value = response.Content.ReadAsStringAsync().Result;
                MatchCollection matches = Regex.Matches(value, "\"tag_name\":\"(.*?)\"");
                foreach (Match match in matches)
                {
                    string fullRelease = match.Groups[1].Value;
                    string release = fullRelease.Replace("release-", "");
                    int releaseVersion = VersionValue(release);
                    if (releaseVersion > appVersion)
                    {
                        MessageBox.Show(string.Format("A new version is available.\nThe latest release is {0}, you are running version {1}.", release, version), "Version Check");
                        done = true;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            if (!done)
            {
                MessageBox.Show("You are running the latest version of the Foenix IDE.", "Version Check");
            }

            // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }

        private void GameEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GGF.Visible)
            {
                GGF.Show();
            }
            else
            {
                GGF.BringToFront();
            }
        }

        private void autorunEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Simulator.Properties.Settings.Default.Autorun = autorunEmulatorToolStripMenuItem.Checked;
            Simulator.Properties.Settings.Default.Save();
        }

        private void assetListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!assetWindow.Visible)
            {
                CenterForm(assetWindow);
                assetWindow.Show();
            }
            else
            {
                assetWindow.BringToFront();
            }
        }

        private void mIDIToVGMConvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MIDI_VGM_From midiForm = new MIDI_VGM_From();
            midiForm.Show();
        }
    }
}

