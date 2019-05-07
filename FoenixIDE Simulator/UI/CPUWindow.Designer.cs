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
            this.stepsInput = new System.Windows.Forms.TextBox();
            this.BPLabel = new System.Windows.Forms.Label();
            this.BPCombo = new System.Windows.Forms.ComboBox();
            this.AddBPButton = new System.Windows.Forms.Button();
            this.DeleteBPButton = new System.Windows.Forms.Button();
            this.stepsLabel = new System.Windows.Forms.Label();
            this.StepButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationInput = new System.Windows.Forms.TextBox();
            this.JumpButton = new System.Windows.Forms.Button();
            this.lastLine = new System.Windows.Forms.TextBox();
            this.stackText = new System.Windows.Forms.TextBox();
            this.HeaderTextbox = new System.Windows.Forms.TextBox();
            this.UpdateTraceTimer = new System.Windows.Forms.Timer(this.components);
            this.SecondPanel = new System.Windows.Forms.Panel();
            this.ClearTraceButton = new System.Windows.Forms.Button();
            this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.DebugPanel = new System.Windows.Forms.PictureBox();
            this.PlusButton = new System.Windows.Forms.Button();
            this.MinusButton = new System.Windows.Forms.Button();
            this.InspectButton = new System.Windows.Forms.Button();
            this.registerDisplay1 = new FoenixIDE.RegisterDisplay();
            this.HeaderPanel.SuspendLayout();
            this.SecondPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DebugPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Controls.Add(this.stepsInput);
            this.HeaderPanel.Controls.Add(this.BPLabel);
            this.HeaderPanel.Controls.Add(this.BPCombo);
            this.HeaderPanel.Controls.Add(this.AddBPButton);
            this.HeaderPanel.Controls.Add(this.DeleteBPButton);
            this.HeaderPanel.Controls.Add(this.stepsLabel);
            this.HeaderPanel.Controls.Add(this.StepButton);
            this.HeaderPanel.Controls.Add(this.RunButton);
            this.HeaderPanel.Controls.Add(this.PauseButton);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(616, 24);
            this.HeaderPanel.TabIndex = 2;
            // 
            // stepsInput
            // 
            this.stepsInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsInput.Location = new System.Drawing.Point(256, 0);
            this.stepsInput.Name = "stepsInput";
            this.stepsInput.Size = new System.Drawing.Size(64, 23);
            this.stepsInput.TabIndex = 3;
            this.stepsInput.Text = "1";
            this.stepsInput.Enter += new System.EventHandler(this.StepsInput_Enter);
            // 
            // BPLabel
            // 
            this.BPLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPLabel.Location = new System.Drawing.Point(351, 0);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.BPLabel.Size = new System.Drawing.Size(96, 24);
            this.BPLabel.TabIndex = 5;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BPCombo
            // 
            this.BPCombo.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new System.Drawing.Point(447, 0);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new System.Drawing.Size(121, 21);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.AddBPButton.Location = new System.Drawing.Point(568, 0);
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
            this.DeleteBPButton.Location = new System.Drawing.Point(592, 0);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new System.EventHandler(this.DeleteBPButton_Click);
            // 
            // stepsLabel
            // 
            this.stepsLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsLabel.Location = new System.Drawing.Point(192, 0);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.stepsLabel.Size = new System.Drawing.Size(64, 24);
            this.stepsLabel.TabIndex = 4;
            this.stepsLabel.Text = "Steps (dec)";
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new System.Drawing.Point(128, 0);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(64, 24);
            this.StepButton.TabIndex = 2;
            this.StepButton.Text = "Step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunButton.Location = new System.Drawing.Point(64, 0);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(64, 24);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PauseButton.Location = new System.Drawing.Point(0, 0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(64, 24);
            this.PauseButton.TabIndex = 0;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
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
            this.lastLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lastLine.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastLine.Location = new System.Drawing.Point(0, 517);
            this.lastLine.Name = "lastLine";
            this.lastLine.Size = new System.Drawing.Size(616, 23);
            this.lastLine.TabIndex = 4;
            this.lastLine.Text = "Click [Step] to execute an instruction";
            this.lastLine.MouseEnter += new System.EventHandler(this.DebugPanel_Leave);
            // 
            // stackText
            // 
            this.stackText.Dock = System.Windows.Forms.DockStyle.Right;
            this.stackText.Font = new System.Drawing.Font("Consolas", 10F);
            this.stackText.Location = new System.Drawing.Point(616, 0);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.Size = new System.Drawing.Size(150, 540);
            this.stackText.TabIndex = 3;
            // 
            // HeaderTextbox
            // 
            this.HeaderTextbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderTextbox.Font = new System.Drawing.Font("Consolas", 10F);
            this.HeaderTextbox.Location = new System.Drawing.Point(0, 101);
            this.HeaderTextbox.Multiline = true;
            this.HeaderTextbox.Name = "HeaderTextbox";
            this.HeaderTextbox.Size = new System.Drawing.Size(616, 20);
            this.HeaderTextbox.TabIndex = 1;
            this.HeaderTextbox.MouseEnter += new System.EventHandler(this.DebugPanel_Leave);
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
            this.SecondPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SecondPanel.Location = new System.Drawing.Point(0, 24);
            this.SecondPanel.Name = "SecondPanel";
            this.SecondPanel.Size = new System.Drawing.Size(616, 24);
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
            this.DebugPanel.Location = new System.Drawing.Point(0, 124);
            this.DebugPanel.Margin = new System.Windows.Forms.Padding(2);
            this.DebugPanel.Name = "DebugPanel";
            this.DebugPanel.Size = new System.Drawing.Size(613, 392);
            this.DebugPanel.TabIndex = 6;
            this.DebugPanel.TabStop = false;
            this.DebugPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DebugPanel_Paint);
            this.DebugPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DebugPanel_MouseMove);
            // 
            // PlusButton
            // 
            this.PlusButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PlusButton.FlatAppearance.BorderSize = 0;
            this.PlusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.MinusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.InspectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InspectButton.Location = new System.Drawing.Point(137, 200);
            this.InspectButton.Margin = new System.Windows.Forms.Padding(2);
            this.InspectButton.Name = "InspectButton";
            this.InspectButton.Size = new System.Drawing.Size(38, 18);
            this.InspectButton.TabIndex = 9;
            this.InspectButton.TabStop = false;
            this.InspectButton.Text = "Mem";
            this.InspectButton.UseVisualStyleBackColor = false;
            this.InspectButton.Visible = false;
            this.InspectButton.Click += new System.EventHandler(this.InspectButton_Click);
            // 
            // registerDisplay1
            // 
            this.registerDisplay1.CPU = null;
            this.registerDisplay1.Dock = System.Windows.Forms.DockStyle.Top;
            this.registerDisplay1.Location = new System.Drawing.Point(0, 48);
            this.registerDisplay1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new System.Drawing.Size(616, 53);
            this.registerDisplay1.TabIndex = 0;
            this.registerDisplay1.MouseEnter += new System.EventHandler(this.DebugPanel_Leave);
            // 
            // CPUWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 540);
            this.Controls.Add(this.InspectButton);
            this.Controls.Add(this.MinusButton);
            this.Controls.Add(this.PlusButton);
            this.Controls.Add(this.DebugPanel);
            this.Controls.Add(this.HeaderTextbox);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.SecondPanel);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.lastLine);
            this.Controls.Add(this.stackText);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(1280, 0);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(787, 592);
            this.Name = "CPUWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CPU Window";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPUWindow_FormClosed);
            this.Load += new System.EventHandler(this.CPUWindow_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.SecondPanel.ResumeLayout(false);
            this.SecondPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DebugPanel)).EndInit();
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
        private global::System.Windows.Forms.Button PauseButton;
        private global::System.Windows.Forms.TextBox lastLine;
        private global::System.Windows.Forms.TextBox stackText;
        private global::System.Windows.Forms.Label locationLabel;
        private global::System.Windows.Forms.Label stepsLabel;
        private global::System.Windows.Forms.TextBox stepsInput;
        private global::System.Windows.Forms.TextBox HeaderTextbox;
        private global::System.Windows.Forms.Timer UpdateTraceTimer;
        private global::System.Windows.Forms.Label BPLabel;
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
    }
}