using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public partial class CPU
    {
        public void Push(int value, int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            int address = Stack.Value;
            Memory[address] = GetByte(value, 0);
            if (bytes >= 2)
                Memory[address - 1] = GetByte(value, 1);
            if (bytes >= 3)
                Memory[address - 2] = GetByte(value, 2);
            Stack.Value -= bytes;
        }

        public void Push(Register Reg, int Offset)
        {
            Push(Reg.Value + Offset, Reg.Bytes);
        }

        public void Push(Register Reg)
        {
            Push(Reg.Value, Reg.Bytes);
        }

        public int Pull(int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            Stack.Value += bytes;
            int address = Stack.Value;
            int ret = Memory[address - 1];
            if (bytes >= 2)
                ret = ret + Memory[address + 2] << 8;
            if (bytes >= 3)
                ret = ret + Memory[address + 3] << 16;

            return ret;
        }

        public void Pull(Register Register)
        {
            Register.Value = Pull(Register.Bytes);
        }

        private byte GetByte(int Value, int Offset)
        {
            if (Offset == 0)
                return (byte)(Value & 0xff);
            if (Offset == 1)
                return (byte)(Value >> 8 & 0xff);
            if (Offset == 2)
                return (byte)(Value >> 16 & 0xff);

            throw new Exception("Offset must be 0-2. Got " + Offset.ToString());
        }

        /// <summary>
        ///  Sets the registers to 8 bits. Sets the emulation flag.
        /// </summary>
        public void SetEmulationMode()
        {
            Flags.Emulation = true;
            A.Length = Register.BitLengthEnum.Bits8;
            A.DiscardUpper = false;
            X.Length = Register.BitLengthEnum.Bits8;
            X.DiscardUpper = true;
            Y.Length = Register.BitLengthEnum.Bits8;
            Y.DiscardUpper = true;
        }

        /// <summary>
        /// Sets the registers to 16 bits. Clears the emulation flag.
        /// </summary>
        public void SetNativeMode()
        {

        }
    }
}
