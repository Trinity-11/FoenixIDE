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
        private int A, X, Y;
        private FoenixIDE.Processor.Flags P;
        private bool showRegisterStatus = false;

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

        private static readonly byte[] NonImmediateOpcodes = {
            OpcodeList.LDA_Absolute,
            OpcodeList.LDA_AbsoluteIndexedWithX,
            OpcodeList.LDA_AbsoluteIndexedWithY,
            OpcodeList.LDA_AbsoluteLong,
            OpcodeList.LDA_AbsoluteLongIndexedWithX,
            OpcodeList.LDA_DirectPage,
            OpcodeList.LDA_DirectPageIndirect,

            OpcodeList.STA_Absolute,
            OpcodeList.STA_AbsoluteIndexedWithX,
            OpcodeList.STA_AbsoluteIndexedWithY,
            OpcodeList.STA_AbsoluteLong,
            OpcodeList.STA_AbsoluteLongIndexedWithX,
            OpcodeList.STA_DirectPage,
            OpcodeList.STA_DirectPageIndirect
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
                if (showRegisterStatus)
                {
                    // Pad out source
                    const int sourceColumnWidth = 12;
                    StringBuilder sourceString = new StringBuilder();
                    sourceString.Append(source);
                    for (int i = 0; i < sourceColumnWidth - source.Length; ++i)
                    {
                        sourceString.Append(' ');
                    }

                    StringBuilder flagsString = new StringBuilder();
                    flagsString.Append(P.Emulation ? 'E' : 'e');
                    flagsString.Append(P.Negative ? 'N' : 'n');
                    flagsString.Append(P.oVerflow ? 'V' : 'v');
                    flagsString.Append(P.accumulatorShort ? 'M' : 'm');
                    flagsString.Append(P.xRegisterShort ? 'X' : 'x');
                    flagsString.Append(P.Decimal ? 'D' : 'd');
                    flagsString.Append(P.IrqDisable ? 'I' : 'i');
                    flagsString.Append(P.Zero ? 'Z' : 'z');
                    flagsString.Append(P.Carry ? 'C' : 'c');

                    evaled = string.Format("{0}  {1} {2}  {3} A:{4:X4} X:{5:X4} Y:{6:X4} P:{7}", PC.ToString("X6"), c.ToString(), sourceString.ToString(), null, A, X, Y, flagsString);
                }
                else
                {
                    evaled = string.Format("{0}  {1} {2}  {3}", PC.ToString("X6"), c.ToString(), source, null);
                }
            }
            return evaled;
        }

        public DebugLine(int pc)
        {
            PC = pc;
            showRegisterStatus = false;
        }

        public DebugLine(int pc, int a, int x, int y, FoenixIDE.Processor.Flags p)
        {
            PC = pc;
            A = a;
            X = x;
            Y = y;
            P = p;
            showRegisterStatus = true;
        }

        public void SetOpcodes(byte[] cmd)
        {
            commandLength = cmd.Length;
            command = cmd;
            StepOver = (Array.Exists(BranchJmpOpcodes, element => element == command[0]));
        }
        public void SetOpcodes(string cmd)
        {
            string[] ops = cmd.Split(',');
            commandLength = ops.Length-1;
            command = new byte[commandLength];
            for (int i = 0; i < commandLength; i++)
            {
                command[i] = Convert.ToByte(ops[i], 16);
            }
            if (commandLength > 0)
            {
                StepOver = (Array.Exists(BranchJmpOpcodes, element => element == command[0]));
            }
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
                if (tokens[0].Length > 3 && !tokens[0].StartsWith(";"))
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

        /*
         * Return true if the line has a non-immediate LDA/STA opcode
         */
        public bool HasAddress()
        {
            return commandLength > 0 && Array.Exists(NonImmediateOpcodes, element => element == command[0]);
        }

        /*
         * Return the name of the address in this line.
         * The format is STA/LDA $123456 or like it.
         */
        public string GetAddressName()
        {
            string mnemonic = source.Substring(4);
            int colon = mnemonic.IndexOf(';');
            if (colon > -1)
            {
                mnemonic = mnemonic.Substring(0, colon - 1).Trim();
            }
            return mnemonic;
        }

        /*
         * Return the address of this line
         */
        public int GetAddress()
        {
            // Read the opcodes in reverse
            int address = 0;
            for (int i = 1; i < commandLength; i++)
            {
                address += command[i] * (int)Math.Pow(256, i - 1);
            }
            return address;
        }
    }
}
