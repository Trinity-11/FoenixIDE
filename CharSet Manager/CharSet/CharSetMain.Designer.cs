namespace CharSet
{
    partial class CharSetMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AsciiMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.nonPetCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PetsciiUGMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PetsciiULMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aNSIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.charViewer1 = new CharSet.CharViewer();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.generateToolStripMenuItem,
            this.sizeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(11, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(2107, 42);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(288, 34);
            this.openToolStripMenuItem.Text = "&Open Character File";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(288, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(288, 34);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // generateToolStripMenuItem
            // 
            this.generateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.AsciiMenu,
            this.nonPetCharactersToolStripMenuItem,
            this.PetsciiUGMenu,
            this.PetsciiULMenu,
            this.aNSIToolStripMenuItem,
            this.copyAllToolStripMenuItem});
            this.generateToolStripMenuItem.Name = "generateToolStripMenuItem";
            this.generateToolStripMenuItem.Size = new System.Drawing.Size(109, 34);
            this.generateToolStripMenuItem.Text = "Generate";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // AsciiMenu
            // 
            this.AsciiMenu.Name = "AsciiMenu";
            this.AsciiMenu.Size = new System.Drawing.Size(285, 34);
            this.AsciiMenu.Text = "ASCII ORDER";
            this.AsciiMenu.Click += new System.EventHandler(this.AsciiMenu_Click);
            // 
            // nonPetCharactersToolStripMenuItem
            // 
            this.nonPetCharactersToolStripMenuItem.Name = "nonPetCharactersToolStripMenuItem";
            this.nonPetCharactersToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            this.nonPetCharactersToolStripMenuItem.Text = "Non-Pet Characters";
            this.nonPetCharactersToolStripMenuItem.Click += new System.EventHandler(this.nonPetCharactersToolStripMenuItem_Click);
            // 
            // PetsciiUGMenu
            // 
            this.PetsciiUGMenu.Name = "PetsciiUGMenu";
            this.PetsciiUGMenu.Size = new System.Drawing.Size(285, 34);
            this.PetsciiUGMenu.Text = "PETSCII UG";
            this.PetsciiUGMenu.Click += new System.EventHandler(this.pETSCIIORDERToolStripMenuItem_Click);
            // 
            // PetsciiULMenu
            // 
            this.PetsciiULMenu.Name = "PetsciiULMenu";
            this.PetsciiULMenu.Size = new System.Drawing.Size(285, 34);
            this.PetsciiULMenu.Text = "PETSCII UL";
            // 
            // aNSIToolStripMenuItem
            // 
            this.aNSIToolStripMenuItem.Name = "aNSIToolStripMenuItem";
            this.aNSIToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            this.aNSIToolStripMenuItem.Text = "ANSI";
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(285, 34);
            this.copyAllToolStripMenuItem.Text = "Copy All";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.copyAllToolStripMenuItem_Click);
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x8ToolStripMenuItem,
            this.x16ToolStripMenuItem});
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(62, 34);
            this.sizeToolStripMenuItem.Text = "Size";
            // 
            // x8ToolStripMenuItem
            // 
            this.x8ToolStripMenuItem.Name = "x8ToolStripMenuItem";
            this.x8ToolStripMenuItem.Size = new System.Drawing.Size(147, 34);
            this.x8ToolStripMenuItem.Text = "8x8";
            this.x8ToolStripMenuItem.Click += new System.EventHandler(this.x8ToolStripMenuItem_Click);
            // 
            // x16ToolStripMenuItem
            // 
            this.x16ToolStripMenuItem.Name = "x16ToolStripMenuItem";
            this.x16ToolStripMenuItem.Size = new System.Drawing.Size(147, 34);
            this.x16ToolStripMenuItem.Text = "8x16";
            this.x16ToolStripMenuItem.Click += new System.EventHandler(this.x16ToolStripMenuItem_Click);
            // 
            // charViewer1
            // 
            this.charViewer1.BackColor = System.Drawing.Color.DarkBlue;
            this.charViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.charViewer1.ForeColor = System.Drawing.Color.Gainsboro;
            this.charViewer1.Location = new System.Drawing.Point(0, 42);
            this.charViewer1.Margin = new System.Windows.Forms.Padding(11);
            this.charViewer1.Name = "charViewer1";
            this.charViewer1.Size = new System.Drawing.Size(2107, 1304);
            this.charViewer1.TabIndex = 0;
            // 
            // CharSetMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2107, 1346);
            this.Controls.Add(this.charViewer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "CharSetMain";
            this.Text = "Character Set Viewer";
            this.Load += new System.EventHandler(this.CharSetMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CharViewer charViewer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PetsciiUGMenu;
        private System.Windows.Forms.ToolStripMenuItem PetsciiULMenu;
        private System.Windows.Forms.ToolStripMenuItem AsciiMenu;
        private System.Windows.Forms.ToolStripMenuItem aNSIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonPetCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}

