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
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.CompactCheckbox = new System.Windows.Forms.CheckBox();
            this.ExportButton = new System.Windows.Forms.Button();
            this.AddressCombo = new System.Windows.Forms.ComboBox();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.ViewButton = new System.Windows.Forms.Button();
            this.EndAddressText = new System.Windows.Forms.TextBox();
            this.StartAddressText = new System.Windows.Forms.TextBox();
            this.MemoryText = new System.Windows.Forms.TextBox();
            this.UpdateDisplayTimer = new System.Windows.Forms.Timer(this.components);
            this.MemoryWindowTooltips = new System.Windows.Forms.ToolTip(this.components);
            this.FooterPanel = new System.Windows.Forms.Panel();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.MCRBit0Button = new System.Windows.Forms.Button();
            this.MCRBit1Button = new System.Windows.Forms.Button();
            this.MCRBit2Button = new System.Windows.Forms.Button();
            this.MCRBit3Button = new System.Windows.Forms.Button();
            this.MCRBit4Button = new System.Windows.Forms.Button();
            this.MCRBit5Button = new System.Windows.Forms.Button();
            this.MCRBit6Button = new System.Windows.Forms.Button();
            this.MCRBit7Button = new System.Windows.Forms.Button();
            this.MasterControlLabel = new System.Windows.Forms.Label();
            this.HighlightPanel = new System.Windows.Forms.TextBox();
            this.HeaderPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPanel.Controls.Add(this.CompactCheckbox);
            this.HeaderPanel.Controls.Add(this.ExportButton);
            this.HeaderPanel.Controls.Add(this.AddressCombo);
            this.HeaderPanel.Controls.Add(this.PreviousButton);
            this.HeaderPanel.Controls.Add(this.NextButton);
            this.HeaderPanel.Controls.Add(this.ViewButton);
            this.HeaderPanel.Controls.Add(this.EndAddressText);
            this.HeaderPanel.Controls.Add(this.StartAddressText);
            this.HeaderPanel.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(4);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(757, 31);
            this.HeaderPanel.TabIndex = 0;
            // 
            // CompactCheckbox
            // 
            this.CompactCheckbox.AutoSize = true;
            this.CompactCheckbox.Checked = true;
            this.CompactCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CompactCheckbox.Location = new System.Drawing.Point(664, 5);
            this.CompactCheckbox.Name = "CompactCheckbox";
            this.CompactCheckbox.Size = new System.Drawing.Size(85, 21);
            this.CompactCheckbox.TabIndex = 12;
            this.CompactCheckbox.Text = "Compact";
            this.CompactCheckbox.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(592, 0);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(70, 29);
            this.ExportButton.TabIndex = 11;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // AddressCombo
            // 
            this.AddressCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AddressCombo.FormattingEnabled = true;
            this.AddressCombo.Items.AddRange(new object[] {
            "Bank $00",
            "Bank $18",
            "Bank $19",
            "Bank $AF (Vicky)",
            "Bank $B0 (Video)",
            "Address $AF:0100 (Bitmap and Tile Registers)",
            "Address $AF:5000 (Tilemap 0)",
            "Address $AF:8000 (Font Memory Bank 0)",
            "Address $AF:E000 (Beatrix)",
            "Unspecified Page"});
            this.AddressCombo.Location = new System.Drawing.Point(286, 3);
            this.AddressCombo.Name = "AddressCombo";
            this.AddressCombo.Size = new System.Drawing.Size(305, 24);
            this.AddressCombo.TabIndex = 10;
            this.AddressCombo.SelectedIndexChanged += new System.EventHandler(this.AddressCombo_SelectedIndexChanged);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousButton.Location = new System.Drawing.Point(251, 0);
            this.PreviousButton.Margin = new System.Windows.Forms.Padding(4);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(33, 29);
            this.PreviousButton.TabIndex = 4;
            this.PreviousButton.Text = "←";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.Location = new System.Drawing.Point(216, 0);
            this.NextButton.Margin = new System.Windows.Forms.Padding(4);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(35, 29);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "→";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // ViewButton
            // 
            this.ViewButton.Location = new System.Drawing.Point(145, 0);
            this.ViewButton.Margin = new System.Windows.Forms.Padding(4);
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(70, 29);
            this.ViewButton.TabIndex = 2;
            this.ViewButton.Text = "View";
            this.ViewButton.UseVisualStyleBackColor = true;
            this.ViewButton.Click += new System.EventHandler(this.ViewButton_Click);
            // 
            // EndAddressText
            // 
            this.EndAddressText.Font = new System.Drawing.Font("Consolas", 10F);
            this.EndAddressText.Location = new System.Drawing.Point(73, 0);
            this.EndAddressText.Margin = new System.Windows.Forms.Padding(4);
            this.EndAddressText.MaxLength = 6;
            this.EndAddressText.Name = "EndAddressText";
            this.EndAddressText.Size = new System.Drawing.Size(70, 27);
            this.EndAddressText.TabIndex = 1;
            this.EndAddressText.Text = "0000FF";
            this.EndAddressText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.EndAddressText.Validated += new System.EventHandler(this.EndAddressText_Validated);
            // 
            // StartAddressText
            // 
            this.StartAddressText.Dock = System.Windows.Forms.DockStyle.Left;
            this.StartAddressText.Font = new System.Drawing.Font("Consolas", 10F);
            this.StartAddressText.Location = new System.Drawing.Point(0, 0);
            this.StartAddressText.Margin = new System.Windows.Forms.Padding(4);
            this.StartAddressText.MaxLength = 6;
            this.StartAddressText.Name = "StartAddressText";
            this.StartAddressText.Size = new System.Drawing.Size(70, 27);
            this.StartAddressText.TabIndex = 0;
            this.StartAddressText.Text = "000000";
            this.StartAddressText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.StartAddressText.Validated += new System.EventHandler(this.StartAddressText_Validated);
            // 
            // MemoryText
            // 
            this.MemoryText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MemoryText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MemoryText.Font = new System.Drawing.Font("Consolas", 10F);
            this.MemoryText.Location = new System.Drawing.Point(0, 28);
            this.MemoryText.Margin = new System.Windows.Forms.Padding(4);
            this.MemoryText.MaxLength = 4096;
            this.MemoryText.Multiline = true;
            this.MemoryText.Name = "MemoryText";
            this.MemoryText.ReadOnly = true;
            this.MemoryText.Size = new System.Drawing.Size(759, 350);
            this.MemoryText.TabIndex = 0;
            this.MemoryText.TabStop = false;
            this.MemoryText.Text = resources.GetString("MemoryText.Text");
            this.MemoryText.WordWrap = false;
            this.MemoryText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MemoryText_MouseMove);
            // 
            // UpdateDisplayTimer
            // 
            this.UpdateDisplayTimer.Enabled = true;
            this.UpdateDisplayTimer.Interval = 1000;
            this.UpdateDisplayTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MemoryWindowTooltips
            // 
            this.MemoryWindowTooltips.ShowAlways = true;
            // 
            // FooterPanel
            // 
            this.FooterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FooterPanel.Controls.Add(this.PositionLabel);
            this.FooterPanel.Controls.Add(this.MCRBit0Button);
            this.FooterPanel.Controls.Add(this.MCRBit1Button);
            this.FooterPanel.Controls.Add(this.MCRBit2Button);
            this.FooterPanel.Controls.Add(this.MCRBit3Button);
            this.FooterPanel.Controls.Add(this.MCRBit4Button);
            this.FooterPanel.Controls.Add(this.MCRBit5Button);
            this.FooterPanel.Controls.Add(this.MCRBit6Button);
            this.FooterPanel.Controls.Add(this.MCRBit7Button);
            this.FooterPanel.Controls.Add(this.MasterControlLabel);
            this.FooterPanel.Location = new System.Drawing.Point(0, 382);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Size = new System.Drawing.Size(762, 37);
            this.FooterPanel.TabIndex = 1;
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(545, 10);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(46, 17);
            this.PositionLabel.TabIndex = 9;
            this.PositionLabel.Text = "label1";
            // 
            // MCRBit0Button
            // 
            this.MCRBit0Button.Location = new System.Drawing.Point(452, 3);
            this.MCRBit0Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit0Button.Name = "MCRBit0Button";
            this.MCRBit0Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit0Button.TabIndex = 8;
            this.MCRBit0Button.Tag = "0";
            this.MCRBit0Button.Text = "Tx";
            this.MCRBit0Button.UseVisualStyleBackColor = true;
            this.MCRBit0Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit1Button
            // 
            this.MCRBit1Button.Location = new System.Drawing.Point(416, 3);
            this.MCRBit1Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit1Button.Name = "MCRBit1Button";
            this.MCRBit1Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit1Button.TabIndex = 7;
            this.MCRBit1Button.Tag = "0";
            this.MCRBit1Button.Text = "Ov";
            this.MCRBit1Button.UseVisualStyleBackColor = true;
            this.MCRBit1Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit2Button
            // 
            this.MCRBit2Button.Location = new System.Drawing.Point(380, 3);
            this.MCRBit2Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit2Button.Name = "MCRBit2Button";
            this.MCRBit2Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit2Button.TabIndex = 6;
            this.MCRBit2Button.Tag = "0";
            this.MCRBit2Button.Text = "G";
            this.MCRBit2Button.UseVisualStyleBackColor = true;
            this.MCRBit2Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit3Button
            // 
            this.MCRBit3Button.Location = new System.Drawing.Point(344, 3);
            this.MCRBit3Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit3Button.Name = "MCRBit3Button";
            this.MCRBit3Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit3Button.TabIndex = 5;
            this.MCRBit3Button.Tag = "0";
            this.MCRBit3Button.Text = "B";
            this.MCRBit3Button.UseVisualStyleBackColor = true;
            this.MCRBit3Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit4Button
            // 
            this.MCRBit4Button.Location = new System.Drawing.Point(301, 3);
            this.MCRBit4Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit4Button.Name = "MCRBit4Button";
            this.MCRBit4Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit4Button.TabIndex = 4;
            this.MCRBit4Button.Tag = "0";
            this.MCRBit4Button.Text = "Ti";
            this.MCRBit4Button.UseVisualStyleBackColor = true;
            this.MCRBit4Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit5Button
            // 
            this.MCRBit5Button.Location = new System.Drawing.Point(265, 3);
            this.MCRBit5Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit5Button.Name = "MCRBit5Button";
            this.MCRBit5Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit5Button.TabIndex = 3;
            this.MCRBit5Button.Tag = "0";
            this.MCRBit5Button.Text = "S";
            this.MCRBit5Button.UseVisualStyleBackColor = true;
            this.MCRBit5Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit6Button
            // 
            this.MCRBit6Button.Location = new System.Drawing.Point(229, 3);
            this.MCRBit6Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit6Button.Name = "MCRBit6Button";
            this.MCRBit6Button.Size = new System.Drawing.Size(35, 32);
            this.MCRBit6Button.TabIndex = 2;
            this.MCRBit6Button.Tag = "0";
            this.MCRBit6Button.Text = "Ga";
            this.MCRBit6Button.UseVisualStyleBackColor = true;
            this.MCRBit6Button.Click += new System.EventHandler(this.MCRBitButton_Click);
            // 
            // MCRBit7Button
            // 
            this.MCRBit7Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MCRBit7Button.Location = new System.Drawing.Point(193, 3);
            this.MCRBit7Button.Margin = new System.Windows.Forms.Padding(2);
            this.MCRBit7Button.Name = "MCRBit7Button";
            this.MCRBit7Button.Size = new System.Drawing.Size(35, 32);
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
            // HighlightPanel
            // 
            this.HighlightPanel.BackColor = System.Drawing.Color.Red;
            this.HighlightPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HighlightPanel.CausesValidation = false;
            this.HighlightPanel.Font = new System.Drawing.Font("Consolas", 10F);
            this.HighlightPanel.Location = new System.Drawing.Point(387, 331);
            this.HighlightPanel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.HighlightPanel.MaxLength = 2;
            this.HighlightPanel.Name = "HighlightPanel";
            this.HighlightPanel.Size = new System.Drawing.Size(27, 20);
            this.HighlightPanel.TabIndex = 4;
            this.HighlightPanel.TabStop = false;
            this.HighlightPanel.Text = "00";
            this.HighlightPanel.Visible = false;
            this.HighlightPanel.WordWrap = false;
            this.HighlightPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HighlightPanel_KeyUp);
            // 
            // MemoryWindow
            // 
            this.AcceptButton = this.ViewButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 418);
            this.Controls.Add(this.HighlightPanel);
            this.Controls.Add(this.FooterPanel);
            this.Controls.Add(this.MemoryText);
            this.Controls.Add(this.HeaderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(779, 460);
            this.Name = "MemoryWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MemoryWindow";
            this.Load += new System.EventHandler(this.MemoryWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MemoryWindow_KeyDown);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.FooterPanel.ResumeLayout(false);
            this.FooterPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private global::System.Windows.Forms.Button ViewButton;
        private global::System.Windows.Forms.TextBox EndAddressText;
        private global::System.Windows.Forms.TextBox StartAddressText;
        private global::System.Windows.Forms.TextBox MemoryText;
        private global::System.Windows.Forms.Panel HeaderPanel;
        private global::System.Windows.Forms.Timer UpdateDisplayTimer;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.ToolTip MemoryWindowTooltips;
        private System.Windows.Forms.Panel FooterPanel;
        private System.Windows.Forms.Button MCRBit0Button;
        private System.Windows.Forms.Button MCRBit1Button;
        private System.Windows.Forms.Button MCRBit2Button;
        private System.Windows.Forms.Button MCRBit3Button;
        private System.Windows.Forms.Button MCRBit4Button;
        private System.Windows.Forms.Button MCRBit5Button;
        private System.Windows.Forms.Button MCRBit6Button;
        private System.Windows.Forms.Button MCRBit7Button;
        private System.Windows.Forms.Label MasterControlLabel;
        private System.Windows.Forms.ComboBox AddressCombo;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.CheckBox CompactCheckbox;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.TextBox HighlightPanel;
    }
}