using FoenixIDE.Simulator.Controls;

namespace FoenixIDE.UI
{
    partial class CPUWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPUWindow));
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.WatchButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.StepOverButton = new System.Windows.Forms.Button();
            this.BPLabel = new System.Windows.Forms.Label();
            this.BPCombo = new System.Windows.Forms.ComboBox();
            this.AddBPButton = new System.Windows.Forms.Button();
            this.DeleteBPButton = new System.Windows.Forms.Button();
            this.StepButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationInput = new System.Windows.Forms.TextBox();
            this.JumpButton = new System.Windows.Forms.Button();
            this.lastLine = new System.Windows.Forms.TextBox();
            this.stackText = new System.Windows.Forms.TextBox();
            this.UpdateTraceTimer = new System.Windows.Forms.Timer(this.components);
            this.SecondPanel = new System.Windows.Forms.Panel();
            this.ClearTraceButton = new System.Windows.Forms.Button();
            this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.DebugPanel = new System.Windows.Forms.PictureBox();
            this.AddBPOverlayButton = new System.Windows.Forms.Button();
            this.DeleteBPOverlayButton = new System.Windows.Forms.Button();
            this.InspectOverlayButton = new System.Windows.Forms.Button();
            this.StepOverOverlayButton = new System.Windows.Forms.Button();
            this.HeaderTextbox = new System.Windows.Forms.Label();
            this.irqPanel = new System.Windows.Forms.Panel();
            this.SDCardCheckBox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.OPL2LCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.OPL2RCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.MPU401Checkbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.COM1Checkbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.COM2Checkbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.FDCCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.MouseCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.RTCCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.TMR2Checkbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.TMR1Checkbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.TMR0Checkbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.SOLCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.Reg2Label = new System.Windows.Forms.Label();
            this.Reg1Label = new System.Windows.Forms.Label();
            this.Reg0Label = new System.Windows.Forms.Label();
            this.KeyboardCheckBox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.SOFCheckbox = new FoenixIDE.Simulator.Controls.ColorCheckBox();
            this.BreakOnIRQCheckBox = new System.Windows.Forms.CheckBox();
            this.LabelOverlayButton = new System.Windows.Forms.Button();
            this.registerDisplay1 = new FoenixIDE.RegisterDisplay();
            this.HeaderPanel.SuspendLayout();
            this.SecondPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DebugPanel)).BeginInit();
            this.irqPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Controls.Add(this.WatchButton);
            this.HeaderPanel.Controls.Add(this.ResetButton);
            this.HeaderPanel.Controls.Add(this.StepOverButton);
            this.HeaderPanel.Controls.Add(this.BPLabel);
            this.HeaderPanel.Controls.Add(this.BPCombo);
            this.HeaderPanel.Controls.Add(this.AddBPButton);
            this.HeaderPanel.Controls.Add(this.DeleteBPButton);
            this.HeaderPanel.Controls.Add(this.StepButton);
            this.HeaderPanel.Controls.Add(this.RunButton);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(608, 24);
            this.HeaderPanel.TabIndex = 2;
            // 
            // WatchButton
            // 
            this.WatchButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.WatchButton.Location = new System.Drawing.Point(305, 0);
            this.WatchButton.Name = "WatchButton";
            this.WatchButton.Size = new System.Drawing.Size(75, 24);
            this.WatchButton.TabIndex = 5;
            this.WatchButton.Text = "Watch";
            this.WatchButton.UseVisualStyleBackColor = true;
            this.WatchButton.Click += new System.EventHandler(this.WatchButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ResetButton.Location = new System.Drawing.Point(235, 0);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(70, 24);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // StepOverButton
            // 
            this.StepOverButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepOverButton.Location = new System.Drawing.Point(145, 0);
            this.StepOverButton.Name = "StepOverButton";
            this.StepOverButton.Size = new System.Drawing.Size(90, 24);
            this.StepOverButton.TabIndex = 3;
            this.StepOverButton.Text = "Step Over (F7)";
            this.StepOverButton.UseVisualStyleBackColor = true;
            this.StepOverButton.Click += new System.EventHandler(this.StepOverButton_Click);
            // 
            // BPLabel
            // 
            this.BPLabel.Location = new System.Drawing.Point(379, 2);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.BPLabel.Size = new System.Drawing.Size(58, 17);
            this.BPLabel.TabIndex = 9;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BPCombo
            // 
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new System.Drawing.Point(439, 2);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new System.Drawing.Size(121, 21);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Location = new System.Drawing.Point(560, 0);
            this.AddBPButton.Name = "AddBPButton";
            this.AddBPButton.Size = new System.Drawing.Size(24, 24);
            this.AddBPButton.TabIndex = 7;
            this.AddBPButton.Text = "+";
            this.AddBPButton.UseVisualStyleBackColor = true;
            this.AddBPButton.Click += new System.EventHandler(this.AddBPButton_Click);
            // 
            // DeleteBPButton
            // 
            this.DeleteBPButton.Location = new System.Drawing.Point(584, 0);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new System.EventHandler(this.DeleteBPButton_Click);
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new System.Drawing.Point(75, 0);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(70, 24);
            this.StepButton.TabIndex = 2;
            this.StepButton.Text = "Step (F6)";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunButton.Location = new System.Drawing.Point(0, 0);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 24);
            this.RunButton.TabIndex = 1;
            this.RunButton.Tag = "0";
            this.RunButton.Text = "Run (F5)";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationLabel.Location = new System.Drawing.Point(145, 0);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.locationLabel.Size = new System.Drawing.Size(64, 24);
            this.locationLabel.TabIndex = 9;
            this.locationLabel.Text = "Location $";
            // 
            // locationInput
            // 
            this.locationInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationInput.Location = new System.Drawing.Point(209, 0);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new System.Drawing.Size(64, 23);
            this.locationInput.TabIndex = 10;
            this.locationInput.Text = "00:0000";
            this.locationInput.Validated += new System.EventHandler(this.LocationInput_Validated);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.JumpButton.Location = new System.Drawing.Point(75, 0);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(70, 24);
            this.JumpButton.TabIndex = 10;
            this.JumpButton.Text = "Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
            // 
            // lastLine
            // 
            this.lastLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lastLine.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lastLine.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastLine.Location = new System.Drawing.Point(0, 493);
            this.lastLine.Name = "lastLine";
            this.lastLine.ReadOnly = true;
            this.lastLine.Size = new System.Drawing.Size(605, 23);
            this.lastLine.TabIndex = 4;
            this.lastLine.Text = "Click [Step] to execute an instruction";
            this.lastLine.WordWrap = false;
            this.lastLine.MouseEnter += new System.EventHandler(this.DebugPanel_Leave);
            // 
            // stackText
            // 
            this.stackText.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.stackText.Dock = System.Windows.Forms.DockStyle.Right;
            this.stackText.Font = new System.Drawing.Font("Consolas", 10F);
            this.stackText.Location = new System.Drawing.Point(608, 0);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.ReadOnly = true;
            this.stackText.Size = new System.Drawing.Size(150, 515);
            this.stackText.TabIndex = 3;
            // 
            // UpdateTraceTimer
            // 
            this.UpdateTraceTimer.Tick += new System.EventHandler(this.UpdateTraceTick);
            // 
            // SecondPanel
            // 
            this.SecondPanel.Controls.Add(this.locationInput);
            this.SecondPanel.Controls.Add(this.locationLabel);
            this.SecondPanel.Controls.Add(this.JumpButton);
            this.SecondPanel.Controls.Add(this.ClearTraceButton);
            this.SecondPanel.Location = new System.Drawing.Point(0, 25);
            this.SecondPanel.Name = "SecondPanel";
            this.SecondPanel.Size = new System.Drawing.Size(366, 24);
            this.SecondPanel.TabIndex = 5;
            // 
            // ClearTraceButton
            // 
            this.ClearTraceButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ClearTraceButton.Location = new System.Drawing.Point(0, 0);
            this.ClearTraceButton.Name = "ClearTraceButton";
            this.ClearTraceButton.Size = new System.Drawing.Size(75, 24);
            this.ClearTraceButton.TabIndex = 9;
            this.ClearTraceButton.Text = "Clear Trace";
            this.ClearTraceButton.UseVisualStyleBackColor = true;
            this.ClearTraceButton.Click += new System.EventHandler(this.ClearTraceButton_Click);
            // 
            // DebugPanel
            // 
            this.DebugPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DebugPanel.Location = new System.Drawing.Point(0, 126);
            this.DebugPanel.Margin = new System.Windows.Forms.Padding(2);
            this.DebugPanel.Name = "DebugPanel";
            this.DebugPanel.Size = new System.Drawing.Size(605, 367);
            this.DebugPanel.TabIndex = 6;
            this.DebugPanel.TabStop = false;
            this.DebugPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DebugPanel_MouseClick);
            this.DebugPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DebugPanel_MouseMove);
            // 
            // AddBPOverlayButton
            // 
            this.AddBPOverlayButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.AddBPOverlayButton.FlatAppearance.BorderSize = 0;
            this.AddBPOverlayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.AddBPOverlayButton.Location = new System.Drawing.Point(99, 200);
            this.AddBPOverlayButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddBPOverlayButton.Name = "AddBPOverlayButton";
            this.AddBPOverlayButton.Size = new System.Drawing.Size(18, 18);
            this.AddBPOverlayButton.TabIndex = 7;
            this.AddBPOverlayButton.TabStop = false;
            this.AddBPOverlayButton.Text = "+";
            this.AddBPOverlayButton.UseVisualStyleBackColor = false;
            this.AddBPOverlayButton.Visible = false;
            this.AddBPOverlayButton.Click += new System.EventHandler(this.PlusButton_Click);
            // 
            // DeleteBPOverlayButton
            // 
            this.DeleteBPOverlayButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DeleteBPOverlayButton.FlatAppearance.BorderSize = 0;
            this.DeleteBPOverlayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DeleteBPOverlayButton.Location = new System.Drawing.Point(118, 200);
            this.DeleteBPOverlayButton.Margin = new System.Windows.Forms.Padding(2);
            this.DeleteBPOverlayButton.Name = "DeleteBPOverlayButton";
            this.DeleteBPOverlayButton.Size = new System.Drawing.Size(18, 18);
            this.DeleteBPOverlayButton.TabIndex = 8;
            this.DeleteBPOverlayButton.TabStop = false;
            this.DeleteBPOverlayButton.Text = "-";
            this.DeleteBPOverlayButton.UseVisualStyleBackColor = false;
            this.DeleteBPOverlayButton.Visible = false;
            this.DeleteBPOverlayButton.Click += new System.EventHandler(this.MinusButton_Click);
            // 
            // InspectOverlayButton
            // 
            this.InspectOverlayButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.InspectOverlayButton.FlatAppearance.BorderSize = 0;
            this.InspectOverlayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.InspectOverlayButton.Location = new System.Drawing.Point(137, 200);
            this.InspectOverlayButton.Margin = new System.Windows.Forms.Padding(1);
            this.InspectOverlayButton.Name = "InspectOverlayButton";
            this.InspectOverlayButton.Size = new System.Drawing.Size(38, 18);
            this.InspectOverlayButton.TabIndex = 9;
            this.InspectOverlayButton.TabStop = false;
            this.InspectOverlayButton.Text = "Mem";
            this.InspectOverlayButton.UseVisualStyleBackColor = false;
            this.InspectOverlayButton.Visible = false;
            this.InspectOverlayButton.Click += new System.EventHandler(this.InspectButton_Click);
            // 
            // StepOverOverlayButton
            // 
            this.StepOverOverlayButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.StepOverOverlayButton.FlatAppearance.BorderSize = 0;
            this.StepOverOverlayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StepOverOverlayButton.Font = new System.Drawing.Font("Arial Narrow", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StepOverOverlayButton.Location = new System.Drawing.Point(176, 200);
            this.StepOverOverlayButton.Margin = new System.Windows.Forms.Padding(1);
            this.StepOverOverlayButton.Name = "StepOverOverlayButton";
            this.StepOverOverlayButton.Size = new System.Drawing.Size(21, 18);
            this.StepOverOverlayButton.TabIndex = 10;
            this.StepOverOverlayButton.TabStop = false;
            this.StepOverOverlayButton.Text = "►";
            this.StepOverOverlayButton.UseVisualStyleBackColor = false;
            this.StepOverOverlayButton.Visible = false;
            this.StepOverOverlayButton.Click += new System.EventHandler(this.StepOverOverlayButton_Click);
            // 
            // HeaderTextbox
            // 
            this.HeaderTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderTextbox.BackColor = System.Drawing.Color.Black;
            this.HeaderTextbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.HeaderTextbox.Location = new System.Drawing.Point(2, 102);
            this.HeaderTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderTextbox.Name = "HeaderTextbox";
            this.HeaderTextbox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.HeaderTextbox.Size = new System.Drawing.Size(603, 22);
            this.HeaderTextbox.TabIndex = 11;
            this.HeaderTextbox.UseCompatibleTextRendering = true;
            this.HeaderTextbox.UseMnemonic = false;
            // 
            // irqPanel
            // 
            this.irqPanel.Controls.Add(this.SDCardCheckBox);
            this.irqPanel.Controls.Add(this.OPL2LCheckbox);
            this.irqPanel.Controls.Add(this.OPL2RCheckbox);
            this.irqPanel.Controls.Add(this.MPU401Checkbox);
            this.irqPanel.Controls.Add(this.COM1Checkbox);
            this.irqPanel.Controls.Add(this.COM2Checkbox);
            this.irqPanel.Controls.Add(this.FDCCheckbox);
            this.irqPanel.Controls.Add(this.MouseCheckbox);
            this.irqPanel.Controls.Add(this.RTCCheckbox);
            this.irqPanel.Controls.Add(this.TMR2Checkbox);
            this.irqPanel.Controls.Add(this.TMR1Checkbox);
            this.irqPanel.Controls.Add(this.TMR0Checkbox);
            this.irqPanel.Controls.Add(this.SOLCheckbox);
            this.irqPanel.Controls.Add(this.Reg2Label);
            this.irqPanel.Controls.Add(this.Reg1Label);
            this.irqPanel.Controls.Add(this.Reg0Label);
            this.irqPanel.Controls.Add(this.KeyboardCheckBox);
            this.irqPanel.Controls.Add(this.SOFCheckbox);
            this.irqPanel.Controls.Add(this.BreakOnIRQCheckBox);
            this.irqPanel.Location = new System.Drawing.Point(372, 25);
            this.irqPanel.Name = "irqPanel";
            this.irqPanel.Size = new System.Drawing.Size(230, 74);
            this.irqPanel.TabIndex = 12;
            // 
            // SDCardCheckBox
            // 
            this.SDCardCheckBox.Checked = true;
            this.SDCardCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SDCardCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SDCardCheckBox.IsActive = false;
            this.SDCardCheckBox.Location = new System.Drawing.Point(88, 38);
            this.SDCardCheckBox.Name = "SDCardCheckBox";
            this.SDCardCheckBox.Size = new System.Drawing.Size(15, 14);
            this.SDCardCheckBox.TabIndex = 34;
            this.SDCardCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SDCardCheckBox.UseVisualStyleBackColor = true;
            this.SDCardCheckBox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // OPL2LCheckbox
            // 
            this.OPL2LCheckbox.Checked = true;
            this.OPL2LCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OPL2LCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OPL2LCheckbox.IsActive = false;
            this.OPL2LCheckbox.Location = new System.Drawing.Point(189, 56);
            this.OPL2LCheckbox.Name = "OPL2LCheckbox";
            this.OPL2LCheckbox.Size = new System.Drawing.Size(15, 14);
            this.OPL2LCheckbox.TabIndex = 33;
            this.OPL2LCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OPL2LCheckbox.UseVisualStyleBackColor = true;
            this.OPL2LCheckbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // OPL2RCheckbox
            // 
            this.OPL2RCheckbox.Checked = true;
            this.OPL2RCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OPL2RCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OPL2RCheckbox.IsActive = false;
            this.OPL2RCheckbox.Location = new System.Drawing.Point(206, 56);
            this.OPL2RCheckbox.Name = "OPL2RCheckbox";
            this.OPL2RCheckbox.Size = new System.Drawing.Size(15, 14);
            this.OPL2RCheckbox.TabIndex = 32;
            this.OPL2RCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OPL2RCheckbox.UseVisualStyleBackColor = true;
            this.OPL2RCheckbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // MPU401Checkbox
            // 
            this.MPU401Checkbox.Checked = true;
            this.MPU401Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MPU401Checkbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MPU401Checkbox.IsActive = false;
            this.MPU401Checkbox.Location = new System.Drawing.Point(122, 38);
            this.MPU401Checkbox.Name = "MPU401Checkbox";
            this.MPU401Checkbox.Size = new System.Drawing.Size(15, 14);
            this.MPU401Checkbox.TabIndex = 31;
            this.MPU401Checkbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MPU401Checkbox.UseVisualStyleBackColor = true;
            this.MPU401Checkbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // COM1Checkbox
            // 
            this.COM1Checkbox.Checked = true;
            this.COM1Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.COM1Checkbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.COM1Checkbox.IsActive = false;
            this.COM1Checkbox.Location = new System.Drawing.Point(139, 38);
            this.COM1Checkbox.Name = "COM1Checkbox";
            this.COM1Checkbox.Size = new System.Drawing.Size(15, 14);
            this.COM1Checkbox.TabIndex = 30;
            this.COM1Checkbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.COM1Checkbox.UseVisualStyleBackColor = true;
            this.COM1Checkbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // COM2Checkbox
            // 
            this.COM2Checkbox.Checked = true;
            this.COM2Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.COM2Checkbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.COM2Checkbox.IsActive = false;
            this.COM2Checkbox.Location = new System.Drawing.Point(156, 38);
            this.COM2Checkbox.Name = "COM2Checkbox";
            this.COM2Checkbox.Size = new System.Drawing.Size(15, 14);
            this.COM2Checkbox.TabIndex = 29;
            this.COM2Checkbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.COM2Checkbox.UseVisualStyleBackColor = true;
            this.COM2Checkbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // FDCCheckbox
            // 
            this.FDCCheckbox.Checked = true;
            this.FDCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FDCCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FDCCheckbox.IsActive = false;
            this.FDCCheckbox.Location = new System.Drawing.Point(105, 21);
            this.FDCCheckbox.Name = "FDCCheckbox";
            this.FDCCheckbox.Size = new System.Drawing.Size(15, 14);
            this.FDCCheckbox.TabIndex = 28;
            this.FDCCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FDCCheckbox.UseVisualStyleBackColor = true;
            // 
            // MouseCheckbox
            // 
            this.MouseCheckbox.Checked = true;
            this.MouseCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MouseCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MouseCheckbox.IsActive = false;
            this.MouseCheckbox.Location = new System.Drawing.Point(88, 21);
            this.MouseCheckbox.Name = "MouseCheckbox";
            this.MouseCheckbox.Size = new System.Drawing.Size(15, 14);
            this.MouseCheckbox.TabIndex = 27;
            this.MouseCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MouseCheckbox.UseVisualStyleBackColor = true;
            this.MouseCheckbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // RTCCheckbox
            // 
            this.RTCCheckbox.Checked = true;
            this.RTCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RTCCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RTCCheckbox.IsActive = false;
            this.RTCCheckbox.Location = new System.Drawing.Point(122, 21);
            this.RTCCheckbox.Name = "RTCCheckbox";
            this.RTCCheckbox.Size = new System.Drawing.Size(15, 14);
            this.RTCCheckbox.TabIndex = 26;
            this.RTCCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RTCCheckbox.UseVisualStyleBackColor = true;
            // 
            // TMR2Checkbox
            // 
            this.TMR2Checkbox.Checked = true;
            this.TMR2Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TMR2Checkbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TMR2Checkbox.IsActive = false;
            this.TMR2Checkbox.Location = new System.Drawing.Point(139, 21);
            this.TMR2Checkbox.Name = "TMR2Checkbox";
            this.TMR2Checkbox.Size = new System.Drawing.Size(15, 14);
            this.TMR2Checkbox.TabIndex = 25;
            this.TMR2Checkbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TMR2Checkbox.UseVisualStyleBackColor = true;
            // 
            // TMR1Checkbox
            // 
            this.TMR1Checkbox.Checked = true;
            this.TMR1Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TMR1Checkbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TMR1Checkbox.IsActive = false;
            this.TMR1Checkbox.Location = new System.Drawing.Point(156, 21);
            this.TMR1Checkbox.Name = "TMR1Checkbox";
            this.TMR1Checkbox.Size = new System.Drawing.Size(15, 14);
            this.TMR1Checkbox.TabIndex = 24;
            this.TMR1Checkbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TMR1Checkbox.UseVisualStyleBackColor = true;
            // 
            // TMR0Checkbox
            // 
            this.TMR0Checkbox.BackColor = System.Drawing.SystemColors.Control;
            this.TMR0Checkbox.Checked = true;
            this.TMR0Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TMR0Checkbox.FlatAppearance.BorderSize = 0;
            this.TMR0Checkbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TMR0Checkbox.IsActive = false;
            this.TMR0Checkbox.Location = new System.Drawing.Point(173, 21);
            this.TMR0Checkbox.Name = "TMR0Checkbox";
            this.TMR0Checkbox.Size = new System.Drawing.Size(15, 14);
            this.TMR0Checkbox.TabIndex = 23;
            this.TMR0Checkbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TMR0Checkbox.UseVisualStyleBackColor = false;
            this.TMR0Checkbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // SOLCheckbox
            // 
            this.SOLCheckbox.Checked = true;
            this.SOLCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SOLCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SOLCheckbox.IsActive = false;
            this.SOLCheckbox.Location = new System.Drawing.Point(190, 21);
            this.SOLCheckbox.Name = "SOLCheckbox";
            this.SOLCheckbox.Size = new System.Drawing.Size(15, 14);
            this.SOLCheckbox.TabIndex = 22;
            this.SOLCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SOLCheckbox.UseVisualStyleBackColor = true;
            this.SOLCheckbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // Reg2Label
            // 
            this.Reg2Label.AutoSize = true;
            this.Reg2Label.Location = new System.Drawing.Point(3, 57);
            this.Reg2Label.Name = "Reg2Label";
            this.Reg2Label.Size = new System.Drawing.Size(58, 13);
            this.Reg2Label.TabIndex = 21;
            this.Reg2Label.Text = "IRQ Reg 2";
            // 
            // Reg1Label
            // 
            this.Reg1Label.AutoSize = true;
            this.Reg1Label.Location = new System.Drawing.Point(3, 39);
            this.Reg1Label.Name = "Reg1Label";
            this.Reg1Label.Size = new System.Drawing.Size(58, 13);
            this.Reg1Label.TabIndex = 20;
            this.Reg1Label.Text = "IRQ Reg 1";
            // 
            // Reg0Label
            // 
            this.Reg0Label.AutoSize = true;
            this.Reg0Label.Location = new System.Drawing.Point(3, 22);
            this.Reg0Label.Name = "Reg0Label";
            this.Reg0Label.Size = new System.Drawing.Size(58, 13);
            this.Reg0Label.TabIndex = 19;
            this.Reg0Label.Text = "IRQ Reg 0";
            // 
            // KeyboardCheckBox
            // 
            this.KeyboardCheckBox.Checked = true;
            this.KeyboardCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeyboardCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KeyboardCheckBox.IsActive = false;
            this.KeyboardCheckBox.Location = new System.Drawing.Point(206, 39);
            this.KeyboardCheckBox.Name = "KeyboardCheckBox";
            this.KeyboardCheckBox.Size = new System.Drawing.Size(15, 14);
            this.KeyboardCheckBox.TabIndex = 18;
            this.KeyboardCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.KeyboardCheckBox.UseVisualStyleBackColor = true;
            this.KeyboardCheckBox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // SOFCheckbox
            // 
            this.SOFCheckbox.Checked = true;
            this.SOFCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SOFCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SOFCheckbox.IsActive = false;
            this.SOFCheckbox.Location = new System.Drawing.Point(206, 21);
            this.SOFCheckbox.Name = "SOFCheckbox";
            this.SOFCheckbox.Size = new System.Drawing.Size(15, 14);
            this.SOFCheckbox.TabIndex = 17;
            this.SOFCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SOFCheckbox.UseVisualStyleBackColor = true;
            this.SOFCheckbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // BreakOnIRQCheckBox
            // 
            this.BreakOnIRQCheckBox.AutoSize = true;
            this.BreakOnIRQCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.BreakOnIRQCheckBox.Checked = true;
            this.BreakOnIRQCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BreakOnIRQCheckBox.Location = new System.Drawing.Point(3, 3);
            this.BreakOnIRQCheckBox.Name = "BreakOnIRQCheckBox";
            this.BreakOnIRQCheckBox.Size = new System.Drawing.Size(91, 17);
            this.BreakOnIRQCheckBox.TabIndex = 16;
            this.BreakOnIRQCheckBox.Text = "Break on IRQ";
            this.BreakOnIRQCheckBox.UseVisualStyleBackColor = false;
            this.BreakOnIRQCheckBox.CheckedChanged += new System.EventHandler(this.BreakOnIRQCheckBox_CheckedChanged);
            // 
            // LabelOverlayButton
            // 
            this.LabelOverlayButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LabelOverlayButton.FlatAppearance.BorderSize = 0;
            this.LabelOverlayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.LabelOverlayButton.Location = new System.Drawing.Point(198, 200);
            this.LabelOverlayButton.Margin = new System.Windows.Forms.Padding(2);
            this.LabelOverlayButton.Name = "LabelOverlayButton";
            this.LabelOverlayButton.Size = new System.Drawing.Size(18, 18);
            this.LabelOverlayButton.TabIndex = 13;
            this.LabelOverlayButton.TabStop = false;
            this.LabelOverlayButton.Text = "L";
            this.LabelOverlayButton.UseVisualStyleBackColor = false;
            this.LabelOverlayButton.Visible = false;
            this.LabelOverlayButton.Click += new System.EventHandler(this.LabelOverlayButton_Click);
            // 
            // registerDisplay1
            // 
            this.registerDisplay1.CPU = null;
            this.registerDisplay1.Location = new System.Drawing.Point(2, 50);
            this.registerDisplay1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new System.Drawing.Size(366, 49);
            this.registerDisplay1.TabIndex = 0;
            this.registerDisplay1.MouseEnter += new System.EventHandler(this.DebugPanel_Leave);
            // 
            // CPUWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 515);
            this.Controls.Add(this.LabelOverlayButton);
            this.Controls.Add(this.irqPanel);
            this.Controls.Add(this.HeaderTextbox);
            this.Controls.Add(this.StepOverOverlayButton);
            this.Controls.Add(this.InspectOverlayButton);
            this.Controls.Add(this.DeleteBPOverlayButton);
            this.Controls.Add(this.AddBPOverlayButton);
            this.Controls.Add(this.DebugPanel);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.SecondPanel);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.lastLine);
            this.Controls.Add(this.stackText);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(1280, 0);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1023, 554);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(774, 554);
            this.Name = "CPUWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CPU Window";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPUWindow_FormClosed);
            this.Load += new System.EventHandler(this.CPUWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPUWindow_KeyDown);
            this.HeaderPanel.ResumeLayout(false);
            this.SecondPanel.ResumeLayout(false);
            this.SecondPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DebugPanel)).EndInit();
            this.irqPanel.ResumeLayout(false);
            this.irqPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public RegisterDisplay registerDisplay1;
        private global::System.Windows.Forms.Panel HeaderPanel;
        private global::System.Windows.Forms.TextBox locationInput;
        private global::System.Windows.Forms.Button JumpButton;
        private global::System.Windows.Forms.Button RunButton;
        private global::System.Windows.Forms.Button StepButton;
        private global::System.Windows.Forms.TextBox lastLine;
        private global::System.Windows.Forms.TextBox stackText;
        private global::System.Windows.Forms.Label locationLabel;
        private global::System.Windows.Forms.Timer UpdateTraceTimer;
        private global::System.Windows.Forms.ComboBox BPCombo;
        private global::System.Windows.Forms.Button AddBPButton;
        private global::System.Windows.Forms.Button DeleteBPButton;
        private global::System.Windows.Forms.Panel SecondPanel;
        private global::System.Windows.Forms.Button ClearTraceButton;
        private System.Windows.Forms.ToolTip Tooltip;
        private System.Windows.Forms.PictureBox DebugPanel;
        private System.Windows.Forms.Button AddBPOverlayButton;
        private System.Windows.Forms.Button DeleteBPOverlayButton;
        private System.Windows.Forms.Button InspectOverlayButton;
        private System.Windows.Forms.Button StepOverOverlayButton;
        private System.Windows.Forms.Label HeaderTextbox;
        private System.Windows.Forms.Label BPLabel;
        private System.Windows.Forms.Panel irqPanel;
        private System.Windows.Forms.CheckBox BreakOnIRQCheckBox;
        private System.Windows.Forms.Label Reg2Label;
        private System.Windows.Forms.Label Reg1Label;
        private System.Windows.Forms.Label Reg0Label;
        private System.Windows.Forms.Button StepOverButton;
        private System.Windows.Forms.Button LabelOverlayButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button WatchButton;
        private ColorCheckBox SOLCheckbox;
        private ColorCheckBox MPU401Checkbox;
        private ColorCheckBox COM1Checkbox;
        private ColorCheckBox COM2Checkbox;
        private ColorCheckBox FDCCheckbox;
        private ColorCheckBox MouseCheckbox;
        private ColorCheckBox RTCCheckbox;
        private ColorCheckBox TMR2Checkbox;
        private ColorCheckBox TMR1Checkbox;
        private ColorCheckBox TMR0Checkbox;
        private ColorCheckBox OPL2LCheckbox;
        private ColorCheckBox OPL2RCheckbox;
        private ColorCheckBox SDCardCheckBox;
        private ColorCheckBox SOFCheckbox;
        private ColorCheckBox KeyboardCheckBox;
    }
}