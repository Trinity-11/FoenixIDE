using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nu64.CharEdit
{
    public partial class CharEditMain : Form
    {

        public CharEditMain()
        {
            InitializeComponent();
        }

        private void clearrToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "ROM file|*.bin|PNG Image|*.png|BMP Image|*.bmp|All Files|*.*";
            if (f.ShowDialog() == DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(f.FileName).ToLower();
                switch (ext)
                {
                    case ".bin":
                        charViewer1.SaveBin(f.FileName, charViewer1.FontData);
                        break;
                    case ".png":
                    case ".bmp":
                        //charViewer1.InputData = charViewer1.LoadPNG(f.FileName);
                        break;
                }
                charViewer1.Refresh();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "PNG Image|*.png|BMP Image|*.bmp|ROM BIN file|*.bin|All Files|*.*";
            if (f.ShowDialog() == DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(f.FileName).ToLower();
                switch (ext)
                {
                    case ".bin":
                        charViewer1.FontData = charViewer1.LoadBin(f.FileName);
                        break;
                    case ".png":
                    case ".bmp":
                        charViewer1.FontData = charViewer1.LoadPNG(f.FileName);
                        break;
                }
                charViewer1.Refresh();
            }
        }

        private void CharEdWindow_Load(object sender, EventArgs e)
        {
            charViewer1.CharacterSelected += CharViewer1_CharacterSelected;
            editControl1.CharacterSaved += EditControl1_CharacterSaved;
        }

        private void EditControl1_CharacterSaved(object sender, EventArgs e)
        {
            charViewer1.Refresh();
        }

        private void CharViewer1_CharacterSelected(object sender, EventArgs e)
        {
            editControl1.LoadCharacter(charViewer1.FontData, charViewer1.SelectedIndex, charViewer1.BytesPerCharacter);
        }
    }
}
