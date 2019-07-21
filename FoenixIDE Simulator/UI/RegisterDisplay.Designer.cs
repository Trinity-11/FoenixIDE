namespace FoenixIDE
{
    partial class RegisterDisplay
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Flags = new FoenixIDE.UI.RegisterControl();
            this.panel7 = new System.Windows.Forms.Panel();
            this.D = new FoenixIDE.UI.RegisterControl();
            this.panel6 = new System.Windows.Forms.Panel();
            this.DBR = new FoenixIDE.UI.RegisterControl();
            this.panel5 = new System.Windows.Forms.Panel();
            this.Stack = new FoenixIDE.UI.RegisterControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Y = new FoenixIDE.UI.RegisterControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.X = new FoenixIDE.UI.RegisterControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.A = new FoenixIDE.UI.RegisterControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PC = new FoenixIDE.UI.RegisterControl();
            this.panel8 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Flags);
            this.groupBox1.Controls.Add(this.panel7);
            this.groupBox1.Controls.Add(this.D);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Controls.Add(this.DBR);
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.Stack);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.Y);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.X);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.A);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.PC);
            this.groupBox1.Controls.Add(this.panel8);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registers";
            // 
            // Flags
            // 
            this.Flags.Bank = null;
            this.Flags.Caption = "Flags";
            this.Flags.Dock = System.Windows.Forms.DockStyle.Left;
            this.Flags.Location = new System.Drawing.Point(277, 16);
            this.Flags.Name = "Flags";
            this.Flags.Register = null;
            this.Flags.Size = new System.Drawing.Size(76, 34);
            this.Flags.TabIndex = 14;
            this.Flags.Value = "XXXXXXXX";
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(273, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(4, 34);
            this.panel7.TabIndex = 13;
            // 
            // D
            // 
            this.D.Bank = null;
            this.D.Caption = "Direct";
            this.D.Dock = System.Windows.Forms.DockStyle.Left;
            this.D.Location = new System.Drawing.Point(240, 16);
            this.D.Name = "D";
            this.D.Register = null;
            this.D.Size = new System.Drawing.Size(33, 34);
            this.D.TabIndex = 12;
            this.D.Value = "0000";
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(236, 16);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(4, 34);
            this.panel6.TabIndex = 11;
            // 
            // DBR
            // 
            this.DBR.Bank = null;
            this.DBR.Caption = "DBR";
            this.DBR.Dock = System.Windows.Forms.DockStyle.Left;
            this.DBR.Location = new System.Drawing.Point(203, 16);
            this.DBR.Name = "DBR";
            this.DBR.Register = null;
            this.DBR.Size = new System.Drawing.Size(33, 34);
            this.DBR.TabIndex = 10;
            this.DBR.Value = "0000";
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(199, 16);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(4, 34);
            this.panel5.TabIndex = 9;
            // 
            // Stack
            // 
            this.Stack.Bank = null;
            this.Stack.Caption = "S";
            this.Stack.Dock = System.Windows.Forms.DockStyle.Left;
            this.Stack.Location = new System.Drawing.Point(166, 16);
            this.Stack.Name = "Stack";
            this.Stack.Register = null;
            this.Stack.Size = new System.Drawing.Size(33, 34);
            this.Stack.TabIndex = 8;
            this.Stack.Value = "0000";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(162, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(4, 34);
            this.panel4.TabIndex = 7;
            // 
            // Y
            // 
            this.Y.Bank = null;
            this.Y.Caption = "Y";
            this.Y.Dock = System.Windows.Forms.DockStyle.Left;
            this.Y.Location = new System.Drawing.Point(129, 16);
            this.Y.Name = "Y";
            this.Y.Register = null;
            this.Y.Size = new System.Drawing.Size(33, 34);
            this.Y.TabIndex = 6;
            this.Y.Value = "0000";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(125, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(4, 34);
            this.panel3.TabIndex = 5;
            // 
            // X
            // 
            this.X.Bank = null;
            this.X.Caption = "X";
            this.X.Dock = System.Windows.Forms.DockStyle.Left;
            this.X.Location = new System.Drawing.Point(92, 16);
            this.X.Name = "X";
            this.X.Register = null;
            this.X.Size = new System.Drawing.Size(33, 34);
            this.X.TabIndex = 4;
            this.X.Value = "0000";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(88, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(4, 34);
            this.panel2.TabIndex = 3;
            // 
            // A
            // 
            this.A.Bank = null;
            this.A.Caption = "A";
            this.A.Dock = System.Windows.Forms.DockStyle.Left;
            this.A.Location = new System.Drawing.Point(55, 16);
            this.A.Name = "A";
            this.A.Register = null;
            this.A.Size = new System.Drawing.Size(33, 34);
            this.A.TabIndex = 2;
            this.A.Value = "0000";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(51, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(4, 34);
            this.panel1.TabIndex = 1;
            // 
            // PC
            // 
            this.PC.Bank = null;
            this.PC.Caption = "PC";
            this.PC.Dock = System.Windows.Forms.DockStyle.Left;
            this.PC.Location = new System.Drawing.Point(7, 16);
            this.PC.Name = "PC";
            this.PC.Register = null;
            this.PC.Size = new System.Drawing.Size(44, 34);
            this.PC.TabIndex = 0;
            this.PC.Value = "000000";
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(3, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(4, 34);
            this.panel8.TabIndex = 15;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // RegisterDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "RegisterDisplay";
            this.Size = new System.Drawing.Size(466, 53);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.GroupBox groupBox1;
        private UI.RegisterControl A;
        private global::System.Windows.Forms.Panel panel1;
        private UI.RegisterControl PC;
        private UI.RegisterControl DBR;
        private global::System.Windows.Forms.Panel panel5;
        private UI.RegisterControl Stack;
        private global::System.Windows.Forms.Panel panel4;
        private UI.RegisterControl Y;
        private global::System.Windows.Forms.Panel panel3;
        private UI.RegisterControl X;
        private global::System.Windows.Forms.Panel panel2;
        private UI.RegisterControl Flags;
        private global::System.Windows.Forms.Panel panel7;
        private UI.RegisterControl D;
        private global::System.Windows.Forms.Panel panel6;
        private global::System.Windows.Forms.Panel panel8;
        private global::System.Windows.Forms.Timer timer1;
    }
}
