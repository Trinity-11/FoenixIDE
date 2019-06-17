using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using FoenixIDE;

namespace FoenixIDE.Processor
{
    public class OpCode
    {
        public byte Value;
        public string Mnemonic;
        public AddressModes AddressMode;
        public delegate void ExecuteDelegate(byte Instruction, AddressModes AddressMode, int Signature);
        public event ExecuteDelegate ExecuteOp;
        public int Length8Bit;
        public Register ActionRegister = null;

        public OpCode(byte Value, string Mnemonic, int Length8Bit, Register ActionRegister, AddressModes Mode, ExecuteDelegate newDelegate)
        {
            this.Value = Value;
            this.Length8Bit = Length8Bit;
            this.ActionRegister = ActionRegister;
            this.Mnemonic = Mnemonic;
            this.AddressMode = Mode;
            this.ExecuteOp += newDelegate;

            global::System.Diagnostics.Debug.WriteLine("public const int " + Mnemonic + "_" + Mode.ToString() + "=0x" + Value.ToString("X2") + ";");
        }

        public OpCode(byte Value, string Mnemonic, int Length, AddressModes Mode, ExecuteDelegate newDelegate)
        {
            this.Value = Value;
            this.Length8Bit = Length;
            this.Mnemonic = Mnemonic;
            this.AddressMode = Mode;
            this.ExecuteOp += newDelegate;

            global::System.Diagnostics.Debug.WriteLine("public const int " + Mnemonic + "_" + Mode.ToString() + "=0x" + Value.ToString("X2") + ";");
        }

        public void Execute(int SignatureBytes)
        {
            if (ExecuteOp == null)
                throw new NotImplementedException("Tried to execute " + this.Mnemonic + " but it is not implemented.");

            ExecuteOp(Value, AddressMode, SignatureBytes);
        }

        public int Length
        {
            get
            {
                if (ActionRegister != null && ActionRegister.Width == 2)
                    return Length8Bit + 1;

                return Length8Bit;
            }
        }

        public override string ToString()
        {
            return this.Mnemonic + " " + this.AddressMode.ToString();
        }

        public string ToString(int Signature)
        {
            string arg, sig;
            if (this.Length == 3)
                sig = "$"+Signature.ToString("X4");
            else if (this.Length == 4)
                sig = "$"+Signature.ToString("X6");
            else
                sig = "$"+Signature.ToString("X2");

            switch (this.AddressMode)
            {
                case AddressModes.Interrupt:
                    arg = sig;
                    break;
                case AddressModes.Immediate:
                    arg = "#" + sig;
                    break;
                case AddressModes.DirectPage:
                case AddressModes.Absolute:
                case AddressModes.AbsoluteLong:
                    arg = sig;
                    break;
                case AddressModes.DirectPageIndirect:
                case AddressModes.JmpAbsoluteIndirect:
                    arg = "(" + sig + ")";
                    break;
                case AddressModes.DirectPageIndexedIndirectWithX:
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
                    arg = "(" + sig + ",X)";
                    break;
                case AddressModes.DirectPageIndexedWithX:
                case AddressModes.AbsoluteIndexedWithX:
                case AddressModes.AbsoluteLongIndexedWithX:
                    arg = sig + ",X";
                    break;
                case AddressModes.DirectPageIndexedWithY:
                case AddressModes.AbsoluteIndexedWithY:
                case AddressModes.AbsoluteLongIndexedWithY:
                    arg = sig + ",Y";
                    break;
                case AddressModes.DirectPageIndirectIndexedWithY:
                    arg = "(" + sig + "),Y";
                    break;
                case AddressModes.DirectPageIndirectLong:
                case AddressModes.JmpAbsoluteIndirectLong:
                    arg = "[" + sig + "]";
                    break;
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    arg = "[DP+" + sig + "],Y";
                    break;
                case AddressModes.ProgramCounterRelative:
                case AddressModes.ProgramCounterRelativeLong:
                    arg = sig;
                    break;
                //case AddressModes.StackAbsolute:
                //    arg = sig;
                //    break;
                case AddressModes.StackDirectPageIndirect:
                    arg = sig;
                    break;
                case AddressModes.StackRelative:
                    arg = sig + ",S";
                    break;
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    arg = "(" + sig + ",S),Y";
                    break;
                case AddressModes.StackProgramCounterRelativeLong:
                    arg = sig;
                    break;
                default:
                    arg = "";
                    break;
            }
            return this.Mnemonic + " " + arg;
        }

    }
}
