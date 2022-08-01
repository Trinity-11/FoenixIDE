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
            Dictionary<string, string> context = new Dictionary<string, string>
            {
                { "Continue", "true" }
            };
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].Trim())
                {
                    // the hex file to load is specified
                    case "-h":
                    case "--hex":
                        // a kernel file must be specified
                        if (args[i + 1].Trim().StartsWith("-") || !args[i + 1].Trim().EndsWith("hex"))
                        {
                            Console.Out.WriteLine("You must specify a hex file.");
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
                    // Board Version B or C
                    case "-b":
                    case "--board":
                        string verArg = args[i + 1];
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
                        }
                        break;
                    case "--help":
                        DisplayUsage();
                        context["Continue"] = "false";
                        break;
                    default:
                        Console.Out.WriteLine("Unknown switch used:" + args[i].Trim());
                        DisplayUsage();
                        context["Continue"] = "false";
                        break;
                }
            }
            return context;
        }

        static void DisplayUsage()
        {
            Console.Out.WriteLine("Foenix IDE Command Line Usage:");
            Console.Out.WriteLine("   -h, --hex: kernel file name");
            Console.Out.WriteLine("   -j, --jump: jump to specified address");
            Console.Out.WriteLine("   -r, --run: autorun true/false");
            Console.Out.WriteLine("   -i, --irq: disable IRQs true/false");
            Console.Out.WriteLine("   -b, --board: board revision b, c or u");
            Console.Out.WriteLine("   --help: show this usage");
        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Application.Exit();
        }
    }
}
