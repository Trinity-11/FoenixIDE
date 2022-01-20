namespace MIDI_to_VGM_Converter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MIDIOutputText = new System.Windows.Forms.TextBox();
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.FileLabel = new System.Windows.Forms.Label();
            this.GeneratePanel = new System.Windows.Forms.Panel();
            this.SingleChannel = new System.Windows.Forms.ComboBox();
            this.SingleChannelLabel = new System.Windows.Forms.Label();
            this.PercussionMode = new System.Windows.Forms.CheckBox();
            this.GenerateVGMButton = new System.Windows.Forms.Button();
            this.GeneratePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MIDIOutputText
            // 
            this.MIDIOutputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutputText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MIDIOutputText.Location = new System.Drawing.Point(16, 73);
            this.MIDIOutputText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MIDIOutputText.MaxLength = 65536;
            this.MIDIOutputText.Multiline = true;
            this.MIDIOutputText.Name = "MIDIOutputText";
            this.MIDIOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MIDIOutputText.Size = new System.Drawing.Size(1033, 466);
            this.MIDIOutputText.TabIndex = 3;
            this.MIDIOutputText.WordWrap = false;
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(16, 4);
            this.ReadFileButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(100, 28);
            this.ReadFileButton.TabIndex = 2;
            this.ReadFileButton.Text = "Read File";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(124, 14);
            this.FileLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(0, 17);
            this.FileLabel.TabIndex = 4;
            // 
            // GeneratePanel
            // 
            this.GeneratePanel.Controls.Add(this.SingleChannel);
            this.GeneratePanel.Controls.Add(this.SingleChannelLabel);
            this.GeneratePanel.Controls.Add(this.PercussionMode);
            this.GeneratePanel.Controls.Add(this.GenerateVGMButton);
            this.GeneratePanel.Enabled = false;
            this.GeneratePanel.Location = new System.Drawing.Point(13, 33);
            this.GeneratePanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GeneratePanel.Name = "GeneratePanel";
            this.GeneratePanel.Size = new System.Drawing.Size(1037, 32);
            this.GeneratePanel.TabIndex = 10;
            // 
            // SingleChannel
            // 
            this.SingleChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SingleChannel.FormattingEnabled = true;
            this.SingleChannel.Items.AddRange(new object[] {
            "All",
            "Channel 1",
            "Channel 2",
            "Channel 3",
            "Channel 4",
            "Channel 5",
            "Channel 6",
            "Channel 7",
            "Channel 8",
            "Channel 9",
            "Channel 10 (Drums)",
            "Channel 11",
            "Channel 12",
            "Channel 13",
            "Channel 14",
            "Channel 15",
            "Channel 16"});
            this.SingleChannel.Location = new System.Drawing.Point(461, 4);
            this.SingleChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SingleChannel.Name = "SingleChannel";
            this.SingleChannel.Size = new System.Drawing.Size(160, 24);
            this.SingleChannel.TabIndex = 14;
            // 
            // SingleChannelLabel
            // 
            this.SingleChannelLabel.AutoSize = true;
            this.SingleChannelLabel.Location = new System.Drawing.Point(409, 7);
            this.SingleChannelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SingleChannelLabel.Name = "SingleChannelLabel";
            this.SingleChannelLabel.Size = new System.Drawing.Size(44, 17);
            this.SingleChannelLabel.TabIndex = 13;
            this.SingleChannelLabel.Text = "Track";
            // 
            // PercussionMode
            // 
            this.PercussionMode.AutoSize = true;
            this.PercussionMode.Checked = true;
            this.PercussionMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PercussionMode.Location = new System.Drawing.Point(145, 6);
            this.PercussionMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PercussionMode.Name = "PercussionMode";
            this.PercussionMode.Size = new System.Drawing.Size(228, 21);
            this.PercussionMode.TabIndex = 11;
            this.PercussionMode.Text = "Enable Percussion Mode ($BD)";
            this.PercussionMode.UseVisualStyleBackColor = true;
            this.PercussionMode.CheckedChanged += new System.EventHandler(this.PercussionMode_CheckedChanged);
            // 
            // GenerateVGMButton
            // 
            this.GenerateVGMButton.Location = new System.Drawing.Point(3, 1);
            this.GenerateVGMButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GenerateVGMButton.Name = "GenerateVGMButton";
            this.GenerateVGMButton.Size = new System.Drawing.Size(133, 28);
            this.GenerateVGMButton.TabIndex = 10;
            this.GenerateVGMButton.Text = "Generate VGM";
            this.GenerateVGMButton.UseVisualStyleBackColor = true;
            this.GenerateVGMButton.Click += new System.EventHandler(this.GenerateVGMButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.GeneratePanel);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.MIDIOutputText);
            this.Controls.Add(this.ReadFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "MIDI to VGM Conversion";
            this.GeneratePanel.ResumeLayout(false);
            this.GeneratePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MIDIOutputText;
        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Panel GeneratePanel;
        private System.Windows.Forms.ComboBox SingleChannel;
        private System.Windows.Forms.Label SingleChannelLabel;
        private System.Windows.Forms.CheckBox PercussionMode;
        private System.Windows.Forms.Button GenerateVGMButton;
    }
}

