namespace FoenixIDE.UI
{
    partial class GameGeneratorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameGeneratorForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CodeTextBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ViewAssetsButton = new System.Windows.Forms.Button();
            this.GenerateASMButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.CodeTextBox);
            this.panel1.Location = new System.Drawing.Point(3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 531);
            this.panel1.TabIndex = 0;
            // 
            // CodeTextBox
            // 
            this.CodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeTextBox.Location = new System.Drawing.Point(0, 0);
            this.CodeTextBox.Multiline = true;
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CodeTextBox.Size = new System.Drawing.Size(452, 531);
            this.CodeTextBox.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.CloseButton);
            this.panel2.Controls.Add(this.SaveButton);
            this.panel2.Controls.Add(this.LoadButton);
            this.panel2.Controls.Add(this.ViewAssetsButton);
            this.panel2.Controls.Add(this.GenerateASMButton);
            this.panel2.Location = new System.Drawing.Point(461, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(113, 531);
            this.panel2.TabIndex = 1;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CloseButton.Location = new System.Drawing.Point(9, 504);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(98, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(9, 107);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(98, 23);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save Game";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(9, 78);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(98, 23);
            this.LoadButton.TabIndex = 2;
            this.LoadButton.Text = "Load Game";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // ViewAssetsButton
            // 
            this.ViewAssetsButton.Location = new System.Drawing.Point(8, 37);
            this.ViewAssetsButton.Name = "ViewAssetsButton";
            this.ViewAssetsButton.Size = new System.Drawing.Size(98, 23);
            this.ViewAssetsButton.TabIndex = 1;
            this.ViewAssetsButton.Text = "View Assets";
            this.ViewAssetsButton.UseVisualStyleBackColor = true;
            this.ViewAssetsButton.Click += new System.EventHandler(this.ViewAssetsButton_Click);
            // 
            // GenerateASMButton
            // 
            this.GenerateASMButton.Location = new System.Drawing.Point(8, 8);
            this.GenerateASMButton.Name = "GenerateASMButton";
            this.GenerateASMButton.Size = new System.Drawing.Size(98, 23);
            this.GenerateASMButton.TabIndex = 0;
            this.GenerateASMButton.Text = "Generate ASM";
            this.GenerateASMButton.UseVisualStyleBackColor = true;
            this.GenerateASMButton.Click += new System.EventHandler(this.GenerateASMButton_ClickAsync);
            // 
            // GameGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 537);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(596, 576);
            this.Name = "GameGeneratorForm";
            this.Text = "Game Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameGeneratorForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox CodeTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button ViewAssetsButton;
        private System.Windows.Forms.Button GenerateASMButton;
    }
}

