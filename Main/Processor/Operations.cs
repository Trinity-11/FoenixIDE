using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using FoenixIDE;
using FoenixIDE.MemoryLocations;

namespace FoenixIDE.Processor
{
    /* 
     * This file contains all of the opcode routines for the Operations class. 
    */
    public class Operations
    {
        private CPU cpu;
        /// <summary>
        /// Used for addressing modes that require no signature
        /// </summary>
        public const int ADDRESS_IMMEDIATE = 0xf000001;
        public const int ADDRESS_IMPLIED = 0xf000002;

        public delegate void SimulatorCommandEvent(int EventID);
        public event SimulatorCommandEvent SimulatorCommand;

        public Operations(CPU cPU)
        {
            this.cpu = cPU;
        }

        /// <summary>
        /// Branch instructions take a *signed* 8-bit value. The offset is added to the address of the NEXT instruction, so 
        /// branches are always PC + 2 + offset.
        /// </summary>
        /// <param name="b"></param>
        public void BranchNear(byte b)
        {
            int offset = MakeSignedByte(b);
            cpu.PC += offset;
        }

        public sbyte MakeSignedByte(byte b)
        {
            return (sbyte)b;
        }

        public Int16 MakeSignedInt(UInt16 b)
        {
            return (Int16)b;
        }

        public Int16 MakeSignedWord(UInt16 b)
        {
            return (Int16)b;
        }

        private int GetAddress(AddressModes addressMode, int SignatureBytes, RegisterBankNumber Bank)
        {
            int addr;
            int ptr;
            switch (addressMode)
            {
                // The address will not be used in Immediate or Implied mode, but 
                case AddressModes.Immediate:
                    return ADDRESS_IMMEDIATE;
                case AddressModes.Implied:
                    return ADDRESS_IMPLIED;
                case AddressModes.Absolute:
                    return Bank.GetLongAddress(SignatureBytes);
                case AddressModes.AbsoluteLong:
                    return SignatureBytes;
                case AddressModes.AbsoluteIndexedWithX:
                    return Bank.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.AbsoluteLongIndexedWithX:
                    return SignatureBytes + cpu.X.Value;
                case AddressModes.AbsoluteIndexedWithY:
                    return Bank.GetLongAddress(SignatureBytes + cpu.Y.Value);
                case AddressModes.AbsoluteLongIndexedWithY:
                    return Bank.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.DirectPage:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes);
                case AddressModes.DirectPageIndexedWithX:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.DirectPageIndexedWithY:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes + cpu.Y.Value);
                case AddressModes.DirectPageIndexedIndirectWithX:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes) + cpu.X.Value;
                    ptr = cpu.MemMgr.ReadWord(addr);
                    //return cpu.ProgramBank.GetLongAddress(ptr);
                    return (cpu.PC & 0xFF_0000) + ptr;
                case AddressModes.DirectPageIndirect:
                    addr = Bank.GetLongAddress(cpu.DirectPage.GetLongAddress(SignatureBytes) & 0xFFFF);
                    ptr = cpu.MemMgr.ReadWord(addr);
                    return ptr;
                case AddressModes.DirectPageIndirectIndexedWithY:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.MemMgr.ReadWord(addr) + cpu.Y.Value;
                    // why do I keep flipping on this?
                    return cpu.DataBank.GetLongAddress(ptr);
                //return ptr;
                case AddressModes.DirectPageIndirectLong:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.MemMgr.ReadLong(addr);
                    return ptr;
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.MemMgr.ReadLong(addr) + cpu.Y.Value;
                    return ptr;
                case AddressModes.ProgramCounterRelative:
                    ptr = MakeSignedByte((byte)SignatureBytes);
                    addr = cpu.PC + ptr;
                    return addr;
                case AddressModes.ProgramCounterRelativeLong:
                    ptr = MakeSignedInt((UInt16)SignatureBytes);
                    addr = cpu.PC + ptr;
                    return addr;
                case AddressModes.StackImplied:
                    //case AddressModes.StackAbsolute:
                    return 0;
                case AddressModes.StackDirectPageIndirect:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes);
                case AddressModes.StackRelative:
                    return cpu.Stack.Value + SignatureBytes;
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    int bankOffset = Bank.Value << 16;
                    addr = bankOffset + (cpu.Stack.Value + SignatureBytes);
                    return bankOffset + cpu.MemMgr.ReadWord(addr) + cpu.Y.Value;
                case AddressModes.StackProgramCounterRelativeLong:
                    return SignatureBytes;

                // Jump and JSR indirect references vectors located in Bank 0
                case AddressModes.JmpAbsoluteIndirect:
                    addr = SignatureBytes;
                    ptr = cpu.MemMgr.ReadWord(addr);
                    //return cpu.ProgramBank.GetLongAddress(ptr);
                    return (cpu.PC & 0xFF_0000) + ptr;
                case AddressModes.JmpAbsoluteIndirectLong:
                    addr = SignatureBytes;
                    ptr = cpu.MemMgr.ReadLong(addr);
                    return ptr;
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
                    addr = SignatureBytes + cpu.X.Value;
                    //ptr = cpu.Memory.ReadWord(cpu.ProgramBank.GetLongAddress(addr));
                    ptr = cpu.MemMgr.ReadWord((cpu.PC & 0xFF_0000) + addr);
                    //return cpu.ProgramBank.GetLongAddress(ptr);
                    return (cpu.PC & 0xFF_0000) + ptr;
                case AddressModes.Accumulator:
                    return 0;
                default:
                    throw new NotImplementedException("GetAddress() Address mode not implemented: " + addressMode.ToString());
            }
        }

        /// <summary>
        /// Retrieve final data from memory, based on address mode. 
        /// <para>For immediate addressing, just returns the input value</para>
        /// <para>For absolute addressing, returns data at address in signature bytes</para>
        /// <para>For indirect addressing, returns data at address pointed to by address in signature</para>
        /// <para>For indexed modes, uses appropriate index register to adjust the address</para>
        /// </summary>
        /// <param name="mode">Address mode. Direct, Absolute, Immediate, etc. Each mode determines where the data 
        /// is located and how the signature bytes are interpreted.</param>
        /// <param name="signatureBytes">byte or bytes immediately following the opcode. Varies based on the opcode.</param>
        /// Otherwise uses the Data Bank Register, if appropriate.</param>
        /// <returns></returns>
        public int GetValue(AddressModes mode, int signatureBytes, int width)
        {
            switch (mode)
            {
                case AddressModes.Accumulator:
                    return cpu.A.Value;
                case AddressModes.Absolute:
                    return GetAbsolute(signatureBytes, cpu.DataBank, width);
                case AddressModes.AbsoluteLong:
                    return GetAbsoluteLong(signatureBytes);
                case AddressModes.JmpAbsoluteIndirect:
                    // JMP (addr)
                    //return GetAbsoluteIndirectAddress(signatureBytes, cpu.ProgramBank);
                    return GetAbsoluteIndirectAddressLong((cpu.PC & 0xFF_0000) + signatureBytes);
                case AddressModes.JmpAbsoluteIndirectLong:
                    // JMP [addr] - jumps to a 24-bit address pointed to by addr in direct page.
                    return GetAbsoluteIndirectAddressLong(signatureBytes);
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
                    // JMP (addr,X)
                    return GetJumpAbsoluteIndexedIndirect(signatureBytes, cpu.X);
                case AddressModes.AbsoluteIndexedWithX:
                    // LDA $2000,X
                    return GetIndexed(signatureBytes, cpu.DataBank, cpu.X, width);
                case AddressModes.AbsoluteLongIndexedWithX:
                    // LDA $12D080,X
                    return GetAbsoluteLongIndexed(signatureBytes, cpu.X);
                case AddressModes.AbsoluteIndexedWithY:
                    return GetIndexed(signatureBytes, cpu.DataBank, cpu.Y, width);
                case AddressModes.AbsoluteLongIndexedWithY:
                    return GetAbsoluteLongIndexed(signatureBytes, cpu.Y);
                case AddressModes.DirectPage:
                    return GetAbsolute(signatureBytes, cpu.DirectPage, width);
                case AddressModes.DirectPageIndexedWithX:
                    return GetIndexed(signatureBytes, cpu.DirectPage, cpu.X, width);
                case AddressModes.DirectPageIndexedWithY:
                    return GetIndexed(signatureBytes, cpu.DirectPage, cpu.Y, width);
                case AddressModes.DirectPageIndexedIndirectWithX:
                    //LDA(dp, X)
                    return GetDirectIndexedIndirect(signatureBytes, cpu.X);
                case AddressModes.DirectPageIndirect:
                    //LDA (dp)
                    return GetDirectIndirect(signatureBytes);
                case AddressModes.DirectPageIndirectIndexedWithY:
                    //LDA(dp),Y
                    return GetDirectPageIndirectIndexed(signatureBytes, cpu.Y);
                case AddressModes.DirectPageIndirectLong:
                    return GetDirectIndirectLong(signatureBytes);
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    return GetDirectPageIndirectIndexedLong(signatureBytes, cpu.Y);
                case AddressModes.ProgramCounterRelative:
                    return cpu.PC + 2 + MakeSignedByte((byte)signatureBytes);
                case AddressModes.ProgramCounterRelativeLong:
                    return cpu.PC + 3 + MakeSignedWord((UInt16)signatureBytes);
                case AddressModes.StackImplied:
                    return cpu.Stack.Value;
                //case AddressModes.StackAbsolute:
                //    return signatureBytes;
                case AddressModes.StackDirectPageIndirect:
                    throw new NotImplementedException();
                case AddressModes.StackRelative:
                    return GetAbsoluteLong(cpu.Stack.Value + signatureBytes);
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    return GetAbsoluteLong(cpu.MemMgr.ReadWord(cpu.Stack.Value + signatureBytes) + cpu.Y.Value);
                case AddressModes.StackProgramCounterRelativeLong:
                    throw new NotImplementedException();
            }
            return signatureBytes;
        }

        private int GetDirectIndirect(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.MemMgr.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.MemMgr.ReadWord(ptr);
        }

        private int GetDirectIndirectLong(int sig)
        {
            int addr = cpu.DirectPage.GetLongAddress(sig);
            int ptr = cpu.MemMgr.ReadLong(addr);
            return cpu.MemMgr.ReadWord(ptr);
        }

        private int GetDirectPageIndirectIndexedLong(int sig, Register Y)
        {
            int addr =  cpu.DirectPage.GetLongAddress(sig);

            // This effective address can overflow into the next bank.
            int ptr = cpu.MemMgr.ReadLong(addr) + Y.Value;
            return (cpu.A.Width == 1) ? cpu.MemMgr.ReadByte(ptr) : cpu.MemMgr.ReadWord(ptr);
        }

        /// <summary>
        /// LDA (D),Y - returns value pointed to by (D),Y, where D is in Direct page. Final value will be in Data bank. 
        /// </summary>
        /// <param name="sig">Address in direct page</param>
        /// <param name="Y">Register to index</param>
        /// <returns></returns>
        private int GetDirectPageIndirectIndexed(int sig, Register Y)
        {
            // The indirect address must be in Bank 0
            int addr = cpu.DirectPage.GetLongAddress(sig) & 0xFFFF;

            int ptr = cpu.MemMgr.ReadWord(addr) + Y.Value;
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return (cpu.A.Width == 1) ? cpu.MemMgr.ReadByte(ptr) : cpu.MemMgr.ReadWord(ptr);               
        }

        /// <summary>
        /// LDA (D,X) - returns value pointed to by D,X, where D is in Direct page. Final value will be in Data bank.
        /// </summary>
        /// <param name="sig">Address in direct page</param>
        /// <param name="X">Register to index</param>
        /// <returns></returns>
        private int GetDirectIndexedIndirect(int sig, Register X)
        {
            int addr = cpu.DirectPage.GetLongAddress(sig + X.Value);
            int ptr = cpu.MemMgr.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return (cpu.A.Width == 1) ? cpu.MemMgr.ReadByte(ptr) : cpu.MemMgr.ReadWord(ptr);
        }

        private int GetAbsoluteLong(int sig)
        {
            return (cpu.A.Width == 1) ? cpu.MemMgr.ReadByte(sig) : cpu.MemMgr.ReadWord(sig);
        }

        private int GetAbsoluteLongIndexed(int sig, Register Index)
        {
            return (cpu.A.Width == 1) ? cpu.MemMgr.ReadByte(sig + Index.Value) : cpu.MemMgr.ReadWord(sig + Index.Value);
        }

        /// <summary>
        /// Read memory at specified address. Optionally use bank register 
        /// to select the relevant bank.
        /// </summary>
        /// <param name="sig"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        private int GetAbsolute(int sig, Register bank, int width)
        {
            return (width == 1) ? cpu.MemMgr.ReadByte(bank.GetLongAddress(sig)) : cpu.MemMgr.ReadWord(bank.GetLongAddress(sig));
        }

        /// <summary>
        /// LDA $2000,X
        /// </summary>
        /// <param name="sig"></param>
        /// <param name="bank"></param>
        /// <param name="Index">The Index register - maybe short or long.</param>
        /// <param name="width">The width of the register requesting data</param>
        /// <returns></returns>
        private int GetIndexed(int sig, Register bank, Register Index, int width)
        {
            int addr = bank.GetLongAddress(sig);
            addr += Index.Value;
            return (width == 1) ? cpu.MemMgr.ReadByte(addr) : cpu.MemMgr.ReadWord(addr);
        }

        public int GetAbsoluteIndirectAddressLong(int sig)
        {
            int ptr = cpu.DirectPage.GetLongAddress(sig);
            int addr = cpu.MemMgr.ReadLong(ptr);
            return cpu.MemMgr.ReadWord(addr);
        }

        /// <summary>
        /// Get an indirect, indexed Jump address=: JMP ($1200,X)
        /// This looks at location $1200+X in Bank 0 to get the pointer. Then returns an address 
        /// in the current Program Bank (PBR + ($1200,X))
        /// </summary>
        /// <param name="sig">Address of pointer</param>
        /// <param name="Index">Offset of address</param>
        /// <returns></returns>
        private int GetJumpAbsoluteIndexedIndirect(int sig, Register Index)
        {
            int ptr = sig + Index.Value;
            int addr = cpu.MemMgr.ReadWord(ptr);
            //return cpu.ProgramBank.GetLongAddress(ptr);
            return (cpu.PC & 0xFF_0000) + addr;
        }

        /// <summary>
        /// BRK and COP instruction. Pushes the  Program Bank Register, the Program Counter, and the Flags onto the stack. 
        /// Then switches to the Bank 0 addresses stored in the approriate vector. 
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteInterrupt(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            cpu.OpcodeLength = 2;
            cpu.OpcodeCycles = 8;

            switch (instruction)
            {
                case OpcodeList.BRK_Interrupt:
                    cpu.Interrupt(InteruptTypes.BRK);
                    break;
                case OpcodeList.COP_Interrupt:
                    cpu.Interrupt(InteruptTypes.COP);
                    break;
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteInterrupt: " + instruction.ToString("X2"));
            }
            effectiveAddress = -1;
        }

        public void ExecuteORA(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value |= val;
            cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
            effectiveAddress = -1;
        }

        /// <summary>
        /// Test memory against the Accumulator. Sets Z based on the result of an AND. 
        /// Also sets or clears bits in memory based on the bitmask in the accumulator. 
        /// TSB sets bits in memory based on A. TRB clears bits in memory based on A.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteTSBTRB(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            int test = val & cpu.A.Value;
            cpu.Flags.SetZ(test);

            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            effectiveAddress = addr;
            switch (instruction)
            {
                case OpcodeList.TSB_Absolute:
                case OpcodeList.TSB_DirectPage:
                    cpu.MemMgr.Write(addr, val | cpu.A.Value, cpu.A.Width);
                    break;
                case OpcodeList.TRB_Absolute:
                case OpcodeList.TRB_DirectPage:
                    // reset bits in memory when that bit is 1 in A
                    // AND to get bits that are both 1
                    // XOR to force thoses off in memory.
                    int mask = val & cpu.A.Value;
                    cpu.MemMgr.Write(addr, val ^ mask, cpu.A.Width);
                    break;
                default:
                    throw new NotImplementedException("ExecuteTSBTRB() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteShift(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            
            switch (instruction)
            {
                case OpcodeList.ASL_DirectPage:
                case OpcodeList.ASL_Accumulator:
                case OpcodeList.ASL_Absolute:
                case OpcodeList.ASL_DirectPageIndexedWithX:
                case OpcodeList.ASL_AbsoluteIndexedWithX:
                    val <<= 1;
                    if (cpu.A.Width == 1)
                    {
                        cpu.Flags.Carry = val > 0xff;
                        val &= 0xff;
                    }
                    else if (cpu.A.Width == 2)
                    {
                        cpu.Flags.Carry = val > 0xffff;
                        val &= 0xffff;
                    }
                    break;
                case OpcodeList.LSR_DirectPage:
                case OpcodeList.LSR_Accumulator:
                case OpcodeList.LSR_Absolute:
                case OpcodeList.LSR_DirectPageIndexedWithX:
                case OpcodeList.LSR_AbsoluteIndexedWithX:
                    cpu.Flags.Carry = (val & 1) == 1;
                    val >>= 1;
                    break;
                case OpcodeList.ROL_DirectPage:
                case OpcodeList.ROL_Accumulator:
                case OpcodeList.ROL_Absolute:
                case OpcodeList.ROL_DirectPageIndexedWithX:
                case OpcodeList.ROL_AbsoluteIndexedWithX:
                    val <<= 1;
                    if (cpu.Flags.Carry)
                        val += 1;
                    if (cpu.A.Width == 1)
                    {
                        cpu.Flags.Carry = val > 0xff;
                        val &= 0xff;
                    }
                    else if (cpu.A.Width == 2)
                    {
                        cpu.Flags.Carry = val > 0xffff;
                        val &= 0xffff;
                    }
                    break;
                case OpcodeList.ROR_DirectPage:
                case OpcodeList.ROR_Accumulator:
                case OpcodeList.ROR_Absolute:
                case OpcodeList.ROR_DirectPageIndexedWithX:
                case OpcodeList.ROR_AbsoluteIndexedWithX:
                    if (cpu.Flags.Carry)
                    {
                        if (cpu.A.Width == 1)
                            val += 0x100;
                        else if (cpu.A.Width == 2)
                            val += 0x10000;
                    }
                    cpu.Flags.Carry = (val & 1) == 1;
                    val >>= 1;
                    break;
                default:
                    throw new NotImplementedException("ExecuteASL() opcode not implemented: " + instruction.ToString("X2"));
            }

            cpu.Flags.SetNZ(val, cpu.A.Width);
            if (addressMode == AddressModes.Accumulator)
            {
                cpu.A.Value = val;
                effectiveAddress = -1;
            }
            else
            {
                int addr = GetAddress(addressMode, signature, cpu.DataBank);
                effectiveAddress = addr;
                cpu.MemMgr.Write(addr, val, cpu.A.Width);
            }
        }

        public void ExecuteStack(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            switch (instruction)
            {
                case OpcodeList.PHA_StackImplied:
                    cpu.Push(cpu.A);
                    break;
                case OpcodeList.PLA_StackImplied:
                    cpu.PullInto(cpu.A);
                    cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
                    break;
                case OpcodeList.PHX_StackImplied:
                    cpu.Push(cpu.X);
                    break;
                case OpcodeList.PLX_StackImplied:
                    cpu.PullInto(cpu.X);
                    cpu.Flags.SetNZ(cpu.X.Value, cpu.X.Width);
                    break;
                case OpcodeList.PHY_StackImplied:
                    cpu.Push(cpu.Y);
                    break;
                case OpcodeList.PLY_StackImplied:
                    cpu.PullInto(cpu.Y);
                    cpu.Flags.SetNZ(cpu.Y.Value, cpu.Y.Width);
                    break;
                case OpcodeList.PHB_StackImplied:
                    cpu.Push(cpu.DataBank);
                    break;
                case OpcodeList.PLB_StackImplied:
                    cpu.PullInto(cpu.DataBank);
                    cpu.Flags.SetNZ(cpu.DataBank.Value, cpu.DataBank.Width);
                    break;
                case OpcodeList.PHD_StackImplied:
                    cpu.Push(cpu.DirectPage);
                    break;
                case OpcodeList.PLD_StackImplied:
                    cpu.PullInto(cpu.DirectPage);
                    cpu.Flags.SetNZ(cpu.DirectPage.Value, cpu.DirectPage.Width);
                    break;
                case OpcodeList.PHK_StackImplied:
                    cpu.Push(cpu.PC >> 16, 1);
                    break;
                case OpcodeList.PHP_StackImplied:
                    cpu.Push(cpu.Flags);
                    break;
                case OpcodeList.PLP_StackImplied:
                    cpu.PullInto(cpu.Flags);
                    cpu.SyncFlags();
                    break;
                case OpcodeList.PEA_StackAbsolute:
                    // push operand to the stack
                    cpu.Push(signature, 2);
                    break;
                case OpcodeList.PEI_StackDirectPageIndirect:
                    // Read the word at direct page address specified by operand - in Bank 0
                    int addr = cpu.DirectPage.GetLongAddress(signature & 0xFF) & 0xFFFF;
                    effectiveAddress = addr;
                    cpu.Push(cpu.MemMgr.ReadWord(addr), 2);
                    break;
                case OpcodeList.PER_StackProgramCounterRelativeLong:
                    int effRelAddr = (cpu.PC + signature) & 0xFFFF;
                    cpu.Push(effRelAddr, 2);
                    break;
                default:
                    throw new NotImplementedException("ExecuteStack() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteBranch(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            bool takeBranch;
            effectiveAddress = -1;
            switch (instruction)
            {
                case OpcodeList.BCC_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.Carry;
                    break;
                case OpcodeList.BCS_ProgramCounterRelative:
                    takeBranch = cpu.Flags.Carry;
                    break;
                case OpcodeList.BEQ_ProgramCounterRelative:
                    takeBranch = cpu.Flags.Zero;
                    break;
                case OpcodeList.BMI_ProgramCounterRelative:
                    takeBranch = cpu.Flags.Negative;
                    break;
                case OpcodeList.BNE_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.Zero;
                    break;
                case OpcodeList.BPL_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.Negative;
                    break;
                case OpcodeList.BRA_ProgramCounterRelative:
                    takeBranch = true;
                    break;
                case OpcodeList.BVC_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.oVerflow;
                    break;
                case OpcodeList.BVS_ProgramCounterRelative:
                    takeBranch = cpu.Flags.oVerflow;
                    break;
                case OpcodeList.BRL_ProgramCounterRelativeLong:
                    takeBranch = false;  // we are actually always taking this branch, but the offset is a word
                    int offset = MakeSignedWord((UInt16)signature);
                    cpu.PC += offset;
                    break;
                default:
                    throw new NotImplementedException("ExecuteBranch() opcode not implemented: " + instruction.ToString("X2"));
            }

            if (takeBranch)
                BranchNear((byte)signature);
        }

        public void ExecuteStatusReg(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            switch (instruction)
            {
                case OpcodeList.CLC_Implied:
                    cpu.Flags.Carry = false;
                    break;
                case OpcodeList.SEC_Implied:
                    cpu.Flags.Carry = true;
                    break;
                case OpcodeList.CLD_Implied:
                    cpu.Flags.Decimal = false;
                    break;
                case OpcodeList.SED_Implied:
                    cpu.Flags.Decimal = true;
                    break;
                case OpcodeList.CLI_Implied:
                    cpu.Flags.IrqDisable = false;
                    break;
                case OpcodeList.SEI_Implied:
                    cpu.Flags.IrqDisable = true;
                    break;
                case OpcodeList.CLV_Implied:
                    cpu.Flags.oVerflow = false;
                    break;
                case OpcodeList.REP_Immediate:
                    // reset (clear) flag bits that are 1 in the argument
                    // do this by flipping the argument bits, then ANDing 
                    // them to the flag bits 
                    int flip = signature ^ 0xff;
                    cpu.Flags.Value &= flip;
                    break;
                case OpcodeList.SEP_Immediate:
                    // set flag bits that are 1 in the argument. 
                    cpu.Flags.Value |= signature;
                    break;
                case OpcodeList.XCE_Implied:
                    cpu.Flags.SwapCE();
                    break;
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteStatusReg: " + instruction.ToString("X2"));
            }

            cpu.SyncFlags();
        }

        public void ExecuteINCDEC(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int bval;
            int addr;
            effectiveAddress = -1;
            switch (instruction)
            {
                case OpcodeList.DEC_Accumulator:
                    cpu.A.Dec();
                    //cpu.A.Value -= 1;
                    cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
                    break;
                case OpcodeList.DEC_DirectPage:
                case OpcodeList.DEC_Absolute:
                case OpcodeList.DEC_DirectPageIndexedWithX:
                case OpcodeList.DEC_AbsoluteIndexedWithX:
                    bval = GetValue(addressMode, signature, cpu.A.Width);
                    addr = GetAddress(addressMode, signature, cpu.DataBank);
                    bval--;
                    if (cpu.A.Width == 1)
                    {
                        cpu.MemMgr.WriteByte(addr, (byte)bval);
                        cpu.Flags.SetNZ(bval, 1);
                    }
                    else
                    {
                        cpu.MemMgr.WriteWord(addr, bval);
                        cpu.Flags.SetNZ(bval, 2);
                    }
                    
                    break;

                case OpcodeList.INC_Accumulator:
                    //cpu.A.Value += 1;
                    cpu.A.Inc();
                    cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
                    break;
                case OpcodeList.INC_DirectPage:
                case OpcodeList.INC_Absolute:
                case OpcodeList.INC_DirectPageIndexedWithX:
                case OpcodeList.INC_AbsoluteIndexedWithX:
                    //addr = cpu.DirectPage.GetLongAddress(addr);
                    bval = GetValue(addressMode, signature, cpu.A.Width);
                    addr = GetAddress(addressMode, signature, cpu.DataBank);
                    bval++;
                    if (cpu.A.Width == 1)
                    {
                        cpu.MemMgr.WriteByte(addr, (byte)bval);
                        cpu.Flags.SetNZ(bval, 1);
                    }
                    else
                    {
                        cpu.MemMgr.WriteWord(addr, bval);
                        cpu.Flags.SetNZ(bval, 2);
                    }
                    
                    break;

                case OpcodeList.DEX_Implied:
                    //cpu.X.Value -= 1;
                    cpu.X.Dec();
                    cpu.Flags.SetNZ(cpu.X.Value, cpu.X.Width);
                    break;
                case OpcodeList.INX_Implied:
                    //cpu.X.Value += 1;
                    cpu.X.Inc();
                    cpu.Flags.SetNZ(cpu.X.Value, cpu.X.Width);
                    break;
                case OpcodeList.DEY_Implied:
                    //cpu.Y.Value -= 1;
                    cpu.Y.Dec();
                    cpu.Flags.SetNZ(cpu.Y.Value, cpu.Y.Width);
                    break;
                case OpcodeList.INY_Implied:
                    //cpu.Y.Value += 1;
                    cpu.Y.Inc();
                    cpu.Flags.SetNZ(cpu.Y.Value, cpu.Y.Width);
                    break;
                default:
                    break;
            }
        }

        public void ExecuteTransfer(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int transWidth;
            effectiveAddress = -1;
            switch (instruction)
            {
                // C - D - always 16-bit (except in emulation mode)
                case OpcodeList.TCD_Implied:
                    cpu.DirectPage.Value = cpu.A.Value16;
                    cpu.Flags.SetNZ(cpu.DirectPage.Value, 2);
                    break;
                case OpcodeList.TDC_Implied:
                    cpu.A.Value16 = cpu.DirectPage.Value;
                    cpu.Flags.SetNZ(cpu.A.Value16, 2);
                    break;
                case OpcodeList.TCS_Implied:
                    cpu.Stack.Value = cpu.A.Value16;
                    cpu.Stack.TopOfStack = cpu.A.Value16;  // TCS is not available to in emulation mode
                    cpu.Flags.SetNZ(cpu.Stack.Value, 2);
                    break;
                case OpcodeList.TSC_Implied:
                    cpu.A.Value16 = cpu.Stack.Value;
                    cpu.Flags.SetNZ(cpu.A.Value16, 2);
                    break;

                // A - X
                case OpcodeList.TXA_Implied:
                    transWidth = cpu.A.Width;
                    cpu.A.Value = transWidth == 1? cpu.X.Value & 0xFF : cpu.X.Value;
                    cpu.Flags.SetNZ(cpu.A.Value, transWidth);
                    break;
                case OpcodeList.TAX_Implied:
                    transWidth = cpu.X.Width;
                    cpu.X.Value = transWidth == 1 ? cpu.A.Value16 & 0xFF : cpu.A.Value16;
                    cpu.Flags.SetNZ(cpu.X.Value, transWidth);
                    break;
                // A - Y
                case OpcodeList.TYA_Implied:
                    transWidth = cpu.A.Width;
                    cpu.A.Value = transWidth == 1 ? cpu.Y.Value & 0xFF : cpu.Y.Value;
                    cpu.Flags.SetNZ(cpu.A.Value, transWidth);
                    break;
                case OpcodeList.TAY_Implied:
                    transWidth = cpu.Y.Width;
                    cpu.Y.Value = transWidth == 1 ? cpu.A.Value16 & 0xFF : cpu.A.Value16;
                    cpu.Flags.SetNZ(cpu.Y.Value, transWidth);
                    break;

                // S - X
                case OpcodeList.TSX_Implied:
                    transWidth = cpu.X.Width;
                    cpu.X.Value = transWidth == 1 ? cpu.Stack.Value & 0xFF : cpu.Stack.Value;
                    cpu.Flags.SetNZ(cpu.X.Value, transWidth);
                    break;
                case OpcodeList.TXS_Implied:
                    cpu.Stack.Value = cpu.Flags.Emulation ? 0x100 + cpu.X.Value : cpu.X.Value;
                    cpu.Stack.TopOfStack = cpu.Stack.Value;
                    break;

                // X - Y
                case OpcodeList.TXY_Implied:
                    transWidth = cpu.Y.Width;
                    cpu.Y.Value = transWidth == 1 ? cpu.X.Value & 0xFF : cpu.X.Value;
                    cpu.Flags.SetNZ(cpu.Y.Value, transWidth);
                    break;
                
                case OpcodeList.TYX_Implied:
                    transWidth = cpu.X.Width;
                    cpu.X.Value = transWidth == 1 ? cpu.Y.Value & 0xFF : cpu.Y.Value;
                    cpu.Flags.SetNZ(cpu.X.Value, transWidth);
                    break;
                default:
                    throw new NotImplementedException("ExecuteTransfer() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteJumpReturn(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            RegisterBankNumber fakeBank = new RegisterBankNumber
            {
                Value = cpu.PC >> 16
            };
            int addr = GetAddress(addressMode, signature, fakeBank);
            switch (instruction)
            {
                case OpcodeList.JSR_Absolute:
                case OpcodeList.JSR_AbsoluteIndexedIndirectWithX:
                    cpu.Push(cpu.PC - 1, 2);
                    cpu.JumpLong(addr);
                    return;
                case OpcodeList.JSR_AbsoluteLong:
                    cpu.Push(cpu.PC - 1, 3);
                    cpu.JumpLong(addr);
                    return;
                case OpcodeList.JMP_Absolute:
                case OpcodeList.JMP_AbsoluteLong:
                case OpcodeList.JMP_AbsoluteIndirect:
                case OpcodeList.JMP_AbsoluteIndexedIndirectWithX:
                case OpcodeList.JMP_AbsoluteIndirectLong:
                    cpu.JumpLong(addr);
                    return;
                
                // RTS, RTL, RTI
                case OpcodeList.RTI_StackImplied:
                    cpu.Flags.SetFlags(cpu.Pull(1));
                    cpu.SyncFlags();
                    if (cpu.Flags.Emulation)
                    {
                        cpu.JumpShort(cpu.Pull(2));
                    }
                    else
                    {
                        cpu.JumpLong(cpu.Pull(3));
                    }
                    return;
                case OpcodeList.RTS_StackImplied:
                    cpu.JumpShort(cpu.Pull(2) + 1);
                    return;
                case OpcodeList.RTL_StackImplied:
                    addr = cpu.Pull(3);
                    cpu.JumpLong(addr + 1);
                    return;
                default:
                    throw new NotImplementedException("ExecuteJumpReturn() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteAND(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int data = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value &= data;
            cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
        }

        public void ExecuteBIT(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int data = GetValue(addressMode, signature, cpu.A.Width);
            int result = cpu.A.Value & data;
            cpu.Flags.SetZ(result);
            if (addressMode != AddressModes.Immediate)
            {
                if (cpu.A.Width == 2)
                {
                    cpu.Flags.oVerflow = (data & 0x4000) != 0;
                    cpu.Flags.Negative = (data & 0x8000) != 0;
                }
                else
                {
                    cpu.Flags.oVerflow = (data & 0x40) != 0;
                    cpu.Flags.Negative = (data & 0x80) != 0;
                }
            }
        }

        public void ExecuteEOR(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int val = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value ^= val;
            cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
        }

        public void ExecuteMisc(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            switch (instruction)
            {
                // WDM is a 2-byte NOP and an easter egg. William D Mensch designed the 6502 and 65816.
                // We will use this to give the simulator commands
                case OpcodeList.WDM_Implied:
                    OnSimulatorCommand(signature);
                    break;
                case OpcodeList.NOP_Implied:
                    break;
                case OpcodeList.STP_Implied: //stop
                    cpu.Halt();
                    break;
                case OpcodeList.XBA_Implied: // transfer B into A
                    cpu.A.Swap();
                    cpu.Flags.Zero = (cpu.A.Low == 0);
                    cpu.Flags.Negative = (cpu.A.Low & 0x80) != 0;
                    break;
                default:
                    throw new NotImplementedException("ExecuteMisc() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        private void OnSimulatorCommand(int signature)
        {
            if (SimulatorCommand == null)
                return;
            SimulatorCommand(signature);
        }

        /// <summary>
        /// Block moves.
        /// <para>C = bytes to move +1 (so if we're moving 20 bytes, C=19</para>
        /// <para>X=16-bit source address</para>
        /// <para>Y=16-bit destination address</para>
        /// <para>Operand bytes are the source and destination banks.</para>
        /// <para>In the assembled code, order is dest,source. In source code, specify source,dest.</para>
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteBlockMove(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int sourceBank = (signature << 8) & 0xff0000;
            int destBank = (signature << 16) & 0xff0000;
            effectiveAddress = -1;
            cpu.DataBank.Value = signature & 0xFF;

            // For MVN, X and Y are incremented
            // For MVP, X and Y are decremented
            int dir = (instruction == OpcodeList.MVP_BlockMove) ? -1 : 1;

            do
            {
                // The addresses must remain in the correct bank, so the addresses will wrap
                int sourceAddr = sourceBank + cpu.X.Value;
                int destAddr = destBank + cpu.Y.Value;
                cpu.MemMgr[destAddr] = cpu.MemMgr[sourceAddr];
                cpu.X.Value += dir;
                cpu.Y.Value += dir;
                cpu.A.Value--;
            }
            while (cpu.A.Value != 0xFFFF);
        }

        public void ExecuteADC(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int val = GetValue(addressMode, signature, cpu.A.Width);
            int nv;
            if (cpu.Flags.Decimal)
                nv = HexVal(BCDVal(val) + BCDVal(cpu.A.Value) + cpu.Flags.CarryBit);
            else
            { 
                nv = val + cpu.A.Value + cpu.Flags.CarryBit;
            
                // We need to detect a wraparound - when the sign changes but there is no overflow
                if (cpu.A.Width == 1)
                {
                    cpu.Flags.oVerflow = ((cpu.A.Value ^ nv) & ((val + cpu.Flags.CarryBit) ^ nv) & 0x80) != 0;
                }
                else
                {
                    cpu.Flags.oVerflow = ((cpu.A.Value ^ nv) & ((val + cpu.Flags.CarryBit) ^ nv) & 0x8000) != 0;
                }
            }
            cpu.Flags.Carry = (nv < 0 || nv > cpu.A.MaxUnsigned);
            cpu.Flags.SetNZ(nv, cpu.A.Width);

            cpu.A.Value = nv;
        }

        /// <summary>
        /// Subtract value from accumulator. Carry acts as a "borrow". When Carry is 0,
        /// subtract one more from Accumulator. Carry will be 0 if result < 0 and 1 if result >= 0.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteSBC(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int val = GetValue(addressMode, signature, cpu.A.Width);
            int nv;
            if (cpu.Flags.Decimal)
                nv = HexVal(BCDVal(cpu.A.Value) - BCDVal(val + 1) + cpu.Flags.CarryBit);
            else
            {
                nv = cpu.A.Value - val - 1 + cpu.Flags.CarryBit;

                if (cpu.A.Width == 1)
                {
                    cpu.Flags.oVerflow = ((cpu.A.Value ^ nv) & ((0x100 - val - 1 + cpu.Flags.CarryBit) ^ nv) & 0x80) != 0;
                }
                else
                {
                    cpu.Flags.oVerflow = ((cpu.A.Value ^ nv) & ((0x10000 - val - 1 + cpu.Flags.CarryBit) ^ nv) & 0x8000) != 0;
                }
            }
            cpu.Flags.Carry = (nv >= 0 && nv <= cpu.A.MaxUnsigned);
            cpu.Flags.SetNZ(nv, cpu.A.Width);

            cpu.A.Value = nv;
        }

        private int BCDVal(int value)
        {
            if (value < 256)
            {
                return (((value & 0xF0) >> 4 ) * 10) + (value & 0xF);
            }
            else
            {
                int val = (((value & 0xF000) >> 12) * 1000) + (((value & 0xF00) >> 8 ) * 100) + (((value & 0xF0) >> 4 ) * 10 )  + ( value & 0xF );
                return val;
            }
        }
        private int HexVal(int bcd)
        {
            return bcd / 10000 * 256*256 + (bcd % 10000) / 1000 * 256 * 16 + ((bcd % 10000) % 1000) / 100 * 256 + (((bcd % 10000) % 1000) % 100)/ 10 * 16 + (((bcd % 10000) % 1000) % 100) % 10;
        }
        public void ExecuteSTZ(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            effectiveAddress = addr;
            cpu.MemMgr.Write(addr, 0, cpu.A.Width);
        }

        public void ExecuteSTA(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            effectiveAddress = addr;
            cpu.MemMgr.Write(addr, cpu.A.Value, cpu.A.Width);
        }

        public void ExecuteSTY(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            effectiveAddress = addr;
            cpu.MemMgr.Write(addr, cpu.Y.Value, cpu.Y.Width);
        }

        public void ExecuteSTX(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            effectiveAddress = addr;
            cpu.MemMgr.Write(addr, cpu.X.Value, cpu.X.Width);
        }

        public void ExecuteLDA(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int val = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value = val;
            cpu.Flags.SetNZ(cpu.A.Value, cpu.A.Width);
        }

        public void ExecuteLDX(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int val = GetValue(addressMode, signature, cpu.X.Width);
            cpu.X.Value = val;
            cpu.Flags.SetNZ(cpu.X.Value, cpu.X.Width);
        }

        public void ExecuteLDY(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            int val = GetValue(addressMode, signature, cpu.Y.Width);
            cpu.Y.Value = val;
            cpu.Flags.SetNZ(cpu.Y.Value, cpu.Y.Width);
        }

        public void ExecuteCPX(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            Compare(addressMode, signature, cpu.X);
        }

        public void ExecuteCPY(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            Compare(addressMode, signature, cpu.Y);
        }

        public void ExecuteCMP(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            Compare(addressMode, signature, cpu.A);
        }

        public void Compare(AddressModes addressMode, int signature, Register Reg)
        {
            int val = GetValue(addressMode, signature, Reg.Width);
            val = (Reg.Width == 1) ? val & 0xFF : val;
            int subResult = Reg.Value - val;
            cpu.Flags.Zero = subResult == 0;
            cpu.Flags.Carry = Reg.Value >= val;
            cpu.Flags.Negative = Reg.Width == 1 ? (subResult & 0x80) == 0x80 : (subResult & 0x8000) == 0x8000;
        }

        public void ExecuteWAI(byte Instruction, AddressModes AddressMode, int signature, out int effectiveAddress)
        {
            effectiveAddress = -1;
            cpu.Waiting = true;
        }

        // NEW instructions to support the 65C02!!
        public void ResetMemoryBit(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            // the first nibble in the instruction is the bit to modify
            byte bit = (byte)((instruction >> 4) & 7);
            byte nibble = (byte)((1 << bit) ^ 0xFF);

            // direct page addressing
            int address = cpu.DirectPage.Value + signature;
            effectiveAddress = address;
            byte val = cpu.MemMgr.ReadByte(address);
            
            // Reset the bit
            val = (byte)(val & nibble);
            // Assign to the address
            cpu.MemMgr.WriteByte(address, val);
        }

        public void SetMemoryBit(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            // the first nibble in the instruction is the bit to modify
            byte bit = (byte)((instruction >> 4) & 7);
            byte nibble = (byte)(1 << bit);

            // direct page addressing
            int address = cpu.DirectPage.Value + signature;
            effectiveAddress = address;
            byte val = cpu.MemMgr.ReadByte(address);

            // Reset the bit
            val = (byte)(val | nibble);
            // Assign to the address
            cpu.MemMgr.WriteByte(address, val);
        }

        public void BranchBITReset(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            // the first nibble in the instruction is the bit to modify
            byte bit = (byte)((instruction >> 4) & 7);
            byte nibble = (byte)(1 << bit);

            // direct page addressing
            int address = cpu.DirectPage.Value + (signature >> 8);
            effectiveAddress = address;
            byte val = cpu.MemMgr.ReadByte(address);
            if ((val & nibble) == 0)
            {
                BranchNear((byte)(signature & 0xFF));
            }
        }

        public void BranchBITSet(byte instruction, AddressModes addressMode, int signature, out int effectiveAddress)
        {
            // the first nibble in the instruction is the bit to modify
            byte bit = (byte)((instruction >> 4) & 7);
            byte nibble = (byte)(1 << bit);

            // direct page addressing
            int address = cpu.DirectPage.Value + (signature >> 8);
            effectiveAddress = address;
            byte val = cpu.MemMgr.ReadByte(address);
            if ((val & nibble) == 1)
            {
                BranchNear((byte)(signature & 0xFF));
            }
        }

    }
}
