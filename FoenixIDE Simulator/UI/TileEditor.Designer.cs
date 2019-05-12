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
            this.Layer3Button = new System.Windows.Forms.Button();
            this.Layer2Button = new System.Windows.Forms.Button();
            this.Layer1Button = new System.Windows.Forms.Button();
            this.Layer0Button = new System.Windows.Forms.Button();
            this.LayerEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.TilesetAddressLabel = new System.Windows.Forms.Label();
            this.TilesetAddressText = new System.Windows.Forms.TextBox();
            this.TilesetViewer = new System.Windows.Forms.PictureBox();
            this.TileSelectedLabel = new System.Windows.Forms.Label();
            this.LUTLabel = new System.Windows.Forms.Label();
            this.LUTDomain = new System.Windows.Forms.DomainUpDown();
            this.XLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.StrideXText = new System.Windows.Forms.TextBox();
            this.StrideYText = new System.Windows.Forms.TextBox();
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
            this.HeaderPanel.Location = new System.Drawing.Point(1, -3);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(6);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(598, 54);
            this.HeaderPanel.TabIndex = 0;
            // 
            // Layer3Button
            // 
            this.Layer3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer3Button.Location = new System.Drawing.Point(451, 6);
            this.Layer3Button.Margin = new System.Windows.Forms.Padding(6);
            this.Layer3Button.Name = "Layer3Button";
            this.Layer3Button.Size = new System.Drawing.Size(138, 42);
            this.Layer3Button.TabIndex = 3;
            this.Layer3Button.Tag = "3";
            this.Layer3Button.Text = "Layer 3";
            this.Layer3Button.UseVisualStyleBackColor = true;
            this.Layer3Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // Layer2Button
            // 
            this.Layer2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer2Button.Location = new System.Drawing.Point(302, 6);
            this.Layer2Button.Margin = new System.Windows.Forms.Padding(6);
            this.Layer2Button.Name = "Layer2Button";
            this.Layer2Button.Size = new System.Drawing.Size(138, 42);
            this.Layer2Button.TabIndex = 2;
            this.Layer2Button.Tag = "2";
            this.Layer2Button.Text = "Layer 2";
            this.Layer2Button.UseVisualStyleBackColor = true;
            this.Layer2Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // Layer1Button
            // 
            this.Layer1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer1Button.Location = new System.Drawing.Point(154, 6);
            this.Layer1Button.Margin = new System.Windows.Forms.Padding(6);
            this.Layer1Button.Name = "Layer1Button";
            this.Layer1Button.Size = new System.Drawing.Size(138, 42);
            this.Layer1Button.TabIndex = 1;
            this.Layer1Button.Tag = "1";
            this.Layer1Button.Text = "Layer 1";
            this.Layer1Button.UseVisualStyleBackColor = true;
            this.Layer1Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // Layer0Button
            // 
            this.Layer0Button.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Layer0Button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Layer0Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Layer0Button.Location = new System.Drawing.Point(6, 6);
            this.Layer0Button.Margin = new System.Windows.Forms.Padding(6);
            this.Layer0Button.Name = "Layer0Button";
            this.Layer0Button.Size = new System.Drawing.Size(138, 42);
            this.Layer0Button.TabIndex = 0;
            this.Layer0Button.Tag = "0";
            this.Layer0Button.Text = "Layer 0";
            this.Layer0Button.UseVisualStyleBackColor = false;
            this.Layer0Button.Click += new System.EventHandler(this.Layer0Button_Click);
            // 
            // LayerEnabledCheckbox
            // 
            this.LayerEnabledCheckbox.AutoSize = true;
            this.LayerEnabledCheckbox.Location = new System.Drawing.Point(18, 56);
            this.LayerEnabledCheckbox.Margin = new System.Windows.Forms.Padding(6);
            this.LayerEnabledCheckbox.Name = "LayerEnabledCheckbox";
            this.LayerEnabledCheckbox.Size = new System.Drawing.Size(164, 29);
            this.LayerEnabledCheckbox.TabIndex = 1;
            this.LayerEnabledCheckbox.Text = "Layer Enabled";
            this.LayerEnabledCheckbox.UseVisualStyleBackColor = true;
            this.LayerEnabledCheckbox.Click += new System.EventHandler(this.LayerEnabledCheckbox_Click);
            // 
            // TilesetAddressLabel
            // 
            this.TilesetAddressLabel.AutoSize = true;
            this.TilesetAddressLabel.Location = new System.Drawing.Point(12, 98);
            this.TilesetAddressLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.TilesetAddressLabel.Name = "TilesetAddressLabel";
            this.TilesetAddressLabel.Size = new System.Drawing.Size(170, 25);
            this.TilesetAddressLabel.TabIndex = 2;
            this.TilesetAddressLabel.Text = "Tileset Address: $";
            // 
            // TilesetAddressText
            // 
            this.TilesetAddressText.Location = new System.Drawing.Point(180, 93);
            this.TilesetAddressText.Margin = new System.Windows.Forms.Padding(6);
            this.TilesetAddressText.MaxLength = 6;
            this.TilesetAddressText.Name = "TilesetAddressText";
            this.TilesetAddressText.Size = new System.Drawing.Size(180, 29);
            this.TilesetAddressText.TabIndex = 3;
            this.TilesetAddressText.TextChanged += new System.EventHandler(this.TilesetAddressText_TextChanged);
            // 
            // TilesetViewer
            // 
            this.TilesetViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TilesetViewer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TilesetViewer.Location = new System.Drawing.Point(45, 136);
            this.TilesetViewer.Margin = new System.Windows.Forms.Padding(6);
            this.TilesetViewer.Name = "TilesetViewer";
            this.TilesetViewer.Size = new System.Drawing.Size(504, 508);
            this.TilesetViewer.TabIndex = 4;
            this.TilesetViewer.TabStop = false;
            this.TilesetViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.TilesetViewer_Paint);
            this.TilesetViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TilesetViewer_MouseClick);
            this.TilesetViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TilesetViewer_MouseMove);
            // 
            // TileSelectedLabel
            // 
            this.TileSelectedLabel.AutoSize = true;
            this.TileSelectedLabel.Location = new System.Drawing.Point(12, 657);
            this.TileSelectedLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.TileSelectedLabel.Name = "TileSelectedLabel";
            this.TileSelectedLabel.Size = new System.Drawing.Size(148, 25);
            this.TileSelectedLabel.TabIndex = 5;
            this.TileSelectedLabel.Text = "Tile Selected: $";
            // 
            // LUTLabel
            // 
            this.LUTLabel.AutoSize = true;
            this.LUTLabel.Location = new System.Drawing.Point(230, 57);
            this.LUTLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LUTLabel.Name = "LUTLabel";
            this.LUTLabel.Size = new System.Drawing.Size(56, 25);
            this.LUTLabel.TabIndex = 6;
            this.LUTLabel.Text = "LUT:";
            // 
            // LUTDomain
            // 
            this.LUTDomain.Items.Add("0");
            this.LUTDomain.Items.Add("1");
            this.LUTDomain.Items.Add("2");
            this.LUTDomain.Items.Add("3");
            this.LUTDomain.Location = new System.Drawing.Point(283, 54);
            this.LUTDomain.Margin = new System.Windows.Forms.Padding(4);
            this.LUTDomain.Name = "LUTDomain";
            this.LUTDomain.Size = new System.Drawing.Size(52, 29);
            this.LUTDomain.TabIndex = 7;
            this.LUTDomain.Text = "0";
            this.LUTDomain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LUTDomain.Wrap = true;
            this.LUTDomain.SelectedItemChanged += new System.EventHandler(this.LUTDomain_SelectedItemChanged);
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.Location = new System.Drawing.Point(385, 57);
            this.XLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(88, 25);
            this.XLabel.TabIndex = 8;
            this.XLabel.Text = "Stride X:";
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Location = new System.Drawing.Point(385, 98);
            this.YLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(87, 25);
            this.YLabel.TabIndex = 9;
            this.YLabel.Text = "Stride Y:";
            // 
            // StrideXText
            // 
            this.StrideXText.Location = new System.Drawing.Point(483, 54);
            this.StrideXText.Margin = new System.Windows.Forms.Padding(6);
            this.StrideXText.MaxLength = 4;
            this.StrideXText.Name = "StrideXText";
            this.StrideXText.Size = new System.Drawing.Size(66, 29);
            this.StrideXText.TabIndex = 10;
            this.StrideXText.Text = "0";
            this.StrideXText.TextChanged += new System.EventHandler(this.StrideXText_TextChanged);
            // 
            // StrideYText
            // 
            this.StrideYText.Location = new System.Drawing.Point(482, 93);
            this.StrideYText.Margin = new System.Windows.Forms.Padding(6);
            this.StrideYText.MaxLength = 4;
            this.StrideYText.Name = "StrideYText";
            this.StrideYText.Size = new System.Drawing.Size(66, 29);
            this.StrideYText.TabIndex = 11;
            this.StrideYText.Text = "0";
            this.StrideYText.TextChanged += new System.EventHandler(this.StrideYText_TextChanged);
            // 
            // TileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 693);
            this.Controls.Add(this.StrideYText);
            this.Controls.Add(this.StrideXText);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.LUTDomain);
            this.Controls.Add(this.LUTLabel);
            this.Controls.Add(this.TileSelectedLabel);
            this.Controls.Add(this.TilesetViewer);
            this.Controls.Add(this.TilesetAddressText);
            this.Controls.Add(this.TilesetAddressLabel);
            this.Controls.Add(this.LayerEnabledCheckbox);
            this.Controls.Add(this.HeaderPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "TileEditor";
            this.Text = "TileEditor";
            this.Load += new System.EventHandler(this.TileEditor_Load);
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
        private System.Windows.Forms.CheckBox LayerEnabledCheckbox;
        private System.Windows.Forms.Label TilesetAddressLabel;
        private System.Windows.Forms.TextBox TilesetAddressText;
        private System.Windows.Forms.Label TileSelectedLabel;
        private System.Windows.Forms.Label LUTLabel;
        private System.Windows.Forms.DomainUpDown LUTDomain;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.TextBox StrideXText;
        private System.Windows.Forms.TextBox StrideYText;
        private System.Windows.Forms.PictureBox TilesetViewer;
        private System.Windows.Forms.Button Layer0Button;
    }
}