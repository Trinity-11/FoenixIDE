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
        public FoenixSystem kernel;
        private Image DeleteImage;
        public static WatchForm Instance;

        public WatchForm()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Watch_Load(object sender, EventArgs e)
        {
            WatchGrid.CellValueNeeded += WatchGrid_CellValueNeeded;
            DeleteImage = Simulator.Properties.Resources.delete_btn;
            WatchGrid.RowCount = kernel.WatchList.Count;
        }

        /**
         * Update the values in the visible cells
         */
        private void WatchUpdateTimer_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, WatchedMemory> kvp in kernel.WatchList)
            {
                WatchedMemory mem = kvp.Value;
                mem.val8bit = kernel.MemMgr.ReadByte(mem.address);
                mem.val16bit = kernel.MemMgr.ReadWord(mem.address);
            }
            WatchGrid.Invalidate();
        }

        private void WatchGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                KeyValuePair<int, WatchedMemory> kvp = kernel.WatchList.ElementAt(e.RowIndex);
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
                    case 4:
                        e.Value = DeleteImage;
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
            // Delete the row, but copy the values into our input boxes
            if (e.ColumnIndex == 4)
            {
                // Get the address for the RowIndex
                KeyValuePair<int, WatchedMemory> kvp = kernel.WatchList.ElementAt(e.RowIndex);
                NameText.Text = kvp.Value.name;
                AddressText.Text = "$" + kvp.Value.address.ToString("X6");
                kernel.WatchList.Remove(kvp.Key);
                WatchGrid.RowCount -= 1;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (AddressText.Text.Length > 0)
            {
                int addressVal = Convert.ToInt32(AddressText.Text.Replace("$", "").Replace(":", ""), 16);
                if (NameText.Text.Length > 0 && addressVal > 0)
                {
                    if (kernel.WatchList.ContainsKey(addressVal))
                    {
                        kernel.WatchList.Remove(addressVal);
                    }
                    WatchedMemory mem = new WatchedMemory(NameText.Text, addressVal,
                        kernel.MemMgr.ReadByte(addressVal),
                        kernel.MemMgr.ReadWord(addressVal)
                    );
                    kernel.WatchList.Add(addressVal, mem);
                    WatchGrid.RowCount = kernel.WatchList.Count;
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
    }
}
