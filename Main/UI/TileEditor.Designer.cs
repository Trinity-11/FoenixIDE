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
            redPen.Dispose();
            whiteBrush.Dispose();
            whitePen.Dispose();
            yellowPen.Dispose();
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
            this.Tilemap3Button = new System.Windows.Forms.Button();
            this.Tilemap2Button = new System.Windows.Forms.Button();
            this.Tilemap1Button = new System.Windows.Forms.Button();
            this.Tilemap0Button = new System.Windows.Forms.Button();
            this.TilesetViewer = new System.Windows.Forms.PictureBox();
            this.LeftTileSelectedLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TilemapGroup = new System.Windows.Forms.GroupBox();
            this.checkSmallTiles = new System.Windows.Forms.CheckBox();
            this.btnMemory = new System.Windows.Forms.Button();
            this.WindowY = new System.Windows.Forms.TextBox();
            this.WindowX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SaveTilesetButton = new System.Windows.Forms.Button();
            this.ClearTilemapButton = new System.Windows.Forms.Button();
            this.TilemapHeight = new System.Windows.Forms.TextBox();
            this.TilemapWidth = new System.Windows.Forms.TextBox();
            this.YLabel = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.TilemapAddress = new System.Windows.Forms.TextBox();
            this.TilesetAddressLabel = new System.Windows.Forms.Label();
            this.TilemapEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.TilesetList = new System.Windows.Forms.ComboBox();
            this.TilesetGroup = new System.Windows.Forms.GroupBox();
            this.Stride256Checkbox = new System.Windows.Forms.CheckBox();
            this.LutList = new System.Windows.Forms.ComboBox();
            this.TilesetAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RightTileSelectedLabel = new System.Windows.Forms.Label();
            this.leftTile = new System.Windows.Forms.PictureBox();
            this.RightTile = new System.Windows.Forms.PictureBox();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetViewer)).BeginInit();
            this.TilemapGroup.SuspendLayout();
            this.TilesetGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightTile)).BeginInit();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPanel.Controls.Add(this.Tilemap3Button);
            this.HeaderPanel.Controls.Add(this.Tilemap2Button);
            this.HeaderPanel.Controls.Add(this.Tilemap1Button);
            this.HeaderPanel.Controls.Add(this.Tilemap0Button);
            this.HeaderPanel.Location = new System.Drawing.Point(1, -2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(363, 29);
            this.HeaderPanel.TabIndex = 0;
            // 
            // Tilemap3Button
            // 
            this.Tilemap3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tilemap3Button.Location = new System.Drawing.Point(246, 3);
            this.Tilemap3Button.Name = "Tilemap3Button";
            this.Tilemap3Button.Size = new System.Drawing.Size(75, 23);
            this.Tilemap3Button.TabIndex = 3;
            this.Tilemap3Button.Tag = "3";
            this.Tilemap3Button.Text = "Tilemap 3";
            this.Tilemap3Button.UseVisualStyleBackColor = true;
            this.Tilemap3Button.Click += new System.EventHandler(this.Tilemap0Button_Click);
            // 
            // Tilemap2Button
            // 
            this.Tilemap2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tilemap2Button.Location = new System.Drawing.Point(165, 3);
            this.Tilemap2Button.Name = "Tilemap2Button";
            this.Tilemap2Button.Size = new System.Drawing.Size(75, 23);
            this.Tilemap2Button.TabIndex = 2;
            this.Tilemap2Button.Tag = "2";
            this.Tilemap2Button.Text = "Tilemap 2";
            this.Tilemap2Button.UseVisualStyleBackColor = true;
            this.Tilemap2Button.Click += new System.EventHandler(this.Tilemap0Button_Click);
            // 
            // Tilemap1Button
            // 
            this.Tilemap1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tilemap1Button.Location = new System.Drawing.Point(84, 3);
            this.Tilemap1Button.Name = "Tilemap1Button";
            this.Tilemap1Button.Size = new System.Drawing.Size(75, 23);
            this.Tilemap1Button.TabIndex = 1;
            this.Tilemap1Button.Tag = "1";
            this.Tilemap1Button.Text = "Tilemap 1";
            this.Tilemap1Button.UseVisualStyleBackColor = true;
            this.Tilemap1Button.Click += new System.EventHandler(this.Tilemap0Button_Click);
            // 
            // Tilemap0Button
            // 
            this.Tilemap0Button.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Tilemap0Button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Tilemap0Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tilemap0Button.Location = new System.Drawing.Point(3, 3);
            this.Tilemap0Button.Name = "Tilemap0Button";
            this.Tilemap0Button.Size = new System.Drawing.Size(75, 23);
            this.Tilemap0Button.TabIndex = 0;
            this.Tilemap0Button.Tag = "0";
            this.Tilemap0Button.Text = "Tilemap 0";
            this.Tilemap0Button.UseVisualStyleBackColor = false;
            this.Tilemap0Button.Click += new System.EventHandler(this.Tilemap0Button_Click);
            // 
            // TilesetViewer
            // 
            this.TilesetViewer.BackColor = System.Drawing.Color.Black;
            this.TilesetViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TilesetViewer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TilesetViewer.Location = new System.Drawing.Point(4, 246);
            this.TilesetViewer.Margin = new System.Windows.Forms.Padding(0);
            this.TilesetViewer.Name = "TilesetViewer";
            this.TilesetViewer.Size = new System.Drawing.Size(276, 276);
            this.TilesetViewer.TabIndex = 4;
            this.TilesetViewer.TabStop = false;
            this.TilesetViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.TilesetViewer_Paint);
            this.TilesetViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TilesetViewer_MouseClick);
            this.TilesetViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TilesetViewer_MouseMove);
            // 
            // LeftTileSelectedLabel
            // 
            this.LeftTileSelectedLabel.AutoSize = true;
            this.LeftTileSelectedLabel.Location = new System.Drawing.Point(286, 321);
            this.LeftTileSelectedLabel.Name = "LeftTileSelectedLabel";
            this.LeftTileSelectedLabel.Size = new System.Drawing.Size(57, 13);
            this.LeftTileSelectedLabel.TabIndex = 4;
            this.LeftTileSelectedLabel.Text = "Left Tile: $";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tileset:";
            // 
            // TilemapGroup
            // 
            this.TilemapGroup.Controls.Add(this.checkSmallTiles);
            this.TilemapGroup.Controls.Add(this.btnMemory);
            this.TilemapGroup.Controls.Add(this.WindowY);
            this.TilemapGroup.Controls.Add(this.WindowX);
            this.TilemapGroup.Controls.Add(this.label2);
            this.TilemapGroup.Controls.Add(this.label3);
            this.TilemapGroup.Controls.Add(this.SaveTilesetButton);
            this.TilemapGroup.Controls.Add(this.ClearTilemapButton);
            this.TilemapGroup.Controls.Add(this.TilemapHeight);
            this.TilemapGroup.Controls.Add(this.TilemapWidth);
            this.TilemapGroup.Controls.Add(this.YLabel);
            this.TilemapGroup.Controls.Add(this.XLabel);
            this.TilemapGroup.Controls.Add(this.TilemapAddress);
            this.TilemapGroup.Controls.Add(this.TilesetAddressLabel);
            this.TilemapGroup.Controls.Add(this.TilemapEnabledCheckbox);
            this.TilemapGroup.Location = new System.Drawing.Point(5, 30);
            this.TilemapGroup.Name = "TilemapGroup";
            this.TilemapGroup.Size = new System.Drawing.Size(355, 128);
            this.TilemapGroup.TabIndex = 1;
            this.TilemapGroup.TabStop = false;
            this.TilemapGroup.Text = "Tilemap Properties";
            // 
            // checkSmallTiles
            // 
            this.checkSmallTiles.AutoSize = true;
            this.checkSmallTiles.Location = new System.Drawing.Point(14, 98);
            this.checkSmallTiles.Margin = new System.Windows.Forms.Padding(2);
            this.checkSmallTiles.Name = "checkSmallTiles";
            this.checkSmallTiles.Size = new System.Drawing.Size(76, 17);
            this.checkSmallTiles.TabIndex = 14;
            this.checkSmallTiles.Text = "Small Tiles";
            this.checkSmallTiles.UseVisualStyleBackColor = true;
            this.checkSmallTiles.CheckedChanged += new System.EventHandler(this.checkSmallTiles_CheckedChanged);
            // 
            // btnMemory
            // 
            this.btnMemory.Image = global::FoenixIDE.Simulator.Properties.Resources.memory_btn;
            this.btnMemory.Location = new System.Drawing.Point(329, 15);
            this.btnMemory.Name = "btnMemory";
            this.btnMemory.Size = new System.Drawing.Size(24, 24);
            this.btnMemory.TabIndex = 13;
            this.btnMemory.UseVisualStyleBackColor = true;
            this.btnMemory.Click += new System.EventHandler(this.btnMemory_Click);
            // 
            // WindowY
            // 
            this.WindowY.Location = new System.Drawing.Point(223, 66);
            this.WindowY.MaxLength = 4;
            this.WindowY.Name = "WindowY";
            this.WindowY.Size = new System.Drawing.Size(38, 20);
            this.WindowY.TabIndex = 10;
            this.WindowY.Text = "1023";
            this.WindowY.TextChanged += new System.EventHandler(this.WindowY_TextChanged);
            // 
            // WindowX
            // 
            this.WindowX.Location = new System.Drawing.Point(84, 66);
            this.WindowX.MaxLength = 4;
            this.WindowX.Name = "WindowX";
            this.WindowX.Size = new System.Drawing.Size(38, 20);
            this.WindowX.TabIndex = 8;
            this.WindowX.Text = "1023";
            this.WindowX.TextChanged += new System.EventHandler(this.WindowX_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Window Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Window X:";
            // 
            // SaveTilesetButton
            // 
            this.SaveTilesetButton.Location = new System.Drawing.Point(223, 93);
            this.SaveTilesetButton.Name = "SaveTilesetButton";
            this.SaveTilesetButton.Size = new System.Drawing.Size(86, 23);
            this.SaveTilesetButton.TabIndex = 12;
            this.SaveTilesetButton.Text = "Save Tilemap";
            this.SaveTilesetButton.UseVisualStyleBackColor = true;
            this.SaveTilesetButton.Click += new System.EventHandler(this.SaveTilemapButton_Click);
            // 
            // ClearTilemapButton
            // 
            this.ClearTilemapButton.Location = new System.Drawing.Point(133, 93);
            this.ClearTilemapButton.Name = "ClearTilemapButton";
            this.ClearTilemapButton.Size = new System.Drawing.Size(86, 23);
            this.ClearTilemapButton.TabIndex = 11;
            this.ClearTilemapButton.Text = "Clear Tilemap";
            this.ClearTilemapButton.UseVisualStyleBackColor = true;
            this.ClearTilemapButton.Click += new System.EventHandler(this.ClearTilemapButton_Click);
            // 
            // TilemapHeight
            // 
            this.TilemapHeight.Location = new System.Drawing.Point(223, 46);
            this.TilemapHeight.MaxLength = 4;
            this.TilemapHeight.Name = "TilemapHeight";
            this.TilemapHeight.Size = new System.Drawing.Size(38, 20);
            this.TilemapHeight.TabIndex = 6;
            this.TilemapHeight.Text = "1023";
            this.TilemapHeight.TextChanged += new System.EventHandler(this.Height_TextChanged);
            // 
            // TilemapWidth
            // 
            this.TilemapWidth.Location = new System.Drawing.Point(84, 46);
            this.TilemapWidth.MaxLength = 4;
            this.TilemapWidth.Name = "TilemapWidth";
            this.TilemapWidth.Size = new System.Drawing.Size(38, 20);
            this.TilemapWidth.TabIndex = 4;
            this.TilemapWidth.Text = "1023";
            this.TilemapWidth.TextChanged += new System.EventHandler(this.Width_TextChanged);
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Location = new System.Drawing.Point(154, 49);
            this.YLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(41, 13);
            this.YLabel.TabIndex = 5;
            this.YLabel.Text = "Height:";
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.Location = new System.Drawing.Point(14, 49);
            this.XLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(38, 13);
            this.XLabel.TabIndex = 3;
            this.XLabel.Text = "Width:";
            // 
            // TilemapAddress
            // 
            this.TilemapAddress.Location = new System.Drawing.Point(251, 17);
            this.TilemapAddress.MaxLength = 6;
            this.TilemapAddress.Name = "TilemapAddress";
            this.TilemapAddress.Size = new System.Drawing.Size(76, 20);
            this.TilemapAddress.TabIndex = 2;
            this.TilemapAddress.TextChanged += new System.EventHandler(this.TilemapAddress_TextChanged);
            // 
            // TilesetAddressLabel
            // 
            this.TilesetAddressLabel.AutoSize = true;
            this.TilesetAddressLabel.Location = new System.Drawing.Point(154, 20);
            this.TilesetAddressLabel.Name = "TilesetAddressLabel";
            this.TilesetAddressLabel.Size = new System.Drawing.Size(97, 13);
            this.TilesetAddressLabel.TabIndex = 1;
            this.TilesetAddressLabel.Text = "Tilemap Address: $";
            // 
            // TilemapEnabledCheckbox
            // 
            this.TilemapEnabledCheckbox.AutoSize = true;
            this.TilemapEnabledCheckbox.Location = new System.Drawing.Point(14, 19);
            this.TilemapEnabledCheckbox.Name = "TilemapEnabledCheckbox";
            this.TilemapEnabledCheckbox.Size = new System.Drawing.Size(105, 17);
            this.TilemapEnabledCheckbox.TabIndex = 0;
            this.TilemapEnabledCheckbox.Text = "Tilemap Enabled";
            this.TilemapEnabledCheckbox.UseVisualStyleBackColor = true;
            this.TilemapEnabledCheckbox.CheckedChanged += new System.EventHandler(this.TilemapEnabledCheckbox_CheckedChanged);
            // 
            // TilesetList
            // 
            this.TilesetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TilesetList.FormattingEnabled = true;
            this.TilesetList.Items.AddRange(new object[] {
            "Tileset 0",
            "Tileset 1",
            "Tileset 2",
            "Tileset 3",
            "Tileset 4",
            "Tileset 5",
            "Tileset 6",
            "Tileset 7"});
            this.TilesetList.Location = new System.Drawing.Point(54, 170);
            this.TilesetList.Name = "TilesetList";
            this.TilesetList.Size = new System.Drawing.Size(121, 21);
            this.TilesetList.TabIndex = 3;
            this.TilesetList.SelectedIndexChanged += new System.EventHandler(this.TilesetList_SelectedIndexChanged);
            // 
            // TilesetGroup
            // 
            this.TilesetGroup.Controls.Add(this.Stride256Checkbox);
            this.TilesetGroup.Controls.Add(this.LutList);
            this.TilesetGroup.Controls.Add(this.TilesetAddress);
            this.TilesetGroup.Controls.Add(this.label4);
            this.TilesetGroup.Location = new System.Drawing.Point(5, 192);
            this.TilesetGroup.Name = "TilesetGroup";
            this.TilesetGroup.Size = new System.Drawing.Size(355, 48);
            this.TilesetGroup.TabIndex = 5;
            this.TilesetGroup.TabStop = false;
            this.TilesetGroup.Text = "Tileset Properties";
            // 
            // Stride256Checkbox
            // 
            this.Stride256Checkbox.AutoSize = true;
            this.Stride256Checkbox.Location = new System.Drawing.Point(258, 19);
            this.Stride256Checkbox.Name = "Stride256Checkbox";
            this.Stride256Checkbox.Size = new System.Drawing.Size(93, 17);
            this.Stride256Checkbox.TabIndex = 3;
            this.Stride256Checkbox.Text = "Stride 16 Tiles";
            this.Stride256Checkbox.UseVisualStyleBackColor = true;
            this.Stride256Checkbox.CheckedChanged += new System.EventHandler(this.LutList_SelectedIndexChanged);
            // 
            // LutList
            // 
            this.LutList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LutList.FormattingEnabled = true;
            this.LutList.Items.AddRange(new object[] {
            "LUT 0",
            "LUT 1",
            "LUT 2",
            "LUT 3"});
            this.LutList.Location = new System.Drawing.Point(187, 18);
            this.LutList.Name = "LutList";
            this.LutList.Size = new System.Drawing.Size(62, 21);
            this.LutList.TabIndex = 2;
            this.LutList.SelectedIndexChanged += new System.EventHandler(this.LutList_SelectedIndexChanged);
            // 
            // TilesetAddress
            // 
            this.TilesetAddress.Location = new System.Drawing.Point(95, 18);
            this.TilesetAddress.MaxLength = 6;
            this.TilesetAddress.Name = "TilesetAddress";
            this.TilesetAddress.Size = new System.Drawing.Size(88, 20);
            this.TilesetAddress.TabIndex = 1;
            this.TilesetAddress.TextChanged += new System.EventHandler(this.TilesetAddress_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Tileset Address: $";
            // 
            // RightTileSelectedLabel
            // 
            this.RightTileSelectedLabel.AutoSize = true;
            this.RightTileSelectedLabel.Location = new System.Drawing.Point(285, 437);
            this.RightTileSelectedLabel.Name = "RightTileSelectedLabel";
            this.RightTileSelectedLabel.Size = new System.Drawing.Size(64, 13);
            this.RightTileSelectedLabel.TabIndex = 6;
            this.RightTileSelectedLabel.Text = "Right Tile: $";
            // 
            // leftTile
            // 
            this.leftTile.BackColor = System.Drawing.Color.White;
            this.leftTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.leftTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftTile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.leftTile.Location = new System.Drawing.Point(287, 246);
            this.leftTile.Margin = new System.Windows.Forms.Padding(0);
            this.leftTile.MinimumSize = new System.Drawing.Size(71, 70);
            this.leftTile.Name = "leftTile";
            this.leftTile.Size = new System.Drawing.Size(71, 70);
            this.leftTile.TabIndex = 7;
            this.leftTile.TabStop = false;
            this.leftTile.Paint += new System.Windows.Forms.PaintEventHandler(this.leftTile_Paint);
            // 
            // RightTile
            // 
            this.RightTile.BackColor = System.Drawing.Color.White;
            this.RightTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RightTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RightTile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RightTile.Location = new System.Drawing.Point(287, 362);
            this.RightTile.Margin = new System.Windows.Forms.Padding(0);
            this.RightTile.MinimumSize = new System.Drawing.Size(71, 70);
            this.RightTile.Name = "RightTile";
            this.RightTile.Size = new System.Drawing.Size(71, 70);
            this.RightTile.TabIndex = 8;
            this.RightTile.TabStop = false;
            this.RightTile.Paint += new System.Windows.Forms.PaintEventHandler(this.RightTile_Paint);
            // 
            // TileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 545);
            this.Controls.Add(this.RightTile);
            this.Controls.Add(this.leftTile);
            this.Controls.Add(this.RightTileSelectedLabel);
            this.Controls.Add(this.TilesetGroup);
            this.Controls.Add(this.TilesetList);
            this.Controls.Add(this.TilemapGroup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LeftTileSelectedLabel);
            this.Controls.Add(this.TilesetViewer);
            this.Controls.Add(this.HeaderPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "TileEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Tile Editor";
            this.Load += new System.EventHandler(this.TileEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TileEditor_KeyDown);
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TilesetViewer)).EndInit();
            this.TilemapGroup.ResumeLayout(false);
            this.TilemapGroup.PerformLayout();
            this.TilesetGroup.ResumeLayout(false);
            this.TilesetGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightTile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Button Tilemap3Button;
        private System.Windows.Forms.Button Tilemap2Button;
        private System.Windows.Forms.Button Tilemap1Button;
        private System.Windows.Forms.Label LeftTileSelectedLabel;
        private System.Windows.Forms.PictureBox TilesetViewer;
        private System.Windows.Forms.Button Tilemap0Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox TilemapGroup;
        private System.Windows.Forms.TextBox WindowY;
        private System.Windows.Forms.TextBox WindowX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SaveTilesetButton;
        private System.Windows.Forms.Button ClearTilemapButton;
        private System.Windows.Forms.TextBox TilemapHeight;
        private System.Windows.Forms.TextBox TilemapWidth;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.TextBox TilemapAddress;
        private System.Windows.Forms.Label TilesetAddressLabel;
        private System.Windows.Forms.CheckBox TilemapEnabledCheckbox;
        private System.Windows.Forms.ComboBox TilesetList;
        private System.Windows.Forms.GroupBox TilesetGroup;
        private System.Windows.Forms.TextBox TilesetAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox Stride256Checkbox;
        private System.Windows.Forms.ComboBox LutList;
        private System.Windows.Forms.Button btnMemory;
        private System.Windows.Forms.Label RightTileSelectedLabel;
        private System.Windows.Forms.PictureBox leftTile;
        private System.Windows.Forms.PictureBox RightTile;
        private System.Windows.Forms.CheckBox checkSmallTiles;
    }
}