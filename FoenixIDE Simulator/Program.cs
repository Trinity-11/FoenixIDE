using FoenixIDE.UI;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow(args));
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Application.Exit();
        }
    }
}
