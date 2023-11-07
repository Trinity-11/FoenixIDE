using FoenixIDE.UI;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using FoenixIDE.Simulator.UI;

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
            Dictionary<string, string> context = null;
            bool OkToContinue = true;
            if (args.Length > 0)
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                context = DecodeProgramArguments(args);
                OkToContinue = "true".Equals(context["Continue"]);
            }
            if (OkToContinue)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow(context));
            }
        }

        private static Dictionary<String,String> DecodeProgramArguments(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);

            Dictionary<string, string> context = new Dictionary<string, string>
            {
                { "Continue", "true" }
            };
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].Trim())
                {
                    // the hex file to load is specified
                    case "-k":
                    case "--kernel":
                        // a kernel file must be specified
                        if (args[i + 1].Trim().StartsWith("-") 
                            || (
                                !args[i + 1].Trim().ToLower().EndsWith(".hex") 
                                && !args[i + 1].Trim().ToLower().EndsWith(".pgz") 
                                && !args[i + 1].Trim().ToLower().EndsWith(".pgx")
                                && !args[i + 1].Trim().ToLower().EndsWith(".bin")))
                        {
                            Console.Out.WriteLine(" - You must specify a hex, pgz, pgx or bin file.");
                            context["Continue"] = "false";
                        }
                        context.Add("defaultKernel", args[i + 1]);
                        i++; // skip the next argument
                        break;
                    case "-j":
                    case "--jump":
                        // An address must be specified
                        int value = Convert.ToInt32(args[i + 1].Replace("$:", ""), 16);
                        if (value != 0)
                        {
                            context.Add("jumpStartAddress", value.ToString());
                            i++; // skip the next argument
                        }
                        else
                        {
                            Console.Out.WriteLine("Invalid address specified: " + args[i + 1]);
                            context["Continue"] = "false";
                        }
                        break;
                    // Autorun - a value is not expected for this one
                    case "-r":
                    case "--run":
                        string runValue = "true";
                        if (args.Length > (i + 1) && !args[i+1].StartsWith("-"))
                        {
                            runValue = args[i + 1];
                            if (!"true".Equals(runValue) && !"false".Equals(runValue))
                            {
                                runValue = "true";
                            }
                            i++; // skip the next argument
                        }
                        context.Add("autoRun", runValue);
                        break;
                    // Disable IRQs - a value is not expected for this one
                    case "-i":
                    case "--irq":
                        context.Add("disabledIRQs", "true");
                        break;
                    // Board Version B, C, U, U+, Jr, Jr816
                    case "-b":
                    case "--board":
                        string verArg = args[i + 1];
                        if (verArg.StartsWith("-"))
                        {
                            Console.Out.WriteLine("Invalid board specified: " + verArg);
                            context["Continue"] = "false";
                        }
                        else
                        {
                            switch (verArg.ToLower())
                            {
                                case "b":
                                    context.Add("version", "RevB");
                                    break;
                                case "c":
                                    context.Add("version", "RevC");
                                    break;
                                case "u":
                                    context.Add("version", "RevU");
                                    break;
                                case "u+":
                                    context.Add("version", "RevU+");
                                    break;
                                case "jr":
                                    context.Add("version", "RevJr");
                                    break;
                                case "jr816":
                                    context.Add("version", "RevJr816");
                                    break;
                                case "f256k":
                                    context.Add("version", "RevF256K");
                                    break;
                                case "f256k816":
                                    context.Add("version", "RevF256K816");
                                    break;
                                default:
                                    Console.Out.WriteLine("Invalid board specified: " + verArg + ". Must be one of b, c, u, u+, jr, jr816, f256k, f256k816");
                                    context["Continue"] = "false";
                                    break;
                            }
                            i++;
                        }
                        break;
                    case "-h":
                    case "/?":
                    case "--help":
                        DisplayUsage();
                        context["Continue"] = "false";
                        break;
                    case "-v":
                    case "--version":
                        string version = AboutForm.AppVersion();
                        Console.Out.WriteLine("FoenixIDE version: " + version);
                        context["Continue"] = "false";
                        break;
                    default:
                        Console.Out.WriteLine("Unknown switch used:" + args[i].Trim());
                        DisplayUsage();
                        context["Continue"] = "false";
                        break;
                }
            }
            // Add a carriage return
            IntPtr cw = GetConsoleWindow();
            SendMessage(cw, WM_CHAR, (IntPtr)VK_ENTER, IntPtr.Zero);

            FreeConsole();
            return context;
        }

        static void DisplayUsage()
        {
            Console.Out.WriteLine("Foenix IDE Command Line Usage:");
            Console.Out.WriteLine("   -k, --kernel: kernel file name - must be .hex, .pgx, .pgz or .bin file");
            Console.Out.WriteLine("   -j, --jump: jump to specified address");
            Console.Out.WriteLine("   -r, --run: autorun true/false");
            Console.Out.WriteLine("   -i, --irq: disable IRQs true/false");
            Console.Out.WriteLine("   -b, --board: board revision b, c, u, u+, jr, jr816");
            Console.Out.WriteLine("   -h, --help, /?: show this usage");
            Console.Out.WriteLine("   -v, --version: show the version");
        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Application.Exit();
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        const uint WM_CHAR = 0x0102;
        const int VK_ENTER = 0x0D;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();
    }
}
