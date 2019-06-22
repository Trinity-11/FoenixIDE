using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoenixIDE.Processor;
using FoenixIDE.Simulator.FileFormat;

namespace FoenixIDE.UI
{
    public partial class CPUWindow : Form
    {
        private int StepCounter = 0;
        private bool isStepOver = false;

        public Processor.Breakpoints breakpoints = new Processor.Breakpoints();

        public static CPUWindow Instance = null;
        private FoenixSystem kernel = null;

        List<DebugLine> queue = null;
        private Brush labelBrush = new SolidBrush(Color.White);
        private Brush debugBrush = new SolidBrush(Color.Black);
        private Brush yellowBrush = new SolidBrush(Color.Yellow);
        private Brush orangeBrush = new SolidBrush(Color.Orange);
        private Brush redBrush = new SolidBrush(Color.Red);
        private Brush lightBlueBrush = new SolidBrush(Color.LightBlue);

        const int ROW_HEIGHT = 13;
        private int IRQPC = 0; // we only keep track of a single interrupt
        private int TopLineIndex = 0; // this is to help us track which line is the current one being executed

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
            if (kernel.lstFile.Lines.Count > 0)
            {
                queue = kernel.lstFile.Lines;
            }
        }

        private void ThreadProc()
        {
            while (!kernel.CPU.DebugPause && !kernel.CPU.Waiting)
            {
                ExecuteStep();
            }
        }

        private void CPUWindow_Load(object sender, EventArgs e)
        {
            //queue = new Queue<DebugLine>(DebugPanel.Height / ROW_HEIGHT);
            HeaderTextbox.Text = " PC      OPCODES      INSTRUCTION      PC     A    X    Y    SP   DBR DP   NVMXDIZC";
            ClearTrace();
            RefreshStatus();
            Tooltip.SetToolTip(PlusButton, "Add Breakpoint");
            Tooltip.SetToolTip(MinusButton, "Remove Breakpoint");
            Tooltip.SetToolTip(InspectButton, "Browse Memory");
            Tooltip.SetToolTip(StepOverButton, "Step Over");
        }


        private void DebugPanel_Paint(object sender, PaintEventArgs e)
        {
            bool paint = false;
            int currentPC = kernel.CPU.GetLongPC();
            //if ((kernel.CPU.DebugPause))
            if (true)
            {
                int queueLength = queue.Count;
                int painted = 0;
                int index = 0;

                // Draw the position box
                if (position.X > 0 && position.Y > 0)
                {
                    int row = position.Y / ROW_HEIGHT;
                    int col = 12;
                    e.Graphics.FillRectangle(lightBlueBrush, col, row * ROW_HEIGHT, 7 * 6, 14);
                }

                bool offsetPrinted = false;
                foreach (DebugLine line in queue)
                {
                    if (line != null)
                    {
                        if (line.PC == currentPC)
                        {
                            paint = true;
                            TopLineIndex = index;
                            if (!offsetPrinted)
                            {
                                
                                if (index > 4)
                                {
                                    TopLineIndex -= 5;
                                    for (int c = 5; c > 0; c--)
                                    {
                                        DebugLine q0 = queue[index - c];
                                        if (q0.PC == IRQPC)
                                        {
                                            e.Graphics.FillRectangle(orangeBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                        }
                                        if (breakpoints.ContainsKey(q0.PC))
                                        {
                                            e.Graphics.FillRectangle(yellowBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                        }
                                        // Check if the memory still matches the opcodes
                                        if (!q0.CheckOpcodes(kernel.Memory.RAM))
                                        {
                                            e.Graphics.FillRectangle(redBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                        }
                                        if (!q0.isLabel)
                                        {
                                            e.Graphics.DrawString(q0.ToString(), HeaderTextbox.Font, debugBrush, 2, painted * ROW_HEIGHT);
                                        }
                                        else
                                        {
                                            e.Graphics.FillRectangle(debugBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                            e.Graphics.DrawString(q0.ToString(), HeaderTextbox.Font, labelBrush, 2, painted * ROW_HEIGHT);
                                        }
                                        
                                        painted++;
                                    }
                                }
                                offsetPrinted = true;
                            }
                            e.Graphics.FillRectangle(line.PC == IRQPC ? orangeBrush : lightBlueBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                        }
                        if (painted > 26)
                        {
                            paint = false;
                            break;
                        }
                        if (paint)
                        {
                            if (breakpoints.ContainsKey(line.PC))
                            {
                                e.Graphics.FillRectangle(yellowBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            if (line.PC == IRQPC)
                            {
                                e.Graphics.FillRectangle(orangeBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            // Check if the memory still matches the opcodes
                            if (!line.CheckOpcodes(kernel.Memory.RAM))
                            {
                                e.Graphics.FillRectangle(redBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            if (line.PC == currentPC || !line.isLabel)
                            {
                                e.Graphics.DrawString(line.ToString(), HeaderTextbox.Font, debugBrush, 2, painted * ROW_HEIGHT);
                            }
                            else
                            {
                                e.Graphics.FillRectangle(debugBrush, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                e.Graphics.DrawString(line.ToString(), HeaderTextbox.Font, labelBrush, 2, painted * ROW_HEIGHT);
                            }
                            painted++;
                        }
                    }
                    index++;
                }
            }
            else
            {
                e.Graphics.FillRectangle(lightBlueBrush, 0, 0, this.Width, this.Height);
                e.Graphics.DrawString("Running code real fast ... no time to write!", HeaderTextbox.Font, debugBrush, 8, DebugPanel.Height / 2);
            }
        }

        private void DebugPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (kernel.CPU.DebugPause)
            {
                if (e.X > 12 && e.X < 12 + 7 * 6)
                {
                    int top = e.Y / ROW_HEIGHT * ROW_HEIGHT;
                    if ( (e.Y / ROW_HEIGHT != position.Y / ROW_HEIGHT || position.Y == -1) )
                    {
                        position.X = e.X;
                        position.Y = e.Y;

                        PlusButton.Top = DebugPanel.Top + top - 1;
                        MinusButton.Top = DebugPanel.Top + top - 1;
                        InspectButton.Top = DebugPanel.Top + top - 1;
                        StepOverButton.Top = DebugPanel.Top + top - 1;

                        PlusButton.Left = 13;
                        MinusButton.Left = PlusButton.Left + PlusButton.Width;
                        InspectButton.Left = MinusButton.Left + MinusButton.Width;
                        StepOverButton.Left = InspectButton.Left + InspectButton.Width;

                        PlusButton.Visible = true;
                        MinusButton.Visible = true;
                        InspectButton.Visible = true;
                        int row = position.Y / ROW_HEIGHT;
                        // Only show the Step Over button for Jump and Branch commands
                        if (queue.Count > TopLineIndex + row)
                        {
                            DebugLine line = queue[TopLineIndex + row];
                            StepOverButton.Visible = line.StepOver;
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
                if (queue.Count > TopLineIndex + row)
                {
                    DebugLine line = queue[TopLineIndex + row];
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
                if (queue.Count > TopLineIndex + row)
                {
                    DebugLine line = queue[TopLineIndex + row];
                    string value = line.PC.ToString("X6");
                    BPCombo.Text = "$" + value.Substring(0, 2) + ":" + value.Substring(2);
                    DeleteBPButton_Click(null, null);
                }
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
                DebugLine line = queue[TopLineIndex + row];
                MemoryWindow.Instance.GotoAddress(line.PC & 0xFF_FF00);
                MemoryWindow.Instance.BringToFront();
            }
        }

        private void StepOverButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = queue[TopLineIndex + row];
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

        /// <summary>
        /// Executes next step of 65C816 code, logs dubeugging data
        /// if debugging check box is set on CPU Window
        /// </summary>
        public void ExecuteStep()
        {
            StepCounter++;

            int currentPC = kernel.CPU.GetLongPC();
            if (!kernel.CPU.ExecuteNext())
            {
                DebugLine line = null;
                int nextPC = kernel.CPU.GetLongPC();
                if (breakpoints.ContainsKey(nextPC) || (BreakOnIRQCheckBox.Checked && (kernel.CPU.Pins.GetInterruptPinActive || kernel.CPU.CurrentOpcode.Value == 0)))
                {
                    if (UpdateTraceTimer.Enabled)
                    {
                        UpdateTraceTimer.Enabled = false;
                        kernel.CPU.DebugPause = true;
                        //queue.Clear();
                    }
                    if (kernel.CPU.Pins.GetInterruptPinActive)
                    {
                        IRQPC = kernel.CPU.GetLongPC();
                    }
                    if (line == null)
                    {
                        line = GetExecutionInstruction(currentPC);
                    }
                    Invoke(new breakpointSetter(BreakpointReached), new object[] { currentPC });
                }
            }

            // Print the next instruction on lastLine
            if (!UpdateTraceTimer.Enabled)
            {
                //PrintNextInstruction(kernel.CPU.GetLongPC());
            }
        }

        private delegate void lastLineDelegate(string line);
        private void ShowLastLine(string line)
        {
            lastLine.Text = line;
        }

        private DebugLine GetExecutionInstruction(int PC)
        {
            foreach (DebugLine l in queue)
            {
                if (l.PC == PC)
                {
                    return l;
                }
            }
            return null;
        }
        private void PrintNextInstruction(int pc)
        {
            OpCode oc = kernel.CPU.PreFetch();
            int cmdLength = oc.Length;
            byte[] command = new byte[cmdLength];
            for (int i = 0; i < cmdLength; i++)
            {
                command[i] = kernel.Memory.RAM.ReadByte(pc + i);
            }
            string opcodes = oc.ToString(kernel.CPU.ReadSignature(oc, pc));
            //string status = "";
            DebugLine line = new DebugLine(pc, command, opcodes, null);
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

        private void UpdateDebugLines(int newDebugLine, bool state)
        {
            BPCombo.BeginUpdate();
            BPCombo.Items.Clear();
            foreach (KeyValuePair<int,string> bp in breakpoints)
            {
                BPCombo.Items.Add(bp.Value);
            }
            BPCombo.EndUpdate();
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
            IRQPC = 0;
            //queue.Clear();
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

        private void CPUWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RunButton_Click(sender, null);
            }
            else if (e.KeyCode == Keys.F6)
            {
                StepButton_Click(sender, null);
            }
        }
    }
}
