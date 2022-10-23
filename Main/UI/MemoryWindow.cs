using FoenixIDE.MemoryLocations;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class MemoryWindow : Form
    {
        public static MemoryWindow Instance = null;
        public IMappable Memory = null;
        public int StartAddress = 0;
        public int EndAddress = 0xFF;
        const int PageSize = 0xFF;
        public delegate void ButtonClicked(bool value);
        public ButtonClicked SetGamma;
        public ButtonClicked SetHiRes;

        // Delegate function to write MCR Bytes
        public delegate void WriteMCRBytesDelegate(byte low, byte high);
        public WriteMCRBytesDelegate WriteMCRBytes;
        public delegate ushort ReadMCRBytesDelegate(); // little-endian
        public ReadMCRBytesDelegate ReadMCRBytes;

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
            MemoryWindowTooltips.SetToolTip(MCRBit9Button, "Double Pixels");
            MemoryWindowTooltips.SetToolTip(MCRBit8Button, "High-Res");
            MemoryWindowTooltips.SetToolTip(MCRBit7Button, "Disable Video");
            MemoryWindowTooltips.SetToolTip(MCRBit6Button, "Enable Gamma");
            MemoryWindowTooltips.SetToolTip(MCRBit5Button, "Enable Sprites");
            MemoryWindowTooltips.SetToolTip(MCRBit4Button, "Enable Tilemap");
            MemoryWindowTooltips.SetToolTip(MCRBit3Button, "Enable Bitmap");
            MemoryWindowTooltips.SetToolTip(MCRBit2Button, "Enable Graphics Mode");
            MemoryWindowTooltips.SetToolTip(MCRBit1Button, "Enable Text Overlay");
            MemoryWindowTooltips.SetToolTip(MCRBit0Button, "Enable Text");
            MemoryWindowTooltips.SetToolTip(ZeroButton, "Reset Page to Zeroes");

            // Set the MCR
            MCRBit0Button.Tag = 0;
            MCRBit1Button.Tag = 0;
            MCRBit2Button.Tag = 0;
            MCRBit3Button.Tag = 0;
            MCRBit4Button.Tag = 0;
            MCRBit5Button.Tag = 0;
            MCRBit6Button.Tag = 0;
            MCRBit7Button.Tag = 0;
            MCRBit8Button.Tag = 0;
            MCRBit9Button.Tag = 0;

            // Set the Address to Bank $00
            if (Memory is MemoryRAM)
            {
                AddressCombo.Items.Clear();
                AddressCombo.Items.Add("Custom Memory " + Memory.StartAddress.ToString("X6"));
                AddressCombo.Enabled = false;
                HighlightPanel.ReadOnly = true;
                FooterPanel.Visible = false;
                UpdateDisplayTimer.Enabled = false;
                ZeroButton.Visible = false;
            }
            else
            {
                AddressCombo.SelectedIndex = 0;
                HighlightPanel.ReadOnly = true;
                HighlightPanel.ReadOnly = false;
            }

        }

        public void AllowSave()
        {
            SaveButton.Visible = true;
        }
        public void RefreshMemoryView()
        {
            StringBuilder s = new StringBuilder();
            if (Memory == null)
                return;
            //MemoryText.Clear();
            // Display 16 bytes per line
            for (int i = StartAddress; i < EndAddress; i += 0x10)
            {
                s.Append(">");
                if (Memory is MemoryRAM)
                {
                    s.Append((i + Memory.StartAddress).ToString("X6"));
                }
                else
                {
                    s.Append(i.ToString("X6"));
                }

                s.Append("  ");
                StringBuilder text = new StringBuilder();
                for (int j = 0; j < 16; j++)
                {
                    if (i + j < Memory.Length)
                    {
                        int c = Memory.ReadByte(i + j);
                        s.Append(c.ToString("X2"));

                        // Character data
                        if (c < 32 || c > 127)
                            text.Append(".");
                        else
                            text.Append((char)c);
                    }
                    else
                    {
                        s.Append("--");
                        text.Append("-");
                    }
                    s.Append(" ");



                    // Group 8 bytes together
                    if (j == 7 || j == 15)
                    {
                        s.Append(" ");
                        text.Append(" ");
                    }

                }
                s.Append(text);
                if ((i - StartAddress) < 256)
                {
                    s.AppendLine();
                }

            }
            MemoryText.Text = s.ToString();
        }

        private void UpdateDisplayTimer_Tick(object sender, EventArgs e)
        {
            if (!(Memory is MemoryRAM))
            {
                RefreshMemoryView();
                UpdateMCRButtons();
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            RefreshMemoryView();
        }

        private void StartAddressText_Validated(object sender, EventArgs e)
        {
            int requestedAddress = 0;
            try
            {
                requestedAddress = Convert.ToInt32(this.StartAddressText.Text, 16) & 0xFFFF00;
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                GotoAddress(requestedAddress);
                FindMatchedDropDownEntry(requestedAddress);
            }
        }

        public void GotoAddress(int requestedAddress)
        {
            if (Memory is MemoryRAM)
            {
                int newAddress = requestedAddress - Memory.StartAddress;
                if (newAddress >= 0 && (newAddress) < Memory.Length)
                {
                    StartAddress = newAddress;
                    EndAddress = newAddress + PageSize;
                    if (EndAddress > Memory.Length)
                    {
                        EndAddress = Memory.Length;
                    }
                }
                this.StartAddressText.Text = (StartAddress + Memory.StartAddress).ToString("X6");
                this.EndAddressText.Text = (EndAddress + Memory.StartAddress).ToString("X6");
            }
            else
            {
                this.StartAddress = requestedAddress;
                this.EndAddress = requestedAddress + PageSize;
                this.StartAddressText.Text = requestedAddress.ToString("X6");
                this.EndAddressText.Text = EndAddress.ToString("X6");
            }

            HighlightPanel.Visible = false;
            PositionLabel.Text = "";
            RefreshMemoryView();
            if (!(Memory is MemoryRAM) && StartAddressText.Text.StartsWith("AF00"))
            {
                UpdateMCRButtons();
            }
        }

        private void FindMatchedDropDownEntry(int address)
        { // find the address in the dropdown list
            int dropdownAddress = 0;
            bool matched = false;
            foreach (string item in AddressCombo.Items)
            {
                dropdownAddress = 0;
                if (item.StartsWith("Bank"))
                {
                    int start = item.IndexOf('$');
                    dropdownAddress = Convert.ToInt32(item.Substring(start + 1, 2) + "0000", 16);
                }
                else if (item.StartsWith("Address"))
                {
                    int start = item.IndexOf('$');
                    dropdownAddress = Convert.ToInt32(item.Replace(":", "").Substring(start + 1, 6), 16);
                }
                if (dropdownAddress != 0 && dropdownAddress == address)
                {
                    AddressCombo.SelectedItem = item;
                    matched = true;
                    break;
                }
            }
            if (!matched)
            {
                AddressCombo.SelectedItem = "Unspecified Page";
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            // Move Down by one page
            int desiredStart = Convert.ToInt32(this.StartAddressText.Text, 16) + 256;
            if (desiredStart < MemoryMap.FLASH_END)
            {
                GotoAddress(desiredStart);
                FindMatchedDropDownEntry(desiredStart);
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            int desiredStart = Convert.ToInt32(this.StartAddressText.Text, 16) - 256;
            if (desiredStart >= 0)
            {
                GotoAddress(desiredStart);
                FindMatchedDropDownEntry(desiredStart);
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
            else if (value.StartsWith("Address"))
            {
                // Read all 6 characters, but omit the ':'
                int start = value.IndexOf('$');
                startAddress = Convert.ToInt32(value.Replace(":", "").Substring(start + 1, 6), 16);
            }
            else
            {
                return;
            }
            GotoAddress(startAddress);
            ViewButton.Focus();
        }

        private void MemoryWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageDown)
            {
                this.NextButton_Click(sender, e);
            }
            else if (e.KeyCode == Keys.PageUp)
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
            int low = ((int)MCRBit0Button.Tag);
            low |= ((int)MCRBit1Button.Tag) << 1;
            low |= ((int)MCRBit2Button.Tag) << 2;
            low |= ((int)MCRBit3Button.Tag) << 3;
            low |= ((int)MCRBit4Button.Tag) << 4;
            low |= ((int)MCRBit5Button.Tag) << 5;
            low |= ((int)MCRBit6Button.Tag) << 6;
            low |= ((int)MCRBit7Button.Tag) << 7;

            int high = ((int)MCRBit8Button.Tag);
            high |= ((int)MCRBit9Button.Tag) << 1;
            WriteMCRBytes?.Invoke((byte)low, (byte)high);

            if ("AF0000".Equals(StartAddressText.Text))
            {
                RefreshMemoryView();
            }
            if (btn == MCRBit6Button)
            {
                SetGamma?.Invoke((int)MCRBit6Button.Tag == 1);
            }
            if (btn == MCRBit8Button)
            {
                SetHiRes?.Invoke((int)MCRBit8Button.Tag == 1);
            }
        }

        public void UpdateMCRButtons()
        {
            ushort word = ReadMCRBytes.Invoke();
            // split the value
            byte low = (byte)(word & 0xFF);
            byte high = (byte)(word >> 8);
            // Determine if Gamma was changed, if so, toggle the dip switch
            int oldGamma = (int)MCRBit6Button.Tag;
            int newGamma = (low & 0x40) != 0 ? 1 : 0;

            SetMCRButton(MCRBit7Button, (low & 0x80) == 0x80);
            SetMCRButton(MCRBit6Button, newGamma != 0);
            if (oldGamma != newGamma)
            {
                SetGamma?.Invoke(newGamma != 0);
            }
            SetMCRButton(MCRBit5Button, (low & 0x20) == 0x20);
            SetMCRButton(MCRBit4Button, (low & 0x10) == 0x10);
            SetMCRButton(MCRBit3Button, (low & 0x08) == 0x08);
            SetMCRButton(MCRBit2Button, (low & 0x04) == 0x04);
            SetMCRButton(MCRBit1Button, (low & 0x02) == 0x02);
            SetMCRButton(MCRBit0Button, (low & 0x01) == 0x01);

            // High-res and double-pixels
            // Determine if the Hi-Res was changed, if so toggle the dip switch
            int oldHires = (int)MCRBit8Button.Tag;
            int newHires = (high & 0x01) != 0 ? 1 : 0;
            SetMCRButton(MCRBit8Button, (newHires) != 0);
            if (oldHires != newHires)
            {
                SetHiRes?.Invoke(newHires != 0);
            }
            SetMCRButton(MCRBit9Button, (high & 0x02) != 0);
        }

        private void SetMCRButton(Button btn, bool value)
        {
            if (value)
            {
                btn.Tag = 1;
                btn.BackColor = Color.DarkGray;
            }
            else
            {
                btn.Tag = 0;
                btn.BackColor = Control.DefaultBackColor;
            }
        }

        private void MemoryText_MouseMove(object sender, MouseEventArgs e)
        {
            GetAddressPosition(e.Location);
            if (mem.X != -1 && mem.Y != -1)
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

        Point mem = new Point(-1, -1);
        // Retrieve the memory location of the mouse location
        private void GetAddressPosition(Point mouse)
        {
            int line = mouse.Y / 15;
            int col = -1;
            int colWidth = 21;
            int addr = -1;
            int value = 0;
            int offset = 0;
            if (mouse.X > 54 && mouse.X < 230)
            {
                col = 1 + (mouse.X - 66) / colWidth;
                offset = col - 1;
            }
            if (mouse.X > 241 && mouse.X < 400)
            {
                col = 10 + (mouse.X - 242) / colWidth;
                offset = col - 2;
            }
            if (line < 16 && col != -1)
            {
                // Determine the address
                addr = Convert.ToInt32(StartAddressText.Text, 16) + line * 16 + offset;
                if (Memory is MemoryRAM)
                {
                    if (addr - Memory.StartAddress < Memory.Length)
                    {
                        value = Memory.ReadByte(addr - Memory.StartAddress);
                    }
                    else
                    {
                        value = -1;
                    }
                }
                else
                {
                    value = Memory.ReadByte(addr);
                }
                if (value > -1)
                {
                    HighlightPanel.Left = col < 10 ? col * colWidth + 47 : col * colWidth + 34;
                    HighlightPanel.Top = MemoryText.Top + line * 15 + 3;
                    HighlightPanel.Text = value.ToString("X2");
                    HighlightPanel.Visible = true;
                }
                else
                {
                    HighlightPanel.Visible = false;
                }
            }
            else
            {
                HighlightPanel.Visible = false;
            }
            mem.X = addr;
            mem.Y = value;
        }

        private void HighlightPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (mem.X != -1)
            {
                String rawAddress = mem.X.ToString("X6");
                String address = "$" + rawAddress.Substring(0, 2) + ":" + rawAddress.Substring(2);
                if (HighlightPanel.Text != "")
                {
                    // The result may be a hexadecimal value
                    try
                    {
                        byte intResult = Convert.ToByte(HighlightPanel.Text, 16);
                        // Check that the value was changed
                        if (intResult != mem.Y)
                        {
                            Memory.WriteByte(mem.X, intResult);
                            //HighlightPanel.Text = intResult.ToString("X2");
                            RefreshMemoryView();
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void MemoryText_MouseLeave(object sender, EventArgs e)
        {
            HighlightPanel.Visible = false;
        }

        /**
         * Save the content of memory to a binary file.
         */
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream outputFile = File.Create(SaveDialog.FileName);
                MemoryRAM ram = (MemoryRAM)Memory;
                byte[] buffer = new byte[ram.Length];
                ram.CopyIntoBuffer(0, ram.Length, buffer);
                outputFile.Write(buffer, 0, buffer.Length);
                outputFile.Flush();
                outputFile.Close();
            }
        }

        private void ZeroButton_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[256];
            Memory.CopyBuffer(buffer, 0, StartAddress, 256);
        }
    }
}
