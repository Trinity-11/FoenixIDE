namespace FoenixIDE.Simulator.UI
{
    partial class TileEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileEditor));
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.Layer0Button = new System.Windows.Forms.Button();
            this.Layer1Button = new System.Windows.Forms.Button();
            this.Layer2Button = new System.Windows.Forms.Button();
            this.Layer3Button = new System.Windows.Forms.Button();
            this.LayerEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.TilesetAddressLabel = new System.Windows.Forms.Label();
            this.TilesetAddressText = new System.Windows.Forms.TextBox();
            this.TilesetViewer = new System.Windows.Forms.PictureBox();
            this.TileSelectedLabel = new System.Windows.Forms.Label();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPanel.Controls.Add(this.Layer3Button);
            this.HeaderPanel.Controls.Add(this.Layer2Button);
            this.HeaderPanel.Controls.Add(this.Layer1Button);
            this.HeaderPanel.Controls.Add(this.Layer0Button);
            this.HeaderPanel.Location = new System.Drawing.Point(1, -2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(326, 29);
            this.HeaderPanel.TabIndex = 0;
            // 
            // Layer0Button
            // 
            this.Layer0Button.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Layer0Button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Layer0Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer0Button.Location = new System.Drawing.Point(3, 3);
            this.Layer0Button.Name = "Layer0Button";
            this.Layer0Button.Size = new System.Drawing.Size(75, 23);
            this.Layer0Button.TabIndex = 0;
            this.Layer0Button.Tag = "0";
            this.Layer0Button.Text = "Layer 0";
            this.Layer0Button.UseVisualStyleBackColor = false;
            this.Layer0Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // Layer1Button
            // 
            this.Layer1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer1Button.Location = new System.Drawing.Point(84, 3);
            this.Layer1Button.Name = "Layer1Button";
            this.Layer1Button.Size = new System.Drawing.Size(75, 23);
            this.Layer1Button.TabIndex = 1;
            this.Layer1Button.Tag = "1";
            this.Layer1Button.Text = "Layer 1";
            this.Layer1Button.UseVisualStyleBackColor = true;
            this.Layer1Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // Layer2Button
            // 
            this.Layer2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer2Button.Location = new System.Drawing.Point(165, 3);
            this.Layer2Button.Name = "Layer2Button";
            this.Layer2Button.Size = new System.Drawing.Size(75, 23);
            this.Layer2Button.TabIndex = 2;
            this.Layer2Button.Tag = "2";
            this.Layer2Button.Text = "Layer 2";
            this.Layer2Button.UseVisualStyleBackColor = true;
            this.Layer2Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // Layer3Button
            // 
            this.Layer3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer3Button.Location = new System.Drawing.Point(246, 3);
            this.Layer3Button.Name = "Layer3Button";
            this.Layer3Button.Size = new System.Drawing.Size(75, 23);
            this.Layer3Button.TabIndex = 3;
            this.Layer3Button.Tag = "3";
            this.Layer3Button.Text = "Layer 3";
            this.Layer3Button.UseVisualStyleBackColor = true;
            this.Layer3Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // LayerEnabledCheckbox
            // 
            this.LayerEnabledCheckbox.AutoSize = true;
            this.LayerEnabledCheckbox.Location = new System.Drawing.Point(10, 30);
            this.LayerEnabledCheckbox.Name = "LayerEnabledCheckbox";
            this.LayerEnabledCheckbox.Size = new System.Drawing.Size(94, 17);
            this.LayerEnabledCheckbox.TabIndex = 1;
            this.LayerEnabledCheckbox.Text = "Layer Enabled";
            this.LayerEnabledCheckbox.UseVisualStyleBackColor = true;
            // 
            // TilesetAddressLabel
            // 
            this.TilesetAddressLabel.AutoSize = true;
            this.TilesetAddressLabel.Location = new System.Drawing.Point(7, 53);
            this.TilesetAddressLabel.Name = "TilesetAddressLabel";
            this.TilesetAddressLabel.Size = new System.Drawing.Size(91, 13);
            this.TilesetAddressLabel.TabIndex = 2;
            this.TilesetAddressLabel.Text = "Tileset Address: $";
            // 
            // TilesetAddressText
            // 
            this.TilesetAddressText.Location = new System.Drawing.Point(98, 49);
            this.TilesetAddressText.MaxLength = 6;
            this.TilesetAddressText.Name = "TilesetAddressText";
            this.TilesetAddressText.Size = new System.Drawing.Size(100, 20);
            this.TilesetAddressText.TabIndex = 3;
            // 
            // TilesetViewer
            // 
            this.TilesetViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TilesetViewer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TilesetViewer.Location = new System.Drawing.Point(25, 74);
            this.TilesetViewer.Name = "TilesetViewer";
            this.TilesetViewer.Size = new System.Drawing.Size(276, 276);
            this.TilesetViewer.TabIndex = 4;
            this.TilesetViewer.TabStop = false;
            this.TilesetViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.TilesetViewer_Paint);
            this.TilesetViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TilesetViewer_MouseClick);
            this.TilesetViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TilesetViewer_MouseMove);
            // 
            // TileSelectedLabel
            // 
            this.TileSelectedLabel.AutoSize = true;
            this.TileSelectedLabel.Location = new System.Drawing.Point(7, 356);
            this.TileSelectedLabel.Name = "TileSelectedLabel";
            this.TileSelectedLabel.Size = new System.Drawing.Size(81, 13);
            this.TileSelectedLabel.TabIndex = 5;
            this.TileSelectedLabel.Text = "Tile Selected: $";
            // 
            // TileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 375);
            this.Controls.Add(this.TileSelectedLabel);
            this.Controls.Add(this.TilesetViewer);
            this.Controls.Add(this.TilesetAddressText);
            this.Controls.Add(this.TilesetAddressLabel);
            this.Controls.Add(this.LayerEnabledCheckbox);
            this.Controls.Add(this.HeaderPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TileEditor";
            this.Text = "TileEditor";
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TilesetViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Button Layer3Button;
        private System.Windows.Forms.Button Layer2Button;
        private System.Windows.Forms.Button Layer1Button;
        private System.Windows.Forms.Button Layer0Button;
        private System.Windows.Forms.CheckBox LayerEnabledCheckbox;
        private System.Windows.Forms.Label TilesetAddressLabel;
        private System.Windows.Forms.TextBox TilesetAddressText;
        private System.Windows.Forms.PictureBox TilesetViewer;
        private System.Windows.Forms.Label TileSelectedLabel;
    }
}