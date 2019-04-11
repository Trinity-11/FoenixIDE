namespace FoenixIDE.UI
{
    partial class MemoryWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoryWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.IOButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Page18Button = new System.Windows.Forms.Button();
            this.Page00Button = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.ViewButton = new System.Windows.Forms.Button();
            this.EndAddressText = new System.Windows.Forms.TextBox();
            this.StartAddressText = new System.Windows.Forms.TextBox();
            this.MemoryText = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.MCRBit0Button = new System.Windows.Forms.Button();
            this.MCRBit1Button = new System.Windows.Forms.Button();
            this.MCRBit2Button = new System.Windows.Forms.Button();
            this.MCRBit3Button = new System.Windows.Forms.Button();
            this.MCRBit4Button = new System.Windows.Forms.Button();
            this.MCRBit5Button = new System.Windows.Forms.Button();
            this.MCRBit6Button = new System.Windows.Forms.Button();
            this.MCRBit7Button = new System.Windows.Forms.Button();
            this.MasterControlLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.IOButton);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.Page18Button);
            this.panel1.Controls.Add(this.Page00Button);
            this.panel1.Controls.Add(this.PreviousButton);
            this.panel1.Controls.Add(this.NextButton);
            this.panel1.Controls.Add(this.ViewButton);
            this.panel1.Controls.Add(this.EndAddressText);
            this.panel1.Controls.Add(this.StartAddressText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(762, 25);
            this.panel1.TabIndex = 0;
            // 
            // IOButton
            // 
            this.IOButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.IOButton.Location = new System.Drawing.Point(636, 0);
            this.IOButton.Margin = new System.Windows.Forms.Padding(4);
            this.IOButton.Name = "IOButton";
            this.IOButton.Size = new System.Drawing.Size(100, 25);
            this.IOButton.TabIndex = 8;
            this.IOButton.Text = "I/O";
            this.IOButton.UseVisualStyleBackColor = true;
            this.IOButton.Click += new System.EventHandler(this.IOButton_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.Location = new System.Drawing.Point(536, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 25);
            this.button2.TabIndex = 7;
            this.button2.Text = "Page $19";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Page19_Click);
            // 
            // Page18Button
            // 
            this.Page18Button.Dock = System.Windows.Forms.DockStyle.Left;
            this.Page18Button.Location = new System.Drawing.Point(436, 0);
            this.Page18Button.Margin = new System.Windows.Forms.Padding(4);
            this.Page18Button.Name = "Page18Button";
            this.Page18Button.Size = new System.Drawing.Size(100, 25);
            this.Page18Button.TabIndex = 6;
            this.Page18Button.Text = "Page $18";
            this.Page18Button.UseVisualStyleBackColor = true;
            this.Page18Button.Click += new System.EventHandler(this.Page18Button_Click);
            // 
            // Page00Button
            // 
            this.Page00Button.Dock = System.Windows.Forms.DockStyle.Left;
            this.Page00Button.Location = new System.Drawing.Point(336, 0);
            this.Page00Button.Margin = new System.Windows.Forms.Padding(4);
            this.Page00Button.Name = "Page00Button";
            this.Page00Button.Size = new System.Drawing.Size(100, 25);
            this.Page00Button.TabIndex = 5;
            this.Page00Button.Text = "Page $00";
            this.Page00Button.UseVisualStyleBackColor = true;
            this.Page00Button.Click += new System.EventHandler(this.Page00_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PreviousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousButton.Location = new System.Drawing.Point(303, 0);
            this.PreviousButton.Margin = new System.Windows.Forms.Padding(4);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(33, 25);
            this.PreviousButton.TabIndex = 4;
            this.PreviousButton.Text = "←";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.Location = new System.Drawing.Point(268, 0);
            this.NextButton.Margin = new System.Windows.Forms.Padding(4);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(35, 25);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "→";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // ViewButton
            // 
            this.ViewButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ViewButton.Location = new System.Drawing.Point(168, 0);
            this.ViewButton.Margin = new System.Windows.Forms.Padding(4);
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(100, 25);
            this.ViewButton.TabIndex = 2;
            this.ViewButton.Text = "View";
            this.ViewButton.UseVisualStyleBackColor = true;
            this.ViewButton.Click += new System.EventHandler(this.ViewButton_Click);
            // 
            // EndAddressText
            // 
            this.EndAddressText.Dock = System.Windows.Forms.DockStyle.Left;
            this.EndAddressText.Font = new System.Drawing.Font("Consolas", 10F);
            this.EndAddressText.Location = new System.Drawing.Point(84, 0);
            this.EndAddressText.Margin = new System.Windows.Forms.Padding(4);
            this.EndAddressText.Name = "EndAddressText";
            this.EndAddressText.Size = new System.Drawing.Size(84, 27);
            this.EndAddressText.TabIndex = 1;
            this.EndAddressText.Text = "0000FF";
            this.EndAddressText.Validated += new System.EventHandler(this.EndAddressText_Validated);
            // 
            // StartAddressText
            // 
            this.StartAddressText.Dock = System.Windows.Forms.DockStyle.Left;
            this.StartAddressText.Font = new System.Drawing.Font("Consolas", 10F);
            this.StartAddressText.Location = new System.Drawing.Point(0, 0);
            this.StartAddressText.Margin = new System.Windows.Forms.Padding(4);
            this.StartAddressText.Name = "StartAddressText";
            this.StartAddressText.Size = new System.Drawing.Size(84, 27);
            this.StartAddressText.TabIndex = 0;
            this.StartAddressText.Text = "000000";
            this.StartAddressText.Validated += new System.EventHandler(this.StartAddressText_Validated);
            // 
            // MemoryText
            // 
            this.MemoryText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemoryText.Font = new System.Drawing.Font("Consolas", 10F);
            this.MemoryText.Location = new System.Drawing.Point(0, 25);
            this.MemoryText.Margin = new System.Windows.Forms.Padding(4);
            this.MemoryText.Multiline = true;
            this.MemoryText.Name = "MemoryText";
            this.MemoryText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MemoryText.Size = new System.Drawing.Size(762, 347);
            this.MemoryText.TabIndex = 0;
            this.MemoryText.Text = resources.GetString("MemoryText.Text");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.MCRBit0Button);
            this.panel2.Controls.Add(this.MCRBit1Button);
            this.panel2.Controls.Add(this.MCRBit2Button);
            this.panel2.Controls.Add(this.MCRBit3Button);
            this.panel2.Controls.Add(this.MCRBit4Button);
            this.panel2.Controls.Add(this.MCRBit5Button);
            this.panel2.Controls.Add(this.MCRBit6Button);
            this.panel2.Controls.Add(this.MCRBit7Button);
            this.panel2.Controls.Add(this.MasterControlLabel);
            this.panel2.Location = new System.Drawing.Point(0, 379);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(762, 37);
            this.panel2.TabIndex = 1;
            // 
            // MCRBit0Button
            // 
            this.MCRBit0Button.Location = new System.Drawing.Point(452, 3);
            this.MCRBit0Button.Name = "MCRBit0Button";
            this.MCRBit0Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit0Button.TabIndex = 8;
            this.MCRBit0Button.Tag = "0";
            this.MCRBit0Button.Text = "T";
            this.MCRBit0Button.UseVisualStyleBackColor = true;
            this.MCRBit0Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit1Button
            // 
            this.MCRBit1Button.Location = new System.Drawing.Point(416, 3);
            this.MCRBit1Button.Name = "MCRBit1Button";
            this.MCRBit1Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit1Button.TabIndex = 7;
            this.MCRBit1Button.Tag = "0";
            this.MCRBit1Button.Text = "O";
            this.MCRBit1Button.UseVisualStyleBackColor = true;
            this.MCRBit1Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit2Button
            // 
            this.MCRBit2Button.Location = new System.Drawing.Point(380, 3);
            this.MCRBit2Button.Name = "MCRBit2Button";
            this.MCRBit2Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit2Button.TabIndex = 6;
            this.MCRBit2Button.Tag = "0";
            this.MCRBit2Button.Text = "G";
            this.MCRBit2Button.UseVisualStyleBackColor = true;
            this.MCRBit2Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit3Button
            // 
            this.MCRBit3Button.Location = new System.Drawing.Point(344, 3);
            this.MCRBit3Button.Name = "MCRBit3Button";
            this.MCRBit3Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit3Button.TabIndex = 5;
            this.MCRBit3Button.Tag = "0";
            this.MCRBit3Button.Text = "B";
            this.MCRBit3Button.UseVisualStyleBackColor = true;
            this.MCRBit3Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit4Button
            // 
            this.MCRBit4Button.Location = new System.Drawing.Point(301, 3);
            this.MCRBit4Button.Name = "MCRBit4Button";
            this.MCRBit4Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit4Button.TabIndex = 4;
            this.MCRBit4Button.Tag = "0";
            this.MCRBit4Button.Text = "T";
            this.MCRBit4Button.UseVisualStyleBackColor = true;
            this.MCRBit4Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit5Button
            // 
            this.MCRBit5Button.Location = new System.Drawing.Point(265, 3);
            this.MCRBit5Button.Name = "MCRBit5Button";
            this.MCRBit5Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit5Button.TabIndex = 3;
            this.MCRBit5Button.Tag = "0";
            this.MCRBit5Button.Text = "S";
            this.MCRBit5Button.UseVisualStyleBackColor = true;
            this.MCRBit5Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit6Button
            // 
            this.MCRBit6Button.Location = new System.Drawing.Point(229, 3);
            this.MCRBit6Button.Name = "MCRBit6Button";
            this.MCRBit6Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit6Button.TabIndex = 2;
            this.MCRBit6Button.Tag = "0";
            this.MCRBit6Button.Text = "G";
            this.MCRBit6Button.UseVisualStyleBackColor = true;
            this.MCRBit6Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit7Button
            // 
            this.MCRBit7Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MCRBit7Button.Location = new System.Drawing.Point(193, 3);
            this.MCRBit7Button.Name = "MCRBit7Button";
            this.MCRBit7Button.Size = new System.Drawing.Size(35, 30);
            this.MCRBit7Button.TabIndex = 1;
            this.MCRBit7Button.Tag = "0";
            this.MCRBit7Button.Text = "D";
            this.MCRBit7Button.UseVisualStyleBackColor = true;
            this.MCRBit7Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MasterControlLabel
            // 
            this.MasterControlLabel.AutoSize = true;
            this.MasterControlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MasterControlLabel.Location = new System.Drawing.Point(12, 11);
            this.MasterControlLabel.Name = "MasterControlLabel";
            this.MasterControlLabel.Size = new System.Drawing.Size(156, 18);
            this.MasterControlLabel.TabIndex = 0;
            this.MasterControlLabel.Text = "Master Control Reg";
            // 
            // MemoryWindow
            // 
            this.AcceptButton = this.ViewButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 416);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.MemoryText);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(780, 463);
            this.Name = "MemoryWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MemoryWindow";
            this.Load += new System.EventHandler(this.MemoryWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MemoryWindow_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private global::System.Windows.Forms.Button ViewButton;
        private global::System.Windows.Forms.TextBox EndAddressText;
        private global::System.Windows.Forms.TextBox StartAddressText;
        private global::System.Windows.Forms.TextBox MemoryText;
        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Button Page00Button;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Page18Button;
        private System.Windows.Forms.Button IOButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button MCRBit0Button;
        private System.Windows.Forms.Button MCRBit1Button;
        private System.Windows.Forms.Button MCRBit2Button;
        private System.Windows.Forms.Button MCRBit3Button;
        private System.Windows.Forms.Button MCRBit4Button;
        private System.Windows.Forms.Button MCRBit5Button;
        private System.Windows.Forms.Button MCRBit6Button;
        private System.Windows.Forms.Button MCRBit7Button;
        private System.Windows.Forms.Label MasterControlLabel;
    }
}