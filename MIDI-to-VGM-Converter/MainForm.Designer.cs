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
            this.GenerateVGMButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MIDIOutputText
            // 
            this.MIDIOutputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutputText.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MIDIOutputText.Location = new System.Drawing.Point(12, 35);
            this.MIDIOutputText.MaxLength = 65536;
            this.MIDIOutputText.Multiline = true;
            this.MIDIOutputText.Name = "MIDIOutputText";
            this.MIDIOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MIDIOutputText.Size = new System.Drawing.Size(776, 403);
            this.MIDIOutputText.TabIndex = 3;
            this.MIDIOutputText.WordWrap = false;
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(12, 6);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(75, 23);
            this.ReadFileButton.TabIndex = 2;
            this.ReadFileButton.Text = "Read File";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(93, 11);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(0, 13);
            this.FileLabel.TabIndex = 4;
            // 
            // GenerateVGMButton
            // 
            this.GenerateVGMButton.Enabled = false;
            this.GenerateVGMButton.Location = new System.Drawing.Point(688, 6);
            this.GenerateVGMButton.Name = "GenerateVGMButton";
            this.GenerateVGMButton.Size = new System.Drawing.Size(100, 23);
            this.GenerateVGMButton.TabIndex = 5;
            this.GenerateVGMButton.Text = "Generate VGM";
            this.GenerateVGMButton.UseVisualStyleBackColor = true;
            this.GenerateVGMButton.Click += new System.EventHandler(this.GenerateVGMButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GenerateVGMButton);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.MIDIOutputText);
            this.Controls.Add(this.ReadFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MIDI to VGM Conversion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MIDIOutputText;
        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Button GenerateVGMButton;
    }
}

