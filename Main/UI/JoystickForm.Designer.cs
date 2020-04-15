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
            this.UpButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("UpButton.BackgroundImage")));
            this.UpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UpButton.CausesValidation = false;
            this.UpButton.FlatAppearance.BorderSize = 0;
            this.UpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpButton.Location = new System.Drawing.Point(54, 49);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(30, 30);
            this.UpButton.TabIndex = 0;
            this.UpButton.Tag = "1";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.UpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // DownButton
            // 
            this.DownButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DownButton.BackgroundImage")));
            this.DownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DownButton.CausesValidation = false;
            this.DownButton.FlatAppearance.BorderSize = 0;
            this.DownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DownButton.Location = new System.Drawing.Point(54, 119);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(30, 30);
            this.DownButton.TabIndex = 1;
            this.DownButton.Tag = "2";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.DownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // LeftButton
            // 
            this.LeftButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LeftButton.BackgroundImage")));
            this.LeftButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.LeftButton.CausesValidation = false;
            this.LeftButton.FlatAppearance.BorderSize = 0;
            this.LeftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LeftButton.Location = new System.Drawing.Point(18, 85);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(30, 30);
            this.LeftButton.TabIndex = 2;
            this.LeftButton.Tag = "4";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.LeftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // RightButton
            // 
            this.RightButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RightButton.BackgroundImage")));
            this.RightButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RightButton.CausesValidation = false;
            this.RightButton.FlatAppearance.BorderSize = 0;
            this.RightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RightButton.Location = new System.Drawing.Point(90, 85);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(30, 30);
            this.RightButton.TabIndex = 3;
            this.RightButton.Tag = "8";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.RightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // Fire2Button
            // 
            this.Fire2Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Fire2Button.BackgroundImage")));
            this.Fire2Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Fire2Button.CausesValidation = false;
            this.Fire2Button.FlatAppearance.BorderSize = 0;
            this.Fire2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Fire2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fire2Button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Fire2Button.Location = new System.Drawing.Point(90, 16);
            this.Fire2Button.Name = "Fire2Button";
            this.Fire2Button.Size = new System.Drawing.Size(30, 30);
            this.Fire2Button.TabIndex = 4;
            this.Fire2Button.Tag = "32";
            this.Fire2Button.Text = "B";
            this.Fire2Button.UseVisualStyleBackColor = true;
            this.Fire2Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.Fire2Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // Fire1Button
            // 
            this.Fire1Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Fire1Button.BackgroundImage")));
            this.Fire1Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Fire1Button.CausesValidation = false;
            this.Fire1Button.FlatAppearance.BorderSize = 0;
            this.Fire1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Fire1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fire1Button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Fire1Button.Location = new System.Drawing.Point(18, 16);
            this.Fire1Button.Name = "Fire1Button";
            this.Fire1Button.Size = new System.Drawing.Size(30, 30);
            this.Fire1Button.TabIndex = 5;
            this.Fire1Button.Tag = "16";
            this.Fire1Button.Text = "A";
            this.Fire1Button.UseVisualStyleBackColor = true;
            this.Fire1Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AllButtonsDown);
            this.Fire1Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllButtonsUp);
            // 
            // JoystickForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(133, 155);
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
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.JoystickForm_KeyUp);
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