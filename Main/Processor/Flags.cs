using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    public class Flags : Register8
    {
        //flags
        public bool Negative;
        public bool oVerflow;
        public bool Break;
        public bool accumulatorShort;
        public bool xRegisterShort;
        public bool Decimal;
        public bool IrqDisable;
        public bool Zero;
        public bool Carry;
        public bool Emulation;

        /// <summary>
        /// Swap the Carry and Emulation flags. Used by the XCE instruction.
        /// This sets the CPU's emulation mode based on the Carry flag. When
        /// in emulation mode, the CPU can only access 64KB of RAM, cannot use
        /// the PBR and DBR registers, and can only store 8-bit values in A, X, and Y.
        /// <para>To set the CPU to emulation mode, call SEC XCE</para>
        /// <para>To set the CPU to native mode, call CLC XCE</para>
        /// </summary>
        public void SwapCE()
        {
            bool temp = Emulation;
            Emulation = Carry;
            Carry = temp;
        }

        public override int Value
        {
            get
            {
                if (Emulation)
                    return GetFlags(
                        Negative,
                        oVerflow,
                        Break,
                        false,
                        Decimal,
                        IrqDisable,
                        Zero,
                        Carry);
                else
                    return GetFlags(
                        Negative,
                        oVerflow,
                        accumulatorShort,
                        xRegisterShort,
                        Decimal,
                        IrqDisable,
                        Zero,
                        Carry);
            }

            set
            {
                SetFlags(value);
            }
        }

        public virtual int CarryBit
        {
            get
            {
                return Carry ? 1 : 0;
            }
            set
            {
                Carry = value != 0;
            }
        }

        public UInt16 GetFlags(params bool[] flags)
        {
            UInt16 bits = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                bits = (UInt16)(bits << 1);
                if (flags[i])
                    bits = (UInt16)(bits | 1);
            }

            return bits;
        }

        public void SetFlags(int value)
        {
            Negative = (value & 0x80) != 0;
            oVerflow = (value & 0x40) != 0;
            if (!Emulation)
                accumulatorShort = (value & 0x20) != 0;
            else
                Break = (value & 0x20) != 0;
            xRegisterShort = (value & 0x10) != 0;
            Decimal = (value & 8) != 0;
            IrqDisable = (value & 4) != 0;
            Zero = (value & 2) != 0;
            Carry = (value & 1) != 0;
        }

        public override string ToString()
        {
            //NVMXDIZC
            char[] s = new char[10];
            if (Emulation)
            {
                s[0] = Negative ? 'N' : '-';
                s[1] = oVerflow ? 'V' : '-';
                s[2] = Break ? 'B' : '-';
                s[3] = '-';
                s[4] = Decimal ? 'D' : '-';
                s[5] = IrqDisable ? 'I' : '-';
                s[6] = Zero ? 'Z' : '-';
                s[7] = Carry ? 'C' : '-';
                s[8] = ' ';
                s[9] = 'E';
            }
            else
            {
                s[0] = Negative ? 'N' : '-';
                s[1] = oVerflow ? 'V' : '-';
                s[2] = accumulatorShort ? 'M' : '-';
                s[3] = xRegisterShort ? 'X' : '-';
                s[4] = Decimal ? 'D' : '-';
                s[5] = IrqDisable ? 'I' : '-';
                s[6] = Zero ? 'Z' : '-';
                s[7] = Carry ? 'C' : '-';
                s[8] = ' ';
                s[9] = ' ';
            }
            return new string(s);
        }

        public void SetZ(int Val)
        {
            Zero = Val == 0;
        }

        public void SetZ(Register X)
        {
            Zero = X.Value == 0;
        }

        // Deprecated - Delete
        //public void SetNZ_Dep(Register Reg)
        //{
        //    Zero = Reg.Value == 0;
        //    if (Reg.Width == 2)
        //        Negative = ((int)Reg.Value & 0x8000) != 0;
        //    else
        //        Negative = (Reg.Value & 0x80) != 0;
        //}

        public void SetNZ(int Value, int Width)
        {
            Zero = (Width == 1 ? Value & 0xFF : Value & 0xFFFF) == 0;
            if (Width == 1)
                Negative = (Value & 0x80) != 0;
            else if (Width == 2)
                Negative = (Value & 0x8000) != 0;
        }

        public override void Reset()
        {
            Negative = false;
            oVerflow = false;
            Break = false;
            accumulatorShort = false;
            xRegisterShort = false;
            Decimal = false;
            IrqDisable = false;
            Zero = false;
            Carry = false;
            Emulation = false;
        }

    }
}
