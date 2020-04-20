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
    public class DebugLine: ICloneable
    {
        //public bool isBreakpoint = false;
        public int PC;
        byte[] command;
        public int commandLength = 0;
        private string source;
        public bool StepOver = false;
        public string label;
        private string evaled = null;

        private static readonly byte[] BranchJmpOpcodes = {
            OpcodeList.BCC_ProgramCounterRelative,
            OpcodeList.BCS_ProgramCounterRelative,
            OpcodeList.BEQ_ProgramCounterRelative,
            OpcodeList.BMI_ProgramCounterRelative,
            OpcodeList.BNE_ProgramCounterRelative,
            OpcodeList.BPL_ProgramCounterRelative,
            //OpcodeList.BRA_ProgramCounterRelative,
            //OpcodeList.BRL_ProgramCounterRelativeLong,
            OpcodeList.BVC_ProgramCounterRelative,
            OpcodeList.BVS_ProgramCounterRelative,
            //OpcodeList.JMP_Absolute,
            //OpcodeList.JMP_AbsoluteIndexedIndirectWithX,
            //OpcodeList.JMP_AbsoluteIndirect,
            //OpcodeList.JMP_AbsoluteIndirectLong,
            //OpcodeList.JMP_AbsoluteLong,
            OpcodeList.JSR_Absolute,
            OpcodeList.JSR_AbsoluteIndexedIndirectWithX,
            OpcodeList.JSR_AbsoluteLong
        };

        
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
                evaled = string.Format("{0}  {1} {2}  {3}", PC.ToString("X6"), c.ToString(), source, null);
            }
            return evaled;
        }

        public DebugLine(int pc)
        {
            PC = pc;
        }
        public void SetOpcodes(byte[] cmd)
        {
            commandLength = cmd.Length;
            command = cmd;
            StepOver = (Array.Exists(BranchJmpOpcodes, element => element == command[0]));
        }
        public string GetOpcodes()
        {
            StringBuilder c = new StringBuilder();
            for (int i = 0; i < commandLength; i++)
            {
                c.Append(command[i].ToString("X2")).Append(",");
            }
            return c.ToString();
        }
        public string GetSource()
        {
            return source;
        }
        public void SetLabel(string value)
        {
            label = value;
        }
        public void SetMnemonic(string value)
        {
            value = value.Trim(new char[] { ' ' });
            // Detect if the lines contains a label
            string[] tokens = value.Split();
            if (tokens.Length > 0)
            {
                if (tokens[0].Length > 3)
                {
                    label = tokens[0];
                    // Remove the first item
                    source = value.Substring(label.Length).Trim();
                }
                else
                {
                    source = value;
                }
            }
            else
            {
                source = value;
            }
        }

        public bool CheckOpcodes(MemoryLocations.MemoryRAM ram)
        {
            for (int i=0;i<commandLength;i++)
            {
                if (ram.ReadByte(PC + i) != command[i])
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
