namespace FoenixIDE.UI
{
    partial class AccumulatorControl
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
            this.regB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.regA = new System.Windows.Forms.TextBox();
            this.lblDollar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // regB
            // 
            this.regB.BackColor = System.Drawing.SystemColors.InfoText;
            this.regB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.regB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regB.ForeColor = System.Drawing.SystemColors.Info;
            this.regB.Location = new System.Drawing.Point(8, 17);
            this.regB.MaxLength = 2;
            this.regB.Name = "regB";
            this.regB.Size = new System.Drawing.Size(13, 13);
            this.regB.TabIndex = 3;
            this.regB.Text = "00";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Accumulator";
            // 
            // regA
            // 
            this.regA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.regA.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regA.ForeColor = System.Drawing.SystemColors.WindowText;
            this.regA.Location = new System.Drawing.Point(23, 17);
            this.regA.MaxLength = 2;
            this.regA.Name = "regA";
            this.regA.Size = new System.Drawing.Size(14, 13);
            this.regA.TabIndex = 4;
            this.regA.Text = "00";
            // 
            // lblDollar
            // 
            this.lblDollar.AutoSize = true;
            this.lblDollar.Location = new System.Drawing.Point(-3, 16);
            this.lblDollar.Name = "lblDollar";
            this.lblDollar.Size = new System.Drawing.Size(13, 13);
            this.lblDollar.TabIndex = 5;
            this.lblDollar.Text = "$";
            // 
            // AccumulatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.regA);
            this.Controls.Add(this.regB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDollar);
            this.Name = "AccumulatorControl";
            this.Size = new System.Drawing.Size(45, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox regB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox regA;
        private System.Windows.Forms.Label lblDollar;
    }
}
