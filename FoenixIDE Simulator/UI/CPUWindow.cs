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

        Processor.Breakpoints breakpoints = new Processor.Breakpoints();

        public static CPUWindow Instance = null;
        private Processor.CPU _cpu = null;
        private FoenixSystem _kernel = null;

        List<string> PrintQueue = new List<string>();
        static StringBuilder lineBuffer = new StringBuilder();

        const int MAX_LINES = 25;
        const int TRIM_LINES = 1;

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void CPUWindow_Load(object sender, EventArgs e)
        {
            PrintTab(REGISTER_COLUMN);
            HeaderTextbox.Text = GetHeaderText();
            ClearTrace();
            RefreshStatus();
        }

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

        private void RunButton_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            CPU.DebugPause = false;
            UpdateTraceTimer.Enabled = true;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = true;
            UpdateTraceTimer.Enabled = false;
            RefreshStatus();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = true;
            UpdateTraceTimer.Enabled = false;

            int steps = 1;
            int.TryParse(stepsInput.Text, out steps);
            Kernel.CPU.DebugPause = false;
            while (!Kernel.CPU.DebugPause && steps-- > 0)
            {
                ExecuteStep();
                //Application.DoEvents();
            }
            RefreshStatus();
            Kernel.CPU.DebugPause = true;
        }

        private void RefreshStatus()
        {
            this.Text = "Debug: " + StepCounter.ToString();
            
            Kernel.gpu.Refresh();

            UpdateStackDisplay();

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

            PrintQueue.Clear();
            PrintNextInstruction();
        }

        private void PrintNextInstruction()
        {
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

        public void UpdateStackDisplay()
        {
            stackText.Clear();
            stackText.AppendText("Top: $" + CPU.Stack.TopOfStack.ToString("X4") + "\r\n");
            stackText.AppendText("SP : $" + CPU.Stack.Value.ToString("X4") + "\r\n");
            stackText.AppendText("N  : " + (CPU.Stack.TopOfStack - CPU.Stack.Value).ToString().PadLeft(4) + "\r\n");
            stackText.AppendText("───────────\r\n");

            // Display all values on the stack
            if (CPU.Stack.Value != CPU.Stack.TopOfStack)
            {
                int i = CPU.Stack.TopOfStack - CPU.Stack.Value;
                if (i > 100)
                {
                    i = 100;
                }
                while (i > 0)
                {
                    int address = CPU.Stack.Value + i;
                    stackText.AppendText(address.ToString("X4") + " " + CPU.Memory[address].ToString("X2") + "\r\n");
                    i--;
                }
            }
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
                    UpdateTraceTimer.Enabled = false;
                    BPCombo.Text = breakpoints.GetHex(pc);
                }
            }
            catch (Exception ex)
            {
                Print(ex.Message);
                CPU.Halt();
            }
        }

        // Don't try to display the CPU information too often
        private void UpdateTrackeTick(object sender, EventArgs e)
        {
            DateTime t = DateTime.Now.AddMilliseconds(UpdateTraceTimer.Interval / 2);
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
            if (BPCombo.Text.Trim() != "")
            {
                breakpoints.Add(BPCombo.Text);
                BPCombo.Text = breakpoints.Format(BPCombo.Text);
                RefreshBreakpoints();
            }
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
            CPU.Stack.Reset();
            stackText.Clear();
        }

        private void locationInput_Validated(object sender, EventArgs e)
        {
            int jumpAddress = Convert.ToInt32(this.locationInput.Text.Replace(":",""), 16);
            String address = jumpAddress.ToString("X6");
            locationInput.Text = address.Substring(0, 2) + ":" + address.Substring(2);
        }
    }
}
