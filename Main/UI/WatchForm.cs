using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.FileFormat;
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
    public partial class WatchForm : Form
    {
        private FoenixSystem kernel_ref;
        public static WatchForm Instance;

        public WatchForm()
        {
            InitializeComponent();
            Instance = this;
            WatchGrid.CellValueNeeded += WatchGrid_CellValueNeeded;
        }

        public void SetKernel(FoenixSystem krnl)
        {
            kernel_ref = krnl;
            WatchGrid.RowCount = kernel_ref.WatchList.Count;
            WatchUpdateTimer.Enabled = true;
        }

        /**
         * Update the values in the visible cells
         */
        private void WatchUpdateTimer_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, WatchedMemory> kvp in kernel_ref.WatchList)
            {
                WatchedMemory mem = kvp.Value;
                mem.val8bit = kernel_ref.MemMgr.ReadByte(mem.address);
                mem.val16bit = kernel_ref.MemMgr.ReadWord(mem.address);
            }
            WatchGrid.RowCount = kernel_ref.WatchList.Count;
            WatchGrid.Invalidate();
        }

        public void UpdateList()
        {
            WatchGrid.RowCount = kernel_ref.WatchList.Count;
        }

        private void WatchGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                KeyValuePair<int, WatchedMemory> kvp = kernel_ref.WatchList.ElementAt(e.RowIndex);
                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = kvp.Value.name;
                        break;
                    case 1:
                        e.Value = kvp.Value.address.ToString("X6");
                        break;
                    case 2:
                        e.Value = kvp.Value.val8bit.ToString("X2"); ;
                        break;
                    case 3:
                        e.Value = kvp.Value.val16bit.ToString("X4");
                        break;
                }
                
            }
            catch
            {
                // whatever!
            }
        }

        private void WatchGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the address for the RowIndex
            if (e.RowIndex > -1)
            {
                KeyValuePair<int, WatchedMemory> kvp = kernel_ref.WatchList.ElementAt(e.RowIndex);
                switch (e.ColumnIndex)
                {
                    // Browse this page in the Memory Window
                    case 4:
                        MemoryWindow.Instance.GotoAddress(kvp.Key & 0xFFFF00);
                        break;
                    // Delete the row, but copy the values into our input boxes
                    case 5:

                        NameText.Text = kvp.Value.name;
                        AddressText.Text = "$" + kvp.Value.address.ToString("X6");
                        kernel_ref.WatchList.Remove(kvp.Key);
                        WatchGrid.RowCount -= 1;
                        break;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (AddressText.Text.Length > 0)
            {
                int addressVal = Convert.ToInt32(AddressText.Text.Replace("$", "").Replace(":", ""), 16);
                if (NameText.Text.Length > 0 && addressVal > 0)
                {
                    if (kernel_ref.WatchList.ContainsKey(addressVal))
                    {
                        kernel_ref.WatchList.Remove(addressVal);
                    }
                    WatchedMemory mem = new WatchedMemory(NameText.Text, addressVal,
                        kernel_ref.MemMgr.ReadByte(addressVal),
                        kernel_ref.MemMgr.ReadWord(addressVal)
                    );
                    kernel_ref.WatchList.Add(addressVal, mem);
                    WatchGrid.RowCount = kernel_ref.WatchList.Count;
                    NameText.Text = "";
                    AddressText.Text = "";
                }
            }
        }

        private void WatchForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void WatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void WatchForm_Resize(object sender, EventArgs e)
        {
            WatchGrid.Columns[0].Width = Width - 274;
        }
    }
}
