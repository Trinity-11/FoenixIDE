using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class SDCardWindow : Form
    {
        public SDCardWindow()
        {
            InitializeComponent();
            CapacityCombo.SelectedIndex = 3;
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

        private void SDCardEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CapacityCombo.Enabled = Iso_selection.Checked ? false : SDCardEnabled.Checked;
            if (!SDCardEnabled.Checked)
            {
                SDCardFolderText.Text = "";
                CapacityCombo.SelectedIndex = 3;
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
            }
            else
            {
                CapacityCombo.SelectedIndex = 3;
                CapacityCombo.Enabled = true;
            }
            SDCardFolderText.Text = "";
        }
    }
}
