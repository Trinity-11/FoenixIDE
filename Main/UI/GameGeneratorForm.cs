using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public partial class GameGeneratorForm : Form
    {
        public GameGeneratorForm()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Title = "Pick a Foenix Game File to Open";
            openDlg.Filter = "FGM (*.fgm)|*.fgm";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream file = File.OpenRead(openDlg.FileName);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                CodeTextBox.Text = Encoding.ASCII.GetString(buffer);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Title = "Pick a Foenix Game File to Save";
            saveDlg.Filter = "FGM (*.fgm)|*.fgm";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream file = File.Create(saveDlg.FileName);
                byte[] buffer = Encoding.ASCII.GetBytes(CodeTextBox.Text);
                file.Write(buffer, 0, buffer.Length);

            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void GenerateASMButton_Click(object sender, EventArgs e)
        {
            // parse the code and generate .asm file(s)
        }

        private void ViewAssetsButton_Click(object sender, EventArgs e)
        {
            // show the asset window
        }

        private void GameGeneratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // this cancels the close event.
            this.Hide();
        }
    }
}
