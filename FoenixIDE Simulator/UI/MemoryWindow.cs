using FoenixIDE.Common;
using FoenixIDE.MemoryLocations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class MemoryWindow : Form
    {
        public static MemoryWindow Instance = null;
        public IMappable Memory = null;
        public int StartAddress = 0;
        public int EndAddress = 0xFF;

        public MemoryWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void MemoryWindow_Load(object sender, EventArgs e)
        {

        }

        public void RefreshMemoryView()
        {
            StringBuilder s = new StringBuilder();
            if (Memory == null)
                return;
            MemoryText.Clear();
            for (int i = StartAddress; i <= EndAddress; i += 0x10)
            {
                s.Append(">");
                s.Append(i.ToString("X6"));
                s.Append("  ");
                for (int j = 0; j < 16; j++)
                {
                    s.Append(Memory.ReadByte(i + j).ToString("X2"));
                    s.Append(" ");
                    if (j == 7 || j == 15)
                        s.Append(" ");
                }

                for (int j = 0; j < 16; j++)
                {
                    int c = Memory.ReadByte(i + j);
                    if (c < 32 || c > 127)
                        s.Append(".");
                    else
                        s.Append((char)c);
                    if (j == 7 || j == 15)
                        s.Append(" ");
                }
                s.AppendLine();
            }
            MemoryText.AppendText(s.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshMemoryView();
            timer1.Enabled = false;
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            RefreshMemoryView();
        }

        private void StartAddressText_Validated(object sender, EventArgs e)
        {
            try
            {
                int len = this.EndAddress - this.StartAddress;
                this.StartAddress = Convert.ToInt32(this.StartAddressText.Text, 16);
                this.EndAddress = this.StartAddress + len;
                this.EndAddressText.Text = this.EndAddress.ToString("X6");
            }
            catch (global::System.FormatException ex)
            {
                global::System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void EndAddressText_Validated(object sender, EventArgs e)
        {
            try
            {
                this.StartAddress = Convert.ToInt32(this.StartAddressText.Text, 16);
            }
            catch (global::System.FormatException ex)
            {
                global::System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void GotoAddress(int StartAddress)
        {
            this.StartAddress = StartAddress;
            this.EndAddress = StartAddress + 255;

            this.StartAddressText.Text = StartAddress.ToString("X6");
            this.EndAddressText.Text = EndAddress.ToString("X6");
            RefreshMemoryView();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            // Move Down by one page
            int desiredStart = Convert.ToInt32(this.StartAddressText.Text, 16) + 256;
            if (desiredStart < MemoryMap.FLASH_END)
            {
                GotoAddress(desiredStart);
            }
            
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            int desiredStart = Convert.ToInt32(this.StartAddressText.Text, 16) - 256;
            if (desiredStart >= 0)
            {
                GotoAddress(desiredStart);
            }
            
        }

        private void Page00_Click(object sender, EventArgs e)
        {
            GotoAddress(0);
        }

        private void Page18Button_Click(object sender, EventArgs e)
        {
            GotoAddress(0x18_0000);
        }

        private void Page19_Click(object sender, EventArgs e)
        {
            GotoAddress(0x19_0000);
        }

        private void IOButton_Click(object sender, EventArgs e)
        {
            GotoAddress(0xAF_0000);
        }

        private void MemoryWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageDown)
            {
                this.NextButton_Click(sender, e);
            } else if (e.KeyCode == Keys.PageUp)
            {
                this.PreviousButton_Click(sender, e);
            }

        }
    }
}
