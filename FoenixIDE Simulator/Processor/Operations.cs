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
        /// Used for addressing modes that 
        /// </summary>
        public const int ADDRESS_IMMEDIATE = 0xf000001;
        public const int ADDRESS_IMPLIED = 0xf000002;

        public delegate void SimulatorCommandEvent(int EventID);
        public event SimulatorCommandEvent SimulatorCommand;

        public Operations(CPU cPU)
        {
            this.cpu = cPU;
        }

        public void Reset()
        {
            cpu.A.Reset();
            cpu.X.Reset();
            cpu.Y.Reset();
            cpu.Flags.Reset();
            cpu.DataBank.Reset();
            cpu.DirectPage.Reset();
            cpu.ProgramBank.Reset();
            cpu.PC.Reset();

            cpu.PC.Value = cpu.Memory.ReadWord(MemoryMap.VECTOR_RESET);
        }

        /// <summary>
        /// This opcode is not implemented yet. 
        /// </summary>
        public void ExecuteAbort()
        {
            cpu.OpcodeLength = 1;
            cpu.OpcodeCycles = 1;

            cpu.Interrupt(InteruptTypes.ABORT);
        }

        public void OpORA(int val)
        {
            if (cpu.A.Width == 1)
                val = val & 0xff;

            cpu.A.Value = cpu.A.Value | val;
            cpu.Flags.SetNZ(cpu.A);
        }

        public void OpLoad(Register Dest, int value)
        {
            Dest.Value = value;
            cpu.Flags.SetNZ(Dest);
        }

        /// <summary>
        /// Branch instructions take a *signed* 8-bit value. The offset is added to the address of the NEXT instruction, so 
        /// branches are always PC + 2 + offset.
        /// </summary>
        /// <param name="b"></param>
        public void BranchNear(byte b)
        {
            int offset = MakeSignedByte(b);
            cpu.PC.Value += offset;
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
        /// <param name="isCode">Assume the address is code and uses the Program Bank Register. 
        /// Otherwise uses the Data Bank Register, if appropriate.</param>
        /// <returns></returns>
        public int GetValue(AddressModes mode, int signatureBytes, int bytes)
        {
            switch (mode)
            {
                case AddressModes.Accumulator:
                    return cpu.A.Value;
                case AddressModes.Absolute:
                    return GetAbsolute(signatureBytes, cpu.DataBank, bytes);
                case AddressModes.AbsoluteLong:
                    return GetAbsoluteLong(signatureBytes);
                case AddressModes.JmpAbsoluteIndirect:
                    // JMP (addr)
                    return GetAbsoluteIndirectAddress(signatureBytes, cpu.ProgramBank);
                case AddressModes.JmpAbsoluteIndirectLong:
                    // JMP [addr] - jumps to a 24-bit address pointed to by addr in direct page.
                    return GetAbsoluteIndirectAddressLong(signatureBytes);
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
                    // JMP (addr,X)
                    return GetJumpAbsoluteIndexedIndirect(signatureBytes, cpu.ProgramBank, cpu.X);
                case AddressModes.AbsoluteIndexedWithX:
                    // LDA $2000,X
                    return GetIndexed(signatureBytes, cpu.DataBank, cpu.X);
                case AddressModes.AbsoluteLongIndexedWithX:
                    // LDA $12D080,X
                    return GetAbsoluteLongIndexed(signatureBytes, cpu.X);
                case AddressModes.AbsoluteIndexedWithY:
                    return GetIndexed(signatureBytes, cpu.DataBank, cpu.Y);
                case AddressModes.AbsoluteLongIndexedWithY:
                    return GetAbsoluteLongIndexed(signatureBytes, cpu.Y);
                case AddressModes.DirectPage:
                    return GetAbsolute(signatureBytes, cpu.DirectPage, bytes);
                case AddressModes.DirectPageIndexedWithX:
                    return GetIndexed(signatureBytes, cpu.DirectPage, cpu.X);
                case AddressModes.DirectPageIndexedWithY:
                    return GetIndexed(signatureBytes, cpu.DirectPage, cpu.Y);
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
                    return cpu.PC.Value + 2 + MakeSignedByte((byte)signatureBytes);
                case AddressModes.ProgramCounterRelativeLong:
                    return cpu.PC.Value + 3 + MakeSignedWord((UInt16)signatureBytes);
                case AddressModes.StackImplied:
                    return cpu.Stack.Value;
                //case AddressModes.StackAbsolute:
                //    return signatureBytes;
                case AddressModes.StackDirectPageIndirect:
                    throw new NotImplementedException();
                case AddressModes.StackRelative:
                    return GetAbsoluteLong(cpu.Stack.Value + signatureBytes);
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    return GetAbsoluteLong(cpu.Memory.ReadWord(cpu.Stack.Value + signatureBytes) + cpu.Y.Value);
                case AddressModes.StackProgramCounterRelativeLong:
                    throw new NotImplementedException();
            }
            return signatureBytes;
        }

        private int GetDirectIndirect(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.Memory.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.Memory.ReadWord(ptr);
        }

        private int GetDirectIndirectLong(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.Memory.ReadLong(addr);
            return cpu.Memory.ReadWord(ptr);
        }

        private int GetDirectPageIndirectIndexedLong(int Address, Register Y)
        {
            int addr =  cpu.DirectPage.GetLongAddress(Address);

            // This effective address can overflow into the next bank.
            int ptr = cpu.Memory.ReadLong(addr) + Y.Value;
            if (cpu.A.Width == 1)
                return cpu.Memory.ReadByte(ptr);
            else
                return cpu.Memory.ReadWord(ptr);
        }

        /// <summary>
        /// LDA (D),Y - returns value pointed to by (D),Y, where D is in Direct page. Final value will be in Data bank. 
        /// </summary>
        /// <param name="Address">Address in direct page</param>
        /// <param name="X">Register to index</param>
        /// <returns></returns>
        private int GetDirectPageIndirectIndexed(int Address, Register Y)
        {
            // The indirect address must be in Bank 0
            int addr = cpu.DirectPage.GetLongAddress(Address) & 0xFFFF;

            int ptr = cpu.Memory.ReadWord(addr) + Y.Value;
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.Memory.ReadWord(ptr);               
        }

        /// <summary>
        /// LDA (D,X) - returns value pointed to by D,X, where D is in Direct page. Final value will be in Data bank.
        /// </summary>
        /// <param name="Address">Address in direct page</param>
        /// <param name="X">Register to index</param>
        /// <returns></returns>
        private int GetDirectIndexedIndirect(int Address, Register X)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address + X.Value);
            int ptr = cpu.Memory.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.Memory.ReadWord(ptr);
        }

        private int GetAbsoluteLong(int Address)
        {
            return (cpu.A.Width == 1) ? cpu.Memory.ReadByte(Address) : cpu.Memory.ReadWord(Address);
        }

        private int GetAbsoluteLongIndexed(int Address, Register Index)
        {
            return (cpu.A.Width == 1) ? cpu.Memory.ReadByte(Address + Index.Value) : cpu.Memory.ReadWord(Address + Index.Value);
        }

        /// <summary>
        /// Read memory at specified address. Optionally use bank register 
        /// to select the relevant bank.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        private int GetAbsolute(int Address, Register bank, int bytes)
        {
            return (bytes == 1) ? cpu.Memory.ReadByte(bank.GetLongAddress(Address)) : cpu.Memory.ReadWord(bank.GetLongAddress(Address));
        }

        /// <summary>
        /// LDA $2000,X
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="bank"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetIndexed(int Address, Register bank, Register Index)
        {
            int addr = Address;
            addr = bank.GetLongAddress(Address);
            addr = addr + Index.Value;
            return cpu.Memory.ReadWord(addr);
        }

        /// <summary>
        /// Returns a pointer from memory. 
        /// JMP ($xxxx) reads a two-byte address from bank 0. It jumps to that address in the current
        /// program bank (meaning it adds PBR to get the final address.) 
        /// </summary>
        /// <param name="Address">Address of pointer. Final value is address pointer references.</param>
        /// <param name="block"></param>
        /// <returns></returns>
        public int GetAbsoluteIndirectAddress(int Address, Register bank)
        {
            int ptr = cpu.Memory.ReadWord(Address);
            return bank.GetLongAddress(ptr);
        }

        public int GetAbsoluteIndirectAddressLong(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.Memory.ReadLong(addr);
            return cpu.Memory.ReadWord(ptr);
        }

        /// <summary>
        /// Get an indirect, indexed Jump address=: JMP ($1200,X)
        /// This looks at location $1200+X in Bank 0 to get the pointer. Then returns an address 
        /// in the current Program Bank (PBR + ($1200,X))
        /// </summary>
        /// <param name="Address">Address of pointer</param>
        /// <param name="bank">Program Bank</param>
        /// <param name="Index">Offset of address</param>
        /// <returns></returns>
        private int GetJumpAbsoluteIndexedIndirect(int Address, Register bank, Register Index)
        {
            int addr = Address + Index.Value;
            int ptr = cpu.Memory.ReadWord(addr);
            return cpu.ProgramBank.GetLongAddress(ptr);
        }

        /// <summary>
        /// BRK and COP instruction. Pushes the  Program Bank Register, the Program Counter, and the Flags onto the stack. 
        /// Then switches to the Bank 0 addresses stored in the approriate vector. 
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteInterrupt(byte instruction, AddressModes addressMode, int signature)
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
        }

        public void ExecuteORA(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value = cpu.A.Value | val;
            cpu.Flags.SetNZ(cpu.A);
        }

        /// <summary>
        /// Test memory against the Accumulator. Sets Z based on the result of an AND. 
        /// Also sets or clears bits in memory based on the bitmask in the accumulator. 
        /// TSB sets bits in memory based on A. TRB clears bits in memory based on A.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteTSBTRB(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            int test = val & cpu.A.Value;
            cpu.Flags.SetZ(test);

            int addr = GetAddress(addressMode, signature, cpu.DataBank);

            switch (instruction)
            {
                case OpcodeList.TSB_Absolute:
                case OpcodeList.TSB_DirectPage:
                    cpu.Memory.Write(addr, val | cpu.A.Value, cpu.A.Width);
                    break;
                case OpcodeList.TRB_Absolute:
                case OpcodeList.TRB_DirectPage:
                    // reset bits in memory when that bit is 1 in A
                    // AND to get bits that are both 1
                    // XOR to force thoses off in memory.
                    int mask = val & cpu.A.Value;
                    cpu.Memory.Write(addr, val ^ mask, cpu.A.Width);
                    break;
                default:
                    throw new NotImplementedException("ExecuteTSBTRB() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteShift(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            switch (instruction)
            {
                case OpcodeList.ASL_DirectPage:
                case OpcodeList.ASL_Accumulator:
                case OpcodeList.ASL_Absolute:
                case OpcodeList.ASL_DirectPageIndexedWithX:
                case OpcodeList.ASL_AbsoluteIndexedWithX:
                    val = val << 1;
                    if (cpu.A.Width == 1)
                    {
                        cpu.Flags.Carry = val > 0xff;
                        val = val & 0xff;
                    }
                    else if (cpu.A.Width == 2)
                    {
                        cpu.Flags.Carry = val > 0xffff;
                        val = val & 0xffff;
                    }
                    break;
                case OpcodeList.LSR_DirectPage:
                case OpcodeList.LSR_Accumulator:
                case OpcodeList.LSR_Absolute:
                case OpcodeList.LSR_DirectPageIndexedWithX:
                case OpcodeList.LSR_AbsoluteIndexedWithX:
                    cpu.Flags.Carry = (val & 1) == 1;
                    val = val >> 1;
                    break;
                case OpcodeList.ROL_DirectPage:
                case OpcodeList.ROL_Accumulator:
                case OpcodeList.ROL_Absolute:
                case OpcodeList.ROL_DirectPageIndexedWithX:
                case OpcodeList.ROL_AbsoluteIndexedWithX:
                    val = val << 1;
                    if (cpu.Flags.Carry)
                        val += 1;
                    if (cpu.A.Width == 1)
                    {
                        cpu.Flags.Carry = val > 0xff;
                        val = val & 0xff;
                    }
                    else if (cpu.A.Width == 2)
                    {
                        cpu.Flags.Carry = val > 0xffff;
                        val = val & 0xffff;
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
                    val = val >> 1;
                    break;
                default:
                    throw new NotImplementedException("ExecuteASL() opcode not implemented: " + instruction.ToString("X2"));
            }

            cpu.Flags.SetNZ(val, cpu.A.Width);
            if (addressMode == AddressModes.Accumulator)
                cpu.A.Value = val;
            else
                cpu.Memory.Write(addr, val, cpu.A.Width);
        }

        public void ExecuteStack(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.PHA_StackImplied:
                    cpu.Push(cpu.A);
                    break;
                case OpcodeList.PLA_StackImplied:
                    cpu.PullInto(cpu.A);
                    break;
                case OpcodeList.PHX_StackImplied:
                    cpu.Push(cpu.X);
                    break;
                case OpcodeList.PLX_StackImplied:
                    cpu.PullInto(cpu.X);
                    break;
                case OpcodeList.PHY_StackImplied:
                    cpu.Push(cpu.Y);
                    break;
                case OpcodeList.PLY_StackImplied:
                    cpu.PullInto(cpu.Y);
                    break;
                case OpcodeList.PHB_StackImplied:
                    cpu.Push(cpu.DataBank);
                    break;
                case OpcodeList.PLB_StackImplied:
                    cpu.PullInto(cpu.DataBank);
                    break;
                case OpcodeList.PHD_StackImplied:
                    cpu.Push(cpu.DirectPage);
                    break;
                case OpcodeList.PLD_StackImplied:
                    cpu.PullInto(cpu.DirectPage);
                    break;
                case OpcodeList.PHK_StackImplied:
                    cpu.Push(cpu.ProgramBank);
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
                    cpu.Push(cpu.Memory.ReadWord(addr), 2);
                    break;
                case OpcodeList.PER_StackProgramCounterRelativeLong:
                    int effRelAddr = cpu.PC.Value + signature & 0xFFFF;
                    cpu.Push(effRelAddr, 2);
                    break;
                default:
                    throw new NotImplementedException("ExecuteStack() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteBranch(byte instruction, AddressModes addressMode, int signature)
        {
            bool takeBranch = false;
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
                    cpu.PC.Value += offset;
                    break;
                default:
                    throw new NotImplementedException("ExecuteBranch() opcode not implemented: " + instruction.ToString("X2"));
            }

            if (takeBranch)
                BranchNear((byte)signature);
        }

        public void ExecuteStatusReg(byte instruction, AddressModes addressMode, int signature)
        {
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
                    cpu.Flags.Value = cpu.Flags.Value & flip;
                    break;
                case OpcodeList.SEP_Immediate:
                    // set flag bits that are 1 in the argument. 
                    cpu.Flags.Value = cpu.Flags.Value | signature;
                    break;
                case OpcodeList.XCE_Implied:
                    cpu.Flags.SwapCE();
                    break;
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteStatusReg: " + instruction.ToString("X2"));
            }

            cpu.SyncFlags();
        }

        public void ExecuteINCDEC(byte instruction, AddressModes addressMode, int signature)
        {
            int bval = GetValue(addressMode, signature, cpu.A.Width);
            int addr = GetAddress(addressMode, signature, cpu.DataBank);

            switch (instruction)
            {
                case OpcodeList.DEC_Accumulator:
                    cpu.A.Value -= 1;
                    cpu.Flags.SetNZ(cpu.A);
                    break;
                case OpcodeList.DEC_DirectPage:
                case OpcodeList.DEC_Absolute:
                case OpcodeList.DEC_DirectPageIndexedWithX:
                case OpcodeList.DEC_AbsoluteIndexedWithX:
                    bval--;
                    if (cpu.A.Width == 1)
                    {
                        cpu.Memory.WriteByte(addr, (byte)bval);
                    }
                    else
                    {
                        cpu.Memory.WriteWord(addr, bval);
                    }
                    cpu.Flags.SetNZ(bval, 1);
                    break;

                case OpcodeList.INC_Accumulator:
                    cpu.A.Value += 1;
                    cpu.Flags.SetNZ(cpu.A);
                    break;
                case OpcodeList.INC_DirectPage:
                case OpcodeList.INC_Absolute:
                case OpcodeList.INC_DirectPageIndexedWithX:
                case OpcodeList.INC_AbsoluteIndexedWithX:
                    //addr = cpu.DirectPage.GetLongAddress(addr);
                    bval++;
                    if (cpu.A.Width == 1)
                    {
                        cpu.Memory.WriteByte(addr, (byte)bval);
                    }
                    else
                    {
                        cpu.Memory.WriteWord(addr, bval);
                    }
                    
                    cpu.Flags.SetNZ(bval, 1);
                    break;

                case OpcodeList.DEX_Implied:
                    cpu.X.Value -= 1;
                    cpu.Flags.SetNZ(cpu.X);
                    break;
                case OpcodeList.INX_Implied:
                    cpu.X.Value += 1;
                    cpu.Flags.SetNZ(cpu.X);
                    break;
                case OpcodeList.DEY_Implied:
                    cpu.Y.Value -= 1;
                    cpu.Flags.SetNZ(cpu.Y);
                    break;
                case OpcodeList.INY_Implied:
                    cpu.Y.Value += 1;
                    cpu.Flags.SetNZ(cpu.Y);
                    break;
                default:
                    break;
            }


        }

        private int GetAddress(AddressModes addressMode, int SignatureBytes, RegisterBankNumber Bank)
        {
            int addr = 0;
            int ptr = 0;
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
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirect:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.DataBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirectIndexedWithY:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadWord(addr) + cpu.Y.Value;
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirectLong:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadLong(addr);
                    return ptr;
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadLong(addr) + cpu.Y.Value;
                    return ptr;
                case AddressModes.ProgramCounterRelative:
                    ptr = MakeSignedByte((byte)SignatureBytes);
                    addr = cpu.PC.Value + ptr;
                    return addr;
                case AddressModes.ProgramCounterRelativeLong:
                    ptr = MakeSignedInt((UInt16)SignatureBytes);
                    addr = cpu.PC.Value + ptr;
                    return addr;
                case AddressModes.StackImplied:
                    //case AddressModes.StackAbsolute:
                    return 0;
                case AddressModes.StackDirectPageIndirect:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes);
                case AddressModes.StackRelative:
                    return cpu.Stack.Value + SignatureBytes;
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    return cpu.Stack.Value + SignatureBytes + cpu.Y.Value;
                case AddressModes.StackProgramCounterRelativeLong:
                    return SignatureBytes;

                // Jump and JSR indirect references vectors located in Bank 0
                case AddressModes.JmpAbsoluteIndirect:
                    addr = SignatureBytes;
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.JmpAbsoluteIndirectLong:
                    addr = SignatureBytes;
                    ptr = cpu.Memory.ReadLong(addr);
                    return ptr;
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
                    addr = SignatureBytes + cpu.X.Value;
                    ptr = cpu.Memory.ReadWord(cpu.ProgramBank.GetLongAddress(addr));
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.Accumulator:
                    return 0; 
                default:
                    throw new NotImplementedException("GetAddress() Address mode not implemented: " + addressMode.ToString());
            }
        }

        public void ExecuteTransfer(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.TCD_Implied:
                    cpu.DirectPage.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TDC_Implied:
                    cpu.A.Value16 = cpu.DirectPage.Value;
                    break;
                case OpcodeList.TCS_Implied:
                    cpu.Stack.Value = cpu.A.Value16;
                    cpu.Stack.TopOfStack = cpu.A.Value16;
                    break;
                case OpcodeList.TSC_Implied:
                    cpu.A.Value16 = cpu.Stack.Value;
                    break;
                case OpcodeList.TAX_Implied:
                    cpu.X.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TAY_Implied:
                    cpu.Y.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TSX_Implied:
                    cpu.X.Value = cpu.Stack.Value;
                    break;
                case OpcodeList.TXA_Implied:
                    cpu.A.Value = cpu.X.Value;
                    break;
                case OpcodeList.TXS_Implied:
                    cpu.Stack.Value = cpu.X.Value;
                    cpu.Stack.TopOfStack = cpu.X.Value;
                    break;
                case OpcodeList.TXY_Implied:
                    cpu.Y.Value = cpu.X.Value;
                    break;
                case OpcodeList.TYA_Implied:
                    cpu.A.Value = cpu.Y.Value;
                    break;
                case OpcodeList.TYX_Implied:
                    cpu.X.Value = cpu.Y.Value;
                    break;
                default:
                    throw new NotImplementedException("ExecuteTransfer() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteJumpReturn(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.ProgramBank);
            switch (instruction)
            {
                case OpcodeList.JSR_Absolute:
                case OpcodeList.JSR_AbsoluteIndexedIndirectWithX:
                    cpu.Push(cpu.PC);
                    cpu.JumpShort(addr);
                    return;
                case OpcodeList.JSR_AbsoluteLong:
                    cpu.Push(cpu.ProgramBank);
                    cpu.Push(cpu.PC);
                    cpu.JumpLong(addr);
                    return;
                case OpcodeList.JMP_Absolute:
                case OpcodeList.JMP_AbsoluteLong:
                case OpcodeList.JMP_AbsoluteIndirect:
                case OpcodeList.JMP_AbsoluteIndexedIndirectWithX:
                case OpcodeList.JMP_AbsoluteIndirectLong:
                    cpu.JumpLong(addr);
                    return;
                case OpcodeList.RTS_StackImplied:
                    cpu.JumpShort(cpu.Pull(2));
                    return;
                case OpcodeList.RTI_StackImplied:
                    cpu.Flags.SetFlags(cpu.Pull(1));
                    int address = cpu.Pull(2);
                    if (!cpu.Flags.Emulation)
                    {
                        cpu.DataBank.Value = cpu.Pull(1);
                        address += (cpu.DataBank.Value << 16);
                    }
                    cpu.JumpLong(address);
                    cpu.SyncFlags();
                    return;
                case OpcodeList.RTL_StackImplied:
                    addr = cpu.Pull(3);
                    cpu.JumpLong(addr);
                    return;
                default:
                    throw new NotImplementedException("ExecuteJumpReturn() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteAND(byte instruction, AddressModes addressMode, int signature)
        {
            int data = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value = cpu.A.Value & data;
            cpu.Flags.SetNZ(cpu.A);
        }

        public void ExecuteBIT(byte instruction, AddressModes addressMode, int signature)
        {
            int data = GetValue(addressMode, signature, cpu.A.Width);
            int result = cpu.A.Value & data;
            if (addressMode != AddressModes.Immediate)
            {
                cpu.Flags.SetNZ(result, cpu.A.Width);
                if (cpu.A.Width == 2)
                    cpu.Flags.oVerflow = (result & 0x4000) == 0x4000;
                else
                    cpu.Flags.oVerflow = (result & 0x400) == 0x40;
            }
            else
                cpu.Flags.SetZ(result);
        }

        public void ExecuteEOR(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            cpu.A.Value = cpu.A.Value ^ val;
            cpu.Flags.SetNZ(cpu.A);
        }

        public void ExecuteMisc(byte instruction, AddressModes addressMode, int signature)
        {
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
                    cpu.Flags.Negative = ((cpu.A.Low & 0x80) == 0x80);
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
        public void ExecuteBlockMove(byte instruction, AddressModes addressMode, int signature)
        {
            int sourceBank = (signature << 8) & 0xff0000;
            int destBank = (signature << 16) & 0xff0000;

            
            int bytesToMove = cpu.A.Value + 1;

            // For MVN, X and Y are incremented
            // For MVP, X and Y are decremented
            int dir = (instruction == OpcodeList.MVP_BlockMove) ? -1 : 1;

            while (cpu.A.Value != 0xFFFF)
            {
                // The addresses must remain in the correct bank, so the addresses will wrap
                int sourceAddr = sourceBank + cpu.X.Value;
                int destAddr = destBank + cpu.Y.Value;
                cpu.Memory[destAddr] = cpu.Memory[sourceAddr];
                cpu.X.Value += dir;
                cpu.Y.Value += dir;
                cpu.A.Value--;
            }
        }

        public void ExecuteADC(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);

            if (cpu.Flags.Decimal)
                val = HexVal(BCDVal(val) + BCDVal(cpu.A.Value) + cpu.Flags.CarryBit);
            else
                val = val + cpu.A.Value + cpu.Flags.CarryBit;

            cpu.Flags.Carry = (val < 0 || val > cpu.A.MaxUnsigned);
            cpu.Flags.oVerflow = (val < cpu.A.MinSigned || val > cpu.A.MaxSigned);
            cpu.Flags.SetNZ(val, cpu.A.Width);

            cpu.A.Value = val;
        }

        private int BCDVal(int value)
        {
            if (value < 256)
            {
                return (((value & 0xF0) >> 4 ) * 10) + (value & 0xF);
            }
            else
            {
                return (((value & 0xF000) >> 12) * 1000) + (((value & 0xF00) >> 8 ) * 100) + (((value & 0xF0) >> 4 ) * 10 )  + ( value & 0xF );
            }
        }
        private int HexVal(int bcd)
        {
            return bcd / 10 * 16 + bcd % 10;
        }
        public void ExecuteSTZ(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, 0, cpu.A.Width);
        }

        public void ExecuteSTA(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, cpu.A.Value, cpu.A.Width);
        }

        public void ExecuteSTY(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, cpu.Y.Value, cpu.Y.Width);
        }

        public void ExecuteSTX(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, cpu.X.Value, cpu.X.Width);
        }

        public void ExecuteLDA(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);
            if (addressMode == AddressModes.AbsoluteIndexedWithX && (val & 0xff) == 0)
                global::System.Diagnostics.Debug.WriteLine("LDA break " + instruction + "," + signature);
            cpu.A.Value = val;
            cpu.Flags.SetNZ(cpu.A);
        }

        public void ExecuteLDX(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.X.Width);
            cpu.X.Value = val;
            cpu.Flags.SetNZ(cpu.X);
        }

        public void ExecuteLDY(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.Y.Width);
            cpu.Y.Value = val;
            cpu.Flags.SetNZ(cpu.Y);
        }

        public void ExecuteCPX(byte instruction, AddressModes addressMode, int signature)
        {
            Compare(addressMode, signature, cpu.X);
        }

        public void ExecuteCPY(byte instruction, AddressModes addressMode, int signature)
        {
            Compare(addressMode, signature, cpu.Y);
        }

        public void ExecuteCMP(byte instruction, AddressModes addressMode, int signature)
        {
            Compare(addressMode, signature, cpu.A);
        }

        public void Compare(AddressModes addressMode, int signature, Register Reg)
        {
            int val = GetValue(addressMode, signature, Reg.Width);
            if (Reg.Width == 1 && val > 255)
            {
                val = val & 0xFF;
            }

            cpu.Flags.Zero = Reg.Value == val;
            cpu.Flags.Carry = Reg.Value >= val;
            cpu.Flags.Negative = Reg.Value < val;
        }

        /// <summary>
        /// Subtract value from accumulator. Carry acts as a "borrow". When Carry is 0,
        /// subtract one more from Accumulator. Carry will be 0 if result < 0 and 1 if result >= 0.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteSBC(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, cpu.A.Width);

            if (cpu.Flags.Decimal)
                val = HexVal(BCDVal(cpu.A.Value) - BCDVal(val+1) + cpu.Flags.CarryBit);
            else
                val = cpu.A.Value - val - 1 + cpu.Flags.CarryBit;

            cpu.Flags.Carry = (val >= 0 && val <= cpu.A.MaxUnsigned);
            cpu.Flags.oVerflow = (val < cpu.A.MinSigned || val > cpu.A.MaxSigned);
            cpu.Flags.SetNZ(val, cpu.A.Width);

            cpu.A.Value = val;
        }

        public void ExecuteWAI(byte Instruction, AddressModes AddressMode, int Signature)
        {
            cpu.Waiting = true;
        }
    }
}
