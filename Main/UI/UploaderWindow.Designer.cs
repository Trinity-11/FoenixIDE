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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploaderWindow));
            this.ConnectButton = new System.Windows.Forms.Button();
            this.COMPortComboBox = new System.Windows.Forms.ComboBox();
            this.BrowseFileButton = new System.Windows.Forms.Button();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.FileSizeLabel = new System.Windows.Forms.Label();
            this.FileSizeResultLabel = new System.Windows.Forms.Label();
            this.DestinationAddressLabel = new System.Windows.Forms.Label();
            this.SendBinaryButton = new System.Windows.Forms.Button();
            this.C256DestAddress = new System.Windows.Forms.TextBox();
            this.DollarSignLabel = new System.Windows.Forms.Label();
            this.UploadProgressBar = new System.Windows.Forms.ProgressBar();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.SendFileRadio = new System.Windows.Forms.RadioButton();
            this.BlockSendRadio = new System.Windows.Forms.RadioButton();
            this.EmuSrcAddress = new System.Windows.Forms.TextBox();
            this.EmuSourceAddressLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EmuSrcSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FetchRadio = new System.Windows.Forms.RadioButton();
            this.C256SrcSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.C256SrcAddress = new System.Windows.Forms.TextBox();
            this.C256SrcAddressLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.DebugModeCheckbox = new System.Windows.Forms.CheckBox();
            this.ReflashCheckbox = new System.Windows.Forms.CheckBox();
            this.CountdownLabel = new System.Windows.Forms.Label();
            this.RevModeLabel = new System.Windows.Forms.Label();
            this.hideLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(15, 12);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(97, 24);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // COMPortComboBox
            // 
            this.COMPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.COMPortComboBox.Location = new System.Drawing.Point(116, 13);
            this.COMPortComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.COMPortComboBox.Name = "COMPortComboBox";
            this.COMPortComboBox.Size = new System.Drawing.Size(110, 21);
            this.COMPortComboBox.TabIndex = 1;
            // 
            // BrowseFileButton
            // 
            this.BrowseFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseFileButton.Location = new System.Drawing.Point(443, 47);
            this.BrowseFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Size = new System.Drawing.Size(24, 19);
            this.BrowseFileButton.TabIndex = 2;
            this.BrowseFileButton.Text = "...";
            this.BrowseFileButton.UseVisualStyleBackColor = true;
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Location = new System.Drawing.Point(116, 47);
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
            this.FileSizeLabel.Location = new System.Drawing.Point(356, 67);
            this.FileSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(59, 13);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "File Size:";
            // 
            // FileSizeResultLabel
            // 
            this.FileSizeResultLabel.AutoSize = true;
            this.FileSizeResultLabel.Location = new System.Drawing.Point(416, 67);
            this.FileSizeResultLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileSizeResultLabel.Name = "FileSizeResultLabel";
            this.FileSizeResultLabel.Size = new System.Drawing.Size(52, 13);
            this.FileSizeResultLabel.TabIndex = 5;
            this.FileSizeResultLabel.Text = "$00:0000";
            // 
            // DestinationAddressLabel
            // 
            this.DestinationAddressLabel.AutoSize = true;
            this.DestinationAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestinationAddressLabel.Location = new System.Drawing.Point(142, 108);
            this.DestinationAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DestinationAddressLabel.Name = "DestinationAddressLabel";
            this.DestinationAddressLabel.Size = new System.Drawing.Size(119, 13);
            this.DestinationAddressLabel.TabIndex = 6;
            this.DestinationAddressLabel.Text = "C256 Dest Address:";
            this.DestinationAddressLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SendBinaryButton
            // 
            this.SendBinaryButton.Enabled = false;
            this.SendBinaryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendBinaryButton.Location = new System.Drawing.Point(376, 12);
            this.SendBinaryButton.Margin = new System.Windows.Forms.Padding(2);
            this.SendBinaryButton.Name = "SendBinaryButton";
            this.SendBinaryButton.Size = new System.Drawing.Size(92, 24);
            this.SendBinaryButton.TabIndex = 7;
            this.SendBinaryButton.Text = "Send Binary";
            this.SendBinaryButton.UseVisualStyleBackColor = true;
            this.SendBinaryButton.Click += new System.EventHandler(this.SendBinaryButton_Click);
            // 
            // C256DestAddress
            // 
            this.C256DestAddress.Enabled = false;
            this.C256DestAddress.Location = new System.Drawing.Point(272, 105);
            this.C256DestAddress.Margin = new System.Windows.Forms.Padding(2);
            this.C256DestAddress.MaxLength = 7;
            this.C256DestAddress.Name = "C256DestAddress";
            this.C256DestAddress.Size = new System.Drawing.Size(71, 20);
            this.C256DestAddress.TabIndex = 8;
            this.C256DestAddress.Text = "00:0000";
            this.C256DestAddress.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            this.C256DestAddress.Leave += new System.EventHandler(this.BlockAddressTextBox_Leave);
            // 
            // DollarSignLabel
            // 
            this.DollarSignLabel.AutoSize = true;
            this.DollarSignLabel.Location = new System.Drawing.Point(260, 107);
            this.DollarSignLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DollarSignLabel.Name = "DollarSignLabel";
            this.DollarSignLabel.Size = new System.Drawing.Size(13, 13);
            this.DollarSignLabel.TabIndex = 9;
            this.DollarSignLabel.Text = "$";
            // 
            // UploadProgressBar
            // 
            this.UploadProgressBar.Location = new System.Drawing.Point(6, 160);
            this.UploadProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.UploadProgressBar.Name = "UploadProgressBar";
            this.UploadProgressBar.Size = new System.Drawing.Size(459, 24);
            this.UploadProgressBar.Step = 1;
            this.UploadProgressBar.TabIndex = 10;
            this.UploadProgressBar.Value = 100;
            this.UploadProgressBar.Visible = false;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisconnectButton.Location = new System.Drawing.Point(15, 12);
            this.DisconnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(97, 24);
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
            this.SendFileRadio.Location = new System.Drawing.Point(15, 47);
            this.SendFileRadio.Margin = new System.Windows.Forms.Padding(2);
            this.SendFileRadio.Name = "SendFileRadio";
            this.SendFileRadio.Size = new System.Drawing.Size(69, 17);
            this.SendFileRadio.TabIndex = 12;
            this.SendFileRadio.TabStop = true;
            this.SendFileRadio.Text = "Send File";
            this.SendFileRadio.UseVisualStyleBackColor = true;
            this.SendFileRadio.CheckedChanged += new System.EventHandler(this.SendFileRadio_CheckedChanged);
            // 
            // BlockSendRadio
            // 
            this.BlockSendRadio.AutoSize = true;
            this.BlockSendRadio.Location = new System.Drawing.Point(15, 82);
            this.BlockSendRadio.Margin = new System.Windows.Forms.Padding(2);
            this.BlockSendRadio.Name = "BlockSendRadio";
            this.BlockSendRadio.Size = new System.Drawing.Size(120, 17);
            this.BlockSendRadio.TabIndex = 13;
            this.BlockSendRadio.Text = "Send Memory Block";
            this.BlockSendRadio.UseVisualStyleBackColor = true;
            this.BlockSendRadio.CheckedChanged += new System.EventHandler(this.SendFileRadio_CheckedChanged);
            // 
            // EmuSrcAddress
            // 
            this.EmuSrcAddress.Enabled = false;
            this.EmuSrcAddress.Location = new System.Drawing.Point(272, 82);
            this.EmuSrcAddress.Margin = new System.Windows.Forms.Padding(2);
            this.EmuSrcAddress.MaxLength = 7;
            this.EmuSrcAddress.Name = "EmuSrcAddress";
            this.EmuSrcAddress.Size = new System.Drawing.Size(71, 20);
            this.EmuSrcAddress.TabIndex = 15;
            this.EmuSrcAddress.Text = "00:0000";
            this.EmuSrcAddress.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            this.EmuSrcAddress.Leave += new System.EventHandler(this.BlockAddressTextBox_Leave);
            // 
            // EmuSourceAddressLabel
            // 
            this.EmuSourceAddressLabel.AutoSize = true;
            this.EmuSourceAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmuSourceAddressLabel.Location = new System.Drawing.Point(154, 83);
            this.EmuSourceAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EmuSourceAddressLabel.Name = "EmuSourceAddressLabel";
            this.EmuSourceAddressLabel.Size = new System.Drawing.Size(107, 13);
            this.EmuSourceAddressLabel.TabIndex = 14;
            this.EmuSourceAddressLabel.Text = "Emu Src Address:";
            this.EmuSourceAddressLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "$";
            // 
            // EmuSrcSize
            // 
            this.EmuSrcSize.Enabled = false;
            this.EmuSrcSize.Location = new System.Drawing.Point(394, 82);
            this.EmuSrcSize.Margin = new System.Windows.Forms.Padding(2);
            this.EmuSrcSize.MaxLength = 7;
            this.EmuSrcSize.Name = "EmuSrcSize";
            this.EmuSrcSize.Size = new System.Drawing.Size(71, 20);
            this.EmuSrcSize.TabIndex = 18;
            this.EmuSrcSize.Text = "00:0000";
            this.EmuSrcSize.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            this.EmuSrcSize.Leave += new System.EventHandler(this.BlockAddressTextBox_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(346, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(382, 84);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "$";
            // 
            // FetchRadio
            // 
            this.FetchRadio.AutoSize = true;
            this.FetchRadio.Location = new System.Drawing.Point(15, 137);
            this.FetchRadio.Margin = new System.Windows.Forms.Padding(2);
            this.FetchRadio.Name = "FetchRadio";
            this.FetchRadio.Size = new System.Drawing.Size(122, 17);
            this.FetchRadio.TabIndex = 20;
            this.FetchRadio.Text = "Fetch Memory Block";
            this.FetchRadio.UseVisualStyleBackColor = true;
            this.FetchRadio.CheckedChanged += new System.EventHandler(this.SendFileRadio_CheckedChanged);
            // 
            // C256SrcSize
            // 
            this.C256SrcSize.Enabled = false;
            this.C256SrcSize.Location = new System.Drawing.Point(394, 135);
            this.C256SrcSize.Margin = new System.Windows.Forms.Padding(2);
            this.C256SrcSize.MaxLength = 7;
            this.C256SrcSize.Name = "C256SrcSize";
            this.C256SrcSize.Size = new System.Drawing.Size(71, 20);
            this.C256SrcSize.TabIndex = 25;
            this.C256SrcSize.Text = "00:0000";
            this.C256SrcSize.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            this.C256SrcSize.Leave += new System.EventHandler(this.BlockAddressTextBox_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(346, 138);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(382, 139);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "$";
            // 
            // C256SrcAddress
            // 
            this.C256SrcAddress.Enabled = false;
            this.C256SrcAddress.Location = new System.Drawing.Point(272, 135);
            this.C256SrcAddress.Margin = new System.Windows.Forms.Padding(2);
            this.C256SrcAddress.MaxLength = 7;
            this.C256SrcAddress.Name = "C256SrcAddress";
            this.C256SrcAddress.Size = new System.Drawing.Size(71, 20);
            this.C256SrcAddress.TabIndex = 22;
            this.C256SrcAddress.Text = "00:0000";
            this.C256SrcAddress.TextChanged += new System.EventHandler(this.AddressTextBox_TextChanged);
            this.C256SrcAddress.Leave += new System.EventHandler(this.BlockAddressTextBox_Leave);
            // 
            // C256SrcAddressLabel
            // 
            this.C256SrcAddressLabel.AutoSize = true;
            this.C256SrcAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C256SrcAddressLabel.Location = new System.Drawing.Point(149, 138);
            this.C256SrcAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.C256SrcAddressLabel.Name = "C256SrcAddressLabel";
            this.C256SrcAddressLabel.Size = new System.Drawing.Size(112, 13);
            this.C256SrcAddressLabel.TabIndex = 21;
            this.C256SrcAddressLabel.Text = "C256 Src Address:";
            this.C256SrcAddressLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(260, 137);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "$";
            // 
            // DebugModeCheckbox
            // 
            this.DebugModeCheckbox.AutoSize = true;
            this.DebugModeCheckbox.Checked = true;
            this.DebugModeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DebugModeCheckbox.Location = new System.Drawing.Point(15, 105);
            this.DebugModeCheckbox.Name = "DebugModeCheckbox";
            this.DebugModeCheckbox.Size = new System.Drawing.Size(88, 17);
            this.DebugModeCheckbox.TabIndex = 27;
            this.DebugModeCheckbox.Text = "Debug Mode";
            this.DebugModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // ReflashCheckbox
            // 
            this.ReflashCheckbox.AutoSize = true;
            this.ReflashCheckbox.Enabled = false;
            this.ReflashCheckbox.Location = new System.Drawing.Point(320, 17);
            this.ReflashCheckbox.Name = "ReflashCheckbox";
            this.ReflashCheckbox.Size = new System.Drawing.Size(51, 17);
            this.ReflashCheckbox.TabIndex = 28;
            this.ReflashCheckbox.Text = "Flash";
            this.ReflashCheckbox.UseVisualStyleBackColor = true;
            this.ReflashCheckbox.CheckedChanged += new System.EventHandler(this.SendFileRadio_CheckedChanged);
            // 
            // CountdownLabel
            // 
            this.CountdownLabel.BackColor = System.Drawing.Color.LightSeaGreen;
            this.CountdownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CountdownLabel.ForeColor = System.Drawing.Color.White;
            this.CountdownLabel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.CountdownLabel.Location = new System.Drawing.Point(10, 164);
            this.CountdownLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CountdownLabel.Name = "CountdownLabel";
            this.CountdownLabel.Size = new System.Drawing.Size(451, 16);
            this.CountdownLabel.TabIndex = 29;
            this.CountdownLabel.Text = "Erasing Flash";
            this.CountdownLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CountdownLabel.Visible = false;
            // 
            // RevModeLabel
            // 
            this.RevModeLabel.AutoSize = true;
            this.RevModeLabel.Location = new System.Drawing.Point(235, 18);
            this.RevModeLabel.Name = "RevModeLabel";
            this.RevModeLabel.Size = new System.Drawing.Size(67, 13);
            this.RevModeLabel.TabIndex = 30;
            this.RevModeLabel.Text = "Mode: RevB";
            // 
            // hideLabelTimer
            // 
            this.hideLabelTimer.Interval = 5000;
            this.hideLabelTimer.Tick += new System.EventHandler(this.HideLabelTimer_Tick);
            // 
            // UploaderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(472, 193);
            this.Controls.Add(this.RevModeLabel);
            this.Controls.Add(this.CountdownLabel);
            this.Controls.Add(this.ReflashCheckbox);
            this.Controls.Add(this.DebugModeCheckbox);
            this.Controls.Add(this.C256DestAddress);
            this.Controls.Add(this.C256SrcAddress);
            this.Controls.Add(this.EmuSrcAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DollarSignLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.C256SrcSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.C256SrcAddressLabel);
            this.Controls.Add(this.FetchRadio);
            this.Controls.Add(this.EmuSrcSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EmuSourceAddressLabel);
            this.Controls.Add(this.BlockSendRadio);
            this.Controls.Add(this.SendFileRadio);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.UploadProgressBar);
            this.Controls.Add(this.SendBinaryButton);
            this.Controls.Add(this.DestinationAddressLabel);
            this.Controls.Add(this.FileSizeResultLabel);
            this.Controls.Add(this.FileSizeLabel);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.BrowseFileButton);
            this.Controls.Add(this.COMPortComboBox);
            this.Controls.Add(this.ConnectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploaderWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Uploader Window";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UploaderWindow_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UploaderWindow_KeyDown);
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
        private System.Windows.Forms.Label DestinationAddressLabel;
        private System.Windows.Forms.Button SendBinaryButton;
        private System.Windows.Forms.TextBox C256DestAddress;
        private System.Windows.Forms.Label DollarSignLabel;
        private System.Windows.Forms.ProgressBar UploadProgressBar;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.RadioButton SendFileRadio;
        private System.Windows.Forms.RadioButton BlockSendRadio;
        private System.Windows.Forms.TextBox EmuSrcAddress;
        private System.Windows.Forms.Label EmuSourceAddressLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox EmuSrcSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton FetchRadio;
        private System.Windows.Forms.TextBox C256SrcSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox C256SrcAddress;
        private System.Windows.Forms.Label C256SrcAddressLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox DebugModeCheckbox;
        private System.Windows.Forms.CheckBox ReflashCheckbox;
        private System.Windows.Forms.Label CountdownLabel;
        private System.Windows.Forms.Label RevModeLabel;
        private System.Windows.Forms.Timer hideLabelTimer;
    }
}