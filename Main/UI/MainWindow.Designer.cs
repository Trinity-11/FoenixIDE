using FoenixIDE.Display;

namespace FoenixIDE.UI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripRevision = new System.Windows.Forms.ToolStripStatusLabel();
            this.dipSwitch = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastKeyPressed = new System.Windows.Forms.ToolStripStatusLabel();
            this.cpsPerf = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsPerf = new System.Windows.Forms.ToolStripStatusLabel();
            this.SDCardPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenHexFile = new System.Windows.Forms.ToolStripMenuItem();
            this.openHexFileWoZeroingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFNXMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWatchListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWatchListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sDCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joystickSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertHexToPGXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertBinToPGXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terminalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RestartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.gpu = new FoenixIDE.Display.Gpu();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRevision,
            this.dipSwitch,
            this.ModeText,
            this.lastKeyPressed,
            this.cpsPerf,
            this.fpsPerf,
            this.SDCardPath});
            this.statusStrip1.Location = new System.Drawing.Point(0, 543);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(670, 28);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripRevision
            // 
            this.toolStripRevision.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripRevision.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripRevision.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripRevision.Name = "toolStripRevision";
            this.toolStripRevision.Size = new System.Drawing.Size(44, 23);
            this.toolStripRevision.Text = "Rev B";
            this.toolStripRevision.ToolTipText = "Board Version";
            this.toolStripRevision.Click += new System.EventHandler(this.ToolStripRevision_Click);
            // 
            // dipSwitch
            // 
            this.dipSwitch.AutoSize = false;
            this.dipSwitch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dipSwitch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.dipSwitch.Name = "dipSwitch";
            this.dipSwitch.Size = new System.Drawing.Size(129, 23);
            this.dipSwitch.Text = "toolStripStatusLabel1";
            this.dipSwitch.ToolTipText = "DIP Switches";
            this.dipSwitch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DipSwitch_MouseDown);
            this.dipSwitch.Paint += new System.Windows.Forms.PaintEventHandler(this.DipSwitch_Paint);
            // 
            // ModeText
            // 
            this.ModeText.Name = "ModeText";
            this.ModeText.Size = new System.Drawing.Size(29, 23);
            this.ModeText.Text = "Key";
            // 
            // lastKeyPressed
            // 
            this.lastKeyPressed.AutoSize = false;
            this.lastKeyPressed.Name = "lastKeyPressed";
            this.lastKeyPressed.Size = new System.Drawing.Size(30, 23);
            this.lastKeyPressed.Text = "$00";
            // 
            // cpsPerf
            // 
            this.cpsPerf.AutoSize = false;
            this.cpsPerf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cpsPerf.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.cpsPerf.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.cpsPerf.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpsPerf.Name = "cpsPerf";
            this.cpsPerf.Padding = new System.Windows.Forms.Padding(2);
            this.cpsPerf.Size = new System.Drawing.Size(110, 23);
            this.cpsPerf.Text = "CPS: 0";
            this.cpsPerf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cpsPerf.ToolTipText = "Cycles Per Second";
            // 
            // fpsPerf
            // 
            this.fpsPerf.AutoSize = false;
            this.fpsPerf.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.fpsPerf.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.fpsPerf.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpsPerf.Name = "fpsPerf";
            this.fpsPerf.Padding = new System.Windows.Forms.Padding(2);
            this.fpsPerf.Size = new System.Drawing.Size(55, 23);
            this.fpsPerf.Text = "FPS: 0";
            this.fpsPerf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fpsPerf.ToolTipText = "Frames Per Second";
            // 
            // SDCardPath
            // 
            this.SDCardPath.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SDCardPath.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.SDCardPath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SDCardPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.SDCardPath.Name = "SDCardPath";
            this.SDCardPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.SDCardPath.Size = new System.Drawing.Size(258, 23);
            this.SDCardPath.Spring = true;
            this.SDCardPath.Text = "SD Card Disabled";
            this.SDCardPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SDCardPath.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.SDCardPath.Click += new System.EventHandler(this.SDCardToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(670, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenHexFile,
            this.openHexFileWoZeroingToolStripMenuItem,
            this.loadFNXMLFileToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.loadWatchListToolStripMenuItem,
            this.saveWatchListToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // menuOpenHexFile
            // 
            this.menuOpenHexFile.Name = "menuOpenHexFile";
            this.menuOpenHexFile.Size = new System.Drawing.Size(232, 22);
            this.menuOpenHexFile.Text = "&Open Hex File";
            this.menuOpenHexFile.Click += new System.EventHandler(this.MenuOpenHexFile_Click);
            // 
            // openHexFileWoZeroingToolStripMenuItem
            // 
            this.openHexFileWoZeroingToolStripMenuItem.Name = "openHexFileWoZeroingToolStripMenuItem";
            this.openHexFileWoZeroingToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.openHexFileWoZeroingToolStripMenuItem.Text = "Open Hex File w/o &Zeroing";
            this.openHexFileWoZeroingToolStripMenuItem.Click += new System.EventHandler(this.MenuOpenHexFile_Click);
            // 
            // loadFNXMLFileToolStripMenuItem
            // 
            this.loadFNXMLFileToolStripMenuItem.Name = "loadFNXMLFileToolStripMenuItem";
            this.loadFNXMLFileToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.loadFNXMLFileToolStripMenuItem.Text = "&Load Project...";
            this.loadFNXMLFileToolStripMenuItem.Click += new System.EventHandler(this.LoadFNXMLFileToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.saveProjectToolStripMenuItem.Text = "&Save Project...";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectToolStripMenuItem_Click);
            // 
            // loadWatchListToolStripMenuItem
            // 
            this.loadWatchListToolStripMenuItem.Name = "loadWatchListToolStripMenuItem";
            this.loadWatchListToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.loadWatchListToolStripMenuItem.Text = "Load Watch List...";
            this.loadWatchListToolStripMenuItem.Click += new System.EventHandler(this.loadWatchListToolStripMenuItem_Click);
            // 
            // saveWatchListToolStripMenuItem
            // 
            this.saveWatchListToolStripMenuItem.Name = "saveWatchListToolStripMenuItem";
            this.saveWatchListToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.saveWatchListToolStripMenuItem.Text = "Save Watch List...";
            this.saveWatchListToolStripMenuItem.Click += new System.EventHandler(this.saveWatchListToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploaderToolStripMenuItem,
            this.loadImageToolStripMenuItem,
            this.sDCardToolStripMenuItem,
            this.tileEditorToolStripMenuItem,
            this.characterEditorToolStripMenuItem,
            this.joystickSimulatorToolStripMenuItem,
            this.ConvertHexToPGXToolStripMenuItem,
            this.ConvertBinToPGXToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // uploaderToolStripMenuItem
            // 
            this.uploaderToolStripMenuItem.Name = "uploaderToolStripMenuItem";
            this.uploaderToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.uploaderToolStripMenuItem.Text = "&Uploader";
            this.uploaderToolStripMenuItem.Click += new System.EventHandler(this.UploaderToolStripMenuItem_Click);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.loadImageToolStripMenuItem.Text = "&Load Bin/Image";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.LoadImageToolStripMenuItem_Click);
            // 
            // sDCardToolStripMenuItem
            // 
            this.sDCardToolStripMenuItem.Name = "sDCardToolStripMenuItem";
            this.sDCardToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.sDCardToolStripMenuItem.Text = "&SD Card";
            this.sDCardToolStripMenuItem.Click += new System.EventHandler(this.SDCardToolStripMenuItem_Click);
            // 
            // tileEditorToolStripMenuItem
            // 
            this.tileEditorToolStripMenuItem.Name = "tileEditorToolStripMenuItem";
            this.tileEditorToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.tileEditorToolStripMenuItem.Text = "&Tile Editor";
            this.tileEditorToolStripMenuItem.Click += new System.EventHandler(this.TileEditorToolStripMenuItem_Click);
            // 
            // characterEditorToolStripMenuItem
            // 
            this.characterEditorToolStripMenuItem.Name = "characterEditorToolStripMenuItem";
            this.characterEditorToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.characterEditorToolStripMenuItem.Text = "&Character Editor";
            this.characterEditorToolStripMenuItem.Click += new System.EventHandler(this.CharacterEditorToolStripMenuItem_Click);
            // 
            // joystickSimulatorToolStripMenuItem
            // 
            this.joystickSimulatorToolStripMenuItem.Name = "joystickSimulatorToolStripMenuItem";
            this.joystickSimulatorToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.joystickSimulatorToolStripMenuItem.Text = "Joystick Simulator";
            this.joystickSimulatorToolStripMenuItem.Click += new System.EventHandler(this.JoystickSimulatorToolStripMenuItem_Click);
            // 
            // ConvertHexToPGXToolStripMenuItem
            // 
            this.ConvertHexToPGXToolStripMenuItem.Name = "ConvertHexToPGXToolStripMenuItem";
            this.ConvertHexToPGXToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.ConvertHexToPGXToolStripMenuItem.Text = "Convert Hex to PGX";
            this.ConvertHexToPGXToolStripMenuItem.Click += new System.EventHandler(this.ConvertHexToPGXToolStripMenuItem_Click);
            // 
            // ConvertBinToPGXToolStripMenuItem
            // 
            this.ConvertBinToPGXToolStripMenuItem.Name = "ConvertBinToPGXToolStripMenuItem";
            this.ConvertBinToPGXToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.ConvertBinToPGXToolStripMenuItem.Text = "Convert Bin to PGX";
            this.ConvertBinToPGXToolStripMenuItem.Click += new System.EventHandler(this.ConvertBinToPGXToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminalToolStripMenuItem,
            this.cPUToolStripMenuItem,
            this.memoryToolStripMenuItem,
            this.watchListToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(73, 21);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // terminalToolStripMenuItem
            // 
            this.terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            this.terminalToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.terminalToolStripMenuItem.Text = "&Terminal";
            this.terminalToolStripMenuItem.Click += new System.EventHandler(this.TerminalToolStripMenuItem_Click);
            // 
            // cPUToolStripMenuItem
            // 
            this.cPUToolStripMenuItem.Enabled = false;
            this.cPUToolStripMenuItem.Name = "cPUToolStripMenuItem";
            this.cPUToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.cPUToolStripMenuItem.Text = "&CPU";
            this.cPUToolStripMenuItem.Click += new System.EventHandler(this.CPUToolStripMenuItem_Click);
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.Enabled = false;
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            this.memoryToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.memoryToolStripMenuItem.Text = "&Memory";
            this.memoryToolStripMenuItem.Click += new System.EventHandler(this.MemoryToolStripMenuItem_Click);
            // 
            // watchListToolStripMenuItem
            // 
            this.watchListToolStripMenuItem.Name = "watchListToolStripMenuItem";
            this.watchListToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.watchListToolStripMenuItem.Text = "Watch List";
            this.watchListToolStripMenuItem.Click += new System.EventHandler(this.WatchListToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RestartMenuItem,
            this.DebugMenuItem});
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            this.resetToolStripMenuItem.Text = "&Reset";
            // 
            // RestartMenuItem
            // 
            this.RestartMenuItem.Enabled = false;
            this.RestartMenuItem.Name = "RestartMenuItem";
            this.RestartMenuItem.Size = new System.Drawing.Size(117, 22);
            this.RestartMenuItem.Text = "&Restart";
            this.RestartMenuItem.Click += new System.EventHandler(this.RestartMenuItemClick);
            // 
            // DebugMenuItem
            // 
            this.DebugMenuItem.Enabled = false;
            this.DebugMenuItem.Name = "DebugMenuItem";
            this.DebugMenuItem.Size = new System.Drawing.Size(117, 22);
            this.DebugMenuItem.Text = "&Debug";
            this.DebugMenuItem.Click += new System.EventHandler(this.DebugToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(16, 502);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightBlue;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(654, 41);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(16, 502);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightBlue;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(16, 527);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(638, 16);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightBlue;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 25);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(670, 16);
            this.panel4.TabIndex = 6;
            // 
            // gpu
            // 
            this.gpu.BackColor = System.Drawing.Color.Blue;
            this.gpu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gpu.CausesValidation = false;
            this.gpu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpu.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gpu.Location = new System.Drawing.Point(16, 41);
            this.gpu.Margin = new System.Windows.Forms.Padding(4);
            this.gpu.MinimumSize = new System.Drawing.Size(640, 480);
            this.gpu.Name = "gpu";
            this.gpu.Size = new System.Drawing.Size(640, 486);
            this.gpu.TabIndex = 0;
            this.gpu.TabStop = false;
            this.gpu.TileEditorMode = false;
            this.gpu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gpu_MouseDown);
            this.gpu.MouseEnter += new System.EventHandler(this.Gpu_MouseEnter);
            this.gpu.MouseLeave += new System.EventHandler(this.Gpu_MouseLeave);
            this.gpu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Gpu_MouseMove);
            this.gpu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gpu_MouseUp);
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 571);
            this.Controls.Add(this.gpu);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(300, 300);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(686, 610);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Foenix IDE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.BasicWindow_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BasicWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BasicWindow_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Gpu gpu;
        private global::System.Windows.Forms.StatusStrip statusStrip1;
        private global::System.Windows.Forms.ToolStripStatusLabel ModeText;
        private global::System.Windows.Forms.ToolStripStatusLabel lastKeyPressed;
        private global::System.Windows.Forms.MenuStrip menuStrip1;
        private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem cPUToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripStatusLabel cpsPerf;
        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.Panel panel2;
        private global::System.Windows.Forms.Panel panel3;
        private global::System.Windows.Forms.Panel panel4;
        private global::System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem RestartMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem DebugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOpenHexFile;
        private System.Windows.Forms.ToolStripStatusLabel fpsPerf;
        private System.Windows.Forms.ToolStripMenuItem openHexFileWoZeroingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFNXMLFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terminalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sDCardToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel SDCardPath;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRevision;
        private System.Windows.Forms.ToolStripStatusLabel dipSwitch;
        private System.Windows.Forms.ToolTip Tooltip;
        private System.Windows.Forms.ToolStripMenuItem characterEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joystickSimulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem watchListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWatchListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWatchListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConvertHexToPGXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConvertBinToPGXToolStripMenuItem;
    }
}

