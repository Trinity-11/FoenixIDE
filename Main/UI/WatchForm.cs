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
        public SortedList<int, WatchedMemory> watchList = new SortedList<int, WatchedMemory>();
        public MemoryManager memoryMgr;
        private Image DeleteImage;
        public static WatchForm Instance;

        public WatchForm()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Watch_Load(object sender, EventArgs e)
        {
            // Add some sample entries to the data store. 
            watchList.Add(0x13, new WatchedMemory("LINES_VISIBLE", 0x13, 0, 0));
            watchList.Add(0x11, new WatchedMemory("COLS_PER_LINE", 0x11, 0, 0));

            // Set the row count, including the row for new records.
            WatchGrid.RowCount = 2;
            WatchGrid.CellValueNeeded += WatchGrid_CellValueNeeded;
            DeleteImage = Simulator.Properties.Resources.delete_btn;
        }

        /**
         * Update the values in the visible cells
         */
        private void WatchUpdateTimer_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, WatchedMemory> kvp in watchList)
            {
                WatchedMemory mem = kvp.Value;
                mem.val8bit = memoryMgr.ReadByte(mem.address);
                mem.val16bit = memoryMgr.ReadWord(mem.address);
            }
            WatchGrid.Invalidate();
        }

        private void WatchGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                KeyValuePair<int, WatchedMemory> kvp = watchList.ElementAt(e.RowIndex);
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
                KeyValuePair<int, WatchedMemory> kvp = watchList.ElementAt(e.RowIndex);
                NameText.Text = kvp.Value.name;
                AddressText.Text = "$" + kvp.Value.address.ToString("X6");
                watchList.Remove(kvp.Key);
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
                    if (watchList.ContainsKey(addressVal))
                    {
                        watchList.Remove(addressVal);
                    }
                    WatchedMemory mem = new WatchedMemory(NameText.Text, addressVal,
                        memoryMgr.ReadByte(addressVal),
                        memoryMgr.ReadWord(addressVal)
                    );
                    watchList.Add(addressVal, mem);
                    WatchGrid.RowCount = watchList.Count;
                    NameText.Text = "";
                    AddressText.Text = "";
                }
            }
        }

        private void WatchForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
