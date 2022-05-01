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
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(1118, 44);
            this.HeaderPanel.TabIndex = 2;
            // 
            // WatchButton
            // 
            this.WatchButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.WatchButton.Location = new System.Drawing.Point(559, 0);
            this.WatchButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.WatchButton.Name = "WatchButton";
            this.WatchButton.Size = new System.Drawing.Size(138, 44);
            this.WatchButton.TabIndex = 5;
            this.WatchButton.Text = "Watch";
            this.WatchButton.UseVisualStyleBackColor = true;
            this.WatchButton.Click += new System.EventHandler(this.WatchButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ResetButton.Location = new System.Drawing.Point(431, 0);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(128, 44);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // StepOverButton
            // 
            this.StepOverButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepOverButton.Location = new System.Drawing.Point(266, 0);
            this.StepOverButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.StepOverButton.Name = "StepOverButton";
            this.StepOverButton.Size = new System.Drawing.Size(165, 44);
            this.StepOverButton.TabIndex = 3;
            this.StepOverButton.Text = "Step Over (F7)";
            this.StepOverButton.UseVisualStyleBackColor = true;
            this.StepOverButton.Click += new System.EventHandler(this.StepOverButton_Click);
            // 
            // BPLabel
            // 
            this.BPLabel.Location = new System.Drawing.Point(695, 4);
            this.BPLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.BPLabel.Size = new System.Drawing.Size(106, 31);
            this.BPLabel.TabIndex = 9;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BPCombo
            // 
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new System.Drawing.Point(805, 4);
            this.BPCombo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new System.Drawing.Size(219, 32);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Location = new System.Drawing.Point(1027, 0);
            this.AddBPButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.AddBPButton.Name = "AddBPButton";
            this.AddBPButton.Size = new System.Drawing.Size(44, 44);
            this.AddBPButton.TabIndex = 7;
            this.AddBPButton.Text = "+";
            this.AddBPButton.UseVisualStyleBackColor = true;
            this.AddBPButton.Click += new System.EventHandler(this.AddBPButton_Click);
            // 
            // DeleteBPButton
            // 
            this.DeleteBPButton.Location = new System.Drawing.Point(1071, 0);
            this.DeleteBPButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new System.Drawing.Size(44, 44);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new System.EventHandler(this.DeleteBPButton_Click);
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new System.Drawing.Point(138, 0);
            this.StepButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(128, 44);
            this.StepButton.TabIndex = 2;
            this.StepButton.Text = "Step (F6)";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunButton.Location = new System.Drawing.Point(0, 0);
            this.RunButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(138, 44);
            this.RunButton.TabIndex = 1;
            this.RunButton.Tag = "0";
            this.RunButton.Text = "Run (F5)";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationLabel.Location = new System.Drawing.Point(266, 0);
            this.locationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.locationLabel.Size = new System.Drawing.Size(117, 44);
            this.locationLabel.TabIndex = 9;
            this.locationLabel.Text = "Location $";
            // 
            // locationInput
            // 
            this.locationInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationInput.Location = new System.Drawing.Point(383, 0);
            this.locationInput.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new System.Drawing.Size(114, 27);
            this.locationInput.TabIndex = 10;
            this.locationInput.Text = "00:0000";
            this.locationInput.Validated += new System.EventHandler(this.LocationInput_Validated);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.JumpButton.Location = new System.Drawing.Point(138, 0);
            this.JumpButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(128, 44);
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
            this.lastLine.Location = new System.Drawing.Point(0, 910);
            this.lastLine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lastLine.Name = "lastLine";
            this.lastLine.ReadOnly = true;
            this.lastLine.Size = new System.Drawing.Size(1106, 27);
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
            this.stackText.Location = new System.Drawing.Point(1118, 0);
            this.stackText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.ReadOnly = true;
            this.stackText.Size = new System.Drawing.Size(272, 940);
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
            this.SecondPanel.Location = new System.Drawing.Point(0, 46);
            this.SecondPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SecondPanel.Name = "SecondPanel";
            this.SecondPanel.Size = new System.Drawing.Size(671, 44);
            this.SecondPanel.TabIndex = 5;
            // 
            // ClearTraceButton
            // 
            this.ClearTraceButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ClearTraceButton.Location = new System.Drawing.Point(0, 0);
            this.ClearTraceButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ClearTraceButton.Name = "ClearTraceButton";
            this.ClearTraceButton.Size = new System.Drawing.Size(138, 44);
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
            this.DebugPanel.Location = new System.Drawing.Point(0, 233);
            this.DebugPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DebugPanel.Name = "DebugPanel";
            this.DebugPanel.Size = new System.Drawing.Size(1108, 676);
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
            this.AddBPOverlayButton.Location = new System.Drawing.Point(182, 369);
            this.AddBPOverlayButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AddBPOverlayButton.Name = "AddBPOverlayButton";
            this.AddBPOverlayButton.Size = new System.Drawing.Size(33, 33);
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
            this.DeleteBPOverlayButton.Location = new System.Drawing.Point(216, 369);
            this.DeleteBPOverlayButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DeleteBPOverlayButton.Name = "DeleteBPOverlayButton";
            this.DeleteBPOverlayButton.Size = new System.Drawing.Size(33, 33);
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
            this.InspectOverlayButton.Location = new System.Drawing.Point(251, 369);
            this.InspectOverlayButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.InspectOverlayButton.Name = "InspectOverlayButton";
            this.InspectOverlayButton.Size = new System.Drawing.Size(70, 33);
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
            this.StepOverOverlayButton.Location = new System.Drawing.Point(323, 369);
            this.StepOverOverlayButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.StepOverOverlayButton.Name = "StepOverOverlayButton";
            this.StepOverOverlayButton.Size = new System.Drawing.Size(39, 33);
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
            this.HeaderTextbox.Location = new System.Drawing.Point(4, 188);
            this.HeaderTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderTextbox.Name = "HeaderTextbox";
            this.HeaderTextbox.Padding = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.HeaderTextbox.Size = new System.Drawing.Size(1106, 41);
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
            this.irqPanel.Location = new System.Drawing.Point(682, 46);
            this.irqPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.irqPanel.Name = "irqPanel";
            this.irqPanel.Size = new System.Drawing.Size(422, 137);
            this.irqPanel.TabIndex = 12;
            // 
            // SDCardCheckBox
            // 
            this.SDCardCheckBox.Checked = true;
            this.SDCardCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SDCardCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SDCardCheckBox.IsActive = false;
            this.SDCardCheckBox.Location = new System.Drawing.Point(161, 70);
            this.SDCardCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SDCardCheckBox.Name = "SDCardCheckBox";
            this.SDCardCheckBox.Size = new System.Drawing.Size(28, 26);
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
            this.OPL2LCheckbox.Location = new System.Drawing.Point(347, 103);
            this.OPL2LCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.OPL2LCheckbox.Name = "OPL2LCheckbox";
            this.OPL2LCheckbox.Size = new System.Drawing.Size(28, 26);
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
            this.OPL2RCheckbox.Location = new System.Drawing.Point(378, 103);
            this.OPL2RCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.OPL2RCheckbox.Name = "OPL2RCheckbox";
            this.OPL2RCheckbox.Size = new System.Drawing.Size(28, 26);
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
            this.MPU401Checkbox.Location = new System.Drawing.Point(224, 70);
            this.MPU401Checkbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MPU401Checkbox.Name = "MPU401Checkbox";
            this.MPU401Checkbox.Size = new System.Drawing.Size(28, 26);
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
            this.COM1Checkbox.Location = new System.Drawing.Point(255, 70);
            this.COM1Checkbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.COM1Checkbox.Name = "COM1Checkbox";
            this.COM1Checkbox.Size = new System.Drawing.Size(28, 26);
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
            this.COM2Checkbox.Location = new System.Drawing.Point(286, 70);
            this.COM2Checkbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.COM2Checkbox.Name = "COM2Checkbox";
            this.COM2Checkbox.Size = new System.Drawing.Size(28, 26);
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
            this.FDCCheckbox.Location = new System.Drawing.Point(193, 39);
            this.FDCCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.FDCCheckbox.Name = "FDCCheckbox";
            this.FDCCheckbox.Size = new System.Drawing.Size(28, 26);
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
            this.MouseCheckbox.Location = new System.Drawing.Point(161, 39);
            this.MouseCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MouseCheckbox.Name = "MouseCheckbox";
            this.MouseCheckbox.Size = new System.Drawing.Size(28, 26);
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
            this.RTCCheckbox.Location = new System.Drawing.Point(224, 39);
            this.RTCCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RTCCheckbox.Name = "RTCCheckbox";
            this.RTCCheckbox.Size = new System.Drawing.Size(28, 26);
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
            this.TMR2Checkbox.Location = new System.Drawing.Point(255, 39);
            this.TMR2Checkbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TMR2Checkbox.Name = "TMR2Checkbox";
            this.TMR2Checkbox.Size = new System.Drawing.Size(28, 26);
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
            this.TMR1Checkbox.Location = new System.Drawing.Point(286, 39);
            this.TMR1Checkbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TMR1Checkbox.Name = "TMR1Checkbox";
            this.TMR1Checkbox.Size = new System.Drawing.Size(28, 26);
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
            this.TMR0Checkbox.Location = new System.Drawing.Point(317, 39);
            this.TMR0Checkbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TMR0Checkbox.Name = "TMR0Checkbox";
            this.TMR0Checkbox.Size = new System.Drawing.Size(28, 26);
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
            this.SOLCheckbox.Location = new System.Drawing.Point(348, 39);
            this.SOLCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SOLCheckbox.Name = "SOLCheckbox";
            this.SOLCheckbox.Size = new System.Drawing.Size(28, 26);
            this.SOLCheckbox.TabIndex = 22;
            this.SOLCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SOLCheckbox.UseVisualStyleBackColor = true;
            this.SOLCheckbox.CheckedChanged += new System.EventHandler(this.IRQCheckbox_CheckedChanged);
            // 
            // Reg2Label
            // 
            this.Reg2Label.AutoSize = true;
            this.Reg2Label.Location = new System.Drawing.Point(6, 105);
            this.Reg2Label.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Reg2Label.Name = "Reg2Label";
            this.Reg2Label.Size = new System.Drawing.Size(102, 25);
            this.Reg2Label.TabIndex = 21;
            this.Reg2Label.Text = "IRQ Reg 2";
            // 
            // Reg1Label
            // 
            this.Reg1Label.AutoSize = true;
            this.Reg1Label.Location = new System.Drawing.Point(6, 72);
            this.Reg1Label.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Reg1Label.Name = "Reg1Label";
            this.Reg1Label.Size = new System.Drawing.Size(102, 25);
            this.Reg1Label.TabIndex = 20;
            this.Reg1Label.Text = "IRQ Reg 1";
            // 
            // Reg0Label
            // 
            this.Reg0Label.AutoSize = true;
            this.Reg0Label.Location = new System.Drawing.Point(6, 41);
            this.Reg0Label.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Reg0Label.Name = "Reg0Label";
            this.Reg0Label.Size = new System.Drawing.Size(102, 25);
            this.Reg0Label.TabIndex = 19;
            this.Reg0Label.Text = "IRQ Reg 0";
            // 
            // KeyboardCheckBox
            // 
            this.KeyboardCheckBox.Checked = true;
            this.KeyboardCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeyboardCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KeyboardCheckBox.IsActive = false;
            this.KeyboardCheckBox.Location = new System.Drawing.Point(378, 72);
            this.KeyboardCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.KeyboardCheckBox.Name = "KeyboardCheckBox";
            this.KeyboardCheckBox.Size = new System.Drawing.Size(28, 26);
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
            this.SOFCheckbox.Location = new System.Drawing.Point(378, 39);
            this.SOFCheckbox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SOFCheckbox.Name = "SOFCheckbox";
            this.SOFCheckbox.Size = new System.Drawing.Size(28, 26);
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
            this.BreakOnIRQCheckBox.Location = new System.Drawing.Point(6, 6);
            this.BreakOnIRQCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BreakOnIRQCheckBox.Name = "BreakOnIRQCheckBox";
            this.BreakOnIRQCheckBox.Size = new System.Drawing.Size(151, 29);
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
            this.LabelOverlayButton.Location = new System.Drawing.Point(363, 369);
            this.LabelOverlayButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelOverlayButton.Name = "LabelOverlayButton";
            this.LabelOverlayButton.Size = new System.Drawing.Size(33, 33);
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
            this.registerDisplay1.Location = new System.Drawing.Point(4, 92);
            this.registerDisplay1.Margin = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new System.Drawing.Size(671, 90);
            this.registerDisplay1.TabIndex = 0;
            this.registerDisplay1.MouseEnter += new System.EventHandler(this.DebugPanel_Leave);
            // 
            // CPUWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 940);
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
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1862, 987);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1404, 987);
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