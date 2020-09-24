namespace FoenixIDE.Simulator.UI
{
    partial class SDCardWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDCardWindow));
            this.SDCardFolderText = new System.Windows.Forms.TextBox();
            this.FolderSelectButton = new System.Windows.Forms.Button();
            this.SDCardEnabled = new System.Windows.Forms.CheckBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CapacityCombo = new System.Windows.Forms.ComboBox();
            this.Iso_selection = new System.Windows.Forms.CheckBox();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ClusterCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FSTypeCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SDCardFolderText
            // 
            this.SDCardFolderText.Location = new System.Drawing.Point(68, 44);
            this.SDCardFolderText.Margin = new System.Windows.Forms.Padding(2);
            this.SDCardFolderText.Name = "SDCardFolderText";
            this.SDCardFolderText.ReadOnly = true;
            this.SDCardFolderText.Size = new System.Drawing.Size(314, 20);
            this.SDCardFolderText.TabIndex = 5;
            // 
            // FolderSelectButton
            // 
            this.FolderSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderSelectButton.Location = new System.Drawing.Point(386, 44);
            this.FolderSelectButton.Margin = new System.Windows.Forms.Padding(2);
            this.FolderSelectButton.Name = "FolderSelectButton";
            this.FolderSelectButton.Size = new System.Drawing.Size(24, 19);
            this.FolderSelectButton.TabIndex = 4;
            this.FolderSelectButton.Text = "...";
            this.FolderSelectButton.UseVisualStyleBackColor = true;
            this.FolderSelectButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // SDCardEnabled
            // 
            this.SDCardEnabled.AutoSize = true;
            this.SDCardEnabled.Location = new System.Drawing.Point(12, 12);
            this.SDCardEnabled.Name = "SDCardEnabled";
            this.SDCardEnabled.Size = new System.Drawing.Size(102, 17);
            this.SDCardEnabled.TabIndex = 6;
            this.SDCardEnabled.Text = "Enable SD Card";
            this.SDCardEnabled.UseVisualStyleBackColor = true;
            this.SDCardEnabled.CheckedChanged += new System.EventHandler(this.SDCardEnabled_CheckedChanged);
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(12, 47);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(39, 13);
            this.TypeLabel.TabIndex = 7;
            this.TypeLabel.Text = "Folder:";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(176, 94);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 8;
            this.ButtonClose.Text = "&Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Capacity:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "MB";
            // 
            // CapacityCombo
            // 
            this.CapacityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CapacityCombo.FormattingEnabled = true;
            this.CapacityCombo.Items.AddRange(new object[] {
            "8",
            "16",
            "32",
            "64",
            "256",
            "512",
            "1024",
            "2048"});
            this.CapacityCombo.Location = new System.Drawing.Point(68, 67);
            this.CapacityCombo.Name = "CapacityCombo";
            this.CapacityCombo.Size = new System.Drawing.Size(56, 21);
            this.CapacityCombo.TabIndex = 12;
            this.CapacityCombo.SelectedIndexChanged += new System.EventHandler(this.CapacityCombo_SelectedIndexChanged);
            // 
            // Iso_selection
            // 
            this.Iso_selection.AutoSize = true;
            this.Iso_selection.Location = new System.Drawing.Point(142, 12);
            this.Iso_selection.Margin = new System.Windows.Forms.Padding(6);
            this.Iso_selection.Name = "Iso_selection";
            this.Iso_selection.Size = new System.Drawing.Size(110, 17);
            this.Iso_selection.TabIndex = 13;
            this.Iso_selection.Text = "Enable ISO Mode";
            this.Iso_selection.UseVisualStyleBackColor = true;
            this.Iso_selection.CheckedChanged += new System.EventHandler(this.Iso_selection_CheckedChanged);
            // 
            // FileDialog
            // 
            this.FileDialog.Filter = "ISO Files|*.iso|Image Files|*.img";
            this.FileDialog.Title = "Select an Image or ISO File";
            // 
            // ClusterCombo
            // 
            this.ClusterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClusterCombo.FormattingEnabled = true;
            this.ClusterCombo.Items.AddRange(new object[] {
            "512",
            "1024",
            "2048",
            "4096",
            "8192"});
            this.ClusterCombo.Location = new System.Drawing.Point(219, 68);
            this.ClusterCombo.Name = "ClusterCombo";
            this.ClusterCombo.Size = new System.Drawing.Size(56, 21);
            this.ClusterCombo.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(173, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Cluster:";
            // 
            // FSTypeCombo
            // 
            this.FSTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FSTypeCombo.FormattingEnabled = true;
            this.FSTypeCombo.Items.AddRange(new object[] {
            "FAT12",
            "FAT16",
            "FAT32"});
            this.FSTypeCombo.Location = new System.Drawing.Point(354, 67);
            this.FSTypeCombo.Name = "FSTypeCombo";
            this.FSTypeCombo.Size = new System.Drawing.Size(56, 21);
            this.FSTypeCombo.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(314, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Type:";
            // 
            // SDCardWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(425, 129);
            this.Controls.Add(this.FSTypeCombo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ClusterCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Iso_selection);
            this.Controls.Add(this.CapacityCombo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.SDCardEnabled);
            this.Controls.Add(this.SDCardFolderText);
            this.Controls.Add(this.FolderSelectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SDCardWindow";
            this.Text = "SD Card";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SDCardFolderText;
        private System.Windows.Forms.Button FolderSelectButton;
        private System.Windows.Forms.CheckBox SDCardEnabled;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CapacityCombo;
        private System.Windows.Forms.CheckBox Iso_selection;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.ComboBox ClusterCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox FSTypeCombo;
        private System.Windows.Forms.Label label4;
    }
}