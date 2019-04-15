using FoenixIDE.Display;

namespace FoenixIDE.UI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ModeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastKeyPressed = new System.Windows.Forms.ToolStripStatusLabel();
            this.cpsPerf = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsPerf = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeTimer = new System.Windows.Forms.Timer(this.components);
            this.performanceTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gpu = new FoenixIDE.Display.Gpu();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModeText,
            this.lastKeyPressed,
            this.cpsPerf,
            this.fpsPerf});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1532);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 26, 0);
            this.statusStrip1.Size = new System.Drawing.Size(2171, 43);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ModeText
            // 
            this.ModeText.Name = "ModeText";
            this.ModeText.Size = new System.Drawing.Size(112, 38);
            this.ModeText.Text = "Immediate";
            // 
            // lastKeyPressed
            // 
            this.lastKeyPressed.Name = "lastKeyPressed";
            this.lastKeyPressed.Size = new System.Drawing.Size(46, 38);
            this.lastKeyPressed.Text = "$00";
            // 
            // cpsPerf
            // 
            this.cpsPerf.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.cpsPerf.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.cpsPerf.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpsPerf.Name = "cpsPerf";
            this.cpsPerf.Padding = new System.Windows.Forms.Padding(2);
            this.cpsPerf.Size = new System.Drawing.Size(83, 38);
            this.cpsPerf.Text = "CPS: 0";
            // 
            // fpsPerf
            // 
            this.fpsPerf.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.fpsPerf.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.fpsPerf.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpsPerf.Name = "fpsPerf";
            this.fpsPerf.Padding = new System.Windows.Forms.Padding(2);
            this.fpsPerf.Size = new System.Drawing.Size(81, 38);
            this.fpsPerf.Text = "FPS: 0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(11, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(2171, 42);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen,
            this.loadImageToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Size = new System.Drawing.Size(213, 34);
            this.menuOpen.Text = "&Open";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.loadImageToolStripMenuItem.Text = "&Load Image";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cPUToolStripMenuItem,
            this.memoryToolStripMenuItem,
            this.uploaderToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(110, 34);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // cPUToolStripMenuItem
            // 
            this.cPUToolStripMenuItem.Name = "cPUToolStripMenuItem";
            this.cPUToolStripMenuItem.Size = new System.Drawing.Size(188, 34);
            this.cPUToolStripMenuItem.Text = "&CPU";
            this.cPUToolStripMenuItem.Click += new System.EventHandler(this.cPUToolStripMenuItem_Click);
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            this.memoryToolStripMenuItem.Size = new System.Drawing.Size(188, 34);
            this.memoryToolStripMenuItem.Text = "&Memory";
            this.memoryToolStripMenuItem.Click += new System.EventHandler(this.memoryToolStripMenuItem_Click);
            // 
            // uploaderToolStripMenuItem
            // 
            this.uploaderToolStripMenuItem.Name = "uploaderToolStripMenuItem";
            this.uploaderToolStripMenuItem.Size = new System.Drawing.Size(188, 34);
            this.uploaderToolStripMenuItem.Text = "&Uploader";
            this.uploaderToolStripMenuItem.Click += new System.EventHandler(this.uploaderToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem1,
            this.debugToolStripMenuItem});
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(75, 34);
            this.resetToolStripMenuItem.Text = "&Reset";
            // 
            // resetToolStripMenuItem1
            // 
            this.resetToolStripMenuItem1.Name = "resetToolStripMenuItem1";
            this.resetToolStripMenuItem1.Size = new System.Drawing.Size(218, 34);
            this.resetToolStripMenuItem1.Text = "Start/&Restart";
            this.resetToolStripMenuItem1.Click += new System.EventHandler(this.resetToolStripMenuItem1_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.debugToolStripMenuItem.Text = "Start/&Debug";
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.debugToolStripMenuItem_Click);
            // 
            // executeTimer
            // 
            this.executeTimer.Interval = 1;
            this.executeTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // performanceTimer
            // 
            this.performanceTimer.Enabled = true;
            this.performanceTimer.Interval = 1000;
            this.performanceTimer.Tick += new System.EventHandler(this.performanceTimer_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 72);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(29, 1460);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightBlue;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(2142, 72);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(29, 1460);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightBlue;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(29, 1502);
            this.panel3.Margin = new System.Windows.Forms.Padding(6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2113, 30);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightBlue;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 42);
            this.panel4.Margin = new System.Windows.Forms.Padding(6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2171, 30);
            this.panel4.TabIndex = 6;
            // 
            // gpu
            // 
            this.gpu.BackColor = System.Drawing.Color.Blue;
            this.gpu.COLS_PER_LINE = 0;
            this.gpu.ColumnsVisible = 0;
            this.gpu.CursorPos = 0;
            this.gpu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpu.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gpu.LinesVisible = 0;
            this.gpu.Location = new System.Drawing.Point(29, 72);
            this.gpu.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.gpu.MinimumSize = new System.Drawing.Size(639, 480);
            this.gpu.Name = "gpu";
            this.gpu.Size = new System.Drawing.Size(2113, 1430);
            this.gpu.TabIndex = 0;
            this.gpu.X = 0;
            this.gpu.Y = 0;
            this.gpu.VisibleChanged += new System.EventHandler(this.gpu_VisibleChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2171, 1575);
            this.Controls.Add(this.gpu);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(300, 300);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(710, 637);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Foenix IDE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.BasicWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BasicWindow_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BasicWindow_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BasicWindow_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Gpu gpu;
        private global::System.Windows.Forms.StatusStrip statusStrip1;
        private global::System.Windows.Forms.ToolStripStatusLabel ModeText;
        private global::System.Windows.Forms.ToolStripStatusLabel lastKeyPressed;
        private global::System.Windows.Forms.MenuStrip menuStrip1;
        private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem cPUToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
        private global::System.Windows.Forms.Timer executeTimer;
        private global::System.Windows.Forms.ToolStripStatusLabel cpsPerf;
        private global::System.Windows.Forms.Timer performanceTimer;
        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.Panel panel2;
        private global::System.Windows.Forms.Panel panel3;
        private global::System.Windows.Forms.Panel panel4;
        private global::System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private global::System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem1;
        private global::System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem uploaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel fpsPerf;
    }
}

