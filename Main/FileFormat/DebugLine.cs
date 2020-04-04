using FoenixIDE.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.FileFormat
{
    /// <summary>
    /// Container to hold one line of 65C816 code debugging data
    /// </summary>
    class DebugLine: ICloneable
    {
        //public bool isBreakpoint = false;
        byte[] command;
        public int commandLength;
        private String opcodes;
        readonly int[] cpu;
        public bool StepOver = false;
        public bool isLabel = false;
        private string evaled = null;
        private static byte[] BranchJmpOpcodes = {
            OpcodeList.BCC_ProgramCounterRelative,
            OpcodeList.BCS_ProgramCounterRelative,
            OpcodeList.BEQ_ProgramCounterRelative,
            OpcodeList.BMI_ProgramCounterRelative,
            OpcodeList.BNE_ProgramCounterRelative,
            OpcodeList.BPL_ProgramCounterRelative,
            OpcodeList.BRA_ProgramCounterRelative,
            OpcodeList.BRL_ProgramCounterRelativeLong,
            OpcodeList.BVC_ProgramCounterRelative,
            OpcodeList.BVS_ProgramCounterRelative,
            OpcodeList.JMP_Absolute,
            OpcodeList.JMP_AbsoluteIndexedIndirectWithX,
            OpcodeList.JMP_AbsoluteIndirect,
            OpcodeList.JMP_AbsoluteIndirectLong,
            OpcodeList.JMP_AbsoluteLong,
            OpcodeList.JSR_Absolute,
            OpcodeList.JSR_AbsoluteIndexedIndirectWithX,
            OpcodeList.JSR_AbsoluteLong
        };

        public int ProgCntr;
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
                evaled = string.Format(">{0}  {1} {2}  {3}", ProgCntr.ToString("X6"), c.ToString(), opcodes, null);
            }
            return evaled;
        }

        /// <summary>
        /// One and only constructor 
        /// </summary>
        public DebugLine(int pc, byte[] cmd, String oc, int[] cpuSnapshot)
        {
            ProgCntr = pc;
            command = cmd;
            if (cmd != null)
            {
                commandLength = cmd.Length;
                StepOver = (Array.Exists(BranchJmpOpcodes, element => element == command[0]));
            }
            else
            {
                commandLength = 0;
                isLabel = true;
            }
            oc = oc.Trim(new char[] { ' ' });
            // Detect if the lines contains a label
            if (!isLabel)
            {
                string[] tokens = oc.Split();
                if (tokens.Length > 0)
                {
                    isLabel = (tokens[0].Length > 3);
                }
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
        public bool CheckOpcodes(MemoryLocations.MemoryRAM ram)
        {
            for (int i=0;i<commandLength;i++)
            {
                if (ram.ReadByte(ProgCntr + i) != command[i])
                {
                    return false;
                }
            }
            return true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
