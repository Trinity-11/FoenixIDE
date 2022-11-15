using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FoenixIDE.Processor;
using FoenixIDE.Simulator.Controls;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using Microsoft.VisualBasic;

namespace FoenixIDE.UI
{
    public partial class CPUWindow : Form
    {
        private int StepCounter = 0;
        private bool isStepOver = false;
        private const int LABEL_WIDTH = 100;

        private Breakpoints knl_breakpoints;
        private List<DebugLine> codeList = null;

        public static CPUWindow Instance = null;
        private FoenixSystem kernel = null;
        private int[] ActiveLine = {0, 0, 0};  // PC, startofline, width - the point of this is to underline the ADDRESS name
        

        const int ROW_HEIGHT = 13;
        private int IRQPC = 0; // we only keep track of a single interrupt
        private int TopLineIndex = 0; // this is to help us track which line is the current one being executed

        Point position = new Point();
        private int MemoryLimit = 0;

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
            DisableIRQs(true);
            registerDisplay1.RegistersReadOnly(false);
        }

        public void SetKernel(FoenixSystem kernel)
        {
            this.kernel = kernel;
            MemoryLimit = kernel.MemMgr.RAM.Length;
            registerDisplay1.CPU = kernel.CPU;
            knl_breakpoints = kernel.Breakpoints;
            if (knl_breakpoints.Count > 0)
            {
                BPLabel.Text = knl_breakpoints.Count.ToString() + " BP";
                // Update the combo
                foreach (KeyValuePair<int, string> kvp in knl_breakpoints)
                {
                    BPCombo.Items.Add(kvp.Value);
                    UpdateDebugLines(kvp.Key, true);
                }
            }
            else
            {
                BPLabel.Text = "Breakpoint";
            }

            UpdateQueue();
            int pc = kernel.CPU.PC;
            DebugLine line = GetExecutionInstruction(pc);
            if (line == null)
            {
                GenerateNextInstruction(pc);
            }
            DebugPanel.Refresh();
        }

        private void UpdateQueue()
        {
            if (kernel.lstFile != null && kernel.lstFile.Lines.Count > 0)
            {
                codeList = new List<DebugLine>(kernel.lstFile.Lines.Count);
                foreach (DebugLine line in kernel.lstFile.Lines.Values)
                {
                    codeList.Add(line);
                }
            }
            else
            {
                codeList = new List<DebugLine>(DebugPanel.Height / ROW_HEIGHT);
                GenerateNextInstruction(kernel.CPU.PC);
            }
        }

        public void DisableIRQs(bool value)
        {
            BreakOnIRQCheckBox.Checked = !value;
        }

        private void ThreadProc()
        {
            while (!kernel.CPU.DebugPause)
            {
                ExecuteStep();
            }
        }

        private void CPUWindow_Load(object sender, EventArgs e)
        {
            
            HeaderTextbox.Text = "LABEL          PC      OPCODES      SOURCE";
            ClearTrace();
            RefreshStatus();
            Tooltip.SetToolTip(AddBPOverlayButton, "Add Breakpoint");
            Tooltip.SetToolTip(DeleteBPOverlayButton, "Remove Breakpoint");
            Tooltip.SetToolTip(InspectOverlayButton, "Browse Memory");
            Tooltip.SetToolTip(StepOverOverlayButton, "Step Over");
            // Register 0
            Tooltip.SetToolTip(SOFCheckbox, "Break on SOF Interrupts");
            Tooltip.SetToolTip(SOLCheckbox, "Break on SOL Interrupts");
            Tooltip.SetToolTip(TMR0Checkbox, "Break on TMR0 Interrupts");
            Tooltip.SetToolTip(TMR1Checkbox, "Break on TMR1 Interrupts");
            Tooltip.SetToolTip(TMR2Checkbox, "Break on TMR2 Interrupts");
            Tooltip.SetToolTip(RTCCheckbox, "Break on RTC Interrupts");
            Tooltip.SetToolTip(FDCCheckbox, "Break on FDC Interrupts");
            Tooltip.SetToolTip(MouseCheckbox, "Break on Mouse Interrupts");

            // Register 1
            Tooltip.SetToolTip(KeyboardCheckBox, "Break on Keyboard Interrupts");
            Tooltip.SetToolTip(COM2Checkbox, "Break on COM2 Interrupts");
            Tooltip.SetToolTip(COM1Checkbox, "Break on COM1 Interrupts");
            Tooltip.SetToolTip(MPU401Checkbox, "Break on MPU401 Interrupts");
            Tooltip.SetToolTip(SDCardCheckBox, "Break on SD Card Interrupts");

            // Register 2
            Tooltip.SetToolTip(OPL2RCheckbox, "Break on OPL2 Right Interrupts");
            Tooltip.SetToolTip(OPL2LCheckbox, "Break on OPL2 Left Interrupts");
            DebugPanel.Paint += new System.Windows.Forms.PaintEventHandler(DebugPanel_Paint);
        }


        private void DebugPanel_Paint(object sender, PaintEventArgs e)
        {
            bool paint = false;
            int currentPC = kernel.CPU.PC;
            //if ((kernel.CPU.DebugPause))
            if (kernel.CPU.DebugPause && codeList != null)
            {
                int queueLength = codeList.Count;
                int painted = 0;
                int index = 0;

                // Draw the position box
                if (position.X > 0 && position.Y > 0)
                {
                    int row = position.Y / ROW_HEIGHT;
                    int col = 12;
                    e.Graphics.FillRectangle(Brushes.LightBlue, col, row * ROW_HEIGHT, 7 * 6, 14);
                }

                bool offsetPrinted = false;
                foreach (DebugLine line in codeList)
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
                                        DebugLine q0 = codeList[index - c];
                                        // Draw the label as a black box with white text
                                        if (q0.label != null)
                                        {
                                            e.Graphics.FillRectangle(Brushes.Blue, 1, painted * ROW_HEIGHT, LABEL_WIDTH + 2, ROW_HEIGHT + 2);
                                            e.Graphics.DrawString(q0.label, HeaderTextbox.Font, Brushes.Yellow, 2, painted * ROW_HEIGHT);
                                        }
                                        if (q0.PC == IRQPC)
                                        {
                                            e.Graphics.FillRectangle(Brushes.Orange, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                        }
                                        if (knl_breakpoints.ContainsKey(q0.PC))
                                        {
                                            e.Graphics.DrawEllipse(Pens.White, LABEL_WIDTH - ROW_HEIGHT - 1, painted * ROW_HEIGHT, ROW_HEIGHT+1, ROW_HEIGHT+1);
                                            e.Graphics.FillEllipse(Brushes.DarkRed, LABEL_WIDTH - ROW_HEIGHT, painted * ROW_HEIGHT + 1, ROW_HEIGHT, ROW_HEIGHT);
                                        }
                                        // Check if the memory still matches the opcodes
                                        if (!q0.CheckOpcodes(kernel.MemMgr.RAM))
                                        {
                                            e.Graphics.FillRectangle(Brushes.Red, LABEL_WIDTH + 3, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                        }
                                        e.Graphics.DrawString(q0.ToString(), HeaderTextbox.Font, Brushes.Black, LABEL_WIDTH + 2, painted * ROW_HEIGHT);
                                        if (q0.PC == ActiveLine[0])
                                        {
                                            e.Graphics.DrawLine(Pens.Black, LABEL_WIDTH + ActiveLine[1], (painted + 1) * ROW_HEIGHT, LABEL_WIDTH + ActiveLine[1] + ActiveLine[2], (painted + 1) * ROW_HEIGHT);
                                        }
                                        painted++;
                                    }
                                }
                                offsetPrinted = true;
                            }
                            e.Graphics.FillRectangle(line.PC == IRQPC ? Brushes.Orange : Brushes.LightBlue, LABEL_WIDTH + 1, painted * ROW_HEIGHT, DebugPanel.Width, ROW_HEIGHT);

                        }
                        if (painted > 27)
                        {
                            paint = false;
                            break;
                        }
                        if (paint)
                        {
                            if (line.label != null)
                            {
                                e.Graphics.FillRectangle(Brushes.Blue, 1, painted * ROW_HEIGHT, LABEL_WIDTH + 2, ROW_HEIGHT + 2);
                                e.Graphics.DrawString(line.label, HeaderTextbox.Font, Brushes.Yellow, 2, painted * ROW_HEIGHT);
                            }
                            if (knl_breakpoints.ContainsKey(line.PC))
                            {
                                e.Graphics.DrawEllipse(Pens.White, LABEL_WIDTH - ROW_HEIGHT - 1, painted * ROW_HEIGHT, ROW_HEIGHT + 1, ROW_HEIGHT + 1);
                                e.Graphics.FillEllipse(Brushes.DarkRed, LABEL_WIDTH - ROW_HEIGHT, painted * ROW_HEIGHT + 1, ROW_HEIGHT, ROW_HEIGHT);
                            }
                            if (line.PC == IRQPC)
                            {
                                e.Graphics.FillRectangle(Brushes.Orange, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            // Check if the memory still matches the opcodes
                            if (!line.CheckOpcodes(kernel.MemMgr.RAM))
                            {
                                e.Graphics.FillRectangle(Brushes.Red, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            e.Graphics.DrawString(line.ToString(), HeaderTextbox.Font, Brushes.Black, 102, painted * ROW_HEIGHT);
                            if (line.PC == ActiveLine[0])
                            {
                                e.Graphics.DrawLine(Pens.Black, LABEL_WIDTH + ActiveLine[1], (painted + 1) * ROW_HEIGHT, LABEL_WIDTH + ActiveLine[1] + ActiveLine[2], (painted + 1) * ROW_HEIGHT);
                            }
                            painted++;
                        }
                    }
                    index++;
                }
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, 0, 0, this.Width, this.Height);
                e.Graphics.DrawString("Running code real fast ... no time to write!", HeaderTextbox.Font, Brushes.Black, 8, DebugPanel.Height / 2);
            }
        }

        private void DebugPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (kernel.CPU.DebugPause)
            {
                if (e.X > 2 && e.X < 2 + LABEL_WIDTH)
                {
                    int top = e.Y / ROW_HEIGHT * ROW_HEIGHT;
                    ActiveLine[0] = 0;
                    DebugPanel.Cursor = Cursors.Default;
                    if ( (e.Y / ROW_HEIGHT != position.Y / ROW_HEIGHT || position.Y == -1) && e.Y / ROW_HEIGHT < 28 )
                    {
                        position.X = e.X;
                        position.Y = e.Y;

                        AddBPOverlayButton.Top = DebugPanel.Top + top - 1;
                        DeleteBPOverlayButton.Top = DebugPanel.Top + top - 1;
                        InspectOverlayButton.Top = DebugPanel.Top + top - 1;
                        StepOverOverlayButton.Top = DebugPanel.Top + top - 1;
                        LabelOverlayButton.Top = DebugPanel.Top + top - 1;

                        AddBPOverlayButton.Left = 3;
                        DeleteBPOverlayButton.Left = AddBPOverlayButton.Left + AddBPOverlayButton.Width;
                        InspectOverlayButton.Left = DeleteBPOverlayButton.Left + DeleteBPOverlayButton.Width;
                        LabelOverlayButton.Left = InspectOverlayButton.Left + InspectOverlayButton.Width;
                        StepOverOverlayButton.Left = LabelOverlayButton.Left + LabelOverlayButton.Width;

                        AddBPOverlayButton.Visible = true;
                        DeleteBPOverlayButton.Visible = true;
                        InspectOverlayButton.Visible = true;
                        LabelOverlayButton.Visible = true;

                        int row = position.Y / ROW_HEIGHT;
                        // Only show the Step Over button for Jump and Branch commands
                        if (codeList != null && codeList.Count > TopLineIndex + row)
                        {
                            DebugLine line = codeList[TopLineIndex + row];
                            StepOverOverlayButton.Visible = line.StepOver;
                        }
                    }
                }
                else
                {
                    position.X = -1;
                    position.Y = -1;
                    AddBPOverlayButton.Visible = false;
                    DeleteBPOverlayButton.Visible = false;
                    InspectOverlayButton.Visible = false;
                    StepOverOverlayButton.Visible = false;
                    LabelOverlayButton.Visible = false;
                    ActiveLine[0] = 0;
                    int row = e.Y / ROW_HEIGHT;
                    if (codeList != null && codeList.Count > TopLineIndex + row)
                    {
                        DebugLine line = codeList[TopLineIndex + row];
                        // try to highlight the word we are over 
                        if (line.HasAddress())
                        {
                            ActiveLine[0] = line.PC;
                            ActiveLine[1] = 174;
                            ActiveLine[2] = line.GetAddressName().Length * 7;
                            DebugPanel.Cursor = Cursors.Hand;
                        }
                    }
                    if (ActiveLine[0] == 0)
                    {
                        DebugPanel.Cursor = Cursors.Default;
                    }
                }
                DebugPanel.Refresh();
            }
        }

        private void DebugPanel_Leave(object sender, EventArgs e)
        {
            position.X = -1;
            position.Y = -1;
            AddBPOverlayButton.Visible = false;
            DeleteBPOverlayButton.Visible = false;
            InspectOverlayButton.Visible = false;
            StepOverOverlayButton.Visible = false;
            LabelOverlayButton.Visible = false;
            DebugPanel.Refresh();
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                if (codeList.Count > TopLineIndex + row)
                {
                    DebugLine line = codeList[TopLineIndex + row];
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
                if (codeList.Count > TopLineIndex + row)
                {
                    DebugLine line = codeList[TopLineIndex + row];
                    string value = line.PC.ToString("X6");
                    BPCombo.Text = "$" + value.Substring(0, 2) + ":" + value.Substring(2);
                    DeleteBPButton_Click(null, null);
                }
            }
        }

        private void LabelOverlayButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                if (codeList.Count > TopLineIndex + row)
                {
                    DebugLine line = codeList[TopLineIndex + row];
                    string oldValue = line.label;
                    string value = Interaction.InputBox("Enter Label for Address: $" + line.PC.ToString("X6").Insert(2, ":"), "Label Dialog", oldValue, Left + LabelOverlayButton.Left + LabelOverlayButton.Width, Top + LabelOverlayButton.Top);
                    line.label = value;
                    DebugPanel.Invalidate();
                }
            }
        }

        private void AddBPButton_Click(object sender, EventArgs e)
        {
            if (BPCombo.Text.Trim() != "")
            {
                int newValue = knl_breakpoints.Add(BPCombo.Text.Trim().Replace(">",""));
                if (newValue > -1)
                {
                    BPCombo.Text = knl_breakpoints.Format(newValue.ToString("X"));
                    UpdateDebugLines(newValue, true);
                    BPLabel.Text = knl_breakpoints.Count.ToString() + " BP";
                }
            }
        }

        private void DeleteBPButton_Click(object sender, EventArgs e)
        {
            if (BPCombo.Text != "")
            {
                knl_breakpoints.Remove(BPCombo.Text);
                UpdateDebugLines(knl_breakpoints.GetIntFromHex(BPCombo.Text), false);
                BPCombo.Items.Remove(BPCombo.Text);
            }
            if (knl_breakpoints.Count == 0)
                BPCombo.Text = "";
            else
                BPCombo.Text = knl_breakpoints.Values[0];
            BPLabel.Text = knl_breakpoints.Count.ToString() + " BP";
        }

        private void InspectButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = codeList[TopLineIndex + row];
                MemoryWindow.Instance.GotoAddress(line.PC & 0xFF_FF00);
                MemoryWindow.Instance.BringToFront();
            }
        }

        public void RunButton_Click(object sender, EventArgs e)
        {
            DebugPanel_Leave(sender, e);
            if (RunButton.Tag.Equals("0"))
            {
                // Clear the interrupt
                IRQPC = -1;
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, 0);
                kernel.MemMgr.INTERRUPT.WriteFromGabe(1, 0);
                if (kernel.GetVersion() != BoardVersion.RevJr)
                {
                    kernel.MemMgr.INTERRUPT.WriteFromGabe(2, 0);
                    kernel.MemMgr.INTERRUPT.WriteFromGabe(3, 0);
                }
               
                InterruptMatchesCheckboxes();
                registerDisplay1.RegistersReadOnly(true);
                MainWindow.Instance.setGpuPeriod(17);
                kernel.CPU.DebugPause = false;
                lastLine.Text = "";
                kernel.CPU.CPUThread = new Thread(new ThreadStart(ThreadProc));
                UpdateTraceTimer.Enabled = true;
                kernel.CPU.CPUThread.Start();
                RunButton.Text = "Pause (F5)";
                RunButton.Tag = "1";
                DebugPanel.Refresh();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            kernel.CPU.DebugPause = true;
            UpdateTraceTimer.Enabled = false;
            kernel.CPU.Halt();
            //kernel.CPU.CPUThread.Join();
            RunButton.Text = "Run (F5)";
            RunButton.Tag = "0";
            RefreshStatus();
            registerDisplay1.RegistersReadOnly(false);
            MainWindow.Instance.setGpuPeriod(500);
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            DebugPanel_Leave(sender, e);
            kernel.CPU.DebugPause = true;
            //kernel.CPU.CPUThread?.Join();
            RunButton.Text = "Run (F5)";
            RunButton.Tag = "0";
            UpdateTraceTimer.Enabled = false;
            ExecuteStep();
            RefreshStatus();
            registerDisplay1.RegistersReadOnly(false);
            MainWindow.Instance.setGpuPeriod(500);
            kernel.CPU.DebugPause = true;
        }

        private void StepOverOverlayButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = codeList[TopLineIndex + row];
                // Set a breakpoint to the next address
                int nextAddress = line.PC + line.commandLength;
                int newValue = knl_breakpoints.Add(nextAddress.ToString("X"));

                if (newValue != -1)
                {
                    // Run the CPU until the breakpoint is reached
                    RunButton_Click(null, null);

                    // Ensure the breakpoint is removed
                    isStepOver = true;
                }
            }
        }

        private void StepOverButton_Click(object sender, EventArgs e)
        {
            int pc = kernel.CPU.PC;
            if (pc > MemoryLimit)
            {
                string errorMessage = "PC exceeds memory limit.";
                lastLine.Text = errorMessage;
                registerDisplay1.PC.BackColor = Color.Red;
                return;
            }
            DebugLine line = GetExecutionInstruction(pc);
            if (line != null && line.StepOver)
            {
                // Set a breakpoint to the next address
                int nextAddress = pc + line.commandLength;
                int newValue = knl_breakpoints.Add(nextAddress.ToString("X"));

                if (newValue != -1)
                {
                    // Run the CPU until the breakpoint is reached
                    RunButton_Click(null, null);

                    // Ensure the breakpoint is removed
                    isStepOver = true;
                    RunButton.Text = "Run (F5)";
                    RunButton.Tag = "0";
                    registerDisplay1.RegistersReadOnly(false);
                    MainWindow.Instance.setGpuPeriod(500);
                }
            }
            else
            {
                ExecuteStep();
                RefreshStatus();
            }
        }

        private void RefreshStatus()
        {
            if (kernel.CPU.DebugPause)
            {
                DebugPanel.Refresh();
                UpdateStackDisplay();
            }
            registerDisplay1.UpdateRegisters();
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
                    stackText.AppendText(address.ToString("X4") + " " + kernel.CPU.MemMgr[address].ToString("X2") + "\r\n");
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
            RunButton.Text = "Run (F5)";
            RunButton.Tag = "0";
            if (isStepOver)
            {
                isStepOver = false;
                knl_breakpoints.Remove(pc.ToString("X"));
            }
            else
            {
                BPCombo.Text = knl_breakpoints.GetHex(pc);
            }
            
            RefreshStatus();
            registerDisplay1.RegistersReadOnly(false);
        }
        private delegate void nullParamMethod();

        /// <summary>
        /// Executes next step of 65C816 code, logs dubeugging data
        /// if debugging check box is set on CPU Window
        /// </summary>
        public void ExecuteStep()
        {
            StepCounter++;
            DebugLine line = null;
            int previousPC = kernel.CPU.PC;

            if (!kernel.CPU.ExecuteNext())
            {

                int nextPC = kernel.CPU.PC;
                if (nextPC > MemoryLimit)
                {
                    UpdateTraceTimer.Enabled = false;
                    kernel.CPU.DebugPause = true;
                    string errorMessage = "PC exceeds memory limit.  Calling instruction at address: $" + previousPC.ToString("X6");
                    if (lastLine.InvokeRequired)
                    {
                        lastLine.Invoke(new lastLineDelegate(ShowLastLine), new object[] { errorMessage });
                    }
                    else
                    {
                        lastLine.Text = errorMessage;
                    }
                    return;
                }
                if (knl_breakpoints.ContainsKey(nextPC) ||
                        kernel.CPU.CurrentOpcode.Value == 0 ||
                        (BreakOnIRQCheckBox.Checked && (kernel.CPU.Pins.GetInterruptPinActive && InterruptMatchesCheckboxes()))
                    )
                {
                    if (kernel.CPU.CurrentOpcode.Value == 0)
                    {
                        if (lastLine.InvokeRequired)
                        {
                            lastLine.Invoke((MethodInvoker)delegate
                            {
                                lastLine.Text = "BRK OpCode read";
                            });
                        }
                        else
                        {
                            lastLine.Text = "BRK OpCode read";
                        }
                    }
                    if (UpdateTraceTimer.Enabled || kernel.CPU.CurrentOpcode.Value == 0)
                    {
                        UpdateTraceTimer.Enabled = false;
                        kernel.CPU.DebugPause = true;
                        //queue.Clear();
                    }
                    if (kernel.CPU.Pins.GetInterruptPinActive && !kernel.CPU.Flags.IrqDisable)
                    {
                        IRQPC = kernel.CPU.PC;
                    }
                    if (line == null)
                    {
                        line = GetExecutionInstruction(nextPC);
                        if (line == null)
                        {
                            GenerateNextInstruction(nextPC);
                        }
                    }
                    Invoke(new breakpointSetter(BreakpointReached), new object[] { nextPC });
                }
            }

            // Print the next instruction on lastLine
            if (!UpdateTraceTimer.Enabled && line == null)
            {
                int pc = kernel.CPU.PC;
                line = GetExecutionInstruction(pc);
                if (line == null)
                {
                    GenerateNextInstruction(pc);
                }
            }
                    
        }

        private delegate void lastLineDelegate(string line);
        private void ShowLastLine(string line)
        {
            lastLine.Text = line;
        }

        private DebugLine GetExecutionInstruction(int PC)
        {
            if (kernel.lstFile != null)
            {
                DebugLine dl = codeList.Find(x => x.PC == PC);
                return dl;
            }
            else
            {
                return null;
            }

        }
        private void GenerateNextInstruction(int pc)
        {
            OpCode oc = kernel.CPU.PreFetch();
            if (oc != null)
            {
                int ocLength = oc.Length;
                byte[] command = new byte[ocLength];
                for (int i = 0; i < ocLength; i++)
                {
                    command[i] = kernel.MemMgr.RAM.ReadByte(pc + i);
                }
                string opcodes = oc.ToString(kernel.CPU.ReadSignature(ocLength, pc));
                //string status = "";
                DebugLine line = new DebugLine(pc);
                line.SetOpcodes(command);
                line.SetMnemonic(opcodes);
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
                // find the proper place to insert the line, based on the PC
                int index = 0;
                bool lineAdded = false;
                for (index = 0; index < codeList.Count; index++)
                {
                    DebugLine l = codeList[index];
                    if (l.PC > pc)
                    {
                        codeList.Insert(index, line);
                        lineAdded = true;
                        break;
                    }
                }
                if (!lineAdded)
                {
                    codeList.Add(line);
                }
            }
        }

        private void UpdateDebugLines(int newDebugLine, bool state)
        {
            BPCombo.BeginUpdate();
            BPCombo.Items.Clear();
            foreach (KeyValuePair<int,string> bp in knl_breakpoints)
            {
                BPCombo.Items.Add(bp.Value);
            }
            BPCombo.EndUpdate();
            DebugPanel.Refresh();
        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            int pc = knl_breakpoints.GetIntFromHex(locationInput.Text);
            kernel.CPU.PC = pc;
            ClearTrace();
            DebugLine line = GetExecutionInstruction(pc);
            if (line == null)
            {
                GenerateNextInstruction(pc);
            }
            Invalidate();
        }

        private void ClearTraceButton_Click(object sender, EventArgs e)
        {
            ClearTrace();
        }

        public void ClearTrace()
        {
            StepCounter = 0;
            IRQPC = 0;
            kernel.CPU.Stack.TopOfStack = kernel.CPU.Flags.Emulation ? CPU.DefaultStackValueEmulation : CPU.DefaultStackValueNative;
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
            kernel.CPU.CPUThread?.Join();
        }

        private void CPUWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    RunButton.Focus();
                    RunButton_Click(sender, null);
                    break;

                case Keys.F6:
                    StepButton.Focus();
                    StepButton_Click(sender, null);
                    break;

                case Keys.F7:
                    StepOverButton.Focus();
                    StepOverButton_Click(sender, null);
                    break;
            }
        }

        /// <summary>
        /// 
        /// When checked, we receive interrupts.  When unchecked, all interrupt boxes are hidden and disabled.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BreakOnIRQCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool visible = BreakOnIRQCheckBox.Checked;
            SOFCheckbox.Visible = visible;
            SOLCheckbox.Visible = visible;
            TMR0Checkbox.Visible = visible;
            TMR1Checkbox.Visible = visible;
            TMR2Checkbox.Visible = visible;
            RTCCheckbox.Visible = visible;
            FDCCheckbox.Visible = visible;
            MouseCheckbox.Visible = visible;

            KeyboardCheckBox.Visible = visible;
            COM2Checkbox.Visible = visible;
            COM1Checkbox.Visible = visible;
            MPU401Checkbox.Visible = visible;
            SDCardCheckBox.Visible = visible;

            OPL2LCheckbox.Visible = visible;
            OPL2RCheckbox.Visible = visible;
        }

        /// <summary>
        /// Determine if the objects in IRQ Registers match on of the checkboxes.
        /// </summary>
        /// <returns></returns>
        private bool InterruptMatchesCheckboxes()
        {
            // Read Interrupt Register 0
            byte reg0 = kernel.MemMgr.INTERRUPT.ReadByte(0);
            bool result = false;
            if (SOFCheckbox.Checked && (reg0 & (byte)Register0.FNX0_INT00_SOF) != 0)
            {
                SOFCheckbox.IsActive = true;
                result = true;
            } 
            else
            {
                if (SOFCheckbox.IsActive) SOFCheckbox.IsActive = false;
            }
            if (SOLCheckbox.Checked && (reg0 & (byte)Register0.FNX0_INT01_SOL) != 0)
            {
                SOLCheckbox.IsActive = true;
                result = true;
            }
            else
            {
                if (SOLCheckbox.IsActive) SOLCheckbox.IsActive = false;
            }
            if (TMR0Checkbox.Checked && (reg0 & (byte)Register0.FNX0_INT02_TMR0) != 0)
            {
                TMR0Checkbox.IsActive = true;
                result = true;
            }
            else
            {
                if (TMR0Checkbox.IsActive) TMR0Checkbox.IsActive = false;
            }

            if (TMR1Checkbox.Checked && (reg0 & (byte)Register0.FNX0_INT03_TMR1) != 0)
            {
                TMR1Checkbox.IsActive = true;
                result = true;
            }
            else
            {
                if (TMR1Checkbox.IsActive)
                    TMR1Checkbox.IsActive = false;
            }
            if (TMR2Checkbox.Checked && (reg0 & (byte)Register0.FNX0_INT04_TMR2) != 0)
            {
                TMR2Checkbox.IsActive = true;
                result = true;
            }
            else
            {
                if (TMR2Checkbox.IsActive)
                    TMR2Checkbox.IsActive = false;
            }
            if (MouseCheckbox.Checked && (reg0 & (byte)Register0.FNX0_INT07_MOUSE) != 0)
            {
                MouseCheckbox.IsActive = true;
                result = true;
            }
            else
            {
                if (MouseCheckbox.IsActive)
                    MouseCheckbox.IsActive = false;
            }

            // Read Interrupt Register 1
            byte reg1 = kernel.MemMgr.INTERRUPT.ReadByte(1);
            if (SDCardCheckBox.Checked && (reg1 & (byte)Register1.FNX1_INT07_SDCARD ) != 0)
            {
                SDCardCheckBox.IsActive = true;
                result = true;
            }
            else
            {
                if (SDCardCheckBox.IsActive)
                    SDCardCheckBox.IsActive = false;
            }
            if (KeyboardCheckBox.Checked && (reg1 & (byte)Register1.FNX1_INT00_KBD) != 0)
            {
                KeyboardCheckBox.IsActive = true;
                result = true;
            }
            else
            {
                if (KeyboardCheckBox.IsActive)
                    KeyboardCheckBox.IsActive = false;
            }
            if (kernel.GetVersion() != BoardVersion.RevJr)
            {
                //Read Interrupt Register 2
                byte reg2 = kernel.MemMgr.INTERRUPT.ReadByte(2);
                //Read Interrupt Register 3
                byte reg3 = kernel.MemMgr.INTERRUPT.ReadByte(3);
            }
            return result;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            MainWindow.Instance.RestartMenuItemClick(sender, e);
        }

        private void WatchButton_Click(object sender, EventArgs e)
        {
            MainWindow.Instance.WatchListToolStripMenuItem_Click(sender, e);
        }

        private void DebugPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (ActiveLine[0] != 0 && kernel.lstFile.Lines.ContainsKey(ActiveLine[0]))
            {
                DebugLine line = kernel.lstFile.Lines[ActiveLine[0]];
                if (line != null)
                {
                    string name = line.GetAddressName();
                    int address = line.GetAddress();
                    WatchedMemory mem = new WatchedMemory(name, address, 0, 0, 0);
                    if (kernel.WatchList.ContainsKey(name))
                    {
                        kernel.WatchList.Remove(name);
                    }
                    kernel.WatchList.Add(name, mem);
                    MainWindow.Instance.WatchListToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void IRQCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is ColorCheckBox)
            {
                ColorCheckBox ccb = (ColorCheckBox)sender;
                ccb.IsActive = false;
            }
        }
    }
}
