namespace vgm_reader
{
    partial class VGMForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VGMForm));
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.AY38910Text = new System.Windows.Forms.TextBox();
            this.FileLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(12, 12);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(75, 23);
            this.ReadFileButton.TabIndex = 0;
            this.ReadFileButton.Text = "Read File";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
            // 
            // AY38910Text
            // 
            this.AY38910Text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AY38910Text.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AY38910Text.Location = new System.Drawing.Point(12, 41);
            this.AY38910Text.MaxLength = 65536;
            this.AY38910Text.Multiline = true;
            this.AY38910Text.Name = "AY38910Text";
            this.AY38910Text.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.AY38910Text.Size = new System.Drawing.Size(564, 463);
            this.AY38910Text.TabIndex = 1;
            this.AY38910Text.WordWrap = false;
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(93, 17);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(0, 13);
            this.FileLabel.TabIndex = 2;
            // 
            // VGMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 516);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.AY38910Text);
            this.Controls.Add(this.ReadFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VGMForm";
            this.Text = "VGM Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.TextBox AY38910Text;
        private System.Windows.Forms.Label FileLabel;
    }
}

