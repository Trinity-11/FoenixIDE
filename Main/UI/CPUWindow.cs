using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FoenixIDE.Processor;
using FoenixIDE.Simulator.Controls;
using FoenixIDE.Simulator.Devices;
using FoenixIDE.Simulator.FileFormat;
using FoenixIDE.Simulator.UI;
using Microsoft.VisualBasic;
using System.Text;
using System.IO;

namespace FoenixIDE.UI
{
    public partial class CPUWindow : Form
    {
        private bool isStepOver = false;
        private const int LABEL_WIDTH = 100;

        private List<int> knl_breakpointsExec = new List<int>();
        private List<int> knl_breakpointsRead = new List<int>();
        private List<int> knl_breakpointsWrite = new List<int>();
        private BreakpointWindow breakpointWindow = new BreakpointWindow();
        private List<DebugLine> codeList = null;
        private SortedList<string, int> DbgLabels = new SortedList<string, int>();

        public static CPUWindow Instance = null;
        private FoenixSystem kernel = null;
        private int[] ActiveLine = {0, 0, 0};  // PC, startofline, width - the point of this is to underline the ADDRESS name
        

        const int ROW_HEIGHT = 13;
        private int IRQPC = -1; // we only keep track of a single interrupt
        private int TopLineIndex = 0; // this is to help us track which line is the current one being executed

        Point position = new Point();
        private int MemoryLimit = 0;
        // Depending on the board
        private BoardVersion boardVersion;
        private string lastLoadedKernel;

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
            DisableIRQs(true);
            registerDisplay1.RegistersReadOnly(false);
            breakpointWindow.DeleteEvent += DeleteEventHandler;
        }

        /**
         * Change the board version.
         * The interrupts for F256s are different than C256s.
         */
        public void SetBoardVersion(BoardVersion version)
        {
            boardVersion = version;
            DisplayInterruptTooltips();
            BreakOnIRQCheckBox_CheckedChanged(null, null);
        }

        public void SetKernel(FoenixSystem kernel)
        {
            this.kernel = kernel;
            MemoryLimit = kernel.MemMgr.RAM.Length;
            registerDisplay1.CPU = kernel.CPU;

            UpdateQueue();
            int pc = kernel.CPU.PC;
            DebugLine line = GetExecutionInstruction(pc);
            if (line == null)
            {
                GenerateNextInstruction(pc);
            }
            DebugPanel.Refresh();
        }

        private void DeleteEventHandler()
        {
            DebugPanel.Invalidate();
        }

        // Called when resetting the CPU
        // Need to re-read the DbgLabels because we are reloading the listing from kernel
        private void UpdateQueue()
        {
            string newkernelname = kernel.GetKernelName();

            if (!newkernelname.Equals(lastLoadedKernel) || codeList == null || (kernel.lstFile.Lines.Count>0 && codeList.Count != kernel.lstFile.Lines.Count))
            {
                if (kernel.lstFile != null && kernel.lstFile.Lines.Count > 0)
                {
                    codeList = new List<DebugLine>(kernel.lstFile.Lines.Count);
                    foreach (DebugLine line in kernel.lstFile.Lines.Values)
                    {
                        codeList.Add(line);
                        if (line.label != null)
                        {
                            if (!DbgLabels.ContainsKey(line.label))
                            {
                                DbgLabels.Add(line.label, line.PC);
                            }
                            else
                            {
                                // Check for duplicate labels
                                for (int i = 1; i < 100; i++)
                                {
                                    if (!DbgLabels.ContainsKey(line.label + "#" + i))
                                    {
                                        DbgLabels.Add(line.label + "#" + i, line.PC);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    codeList = new List<DebugLine>(DebugPanel.Height / ROW_HEIGHT);
                    GenerateNextInstruction(kernel.CPU.PC);
                }
                lastLoadedKernel = newkernelname;
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

            DebugPanel.Paint += new System.Windows.Forms.PaintEventHandler(DebugPanel_Paint);
            DisplayInterruptTooltips();
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
                List<int> bkpts = breakpointWindow.GetExecuteBreakpoints();

                // Draw the position box
                if (position.X > 0 && position.Y > 0)
                {
                    int row = position.Y / ROW_HEIGHT;
                    int col = 12;
                    e.Graphics.FillRectangle(Brushes.LightBlue, col, row * ROW_HEIGHT, 7 * 6, 14);
                }

                bool offsetPrinted = false;
                int previousPC = -1;
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
                                        if (bkpts != null && bkpts.Contains(q0.PC))
                                        {
                                            e.Graphics.DrawEllipse(Pens.White, LABEL_WIDTH - ROW_HEIGHT - 1, painted * ROW_HEIGHT, ROW_HEIGHT+1, ROW_HEIGHT+1);
                                            e.Graphics.FillEllipse(Brushes.DarkRed, LABEL_WIDTH - ROW_HEIGHT, painted * ROW_HEIGHT + 1, ROW_HEIGHT, ROW_HEIGHT);
                                        }
                                        // Check if the memory still matches the opcodes
                                        if (!q0.CheckOpcodes(kernel.MemMgr))
                                        {
                                            e.Graphics.FillRectangle(Brushes.Red, LABEL_WIDTH + 3, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                                        }
                                        e.Graphics.DrawString(q0.ToString(), HeaderTextbox.Font, Brushes.Black, LABEL_WIDTH + 2, painted * ROW_HEIGHT);
                                        if (q0.PC == ActiveLine[0])
                                        {
                                            e.Graphics.DrawLine(Pens.Black, LABEL_WIDTH + ActiveLine[1], (painted + 1) * ROW_HEIGHT, LABEL_WIDTH + ActiveLine[1] + ActiveLine[2], (painted + 1) * ROW_HEIGHT);
                                        }
                                        // If the source code listing has a gap, draw a dashed line
                                        if (previousPC != -1 && (previousPC  != q0.PC))
                                        {
                                            Pen dashGreenPen = new Pen(Color.DarkOliveGreen, 2);
                                            dashGreenPen.DashPattern = new float[]{5,2,15,4};
                                            e.Graphics.DrawLine(dashGreenPen, 0, painted * ROW_HEIGHT, this.Width, painted * ROW_HEIGHT);
                                        }
                                        previousPC = q0.PC + q0.commandLength;
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
                            if (bkpts != null && bkpts.Contains(line.PC))
                            {
                                e.Graphics.DrawEllipse(Pens.White, LABEL_WIDTH - ROW_HEIGHT - 1, painted * ROW_HEIGHT, ROW_HEIGHT + 1, ROW_HEIGHT + 1);
                                e.Graphics.FillEllipse(Brushes.DarkRed, LABEL_WIDTH - ROW_HEIGHT, painted * ROW_HEIGHT + 1, ROW_HEIGHT, ROW_HEIGHT);
                            }
                            if (line.PC == IRQPC)
                            {
                                e.Graphics.FillRectangle(Brushes.Orange, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            // Check if the memory still matches the opcodes
                            if (!line.CheckOpcodes(kernel.MemMgr))
                            {
                                e.Graphics.FillRectangle(Brushes.Red, 0, painted * ROW_HEIGHT, this.Width, ROW_HEIGHT);
                            }
                            e.Graphics.DrawString(line.ToString(), HeaderTextbox.Font, Brushes.Black, 102, painted * ROW_HEIGHT);
                            if (line.PC == ActiveLine[0])
                            {
                                e.Graphics.DrawLine(Pens.Black, LABEL_WIDTH + ActiveLine[1], (painted + 1) * ROW_HEIGHT, LABEL_WIDTH + ActiveLine[1] + ActiveLine[2], (painted + 1) * ROW_HEIGHT);
                            }
                            // If the source code listing has a gap, draw a dashed line
                            if (previousPC != -1 && (previousPC != line.PC))
                            {
                                Pen dashGreenPen = new Pen(Color.DarkOliveGreen, 2);
                                dashGreenPen.DashPattern = new float[] { 5, 2, 15, 4 };
                                e.Graphics.DrawLine(dashGreenPen, 0, painted * ROW_HEIGHT, this.Width, painted * ROW_HEIGHT);
                            }
                            previousPC = line.PC + line.commandLength;
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

        private void DisplayInterruptTooltips()
        {
            bool isF256 = BoardVersionHelpers.IsF256(boardVersion);
            if (isF256)
            {
                // this is going to be confusing - the F256 Interrupts are different
                // Register 0
                Tooltip.SetToolTip(SOFCheckbox, "Break on SOF Interrupts");
                Tooltip.SetToolTip(SOLCheckbox, "Break on SOL Interrupts");
                Tooltip.SetToolTip(TMR0Checkbox, "Break on Keyboard Interrupts");
                Tooltip.SetToolTip(TMR1Checkbox, "Break on Mouse Interrupts");
                Tooltip.SetToolTip(TMR2Checkbox, "Break on Timer0 Interrupts");
                Tooltip.SetToolTip(RTCCheckbox, "Break on Timer1 Interrupts");
                Tooltip.SetToolTip(FDCCheckbox, "Reserved");
                Tooltip.SetToolTip(MouseCheckbox, "Break on Cartridge Inserted Interrupts");
                FDCCheckbox.Visible = false;

                // Register 1
                Tooltip.SetToolTip(KeyboardCheckBox, "Break on UART Interrupts");
                Tooltip.SetToolTip(V2SprColCheck, "Reserved");
                Tooltip.SetToolTip(V2BitColCheck, "Reserved");
                Tooltip.SetToolTip(COM2Checkbox, "Reserved");
                Tooltip.SetToolTip(COM1Checkbox, "Break on RTC Interrupts");
                Tooltip.SetToolTip(MPU401Checkbox, "Break on VIA 0 Interrupts");
                Tooltip.SetToolTip(ParallelPortCheck, "Break on VIA 1 Interrupts");
                Tooltip.SetToolTip(SDCardCheckBox, "Break on SD Card Interrupts");
                V2SprColCheck.Visible = false;
                V2BitColCheck.Visible = false;
                COM2Checkbox.Visible = false;

                // Register 2
                Tooltip.SetToolTip(OPL3Checkbox, "Break on IEC Data In Interrupts");
                Tooltip.SetToolTip(GabeInt0Check, "Break on IEC Clock In Interrupts");
                Tooltip.SetToolTip(GabeInt1Check, "Break on IEC ATN In Interrupts");
                Tooltip.SetToolTip(VDMACheck, "Break on IEC SEQ In Interrupts");
                Tooltip.SetToolTip(V2TileColCheck, "Reserved");
                Tooltip.SetToolTip(GabeInt2Check, "Reserved");
                Tooltip.SetToolTip(ExtExpCheck, "Reserved");
                Tooltip.SetToolTip(SDCardInsertCheck, "Reserved");
                V2TileColCheck.Visible = false;
                GabeInt2Check.Visible = false;
                ExtExpCheck.Visible = false;
                SDCardInsertCheck.Visible = false;
            }
            else
            {
                // Register 0
                Tooltip.SetToolTip(SOFCheckbox, "Break on SOF Interrupts");
                Tooltip.SetToolTip(SOLCheckbox, "Break on SOL Interrupts");
                Tooltip.SetToolTip(TMR0Checkbox, "Break on TMR0 Interrupts");
                Tooltip.SetToolTip(TMR1Checkbox, "Break on TMR1 Interrupts");
                Tooltip.SetToolTip(TMR2Checkbox, "Break on TMR2 Interrupts");
                Tooltip.SetToolTip(RTCCheckbox, "Break on RTC Interrupts");
                Tooltip.SetToolTip(FDCCheckbox, "Break on FDC Interrupts");
                Tooltip.SetToolTip(MouseCheckbox, "Break on Mouse Interrupts");
                FDCCheckbox.Visible = true;

                // Register 1
                Tooltip.SetToolTip(KeyboardCheckBox, "Break on Keyboard Interrupts");
                Tooltip.SetToolTip(V2SprColCheck, "Break on Sprite Collision Interrupts");
                Tooltip.SetToolTip(V2BitColCheck, "Break on Bitmap Collision Interrupts");
                Tooltip.SetToolTip(COM2Checkbox, "Break on COM2 Interrupts");
                Tooltip.SetToolTip(COM1Checkbox, "Break on COM1 Interrupts");
                Tooltip.SetToolTip(MPU401Checkbox, "Break on MIDI Ctrlr Interrupts");
                Tooltip.SetToolTip(ParallelPortCheck, "Break on Parallel Interrupts");
                Tooltip.SetToolTip(SDCardCheckBox, "Break on SD Card Interrupts");
                V2SprColCheck.Visible = true;
                V2BitColCheck.Visible = true;
                COM2Checkbox.Visible = true;

                // Register 2
                Tooltip.SetToolTip(OPL3Checkbox, "Break on OPL3 Interrupts");
                Tooltip.SetToolTip(GabeInt0Check, "Break on Gabe INT0 Interrupts");
                Tooltip.SetToolTip(GabeInt1Check, "Break on Gabe INT1 Interrupts");
                Tooltip.SetToolTip(VDMACheck, "Break on VDMA Interrupts");
                Tooltip.SetToolTip(V2TileColCheck, "Break on Tile Collision Interrupts");
                Tooltip.SetToolTip(GabeInt2Check, "Break on Gabe INT2 Interrupts");
                Tooltip.SetToolTip(ExtExpCheck, "Break on External Expansion Interrupts");
                Tooltip.SetToolTip(SDCardInsertCheck, "Break on SDCard Insertion Interrupts");
                V2TileColCheck.Visible = true;
                GabeInt2Check.Visible = true;
                ExtExpCheck.Visible = true;
                SDCardInsertCheck.Visible = true;
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
                    breakpointWindow.AddBreakpoint(line.PC);
                    DebugPanel.Invalidate();
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
                    breakpointWindow.DeleteBreakpoint(line.PC);
                    knl_breakpointsExec.Remove(line.PC);
                    knl_breakpointsRead.Remove(line.PC);
                    knl_breakpointsWrite.Remove(line.PC);
                    DebugPanel.Invalidate();
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
                    InputDialog labelDialog = new InputDialog(
                        "Enter Label for Address: $" + line.PC.ToString("X6").Insert(2, ":"),
                        "Label Dialog",
                        oldValue,
                        Left + LabelOverlayButton.Left + LabelOverlayButton.Width,
                        Top + LabelOverlayButton.Top
                    );
                    DialogResult result = labelDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string label = labelDialog.GetValue();
                        
                        AddLabel(label, line);

                        DebugPanel.Invalidate();
                    }
                }
            }
        }

        private bool AddLabel(string label, DebugLine line)
        {
            if (DbgLabels.ContainsKey(label))
            {
                bool newlabelAssigned = false;
                for (int i = 1; i < 100; i++)
                {
                    if (!DbgLabels.ContainsKey(label + "#" + i))
                    {
                        label = label + "#" + i;
                        newlabelAssigned = true;
                        break;
                    }
                }
                if (!newlabelAssigned)
                {
                    return false;
                }    
            }
            
            DbgLabels.Add(label, line.PC);
            line.SetLabel(label);

            // Replace matching addresses in JMP/JML/JSR/JSL with this label
            foreach (DebugLine srcLine in codeList)
            {
                string opcodes = srcLine.GetOpcodes();
                string[] codes = opcodes.Split(',');
                if (codes[0].Equals("4C") || codes[0].Equals("5C") || codes[0].Equals("20") || codes[0].Equals("22"))
                {
                    if (codes.Length >= 3)
                    {
                        int address = Convert.ToInt32(codes[2] + codes[1], 16);
                        if (address == line.PC)
                        {
                            srcLine.SetSource(srcLine.GetSource().Replace("$" + address.ToString("X4"), label));
                        }
                    }
                }
            }
            return true;
        }
        private void InspectButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                if (TopLineIndex + row <= codeList.Count)
                {
                    DebugLine line = codeList[TopLineIndex + row];
                    MemoryWindow.Instance.GotoAddress(line.PC & 0xFF_FF00);
                }
                MemoryWindow.Instance.BringToFront();
            }
        }

        public void RunButton_Click(object sender, EventArgs e)
        {
            DebugPanel_Leave(sender, e);
            if (RunButton.Tag.Equals("0"))
            {
                // Clear the interrupt
                if (IRQPC != -1 && !kernel.CPU.Flags.IrqDisable)
                {
                    ResetInterrupts();
                }
                TurnOffActiveInterrupts();
                //if (BreakOnIRQCheckBox.Checked)
                //{
                //    InterruptMatchesCheckboxes();
                //}
                registerDisplay1.RegistersReadOnly(true);
                MainWindow.Instance.ResetGPU(true);
                kernel.CPU.DebugPause = false;
                lastLine.Text = "";
                kernel.CPU.CPUThread = new Thread(new ThreadStart(ThreadProc));
                UpdateTraceTimer.Enabled = true;
                if (sender != null)
                {
                    knl_breakpointsExec.Clear();
                }
                addBreakpoints();
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
            MainWindow.Instance.ResetGPU(false);
        }

        private void addBreakpoints()
        {
            List<int> execs = breakpointWindow.GetExecuteBreakpoints();
            foreach (int exec in execs)
            {
                if (!knl_breakpointsExec.Contains(exec))
                {
                    knl_breakpointsExec.Add(exec);
                }
            }
            List<int> reads = breakpointWindow.GetReadBreakpoints();
            foreach (int read in reads)
            {
                if (!knl_breakpointsRead.Contains(read))
                {
                    knl_breakpointsRead.Add(read);
                }
            }
            List<int> writes = breakpointWindow.GetWriteBreakpoints();
            foreach (int write in writes)
            {
                if (!knl_breakpointsWrite.Contains(write))
                {
                    knl_breakpointsWrite.Add(write);
                }
            }
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
            MainWindow.Instance.ResetGPU(false);
            kernel.CPU.DebugPause = true;
        }

        private void StepOverOverlayButton_Click(object sender, EventArgs e)
        {
            if (position.X > 0 && position.Y > 0)
            {
                int row = position.Y / ROW_HEIGHT;
                DebugLine line = codeList[TopLineIndex + row];
                knl_breakpointsExec.Clear();
                // Set a breakpoint to the next address
                knl_breakpointsExec.Add(line.PC);

                // Run the CPU until the breakpoint is reached
                RunButton_Click(null, null);

                // Ensure the breakpoint is removed
                isStepOver = true;
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
                knl_breakpointsExec.Clear();
                // Set a breakpoint to the next address
                knl_breakpointsExec.Add(pc + line.commandLength);

                // Run the CPU until the breakpoint is reached
                RunButton_Click(null, null);

                // Ensure the breakpoint is removed
                isStepOver = true;
                RunButton.Text = "Run (F5)";
                RunButton.Tag = "0";
                registerDisplay1.RegistersReadOnly(false);
                MainWindow.Instance.ResetGPU(false);
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
                DebugPanel.Invalidate();
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
                    stackText.AppendText(address.ToString("X4") + " " + kernel.CPU.MemMgr.ReadByte(address).ToString("X2") + "\r\n");
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
                knl_breakpointsExec.Remove(pc);
            }
            else
            {
                lastLine.Text = "Breakpoint reached $" + pc.ToString("X6");
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
            DebugLine line = null;
            int previousPC = kernel.CPU.PC;

            if (!kernel.CPU.ExecuteNext())
            {

                int nextPC = kernel.CPU.PC;
                int effAddr = kernel.CPU.effectiveAddress;
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
                if (
                     knl_breakpointsRead.Contains(effAddr) ||
                     knl_breakpointsWrite.Contains(effAddr))
                {
                    if (UpdateTraceTimer.Enabled)
                    {
                        UpdateTraceTimer.Enabled = false;
                        kernel.CPU.DebugPause = true;
                    }
                    Invoke(new breakpointSetter(BreakpointReached), new object[] { effAddr });
                }
                if ( knl_breakpointsExec.Contains(nextPC) || 
                     kernel.CPU.CurrentOpcode.Value == 0 ||
                     ( BreakOnIRQCheckBox.Checked && (kernel.CPU.Pins.GetInterruptPinActive && InterruptMatchesCheckboxes()))
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
                    if (kernel.CPU.Pins.GetInterruptPinActive)
                    {
                        IRQPC = kernel.CPU.PC;
                        kernel.CPU.ExecuteNext();
                        kernel.CPU.Pins.IRQ = false;
                        nextPC = kernel.CPU.PC;
                        UpdateInterruptCheckboxes();
                        Invoke(new nullParamMethod(Refresh));
                    }
                    if (line == null)
                    {
                        line = GetExecutionInstruction(nextPC);
                        if (line == null)
                        {
                            GenerateNextInstruction(nextPC);
                        }
                    }
                    Invoke(new breakpointSetter(BreakpointReached), new object[] { IRQPC });
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
            if (codeList.Count > 0)
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
                    command[i] = kernel.MemMgr.ReadByte(pc + i);
                }
                string opcodes = oc.ToString(kernel.CPU.ReadSignature(ocLength, pc));
                //string status = "";
                DebugLine line = new DebugLine(pc);
                line.SetOpcodes(command);
                line.SetMnemonic(opcodes);
                
                // Check if there's a matching label in the DbgLabels list
                if (command[0] == 0x20 || command[0] == 0x4c || command[0] == 0x5c || command[0] == 0x22)
                {
                    int address = command[2] << 8 | command[1];
                    if (DbgLabels.ContainsValue(address))
                    {
                        string dLabel = DbgLabels.Keys[DbgLabels.IndexOfValue(address)];
                        line.SetSource(line.GetSource().Replace("$" + address.ToString("X4"), dLabel));
                    }
                }
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

        public void JumpButton_Click(object sender, EventArgs e)
        {
            int pc = FoenixSystem.TextAddressToInt(locationInput.Text);
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
            ResetInterrupts();
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
            if (!visible)
            {
                ResetInterrupts();
            }
            bool isF256 = BoardVersionHelpers.IsF256(boardVersion);

            // Row 1
            SOFCheckbox.Visible = visible;
            SOLCheckbox.Visible = visible;
            TMR0Checkbox.Visible = visible;
            TMR1Checkbox.Visible = visible;
            TMR2Checkbox.Visible = visible;
            RTCCheckbox.Visible = visible;
            FDCCheckbox.Visible = !isF256 && visible;
            MouseCheckbox.Visible = visible;

            // Row 2
            KeyboardCheckBox.Visible = visible;
            V2SprColCheck.Visible = !isF256 && visible;
            V2BitColCheck.Visible = !isF256 && visible;
            COM2Checkbox.Visible = !isF256 && visible;
            COM1Checkbox.Visible = visible;
            MPU401Checkbox.Visible = visible;
            ParallelPortCheck.Visible = visible;
            SDCardCheckBox.Visible = visible;

            // Row 3
            OPL3Checkbox.Visible = visible;
            GabeInt0Check.Visible = visible;
            GabeInt1Check.Visible = visible;
            VDMACheck.Visible = visible;
            V2TileColCheck.Visible = !isF256 && visible;
            GabeInt2Check.Visible = !isF256 && visible;
            ExtExpCheck.Visible = !isF256 && visible;
            SDCardInsertCheck.Visible = !isF256 && visible;
        }

        /// <summary>
        /// If an interrupt is fired, highlight the corresponding checkbox.
        /// 
        /// </summary>
        /// <returns></returns>
        private void UpdateInterruptCheckboxes()
        {
            // Read Interrupt Register 0
            byte reg0 = kernel.MemMgr.INTERRUPT.ReadByte(0);
            ColorCheckBox[] row1 = { SOFCheckbox, SOLCheckbox, TMR0Checkbox, TMR1Checkbox, TMR2Checkbox, RTCCheckbox, FDCCheckbox, MouseCheckbox };
            for (int i = 0; i < 8; i++)
            {
                row1[i].IsActive = (row1[i].Checked && (reg0 & 1 << i) != 0);
            }

            // Read Interrupt Register 1
            byte reg1 = kernel.MemMgr.INTERRUPT.ReadByte(1);
            ColorCheckBox[] row2 = { KeyboardCheckBox, V2SprColCheck, V2BitColCheck, COM2Checkbox, COM1Checkbox, MPU401Checkbox, ParallelPortCheck, SDCardCheckBox };
            for (int i = 0; i < 8; i++)
            {
                row2[i].IsActive = (row2[i].Checked && (reg1 & 1 << i) != 0);
                
            }

            //Read Interrupt Register 2 - we don't handle these yet
            byte reg2 = kernel.MemMgr.INTERRUPT.ReadByte(2);
            ColorCheckBox[] row3 = { OPL3Checkbox, GabeInt0Check, GabeInt1Check, VDMACheck, V2TileColCheck, GabeInt2Check, ExtExpCheck, SDCardInsertCheck };
            for (int i = 0; i < 8; i++)
            {
                row3[i].IsActive = (row3[i].Checked && (reg1 & 1 << i) != 0);
            }
        }
        /// <summary>
        /// Determine if the objects in IRQ Registers match on of the checkboxes.
        /// </summary>
        /// <returns></returns>
        private bool InterruptMatchesCheckboxes()
        {
            // Read Interrupt Register 0
            byte reg0 = kernel.MemMgr.INTERRUPT.ReadByte(0);
            ColorCheckBox[] row1 = { SOFCheckbox, SOLCheckbox, TMR0Checkbox, TMR1Checkbox, TMR2Checkbox, RTCCheckbox, FDCCheckbox, MouseCheckbox };
            for (int i =0; i<8;i++)
            {
                if (row1[i].Checked && (reg0 & 1 << i) != 0)
                {
                    return true;
                }
            }

            // Read Interrupt Register 1
            byte reg1 = kernel.MemMgr.INTERRUPT.ReadByte(1);
            ColorCheckBox[] row2 = { KeyboardCheckBox, V2SprColCheck, V2BitColCheck, COM2Checkbox, COM1Checkbox, MPU401Checkbox, ParallelPortCheck, SDCardCheckBox };
            for (int i = 0; i < 8; i++)
            {
                if (row2[i].Checked && (reg1 & 1 << i) != 0)
                {
                    return true;
                }
            }

            //Read Interrupt Register 2 - we don't handle these yet
            byte reg2 = kernel.MemMgr.INTERRUPT.ReadByte(2);
            ColorCheckBox[] row3 = { OPL3Checkbox, GabeInt0Check, GabeInt1Check, VDMACheck, V2TileColCheck, GabeInt2Check, ExtExpCheck, SDCardInsertCheck };
            for (int i = 0; i < 8; i++)
            {
                if (row3[i].Checked && (reg1 & 1 << i) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void ResetInterrupts()
        {
            IRQPC = -1;
            if (kernel != null)
            {
                kernel.MemMgr.INTERRUPT.WriteFromGabe(0, 0);
                kernel.MemMgr.INTERRUPT.WriteFromGabe(1, 0);
                kernel.MemMgr.INTERRUPT.WriteFromGabe(2, 0);
                if (!BoardVersionHelpers.IsF256(kernel.GetVersion()))
                {
                    kernel.MemMgr.INTERRUPT.WriteFromGabe(3, 0);
                }
            }
        }

        private void TurnOffActiveInterrupts()
        {
            // turn off the checkboxes
            // Row 1
            SOFCheckbox.IsActive = false;
            SOLCheckbox.IsActive = false;
            TMR0Checkbox.IsActive = false;
            TMR1Checkbox.IsActive = false;
            TMR2Checkbox.IsActive = false;
            RTCCheckbox.IsActive = false;
            FDCCheckbox.IsActive = false;
            MouseCheckbox.IsActive = false;

            // Row 2
            KeyboardCheckBox.IsActive = false;
            V2SprColCheck.IsActive = false;
            V2BitColCheck.IsActive = false;
            COM2Checkbox.IsActive = false;
            COM1Checkbox.IsActive = false;
            MPU401Checkbox.IsActive = false;
            ParallelPortCheck.IsActive = false;
            SDCardCheckBox.IsActive = false;

            // Row 3
            OPL3Checkbox.IsActive = false;
            GabeInt0Check.IsActive = false;
            GabeInt1Check.IsActive = false;
            VDMACheck.IsActive = false;
            V2TileColCheck.IsActive = false;
            GabeInt2Check.IsActive = false;
            ExtExpCheck.IsActive = false;
            SDCardInsertCheck.IsActive = false;
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
            // TODO: remove the reference to kernel.lstFile and replace with codeList.
            DebugLine line = SourceCodeListingFind(ActiveLine[0]);
            if (e.Button == MouseButtons.Left && ActiveLine[0] != 0 && line != null)
            {
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
            else if (e.Button == MouseButtons.Right && kernel.CPU.DebugPause)
            {
                Point contextMenuLocation = DebugPanel.PointToScreen(new Point(e.X, e.Y));
                debugWindowContextMenuStrip.Show(contextMenuLocation);
            }
        }

        private DebugLine SourceCodeListingFind(int pc)
        {
            foreach (DebugLine line in codeList)
            {
                if (line.PC == pc)
                {
                    return line;
                }
            }
            return null;
        }
        private void IRQCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is ColorCheckBox ccb)
            {
                ccb.IsActive = false;
            }
        }

        private void CenterForm(Form form)
        {
            int left = this.Left + (this.Width - form.Width) / 2;
            int top = this.Top + (this.Height - form.Height) / 2;
            form.Location = new Point(left, top);
        }

        private void BreakpointButton_Click(object sender, EventArgs e)
        {
            if (!breakpointWindow.Visible)
            {
                CenterForm(breakpointWindow);
                breakpointWindow.Show();
            }
            else
            {
                breakpointWindow.BringToFront();
            }
        }

        // This is called when new code is loaded in the Main Window.
        public void ClearSourceListing()
        {
            DbgLabels.Clear();
            codeList.Clear();
        }
        private void DebugWindowCopyToClipboardMenuItem_Click(object sender, EventArgs e)
        {
            // This function follows the same logic as DebugPanel_Paint.
            StringBuilder clipboardText = new StringBuilder();

            if (codeList != null)
            {
                int startIndex = -1;
                for (int i = 0; i < codeList.Count; ++i)
                {
                    if (codeList[i].PC != kernel.CPU.PC)
                    {
                        continue;
                    }
                    startIndex = i;
                }

                if (startIndex > 4)
                {
                    startIndex -= 5;
                }
                int endIndex = Math.Min(startIndex + 26, codeList.Count);

                for (int i = startIndex; i < endIndex; ++i)
                {
                    DebugLine line = codeList[i];
                    if (line == null) // Line can be null for invalid opcodes
                    {
                        clipboardText.AppendLine("<invalid instruction>");
                    }
                    else
                    {
                        clipboardText.AppendLine(line.ToString());
                    }
                }
            }

            Clipboard.SetText(clipboardText.ToString());
        }

        private void saveSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string srcListingFileName = lastLoadedKernel.Substring(0, lastLoadedKernel.LastIndexOf('.')) + ".LST";
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Title = "Save Source Listing",
                DefaultExt = ".LST",
                FileName = srcListingFileName,
                Filter = "Source Listing|.LST"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter outputFile = new StreamWriter(saveDialog.FileName))
                {
                    foreach (DebugLine line in codeList)
                    {
                        // Format the label or insert spaces so the source is properly aligned
                        // Add underscores (_) for labels that are shorter than 4 characters long.
                        string label = line.label != null 
                            ? ( 
                                line.label.Length > 4
                                ? string.Format("{0,-10}", line.label)
                                : line.label + new string('_', 4-line.label.Length) + new string(' ', 6)
                              )
                            : new string(' ', 10);
                        outputFile.WriteLine("." + line.PC.ToString("X6") + "\t" + line.GetOpcodes().Replace(',',' ').Trim() + "\t\t" + line.GetSource() + "\t\t" + label + line.GetSource());
                    }
                }
            }
        }
    }
}
