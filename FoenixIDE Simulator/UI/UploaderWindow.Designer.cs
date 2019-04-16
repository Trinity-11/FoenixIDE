namespace FoenixIDE.UI
{
    partial class UploaderWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploaderWindow));
            this.ConnectButton = new System.Windows.Forms.Button();
            this.COMPortComboBox = new System.Windows.Forms.ComboBox();
            this.BrowseFileButton = new System.Windows.Forms.Button();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.FileSizeLabel = new System.Windows.Forms.Label();
            this.FileSizeResultLabel = new System.Windows.Forms.Label();
            this.LoadAddressLabel = new System.Windows.Forms.Label();
            this.SendBinaryButton = new System.Windows.Forms.Button();
            this.LoadAddressTextBox = new System.Windows.Forms.TextBox();
            this.DollarSignLabel = new System.Windows.Forms.Label();
            this.UploadProgressBar = new System.Windows.Forms.ProgressBar();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(17, 18);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(149, 29);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // COMPortComboBox
            // 
            this.COMPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.COMPortComboBox.Location = new System.Drawing.Point(178, 23);
            this.COMPortComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.COMPortComboBox.Name = "COMPortComboBox";
            this.COMPortComboBox.Size = new System.Drawing.Size(146, 24);
            this.COMPortComboBox.TabIndex = 1;
            // 
            // BrowseFileButton
            // 
            this.BrowseFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseFileButton.Location = new System.Drawing.Point(17, 57);
            this.BrowseFileButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Size = new System.Drawing.Size(149, 29);
            this.BrowseFileButton.TabIndex = 2;
            this.BrowseFileButton.Text = "Browse File";
            this.BrowseFileButton.UseVisualStyleBackColor = true;
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(178, 62);
            this.FileNameTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.ReadOnly = true;
            this.FileNameTextBox.Size = new System.Drawing.Size(433, 22);
            this.FileNameTextBox.TabIndex = 3;
            // 
            // FileSizeLabel
            // 
            this.FileSizeLabel.AutoSize = true;
            this.FileSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileSizeLabel.Location = new System.Drawing.Point(17, 98);
            this.FileSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(75, 17);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "File Size:";
            // 
            // FileSizeResultLabel
            // 
            this.FileSizeResultLabel.AutoSize = true;
            this.FileSizeResultLabel.Location = new System.Drawing.Point(177, 98);
            this.FileSizeResultLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeResultLabel.Name = "FileSizeResultLabel";
            this.FileSizeResultLabel.Size = new System.Drawing.Size(68, 17);
            this.FileSizeResultLabel.TabIndex = 5;
            this.FileSizeResultLabel.Text = "$00:0000";
            // 
            // LoadAddressLabel
            // 
            this.LoadAddressLabel.AutoSize = true;
            this.LoadAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadAddressLabel.Location = new System.Drawing.Point(17, 124);
            this.LoadAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LoadAddressLabel.Name = "LoadAddressLabel";
            this.LoadAddressLabel.Size = new System.Drawing.Size(113, 17);
            this.LoadAddressLabel.TabIndex = 6;
            this.LoadAddressLabel.Text = "Load Address:";
            // 
            // SendBinaryButton
            // 
            this.SendBinaryButton.Enabled = false;
            this.SendBinaryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendBinaryButton.Location = new System.Drawing.Point(17, 153);
            this.SendBinaryButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SendBinaryButton.Name = "SendBinaryButton";
            this.SendBinaryButton.Size = new System.Drawing.Size(149, 29);
            this.SendBinaryButton.TabIndex = 7;
            this.SendBinaryButton.Text = "Send Binary";
            this.SendBinaryButton.UseVisualStyleBackColor = true;
            this.SendBinaryButton.Click += new System.EventHandler(this.SendBinaryButton_Click);
            // 
            // LoadAddressTextBox
            // 
            this.LoadAddressTextBox.Enabled = false;
            this.LoadAddressTextBox.Location = new System.Drawing.Point(190, 122);
            this.LoadAddressTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoadAddressTextBox.Name = "LoadAddressTextBox";
            this.LoadAddressTextBox.Size = new System.Drawing.Size(93, 22);
            this.LoadAddressTextBox.TabIndex = 8;
            this.LoadAddressTextBox.Text = "00:0000";
            this.LoadAddressTextBox.TextChanged += new System.EventHandler(this.LoadAddressTextBox_TextChanged);
            // 
            // DollarSignLabel
            // 
            this.DollarSignLabel.AutoSize = true;
            this.DollarSignLabel.Location = new System.Drawing.Point(175, 124);
            this.DollarSignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DollarSignLabel.Name = "DollarSignLabel";
            this.DollarSignLabel.Size = new System.Drawing.Size(16, 17);
            this.DollarSignLabel.TabIndex = 9;
            this.DollarSignLabel.Text = "$";
            // 
            // UploadProgressBar
            // 
            this.UploadProgressBar.Location = new System.Drawing.Point(17, 196);
            this.UploadProgressBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.UploadProgressBar.Name = "UploadProgressBar";
            this.UploadProgressBar.Size = new System.Drawing.Size(593, 26);
            this.UploadProgressBar.TabIndex = 10;
            this.UploadProgressBar.Visible = false;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectButton.Location = new System.Drawing.Point(17, 18);
            this.DisconnectButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(149, 29);
            this.DisconnectButton.TabIndex = 11;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Visible = false;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // UploaderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(634, 238);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.UploadProgressBar);
            this.Controls.Add(this.LoadAddressTextBox);
            this.Controls.Add(this.SendBinaryButton);
            this.Controls.Add(this.LoadAddressLabel);
            this.Controls.Add(this.FileSizeResultLabel);
            this.Controls.Add(this.FileSizeLabel);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.BrowseFileButton);
            this.Controls.Add(this.COMPortComboBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.DollarSignLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploaderWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Uploader Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.ComboBox COMPortComboBox;
        private System.Windows.Forms.Button BrowseFileButton;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Label FileSizeLabel;
        private System.Windows.Forms.Label FileSizeResultLabel;
        private System.Windows.Forms.Label LoadAddressLabel;
        private System.Windows.Forms.Button SendBinaryButton;
        private System.Windows.Forms.TextBox LoadAddressTextBox;
        private System.Windows.Forms.Label DollarSignLabel;
        private System.Windows.Forms.ProgressBar UploadProgressBar;
        private System.Windows.Forms.Button DisconnectButton;
    }
}