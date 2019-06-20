using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.FileFormat
{
    /// <summary>
    /// Container to hold one line of 65C816 code debugging data
    /// </summary>
    class DebugLine
    {
        public bool isBreakpoint = false;
        byte[] command;
        public int commandLength;
        private String opcodes;
        readonly int[] cpu;
        public bool StepOver = false;
        private string evaled = null;

        public int PC;
        // Only expand when it's going to be displayed
        override public string ToString()
        {
            if (evaled == null)
            {
                StringBuilder c = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    if (i < commandLength)
                        c.Append(command[i].ToString("X2")).Append(" ");
                    else
                        c.Append("   ");
                }
                //String OpCodes = opcodes + new string(' ', 14 - opcodes.Length);
                //String state = FormatSnapshot();
                StepOver = (opcodes.StartsWith("B") || opcodes.StartsWith("J"));
                evaled = string.Format(">{0}  {1} {2}  {3}", PC.ToString("X6"), c.ToString(), opcodes, null);
            }
            return evaled;
        }

        /// <summary>
        /// One and only constructor 
        /// </summary>
        public DebugLine(int pc, byte[] cmd, String oc, int[] cpuSnapshot)
        {
            PC = pc;
            command = cmd;
            if (cmd != null)
            {
                commandLength = cmd.Length;
            }
            else
            {
                commandLength = 0;
            }
            opcodes = oc;
            cpu = cpuSnapshot;
        }

        private String FormatSnapshot()
        {
            if (cpu != null)
            {
                StringBuilder s = new StringBuilder(47);
                s.Append(';')
                 .Append(cpu[0].ToString("X6")).Append(' ')
                 .Append(cpu[1].ToString("X4")).Append(' ')
                 .Append(cpu[2].ToString("X4")).Append(' ')
                 .Append(cpu[3].ToString("X4")).Append(' ')
                 .Append(cpu[4].ToString("X4")).Append(' ')
                 .Append(cpu[5].ToString("X2")).Append(' ').Append(' ')
                 .Append(cpu[6].ToString("X4")).Append(' ');
                Processor.Flags localFlags = new Processor.Flags();
                localFlags.SetFlags(cpu[7]);
                s.Append(localFlags);
                return s.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
