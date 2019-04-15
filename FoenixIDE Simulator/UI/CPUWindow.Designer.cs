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
            this.messageText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.ClearTraceButton = new System.Windows.Forms.Button();
            this.registerDisplay1 = new FoenixIDE.RegisterDisplay();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageText
            // 
            this.messageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageText.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageText.Location = new System.Drawing.Point(0, 220);
            this.messageText.Margin = new System.Windows.Forms.Padding(6);
            this.messageText.MaxLength = 128;
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.ReadOnly = true;
            this.messageText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.messageText.Size = new System.Drawing.Size(1142, 767);
            this.messageText.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stepsInput);
            this.panel1.Controls.Add(this.BPLabel);
            this.panel1.Controls.Add(this.BPCombo);
            this.panel1.Controls.Add(this.AddBPButton);
            this.panel1.Controls.Add(this.DeleteBPButton);
            this.panel1.Controls.Add(this.stepsLabel);
            this.panel1.Controls.Add(this.StepButton);
            this.panel1.Controls.Add(this.RunButton);
            this.panel1.Controls.Add(this.PauseButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1142, 44);
            this.panel1.TabIndex = 2;
            // 
            // stepsInput
            // 
            this.stepsInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsInput.Location = new System.Drawing.Point(468, 0);
            this.stepsInput.Margin = new System.Windows.Forms.Padding(6);
            this.stepsInput.Name = "stepsInput";
            this.stepsInput.Size = new System.Drawing.Size(114, 35);
            this.stepsInput.TabIndex = 3;
            this.stepsInput.Text = "1";
            this.stepsInput.Enter += new System.EventHandler(this.stepsInput_Enter);
            // 
            // BPLabel
            // 
            this.BPLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPLabel.Location = new System.Drawing.Point(660, 0);
            this.BPLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.BPLabel.Size = new System.Drawing.Size(176, 44);
            this.BPLabel.TabIndex = 5;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BPCombo
            // 
            this.BPCombo.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new System.Drawing.Point(836, 0);
            this.BPCombo.Margin = new System.Windows.Forms.Padding(6);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new System.Drawing.Size(218, 32);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.AddBPButton.Location = new System.Drawing.Point(1054, 0);
            this.AddBPButton.Margin = new System.Windows.Forms.Padding(6);
            this.AddBPButton.Name = "AddBPButton";
            this.AddBPButton.Size = new System.Drawing.Size(44, 44);
            this.AddBPButton.TabIndex = 7;
            this.AddBPButton.Text = "+";
            this.AddBPButton.UseVisualStyleBackColor = true;
            this.AddBPButton.Click += new System.EventHandler(this.AddBPButton_Click);
            // 
            // DeleteBPButton
            // 
            this.DeleteBPButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.DeleteBPButton.Location = new System.Drawing.Point(1098, 0);
            this.DeleteBPButton.Margin = new System.Windows.Forms.Padding(6);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new System.Drawing.Size(44, 44);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new System.EventHandler(this.DeleteBPButton_Click);
            // 
            // stepsLabel
            // 
            this.stepsLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsLabel.Location = new System.Drawing.Point(351, 0);
            this.stepsLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.stepsLabel.Size = new System.Drawing.Size(117, 44);
            this.stepsLabel.TabIndex = 4;
            this.stepsLabel.Text = "Steps (dec)";
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new System.Drawing.Point(234, 0);
            this.StepButton.Margin = new System.Windows.Forms.Padding(6);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(117, 44);
            this.StepButton.TabIndex = 2;
            this.StepButton.Text = "Step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunButton.Location = new System.Drawing.Point(117, 0);
            this.RunButton.Margin = new System.Windows.Forms.Padding(6);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(117, 44);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PauseButton.Location = new System.Drawing.Point(0, 0);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(6);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(117, 44);
            this.PauseButton.TabIndex = 0;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationLabel.Location = new System.Drawing.Point(293, 0);
            this.locationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.locationLabel.Size = new System.Drawing.Size(117, 44);
            this.locationLabel.TabIndex = 9;
            this.locationLabel.Text = "Location $";
            // 
            // locationInput
            // 
            this.locationInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationInput.Location = new System.Drawing.Point(410, 0);
            this.locationInput.Margin = new System.Windows.Forms.Padding(6);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new System.Drawing.Size(114, 35);
            this.locationInput.TabIndex = 10;
            this.locationInput.Text = "000000";
            this.locationInput.TextChanged += new System.EventHandler(this.locationInput_TextChanged);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.JumpButton.Location = new System.Drawing.Point(176, 0);
            this.JumpButton.Margin = new System.Windows.Forms.Padding(6);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(117, 44);
            this.JumpButton.TabIndex = 11;
            this.JumpButton.Text = "Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
            // 
            // lastLine
            // 
            this.lastLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lastLine.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastLine.Location = new System.Drawing.Point(0, 987);
            this.lastLine.Margin = new System.Windows.Forms.Padding(6);
            this.lastLine.Name = "lastLine";
            this.lastLine.Size = new System.Drawing.Size(1142, 35);
            this.lastLine.TabIndex = 4;
            this.lastLine.Text = "Click [Step] to execute an instruction";
            // 
            // stackText
            // 
            this.stackText.Dock = System.Windows.Forms.DockStyle.Right;
            this.stackText.Font = new System.Drawing.Font("Consolas", 10F);
            this.stackText.Location = new System.Drawing.Point(1142, 0);
            this.stackText.Margin = new System.Windows.Forms.Padding(6);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.Size = new System.Drawing.Size(272, 1022);
            this.stackText.TabIndex = 3;
            // 
            // HeaderTextbox
            // 
            this.HeaderTextbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderTextbox.Font = new System.Drawing.Font("Consolas", 10F);
            this.HeaderTextbox.Location = new System.Drawing.Point(0, 186);
            this.HeaderTextbox.Margin = new System.Windows.Forms.Padding(6);
            this.HeaderTextbox.Multiline = true;
            this.HeaderTextbox.Name = "HeaderTextbox";
            this.HeaderTextbox.Size = new System.Drawing.Size(1142, 34);
            this.HeaderTextbox.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.locationInput);
            this.panel2.Controls.Add(this.locationLabel);
            this.panel2.Controls.Add(this.JumpButton);
            this.panel2.Controls.Add(this.ClearTraceButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 44);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1142, 44);
            this.panel2.TabIndex = 5;
            // 
            // ClearTraceButton
            // 
            this.ClearTraceButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ClearTraceButton.Location = new System.Drawing.Point(0, 0);
            this.ClearTraceButton.Margin = new System.Windows.Forms.Padding(6);
            this.ClearTraceButton.Name = "ClearTraceButton";
            this.ClearTraceButton.Size = new System.Drawing.Size(176, 44);
            this.ClearTraceButton.TabIndex = 12;
            this.ClearTraceButton.Text = "Clear Trace";
            this.ClearTraceButton.UseVisualStyleBackColor = true;
            this.ClearTraceButton.Click += new System.EventHandler(this.ClearTraceButton_Click);
            // 
            // registerDisplay1
            // 
            this.registerDisplay1.CPU = null;
            this.registerDisplay1.Dock = System.Windows.Forms.DockStyle.Top;
            this.registerDisplay1.Location = new System.Drawing.Point(0, 88);
            this.registerDisplay1.Margin = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new System.Drawing.Size(1142, 98);
            this.registerDisplay1.TabIndex = 0;
            // 
            // CPUWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1414, 1022);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.HeaderTextbox);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lastLine);
            this.Controls.Add(this.stackText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(1280, 0);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "CPUWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CPU Window";
            this.Load += new System.EventHandler(this.CPUWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public RegisterDisplay registerDisplay1;
        private global::System.Windows.Forms.TextBox messageText;
        private global::System.Windows.Forms.Panel panel1;
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
        private global::System.Windows.Forms.Timer timer1;
        private global::System.Windows.Forms.Label BPLabel;
        private global::System.Windows.Forms.ComboBox BPCombo;
        private global::System.Windows.Forms.Button AddBPButton;
        private global::System.Windows.Forms.Button DeleteBPButton;
        private global::System.Windows.Forms.Panel panel2;
        private global::System.Windows.Forms.Button ClearTraceButton;
    }
}