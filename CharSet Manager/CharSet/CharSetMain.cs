using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CharSet
{
    public partial class CharSetMain : Form
    {
        public CharSetMain()
        {
            InitializeComponent();
        }

        private void CharSetMain_Load(object sender, EventArgs e)
        {
            //charViewer1.InputData = charViewer1.LoadBin("characters.901225-01.bin");
            //charViewer1.CustomData =  charViewer1.LoadData("custom.bin");
            charViewer1.InputData = charViewer1.LoadPNG("PET-ASCII.png");
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
                        charViewer1.InputData = charViewer1.LoadBin(f.FileName);
                        break;
                    case ".png":
                    case ".bmp":
                        charViewer1.InputData = charViewer1.LoadPNG(f.FileName);
                        break;
                }
                charViewer1.Refresh();
            }
        }

        private void pETSCIIORDERToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AsciiMenu_Click(object sender, EventArgs e)
        {
            charViewer1.ConvertPETtoASCII();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            charViewer1.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            charViewer1.CopyAll();
        }

        private void openPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nonPetCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            charViewer1.CopyNonPET();
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
                        charViewer1.SaveBin(f.FileName, charViewer1.OutputData);
                        break;
                    case ".png":
                    case ".bmp":
                        //charViewer1.InputData = charViewer1.LoadPNG(f.FileName);
                        break;
                }
                charViewer1.Refresh();
            }
        }

        private void x16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            charViewer1.BytesPerCharacter = 16;
            charViewer1.Refresh();
        }

        private void x8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            charViewer1.BytesPerCharacter = 8;
            charViewer1.Refresh();
        }
    }
}
