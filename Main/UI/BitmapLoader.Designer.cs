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
            this.StoreButton = new System.Windows.Forms.Button();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.FileTypesCombo = new System.Windows.Forms.ComboBox();
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
            this.FileSizeLabel.Size = new System.Drawing.Size(59, 13);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "File Size:";
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
            this.LoadAddressLabel.Location = new System.Drawing.Point(259, 38);
            this.LoadAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LoadAddressLabel.Name = "LoadAddressLabel";
            this.LoadAddressLabel.Size = new System.Drawing.Size(88, 13);
            this.LoadAddressLabel.TabIndex = 6;
            this.LoadAddressLabel.Text = "Load Address:";
            // 
            // LoadAddressTextBox
            // 
            this.LoadAddressTextBox.Location = new System.Drawing.Point(389, 34);
            this.LoadAddressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.LoadAddressTextBox.Name = "LoadAddressTextBox";
            this.LoadAddressTextBox.Size = new System.Drawing.Size(71, 20);
            this.LoadAddressTextBox.TabIndex = 8;
            this.LoadAddressTextBox.Text = "B0:0000";
            // 
            // DollarSignLabel
            // 
            this.DollarSignLabel.AutoSize = true;
            this.DollarSignLabel.Location = new System.Drawing.Point(377, 35);
            this.DollarSignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DollarSignLabel.Name = "DollarSignLabel";
            this.DollarSignLabel.Size = new System.Drawing.Size(13, 13);
            this.DollarSignLabel.TabIndex = 9;
            this.DollarSignLabel.Text = "$";
            // 
            // StoreButton
            // 
            this.StoreButton.Enabled = false;
            this.StoreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoreButton.Location = new System.Drawing.Point(189, 110);
            this.StoreButton.Margin = new System.Windows.Forms.Padding(2);
            this.StoreButton.Name = "StoreButton";
            this.StoreButton.Size = new System.Drawing.Size(112, 24);
            this.StoreButton.TabIndex = 7;
            this.StoreButton.Text = "Store";
            this.StoreButton.UseVisualStyleBackColor = true;
            this.StoreButton.Click += new System.EventHandler(this.StoreButton_Click);
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeLabel.Location = new System.Drawing.Point(15, 70);
            this.TypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(39, 13);
            this.TypeLabel.TabIndex = 12;
            this.TypeLabel.Text = "Type:";
            // 
            // FileTypesCombo
            // 
            this.FileTypesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileTypesCombo.FormattingEnabled = true;
            this.FileTypesCombo.Location = new System.Drawing.Point(137, 67);
            this.FileTypesCombo.Margin = new System.Windows.Forms.Padding(2);
            this.FileTypesCombo.Name = "FileTypesCombo";
            this.FileTypesCombo.Size = new System.Drawing.Size(144, 21);
            this.FileTypesCombo.TabIndex = 13;
            this.FileTypesCombo.SelectedIndexChanged += new System.EventHandler(this.FileTypesCombo_SelectedIndexChanged);
            // 
            // LabelLUT
            // 
            this.LabelLUT.AutoSize = true;
            this.LabelLUT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelLUT.Location = new System.Drawing.Point(309, 70);
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
            this.LUTCombo.Location = new System.Drawing.Point(348, 67);
            this.LUTCombo.Margin = new System.Windows.Forms.Padding(2);
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
            this.Controls.Add(this.FileTypesCombo);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.LoadAddressTextBox);
            this.Controls.Add(this.StoreButton);
            this.Controls.Add(this.LoadAddressLabel);
            this.Controls.Add(this.FileSizeResultLabel);
            this.Controls.Add(this.FileSizeLabel);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.BrowseFileButton);
            this.Controls.Add(this.DollarSignLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BitmapLoader";
            this.Text = "Bitmap/Bin Loader Window";
            this.Load += new System.EventHandler(this.BitmapLoader_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BitmapLoader_KeyDown);
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
        private System.Windows.Forms.Button StoreButton;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ComboBox FileTypesCombo;
        private System.Windows.Forms.Label LabelLUT;
        private System.Windows.Forms.ComboBox LUTCombo;
    }
}