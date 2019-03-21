using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoenixIDE.Processor;

namespace FoenixIDE.UI
{
    public partial class CPUWindow : Form
    {
        private const int MNEMONIC_COLUMN = 22;
        private const int REGISTER_COLUMN = 34;
        private int StepCounter = 0;
        string[] listing;
        Processor.Breakpoints breakpoints = new Processor.Breakpoints();

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public static CPUWindow Instance = null;
        private Processor.CPU _cpu = null;
        private FoenixSystem _kernel = null;

        List<string> PrintQueue = new List<string>();
        static StringBuilder lineBuffer = new StringBuilder();

        public CPU CPU
        {
            get
            {
                return this._cpu;
            }

            set
            {
                this._cpu = value;
                registerDisplay1.CPU = value;
            }
        }

        public FoenixSystem Kernel
        {
            get
            {
                return this._kernel;
            }

            set
            {
                this._kernel = value;
            }
        }


        public static void PrintChar(char c)
        {
            if (c == '\r')
            {
                PrintLine(lineBuffer.ToString());
                lineBuffer.Clear();
            }
            else if (c == '\n')
            {
                // do nothing
            }
            else
            {
                lineBuffer.Append(c);
            }
        }

        public static void PrintTab(int x)
        {
            while (lineBuffer.Length < x)
                lineBuffer.Append(" ");
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = true;
            timer1.Enabled = false;
            RefreshStatus();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = true;
            timer1.Enabled = false;

            int steps = 1;
            int.TryParse(stepsInput.Text, out steps);
            Kernel.CPU.DebugPause = false;
            while (!Kernel.CPU.DebugPause && steps-- > 0)
            {
                ExecuteStep();
                Application.DoEvents();
            }
            RefreshStatus();
            Kernel.CPU.DebugPause = true;
        }

        private void RefreshStatus()
        {
            this.Text = "Debug: " + StepCounter.ToString();
            UpdateStackDisplay();
            Kernel.gpu.Refresh();

            string[] lines = new string[messageText.Lines.Length + PrintQueue.Count];
            messageText.Lines.CopyTo(lines, 0);
            PrintQueue.CopyTo(lines, messageText.Lines.Length);

            int startIndex = 0;
            int itemCount = lines.Length;
            if (itemCount > 128)
            {
                startIndex = itemCount - 128;
                string[] decimated = lines.Skip(startIndex).ToArray();
                messageText.Lines = decimated;
            }
            else
            {
                messageText.Lines = lines;
            }
            messageText.SelectionStart = messageText.TextLength;
            messageText.ScrollToCaret();

            //if (PrintQueue.Count > 5)
            //{
            //    string[] lines = new string[messageText.Lines.Length + PrintQueue.Count];
            //    if (messageText.Lines.Length > 0)
            //        Array.Copy(messageText.Lines, lines, messageText.Lines.Length);
            //    PrintQueue.CopyTo(lines, messageText.Lines.Length);
            //    messageText.Lines = lines;

            //    messageText.AppendText(" ");
            //}
            //else
            //{
            //    for (int i = 0; i < PrintQueue.Count; i++)
            //    {
            //        if (messageText.Lines.Length > 127)
            //        {
            //            //skip one line
            //            messageText.Lines = messageText.Lines.Skip(1).ToArray();
            //        }
            //        messageText.AppendText("\r\n");
            //        messageText.AppendText(PrintQueue[i]);
            //    }
            //}
            PrintQueue.Clear();
            PrintNextInstruction();
        }

        private void PrintNextInstruction()
        {
            //if (listing != null || listing.Length > 0)
            //{
            //    string pc = "." + CPU.GetLongPC().ToString("x6");
            //    foreach (string line in listing)
            //    {
            //        if (line.StartsWith(pc))
            //        {
            //            lastLine.Text = line;
            //            return;
            //        }
            //    }
            //}

            OpCode oc = CPU.PreFetch();
            int start = CPU.GetLongPC();
            PrintPC(start);

            int end = start + oc.Length;
            for (int i = start; i < end; i++)
            {
                Print(CPU.Memory[i].ToString("X2"));
                Print(" ");
            }

            int s = CPU.ReadSignature(oc);
            PrintTab(MNEMONIC_COLUMN);
            Print(oc.ToString(s));
            //PrintTab(REGISTER_COLUMN);
            //Print(Kernel.Monitor.GetRegisterText());
            Instance.lastLine.Text = lineBuffer.ToString();
            lineBuffer.Clear();
        }

        string GetHeaderText()
        {
            StringBuilder s = new StringBuilder();
            s.Append("PC    INSTRUCTION");
            s.Append(new string(' ', REGISTER_COLUMN - s.Length));
            s.Append(Kernel.Monitor.GetRegisterHeader());
            return s.ToString();
        }

        const int MAX_LINES = 25;
        const int TRIM_LINES = 1;
        private void PrintPC(int pc1)
        {
            Print("." + pc1.ToString("X6") + "  ");
        }

        public void PrintStatus(int lastPC, int newPC)
        {
            for (int i = lastPC; i < newPC; i++)
            {
                Print(CPU.Memory[i].ToString("X2"));
                Print(" ");
            }
            PrintTab(MNEMONIC_COLUMN);
            Print(CPU.Opcode.ToString(CPU.SignatureBytes));
            PrintTab(REGISTER_COLUMN);
            Print(Kernel.Monitor.GetRegisterText());
            PrintLine();
        }

        public static void Print(string message)
        {
            lineBuffer.Append(message);
        }

        public static void PrintLine(string message)
        {
            lineBuffer.Append(message);
            PrintLine();
        }

        public static void PrintClear()
        {
            lineBuffer.Clear();
        }

        public static void PrintLine()
        {
            Instance.PrintQueue.Add(lineBuffer.ToString());
            PrintClear();
        }

        int TopOfStack = 0xd6ff;
        public void UpdateStackDisplay()
        {
            if (CPU.Stack.Value > TopOfStack)
                TopOfStack = CPU.Stack.Value;

            stackText.Clear();
            stackText.AppendText("Top: $" + TopOfStack.ToString("X4") + "\r\n");
            stackText.AppendText("SP : $" + CPU.Stack.Value.ToString("X4") + "\r\n");
            stackText.AppendText("N  : " + (TopOfStack - CPU.Stack.Value).ToString().PadLeft(4) + "\r\n");
            stackText.AppendText("───────────\r\n");

            int i = TopOfStack;
            if (CPU.Stack.Value == 0)
                i = 0;
            else if (CPU.Stack.Value - i > 1000)
                i = CPU.Stack.Value - 1000;
            while (i > CPU.Stack.Value)
            {
                stackText.AppendText(i.ToString("X4") + " " + CPU.Memory[i].ToString("X2") + "\r\n");
                i--;
            }

        }

        const int COUNTER_STEPS = 1000;
        private void RunButton_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            //int counter = COUNTER_STEPS;
            CPU.DebugPause = false;
            timer1.Enabled = true;
        }

        private void locationInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void stepsInput_Enter(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            tb.SelectAll();
        }

        public void ExecuteStep()
        {
            try
            {
                StepCounter++;

                PrintClear();

                int pc1 = CPU.GetLongPC();
                PrintPC(pc1);
                CPU.ExecuteNext();
                int pc2 = pc1 + CPU.Opcode.Length;
                PrintStatus(pc1, pc2);

                int pc = CPU.GetLongPC();
                if (breakpoints.ContainsKey(pc))
                {
                    CPU.DebugPause = true;
                    timer1.Enabled = false;
                    BPCombo.Text = breakpoints.GetHex(pc);
                }
            }
            catch (Exception ex)
            {
                Print(ex.Message);
                CPU.Halt();
            }
        }

        private void CPUWindow_Load(object sender, EventArgs e)
        {
            //if (messageText.Lines.Length == 0)
            //    PrintLine(GetHeaderText());
            PrintTab(REGISTER_COLUMN);
            HeaderTextbox.Text = GetHeaderText();
            //PrintLine(Kernel.Monitor.GetRegisterText());
            ClearTrace();
            RefreshStatus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime t = DateTime.Now.AddMilliseconds(timer1.Interval / 2);
            while (DateTime.Now < t)
            {
                if (CPU.DebugPause || CPU.Waiting)
                    break;
                ExecuteStep();
            }
            RefreshStatus();
        }

        private void RefreshBreakpoints()
        {
            BPCombo.Items.Clear();
            foreach (string s in breakpoints.Values)
            {
                BPCombo.Items.Add(s);
            }
            BPLabel.Text = breakpoints.Count.ToString() + " Breakpoints";
        }

        private void AddBPButton_Click(object sender, EventArgs e)
        {
            breakpoints.Add(BPCombo.Text);
            BPCombo.Text = breakpoints.Format(BPCombo.Text);
            RefreshBreakpoints();
        }

        private void DeleteBPButton_Click(object sender, EventArgs e)
        {
            if (BPCombo.Text != "")
                breakpoints.Remove(BPCombo.Text);
            if (breakpoints.Count == 0)
                BPCombo.Text = "";
            else
                BPCombo.Text = breakpoints.Values[0];
            RefreshBreakpoints();
        }

        private void MemoryButton_Click(object sender, EventArgs e)
        {

        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            int pc = breakpoints.GetIntFromHex(locationInput.Text);
            CPU.SetLongPC(pc);
            ClearTrace();
            CPU.ExecuteNext();
        }

        private void ClearTraceButton_Click(object sender, EventArgs e)
        {
            ClearTrace();
        }

        public void ClearTrace()
        {
            StepCounter = 0;
            messageText.Clear();
            //this.listing = global::System.IO.File.ReadAllLines(@"ROMs\kernel.lst");
        }
    }
}
