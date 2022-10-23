using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Processor
{
    public class OpcodeList
    {
        #region OPCODE constants

        public const int PUSH2 = 16;
        public const int ADD1_16BIT = 32;

        public const int BRK_Interrupt = 0x00;
        public const int ORA_DirectPageIndexedIndirectWithX = 0x01;
        public const int COP_Interrupt = 0x02;
        public const int ORA_StackRelative = 0x03;
        public const int TSB_DirectPage = 0x04;
        public const int ORA_DirectPage = 0x05;
        public const int ASL_DirectPage = 0x06;
        public const int ORA_DirectPageIndirectLong = 0x07;
        // variant
        public const int RMB0_DirectPage = 0x07; // 2 bytes

        public const int PHP_StackImplied = 0x08;
        public const int ORA_Immediate = 0x09;
        public const int ASL_Accumulator = 0x0A;
        public const int PHD_StackImplied = 0x0B;
        public const int TSB_Absolute = 0x0C;
        public const int ORA_Absolute = 0x0D;
        public const int ASL_Absolute = 0x0E;
        public const int ORA_AbsoluteLong = 0x0F;
        // variant
        public const int BBR0_DirectPage = 0x0F; // 3 bytes

        public const int BPL_ProgramCounterRelative = 0x10;
        public const int ORA_DirectPageIndirectIndexedWithY = 0x11;
        public const int ORA_DirectPageIndirect = 0x12;
        public const int ORA_StackRelativeIndirectIndexedWithY = 0x13;
        public const int TRB_DirectPage = 0x14;
        public const int ORA_DirectPageIndexedWithX = 0x15;
        public const int ASL_DirectPageIndexedWithX = 0x16;
        public const int ORA_DirectPageIndirectLongIndexedWithY = 0x17;
        // variant
        public const int RMB1_DirectPage = 0x17; // 2 bytes

        public const int CLC_Implied = 0x18;
        public const int ORA_AbsoluteIndexedWithY = 0x19;
        public const int INC_Accumulator = 0x1A;
        public const int TCS_Implied = 0x1B;
        public const int TRB_Absolute = 0x1C;
        public const int ORA_AbsoluteIndexedWithX = 0x1D;
        public const int ASL_AbsoluteIndexedWithX = 0x1E;
        public const int ORA_AbsoluteLongIndexedWithX = 0x1F;
        // variant
        public const int BBR1_DirectPage = 0x1F; // 3 bytes

        public const int JSR_Absolute = 0x20;
        public const int AND_DirectPageIndexedIndirectWithX = 0x21;
        public const int JSR_AbsoluteLong = 0x22;
        public const int AND_StackRelative = 0x23;
        public const int BIT_DirectPage = 0x24;
        public const int AND_DirectPage = 0x25;
        public const int ROL_DirectPage = 0x26;
        public const int AND_DirectPageIndirectLong = 0x27;
        // variant
        public const int RMB2_DirectPage = 0x27; // 2 bytes

        public const int PLP_StackImplied = 0x28;
        public const int AND_Immediate = 0x29;
        public const int ROL_Accumulator = 0x2A;
        public const int PLD_StackImplied = 0x2B;
        public const int BIT_Absolute = 0x2C;
        public const int AND_Absolute = 0x2D;
        public const int ROL_Absolute = 0x2E;
        public const int AND_AbsoluteLong = 0x2F;
        // variant
        public const int BBR2_DirectPage = 0x2F; // 3 bytes

        public const int BMI_ProgramCounterRelative = 0x30;
        public const int AND_DirectPageIndirectIndexedWithY = 0x31;
        public const int AND_DirectPageIndirect = 0x32;
        public const int AND_StackRelativeIndirectIndexedWithY = 0x33;
        public const int BIT_DirectPageIndexedWithX = 0x34;
        public const int AND_DirectPageIndexedWithX = 0x35;
        public const int ROL_DirectPageIndexedWithX = 0x36;
        public const int AND_DirectPageIndirectLongIndexedWithY = 0x37;
        // variant
        public const int RMB3_DirectPage = 0x37; // 2 bytes

        public const int SEC_Implied = 0x38;
        public const int AND_AbsoluteIndexedWithY = 0x39;
        public const int DEC_Accumulator = 0x3A;
        public const int TSC_Implied = 0x3B;
        public const int BIT_AbsoluteIndexedWithX = 0x3C;
        public const int AND_AbsoluteIndexedWithX = 0x3D;
        public const int ROL_AbsoluteIndexedWithX = 0x3E;
        public const int AND_AbsoluteLongIndexedWithX = 0x3F;
        // variant
        public const int BBR3_DirectPage = 0x3F; // 3 bytes

        public const int RTI_StackImplied = 0x40;
        public const int EOR_DirectPageIndexedIndirectWithX = 0x41;
        public const int WDM_Implied = 0x42;
        public const int EOR_StackRelative = 0x43;
        public const int MVP_BlockMove = 0x44;
        public const int EOR_DirectPage = 0x45;
        public const int LSR_DirectPage = 0x46;
        public const int EOR_DirectPageIndirectLong = 0x47;
        // variant
        public const int RMB4_DirectPage = 0x47; // 2 bytes

        public const int PHA_StackImplied = 0x48;
        public const int EOR_Immediate = 0x49;
        public const int LSR_Accumulator = 0x4A;
        public const int PHK_StackImplied = 0x4B;
        public const int JMP_Absolute = 0x4C;
        public const int EOR_Absolute = 0x4D;
        public const int LSR_Absolute = 0x4E;
        public const int EOR_AbsoluteLong = 0x4F;
        // variant
        public const int BBR4_DirectPage = 0x4F; // 3 bytes

        public const int BVC_ProgramCounterRelative = 0x50;
        public const int EOR_DirectPageIndirectIndexedWithY = 0x51;
        public const int EOR_DirectPageIndirect = 0x52;
        public const int EOR_StackRelativeIndirectIndexedWithY = 0x53;
        public const int MVN_BlockMove = 0x54;
        public const int EOR_DirectPageIndexedWithX = 0x55;
        public const int LSR_DirectPageIndexedWithX = 0x56;
        public const int EOR_DirectPageIndirectLongIndexedWithY = 0x57;
        // variant
        public const int RMB5_DirectPage = 0x57; // 2 bytes

        public const int CLI_Implied = 0x58;
        public const int EOR_AbsoluteIndexedWithY = 0x59;
        public const int PHY_StackImplied = 0x5A;
        public const int TCD_Implied = 0x5B;
        public const int JMP_AbsoluteLong = 0x5C;
        public const int EOR_AbsoluteIndexedWithX = 0x5D;
        public const int LSR_AbsoluteIndexedWithX = 0x5E;
        public const int EOR_AbsoluteLongIndexedWithX = 0x5F;
        // variant
        public const int BBR5_DirectPage = 0x5F; // 3 bytes

        public const int RTS_StackImplied = 0x60;
        public const int ADC_DirectPageIndexedIndirectWithX = 0x61;
        public const int PER_StackProgramCounterRelativeLong = 0x62;
        public const int ADC_StackRelative = 0x63;
        public const int STZ_DirectPage = 0x64;
        public const int ADC_DirectPage = 0x65;
        public const int ROR_DirectPage = 0x66;
        public const int ADC_DirectPageIndirectLong = 0x67;
        // variant
        public const int RMB6_DirectPage = 0x67; // 2 bytes

        public const int PLA_StackImplied = 0x68;
        public const int ADC_Immediate = 0x69;
        public const int ROR_Accumulator = 0x6A;
        public const int RTL_StackImplied = 0x6B;
        public const int JMP_AbsoluteIndirect = 0x6C;
        public const int ADC_Absolute = 0x6D;
        public const int ROR_Absolute = 0x6E;
        public const int ADC_AbsoluteLong = 0x6F;
        // variant
        public const int BBR6_DirectPage = 0x6F; // 3 bytes

        public const int BVS_ProgramCounterRelative = 0x70;
        public const int ADC_DirectPageIndirectIndexedWithY = 0x71;
        public const int ADC_DirectPageIndirect = 0x72;
        public const int ADC_StackRelativeIndirectIndexedWithY = 0x73;
        public const int STZ_DirectPageIndexedWithX = 0x74;
        public const int ADC_DirectPageIndexedWithX = 0x75;
        public const int ROR_DirectPageIndexedWithX = 0x76;
        public const int ADC_DirectPageIndirectLongIndexedWithY = 0x77;
        // variant
        public const int RMB7_DirectPage = 0x77; // 2 bytes

        public const int SEI_Implied = 0x78;
        public const int ADC_AbsoluteIndexedWithY = 0x79;
        public const int PLY_StackImplied = 0x7A;
        public const int TDC_Implied = 0x7B;
        public const int JMP_AbsoluteIndexedIndirectWithX = 0x7C;
        public const int ADC_AbsoluteIndexedWithX = 0x7D;
        public const int ROR_AbsoluteIndexedWithX = 0x7E;
        public const int ADC_AbsoluteLongIndexedWithX = 0x7F;
        // variant
        public const int BBR7_DirectPage = 0x7F; // 3 bytes

        public const int BRA_ProgramCounterRelative = 0x80;
        public const int STA_DirectPageIndexedIndirectWithX = 0x81;
        public const int BRL_ProgramCounterRelativeLong = 0x82;
        public const int STA_StackRelative = 0x83;
        public const int STY_DirectPage = 0x84;
        public const int STA_DirectPage = 0x85;
        public const int STX_DirectPage = 0x86;
        public const int STA_DirectPageIndirectLong = 0x87;
        // variant
        public const int SMB0_DirectPage = 0x87; // 2 bytes

        public const int DEY_Implied = 0x88;
        public const int BIT_Immediate = 0x89;
        public const int TXA_Implied = 0x8A;
        public const int PHB_StackImplied = 0x8B;
        public const int STY_Absolute = 0x8C;
        public const int STA_Absolute = 0x8D;
        public const int STX_Absolute = 0x8E;
        public const int STA_AbsoluteLong = 0x8F;
        // variant
        public const int BBS0_DirectPage = 0x8F; // 3 bytes

        public const int BCC_ProgramCounterRelative = 0x90;
        public const int STA_DirectPageIndirectIndexedWithY = 0x91;
        public const int STA_DirectPageIndirect = 0x92;
        public const int STA_StackRelativeIndirectIndexedWithY = 0x93;
        public const int STY_DirectPageIndexedWithX = 0x94;
        public const int STA_DirectPageIndexedWithX = 0x95;
        public const int STX_DirectPageIndexedWithY = 0x96;
        public const int STA_DirectPageIndirectLongIndexedWithY = 0x97;
        // variant
        public const int SMB1_DirectPage = 0x97; // 2 bytes

        public const int TYA_Implied = 0x98;
        public const int STA_AbsoluteIndexedWithY = 0x99;
        public const int TXS_Implied = 0x9A;
        public const int TXY_Implied = 0x9B;
        public const int STZ_Absolute = 0x9C;
        public const int STA_AbsoluteIndexedWithX = 0x9D;
        public const int STZ_AbsoluteIndexedWithX = 0x9E;
        public const int STA_AbsoluteLongIndexedWithX = 0x9F;
        // variant
        public const int BBS1_DirectPage = 0x9F; // 3 bytes

        public const int LDY_Immediate = 0xA0;
        public const int LDA_DirectPageIndexedIndirectWithX = 0xA1;
        public const int LDX_Immediate = 0xA2;
        public const int LDA_StackRelative = 0xA3;
        public const int LDY_DirectPage = 0xA4;
        public const int LDA_DirectPage = 0xA5;
        public const int LDX_DirectPage = 0xA6;
        public const int LDA_DirectPageIndirectLong = 0xA7;
        // variant
        public const int SMB2_DirectPage = 0xA7; // 2 bytes

        public const int TAY_Implied = 0xA8;
        public const int LDA_Immediate = 0xA9;
        public const int TAX_Implied = 0xAA;
        public const int PLB_StackImplied = 0xAB;
        public const int LDY_Absolute = 0xAC;
        public const int LDA_Absolute = 0xAD;
        public const int LDX_Absolute = 0xAE;
        public const int LDA_AbsoluteLong = 0xAF;
        // variant
        public const int BBS2_DirectPage = 0xAF; // 3 bytes

        public const int BCS_ProgramCounterRelative = 0xB0;
        public const int LDA_DirectPageIndirectIndexedWithY = 0xB1;
        public const int LDA_DirectPageIndirect = 0xB2;
        public const int LDA_StackRelativeIndirectIndexedWithY = 0xB3;
        public const int LDY_DirectPageIndexedWithX = 0xB4;
        public const int LDA_DirectPageIndexedWithX = 0xB5;
        public const int LDX_DirectPageIndexedWithY = 0xB6;
        public const int LDA_DirectPageIndirectLongIndexedWithY = 0xB7;
        // variant
        public const int SMB3_DirectPage = 0xB7; // 2 bytes

        public const int CLV_Implied = 0xB8;
        public const int LDA_AbsoluteIndexedWithY = 0xB9;
        public const int TSX_Implied = 0xBA;
        public const int TYX_Implied = 0xBB;
        public const int LDY_AbsoluteIndexedWithX = 0xBC;
        public const int LDA_AbsoluteIndexedWithX = 0xBD;
        public const int LDX_AbsoluteIndexedWithY = 0xBE;
        public const int LDA_AbsoluteLongIndexedWithX = 0xBF;
        // variant
        public const int BBS3_DirectPage = 0xBF; // 3 bytes

        public const int CPY_Immediate = 0xC0;
        public const int CMP_DirectPageIndexedIndirectWithX = 0xC1;
        public const int REP_Immediate = 0xC2;
        public const int CMP_StackRelative = 0xC3;
        public const int CPY_DirectPage = 0xC4;
        public const int CMP_DirectPage = 0xC5;
        public const int DEC_DirectPage = 0xC6;
        public const int CMP_DirectPageIndirectLong = 0xC7;
        // variant
        public const int SMB4_DirectPage = 0xC7; // 2 bytes

        public const int INY_Implied = 0xC8;
        public const int CMP_Immediate = 0xC9;
        public const int DEX_Implied = 0xCA;
        public const int WAI_Implied = 0xCB;
        public const int CPY_Absolute = 0xCC;
        public const int CMP_Absolute = 0xCD;
        public const int DEC_Absolute = 0xCE;
        public const int CMP_AbsoluteLong = 0xCF;
        // variant
        public const int BBS4_DirectPage = 0xCF; // 3 bytes

        public const int BNE_ProgramCounterRelative = 0xD0;
        public const int CMP_DirectPageIndirectIndexedWithY = 0xD1;
        public const int CMP_DirectPageIndirect = 0xD2;
        public const int CMP_StackRelativeIndirectIndexedWithY = 0xD3;
        public const int PEI_StackDirectPageIndirect = 0xD4;
        public const int CMP_DirectPageIndexedWithX = 0xD5;
        public const int DEC_DirectPageIndexedWithX = 0xD6;
        public const int CMP_DirectPageIndirectLongIndexedWithY = 0xD7;
        // variant
        public const int SMB5_DirectPage = 0xD7; // 2 bytes

        public const int CLD_Implied = 0xD8;
        public const int CMP_AbsoluteIndexedWithY = 0xD9;
        public const int PHX_StackImplied = 0xDA;
        public const int STP_Implied = 0xDB;
        public const int JMP_AbsoluteIndirectLong = 0xDC;
        public const int CMP_AbsoluteIndexedWithX = 0xDD;
        public const int DEC_AbsoluteIndexedWithX = 0xDE;
        public const int CMP_AbsoluteLongIndexedWithX = 0xDF;
        // variant
        public const int BBS5_DirectPage = 0xDF; // 3 bytes

        public const int CPX_Immediate = 0xE0;
        public const int SBC_DirectPageIndexedIndirectWithX = 0xE1;
        public const int SEP_Immediate = 0xE2;
        public const int SBC_StackRelative = 0xE3;
        public const int CPX_DirectPage = 0xE4;
        public const int SBC_DirectPage = 0xE5;
        public const int INC_DirectPage = 0xE6;
        public const int SBC_DirectPageIndirectLong = 0xE7;
        // variant
        public const int SMB6_DirectPage = 0xE7; // 2 bytes

        public const int INX_Implied = 0xE8;
        public const int SBC_Immediate = 0xE9;
        public const int NOP_Implied = 0xEA;
        public const int XBA_Implied = 0xEB;
        public const int CPX_Absolute = 0xEC;
        public const int SBC_Absolute = 0xED;
        public const int INC_Absolute = 0xEE;
        public const int SBC_AbsoluteLong = 0xEF;
        // variant
        public const int BBS6_DirectPage = 0xEF; // 3 bytes

        public const int BEQ_ProgramCounterRelative = 0xF0;
        public const int SBC_DirectPageIndirectIndexedWithY = 0xF1;
        public const int SBC_DirectPageIndirect = 0xF2;
        public const int SBC_StackRelativeIndirectIndexedWithY = 0xF3;
        public const int PEA_StackAbsolute = 0xF4;
        public const int SBC_DirectPageIndexedWithX = 0xF5;
        public const int INC_DirectPageIndexedWithX = 0xF6;
        public const int SBC_DirectPageIndirectLongIndexedWithY = 0xF7;
        // variant
        public const int SMB7_DirectPage = 0xF7; // 2 bytes

        public const int SED_Implied = 0xF8;
        public const int SBC_AbsoluteIndexedWithY = 0xF9;
        public const int PLX_StackImplied = 0xFA;
        public const int XCE_Implied = 0xFB;
        public const int JSR_AbsoluteIndexedIndirectWithX = 0xFC;
        public const int SBC_AbsoluteIndexedWithX = 0xFD;
        public const int INC_AbsoluteIndexedWithX = 0xFE;
        public const int SBC_AbsoluteLongIndexedWithX = 0xFF;
        // variant
        public const int BBS7_DirectPage = 0xFF; // 3 bytes

        #endregion constants
        readonly OpCode[] list = new OpCode[256];

        public OpCode this[byte index]
        {
            get => list[index];
        }

        // This is pretty ugly to look at - each opcode for each processor has to be correct
        public OpcodeList(Operations operations, CPU CPU, bool is6502)
        {
            list[0x00] = new OpCode(0x00, "BRK", 2, AddressModes.Interrupt, new OpCode.ExecuteDelegate(operations.ExecuteInterrupt));
            list[0x01] = new OpCode(0x01, "ORA", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteORA));

            list[0x04] = new OpCode(0x04, "TSB", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB));
            list[0x05] = new OpCode(0x05, "ORA", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0x06] = new OpCode(0x06, "ASL", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            
            list[0x08] = new OpCode(0x08, "PHP", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0x09] = new OpCode(0x09, "ORA", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0xA] = new OpCode(0x0A, "ASL", 1, AddressModes.Accumulator, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            
            list[0xC] = new OpCode(0x0C, "TSB", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB));
            list[0xD] = new OpCode(0x0D, "ORA", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0xE] = new OpCode(0x0E, "ASL", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            
            list[0x10] = new OpCode(0x10, "BPL", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0x11] = new OpCode(0x11, "ORA", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0x12] = new OpCode(0x12, "ORA", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteORA));

            list[0x14] = new OpCode(0x14, "TRB", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB));
            list[0x15] = new OpCode(0x15, "ORA", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0x16] = new OpCode(0x16, "ASL", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            
            list[0x18] = new OpCode(0x18, "CLC", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0x19] = new OpCode(0x19, "ORA", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0x1A] = new OpCode(0x1A, "INC", 1, AddressModes.Accumulator, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            // No TCS - 1B
            list[0x1C] = new OpCode(0x1C, "TRB", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB));
            list[0x1D] = new OpCode(0x1D, "ORA", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteORA));
            list[0x1E] = new OpCode(0x1E, "ASL", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No ORA Absolute Long
            list[0x20] = new OpCode(0x20, "JSR", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x21] = new OpCode(0x21, "AND", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            // No JSR
            // No AND Stack Rel
            list[0x24] = new OpCode(0x24, "BIT", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteBIT));
            list[0x25] = new OpCode(0x25, "AND", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x26] = new OpCode(0x26, "ROL", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No AND DP indirect long
            list[0x28] = new OpCode(0x28, "PLP", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0x29] = new OpCode(0x29, "AND", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x2A] = new OpCode(0x2A, "ROL", 1, AddressModes.Accumulator, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No PLD
            list[0x2C] = new OpCode(0x2C, "BIT", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteBIT));
            list[0x2D] = new OpCode(0x2D, "AND", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x2E] = new OpCode(0x2E, "ROL", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No AND Abs Long
            list[0x30] = new OpCode(0x30, "BMI", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0x31] = new OpCode(0x31, "AND", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x32] = new OpCode(0x32, "AND", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            // No AND SR Ind Indexed
            list[0x34] = new OpCode(0x34, "BIT", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteBIT));
            list[0x35] = new OpCode(0x35, "AND", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x36] = new OpCode(0x36, "ROL", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No AND DP Ind Long
            list[0x38] = new OpCode(0x38, "SEC", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0x39] = new OpCode(0x39, "AND", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x3A] = new OpCode(0x3A, "DEC", 1, AddressModes.Accumulator, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            // No TSC
            list[0x3C] = new OpCode(0x3C, "BIT", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteBIT));
            list[0x3D] = new OpCode(0x3D, "AND", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteAND));
            list[0x3E] = new OpCode(0x3E, "ROL", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No AND Abs Long
            list[0x40] = new OpCode(0x40, "RTI", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x41] = new OpCode(0x41, "EOR", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            // No WDM
            // No EOR Stack Rel
            // No MVP
            list[0x45] = new OpCode(0x45, "EOR", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x46] = new OpCode(0x46, "LSR", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No EOR Ind Long
            list[0x48] = new OpCode(0x48, "PHA", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0x49] = new OpCode(0x49, "EOR", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x4A] = new OpCode(0x4A, "LSR", 1, AddressModes.Accumulator, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No PHK
            list[0x4C] = new OpCode(0x4C, "JMP", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x4D] = new OpCode(0x4D, "EOR", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x4E] = new OpCode(0x4E, "LSR", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // No EOR Abs Long - 4F
            list[0x50] = new OpCode(0x50, "BVC", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0x51] = new OpCode(0x51, "EOR", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x52] = new OpCode(0x52, "EOR", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            //  No 0x53, "EOR", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            //  No 0x54, "MVN", 3, AddressModes.BlockMove, new OpCode.ExecuteDelegate(operations.ExecuteBlockMove));
            list[0x55] = new OpCode(0x55, "EOR", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x56] = new OpCode(0x56, "LSR", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            //  No 0x57, "EOR", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x58] = new OpCode(0x58, "CLI", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0x59] = new OpCode(0x59, "EOR", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x5A] = new OpCode(0x5A, "PHY", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));

            //  No 0x5B, "TCD", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            //  No 0x5C, "JMP", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x5D] = new OpCode(0x5D, "EOR", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x5E] = new OpCode(0x5E, "LSR", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            //  No 0x5F, "EOR", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
            list[0x60] = new OpCode(0x60, "RTS", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x61] = new OpCode(0x61, "ADC", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            //  No 0x62, "PER", 3, AddressModes.StackProgramCounterRelativeLong, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            //  No 0x63, "ADC", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x64] = new OpCode(0x64, "STZ", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteSTZ));
            list[0x65] = new OpCode(0x65, "ADC", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x66] = new OpCode(0x66, "ROR", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            // list[0x21] = new OpCode(0x67, "ADC", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x68] = new OpCode(0x68, "PLA", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0x69] = new OpCode(0x69, "ADC", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x6A] = new OpCode(0x6A, "ROR", 1, AddressModes.Accumulator, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            //  No 0x6B, "RTL", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x6C] = new OpCode(0x6C, "JMP", 3, AddressModes.JmpAbsoluteIndirect, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x6D] = new OpCode(0x6D, "ADC", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x6E] = new OpCode(0x6E, "ROR", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            //  No 0x6F, "ADC", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x70] = new OpCode(0x70, "BVS", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0x71] = new OpCode(0x71, "ADC", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x72] = new OpCode(0x72, "ADC", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            //  No 0x73, "ADC", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x74] = new OpCode(0x74, "STZ", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTZ));
            list[0x75] = new OpCode(0x75, "ADC", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x76] = new OpCode(0x76, "ROR", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            //  No 0x77, "ADC", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x78] = new OpCode(0x78, "SEI", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0x79] = new OpCode(0x79, "ADC", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x7A] = new OpCode(0x7A, "PLY", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            //  No 0x7B, "TDC", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            list[0x7C] = new OpCode(0x7C, "JMP", 3, AddressModes.JmpAbsoluteIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0x7D] = new OpCode(0x7D, "ADC", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x7E] = new OpCode(0x7E, "ROR", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteShift));
            //  No 0x7F, "ADC", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteADC));
            list[0x80] = new OpCode(0x80, "BRA", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0x81] = new OpCode(0x81, "STA", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            //  No 0x82, "BRL", 3, AddressModes.ProgramCounterRelativeLong, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            //  No 0x83, "STA", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x84] = new OpCode(0x84, "STY", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteSTY));
            list[0x85] = new OpCode(0x85, "STA", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x86] = new OpCode(0x86, "STX", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteSTX));
            //  No 0x87, "STA", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x88] = new OpCode(0x88, "DEY", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            list[0x89] = new OpCode(0x89, "BIT", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT));
            list[0x8A] = new OpCode(0x8A, "TXA", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            //  No 0x8B, "PHB", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0x8C] = new OpCode(0x8C, "STY", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteSTY));
            list[0x8D] = new OpCode(0x8D, "STA", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x8E] = new OpCode(0x8E, "STX", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteSTX));
            //  No 0x8F, "STA", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x90] = new OpCode(0x90, "BCC", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0x91] = new OpCode(0x91, "STA", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x92] = new OpCode(0x92, "STA", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            //  No 0x93, "STA", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x94] = new OpCode(0x94, "STY", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTY));
            list[0x95] = new OpCode(0x95, "STA", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x96] = new OpCode(0x96, "STX", 2, AddressModes.DirectPageIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTX));
            //  No 0x97, "STA", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x98] = new OpCode(0x98, "TYA", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            list[0x99] = new OpCode(0x99, "STA", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x9A] = new OpCode(0x9A, "TXS", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            //  No 0x9B, "TXY", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            list[0x9C] = new OpCode(0x9C, "STZ", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteSTZ));
            list[0x9D] = new OpCode(0x9D, "STA", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0x9E] = new OpCode(0x9E, "STZ", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTZ));
            //  No 0x9F, "STA", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
            list[0xA0] = new OpCode(0xA0, "LDY", 2, CPU.Y, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteLDY));
            list[0xA1] = new OpCode(0xA1, "LDA", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xA2] = new OpCode(0xA2, "LDX", 2, CPU.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteLDX));
            //  No 0xA3, "LDA", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xA4] = new OpCode(0xA4, "LDY", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteLDY));
            list[0xA5] = new OpCode(0xA5, "LDA", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xA6] = new OpCode(0xA6, "LDX", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteLDX));
            //  No 0xA7, "LDA", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xA8] = new OpCode(0xA8, "TAY", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            list[0xA9] = new OpCode(0xA9, "LDA", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xAA] = new OpCode(0xAA, "TAX", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            //  No 0xAB, "PLB", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0xAC] = new OpCode(0xAC, "LDY", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteLDY));
            list[0xAD] = new OpCode(0xAD, "LDA", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xAE] = new OpCode(0xAE, "LDX", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteLDX));
            //  No 0xAF, "LDA", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xB0] = new OpCode(0xB0, "BCS", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0xB1] = new OpCode(0xB1, "LDA", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xB2] = new OpCode(0xB2, "LDA", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            //  No 0xB3, "LDA", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xB4] = new OpCode(0xB4, "LDY", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDY));
            list[0xB5] = new OpCode(0xB5, "LDA", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xB6] = new OpCode(0xB6, "LDX", 2, AddressModes.DirectPageIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDX));
            //  No 0xB7, "LDA", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xB8] = new OpCode(0xB8, "CLV", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0xB9] = new OpCode(0xB9, "LDA", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xBA] = new OpCode(0xBA, "TSX", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            //  No 0xBB, "TYX", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
            list[0xBC] = new OpCode(0xBC, "LDY", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDY));
            list[0xBD] = new OpCode(0xBD, "LDA", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xBE] = new OpCode(0xBE, "LDX", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDX));
            //  No 0xBF, "LDA", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
            list[0xC0] = new OpCode(0xC0, "CPY", 2, CPU.Y, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteCPY));
            list[0xC1] = new OpCode(0xC1, "CMP", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            //  No 0xC2, "REP", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            //  No 0xC3, "CMP", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xC4] = new OpCode(0xC4, "CPY", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteCPY));
            list[0xC5] = new OpCode(0xC5, "CMP", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xC6] = new OpCode(0xC6, "DEC", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            //  No 0xC7, "CMP", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xC8] = new OpCode(0xC8, "INY", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            list[0xC9] = new OpCode(0xC9, "CMP", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xCA] = new OpCode(0xCA, "DEX", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            list[0xCB] = new OpCode(0xCB, "WAI", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteWAI));
            list[0xCC] = new OpCode(0xCC, "CPY", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteCPY));
            list[0xCD] = new OpCode(0xCD, "CMP", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xCE] = new OpCode(0xCE, "DEC", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            //  No 0xCF, "CMP", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xD0] = new OpCode(0xD0, "BNE", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0xD1] = new OpCode(0xD1, "CMP", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xD2] = new OpCode(0xD2, "CMP", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            //  No 0xD3, "CMP", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            //  No 0xD4, "PEI", 2, AddressModes.StackDirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0xD5] = new OpCode(0xD5, "CMP", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xD6] = new OpCode(0xD6, "DEC", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            //  No 0xD7, "CMP", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xD8] = new OpCode(0xD8, "CLD", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0xD9] = new OpCode(0xD9, "CMP", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xDA] = new OpCode(0xDA, "PHX", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0xDB] = new OpCode(0xDB, "STP", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteMisc));
            //  No 0xDC, "JMP", 3, AddressModes.JmpAbsoluteIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0xDD] = new OpCode(0xDD, "CMP", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xDE] = new OpCode(0xDE, "DEC", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            //  No 0xDF, "CMP", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
            list[0xE0] = new OpCode(0xE0, "CPX", 2, CPU.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteCPX));
            list[0xE1] = new OpCode(0xE1, "SBC", 2, AddressModes.DirectPageIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            //  No 0xE2, "SEP", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            //  No 0xE3, "SBC", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xE4] = new OpCode(0xE4, "CPX", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteCPX));
            list[0xE5] = new OpCode(0xE5, "SBC", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xE6] = new OpCode(0xE6, "INC", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            //  No 0xE7, "SBC", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xE8] = new OpCode(0xE8, "INX", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            list[0xE9] = new OpCode(0xE9, "SBC", 2, CPU.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xEA] = new OpCode(0xEA, "NOP", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteMisc));
            // No XBA
            list[0xEC] = new OpCode(0xEC, "CPX", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteCPX));
            list[0xED] = new OpCode(0xED, "SBC", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xEE] = new OpCode(0xEE, "INC", 3, AddressModes.Absolute, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            // No SBC
            list[0xF0] = new OpCode(0xF0, "BEQ", 2, AddressModes.ProgramCounterRelative, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
            list[0xF1] = new OpCode(0xF1, "SBC", 2, AddressModes.DirectPageIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xF2] = new OpCode(0xF2, "SBC", 2, AddressModes.DirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            //  No 0xF3, "SBC", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            //  No 0xF4, "PEA", 3, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            list[0xF5] = new OpCode(0xF5, "SBC", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xF6] = new OpCode(0xF6, "INC", 2, AddressModes.DirectPageIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));
            // No SBC
            list[0xF8] = new OpCode(0xF8, "SED", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            list[0xF9] = new OpCode(0xF9, "SBC", 3, AddressModes.AbsoluteIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xFA] = new OpCode(0xFA, "PLX", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
            //  No 0xFB, "XCE", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
            //  No 0xFC, "JSR", 3, AddressModes.JmpAbsoluteIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
            list[0xFD] = new OpCode(0xFD, "SBC", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            list[0xFE] = new OpCode(0xFE, "INC", 3, AddressModes.AbsoluteIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC));

            if (!is6502)
            {
                list[0x02] = new OpCode(0x02, "COP", 2, AddressModes.Interrupt, new OpCode.ExecuteDelegate(operations.ExecuteInterrupt));
                list[0x03] = new OpCode(0x03, "ORA", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteORA));
                list[0x07] = new OpCode(0x07, "ORA", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteORA));
                list[0x0B] = new OpCode(0x0B, "PHD", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0x0F] = new OpCode(0x0F, "ORA", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteORA));
                list[0x13] = new OpCode(0x13, "ORA", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteORA));
                list[0x17] = new OpCode(0x17, "ORA", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteORA));
                list[0x1B] = new OpCode(0x1B, "TCS", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
                list[0x1F] = new OpCode(0x1F, "ORA", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteORA));
                list[0x22] = new OpCode(0x22, "JSR", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
                list[0x23] = new OpCode(0x23, "AND", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteAND));
                list[0x27] = new OpCode(0x27, "AND", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteAND));
                list[0x2B] = new OpCode(0x2B, "PLD", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0x2F] = new OpCode(0x2F, "AND", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteAND));
                list[0x33] = new OpCode(0x33, "AND", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteAND));
                list[0x37] = new OpCode(0x37, "AND", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteAND));
                list[0x3B] = new OpCode(0x3B, "TSC", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
                list[0x3F] = new OpCode(0x3F, "AND", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteAND));
                list[0x42] = new OpCode(0x42, "WDM", 2, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteMisc));
                list[0x43] = new OpCode(0x43, "EOR", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
                list[0x44] = new OpCode(0x44, "MVP", 3, AddressModes.BlockMove, new OpCode.ExecuteDelegate(operations.ExecuteBlockMove));
                list[0x47] = new OpCode(0x47, "EOR", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
                list[0x4B] = new OpCode(0x4B, "PHK", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0x4F] = new OpCode(0x4F, "EOR", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
                list[0x53] = new OpCode(0x53, "EOR", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
                list[0x54] = new OpCode(0x54, "MVN", 3, AddressModes.BlockMove, new OpCode.ExecuteDelegate(operations.ExecuteBlockMove));
                list[0x57] = new OpCode(0x57, "EOR", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
                list[0x5B] = new OpCode(0x5B, "TCD", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
                list[0x5C] = new OpCode(0x5C, "JMP", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
                list[0x5F] = new OpCode(0x5F, "EOR", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteEOR));
                list[0x62] = new OpCode(0x62, "PER", 3, AddressModes.StackProgramCounterRelativeLong, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0x63] = new OpCode(0x63, "ADC", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteADC));
                list[0x67] = new OpCode(0x67, "ADC", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteADC));
                list[0x6B] = new OpCode(0x6B, "RTL", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
                list[0x6F] = new OpCode(0x6F, "ADC", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteADC));
                list[0x73] = new OpCode(0x73, "ADC", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteADC));
                list[0x77] = new OpCode(0x77, "ADC", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteADC));
                list[0x7B] = new OpCode(0x7B, "TDC", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
                list[0x7F] = new OpCode(0x7F, "ADC", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteADC));
                list[0x82] = new OpCode(0x82, "BRL", 3, AddressModes.ProgramCounterRelativeLong, new OpCode.ExecuteDelegate(operations.ExecuteBranch));
                list[0x83] = new OpCode(0x83, "STA", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
                list[0x87] = new OpCode(0x87, "STA", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
                list[0x8B] = new OpCode(0x8B, "PHB", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0x8F] = new OpCode(0x8F, "STA", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
                list[0x93] = new OpCode(0x93, "STA", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
                list[0x97] = new OpCode(0x97, "STA", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
                list[0x9B] = new OpCode(0x9B, "TXY", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
                list[0x9F] = new OpCode(0x9F, "STA", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSTA));
                list[0xA3] = new OpCode(0xA3, "LDA", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
                list[0xA7] = new OpCode(0xA7, "LDA", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
                list[0xAB] = new OpCode(0xAB, "PLB", 1, AddressModes.StackImplied, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0xAF] = new OpCode(0xAF, "LDA", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
                list[0xB3] = new OpCode(0xB3, "LDA", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
                list[0xB7] = new OpCode(0xB7, "LDA", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
                list[0xBB] = new OpCode(0xBB, "TYX", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteTransfer));
                list[0xBF] = new OpCode(0xBF, "LDA", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteLDA));
                list[0xC2] = new OpCode(0xC2, "REP", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
                list[0xC3] = new OpCode(0xC3, "CMP", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
                list[0xC7] = new OpCode(0xC7, "CMP", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
                list[0xCF] = new OpCode(0xCF, "CMP", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
                list[0xD3] = new OpCode(0xD3, "CMP", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
                list[0xD4] = new OpCode(0xD4, "PEI", 2, AddressModes.StackDirectPageIndirect, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0xD7] = new OpCode(0xD7, "CMP", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
                list[0xDC] = new OpCode(0xDC, "JMP", 3, AddressModes.JmpAbsoluteIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
                list[0xDF] = new OpCode(0xDF, "CMP", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteCMP));
                list[0xE2] = new OpCode(0xE2, "SEP", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
                list[0xE3] = new OpCode(0xE3, "SBC", 2, AddressModes.StackRelative, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
                list[0xE7] = new OpCode(0xE7, "SBC", 2, AddressModes.DirectPageIndirectLong, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
                list[0xEB] = new OpCode(0xEB, "XBA", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteMisc));
                list[0xEF] = new OpCode(0xEF, "SBC", 4, AddressModes.AbsoluteLong, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
                list[0xF3] = new OpCode(0xF3, "SBC", 2, AddressModes.StackRelativeIndirectIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
                list[0xF4] = new OpCode(0xF4, "PEA", 3, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteStack));
                list[0xF7] = new OpCode(0xF7, "SBC", 2, AddressModes.DirectPageIndirectLongIndexedWithY, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
                list[0xFB] = new OpCode(0xFB, "XCE", 1, AddressModes.Implied, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg));
                list[0xFC] = new OpCode(0xFC, "JSR", 3, AddressModes.JmpAbsoluteIndexedIndirectWithX, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn));
                list[0xFF] = new OpCode(0xFF, "SBC", 4, AddressModes.AbsoluteLongIndexedWithX, new OpCode.ExecuteDelegate(operations.ExecuteSBC));
            }
            else
            {

                // Variants - Rockwell
                list[0x07] = new OpCode(0x07, "RMB0", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x17] = new OpCode(0x17, "RMB1", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x27] = new OpCode(0x27, "RMB2", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x37] = new OpCode(0x37, "RMB3", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x47] = new OpCode(0x47, "RMB4", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x57] = new OpCode(0x57, "RMB5", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x67] = new OpCode(0x67, "RMB6", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));
                list[0x77] = new OpCode(0x77, "RMB7", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.ResetMemoryBit));

                list[0x0F] = new OpCode(0x0F, "BBR0", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x1F] = new OpCode(0x1F, "BBR1", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x2F] = new OpCode(0x2F, "BBR2", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x3F] = new OpCode(0x3F, "BBR3", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x4F] = new OpCode(0x4F, "BBR4", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x5F] = new OpCode(0x5F, "BBR5", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x6F] = new OpCode(0x6F, "BBR6", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));
                list[0x7F] = new OpCode(0x7F, "BBR7", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITReset));

                list[0x87] = new OpCode(0x87, "SMB0", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0x97] = new OpCode(0x97, "SMB1", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0xA7] = new OpCode(0xA7, "SMB2", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0xB7] = new OpCode(0xB7, "SMB3", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0xC7] = new OpCode(0xC7, "SMB4", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0xD7] = new OpCode(0xD7, "SMB5", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0xE7] = new OpCode(0xE7, "SMB6", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));
                list[0xF7] = new OpCode(0xF7, "SMB7", 2, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.SetMemoryBit));

                list[0x8F] = new OpCode(0x8F, "BBS0", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0x9F] = new OpCode(0x9F, "BBS1", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0xAF] = new OpCode(0xAF, "BBS2", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0xBF] = new OpCode(0xBF, "BBS3", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0xCF] = new OpCode(0xCF, "BBS4", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0xDF] = new OpCode(0xDF, "BBS5", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0xEF] = new OpCode(0xEF, "BBS6", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
                list[0xFF] = new OpCode(0xFF, "BBS7", 3, AddressModes.DirectPage, new OpCode.ExecuteDelegate(operations.BranchBITSet));
            }
        }
    }
}
