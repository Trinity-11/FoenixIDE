using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class BreakpointWindow : Form
    {
        private TextBox[] txtAddresses;
        private CheckBox[] chkExecutes;
        private CheckBox[] chkReads;
        private CheckBox[] chkWrites;
        private Button[] btnDeletes;

        public delegate void DeleteEventEvent();
        public DeleteEventEvent DeleteEvent;

        public BreakpointWindow()
        {
            InitializeComponent();
            txtAddresses = new TextBox[6];
            chkExecutes = new CheckBox[6];
            chkReads = new CheckBox[6];
            chkWrites = new CheckBox[6];
            btnDeletes = new Button[6];
            int controlIndex = 4;
            int y = 22;
            for (int i = 0; i< 6; i++)
            {
                txtAddresses[i] = AddTextBox(6, y, "textAddress" + i, controlIndex++);
                chkExecutes[i] = AddCheckBox(85, y + 4, "chkExec" + i, controlIndex++);
                chkReads[i] = AddCheckBox(125, y + 4, "chkRead" + i, controlIndex++);
                chkWrites[i] = AddCheckBox(165, y + 4, "chkWrite" + i, controlIndex++);
                btnDeletes[i] = AddDeleteButton(190, y, "chkWrite" + i, controlIndex++);
                y += 20;
            }
        }

        private TextBox AddTextBox(int left, int top, string name, int index)
        {
            TextBox tb = new TextBox
            {
                Location = new Point(left, top),
                Name = name,
                Size = new Size(60, 24),
                TabIndex = index
            };
            Controls.Add(tb);
            //tb.TextChanged += BreakpointsText_Changed;
            tb.KeyDown += BreakpointsText_KeyDown;
            tb.KeyUp += BreakpointsText_KeyUp;
            return tb;
        }

        private CheckBox AddCheckBox(int left, int top, string name, int index)
        {
            CheckBox cb = new CheckBox
            {
                AutoSize = true,
                Location = new Point(left, top),
                Name = name,
                Size = new Size(15, 14),
                TabIndex = index,
                UseVisualStyleBackColor = true
            };
            Controls.Add(cb);
            return cb;
        }

        private Button AddDeleteButton(int left, int top, string name, int index)
        {
            Button db = new Button
            {
                Image = global::FoenixIDE.Simulator.Properties.Resources.delete_btn,
                Location = new Point(left, top),
                Name = name,
                Size = new Size(25, 23),
                TabIndex = index,
                UseVisualStyleBackColor = true
            };
            db.Click += new System.EventHandler(this.BtnDelete_Click);
            toolTips.SetToolTip(db, "Remove Breakpoint");
            Controls.Add(db);
            return db;
        }

        private void Breakpoints_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        // Is this the only way to detect a paste event?  This is awful.
        bool pasteEvent = false;
        private void BreakpointsText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                pasteEvent = true;
            }
        }
        private void BreakpointsText_KeyUp(object sender, KeyEventArgs e)
        {
            if (pasteEvent)
            {
                // Remove all formatting marks $:
                string value = ((TextBox)sender).Text.Replace("$", "").Replace(":", "");
                try
                {
                    int rawVal = Convert.ToInt32(value, 16);
                    string newValue = rawVal.ToString("X6");
                    string address = "$" + newValue.Substring(0, 2) + ":" + newValue.Substring(2);
                    ((TextBox)sender).Text = address;
                }
                catch
                {
                    // nothing happens
                }
                pasteEvent = false;
            }
        }

        private void BreakpointsText_Changed(object sender, EventArgs args)
        {
            // Remove all formatting marks $:
            string value = ((TextBox)sender).Text.Replace("$", "").Replace(":", "");
            try
            {
                int rawVal = Convert.ToInt32(value, 16);
                string newValue = rawVal.ToString("X6");
                string address = "$" + newValue.Substring(0, 2) + ":" + newValue.Substring(2);
                ((TextBox)sender).Text = address;
            }
            catch
            {
                // nothing happens
            }
        }

        private void Breakpoints_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public List<int> GetExecuteBreakpoints()
        {
            List<int> bp = new List<int>();
            for (int i=0;i<6;i++)
            {
                if (chkExecutes[i].Checked && txtAddresses[i].Text.Length > 0)
                {
                    bp.Add(FoenixSystem.TextAddressToInt(txtAddresses[i].Text));
                }
            }
            return bp;
        }

        public List<int> GetReadBreakpoints()
        {
            List<int> bp = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                if (chkReads[i].Checked && txtAddresses[i].Text.Length > 0)
                {
                    bp.Add(FoenixSystem.TextAddressToInt(txtAddresses[i].Text));
                }
            }
            return bp;
        }

        public List<int> GetWriteBreakpoints()
        {
            List<int> bp = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                if (chkWrites[i].Checked && txtAddresses[i].Text.Length > 0)
                {
                    bp.Add(FoenixSystem.TextAddressToInt(txtAddresses[i].Text));
                }
            }
            return bp;
        }

        public void AddBreakpoint(int value)
        {
            // First check if the breakpoint already exists
            List<int> existing = GetExecuteBreakpoints();
            if (!existing.Contains(value))
            {
                for (int i = 0; i < 6; i++)
                {
                    if (txtAddresses[i].Text.Length == 0)
                    {
                        string rawAddress = value.ToString("X6");
                        string address = "$" + rawAddress.Substring(0, 2) + ":" + rawAddress.Substring(2);
                        txtAddresses[i].Text = address;
                        chkExecutes[i].Checked = true;
                        chkReads[i].Checked = false;
                        chkWrites[i].Checked = false;
                        break;
                    }
                }
            }
        }

        public void DeleteBreakpoint(int value)
        {
            string rawAddress = value.ToString("X6");
            string address = "$" + rawAddress.Substring(0, 2) + ":" + rawAddress.Substring(2);
            for (int i = 0; i < 6; i++)
            {
                if (address.Equals(txtAddresses[i].Text))
                {
                    txtAddresses[i].Text = "";
                    chkExecutes[i].Checked = false;
                    chkReads[i].Checked = false;
                    chkWrites[i].Checked = false;
                    break;
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int row = (((Button)sender).Top - 22) / 20;
            txtAddresses[row].Text = "";
            chkExecutes[row].Checked = false;
            chkReads[row].Checked = false;
            chkWrites[row].Checked = false;
            DeleteEvent.Invoke();
        }
    }
}
