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
            this.StepOver = new System.Windows.Forms.Button();
            this.BPLabel = new System.Windows.Forms.Label();
            this.stepsInput = new System.Windows.Forms.TextBox();
            this.BPCombo = new System.Windows.Forms.ComboBox();
            this.AddBPButton = new System.Windows.Forms.Button();
            this.DeleteBPButton = new System.Windows.Forms.Button();
            this.stepsLabel = new System.Windows.Forms.Label();
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
            this.PlusButton = new System.Windows.Forms.Button();
            this.MinusButton = new System.Windows.Forms.Button();
            this.InspectButton = new System.Windows.Forms.Button();
            this.StepOverButton = new System.Windows.Forms.Button();
            this.HeaderTextbox = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SDCardCheckBox = new System.Windows.Forms.CheckBox();
            this.OPL2LCheckbox = new System.Windows.Forms.CheckBox();
            this.OPL2RCheckbox = new System.Windows.Forms.CheckBox();
            this.MPU401Checkbox = new System.Windows.Forms.CheckBox();
            this.COM1Checkbox = new System.Windows.Forms.CheckBox();
            this.COM2Checkbox = new System.Windows.Forms.CheckBox();
            this.FDCCheckbox = new System.Windows.Forms.CheckBox();
            this.MouseCheckbox = new System.Windows.Forms.CheckBox();
            this.RTCCheckbox = new System.Windows.Forms.CheckBox();
            this.TMR2Checkbox = new System.Windows.Forms.CheckBox();
            this.TMR1Checkbox = new System.Windows.Forms.CheckBox();
            this.TMR0Checkbox = new System.Windows.Forms.CheckBox();
            this.SOLCheckbox = new System.Windows.Forms.CheckBox();
            this.Reg2Label = new System.Windows.Forms.Label();
            this.Reg1Label = new System.Windows.Forms.Label();
            this.Reg0Label = new System.Windows.Forms.Label();
            this.KeyboardCheckBox = new System.Windows.Forms.CheckBox();
            this.SOFCheckbox = new System.Windows.Forms.CheckBox();
            this.BreakOnIRQCheckBox = new System.Windows.Forms.CheckBox();
            this.registerDisplay1 = new FoenixIDE.RegisterDisplay();
            this.HeaderPanel.SuspendLayout();
            this.SecondPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DebugPanel)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Controls.Add(this.StepOver);
            this.HeaderPanel.Controls.Add(this.BPLabel);
            this.HeaderPanel.Controls.Add(this.stepsInput);
            this.HeaderPanel.Controls.Add(this.BPCombo);
            this.HeaderPanel.Controls.Add(this.AddBPButton);
            this.HeaderPanel.Controls.Add(this.DeleteBPButton);
            this.HeaderPanel.Controls.Add(this.stepsLabel);
            this.HeaderPanel.Controls.Add(this.StepButton);
            this.HeaderPanel.Controls.Add(this.RunButton);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(608, 24);
            this.HeaderPanel.TabIndex = 2;
            // 
            // StepOver
            // 
            this.StepOver.Location = new System.Drawing.Point(149, 0);
            this.StepOver.Name = "StepOver";
            this.StepOver.Size = new System.Drawing.Size(87, 24);
            this.StepOver.TabIndex = 10;
            this.StepOver.Text = "Step Over (F7)";
            this.StepOver.UseVisualStyleBackColor = true;
            this.StepOver.Click += new System.EventHandler(this.StepOver_Click);
            // 
            // BPLabel
            // 
            this.BPLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BPLabel.Location = new System.Drawing.Point(375, 1);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.BPLabel.Size = new System.Drawing.Size(58, 17);
            this.BPLabel.TabIndex = 9;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // stepsInput
            // 
            this.stepsInput.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.stepsInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsInput.Location = new System.Drawing.Point(333, 1);
            this.stepsInput.Name = "stepsInput";
            this.stepsInput.Size = new System.Drawing.Size(33, 23);
            this.stepsInput.TabIndex = 3;
            this.stepsInput.Text = "1";
            this.stepsInput.Enter += new System.EventHandler(this.StepsInput_Enter);
            // 
            // BPCombo
            // 
            this.BPCombo.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new System.Drawing.Point(439, 0);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new System.Drawing.Size(121, 21);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Dock = System.Windows.Forms.DockStyle.Right;
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
            this.DeleteBPButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.DeleteBPButton.Location = new System.Drawing.Point(584, 0);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new System.EventHandler(this.DeleteBPButton_Click);
            // 
            // stepsLabel
            // 
            this.stepsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.stepsLabel.Location = new System.Drawing.Point(248, 0);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Padding = new System.Windows.Forms.Padding(10, 4, 0, 0);
            this.stepsLabel.Size = new System.Drawing.Size(79, 24);
            this.stepsLabel.TabIndex = 4;
            this.stepsLabel.Text = "Steps (dec)";
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new System.Drawing.Point(77, 0);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(72, 24);
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
            this.RunButton.Size = new System.Drawing.Size(77, 24);
            this.RunButton.TabIndex = 1;
            this.RunButton.Tag = "0";
            this.RunButton.Text = "Run (F5)";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationLabel.Location = new System.Drawing.Point(160, 0);
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
            this.locationInput.Location = new System.Drawing.Point(224, 0);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new System.Drawing.Size(64, 23);
            this.locationInput.TabIndex = 10;
            this.locationInput.Text = "00:0000";
            this.locationInput.Validated += new System.EventHandler(this.LocationInput_Validated);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.JumpButton.Location = new System.Drawing.Point(96, 0);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(64, 24);
            this.JumpButton.TabIndex = 11;
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
            this.stackText.Size = new System.Drawing.Size(150, 517);
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
            this.SecondPanel.Size = new System.Drawing.Size(316, 24);
            this.SecondPanel.TabIndex = 5;
            // 
            // ClearTraceButton
            // 
            this.ClearTraceButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ClearTraceButton.Location = new System.Drawing.Point(0, 0);
            this.ClearTraceButton.Name = "ClearTraceButton";
            this.ClearTraceButton.Size = new System.Drawing.Size(96, 24);
            this.ClearTraceButton.TabIndex = 12;
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
            this.DebugPanel.Location = new System.Drawing.Point(0, 130);
            this.DebugPanel.Margin = new System.Windows.Forms.Padding(2);
            this.DebugPanel.Name = "DebugPanel";
            this.DebugPanel.Size = new System.Drawing.Size(605, 363);
            this.DebugPanel.TabIndex = 6;
            this.DebugPanel.TabStop = false;
            this.DebugPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DebugPanel_Paint);
            this.DebugPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DebugPanel_MouseMove);
            // 
            // PlusButton
            // 
            this.PlusButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PlusButton.FlatAppearance.BorderSize = 0;
            this.PlusButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.PlusButton.Location = new System.Drawing.Point(99, 200);
            this.PlusButton.Margin = new System.Windows.Forms.Padding(2);
            this.PlusButton.Name = "PlusButton";
            this.PlusButton.Size = new System.Drawing.Size(18, 18);
            this.PlusButton.TabIndex = 7;
            this.PlusButton.TabStop = false;
            this.PlusButton.Text = "+";
            this.PlusButton.UseVisualStyleBackColor = false;
            this.PlusButton.Visible = false;
            this.PlusButton.Click += new System.EventHandler(this.PlusButton_Click);
            // 
            // MinusButton
            // 
            this.MinusButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.MinusButton.FlatAppearance.BorderSize = 0;
            this.MinusButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.MinusButton.Location = new System.Drawing.Point(118, 200);
            this.MinusButton.Margin = new System.Windows.Forms.Padding(2);
            this.MinusButton.Name = "MinusButton";
            this.MinusButton.Size = new System.Drawing.Size(18, 18);
            this.MinusButton.TabIndex = 8;
            this.MinusButton.TabStop = false;
            this.MinusButton.Text = "-";
            this.MinusButton.UseVisualStyleBackColor = false;
            this.MinusButton.Visible = false;
            this.MinusButton.Click += new System.EventHandler(this.MinusButton_Click);
            // 
            // InspectButton
            // 
            this.InspectButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.InspectButton.FlatAppearance.BorderSize = 0;
            this.InspectButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.InspectButton.Location = new System.Drawing.Point(137, 200);
            this.InspectButton.Margin = new System.Windows.Forms.Padding(1);
            this.InspectButton.Name = "InspectButton";
            this.InspectButton.Size = new System.Drawing.Size(38, 18);
            this.InspectButton.TabIndex = 9;
            this.InspectButton.TabStop = false;
            this.InspectButton.Text = "Mem";
            this.InspectButton.UseVisualStyleBackColor = false;
            this.InspectButton.Visible = false;
            this.InspectButton.Click += new System.EventHandler(this.InspectButton_Click);
            // 
            // StepOverButton
            // 
            this.StepOverButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.StepOverButton.FlatAppearance.BorderSize = 0;
            this.StepOverButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StepOverButton.Font = new System.Drawing.Font("Arial Narrow", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StepOverButton.Location = new System.Drawing.Point(176, 200);
            this.StepOverButton.Margin = new System.Windows.Forms.Padding(1);
            this.StepOverButton.Name = "StepOverButton";
            this.StepOverButton.Size = new System.Drawing.Size(21, 18);
            this.StepOverButton.TabIndex = 10;
            this.StepOverButton.TabStop = false;
            this.StepOverButton.Text = "►";
            this.StepOverButton.UseVisualStyleBackColor = false;
            this.StepOverButton.Visible = false;
            this.StepOverButton.Click += new System.EventHandler(this.StepOverButton_Click);
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
            this.HeaderTextbox.Size = new System.Drawing.Size(603, 26);
            this.HeaderTextbox.TabIndex = 11;
            this.HeaderTextbox.UseCompatibleTextRendering = true;
            this.HeaderTextbox.UseMnemonic = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SDCardCheckBox);
            this.panel1.Controls.Add(this.OPL2LCheckbox);
            this.panel1.Controls.Add(this.OPL2RCheckbox);
            this.panel1.Controls.Add(this.MPU401Checkbox);
            this.panel1.Controls.Add(this.COM1Checkbox);
            this.panel1.Controls.Add(this.COM2Checkbox);
            this.panel1.Controls.Add(this.FDCCheckbox);
            this.panel1.Controls.Add(this.MouseCheckbox);
            this.panel1.Controls.Add(this.RTCCheckbox);
            this.panel1.Controls.Add(this.TMR2Checkbox);
            this.panel1.Controls.Add(this.TMR1Checkbox);
            this.panel1.Controls.Add(this.TMR0Checkbox);
            this.panel1.Controls.Add(this.SOLCheckbox);
            this.panel1.Controls.Add(this.Reg2Label);
            this.panel1.Controls.Add(this.Reg1Label);
            this.panel1.Controls.Add(this.Reg0Label);
            this.panel1.Controls.Add(this.KeyboardCheckBox);
            this.panel1.Controls.Add(this.SOFCheckbox);
            this.panel1.Controls.Add(this.BreakOnIRQCheckBox);
            this.panel1.Location = new System.Drawing.Point(372, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 74);
            this.panel1.TabIndex = 12;
            // 
            // SDCardCheckBox
            // 
            this.SDCardCheckBox.Checked = true;
            this.SDCardCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SDCardCheckBox.Location = new System.Drawing.Point(88, 38);
            this.SDCardCheckBox.Name = "SDCardCheckBox";
            this.SDCardCheckBox.Size = new System.Drawing.Size(15, 14);
            this.SDCardCheckBox.TabIndex = 34;
            this.SDCardCheckBox.UseVisualStyleBackColor = true;
            // 
            // OPL2LCheckbox
            // 
            this.OPL2LCheckbox.Checked = true;
            this.OPL2LCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OPL2LCheckbox.Location = new System.Drawing.Point(189, 56);
            this.OPL2LCheckbox.Name = "OPL2LCheckbox";
            this.OPL2LCheckbox.Size = new System.Drawing.Size(15, 14);
            this.OPL2LCheckbox.TabIndex = 33;
            this.OPL2LCheckbox.UseVisualStyleBackColor = true;
            // 
            // OPL2RCheckbox
            // 
            this.OPL2RCheckbox.Checked = true;
            this.OPL2RCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OPL2RCheckbox.Location = new System.Drawing.Point(206, 56);
            this.OPL2RCheckbox.Name = "OPL2RCheckbox";
            this.OPL2RCheckbox.Size = new System.Drawing.Size(15, 14);
            this.OPL2RCheckbox.TabIndex = 32;
            this.OPL2RCheckbox.UseVisualStyleBackColor = true;
            // 
            // MPU401Checkbox
            // 
            this.MPU401Checkbox.Checked = true;
            this.MPU401Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MPU401Checkbox.Location = new System.Drawing.Point(122, 38);
            this.MPU401Checkbox.Name = "MPU401Checkbox";
            this.MPU401Checkbox.Size = new System.Drawing.Size(15, 14);
            this.MPU401Checkbox.TabIndex = 31;
            this.MPU401Checkbox.UseVisualStyleBackColor = true;
            // 
            // COM1Checkbox
            // 
            this.COM1Checkbox.Checked = true;
            this.COM1Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.COM1Checkbox.Location = new System.Drawing.Point(139, 38);
            this.COM1Checkbox.Name = "COM1Checkbox";
            this.COM1Checkbox.Size = new System.Drawing.Size(15, 14);
            this.COM1Checkbox.TabIndex = 30;
            this.COM1Checkbox.UseVisualStyleBackColor = true;
            // 
            // COM2Checkbox
            // 
            this.COM2Checkbox.Checked = true;
            this.COM2Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.COM2Checkbox.Location = new System.Drawing.Point(156, 38);
            this.COM2Checkbox.Name = "COM2Checkbox";
            this.COM2Checkbox.Size = new System.Drawing.Size(15, 14);
            this.COM2Checkbox.TabIndex = 29;
            this.COM2Checkbox.UseVisualStyleBackColor = true;
            // 
            // FDCCheckbox
            // 
            this.FDCCheckbox.Checked = true;
            this.FDCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FDCCheckbox.Location = new System.Drawing.Point(105, 21);
            this.FDCCheckbox.Name = "FDCCheckbox";
            this.FDCCheckbox.Size = new System.Drawing.Size(15, 14);
            this.FDCCheckbox.TabIndex = 28;
            this.FDCCheckbox.UseVisualStyleBackColor = true;
            // 
            // MouseCheckbox
            // 
            this.MouseCheckbox.Checked = true;
            this.MouseCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MouseCheckbox.Location = new System.Drawing.Point(88, 21);
            this.MouseCheckbox.Name = "MouseCheckbox";
            this.MouseCheckbox.Size = new System.Drawing.Size(15, 14);
            this.MouseCheckbox.TabIndex = 27;
            this.MouseCheckbox.UseVisualStyleBackColor = true;
            // 
            // RTCCheckbox
            // 
            this.RTCCheckbox.Checked = true;
            this.RTCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RTCCheckbox.Location = new System.Drawing.Point(122, 21);
            this.RTCCheckbox.Name = "RTCCheckbox";
            this.RTCCheckbox.Size = new System.Drawing.Size(15, 14);
            this.RTCCheckbox.TabIndex = 26;
            this.RTCCheckbox.UseVisualStyleBackColor = true;
            // 
            // TMR2Checkbox
            // 
            this.TMR2Checkbox.Checked = true;
            this.TMR2Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TMR2Checkbox.Location = new System.Drawing.Point(139, 21);
            this.TMR2Checkbox.Name = "TMR2Checkbox";
            this.TMR2Checkbox.Size = new System.Drawing.Size(15, 14);
            this.TMR2Checkbox.TabIndex = 25;
            this.TMR2Checkbox.UseVisualStyleBackColor = true;
            // 
            // TMR1Checkbox
            // 
            this.TMR1Checkbox.Checked = true;
            this.TMR1Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TMR1Checkbox.Location = new System.Drawing.Point(156, 21);
            this.TMR1Checkbox.Name = "TMR1Checkbox";
            this.TMR1Checkbox.Size = new System.Drawing.Size(15, 14);
            this.TMR1Checkbox.TabIndex = 24;
            this.TMR1Checkbox.UseVisualStyleBackColor = true;
            // 
            // TMR0Checkbox
            // 
            this.TMR0Checkbox.Checked = true;
            this.TMR0Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TMR0Checkbox.Location = new System.Drawing.Point(173, 21);
            this.TMR0Checkbox.Name = "TMR0Checkbox";
            this.TMR0Checkbox.Size = new System.Drawing.Size(15, 14);
            this.TMR0Checkbox.TabIndex = 23;
            this.TMR0Checkbox.UseVisualStyleBackColor = true;
            // 
            // SOLCheckbox
            // 
            this.SOLCheckbox.Checked = true;
            this.SOLCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SOLCheckbox.Location = new System.Drawing.Point(190, 21);
            this.SOLCheckbox.Name = "SOLCheckbox";
            this.SOLCheckbox.Size = new System.Drawing.Size(15, 14);
            this.SOLCheckbox.TabIndex = 22;
            this.SOLCheckbox.UseVisualStyleBackColor = true;
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
            this.KeyboardCheckBox.Location = new System.Drawing.Point(206, 39);
            this.KeyboardCheckBox.Name = "KeyboardCheckBox";
            this.KeyboardCheckBox.Size = new System.Drawing.Size(15, 14);
            this.KeyboardCheckBox.TabIndex = 18;
            this.KeyboardCheckBox.UseVisualStyleBackColor = true;
            // 
            // SOFCheckbox
            // 
            this.SOFCheckbox.Checked = true;
            this.SOFCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SOFCheckbox.Location = new System.Drawing.Point(206, 21);
            this.SOFCheckbox.Name = "SOFCheckbox";
            this.SOFCheckbox.Size = new System.Drawing.Size(15, 14);
            this.SOFCheckbox.TabIndex = 17;
            this.SOFCheckbox.UseVisualStyleBackColor = true;
            // 
            // BreakOnIRQCheckBox
            // 
            this.BreakOnIRQCheckBox.AutoSize = true;
            this.BreakOnIRQCheckBox.Checked = true;
            this.BreakOnIRQCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BreakOnIRQCheckBox.Location = new System.Drawing.Point(3, 3);
            this.BreakOnIRQCheckBox.Name = "BreakOnIRQCheckBox";
            this.BreakOnIRQCheckBox.Size = new System.Drawing.Size(91, 17);
            this.BreakOnIRQCheckBox.TabIndex = 16;
            this.BreakOnIRQCheckBox.Text = "Break on IRQ";
            this.BreakOnIRQCheckBox.UseVisualStyleBackColor = true;
            this.BreakOnIRQCheckBox.CheckedChanged += new System.EventHandler(this.BreakOnIRQCheckBox_CheckedChanged);
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
            this.ClientSize = new System.Drawing.Size(758, 517);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.HeaderTextbox);
            this.Controls.Add(this.StepOverButton);
            this.Controls.Add(this.InspectButton);
            this.Controls.Add(this.MinusButton);
            this.Controls.Add(this.PlusButton);
            this.Controls.Add(this.DebugPanel);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.SecondPanel);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.lastLine);
            this.Controls.Add(this.stackText);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(1280, 0);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(774, 556);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(774, 556);
            this.Name = "CPUWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CPU Window";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPUWindow_FormClosed);
            this.Load += new System.EventHandler(this.CPUWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CPUWindow_KeyDown);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.SecondPanel.ResumeLayout(false);
            this.SecondPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DebugPanel)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private global::System.Windows.Forms.Label stepsLabel;
        private global::System.Windows.Forms.TextBox stepsInput;
        private global::System.Windows.Forms.Timer UpdateTraceTimer;
        private global::System.Windows.Forms.ComboBox BPCombo;
        private global::System.Windows.Forms.Button AddBPButton;
        private global::System.Windows.Forms.Button DeleteBPButton;
        private global::System.Windows.Forms.Panel SecondPanel;
        private global::System.Windows.Forms.Button ClearTraceButton;
        private System.Windows.Forms.ToolTip Tooltip;
        private System.Windows.Forms.PictureBox DebugPanel;
        private System.Windows.Forms.Button PlusButton;
        private System.Windows.Forms.Button MinusButton;
        private System.Windows.Forms.Button InspectButton;
        private System.Windows.Forms.Button StepOverButton;
        private System.Windows.Forms.Label HeaderTextbox;
        private System.Windows.Forms.Label BPLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox KeyboardCheckBox;
        private System.Windows.Forms.CheckBox SOFCheckbox;
        private System.Windows.Forms.CheckBox BreakOnIRQCheckBox;
        private System.Windows.Forms.CheckBox SOLCheckbox;
        private System.Windows.Forms.Label Reg2Label;
        private System.Windows.Forms.Label Reg1Label;
        private System.Windows.Forms.Label Reg0Label;
        private System.Windows.Forms.CheckBox MPU401Checkbox;
        private System.Windows.Forms.CheckBox COM1Checkbox;
        private System.Windows.Forms.CheckBox COM2Checkbox;
        private System.Windows.Forms.CheckBox FDCCheckbox;
        private System.Windows.Forms.CheckBox MouseCheckbox;
        private System.Windows.Forms.CheckBox RTCCheckbox;
        private System.Windows.Forms.CheckBox TMR2Checkbox;
        private System.Windows.Forms.CheckBox TMR1Checkbox;
        private System.Windows.Forms.CheckBox TMR0Checkbox;
        private System.Windows.Forms.CheckBox OPL2LCheckbox;
        private System.Windows.Forms.CheckBox OPL2RCheckbox;
        private System.Windows.Forms.CheckBox SDCardCheckBox;
        private System.Windows.Forms.Button StepOver;
    }
}