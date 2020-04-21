namespace FoenixIDE.UI
{
    partial class WatchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatchForm));
            this.WatchGrid = new System.Windows.Forms.DataGridView();
            this.AddressName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Val8bit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Val16bit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.WatchUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.AddButton = new System.Windows.Forms.Button();
            this.NameText = new System.Windows.Forms.TextBox();
            this.AddressText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.WatchGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // WatchGrid
            // 
            this.WatchGrid.AllowUserToAddRows = false;
            this.WatchGrid.AllowUserToDeleteRows = false;
            this.WatchGrid.AllowUserToResizeRows = false;
            this.WatchGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WatchGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.WatchGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WatchGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AddressName,
            this.Address,
            this.Val8bit,
            this.Val16bit,
            this.ColDelete});
            this.WatchGrid.Location = new System.Drawing.Point(2, 28);
            this.WatchGrid.MultiSelect = false;
            this.WatchGrid.Name = "WatchGrid";
            this.WatchGrid.RowHeadersVisible = false;
            this.WatchGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.WatchGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.WatchGrid.ShowCellErrors = false;
            this.WatchGrid.ShowEditingIcon = false;
            this.WatchGrid.ShowRowErrors = false;
            this.WatchGrid.Size = new System.Drawing.Size(348, 422);
            this.WatchGrid.TabIndex = 3;
            this.WatchGrid.VirtualMode = true;
            this.WatchGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.WatchGrid_CellClick);
            // 
            // AddressName
            // 
            this.AddressName.FillWeight = 150F;
            this.AddressName.HeaderText = "Name";
            this.AddressName.MaxInputLength = 32;
            this.AddressName.Name = "AddressName";
            this.AddressName.ReadOnly = true;
            this.AddressName.Width = 120;
            // 
            // Address
            // 
            this.Address.FillWeight = 80F;
            this.Address.HeaderText = "Address";
            this.Address.MaxInputLength = 6;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Address.Width = 80;
            // 
            // Val8bit
            // 
            this.Val8bit.FillWeight = 40F;
            this.Val8bit.HeaderText = "8-Bits";
            this.Val8bit.MaxInputLength = 2;
            this.Val8bit.Name = "Val8bit";
            this.Val8bit.ReadOnly = true;
            this.Val8bit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Val8bit.Width = 40;
            // 
            // Val16bit
            // 
            this.Val16bit.FillWeight = 60F;
            this.Val16bit.HeaderText = "16-Bits";
            this.Val16bit.MaxInputLength = 4;
            this.Val16bit.Name = "Val16bit";
            this.Val16bit.ReadOnly = true;
            this.Val16bit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Val16bit.Width = 60;
            // 
            // ColDelete
            // 
            this.ColDelete.FillWeight = 24F;
            this.ColDelete.HeaderText = "D";
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.ReadOnly = true;
            this.ColDelete.Width = 24;
            // 
            // WatchUpdateTimer
            // 
            this.WatchUpdateTimer.Enabled = true;
            this.WatchUpdateTimer.Interval = 1000;
            this.WatchUpdateTimer.Tick += new System.EventHandler(this.WatchUpdateTimer_Tick);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(271, 3);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // NameText
            // 
            this.NameText.Location = new System.Drawing.Point(3, 5);
            this.NameText.MaxLength = 32;
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(121, 20);
            this.NameText.TabIndex = 0;
            // 
            // AddressText
            // 
            this.AddressText.Location = new System.Drawing.Point(127, 5);
            this.AddressText.MaxLength = 6;
            this.AddressText.Name = "AddressText";
            this.AddressText.Size = new System.Drawing.Size(76, 20);
            this.AddressText.TabIndex = 1;
            // 
            // WatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 450);
            this.Controls.Add(this.AddressText);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.WatchGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(367, 489);
            this.Name = "WatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Watch List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WatchForm_FormClosing);
            this.Load += new System.EventHandler(this.Watch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WatchForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.WatchGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView WatchGrid;
        private System.Windows.Forms.Timer WatchUpdateTimer;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Val8bit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Val16bit;
        private System.Windows.Forms.DataGridViewImageColumn ColDelete;
        private System.Windows.Forms.TextBox NameText;
        private System.Windows.Forms.TextBox AddressText;
    }
}