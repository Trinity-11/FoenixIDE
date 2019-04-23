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
            this.SendFileRadio = new System.Windows.Forms.RadioButton();
            this.BlockSendRadio = new System.Windows.Forms.RadioButton();
            this.BlockAddressTextBox = new System.Windows.Forms.TextBox();
            this.BlockAddressLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BlockSizeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(17, 18);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(2);
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
            this.COMPortComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.COMPortComboBox.Name = "COMPortComboBox";
            this.COMPortComboBox.Size = new System.Drawing.Size(146, 24);
            this.COMPortComboBox.TabIndex = 1;
            // 
            // BrowseFileButton
            // 
            this.BrowseFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseFileButton.Location = new System.Drawing.Point(591, 58);
            this.BrowseFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Size = new System.Drawing.Size(32, 23);
            this.BrowseFileButton.TabIndex = 2;
            this.BrowseFileButton.Text = "...";
            this.BrowseFileButton.UseVisualStyleBackColor = true;
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(154, 58);
            this.FileNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.ReadOnly = true;
            this.FileNameTextBox.Size = new System.Drawing.Size(433, 22);
            this.FileNameTextBox.TabIndex = 3;
            // 
            // FileSizeLabel
            // 
            this.FileSizeLabel.AutoSize = true;
            this.FileSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileSizeLabel.Location = new System.Drawing.Point(151, 82);
            this.FileSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(75, 17);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "File Size:";
            // 
            // FileSizeResultLabel
            // 
            this.FileSizeResultLabel.AutoSize = true;
            this.FileSizeResultLabel.Location = new System.Drawing.Point(311, 82);
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
            this.LoadAddressLabel.Location = new System.Drawing.Point(15, 154);
            this.LoadAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LoadAddressLabel.Name = "LoadAddressLabel";
            this.LoadAddressLabel.Size = new System.Drawing.Size(159, 17);
            this.LoadAddressLabel.TabIndex = 6;
            this.LoadAddressLabel.Text = "Destination Address:";
            // 
            // SendBinaryButton
            // 
            this.SendBinaryButton.Enabled = false;
            this.SendBinaryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendBinaryButton.Location = new System.Drawing.Point(474, 152);
            this.SendBinaryButton.Margin = new System.Windows.Forms.Padding(2);
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
            this.LoadAddressTextBox.Location = new System.Drawing.Point(193, 152);
            this.LoadAddressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.LoadAddressTextBox.Name = "LoadAddressTextBox";
            this.LoadAddressTextBox.Size = new System.Drawing.Size(93, 22);
            this.LoadAddressTextBox.TabIndex = 8;
            this.LoadAddressTextBox.Text = "00:0000";
            this.LoadAddressTextBox.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            // 
            // DollarSignLabel
            // 
            this.DollarSignLabel.AutoSize = true;
            this.DollarSignLabel.Location = new System.Drawing.Point(178, 154);
            this.DollarSignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DollarSignLabel.Name = "DollarSignLabel";
            this.DollarSignLabel.Size = new System.Drawing.Size(16, 17);
            this.DollarSignLabel.TabIndex = 9;
            this.DollarSignLabel.Text = "$";
            // 
            // UploadProgressBar
            // 
            this.UploadProgressBar.Location = new System.Drawing.Point(11, 196);
            this.UploadProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.UploadProgressBar.Name = "UploadProgressBar";
            this.UploadProgressBar.Size = new System.Drawing.Size(612, 26);
            this.UploadProgressBar.TabIndex = 10;
            this.UploadProgressBar.Visible = false;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectButton.Location = new System.Drawing.Point(17, 18);
            this.DisconnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(149, 29);
            this.DisconnectButton.TabIndex = 11;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Visible = false;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // SendFileRadio
            // 
            this.SendFileRadio.AutoSize = true;
            this.SendFileRadio.Checked = true;
            this.SendFileRadio.Location = new System.Drawing.Point(20, 58);
            this.SendFileRadio.Name = "SendFileRadio";
            this.SendFileRadio.Size = new System.Drawing.Size(88, 21);
            this.SendFileRadio.TabIndex = 12;
            this.SendFileRadio.TabStop = true;
            this.SendFileRadio.Text = "Send File";
            this.SendFileRadio.UseVisualStyleBackColor = true;
            this.SendFileRadio.CheckedChanged += new System.EventHandler(this.SendFileRadio_CheckedChanged);
            // 
            // BlockSendRadio
            // 
            this.BlockSendRadio.AutoSize = true;
            this.BlockSendRadio.Location = new System.Drawing.Point(20, 116);
            this.BlockSendRadio.Name = "BlockSendRadio";
            this.BlockSendRadio.Size = new System.Drawing.Size(154, 21);
            this.BlockSendRadio.TabIndex = 13;
            this.BlockSendRadio.Text = "Send Memory Block";
            this.BlockSendRadio.UseVisualStyleBackColor = true;
            this.BlockSendRadio.CheckedChanged += new System.EventHandler(this.SendFileRadio_CheckedChanged);
            // 
            // BlockAddressTextBox
            // 
            this.BlockAddressTextBox.Enabled = false;
            this.BlockAddressTextBox.Location = new System.Drawing.Point(335, 117);
            this.BlockAddressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.BlockAddressTextBox.Name = "BlockAddressTextBox";
            this.BlockAddressTextBox.Size = new System.Drawing.Size(93, 22);
            this.BlockAddressTextBox.TabIndex = 15;
            this.BlockAddressTextBox.Text = "00:0000";
            this.BlockAddressTextBox.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            // 
            // BlockAddressLabel
            // 
            this.BlockAddressLabel.AutoSize = true;
            this.BlockAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlockAddressLabel.Location = new System.Drawing.Point(197, 119);
            this.BlockAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BlockAddressLabel.Name = "BlockAddressLabel";
            this.BlockAddressLabel.Size = new System.Drawing.Size(116, 17);
            this.BlockAddressLabel.TabIndex = 14;
            this.BlockAddressLabel.Text = "Block Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "$";
            // 
            // BlockSizeTextBox
            // 
            this.BlockSizeTextBox.Enabled = false;
            this.BlockSizeTextBox.Location = new System.Drawing.Point(530, 117);
            this.BlockSizeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.BlockSizeTextBox.Name = "BlockSizeTextBox";
            this.BlockSizeTextBox.Size = new System.Drawing.Size(93, 22);
            this.BlockSizeTextBox.TabIndex = 18;
            this.BlockSizeTextBox.Text = "00:0000";
            this.BlockSizeTextBox.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(467, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(515, 119);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "$";
            // 
            // UploaderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(634, 238);
            this.Controls.Add(this.BlockSizeTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BlockAddressTextBox);
            this.Controls.Add(this.BlockAddressLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BlockSendRadio);
            this.Controls.Add(this.SendFileRadio);
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
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.RadioButton SendFileRadio;
        private System.Windows.Forms.RadioButton BlockSendRadio;
        private System.Windows.Forms.TextBox BlockAddressTextBox;
        private System.Windows.Forms.Label BlockAddressLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BlockSizeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}