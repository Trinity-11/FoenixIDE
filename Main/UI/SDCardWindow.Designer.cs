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
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CapacityCombo = new System.Windows.Forms.ComboBox();
            this.Iso_sellection = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SDCardFolderText
            // 
            this.SDCardFolderText.Location = new System.Drawing.Point(125, 81);
            this.SDCardFolderText.Margin = new System.Windows.Forms.Padding(4);
            this.SDCardFolderText.Name = "SDCardFolderText";
            this.SDCardFolderText.ReadOnly = true;
            this.SDCardFolderText.Size = new System.Drawing.Size(572, 29);
            this.SDCardFolderText.TabIndex = 5;
            this.SDCardFolderText.Text = "D:\\\\Old_PC\\\\C256\\\\FMX\\\\SD_IDE_CONTENT";
            // 
            // FolderSelectButton
            // 
            this.FolderSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderSelectButton.Location = new System.Drawing.Point(708, 81);
            this.FolderSelectButton.Margin = new System.Windows.Forms.Padding(4);
            this.FolderSelectButton.Name = "FolderSelectButton";
            this.FolderSelectButton.Size = new System.Drawing.Size(44, 35);
            this.FolderSelectButton.TabIndex = 4;
            this.FolderSelectButton.Text = "...";
            this.FolderSelectButton.UseVisualStyleBackColor = true;
            this.FolderSelectButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // SDCardEnabled
            // 
            this.SDCardEnabled.AutoSize = true;
            this.SDCardEnabled.Checked = true;
            this.SDCardEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SDCardEnabled.Location = new System.Drawing.Point(22, 22);
            this.SDCardEnabled.Margin = new System.Windows.Forms.Padding(6);
            this.SDCardEnabled.Name = "SDCardEnabled";
            this.SDCardEnabled.Size = new System.Drawing.Size(180, 29);
            this.SDCardEnabled.TabIndex = 6;
            this.SDCardEnabled.Text = "Enable SD Card";
            this.SDCardEnabled.UseVisualStyleBackColor = true;
            this.SDCardEnabled.CheckedChanged += new System.EventHandler(this.SDCardEnabled_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Folder:";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(323, 174);
            this.ButtonClose.Margin = new System.Windows.Forms.Padding(6);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(138, 42);
            this.ButtonClose.TabIndex = 8;
            this.ButtonClose.Text = "&Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Capacity:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 131);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 25);
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
            "256"});
            this.CapacityCombo.Location = new System.Drawing.Point(125, 124);
            this.CapacityCombo.Margin = new System.Windows.Forms.Padding(6);
            this.CapacityCombo.Name = "CapacityCombo";
            this.CapacityCombo.Size = new System.Drawing.Size(99, 32);
            this.CapacityCombo.TabIndex = 12;
            // 
            // Iso_sellection
            // 
            this.Iso_sellection.AutoSize = true;
            this.Iso_sellection.Checked = true;
            this.Iso_sellection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Iso_sellection.Location = new System.Drawing.Point(376, 22);
            this.Iso_sellection.Margin = new System.Windows.Forms.Padding(6);
            this.Iso_sellection.Name = "Iso_sellection";
            this.Iso_sellection.Size = new System.Drawing.Size(193, 29);
            this.Iso_sellection.TabIndex = 13;
            this.Iso_sellection.Text = "Enable ISO mode";
            this.Iso_sellection.UseVisualStyleBackColor = true;
            this.Iso_sellection.CheckedChanged += new System.EventHandler(this.Iso_sellection_CheckedChanged);
            // 
            // SDCardWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(779, 238);
            this.Controls.Add(this.Iso_sellection);
            this.Controls.Add(this.CapacityCombo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SDCardEnabled);
            this.Controls.Add(this.SDCardFolderText);
            this.Controls.Add(this.FolderSelectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "SDCardWindow";
            this.Text = "SD Card";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SDCardFolderText;
        private System.Windows.Forms.Button FolderSelectButton;
        private System.Windows.Forms.CheckBox SDCardEnabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CapacityCombo;
        private System.Windows.Forms.CheckBox Iso_sellection;
    }
}