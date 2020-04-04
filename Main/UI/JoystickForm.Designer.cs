namespace FoenixIDE.Simulator.UI
{
    partial class JoystickForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoystickForm));
            this.UpButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.LeftButton = new System.Windows.Forms.Button();
            this.RightButton = new System.Windows.Forms.Button();
            this.Fire2Button = new System.Windows.Forms.Button();
            this.Fire1Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UpButton
            // 
            this.UpButton.Location = new System.Drawing.Point(76, 63);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(54, 23);
            this.UpButton.TabIndex = 0;
            this.UpButton.Tag = "1";
            this.UpButton.Text = "Up";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.UpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // DownButton
            // 
            this.DownButton.Location = new System.Drawing.Point(76, 122);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(54, 23);
            this.DownButton.TabIndex = 1;
            this.DownButton.Tag = "2";
            this.DownButton.Text = "Down";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.DownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // LeftButton
            // 
            this.LeftButton.Location = new System.Drawing.Point(26, 92);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(54, 23);
            this.LeftButton.TabIndex = 2;
            this.LeftButton.Tag = "4";
            this.LeftButton.Text = "Left";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.LeftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // RightButton
            // 
            this.RightButton.Location = new System.Drawing.Point(123, 92);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(54, 23);
            this.RightButton.TabIndex = 3;
            this.RightButton.Tag = "8";
            this.RightButton.Text = "Right";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.RightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // Fire2Button
            // 
            this.Fire2Button.Location = new System.Drawing.Point(123, 12);
            this.Fire2Button.Name = "Fire2Button";
            this.Fire2Button.Size = new System.Drawing.Size(64, 23);
            this.Fire2Button.TabIndex = 4;
            this.Fire2Button.Tag = "32";
            this.Fire2Button.Text = "Button 2";
            this.Fire2Button.UseVisualStyleBackColor = true;
            this.Fire2Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.Fire2Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // Fire1Button
            // 
            this.Fire1Button.Location = new System.Drawing.Point(16, 12);
            this.Fire1Button.Name = "Fire1Button";
            this.Fire1Button.Size = new System.Drawing.Size(64, 23);
            this.Fire1Button.TabIndex = 5;
            this.Fire1Button.Tag = "16";
            this.Fire1Button.Text = "Button 1";
            this.Fire1Button.UseVisualStyleBackColor = true;
            this.Fire1Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.Fire1Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // JoystickForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 157);
            this.Controls.Add(this.Fire1Button);
            this.Controls.Add(this.Fire2Button);
            this.Controls.Add(this.RightButton);
            this.Controls.Add(this.LeftButton);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.UpButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "JoystickForm";
            this.Text = "Joystick";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JoystickForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.JoystickForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button LeftButton;
        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.Button Fire2Button;
        private System.Windows.Forms.Button Fire1Button;
    }
}