
using System.Windows.Forms;
using System.Drawing;

namespace FoenixIDE.Simulator.UI
{
    partial class InputDialog
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddressWindow));
            this.layoutPanel = new FlowLayoutPanel();
            this.lblAddress = new Label();
            this.txtAddress = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.layoutPanel.SuspendLayout();
            this.SuspendLayout();
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);

            int y = 10;
            // 
            // txtLable (dynamic height)
            //
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(this.prompt, this.lblAddress.Font, 320);
                this.lblAddress.Width = 320;
                this.lblAddress.Height = (int)(size.Height + .5);
            }
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Location = new Point(5, y);
            this.lblAddress.TabIndex = 0;
            this.lblAddress.Text = this.prompt;
            y += this.lblAddress.Height + 10;
            // 
            // txtAddress
            // 
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new Size(65, 13);
            this.txtAddress.Location = new Point(5, y);
            this.txtAddress.TabIndex = 1;
            this.txtAddress.Text = "000000";
            y += this.txtAddress.Height + 10;
            // 
            // btnOk
            // 
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(90, 25);
            this.btnOk.Location = new Point(320 - 90 - 5, y);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.DialogResult = DialogResult.OK;
            y += this.btnOk.Height + 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(90, 25);
            this.btnCancel.Location = new Point(320 - 90 - 5, y);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Margin = new Padding(0, 0, 0, 10);
            this.btnCancel.DialogResult = DialogResult.Cancel;

            y += this.btnCancel.Height + 5;
           
            //this.btnOk.Anchor = AnchorStyles.Right;
            //
            // Dialog
            ///
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.layoutPanel);
            this.Text = this.title;
            this.Name = "Address";
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = true;

            this.layoutPanel.ResumeLayout(false);
            //this.layoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Label lblAddress;
        private FlowLayoutPanel layoutPanel;
        private TextBox txtAddress;
        private Button btnOk;
        private Button btnCancel;

    }
}