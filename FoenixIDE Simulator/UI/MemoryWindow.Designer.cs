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
            this.panel1.SuspendLayout();
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
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1087, 37);
            this.panel1.TabIndex = 0;
            // 
            // IOButton
            // 
            this.IOButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.IOButton.Location = new System.Drawing.Point(873, 0);
            this.IOButton.Margin = new System.Windows.Forms.Padding(6);
            this.IOButton.Name = "IOButton";
            this.IOButton.Size = new System.Drawing.Size(138, 37);
            this.IOButton.TabIndex = 8;
            this.IOButton.Text = "I/O";
            this.IOButton.UseVisualStyleBackColor = true;
            this.IOButton.Click += new System.EventHandler(this.IOButton_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.Location = new System.Drawing.Point(735, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 37);
            this.button2.TabIndex = 7;
            this.button2.Text = "Page $19";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Page19_Click);
            // 
            // Page18Button
            // 
            this.Page18Button.Dock = System.Windows.Forms.DockStyle.Left;
            this.Page18Button.Location = new System.Drawing.Point(597, 0);
            this.Page18Button.Margin = new System.Windows.Forms.Padding(6);
            this.Page18Button.Name = "Page18Button";
            this.Page18Button.Size = new System.Drawing.Size(138, 37);
            this.Page18Button.TabIndex = 6;
            this.Page18Button.Text = "Page $18";
            this.Page18Button.UseVisualStyleBackColor = true;
            this.Page18Button.Click += new System.EventHandler(this.Page18Button_Click);
            // 
            // Page00Button
            // 
            this.Page00Button.Dock = System.Windows.Forms.DockStyle.Left;
            this.Page00Button.Location = new System.Drawing.Point(459, 0);
            this.Page00Button.Margin = new System.Windows.Forms.Padding(6);
            this.Page00Button.Name = "Page00Button";
            this.Page00Button.Size = new System.Drawing.Size(138, 37);
            this.Page00Button.TabIndex = 5;
            this.Page00Button.Text = "Page $00";
            this.Page00Button.UseVisualStyleBackColor = true;
            this.Page00Button.Click += new System.EventHandler(this.Page00_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PreviousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousButton.Location = new System.Drawing.Point(414, 0);
            this.PreviousButton.Margin = new System.Windows.Forms.Padding(6);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(45, 37);
            this.PreviousButton.TabIndex = 4;
            this.PreviousButton.Text = "←";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.Location = new System.Drawing.Point(366, 0);
            this.NextButton.Margin = new System.Windows.Forms.Padding(6);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(48, 37);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "→";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // ViewButton
            // 
            this.ViewButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ViewButton.Location = new System.Drawing.Point(228, 0);
            this.ViewButton.Margin = new System.Windows.Forms.Padding(6);
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(138, 37);
            this.ViewButton.TabIndex = 2;
            this.ViewButton.Text = "View";
            this.ViewButton.UseVisualStyleBackColor = true;
            this.ViewButton.Click += new System.EventHandler(this.ViewButton_Click);
            // 
            // EndAddressText
            // 
            this.EndAddressText.Dock = System.Windows.Forms.DockStyle.Left;
            this.EndAddressText.Font = new System.Drawing.Font("Consolas", 10F);
            this.EndAddressText.Location = new System.Drawing.Point(114, 0);
            this.EndAddressText.Margin = new System.Windows.Forms.Padding(6);
            this.EndAddressText.Name = "EndAddressText";
            this.EndAddressText.Size = new System.Drawing.Size(114, 35);
            this.EndAddressText.TabIndex = 1;
            this.EndAddressText.Text = "0000FF";
            this.EndAddressText.Validated += new System.EventHandler(this.EndAddressText_Validated);
            // 
            // StartAddressText
            // 
            this.StartAddressText.Dock = System.Windows.Forms.DockStyle.Left;
            this.StartAddressText.Font = new System.Drawing.Font("Consolas", 10F);
            this.StartAddressText.Location = new System.Drawing.Point(0, 0);
            this.StartAddressText.Margin = new System.Windows.Forms.Padding(6);
            this.StartAddressText.Name = "StartAddressText";
            this.StartAddressText.Size = new System.Drawing.Size(114, 35);
            this.StartAddressText.TabIndex = 0;
            this.StartAddressText.Text = "000000";
            this.StartAddressText.Validated += new System.EventHandler(this.StartAddressText_Validated);
            // 
            // MemoryText
            // 
            this.MemoryText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MemoryText.Font = new System.Drawing.Font("Consolas", 10F);
            this.MemoryText.Location = new System.Drawing.Point(0, 37);
            this.MemoryText.Margin = new System.Windows.Forms.Padding(6);
            this.MemoryText.Multiline = true;
            this.MemoryText.Name = "MemoryText";
            this.MemoryText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MemoryText.Size = new System.Drawing.Size(1087, 508);
            this.MemoryText.TabIndex = 0;
            this.MemoryText.Text = resources.GetString("MemoryText.Text");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MemoryWindow
            // 
            this.AcceptButton = this.ViewButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 545);
            this.Controls.Add(this.MemoryText);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MemoryWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MemoryWindow";
            this.Load += new System.EventHandler(this.MemoryWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MemoryWindow_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
    }
}