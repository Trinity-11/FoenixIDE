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
        }

        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            if (SDCardFolderText.Text.Length == 0)
            {
                FolderDialog.SelectedPath = Application.StartupPath;
            }
            if (FolderDialog.ShowDialog() == DialogResult.OK) {
                SDCardFolderText.Text = FolderDialog.SelectedPath;
                SDCardEnabled.Checked = true;
                Close();
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

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

        private void SDCardEnabled_CheckedChanged(object sender, EventArgs e)
        {
            FolderSelectButton.Enabled = SDCardEnabled.Checked;
            if (!SDCardEnabled.Checked)
            {
                SDCardFolderText.Text = "";
            }
        }
    }
}
