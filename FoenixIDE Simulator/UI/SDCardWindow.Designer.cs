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
            this.CapacityText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Folder:";
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
            // CapacityText
            // 
            this.CapacityText.Enabled = false;
            this.CapacityText.Location = new System.Drawing.Point(68, 67);
            this.CapacityText.Margin = new System.Windows.Forms.Padding(2);
            this.CapacityText.MaxLength = 4;
            this.CapacityText.Name = "CapacityText";
            this.CapacityText.Size = new System.Drawing.Size(58, 20);
            this.CapacityText.TabIndex = 9;
            this.CapacityText.Text = "64";
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
            // SDCardWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(425, 129);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CapacityText);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CapacityText;
        private System.Windows.Forms.Label label3;
    }
}