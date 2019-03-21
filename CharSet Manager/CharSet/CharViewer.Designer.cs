namespace CharSet
{
    partial class CharViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CharViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkBlue;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Name = "CharViewer";
            this.Size = new System.Drawing.Size(572, 457);
            this.SizeChanged += new System.EventHandler(this.CharViewer_SizeChanged);
            this.Click += new System.EventHandler(this.CharViewer_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CharViewer_Paint);
            this.Resize += new System.EventHandler(this.CharViewer_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
