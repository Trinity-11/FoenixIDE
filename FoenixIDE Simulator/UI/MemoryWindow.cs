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
using System.Xml;

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
            MemoryWindowTooltips.SetToolTip(NextButton, "Next Page");
            MemoryWindowTooltips.SetToolTip(PreviousButton, "Previous Page");

            // MCR Tooltips
            MemoryWindowTooltips.SetToolTip(MCRBit7Button, "Disable Video");
            MemoryWindowTooltips.SetToolTip(MCRBit6Button, "Enable Gamma");
            MemoryWindowTooltips.SetToolTip(MCRBit5Button, "Enable Sprites");
            MemoryWindowTooltips.SetToolTip(MCRBit4Button, "Enable Tilemap");
            MemoryWindowTooltips.SetToolTip(MCRBit3Button, "Enable Bitmap");
            MemoryWindowTooltips.SetToolTip(MCRBit2Button, "Enable Graphics Mode");
            MemoryWindowTooltips.SetToolTip(MCRBit1Button, "Enable Text Overlay");
            MemoryWindowTooltips.SetToolTip(MCRBit0Button, "Enable Text");
            MemoryWindowTooltips.SetToolTip(ExportButton, "Export Memory to Foenix File");

            // Set the MCR
            MCRBit0Button.Tag = 0;
            MCRBit1Button.Tag = 0;
            MCRBit2Button.Tag = 0;
            MCRBit3Button.Tag = 0;
            MCRBit4Button.Tag = 0;
            MCRBit5Button.Tag = 0;
            MCRBit6Button.Tag = 0;
            MCRBit7Button.Tag = 0;

            // Set the Address to Bank $00
            AddressCombo.SelectedIndex = 0;
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
            UpdateDisplayTimer.Enabled = false;
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

        private void AddressCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String value = (String)AddressCombo.SelectedItem;
            int startAddress = 0;
            if (value.StartsWith("Bank"))
            {
                // Read two characters and pad with '0000' to get a 24 bit address
                int start = value.IndexOf('$');
                startAddress = Convert.ToInt32(value.Substring(start + 1, 2) + "0000", 16);
            }
            else
            {
                // Read all 6 characters, but omit the ':'
                int start = value.IndexOf('$');
                startAddress = Convert.ToInt32(value.Replace(":", "").Substring(start + 1, 6), 16);
            }
            GotoAddress(startAddress);
            MemoryText.Focus();
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

        /*
         * Change the Master Control Register (MCR).
         * This allows for displaying text, overlay on top of graphics.
         */
        private void MCRBitButton_Click(object sender, EventArgs e)
        {
            // toggle the button tag 0 or 1
            Button btn = ((Button)sender);
            if ((int)btn.Tag == 0)
            {
                btn.Tag = 1;
                btn.BackColor = Color.DarkGray;
            }
            else
            {
                btn.Tag = 0;
                btn.BackColor = Control.DefaultBackColor;
            }
            // Save the value of all buttons to the Master Control Memory Location
            int value = ((int)MCRBit0Button.Tag);
            value |= ((int)MCRBit1Button.Tag) << 1;
            value |= ((int)MCRBit2Button.Tag) << 2;
            value |= ((int)MCRBit3Button.Tag) << 3;
            value |= ((int)MCRBit4Button.Tag) << 4;
            value |= ((int)MCRBit5Button.Tag) << 5;
            value |= ((int)MCRBit6Button.Tag) << 6;
            value |= ((int)MCRBit7Button.Tag) << 7;
            Memory.WriteByte(0xAF_0000, (byte)value);
        }

        /*
         * Export all memory content to an XML file.
         */
        private void ExportButton_Click(object sender, EventArgs e)
        {
            // Pick the file to create
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save Memory to Foenix File";
            dialog.CheckPathExists = true;
            dialog.Filter = "Foenix IDE File (.fnx) | *.fnx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlWriter xmlWriter = XmlWriter.Create(dialog.FileName);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteRaw("\r");
                xmlWriter.WriteComment("Export of FoenixIDE for C256.  All values are in hexadecimal form");
                xmlWriter.WriteRaw("\r");
                xmlWriter.WriteStartElement("pages");
                bool compact = CompactCheckbox.Checked;
                if (compact)
                {
                    xmlWriter.WriteAttributeString("format", "compact");
                }
                else
                {
                    xmlWriter.WriteAttributeString("format", "full");
                }

                xmlWriter.WriteRaw("\r");

                // We don't need to scan $FFFF pages, only scan the ones we know are gettings used
                // Scan each of the banks and pages and save to an XML file
                // If a page is blank, don't export it.
                for (int i = 0; i < 0x200000; i = i + 256)
                {
                    if (PageChecksum(i) != 0)
                    {
                        WriteData(i, xmlWriter, compact);
                    }
                }

                for (int i = 0xAF_0000; i < 0xF0_0000; i = i + 256)
                {
                    if (PageChecksum(i) != 0)
                    {
                        WriteData(i, xmlWriter, compact);
                    }
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }

        private void WriteData(int startAddress, XmlWriter writer, bool compact)
        {
            writer.WriteStartElement("page");
            writer.WriteAttributeString("start-address", "$" + startAddress.ToString("X6"));
            writer.WriteAttributeString("bank", "$" + startAddress.ToString("X6").Substring(0, 2));
            writer.WriteRaw("\r");

            // Write 8 bytes per data line
            for (int i = 0; i < 256; i = i + 8)
            {
                WritePhrase(startAddress + i, writer, compact);
            }
            writer.WriteEndElement();
            writer.WriteRaw("\r");
        }

        // Only write a phrase if the bytes are non-zero
        private void WritePhrase(int startAddress, XmlWriter writer, bool compact)
        {
            if (PhraseChecksum(startAddress) == 0 && !compact || PhraseChecksum(startAddress) != 0)
            {
                writer.WriteStartElement("data");
                writer.WriteAttributeString("address", "$" + (startAddress).ToString("X6"));
                for (int i = 0; i < 8; i++)
                {
                    writer.WriteString(Memory.ReadByte(startAddress + i).ToString("X2") + " ");
                }
                writer.WriteEndElement();
                writer.WriteRaw("\r");
            }
        }

        // Sum 256 bytes
        private int PageChecksum(int startAddress)
        {
            int sum = 0;
            for (int i = 0; i < 255; i++)
            {
                sum += Memory.ReadByte(startAddress + i);
            }
            return sum;
        }

        // Sum 8 bytes
        private int PhraseChecksum(int startAddress)
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum += Memory.ReadByte(startAddress + i);
            }
            return sum;
        }

        Point mem = new Point(-1,-1);
        // Retrieve the memory location of the mouse location
        private void GetAddressPosition(Point mouse)
        {
            int line = mouse.Y / 15;
            int col = -1;
            int colWidth = 21;
            int addr = -1;
            int value = 0;
            if (mouse.X > 54 && mouse.X < 230)
            {
                col = 1 + (mouse.X - 66) / colWidth;
            }
            if (mouse.X > 241 && mouse.X < 400)
            {
                col = 10 + (mouse.X - 242) / colWidth;
            }
            if (line < 16 && col != -1)
            {
                String[] lineText = MemoryText.Lines[line].Split(' ');
                String text = lineText[col + 1];
                String address = lineText[0].Substring(1);
                addr = Convert.ToInt32(address, 16) + ((col < 10) ? (col - 1) : (col - 2));
                value = Convert.ToByte(text, 16);
                
                HighlightPanel.Left = col<10 ? col * colWidth + 44 : col * colWidth + 30;
                HighlightPanel.Top = MemoryText.Top + line * 15 + 2;
                HighlightPanel.Text = text;
                HighlightPanel.Visible = true;
            }
            else
            {
                HighlightPanel.Visible = false;
            }
            mem.X = addr;
            mem.Y = value;
        }

        // Offer the user to modify the address value
        private void MemoryText_MouseClick(object sender, MouseEventArgs e)
        {
            if (mem.X != -1)
            {
                String rawAddress = mem.X.ToString("X6");
                String address = "$" + rawAddress.Substring(0, 2) + ":" + rawAddress.Substring(2);
                int left = this.Left + (this.Width - 443) /2;
                int top = this.Top + (this.Height - 190) /2;
                String result = Microsoft.VisualBasic.Interaction.InputBox("Enter New Hex Value for address " + address, "Modify Memory Location", mem.Y.ToString("X2"), left, top);
                if (result != "")
                {
                    // The result may be a hexadecimal value
                    byte intResult = Convert.ToByte(result, 16);
                    // Check that the value was changed
                    if (intResult != mem.Y)
                    {
                        Memory.WriteByte(mem.X, intResult);
                        RefreshMemoryView();
                    }
                }
            }
        }

        
        private void MemoryText_MouseMove(object sender, MouseEventArgs e)
        {
            GetAddressPosition(e.Location);
            if (mem.X != -1)
            {
                String val = mem.Y.ToString("X2");
                
                String address = mem.X.ToString("X6");
                PositionLabel.Text = "Adress: $" + address.Substring(0, 2) + ":" + address.Substring(2) + ", Value: " + val; // + ", X: " + e.X + ", Y: " + e.Y + ", Col: " + col + ", Line: " + line;
            }
            else
            {
                PositionLabel.Text = "";
            }
        }
    }
}
