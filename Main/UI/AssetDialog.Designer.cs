
namespace FoenixIDE.UI
{
    partial class AssetWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetWindow));
            this.AssetGrid = new System.Windows.Forms.DataGridView();
            this.AddressName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Val8bit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Val16bit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Export = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColMemory = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.AddAssetButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AssetGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // AssetGrid
            // 
            this.AssetGrid.AllowUserToAddRows = false;
            this.AssetGrid.AllowUserToDeleteRows = false;
            this.AssetGrid.AllowUserToResizeRows = false;
            this.AssetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.AssetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AssetGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AddressName,
            this.Address,
            this.Val8bit,
            this.Val16bit,
            this.Export,
            this.ColMemory,
            this.ColDelete});
            this.AssetGrid.Location = new System.Drawing.Point(-2, -1);
            this.AssetGrid.MultiSelect = false;
            this.AssetGrid.Name = "AssetGrid";
            this.AssetGrid.RowHeadersVisible = false;
            this.AssetGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AssetGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AssetGrid.ShowCellErrors = false;
            this.AssetGrid.ShowEditingIcon = false;
            this.AssetGrid.ShowRowErrors = false;
            this.AssetGrid.Size = new System.Drawing.Size(533, 195);
            this.AssetGrid.TabIndex = 4;
            this.AssetGrid.VirtualMode = true;
            this.AssetGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AssetGrid_CellClick);
            // 
            // AddressName
            // 
            this.AddressName.FillWeight = 150F;
            this.AddressName.HeaderText = "Name";
            this.AddressName.MaxInputLength = 48;
            this.AddressName.Name = "AddressName";
            this.AddressName.ReadOnly = true;
            this.AddressName.Width = 160;
            // 
            // Address
            // 
            this.Address.HeaderText = "Start Address";
            this.Address.MaxInputLength = 6;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Val8bit
            // 
            this.Val8bit.HeaderText = "End Address";
            this.Val8bit.MaxInputLength = 6;
            this.Val8bit.Name = "Val8bit";
            this.Val8bit.ReadOnly = true;
            this.Val8bit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Val16bit
            // 
            this.Val16bit.FillWeight = 60F;
            this.Val16bit.HeaderText = "Type";
            this.Val16bit.MaxInputLength = 10;
            this.Val16bit.Name = "Val16bit";
            this.Val16bit.ReadOnly = true;
            this.Val16bit.Width = 80;
            // 
            // Export
            // 
            this.Export.FillWeight = 24F;
            this.Export.HeaderText = "Ex";
            this.Export.Image = global::FoenixIDE.Simulator.Properties.Resources.save_btn;
            this.Export.MinimumWidth = 24;
            this.Export.Name = "Export";
            this.Export.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Export.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Export.ToolTipText = "Export Data to File";
            this.Export.Width = 24;
            // 
            // ColMemory
            // 
            this.ColMemory.FillWeight = 24F;
            this.ColMemory.HeaderText = "M";
            this.ColMemory.Image = global::FoenixIDE.Simulator.Properties.Resources.memory_btn;
            this.ColMemory.MinimumWidth = 24;
            this.ColMemory.Name = "ColMemory";
            this.ColMemory.ReadOnly = true;
            this.ColMemory.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColMemory.ToolTipText = "Show in Memory Window";
            this.ColMemory.Width = 24;
            // 
            // ColDelete
            // 
            this.ColDelete.FillWeight = 24F;
            this.ColDelete.HeaderText = "D";
            this.ColDelete.Image = global::FoenixIDE.Simulator.Properties.Resources.delete_btn;
            this.ColDelete.MinimumWidth = 24;
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.ReadOnly = true;
            this.ColDelete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColDelete.ToolTipText = "Delete Item";
            this.ColDelete.Width = 24;
            // 
            // AddAssetButton
            // 
            this.AddAssetButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.AddAssetButton.Location = new System.Drawing.Point(224, 196);
            this.AddAssetButton.Name = "AddAssetButton";
            this.AddAssetButton.Size = new System.Drawing.Size(75, 23);
            this.AddAssetButton.TabIndex = 5;
            this.AddAssetButton.Text = "Add Asset";
            this.AddAssetButton.UseVisualStyleBackColor = true;
            this.AddAssetButton.Click += new System.EventHandler(this.AddAssetButton_Click);
            // 
            // AssetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 221);
            this.Controls.Add(this.AddAssetButton);
            this.Controls.Add(this.AssetGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(549, 1000);
            this.MinimumSize = new System.Drawing.Size(549, 260);
            this.Name = "AssetWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Asset Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AssetWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.AssetGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView AssetGrid;
        private System.Windows.Forms.Button AddAssetButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Val8bit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Val16bit;
        private System.Windows.Forms.DataGridViewImageColumn Export;
        private System.Windows.Forms.DataGridViewImageColumn ColMemory;
        private System.Windows.Forms.DataGridViewImageColumn ColDelete;
    }
}