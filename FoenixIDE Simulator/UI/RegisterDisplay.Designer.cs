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
            this.components = new global::System.ComponentModel.Container();
            this.groupBox1 = new global::System.Windows.Forms.GroupBox();
            this.panel7 = new global::System.Windows.Forms.Panel();
            this.panel6 = new global::System.Windows.Forms.Panel();
            this.panel5 = new global::System.Windows.Forms.Panel();
            this.panel4 = new global::System.Windows.Forms.Panel();
            this.panel3 = new global::System.Windows.Forms.Panel();
            this.panel2 = new global::System.Windows.Forms.Panel();
            this.panel1 = new global::System.Windows.Forms.Panel();
            this.panel8 = new global::System.Windows.Forms.Panel();
            this.timer1 = new global::System.Windows.Forms.Timer(this.components);
            this.Flags = new FoenixIDE.UI.RegisterControl();
            this.D = new FoenixIDE.UI.RegisterControl();
            this.DBR = new FoenixIDE.UI.RegisterControl();
            this.Stack = new FoenixIDE.UI.RegisterControl();
            this.Y = new FoenixIDE.UI.RegisterControl();
            this.X = new FoenixIDE.UI.RegisterControl();
            this.A = new FoenixIDE.UI.RegisterControl();
            this.PC = new FoenixIDE.UI.RegisterControl();
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
            this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new global::System.Drawing.Size(466, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registers";
            // 
            // panel7
            // 
            this.panel7.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new global::System.Drawing.Point(381, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new global::System.Drawing.Size(4, 45);
            this.panel7.TabIndex = 13;
            // 
            // panel6
            // 
            this.panel6.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new global::System.Drawing.Point(327, 16);
            this.panel6.Name = "panel6";
            this.panel6.Size = new global::System.Drawing.Size(4, 45);
            this.panel6.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new global::System.Drawing.Point(273, 16);
            this.panel5.Name = "panel5";
            this.panel5.Size = new global::System.Drawing.Size(4, 45);
            this.panel5.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new global::System.Drawing.Point(219, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new global::System.Drawing.Size(4, 45);
            this.panel4.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new global::System.Drawing.Point(165, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new global::System.Drawing.Size(4, 45);
            this.panel3.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new global::System.Drawing.Point(111, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new global::System.Drawing.Size(4, 45);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new global::System.Drawing.Point(57, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new global::System.Drawing.Size(4, 45);
            this.panel1.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new global::System.Drawing.Point(3, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new global::System.Drawing.Size(4, 45);
            this.panel8.TabIndex = 15;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
            // 
            // Flags
            // 
            this.Flags.Bank = null;
            this.Flags.Caption = "Flags";
            this.Flags.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.Flags.Location = new global::System.Drawing.Point(385, 16);
            this.Flags.Name = "Flags";
            this.Flags.Register = null;
            this.Flags.Size = new global::System.Drawing.Size(69, 45);
            this.Flags.TabIndex = 14;
            this.Flags.Value = "XXXXXXXX";
            // 
            // D
            // 
            this.D.Bank = null;
            this.D.Caption = "Direct";
            this.D.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.D.Location = new global::System.Drawing.Point(331, 16);
            this.D.Name = "D";
            this.D.Register = null;
            this.D.Size = new global::System.Drawing.Size(50, 45);
            this.D.TabIndex = 12;
            this.D.Value = "0000";
            // 
            // DBR
            // 
            this.DBR.Bank = null;
            this.DBR.Caption = "DBR";
            this.DBR.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.DBR.Location = new global::System.Drawing.Point(277, 16);
            this.DBR.Name = "DBR";
            this.DBR.Register = null;
            this.DBR.Size = new global::System.Drawing.Size(50, 45);
            this.DBR.TabIndex = 10;
            this.DBR.Value = "0000";
            // 
            // Stack
            // 
            this.Stack.Bank = null;
            this.Stack.Caption = "S";
            this.Stack.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.Stack.Location = new global::System.Drawing.Point(223, 16);
            this.Stack.Name = "Stack";
            this.Stack.Register = null;
            this.Stack.Size = new global::System.Drawing.Size(50, 45);
            this.Stack.TabIndex = 8;
            this.Stack.Value = "0000";
            // 
            // Y
            // 
            this.Y.Bank = null;
            this.Y.Caption = "Y";
            this.Y.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.Y.Location = new global::System.Drawing.Point(169, 16);
            this.Y.Name = "Y";
            this.Y.Register = null;
            this.Y.Size = new global::System.Drawing.Size(50, 45);
            this.Y.TabIndex = 6;
            this.Y.Value = "0000";
            // 
            // X
            // 
            this.X.Bank = null;
            this.X.Caption = "X";
            this.X.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.X.Location = new global::System.Drawing.Point(115, 16);
            this.X.Name = "X";
            this.X.Register = null;
            this.X.Size = new global::System.Drawing.Size(50, 45);
            this.X.TabIndex = 4;
            this.X.Value = "0000";
            // 
            // A
            // 
            this.A.Bank = null;
            this.A.Caption = "A";
            this.A.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.A.Location = new global::System.Drawing.Point(61, 16);
            this.A.Name = "A";
            this.A.Register = null;
            this.A.Size = new global::System.Drawing.Size(50, 45);
            this.A.TabIndex = 2;
            this.A.Value = "0000";
            // 
            // PC
            // 
            this.PC.Bank = null;
            this.PC.Caption = "PC";
            this.PC.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.PC.Location = new global::System.Drawing.Point(7, 16);
            this.PC.Name = "PC";
            this.PC.Register = null;
            this.PC.Size = new global::System.Drawing.Size(50, 45);
            this.PC.TabIndex = 0;
            this.PC.Value = "000000";
            // 
            // RegisterDisplay
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "RegisterDisplay";
            this.Size = new global::System.Drawing.Size(466, 64);
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
