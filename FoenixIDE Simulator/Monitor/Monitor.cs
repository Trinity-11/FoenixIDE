using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE;
using FoenixIDE.MemoryLocations;

namespace FoenixIDE.Monitor
{
    public class Monitor : ReadyHandler
    {
        public FoenixSystem kernel = null;

        char commmand = ' ';
        int[] args = new int[5];

        public Monitor(FoenixSystem NewKernel)
        {
            this.kernel = NewKernel;
        }

        public void Ready()
        {
            PrintRegisters();
        }

        public string GetRegisterText()
        {
            StringBuilder s = new StringBuilder();
            s.Append(';');
            s.Append(kernel.CPU.GetLongPC().ToString("X6"));
            s.Append(' ');
            s.Append(kernel.CPU.A.Value.ToString("X4"));
            s.Append(' ');
            s.Append(kernel.CPU.X.Value.ToString("X4"));
            s.Append(' ');
            s.Append(kernel.CPU.Y.Value.ToString("X4"));
            s.Append(' ');
            s.Append(kernel.CPU.Stack.Value.ToString("X4"));
            s.Append(' ');
            s.Append(kernel.CPU.DataBank.Value.ToString("X2"));
            s.Append(' ');
            s.Append(' ');
            s.Append(kernel.CPU.DirectPage.Value.ToString("X4"));
            s.Append(' ');
            s.Append(kernel.CPU.Flags.ToString());
            return s.ToString();
        }

        public void PrintRegisters(bool printHeader = true)
        {
            //  PC     A    X    Y    SP   DBR DP   NVMXDIZC
            // ;000000 0000 0000 0000 0000 00  0000 11111111
            if (printHeader)
                PrintRegisterHeader();
            kernel.PrintLine(GetRegisterText());
        }

        public string GetRegisterHeader()
        {
            return " PC     A    X    Y    SP   DBR DP   NVMXDIZC";
        }

        public void PrintRegisterHeader()
        {
            kernel.PrintLine(GetRegisterHeader());
        }

        public void PrintStoredRegisters()
        {
            //  PC     A    X    Y    SP   DBR DP   NVMXDIZC
            // ;000000 0000 0000 0000 0000 00  0000 11111111
            PrintRegisterHeader();
            kernel.PrintChar(';');
            kernel.PrintMemHex(3, MemoryMap.CPUPC);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap.CPUA);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap.CPUX);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap.CPUY);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap.CPUSTACK);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(1, MemoryMap.CPUDBR);
            kernel.PrintChar(' ');
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap.CPUDP);
            kernel.PrintChar(' ');
            kernel.PrintMemBinary(1, MemoryMap.CPUFLAGS);
            kernel.PrintChar(' ');
            kernel.PrintLine();
        }

        public void ReturnPressed(int LineStart)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < kernel.gpu.ColumnsVisible; i++)
            {
                s.Append(kernel.Memory[LineStart]+i);
            }

            Execute(s.ToString());
        }

        public void PrintGreeting()
        {
            kernel.PrintLine("         Machine Monitor v0.1 (dev)");
        }

        public void Execute(string Line)
        {
            commmand = ' ';
            int pos = 0;
            while (commmand == ' ' && pos < Line.Length)
            {
                commmand = Line[pos];
            }

            switch (commmand)
            {
                case '?':
                    kernel.PrintLine();
                    string s = global::System.IO.File.ReadAllText("Monitor\\Monitor Help.txt");
                    kernel.PrintLine(s);
                    break;
                case ' ':
                case '\0':
                    kernel.PrintLine();
                    break;
                default:
                    kernel.PrintLine();
                    kernel.PrintLine("Error (? for help)");
                    break;
            }
        }
    }
}
