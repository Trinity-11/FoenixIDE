using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64;

namespace Nu64.Processor
{
    /* 
     * This file contains all of the opcode routines for the Operations class. 
    */
    public partial class CPU
    {
        /// <summary>
        /// This opcode is not implemented yet. Attempting to execute it will crash the program. 
        /// Call the kernel "abort with error message" routine. 
        /// </summary>
        private void OpNotImplemented()
        {
            throw new NotImplementedException();
        }

        private void OpORA(int val)
        {
            if (A.Length == Register.BitLengthEnum.Bits8)
                val = val & 0xff;

            A.Value = A.Value | val;

            Flags.Negative = A.GetNegativeFlag();
            Flags.Zero = A.GetZeroFlag();
        }

        public void OpBRK()
        {
            opLength = 2;
            opCycles = 8;

            if (!Flags.Emulation)
                Push(K);
            Push(PC, 2);
            Push(P);
            PBR.Value = 0;
            if (!Flags.Emulation)
                PC.Value = Memory.ReadWord(0xffe6);
            else
                PC.Value = Memory.ReadWord(0xfffe);
        }

        public void OpCOP()
        {
            A.Value = GetNextByte(0);
            OpBRK();
        }

        public void OpLoad(Register Dest, int value)
        {
            Dest.Value = value;
            Flags.Negative = Dest.GetNegativeFlag();
            Flags.Zero = Dest.GetZeroFlag();
        }
    }
}
