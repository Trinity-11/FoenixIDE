namespace FoenixIDE.UI
{
    partial class GameGeneratorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameGeneratorForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CodeTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbEVID = new System.Windows.Forms.CheckBox();
            this.cbCollision1 = new System.Windows.Forms.CheckBox();
            this.cbCollision0 = new System.Windows.Forms.CheckBox();
            this.cbKeyboard = new System.Windows.Forms.CheckBox();
            this.cbMouse = new System.Windows.Forms.CheckBox();
            this.cbTimer2 = new System.Windows.Forms.CheckBox();
            this.cbTimer1 = new System.Windows.Forms.CheckBox();
            this.cbSOL = new System.Windows.Forms.CheckBox();
            this.cbSOF = new System.Windows.Forms.CheckBox();
            this.cbTimer0 = new System.Windows.Forms.CheckBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ViewAssetsButton = new System.Windows.Forms.Button();
            this.GenerateASMButton = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodeTextBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.CodeTextBox);
            this.panel1.Location = new System.Drawing.Point(3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 531);
            this.panel1.TabIndex = 0;
            // 
            // CodeTextBox
            // 
            this.CodeTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.CodeTextBox.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]" +
    "*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.CodeTextBox.AutoScrollMinSize = new System.Drawing.Size(634, 675);
            this.CodeTextBox.BackBrush = null;
            this.CodeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CodeTextBox.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.CodeTextBox.CharHeight = 15;
            this.CodeTextBox.CharWidth = 7;
            this.CodeTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.CodeTextBox.DelayedEventsInterval = 500;
            this.CodeTextBox.DelayedTextChangedInterval = 500;
            this.CodeTextBox.DescriptionFile = "C:\\Working\\foenix\\emulator\\Main\\Resources\\foenix-game-generator-syntax.xml";
            this.CodeTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.CodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.CodeTextBox.IsReplaceMode = false;
            this.CodeTextBox.LeftBracket = '(';
            this.CodeTextBox.LeftBracket2 = '{';
            this.CodeTextBox.Location = new System.Drawing.Point(0, 0);
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.CodeTextBox.RightBracket = ')';
            this.CodeTextBox.RightBracket2 = '}';
            this.CodeTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.CodeTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("CodeTextBox.ServiceColors")));
            this.CodeTextBox.Size = new System.Drawing.Size(452, 531);
            this.CodeTextBox.TabIndex = 4;
            this.CodeTextBox.Text = resources.GetString("CodeTextBox.Text");
            this.CodeTextBox.Zoom = 100;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.cbEVID);
            this.panel2.Controls.Add(this.cbCollision1);
            this.panel2.Controls.Add(this.cbCollision0);
            this.panel2.Controls.Add(this.cbKeyboard);
            this.panel2.Controls.Add(this.cbMouse);
            this.panel2.Controls.Add(this.cbTimer2);
            this.panel2.Controls.Add(this.cbTimer1);
            this.panel2.Controls.Add(this.cbSOL);
            this.panel2.Controls.Add(this.cbSOF);
            this.panel2.Controls.Add(this.cbTimer0);
            this.panel2.Controls.Add(this.CloseButton);
            this.panel2.Controls.Add(this.SaveButton);
            this.panel2.Controls.Add(this.LoadButton);
            this.panel2.Controls.Add(this.ViewAssetsButton);
            this.panel2.Controls.Add(this.GenerateASMButton);
            this.panel2.Location = new System.Drawing.Point(461, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(113, 531);
            this.panel2.TabIndex = 1;
            // 
            // cbEVID
            // 
            this.cbEVID.AutoSize = true;
            this.cbEVID.Location = new System.Drawing.Point(8, 388);
            this.cbEVID.Name = "cbEVID";
            this.cbEVID.Size = new System.Drawing.Size(91, 30);
            this.cbEVID.TabIndex = 14;
            this.cbEVID.Text = "Send Debug\r\nCode to EVID";
            this.cbEVID.UseVisualStyleBackColor = true;
            // 
            // cbCollision1
            // 
            this.cbCollision1.AutoSize = true;
            this.cbCollision1.Location = new System.Drawing.Point(9, 343);
            this.cbCollision1.Name = "cbCollision1";
            this.cbCollision1.Size = new System.Drawing.Size(96, 17);
            this.cbCollision1.TabIndex = 13;
            this.cbCollision1.Text = "STT_COL IRQ";
            this.cbCollision1.UseVisualStyleBackColor = true;
            this.cbCollision1.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbCollision0
            // 
            this.cbCollision0.AutoSize = true;
            this.cbCollision0.Location = new System.Drawing.Point(9, 320);
            this.cbCollision0.Name = "cbCollision0";
            this.cbCollision0.Size = new System.Drawing.Size(96, 17);
            this.cbCollision0.TabIndex = 12;
            this.cbCollision0.Text = "STS_COL IRQ";
            this.cbCollision0.UseVisualStyleBackColor = true;
            this.cbCollision0.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbKeyboard
            // 
            this.cbKeyboard.AutoSize = true;
            this.cbKeyboard.Location = new System.Drawing.Point(9, 297);
            this.cbKeyboard.Name = "cbKeyboard";
            this.cbKeyboard.Size = new System.Drawing.Size(93, 17);
            this.cbKeyboard.TabIndex = 11;
            this.cbKeyboard.Text = "Keyboard IRQ";
            this.cbKeyboard.UseVisualStyleBackColor = true;
            this.cbKeyboard.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbMouse
            // 
            this.cbMouse.AutoSize = true;
            this.cbMouse.Location = new System.Drawing.Point(9, 264);
            this.cbMouse.Name = "cbMouse";
            this.cbMouse.Size = new System.Drawing.Size(80, 17);
            this.cbMouse.TabIndex = 10;
            this.cbMouse.Text = "Mouse IRQ";
            this.cbMouse.UseVisualStyleBackColor = true;
            this.cbMouse.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbTimer2
            // 
            this.cbTimer2.AutoSize = true;
            this.cbTimer2.Location = new System.Drawing.Point(9, 241);
            this.cbTimer2.Name = "cbTimer2";
            this.cbTimer2.Size = new System.Drawing.Size(80, 17);
            this.cbTimer2.TabIndex = 9;
            this.cbTimer2.Text = "Timer2 IRQ";
            this.cbTimer2.UseVisualStyleBackColor = true;
            this.cbTimer2.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbTimer1
            // 
            this.cbTimer1.AutoSize = true;
            this.cbTimer1.Location = new System.Drawing.Point(9, 218);
            this.cbTimer1.Name = "cbTimer1";
            this.cbTimer1.Size = new System.Drawing.Size(80, 17);
            this.cbTimer1.TabIndex = 8;
            this.cbTimer1.Text = "Timer1 IRQ";
            this.cbTimer1.UseVisualStyleBackColor = true;
            this.cbTimer1.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbSOL
            // 
            this.cbSOL.AutoSize = true;
            this.cbSOL.Location = new System.Drawing.Point(9, 172);
            this.cbSOL.Name = "cbSOL";
            this.cbSOL.Size = new System.Drawing.Size(69, 17);
            this.cbSOL.TabIndex = 7;
            this.cbSOL.Text = "SOL IRQ";
            this.cbSOL.UseVisualStyleBackColor = true;
            this.cbSOL.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbSOF
            // 
            this.cbSOF.AutoSize = true;
            this.cbSOF.Location = new System.Drawing.Point(9, 149);
            this.cbSOF.Name = "cbSOF";
            this.cbSOF.Size = new System.Drawing.Size(69, 17);
            this.cbSOF.TabIndex = 6;
            this.cbSOF.Text = "SOF IRQ";
            this.cbSOF.UseVisualStyleBackColor = true;
            this.cbSOF.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // cbTimer0
            // 
            this.cbTimer0.AutoSize = true;
            this.cbTimer0.Location = new System.Drawing.Point(9, 195);
            this.cbTimer0.Name = "cbTimer0";
            this.cbTimer0.Size = new System.Drawing.Size(80, 17);
            this.cbTimer0.TabIndex = 5;
            this.cbTimer0.Text = "Timer0 IRQ";
            this.cbTimer0.UseVisualStyleBackColor = true;
            this.cbTimer0.CheckedChanged += new System.EventHandler(this.cbIRQ_CheckedChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CloseButton.Location = new System.Drawing.Point(9, 504);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(98, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(9, 107);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(98, 23);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save Game";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(9, 78);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(98, 23);
            this.LoadButton.TabIndex = 2;
            this.LoadButton.Text = "Load Game";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // ViewAssetsButton
            // 
            this.ViewAssetsButton.Location = new System.Drawing.Point(8, 37);
            this.ViewAssetsButton.Name = "ViewAssetsButton";
            this.ViewAssetsButton.Size = new System.Drawing.Size(98, 23);
            this.ViewAssetsButton.TabIndex = 1;
            this.ViewAssetsButton.Text = "View Assets";
            this.ViewAssetsButton.UseVisualStyleBackColor = true;
            this.ViewAssetsButton.Click += new System.EventHandler(this.ViewAssetsButton_Click);
            // 
            // GenerateASMButton
            // 
            this.GenerateASMButton.Location = new System.Drawing.Point(8, 8);
            this.GenerateASMButton.Name = "GenerateASMButton";
            this.GenerateASMButton.Size = new System.Drawing.Size(98, 23);
            this.GenerateASMButton.TabIndex = 0;
            this.GenerateASMButton.Text = "Generate ASM";
            this.GenerateASMButton.UseVisualStyleBackColor = true;
            this.GenerateASMButton.Click += new System.EventHandler(this.GenerateASMButton_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // GameGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 537);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(596, 576);
            this.Name = "GameGeneratorForm";
            this.Text = "Game Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameGeneratorForm_FormClosing);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CodeTextBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button ViewAssetsButton;
        private System.Windows.Forms.Button GenerateASMButton;
        private System.Windows.Forms.CheckBox cbTimer0;
        private System.Windows.Forms.CheckBox cbSOF;
        private System.Windows.Forms.CheckBox cbMouse;
        private System.Windows.Forms.CheckBox cbTimer2;
        private System.Windows.Forms.CheckBox cbTimer1;
        private System.Windows.Forms.CheckBox cbSOL;
        private System.Windows.Forms.CheckBox cbKeyboard;
        private System.Windows.Forms.CheckBox cbCollision0;
        private System.Windows.Forms.CheckBox cbCollision1;
        private FastColoredTextBoxNS.FastColoredTextBox CodeTextBox;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.CheckBox cbEVID;
    }
}

