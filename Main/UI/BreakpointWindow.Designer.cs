
namespace FoenixIDE.Simulator.UI
{
    partial class BreakpointWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BreakpointWindow));
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblExecute = new System.Windows.Forms.Label();
            this.lblRead = new System.Windows.Forms.Label();
            this.lblWrite = new System.Windows.Forms.Label();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(11, 5);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 0;
            this.lblAddress.Text = "Address";
            // 
            // lblExecute
            // 
            this.lblExecute.AutoSize = true;
            this.lblExecute.Location = new System.Drawing.Point(77, 5);
            this.lblExecute.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExecute.Name = "lblExecute";
            this.lblExecute.Size = new System.Drawing.Size(31, 13);
            this.lblExecute.TabIndex = 1;
            this.lblExecute.Text = "Exec";
            // 
            // lblRead
            // 
            this.lblRead.AutoSize = true;
            this.lblRead.Location = new System.Drawing.Point(116, 5);
            this.lblRead.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRead.Name = "lblRead";
            this.lblRead.Size = new System.Drawing.Size(33, 13);
            this.lblRead.TabIndex = 2;
            this.lblRead.Text = "Read";
            // 
            // lblWrite
            // 
            this.lblWrite.AutoSize = true;
            this.lblWrite.Location = new System.Drawing.Point(154, 5);
            this.lblWrite.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWrite.Name = "lblWrite";
            this.lblWrite.Size = new System.Drawing.Size(32, 13);
            this.lblWrite.TabIndex = 3;
            this.lblWrite.Text = "Write";
            // 
            // BreakpointWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 152);
            this.Controls.Add(this.lblWrite);
            this.Controls.Add(this.lblRead);
            this.Controls.Add(this.lblExecute);
            this.Controls.Add(this.lblAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BreakpointWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Breakpoints";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Breakpoints_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Breakpoints_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblExecute;
        private System.Windows.Forms.Label lblRead;
        private System.Windows.Forms.Label lblWrite;
        private System.Windows.Forms.ToolTip toolTips;
    }
}