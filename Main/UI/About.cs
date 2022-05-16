using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        public static string AppVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        private void AboutFrom_Load(object sender, EventArgs e)
        {
            
            label1.Text = "Foenix IDE Version " + AppVersion();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
