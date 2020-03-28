using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.CharEditor
{
    public partial class CharEditorWindow : Form
    {

        public CharEditorWindow()
        {
            InitializeComponent();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog {
                Filter = "ROM file|*.bin|PNG Image|*.png|BMP Image|*.bmp|All Files|*.*"
            };
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
                        MessageBox.Show("Saving to PNG and BMP not implemented yet");
                        break;
                }
                charViewer1.Refresh();
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog
            {
                Filter = "Image Files (*.BMP *.PNG *.BIN)|*.BMP;*.PNG;*.BIN|ROM file|*.bin|PNG Image|*.png|BMP Image|*.bmp|All Files|*.*"
            };
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

        /**
         * This method is called when a cell is selected in the ViewControl
         */
        private void CharViewer1_CharacterSelected(object sender, EventArgs e)
        {
            int value = charViewer1.SelectedIndex;
            SelectedIndexLabel.Text = "Dec:" + value + ", Hex: $" + value.ToString("X2") + ", Char: " + Convert.ToChar(value);
            editControl1.LoadCharacter(charViewer1.FontData, value, charViewer1.BytesPerCharacter);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void CharEditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CharEditorWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
