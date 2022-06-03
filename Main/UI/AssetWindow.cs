using System;
using System.IO;
using System.Windows.Forms;
using static FoenixIDE.Simulator.FileFormat.ResourceChecker;

namespace FoenixIDE.UI
{
    public partial class AssetWindow : Form
    {
        private FoenixSystem kernel_ref;
        public static AssetWindow Instance;


        public AssetWindow()
        {
            InitializeComponent();
            AssetGrid.CellValueNeeded += AssetGrid_CellValueNeeded;
            Instance = this;
        }

        public void SetKernel(FoenixSystem krnl)
        {
            kernel_ref = krnl;
            AssetGrid.RowCount = kernel_ref.ResCheckerRef.Items.Count;
        }

        private void AssetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void AssetGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                Resource res = kernel_ref.ResCheckerRef.Items[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = res.Name;
                        break;
                    case 1:
                        e.Value = res.StartAddress.ToString("X6");
                        break;
                    case 2:
                        e.Value = (res.StartAddress + res.Length - 1).ToString("X6");
                        break;
                    case 3:
                        e.Value = res.FileType.ToString();
                        break;
                }

            }
            catch
            {
                // whatever!
            }
        }

        private void AddAssetButton_Click(object sender, EventArgs e)
        {
            MainWindow.Instance.LoadImageToolStripMenuItem_Click(this, null);
        }

        private void AssetGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the resource for the RowIndex
            if (e.RowIndex > -1)
            {
                Resource res = kernel_ref.ResCheckerRef.Items[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    // Export the asset to file
                    case 4:
                        FileInfo info = new FileInfo(res.Name);
                        SaveFileDialog saveDlg = new SaveFileDialog
                        {
                            Title = "Save Asset to File",
                            FileName = info.Name
                        };
                        switch (res.FileType)
                        {
                            case ResourceType.lut:
                                saveDlg.Filter = "Palette File (*.pal)|*.pal|Raw File (*.bin)|*.bin|Tilemap File (*.tlm)|*.tlm";
                                break;
                            case ResourceType.tilemap:
                                saveDlg.Filter = "Tilemap File (*.tlm)|*.tlm|Palette File (*.pal)|*.pal|Raw File (*.bin)|*.bin";
                                break;
                            default:
                                saveDlg.Filter = "Raw File (*.bin)|*.bin|Palette File (*.pal)|*.pal|Tilemap File (*.tlm)|*.tlm";
                                break;
                        }
                        if (saveDlg.ShowDialog() == DialogResult.OK)
                        {

                            FileStream dataFile = File.Create(saveDlg.FileName, 0x800, FileOptions.SequentialScan);
                            byte[] buffer = new byte[res.Length];
                            kernel_ref.MemMgr.CopyIntoBuffer(res.StartAddress, res.Length, buffer);
                            dataFile.Write(buffer, 0, res.Length);
                            dataFile.Close();

                        }
                        break;
                    // Browse this page in the Memory Window
                    case 5:
                        MemoryWindow.Instance.GotoAddress(res.StartAddress & 0xFFFF00);
                        break;
                    // Delete the row, but copy the values into our input boxes
                    case 6:
                        kernel_ref.ResCheckerRef.Items.Remove(res);
                        // zero the data at the location
                        byte[] zeroes = new byte[res.Length];
                        kernel_ref.MemMgr.CopyBuffer(zeroes, 0, res.StartAddress, res.Length);
                        AssetGrid.RowCount -= 1;
                        break;
                }
            }
        }

        public void UpdateAssets()
        {
            AssetGrid.RowCount = kernel_ref.ResCheckerRef.Items.Count;
        }
    }
}
