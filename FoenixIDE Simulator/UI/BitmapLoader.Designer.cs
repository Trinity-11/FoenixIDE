namespace FoenixIDE.UI
{
    partial class BitmapLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BitmapLoader));
            this.BrowseFileButton = new System.Windows.Forms.Button();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.FileSizeLabel = new System.Windows.Forms.Label();
            this.FileSizeResultLabel = new System.Windows.Forms.Label();
            this.LoadAddressLabel = new System.Windows.Forms.Label();
            this.LoadAddressTextBox = new System.Windows.Forms.TextBox();
            this.DollarSignLabel = new System.Windows.Forms.Label();
            this.BitmapSizeLabel = new System.Windows.Forms.Label();
            this.BitmapSizeValueLabel = new System.Windows.Forms.Label();
            this.StoreButton = new System.Windows.Forms.Button();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.BitmapTypesCombo = new System.Windows.Forms.ComboBox();
            this.PixelDepthLabel = new System.Windows.Forms.Label();
            this.PixelDepthValueLabel = new System.Windows.Forms.Label();
            this.LabelLUT = new System.Windows.Forms.Label();
            this.LUTCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // BrowseFileButton
            // 
            this.BrowseFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseFileButton.Location = new System.Drawing.Point(13, 5);
            this.BrowseFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Size = new System.Drawing.Size(112, 24);
            this.BrowseFileButton.TabIndex = 2;
            this.BrowseFileButton.Text = "Browse File";
            this.BrowseFileButton.UseVisualStyleBackColor = true;
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(134, 9);
            this.FileNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.ReadOnly = true;
            this.FileNameTextBox.Size = new System.Drawing.Size(326, 20);
            this.FileNameTextBox.TabIndex = 3;
            // 
            // FileSizeLabel
            // 
            this.FileSizeLabel.AutoSize = true;
            this.FileSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileSizeLabel.Location = new System.Drawing.Point(11, 37);
            this.FileSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(73, 13);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "Image Size:";
            // 
            // FileSizeResultLabel
            // 
            this.FileSizeResultLabel.AutoSize = true;
            this.FileSizeResultLabel.Location = new System.Drawing.Point(131, 37);
            this.FileSizeResultLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeResultLabel.Name = "FileSizeResultLabel";
            this.FileSizeResultLabel.Size = new System.Drawing.Size(52, 13);
            this.FileSizeResultLabel.TabIndex = 5;
            this.FileSizeResultLabel.Text = "$00:0000";
            // 
            // LoadAddressLabel
            // 
            this.LoadAddressLabel.AutoSize = true;
            this.LoadAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadAddressLabel.Location = new System.Drawing.Point(12, 57);
            this.LoadAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LoadAddressLabel.Name = "LoadAddressLabel";
            this.LoadAddressLabel.Size = new System.Drawing.Size(88, 13);
            this.LoadAddressLabel.TabIndex = 6;
            this.LoadAddressLabel.Text = "Load Address:";
            // 
            // LoadAddressTextBox
            // 
            this.LoadAddressTextBox.Location = new System.Drawing.Point(142, 53);
            this.LoadAddressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.LoadAddressTextBox.Name = "LoadAddressTextBox";
            this.LoadAddressTextBox.Size = new System.Drawing.Size(71, 20);
            this.LoadAddressTextBox.TabIndex = 8;
            this.LoadAddressTextBox.Text = "B0:0000";
            this.LoadAddressTextBox.TextChanged += new System.EventHandler(this.LoadAddressTextBox_TextChanged);
            // 
            // DollarSignLabel
            // 
            this.DollarSignLabel.AutoSize = true;
            this.DollarSignLabel.Location = new System.Drawing.Point(130, 54);
            this.DollarSignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DollarSignLabel.Name = "DollarSignLabel";
            this.DollarSignLabel.Size = new System.Drawing.Size(13, 13);
            this.DollarSignLabel.TabIndex = 9;
            this.DollarSignLabel.Text = "$";
            // 
            // BitmapSizeLabel
            // 
            this.BitmapSizeLabel.AutoSize = true;
            this.BitmapSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BitmapSizeLabel.Location = new System.Drawing.Point(230, 37);
            this.BitmapSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BitmapSizeLabel.Name = "BitmapSizeLabel";
            this.BitmapSizeLabel.Size = new System.Drawing.Size(77, 13);
            this.BitmapSizeLabel.TabIndex = 10;
            this.BitmapSizeLabel.Text = "Bitmap Size:";
            // 
            // BitmapSizeValueLabel
            // 
            this.BitmapSizeValueLabel.AutoSize = true;
            this.BitmapSizeValueLabel.Location = new System.Drawing.Point(306, 37);
            this.BitmapSizeValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BitmapSizeValueLabel.Name = "BitmapSizeValueLabel";
            this.BitmapSizeValueLabel.Size = new System.Drawing.Size(30, 13);
            this.BitmapSizeValueLabel.TabIndex = 11;
            this.BitmapSizeValueLabel.Text = "0 x 0";
            // 
            // StoreButton
            // 
            this.StoreButton.Enabled = false;
            this.StoreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoreButton.Location = new System.Drawing.Point(13, 110);
            this.StoreButton.Margin = new System.Windows.Forms.Padding(2);
            this.StoreButton.Name = "StoreButton";
            this.StoreButton.Size = new System.Drawing.Size(112, 24);
            this.StoreButton.TabIndex = 7;
            this.StoreButton.Text = "Store";
            this.StoreButton.UseVisualStyleBackColor = true;
            this.StoreButton.Click += new System.EventHandler(this.StoreButton_Click);
            // 
            // label3
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeLabel.Location = new System.Drawing.Point(12, 78);
            this.TypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(39, 13);
            this.TypeLabel.TabIndex = 12;
            this.TypeLabel.Text = "Type:";
            // 
            // BitmapTypesCombo
            // 
            this.BitmapTypesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BitmapTypesCombo.FormattingEnabled = true;
            this.BitmapTypesCombo.Location = new System.Drawing.Point(134, 78);
            this.BitmapTypesCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BitmapTypesCombo.Name = "BitmapTypesCombo";
            this.BitmapTypesCombo.Size = new System.Drawing.Size(144, 21);
            this.BitmapTypesCombo.TabIndex = 13;
            this.BitmapTypesCombo.SelectedIndexChanged += new System.EventHandler(this.BitmapTypesCombo_SelectedIndexChanged);
            // 
            // PixelDepthLabel
            // 
            this.PixelDepthLabel.AutoSize = true;
            this.PixelDepthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PixelDepthLabel.Location = new System.Drawing.Point(357, 37);
            this.PixelDepthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PixelDepthLabel.Name = "PixelDepthLabel";
            this.PixelDepthLabel.Size = new System.Drawing.Size(76, 13);
            this.PixelDepthLabel.TabIndex = 14;
            this.PixelDepthLabel.Text = "Pixel Depth:";
            // 
            // PixelDepthValueLabel
            // 
            this.PixelDepthValueLabel.AutoSize = true;
            this.PixelDepthValueLabel.Location = new System.Drawing.Point(430, 37);
            this.PixelDepthValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PixelDepthValueLabel.Name = "PixelDepthValueLabel";
            this.PixelDepthValueLabel.Size = new System.Drawing.Size(13, 13);
            this.PixelDepthValueLabel.TabIndex = 15;
            this.PixelDepthValueLabel.Text = "0";
            // 
            // label1
            // 
            this.LabelLUT.AutoSize = true;
            this.LabelLUT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelLUT.Location = new System.Drawing.Point(306, 78);
            this.LabelLUT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelLUT.Name = "LabelLUT";
            this.LabelLUT.Size = new System.Drawing.Size(35, 13);
            this.LabelLUT.TabIndex = 16;
            this.LabelLUT.Text = "LUT:";
            // 
            // LUTCombo
            // 
            this.LUTCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LUTCombo.FormattingEnabled = true;
            this.LUTCombo.Location = new System.Drawing.Point(342, 78);
            this.LUTCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LUTCombo.Name = "LUTCombo";
            this.LUTCombo.Size = new System.Drawing.Size(117, 21);
            this.LUTCombo.TabIndex = 17;
            // 
            // BitmapLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 145);
            this.Controls.Add(this.LUTCombo);
            this.Controls.Add(this.LabelLUT);
            this.Controls.Add(this.PixelDepthValueLabel);
            this.Controls.Add(this.PixelDepthLabel);
            this.Controls.Add(this.BitmapTypesCombo);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.BitmapSizeValueLabel);
            this.Controls.Add(this.BitmapSizeLabel);
            this.Controls.Add(this.LoadAddressTextBox);
            this.Controls.Add(this.StoreButton);
            this.Controls.Add(this.LoadAddressLabel);
            this.Controls.Add(this.FileSizeResultLabel);
            this.Controls.Add(this.FileSizeLabel);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.BrowseFileButton);
            this.Controls.Add(this.DollarSignLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BitmapLoader";
            this.Text = "Bitmap Loader Window";
            this.Load += new System.EventHandler(this.BitmapLoader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BrowseFileButton;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Label FileSizeLabel;
        private System.Windows.Forms.Label FileSizeResultLabel;
        private System.Windows.Forms.Label LoadAddressLabel;
        private System.Windows.Forms.TextBox LoadAddressTextBox;
        private System.Windows.Forms.Label DollarSignLabel;
        private System.Windows.Forms.Label BitmapSizeLabel;
        private System.Windows.Forms.Label BitmapSizeValueLabel;
        private System.Windows.Forms.Button StoreButton;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ComboBox BitmapTypesCombo;
        private System.Windows.Forms.Label PixelDepthLabel;
        private System.Windows.Forms.Label PixelDepthValueLabel;
        private System.Windows.Forms.Label LabelLUT;
        private System.Windows.Forms.ComboBox LUTCombo;
    }
}