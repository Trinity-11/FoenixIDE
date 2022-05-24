using System;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class SDCardWindow : Form
    {
        public SDCardWindow()
        {
            InitializeComponent();
            CapacityCombo.SelectedIndex = 3; // 64 MB
            ClusterCombo.SelectedIndex = 0;  // 512 bytes
            FSTypeCombo.SelectedIndex = 2;   // FAT32
        }

        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            if (Iso_selection.Checked)
            {
                if (FileDialog.ShowDialog() == DialogResult.OK)
                {
                    SDCardFolderText.Text = FileDialog.FileName;
                    SDCardEnabled.Checked = true;
                }
            }
            else
            {
                if (SDCardFolderText.Text.Length == 0)
                {
                    FolderDialog.SelectedPath = Application.StartupPath;
                }
                if (FolderDialog.ShowDialog() == DialogResult.OK)
                {
                    SDCardFolderText.Text = FolderDialog.SelectedPath;
                    SDCardEnabled.Checked = true;
                }
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Virtual SD Card Path
        public void SetPath(string path)
        {
            if (path != null && path.Length > 0)
            {
                SDCardEnabled.Checked = true;
                SDCardFolderText.Text = path;
            }
            else
            {
                SDCardEnabled.Checked = false;
                SDCardFolderText.Text = "";
            }

        }
        public string GetPath()
        {
            return SDCardEnabled.Checked ? SDCardFolderText.Text : null;
        }

        // Virtual SD Card Capacity
        public void SetCapacity(int value)
        {
            int index = CapacityCombo.Items.IndexOf(value);
            if (index != -1)
            {
                CapacityCombo.SelectedIndex = index;
            }
        }
        public int GetCapacity()
        {
            return Convert.ToInt32(CapacityCombo.SelectedItem);
        }

        // Virtual SD Card Cluster Size
        public void SetClusterSize(int value)
        {
            int index = ClusterCombo.Items.IndexOf(value);
            if (index != -1)
            {
                ClusterCombo.SelectedIndex = index;
            }
        }
        public int GetClusterSize()
        {
            return Convert.ToInt32(ClusterCombo.SelectedItem);
        }

        // Virtual SD Card Filesystem Type
        public void SetFSType(string fsname)
        {
            int index = FSTypeCombo.Items.IndexOf(fsname);
            if (index != -1)
            {
                FSTypeCombo.SelectedIndex = index;
            }
        }
        public string GetFSType()
        {
            return (string)FSTypeCombo.SelectedItem;
        }

        private void SDCardEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CapacityCombo.Enabled = Iso_selection.Checked ? false : SDCardEnabled.Checked;
            FSTypeCombo.Enabled = Iso_selection.Checked ? false : SDCardEnabled.Checked;
            ClusterCombo.Enabled = Iso_selection.Checked ? false : SDCardEnabled.Checked;
            if (!SDCardEnabled.Checked)
            {
                SDCardFolderText.Text = "";
                CapacityCombo.SelectedIndex = 3;
                FSTypeCombo.SelectedIndex = 2;
                ClusterCombo.SelectedIndex = 0;
            }
        }

        public bool GetISOMode()
        {
            return Iso_selection.Checked ? true : false;
        }

        private void Iso_selection_CheckedChanged(object sender, EventArgs e)
        {
            TypeLabel.Text = Iso_selection.Checked ? "Image:" : "Folder:";
            if (Iso_selection.Checked)
            {
                CapacityCombo.SelectedIndex = -1;
                CapacityCombo.Enabled = false;
                ClusterCombo.SelectedIndex = -1;
                ClusterCombo.Enabled = false;
                FSTypeCombo.SelectedIndex = -1;
                FSTypeCombo.Enabled = false;
            }
            else
            {
                CapacityCombo.SelectedIndex = 3; // 64 MB
                CapacityCombo.Enabled = true;
                ClusterCombo.SelectedIndex = 0; // 512
                ClusterCombo.Enabled = true;
                FSTypeCombo.SelectedIndex = 2; // FAT32
                FSTypeCombo.Enabled = true;
            }
            SDCardFolderText.Text = "";
        }

        private void CapacityCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CapacityCombo.SelectedIndex)
            {
                case 0: // 8MB
                    ClusterCombo.SelectedIndex = 3; // 4096
                    FSTypeCombo.SelectedIndex = 0; // FAT12
                    break;
                case 1: // 16 MB
                    ClusterCombo.SelectedIndex = 4; // 8192
                    FSTypeCombo.SelectedIndex = 0; // FAT12
                    break;
                case 2: // 32 MB
                    ClusterCombo.SelectedIndex = 1; // 1024
                    FSTypeCombo.SelectedIndex = 1; // FAT16
                    break;
                case 3: // 64 MB
                    ClusterCombo.SelectedIndex = 2; // 2048
                    FSTypeCombo.SelectedIndex = 1; // FAT16
                    break;
                case 4: // 128 MB
                    ClusterCombo.SelectedIndex = 3; // 4096
                    FSTypeCombo.SelectedIndex = 1; // FAT16
                    break;
                case 5: // 256 MB
                    ClusterCombo.SelectedIndex = 0; // 512
                    FSTypeCombo.SelectedIndex = 2; // FAT32
                    break;
                case 6: // 512 MB
                    ClusterCombo.SelectedIndex = 1; // 1024
                    FSTypeCombo.SelectedIndex = 2; // FAT32
                    break;
                case 7: // 1024 MB
                    ClusterCombo.SelectedIndex = 2; // 2048
                    FSTypeCombo.SelectedIndex = 2; // FAT32
                    break;
                case 8: // 2048 MB
                    ClusterCombo.SelectedIndex = 3; // 4096
                    FSTypeCombo.SelectedIndex = 2; // FAT32
                    break;
            }
        }
    }
}
