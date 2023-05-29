namespace FoenixIDE.UI
{
    partial class MIDI_VGM_From
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MIDI_VGM_From));
            this.MIDIOutputText = new System.Windows.Forms.TextBox();
            this.ReadFileButton = new System.Windows.Forms.Button();
            this.FileLabel = new System.Windows.Forms.Label();
            this.GeneratePanel = new System.Windows.Forms.Panel();
            this.PercussionMode = new System.Windows.Forms.CheckBox();
            this.GenerateVGMButton = new System.Windows.Forms.Button();
            this.gridSummary = new System.Windows.Forms.DataGridView();
            this.colTrackNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMidiEventCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMidiChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPolyBool = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMultiChannelBool = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.GeneratePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // MIDIOutputText
            // 
            this.MIDIOutputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutputText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MIDIOutputText.Location = new System.Drawing.Point(394, 82);
            this.MIDIOutputText.MaxLength = 65536;
            this.MIDIOutputText.Multiline = true;
            this.MIDIOutputText.Name = "MIDIOutputText";
            this.MIDIOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MIDIOutputText.Size = new System.Drawing.Size(394, 357);
            this.MIDIOutputText.TabIndex = 3;
            this.MIDIOutputText.WordWrap = false;
            // 
            // ReadFileButton
            // 
            this.ReadFileButton.Location = new System.Drawing.Point(12, 3);
            this.ReadFileButton.Name = "ReadFileButton";
            this.ReadFileButton.Size = new System.Drawing.Size(75, 23);
            this.ReadFileButton.TabIndex = 2;
            this.ReadFileButton.Text = "Read File";
            this.ReadFileButton.UseVisualStyleBackColor = true;
            this.ReadFileButton.Click += new System.EventHandler(this.ReadFileButton_Click);
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(93, 11);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(0, 13);
            this.FileLabel.TabIndex = 4;
            // 
            // GeneratePanel
            // 
            this.GeneratePanel.Controls.Add(this.PercussionMode);
            this.GeneratePanel.Controls.Add(this.GenerateVGMButton);
            this.GeneratePanel.Enabled = false;
            this.GeneratePanel.Location = new System.Drawing.Point(10, 53);
            this.GeneratePanel.Name = "GeneratePanel";
            this.GeneratePanel.Size = new System.Drawing.Size(778, 26);
            this.GeneratePanel.TabIndex = 10;
            // 
            // PercussionMode
            // 
            this.PercussionMode.AutoSize = true;
            this.PercussionMode.Checked = true;
            this.PercussionMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PercussionMode.Location = new System.Drawing.Point(108, 5);
            this.PercussionMode.Name = "PercussionMode";
            this.PercussionMode.Size = new System.Drawing.Size(174, 17);
            this.PercussionMode.TabIndex = 11;
            this.PercussionMode.Text = "Enable Percussion Mode ($BD)";
            this.PercussionMode.UseVisualStyleBackColor = true;
            this.PercussionMode.CheckedChanged += new System.EventHandler(this.PercussionMode_CheckedChanged);
            // 
            // GenerateVGMButton
            // 
            this.GenerateVGMButton.Location = new System.Drawing.Point(2, 1);
            this.GenerateVGMButton.Name = "GenerateVGMButton";
            this.GenerateVGMButton.Size = new System.Drawing.Size(100, 23);
            this.GenerateVGMButton.TabIndex = 10;
            this.GenerateVGMButton.Text = "Generate VGM";
            this.GenerateVGMButton.UseVisualStyleBackColor = true;
            this.GenerateVGMButton.Click += new System.EventHandler(this.GenerateVGMButton_Click);
            // 
            // gridSummary
            // 
            this.gridSummary.AllowUserToAddRows = false;
            this.gridSummary.AllowUserToDeleteRows = false;
            this.gridSummary.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSummary.ColumnHeadersHeight = 25;
            this.gridSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTrackNumber,
            this.colDuration,
            this.colMidiEventCount,
            this.colMidiChannel,
            this.colPolyBool,
            this.colMultiChannelBool});
            this.gridSummary.EnableHeadersVisualStyles = false;
            this.gridSummary.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.gridSummary.Location = new System.Drawing.Point(10, 84);
            this.gridSummary.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gridSummary.MultiSelect = false;
            this.gridSummary.Name = "gridSummary";
            this.gridSummary.ReadOnly = true;
            this.gridSummary.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridSummary.RowHeadersVisible = false;
            this.gridSummary.RowHeadersWidth = 21;
            this.gridSummary.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Lime;
            this.gridSummary.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridSummary.RowTemplate.Height = 19;
            this.gridSummary.RowTemplate.ReadOnly = true;
            this.gridSummary.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSummary.ShowCellErrors = false;
            this.gridSummary.ShowCellToolTips = false;
            this.gridSummary.ShowEditingIcon = false;
            this.gridSummary.ShowRowErrors = false;
            this.gridSummary.Size = new System.Drawing.Size(379, 356);
            this.gridSummary.TabIndex = 11;
            this.gridSummary.Click += new System.EventHandler(this.gridSummary_Click);
            // 
            // colTrackNumber
            // 
            this.colTrackNumber.DataPropertyName = "Index";
            this.colTrackNumber.HeaderText = "Track";
            this.colTrackNumber.MaxInputLength = 2;
            this.colTrackNumber.MinimumWidth = 6;
            this.colTrackNumber.Name = "colTrackNumber";
            this.colTrackNumber.ReadOnly = true;
            this.colTrackNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTrackNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTrackNumber.Width = 65;
            // 
            // colDuration
            // 
            this.colDuration.DataPropertyName = "TotalTime";
            this.colDuration.HeaderText = "Duration";
            this.colDuration.MaxInputLength = 10;
            this.colDuration.MinimumWidth = 6;
            this.colDuration.Name = "colDuration";
            this.colDuration.ReadOnly = true;
            this.colDuration.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDuration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDuration.Width = 90;
            // 
            // colMidiEventCount
            // 
            this.colMidiEventCount.DataPropertyName = "EventCount";
            this.colMidiEventCount.HeaderText = "Event #";
            this.colMidiEventCount.MinimumWidth = 6;
            this.colMidiEventCount.Name = "colMidiEventCount";
            this.colMidiEventCount.ReadOnly = true;
            this.colMidiEventCount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMidiEventCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMidiEventCount.Width = 75;
            // 
            // colMidiChannel
            // 
            this.colMidiChannel.DataPropertyName = "MidiChannel";
            this.colMidiChannel.HeaderText = "Chnl";
            this.colMidiChannel.MinimumWidth = 6;
            this.colMidiChannel.Name = "colMidiChannel";
            this.colMidiChannel.ReadOnly = true;
            this.colMidiChannel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMidiChannel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMidiChannel.Width = 40;
            // 
            // colPolyBool
            // 
            this.colPolyBool.DataPropertyName = "isPoly";
            this.colPolyBool.HeaderText = "Poly";
            this.colPolyBool.MinimumWidth = 2;
            this.colPolyBool.Name = "colPolyBool";
            this.colPolyBool.ReadOnly = true;
            this.colPolyBool.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colPolyBool.Width = 40;
            // 
            // colMultiChannelBool
            // 
            this.colMultiChannelBool.DataPropertyName = "isMultiChannel";
            this.colMultiChannelBool.HeaderText = "MD";
            this.colMultiChannelBool.MinimumWidth = 2;
            this.colMultiChannelBool.Name = "colMultiChannelBool";
            this.colMultiChannelBool.ReadOnly = true;
            this.colMultiChannelBool.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMultiChannelBool.Width = 40;
            // 
            // MIDI_VGM_From
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gridSummary);
            this.Controls.Add(this.GeneratePanel);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.MIDIOutputText);
            this.Controls.Add(this.ReadFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MIDI_VGM_From";
            this.Text = "MIDI to VGM Conversion";
            this.GeneratePanel.ResumeLayout(false);
            this.GeneratePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSummary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MIDIOutputText;
        private System.Windows.Forms.Button ReadFileButton;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Panel GeneratePanel;
        private System.Windows.Forms.CheckBox PercussionMode;
        private System.Windows.Forms.Button GenerateVGMButton;
        private System.Windows.Forms.DataGridView gridSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMidiEventCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMidiChannel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPolyBool;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMultiChannelBool;
    }
}

