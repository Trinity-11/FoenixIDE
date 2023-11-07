using FoenixIDE.Display;
using System;

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
            this.toolStripRevision = new System.Windows.Forms.ToolStripDropDownButton();
            this.dipSwitch = new System.Windows.Forms.ToolStripStatusLabel();
            this.ModeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastKeyPressed = new System.Windows.Forms.ToolStripStatusLabel();
            this.cpsPerf = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsPerf = new System.Windows.Forms.ToolStripStatusLabel();
            this.SDCardPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFNXMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenHexFile = new System.Windows.Forms.ToolStripMenuItem();
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
            this.convertHexToPGZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mIDIToVGMConvertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autorunEmulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScalingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolution_label_640x480MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scalingseparator1MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.scale1_0X_H480ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale1_5X_H480ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale2_0X_H480ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolution_label_640x400MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scalingseparator2MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.scale1_0X_H400ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale1_5X_H400ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale2_0X_H400ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terminalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DefaultKernelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RestartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.revCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revUPlusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revJrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revJr816ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revF256KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revF256K816ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 511);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(669, 30);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripRevision
            // 
            this.toolStripRevision.AutoSize = false;
            this.toolStripRevision.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.revBToolStripMenuItem,
            this.revCToolStripMenuItem,
            this.revUToolStripMenuItem,
            this.revUPlusToolStripMenuItem,
            this.revJrToolStripMenuItem,
            this.revJr816ToolStripMenuItem,
            this.revF256KToolStripMenuItem,
            this.revF256K816ToolStripMenuItem});
            this.toolStripRevision.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripRevision.Name = "toolStripRevision";
            this.toolStripRevision.Size = new System.Drawing.Size(110, 28);
            this.toolStripRevision.Text = "Rev B";
            this.toolStripRevision.ToolTipText = "Board Version";
            this.toolStripRevision.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStripRevision_DropDownItemClicked);
            // 
            // dipSwitch
            // 
            this.dipSwitch.AutoSize = false;
            this.dipSwitch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dipSwitch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.dipSwitch.Name = "dipSwitch";
            this.dipSwitch.Size = new System.Drawing.Size(129, 25);
            this.dipSwitch.Text = "toolStripStatusLabel1";
            this.dipSwitch.ToolTipText = "DIP Switches";
            this.dipSwitch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DipSwitch_MouseDown);
            this.dipSwitch.Paint += new System.Windows.Forms.PaintEventHandler(this.DipSwitch_Paint);
            // 
            // ModeText
            // 
            this.ModeText.Name = "ModeText";
            this.ModeText.Size = new System.Drawing.Size(26, 25);
            this.ModeText.Text = "Key";
            // 
            // lastKeyPressed
            // 
            this.lastKeyPressed.AutoSize = false;
            this.lastKeyPressed.Name = "lastKeyPressed";
            this.lastKeyPressed.Size = new System.Drawing.Size(30, 25);
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
            this.cpsPerf.Size = new System.Drawing.Size(110, 25);
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
            this.fpsPerf.Size = new System.Drawing.Size(55, 25);
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
            this.SDCardPath.Size = new System.Drawing.Size(204, 25);
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
            this.settingsToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(669, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFNXMLFileToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.menuOpenHexFile,
            this.loadWatchListToolStripMenuItem,
            this.saveWatchListToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadFNXMLFileToolStripMenuItem
            // 
            this.loadFNXMLFileToolStripMenuItem.Name = "loadFNXMLFileToolStripMenuItem";
            this.loadFNXMLFileToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.loadFNXMLFileToolStripMenuItem.Text = "&Load Project...";
            this.loadFNXMLFileToolStripMenuItem.Click += new System.EventHandler(this.LoadFNXMLFileToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveProjectToolStripMenuItem.Text = "&Save Project...";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectToolStripMenuItem_Click);
            // 
            // menuOpenHexFile
            // 
            this.menuOpenHexFile.Name = "menuOpenHexFile";
            this.menuOpenHexFile.Size = new System.Drawing.Size(184, 22);
            this.menuOpenHexFile.Text = "&Open Executable File";
            this.menuOpenHexFile.Click += new System.EventHandler(this.MenuOpenExecutableFile_Click);
            // 
            // loadWatchListToolStripMenuItem
            // 
            this.loadWatchListToolStripMenuItem.Name = "loadWatchListToolStripMenuItem";
            this.loadWatchListToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.loadWatchListToolStripMenuItem.Text = "Load Watch List...";
            this.loadWatchListToolStripMenuItem.Click += new System.EventHandler(this.LoadWatchListToolStripMenuItem_Click);
            // 
            // saveWatchListToolStripMenuItem
            // 
            this.saveWatchListToolStripMenuItem.Name = "saveWatchListToolStripMenuItem";
            this.saveWatchListToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveWatchListToolStripMenuItem.Text = "Save Watch List...";
            this.saveWatchListToolStripMenuItem.Click += new System.EventHandler(this.SaveWatchListToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
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
            this.ConvertBinToPGXToolStripMenuItem,
            this.convertHexToPGZToolStripMenuItem,
            this.mIDIToVGMConvertToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // uploaderToolStripMenuItem
            // 
            this.uploaderToolStripMenuItem.Name = "uploaderToolStripMenuItem";
            this.uploaderToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.uploaderToolStripMenuItem.Text = "&Uploader";
            this.uploaderToolStripMenuItem.Click += new System.EventHandler(this.UploaderToolStripMenuItem_Click);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadImageToolStripMenuItem.Text = "&Load Assets";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.LoadImageToolStripMenuItem_Click);
            // 
            // sDCardToolStripMenuItem
            // 
            this.sDCardToolStripMenuItem.Name = "sDCardToolStripMenuItem";
            this.sDCardToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.sDCardToolStripMenuItem.Text = "&SD Card";
            this.sDCardToolStripMenuItem.Click += new System.EventHandler(this.SDCardToolStripMenuItem_Click);
            // 
            // tileEditorToolStripMenuItem
            // 
            this.tileEditorToolStripMenuItem.Name = "tileEditorToolStripMenuItem";
            this.tileEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.tileEditorToolStripMenuItem.Text = "&Tile Editor";
            this.tileEditorToolStripMenuItem.Click += new System.EventHandler(this.TileEditorToolStripMenuItem_Click);
            // 
            // characterEditorToolStripMenuItem
            // 
            this.characterEditorToolStripMenuItem.Name = "characterEditorToolStripMenuItem";
            this.characterEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.characterEditorToolStripMenuItem.Text = "&Character Editor";
            this.characterEditorToolStripMenuItem.Click += new System.EventHandler(this.CharacterEditorToolStripMenuItem_Click);
            // 
            // joystickSimulatorToolStripMenuItem
            // 
            this.joystickSimulatorToolStripMenuItem.Name = "joystickSimulatorToolStripMenuItem";
            this.joystickSimulatorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.joystickSimulatorToolStripMenuItem.Text = "Joystick Simulator";
            this.joystickSimulatorToolStripMenuItem.Click += new System.EventHandler(this.JoystickSimulatorToolStripMenuItem_Click);
            // 
            // ConvertHexToPGXToolStripMenuItem
            // 
            this.ConvertHexToPGXToolStripMenuItem.Name = "ConvertHexToPGXToolStripMenuItem";
            this.ConvertHexToPGXToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.ConvertHexToPGXToolStripMenuItem.Text = "Convert Hex to PGX";
            this.ConvertHexToPGXToolStripMenuItem.Click += new System.EventHandler(this.ConvertHexToPGXToolStripMenuItem_Click);
            // 
            // ConvertBinToPGXToolStripMenuItem
            // 
            this.ConvertBinToPGXToolStripMenuItem.Name = "ConvertBinToPGXToolStripMenuItem";
            this.ConvertBinToPGXToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.ConvertBinToPGXToolStripMenuItem.Text = "Convert Bin to PGX";
            this.ConvertBinToPGXToolStripMenuItem.Click += new System.EventHandler(this.ConvertBinToPGXToolStripMenuItem_Click);
            // 
            // convertHexToPGZToolStripMenuItem
            // 
            this.convertHexToPGZToolStripMenuItem.Name = "convertHexToPGZToolStripMenuItem";
            this.convertHexToPGZToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.convertHexToPGZToolStripMenuItem.Text = "Convert Hex to PGZ";
            this.convertHexToPGZToolStripMenuItem.Click += new System.EventHandler(this.ConvertHexToPGZToolStripMenuItem_Click);
            // 
            // mIDIToVGMConvertToolStripMenuItem
            // 
            this.mIDIToVGMConvertToolStripMenuItem.Name = "mIDIToVGMConvertToolStripMenuItem";
            this.mIDIToVGMConvertToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.mIDIToVGMConvertToolStripMenuItem.Text = "MIDI to VGM Convert";
            this.mIDIToVGMConvertToolStripMenuItem.Click += new System.EventHandler(this.MIDIToVGMConvertToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autorunEmulatorToolStripMenuItem,
            this.viewScalingToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // autorunEmulatorToolStripMenuItem
            // 
            this.autorunEmulatorToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.autorunEmulatorToolStripMenuItem.Checked = true;
            this.autorunEmulatorToolStripMenuItem.CheckOnClick = true;
            this.autorunEmulatorToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autorunEmulatorToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.autorunEmulatorToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.autorunEmulatorToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.autorunEmulatorToolStripMenuItem.Name = "autorunEmulatorToolStripMenuItem";
            this.autorunEmulatorToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
            this.autorunEmulatorToolStripMenuItem.Size = new System.Drawing.Size(169, 23);
            this.autorunEmulatorToolStripMenuItem.Text = "Autorun Emulator";
            this.autorunEmulatorToolStripMenuItem.Click += new System.EventHandler(this.AutorunEmulatorToolStripMenuItem_Click);
            // 
            // viewScalingToolStripMenuItem
            // 
            this.viewScalingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolution_label_640x480MenuItem,
            this.scalingseparator1MenuItem,
            this.scale1_0X_H480ToolStripMenuItem,
            this.scale1_5X_H480ToolStripMenuItem,
            this.scale2_0X_H480ToolStripMenuItem,
            this.resolution_label_640x400MenuItem,
            this.scalingseparator2MenuItem,
            this.scale1_0X_H400ToolStripMenuItem,
            this.scale1_5X_H400ToolStripMenuItem,
            this.scale2_0X_H400ToolStripMenuItem});
            this.viewScalingToolStripMenuItem.Name = "viewScalingToolStripMenuItem";
            this.viewScalingToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.viewScalingToolStripMenuItem.Text = "View Scaling";
            //
            // Disabled Menu Text entry to specify 640x480
            //
            this.resolution_label_640x480MenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.resolution_label_640x480MenuItem.Name = "resolution_label_640x480MenuItem";
            this.resolution_label_640x480MenuItem.Size = new System.Drawing.Size(126, 22);
            this.resolution_label_640x480MenuItem.Text = "640 x 480";
            this.resolution_label_640x480MenuItem.Enabled = false;
            //
            // Disabled Menu Text entry to specify 640x400 
            //
            this.resolution_label_640x400MenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.resolution_label_640x400MenuItem.Name = "resolution_label_640x400MenuItem";
            this.resolution_label_640x400MenuItem.Size = new System.Drawing.Size(126, 22);
            this.resolution_label_640x400MenuItem.Text = "640 x 400";
            this.resolution_label_640x400MenuItem.Enabled = false;
            // 
            // scale1_0X_H480ToolStripMenuItem 
            // 640x480 at 1x
            // 
            this.scale1_0X_H480ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scale1_0X_H480ToolStripMenuItem.Name = "scale1_0X_H480ToolStripMenuItem";
            this.scale1_0X_H480ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.scale1_0X_H480ToolStripMenuItem.Text = "Scale 1.0X";
            this.scale1_0X_H480ToolStripMenuItem.Click += new System.EventHandler(this.scale1_0X_H480ToolStripMenuItem_Click);
            // 
            // scale1_5X_H480ToolStripMenuItem
            // 640x480 at 1.5x
            // 
            this.scale1_5X_H480ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scale1_5X_H480ToolStripMenuItem.Name = "scale1_5X_H480ToolStripMenuItem";
            this.scale1_5X_H480ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.scale1_5X_H480ToolStripMenuItem.Text = "Scale 1.5X";
            this.scale1_5X_H480ToolStripMenuItem.Click += new System.EventHandler(this.scale1_5X_H480ToolStripMenuItem_Click);
            // 
            // scale2_0X_H480ToolStripMenuItem
            // 640x480 at 2x
            // 
            this.scale2_0X_H480ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scale2_0X_H480ToolStripMenuItem.Name = "scale2_0X_H480ToolStripMenuItem";
            this.scale2_0X_H480ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.scale2_0X_H480ToolStripMenuItem.Text = "Scale 2.0X";
            this.scale2_0X_H480ToolStripMenuItem.Click += new System.EventHandler(this.scale2_0X_H480ToolStripMenuItem_Click);
                        // 
            // scale1_0X_H400ToolStripMenuItem 
            // 640x400 at 1x
            // 
            this.scale1_0X_H400ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scale1_0X_H400ToolStripMenuItem.Name = "scale1_0X_H400ToolStripMenuItem";
            this.scale1_0X_H400ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.scale1_0X_H400ToolStripMenuItem.Text = "Scale 1.0X";
            this.scale1_0X_H400ToolStripMenuItem.Click += new System.EventHandler(this.scale1_0X_H400ToolStripMenuItem_Click);
            // 
            // scale1_5X_H400ToolStripMenuItem
            // 640x400 at 1.5x
            // 
            this.scale1_5X_H400ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scale1_5X_H400ToolStripMenuItem.Name = "scale1_5X_H400ToolStripMenuItem";
            this.scale1_5X_H400ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.scale1_5X_H400ToolStripMenuItem.Text = "Scale 1.5X";
            this.scale1_5X_H400ToolStripMenuItem.Click += new System.EventHandler(this.scale1_5X_H400ToolStripMenuItem_Click);
            // 
            // scale2_0X_H400ToolStripMenuItem
            // 640x400 at 2x
            // 
            this.scale2_0X_H400ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scale2_0X_H400ToolStripMenuItem.Name = "scale2_0X_H400ToolStripMenuItem";
            this.scale2_0X_H400ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.scale2_0X_H400ToolStripMenuItem.Text = "Scale 2.0X";
            this.scale2_0X_H400ToolStripMenuItem.Click += new System.EventHandler(this.scale2_0X_H400ToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminalToolStripMenuItem,
            this.cPUToolStripMenuItem,
            this.memoryToolStripMenuItem,
            this.watchListToolStripMenuItem,
            this.assetListToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // terminalToolStripMenuItem
            // 
            this.terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            this.terminalToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.terminalToolStripMenuItem.Text = "&Terminal";
            this.terminalToolStripMenuItem.Click += new System.EventHandler(this.TerminalToolStripMenuItem_Click);
            // 
            // cPUToolStripMenuItem
            // 
            this.cPUToolStripMenuItem.Enabled = false;
            this.cPUToolStripMenuItem.Name = "cPUToolStripMenuItem";
            this.cPUToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.cPUToolStripMenuItem.Text = "&CPU";
            this.cPUToolStripMenuItem.Click += new System.EventHandler(this.CPUToolStripMenuItem_Click);
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.Enabled = false;
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            this.memoryToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.memoryToolStripMenuItem.Text = "&Memory";
            this.memoryToolStripMenuItem.Click += new System.EventHandler(this.MemoryToolStripMenuItem_Click);
            // 
            // watchListToolStripMenuItem
            // 
            this.watchListToolStripMenuItem.Name = "watchListToolStripMenuItem";
            this.watchListToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.watchListToolStripMenuItem.Text = "Watch List";
            this.watchListToolStripMenuItem.Click += new System.EventHandler(this.WatchListToolStripMenuItem_Click);
            // 
            // assetListToolStripMenuItem
            // 
            this.assetListToolStripMenuItem.Name = "assetListToolStripMenuItem";
            this.assetListToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.assetListToolStripMenuItem.Text = "Asset List";
            this.assetListToolStripMenuItem.Click += new System.EventHandler(this.AssetListToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DefaultKernelToolStripMenuItem,
            this.RestartMenuItem,
            this.DebugMenuItem});
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.resetToolStripMenuItem.Text = "&Reset";
            // 
            // DefaultKernelToolStripMenuItem
            // 
            this.DefaultKernelToolStripMenuItem.Enabled = false;
            this.DefaultKernelToolStripMenuItem.Name = "DefaultKernelToolStripMenuItem";
            this.DefaultKernelToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.DefaultKernelToolStripMenuItem.Text = "Restart Default &Kernel";
            this.DefaultKernelToolStripMenuItem.Click += new System.EventHandler(this.DefaultKernelToolStripMenuItem_Click);
            // 
            // RestartMenuItem
            // 
            this.RestartMenuItem.Enabled = false;
            this.RestartMenuItem.Name = "RestartMenuItem";
            this.RestartMenuItem.Size = new System.Drawing.Size(187, 22);
            this.RestartMenuItem.Text = "&Restart";
            this.RestartMenuItem.Click += new System.EventHandler(this.RestartMenuItemClick);
            // 
            // DebugMenuItem
            // 
            this.DebugMenuItem.Enabled = false;
            this.DebugMenuItem.Name = "DebugMenuItem";
            this.DebugMenuItem.Size = new System.Drawing.Size(187, 22);
            this.DebugMenuItem.Text = "&Debug";
            this.DebugMenuItem.Click += new System.EventHandler(this.DebugToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdateToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for &Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdateToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // revCToolStripMenuItem
            // 
            this.revCToolStripMenuItem.Name = "revCToolStripMenuItem";
            this.revCToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revCToolStripMenuItem.Text = "Rev C";
            // 
            // revUToolStripMenuItem
            // 
            this.revUToolStripMenuItem.Name = "revUToolStripMenuItem";
            this.revUToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revUToolStripMenuItem.Text = "Rev U";
            // 
            // revUToolStripMenuItem1
            // 
            this.revUPlusToolStripMenuItem.Name = "revUToolStripMenuItem1";
            this.revUPlusToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revUPlusToolStripMenuItem.Text = "Rev U+";
            // 
            // revJrToolStripMenuItem
            // 
            this.revJrToolStripMenuItem.Name = "revJrToolStripMenuItem";
            this.revJrToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revJrToolStripMenuItem.Text = "Rev Jr";
            // 
            // revJr816ToolStripMenuItem
            // 
            this.revJr816ToolStripMenuItem.Name = "revJr816ToolStripMenuItem";
            this.revJr816ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revJr816ToolStripMenuItem.Text = "Rev Jr(816)";
            // 
            // revF256KToolStripMenuItem
            // 
            this.revF256KToolStripMenuItem.Name = "revF256KToolStripMenuItem";
            this.revF256KToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revF256KToolStripMenuItem.Text = "Rev F256K";
            // 
            // revF256K816ToolStripMenuItem
            // 
            this.revF256K816ToolStripMenuItem.Name = "revF256K816ToolStripMenuItem";
            this.revF256K816ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revF256K816ToolStripMenuItem.Text = "Rev F256K(816)";
            // 
            // revBToolStripMenuItem
            // 
            this.revBToolStripMenuItem.Name = "revBToolStripMenuItem";
            this.revBToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.revBToolStripMenuItem.Text = "Rev B";
            // 
            // gpu
            // 
            this.gpu.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.gpu.BackColor = System.Drawing.Color.Blue;
            this.gpu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gpu.CausesValidation = false;
            this.gpu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpu.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gpu.Location = new System.Drawing.Point(0, 24);
            this.gpu.Margin = new System.Windows.Forms.Padding(4);
            this.gpu.MinimumSize = new System.Drawing.Size(640, 480);
            this.gpu.Name = "gpu";
            this.gpu.Size = new System.Drawing.Size(669, 487);
            this.gpu.TabIndex = 0;
            this.gpu.TabStop = false;
            this.gpu.TileEditorMode = false;
            this.gpu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Gpu_MouseDown);
            this.gpu.MouseEnter += new System.EventHandler(this.Gpu_MouseEnter);
            this.gpu.MouseLeave += new System.EventHandler(this.Gpu_MouseLeave);
            this.gpu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Gpu_MouseMove);
            this.gpu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Gpu_MouseUp);
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 541);
            this.Controls.Add(this.gpu);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(300, 300);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(685, 580);
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
        private global::System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem RestartMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem DebugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOpenHexFile;
        private System.Windows.Forms.ToolStripStatusLabel fpsPerf;
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
        private System.Windows.Forms.ToolStripDropDownButton toolStripRevision;
        private System.Windows.Forms.ToolStripStatusLabel dipSwitch;
        private System.Windows.Forms.ToolTip Tooltip;
        private System.Windows.Forms.ToolStripMenuItem characterEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joystickSimulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem watchListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWatchListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWatchListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConvertHexToPGXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConvertBinToPGXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autorunEmulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assetListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mIDIToVGMConvertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DefaultKernelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertHexToPGZToolStripMenuItem;
        public Gpu gpu;
        private System.Windows.Forms.ToolStripMenuItem viewScalingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator scalingseparator1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolution_label_640x480MenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale1_0X_H480ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale2_0X_H480ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale1_5X_H480ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolution_label_640x400MenuItem;
        private System.Windows.Forms.ToolStripSeparator scalingseparator2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale1_0X_H400ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale2_0X_H400ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale1_5X_H400ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revJr816ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revJrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revUPlusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revUToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revBToolStripMenuItem;
    }
}

