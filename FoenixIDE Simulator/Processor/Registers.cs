using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public class Registers
    {
        /// <summary>
        /// Accumulator
        /// </summary>
        public Register A = new Register();
        /// <summary>
        /// Data Bank Register
        /// </summary>
        public BankRegister DBR = new BankRegister();
        /// <summary>
        /// Direct Register
        /// </summary>
        public Register D = new Register();
        /// <summary>
        /// Program Bank Register
        /// </summary>
        public BankRegister PBR = new BankRegister();
        /// <summary>
        /// Program Counter
        /// </summary>
        public Register16 PC = new Register16();
        /// <summary>
        /// Processor Status Register
        /// </summary>
        public Flags Flags = new Flags();
        /// <summary>
        /// Stack Pointer. The stack is always in the first 64KB page.
        /// </summary>
        public Register S = new Register();
        /// <summary>
        /// X Index Regiser
        /// </summary>
        public Register X = new Register();
        /// <summary>
        /// Y Index Register
        /// </summary>
        public Register Y = new Register();
        /// <summary>
        /// program banK register
        /// </summary>
        public Register8 K
        {
            get { return PBR; }
        }
        /// <summary>
        /// Processor status register
        /// </summary>
        public Flags P
        {
            get { return this.Flags; }
        }

        public void SetPC(int address)
        {
            PBR.Value = (address & 0xff0000) >> 16;
            PC.Value = (address & 0xffff);
        }

        public Registers()
        {
            SetEmulationMode();
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

        public int GetLongPC()
        {
            return PBR.GetLongAddress(PC);
        }

    }

}
