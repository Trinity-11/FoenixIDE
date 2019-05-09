using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoenixIDE.Processor;

namespace FoenixIDE.UI
{
    public partial class CPUWindow : Form
    {
        private int StepCounter = 0;
        private bool isStepOver = false;

        Processor.Breakpoints breakpoints = new Processor.Breakpoints();

        public static CPUWindow Instance = null;
        private FoenixSystem kernel = null;

        Queue<DebugLine> queue = null;
        private Font debugFont = new Font("Consolas", 9f);
        private Brush debugBrush = new SolidBrush(Color.Black);
        private Brush yellowBrush = new SolidBrush(Color.Yellow);
        private Brush lightBlueBrush = new SolidBrush(Color.LightBlue);

        const int ROW_HEIGHT = 13;

        Thread t;
        Point position = new Point();

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public void SetKernel(FoenixSystem kernel)
        {
            this.kernel = kernel;
            registerDisplay1.CPU = kernel.CPU;
        }

        private void ThreadProc()
        {
            while (!kernel.CPU.DebugPause && !kernel.CPU.Waiting)
            {
                ExecuteStep();
            }
        }

        private class DebugLine
        {
            public bool isBreakpoint = false;
            public readonly int PC;
            readonly string Status;
            public readonly string Command;
            public readonly string OpCodes;
            private readonly String pcString;
            public int commandLength = 1;

            override public string ToString()
            {
                return string.Format("{0}  {1} {2}  {3}", pcString, Command, OpCodes, Status);
            }
            public DebugLine(int pc, byte[] command, string opcodes, string status)
            {
                PC = pc;
                pcString = ">" + pc.ToString("X6");

                StringBuilder c = new StringBuilder();
                commandLength = command.Length;
                for (int i = 0; i < command.Length; i++)
                {
                    c.Append(command[i].ToString("X2")).Append(" ");
                }
                for (int i = command.Length; i < 4; i++)
                {
                    c.Append("   ");
                }
                Command = c.ToString();
                OpCodes = opcodes + new string(' ', 14 - opcodes.Length);
                Status = status;
            }
        }
        private List<string> lines = new List<string>();

        private void CPUWindow_Load(object sender, EventArgs e)
        {
            queue = new Queue<DebugLine>(DebugPanel.Height / ROW_HEIGHT);
            HeaderTextbox.Text = " PC      INSTRUCTION                " + kernel.Monitor.GetRegisterHeader();
            ClearTrace();
            RefreshStatus();
            Tooltip.SetToolTip(PlusButton, "Add Breakpoint");
            Tooltip.SetToolTip(MinusButton, "Remove Breakpoint");
            Tooltip.SetToolTip(InspectButton, "Browse Memory");
            Tooltip.SetToolTip(StepOverButton, "Step Over");
        }

        private void DebugPanel_Paint(object sender, PaintEventArgs e)
        {
            if (kernel.CPU.DebugPause)
            {
                int queueLength = queue.Count;
                int i = 0;

                // Draw the position box
                if (position.X > 0 && position.Y > 0)
                {
                    int row = position.Y / ROW_HEIGHT;
                    int col = 12;
                    e.Graphics.FillRectangle(lightBlueBrush, col, row * ROW_HEIGHT, 7 * 6, 14);
                }
                foreach (DebugLine line in queue)
                {
                    if (line != null)
                    {
                        if (line.isBreakpoint)
                        {
                            e.Graphics.FillRectangle(yellowBrush, 0, i * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                        }
                        e.Graphics.DrawString(line.ToString(), debugFont, debugBrush, 4, i * ROW_HEIGHT);
                    }
                    i++;
                }
            }
            else
            {
                e.Graphics.FillRectangle(lightBlueBrush, 0, 0, this.Width, this.Height);
                e.Graphics.DrawString("Running code real fast ... no time to write!", debugFont, debugBrush, 4, DebugPanel.Height / 2);
            }
        }

        private void DebugPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (kernel.CPU.DebugPause)
            {
                if (e.X > 12 && e.X < 12 + 7 * 6 && e.Y / ROW_HEIGHT < queue.Count)
                {
                    int top = e.Y / ROW_HEIGHT * ROW_HEIGHT;
                    if ( (e.Y / ROW_HEIGHT != position.Y / ROW_HEIGHT || position.Y == -1) )
                    {
                        position.X = e.X;
                        position.Y = e.Y;

                        PlusButton.Top = DebugPanel.Top + top - 2;
                        MinusButton.Top = DebugPanel.Top + top - 2;
                        InspectButton.Top = DebugPanel.Top + top - 2;
                        StepOverButton.Top = DebugPanel.Top + top - 2;

                        PlusButton.Left = 13;
                        MinusButton.Left = PlusButton.Left + PlusButton.Width;
                        InspectButton.Left = MinusButton.Left + MinusButton.Width;
                        StepOverButton.Left = InspectButton.Left + InspectButton.Width;

                        PlusButton.Visible = true;
                        MinusButton.Visible = true;
                        InspectButton.Visible = true;
                        int row = position.Y / ROW_HEIGHT;
                        // Only show the Step Over button for Jump and Branch commands
                        if (queue.Count > row)
                        {
                            DebugLine line = queue.ToArray()[row];
                            StepOverButton.Visible = line.OpCodes.StartsWith("B") || line.OpCodes.StartsWith("J");
                        }
                    }
                }
                else
                {
                    position.X = -1;
                    position.Y = -1;
                    PlusButton.Visible = false;
                    MinusButton.Visible = false;
                    InspectButton.Visible = false;
                    StepOverButton.Visible = false;
                }
                DebugPanel.Refresh();
            }
        }

        private void DebugPanel_Leave(object sender, EventArgs e)
        {
            position.X = -1;
            position.Y = -1;
            PlusButton.Visible = false;
            MinusButton.Visible = false;
            InspectButton.Visible = false;
            StepOverButton.Visible = false;
            DebugPanel.Refresh();
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                if (queue.Count > row)
                {
                    DebugLine line = queue.ToArray()[row];
                    string value = line.PC.ToString("X6");
                    BPCombo.Text = "$" + value.Substring(0, 2) + ":" + value.Substring(2);
                    AddBPButton_Click(null, null);
                }
            }
        }


        private void MinusButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = queue.ToArray()[row];
                line.isBreakpoint = false;
                string value = line.PC.ToString("X6");
                BPCombo.Text = "$" + value.Substring(0, 2) + ":" + value.Substring(2);
                DeleteBPButton_Click(null, null);
            }
        }

        private void AddBPButton_Click(object sender, EventArgs e)
        {
            if (BPCombo.Text.Trim() != "")
            {
                int newValue = breakpoints.Add(BPCombo.Text.Trim().Replace(">",""));
                if (newValue > -1)
                {
                    BPCombo.Text = breakpoints.Format(newValue.ToString("X"));
                    UpdateDebugLines(newValue, true);
                    BPLabel.Text = breakpoints.Count.ToString() + " Breakpoints";
                }
            }
        }

        private void DeleteBPButton_Click(object sender, EventArgs e)
        {
            if (BPCombo.Text != "")
            {
                breakpoints.Remove(BPCombo.Text);
                UpdateDebugLines(breakpoints.GetIntFromHex(BPCombo.Text), false);
                BPCombo.Items.Remove(BPCombo.Text);
            }
            if (breakpoints.Count == 0)
                BPCombo.Text = "";
            else
                BPCombo.Text = breakpoints.Values[0];
            BPLabel.Text = breakpoints.Count.ToString() + " Breakpoints";
        }

        private void InspectButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = queue.ToArray()[row];
                MemoryWindow.Instance.GotoAddress(line.PC & 0xFF_FF00);
                MemoryWindow.Instance.BringToFront();
            }
        }
        private void StepOverButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = queue.ToArray()[row];
                // Set a breakpoint to the next address
                int nextAddress = line.PC + line.commandLength;
                int newValue = breakpoints.Add(nextAddress.ToString("X"));

                if (newValue != -1)
                {
                    // Run the CPU until the breakpoint is reached
                    RunButton_Click(null, null);

                    // Ensure the breakpoint is removed
                    isStepOver = true;
                }
            }
        }

        public void RunButton_Click(object sender, EventArgs e)
        {
            DebugPanel_Leave(sender, e);
            kernel.CPU.DebugPause = false;
            RunButton.Enabled = false;
            lastLine.Text = "";
            t = new Thread(new ThreadStart(ThreadProc));
            UpdateTraceTimer.Enabled = true;
            t.Start();
        }

        public void PauseButton_Click(object sender, EventArgs e)
        {
            DebugPanel_Leave(sender, e);
            kernel.CPU.DebugPause = true;
            t?.Join();
            RunButton.Enabled = true;
            UpdateTraceTimer.Enabled = false;
            RefreshStatus();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            DebugPanel_Leave(sender, e);
            kernel.CPU.DebugPause = true;
            t?.Join();
            UpdateTraceTimer.Enabled = false;
            RunButton.Enabled = true;
            int.TryParse(stepsInput.Text, out int steps);
            while (steps-- > 0)
            {
                ExecuteStep();
            }

            RefreshStatus();
            kernel.CPU.DebugPause = true;
        }

        private void RefreshStatus()
        {
            this.Text = "Debug: " + StepCounter.ToString();
            DebugPanel.Refresh();
            UpdateStackDisplay();
        }

        public void UpdateStackDisplay()
        {
            stackText.Clear();
            stackText.AppendText("Top: $" + kernel.CPU.Stack.TopOfStack.ToString("X4") + "\r\n");
            stackText.AppendText("SP : $" + kernel.CPU.Stack.Value.ToString("X4") + "\r\n");
            stackText.AppendText("N  : " + (kernel.CPU.Stack.TopOfStack - kernel.CPU.Stack.Value).ToString().PadLeft(4) + "\r\n");
            stackText.AppendText("───────────\r\n");

            // Display all values on the stack
            if (kernel.CPU.Stack.Value != kernel.CPU.Stack.TopOfStack)
            {
                int i = kernel.CPU.Stack.TopOfStack - kernel.CPU.Stack.Value;
                if (i > 100)
                {
                    i = 100;
                }
                while (i > 0)
                {
                    int address = kernel.CPU.Stack.Value + i;
                    stackText.AppendText(address.ToString("X4") + " " + kernel.CPU.Memory[address].ToString("X2") + "\r\n");
                    i--;
                }
            }
        }

        private void StepsInput_Enter(object sender, EventArgs e)
        {
            if (!(sender is TextBox tb))
                return;

            tb.SelectAll();
        }

        private delegate void breakpointSetter(int pc);
        private void BreakpointReached(int pc)
        {
            if (isStepOver)
            {
                isStepOver = false;
                breakpoints.Remove(pc.ToString("X"));
            }
            else
            {
                BPCombo.Text = breakpoints.GetHex(pc);
            }
            
            RefreshStatus();
            RunButton.Enabled = true;
            
        }
        private delegate void nullParamMethod();
        public void ExecuteStep()
        {
            StepCounter++;

            int currentPC = kernel.CPU.GetLongPC();
            kernel.CPU.ExecuteNext();
            int cmdLength = kernel.CPU.OpcodeLength;
                
            int start = kernel.CPU.GetLongPC();  // is this a duplicate of currentPC?
            byte[] command = new byte[cmdLength];
            for (int i = 0; i < cmdLength; i++)
            {
                command[i] = kernel.CPU.Memory[currentPC + i];
            }
            string opcodes = kernel.CPU.Opcode.ToString(kernel.CPU.SignatureBytes);
            string status = kernel.Monitor.GetRegisterText();
            DebugLine line = new DebugLine(currentPC, command, opcodes, status);
            queue.Enqueue(line);
            if (queue.Count > (DebugPanel.Height/ROW_HEIGHT))
            {
                queue.Dequeue();
            }
            if (breakpoints.ContainsKey(currentPC))
            {
                kernel.CPU.DebugPause = true;
                line.isBreakpoint = true;
                UpdateTraceTimer.Enabled = false;

                Invoke(new breakpointSetter(BreakpointReached), new object[] { currentPC });
            }

            // Print the next instruction on lastLine
            if (!UpdateTraceTimer.Enabled)
            {
                PrintNextInstruction(kernel.CPU.GetLongPC());
            }
        }
        private delegate void lastLineDelegate(string line);
        private void ShowLastLine(string line)
        {
            lastLine.Text = line;
        }
        private void PrintNextInstruction(int pc)
        {
            OpCode oc = kernel.CPU.PreFetch();
            int cmdLength = oc.Length;
            byte[] command = new byte[cmdLength];
            for (int i = 0; i < cmdLength; i++)
            {
                command[i] = kernel.CPU.Memory[pc + i];
            }
            string opcodes = oc.ToString(kernel.CPU.ReadSignature(oc));
            string status = "";
            DebugLine line = new DebugLine(pc, command, opcodes, status);
            if (!lastLine.InvokeRequired)
            {
                lastLine.Text = line.ToString();
            }
            else
            {
                try
                {
                    lastLine.Invoke(new lastLineDelegate(ShowLastLine), new object[] { line.ToString() });
                }
                finally
                { }
            }
            
        }
        public void UpdateDebugLines(int newDebugLine, bool state)
        {
            BPCombo.BeginUpdate();
            BPCombo.Items.Clear();
            foreach (KeyValuePair<int,string> bp in breakpoints)
            {
                BPCombo.Items.Add(bp.Value);
            }
            BPCombo.EndUpdate();
            foreach (DebugLine line in queue)
            {
                if (line.PC == newDebugLine)
                {
                    line.isBreakpoint = state;
                }
            }
            DebugPanel.Refresh();
        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            int pc = breakpoints.GetIntFromHex(locationInput.Text);
            kernel.CPU.SetLongPC(pc);
            ClearTrace();
            kernel.CPU.ExecuteNext();
        }

        private void ClearTraceButton_Click(object sender, EventArgs e)
        {
            ClearTrace();
        }

        public void ClearTrace()
        {
            StepCounter = 0;
            queue.Clear();
            kernel.CPU.Stack.Reset();
            stackText.Clear();
            DebugPanel.Refresh();
            lastLine.Text = "";
        }

        private void LocationInput_Validated(object sender, EventArgs e)
        {
            int jumpAddress = Convert.ToInt32(this.locationInput.Text.Replace(":",""), 16);
            String address = jumpAddress.ToString("X6");
            locationInput.Text = address.Substring(0, 2) + ":" + address.Substring(2);
        }

        // Don't try to display the CPU information too often
        private void UpdateTraceTick(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void CPUWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Kill the thread
            kernel.CPU.DebugPause = true;
            t?.Join();
        }
    }
}
