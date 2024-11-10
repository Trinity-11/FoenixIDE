using System;
using FoenixIDE.MemoryLocations;
using FoenixIDE.Processor;
using FoenixIDE.Simulator.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * These are ver low level tests.  They allow us to find processor bugs more effectively.
 */
namespace FoenixIDETester
{
    [TestClass]
    public class CpuTests
    {
        CPU cpu;
        MemoryManager MemMgr;

        [TestInitialize]
        public void Setup()
        {
            MemMgr = new MemoryManager
            {
                RAM = new MemoryRAM(0, 3 * 0x20_0000),
                CODEC = new CodecRAM(MemoryMap.CODEC_START_FMX, 4),
                MATH = new MathCoproRegister(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1),
                INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4),
                TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG, 8),
                TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG, 8),
                TIMER2 = new TimerRegister(MemoryMap.TIMER2_CTRL_REG, 8)
            };
            cpu = new CPU(MemMgr, 14_000_000, false);
            cpu.SetEmulationMode();
            Assert.AreEqual(1, cpu.A.Width);
            Assert.AreEqual(1, cpu.X.Width);
        }

        [TestMethod]
        public void ClearCarry()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Carry);
        }


        /*
            # error reported on Foenix Forum by chibiakumas
          	lda #1
	        sta z_B
        infloop:	
	        lda #255
	        adc z_B
	        sta z_C
	        jmp infloop
         */
        [TestMethod]
        public void RunCarrySetTest()
        {
            cpu.PC = 0;
            // lda #1
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            MemMgr.RAM.WriteByte(cpu.PC + 1, 1); // #1
            cpu.ExecuteNext();
            // sta z_B
            byte z_B = 0x20;
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_DirectPage);
            MemMgr.RAM.WriteByte(cpu.PC + 1, z_B);
            cpu.ExecuteNext();
            // lda #255
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            MemMgr.RAM.WriteByte(cpu.PC + 1, 255); // #255
            cpu.ExecuteNext();

            // adc z_B
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_DirectPage);
            MemMgr.RAM.WriteByte(cpu.PC + 1, z_B);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Zero);
            Assert.IsTrue(cpu.Flags.Carry);

            // sta z_C
            byte z_C = 0x21;
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_DirectPage);
            MemMgr.RAM.WriteByte(cpu.PC + 1, z_C);
            cpu.ExecuteNext();
            Assert.AreEqual(0, MemMgr.RAM.ReadByte(z_C));
        }

        // CLC
        // XCE
        // REP #$30
        // LDA #$234
        // TCS
        // LDA #$123
        // STA $237

        // LDA #$FE23
        // LDY #$10
        // STA (3,s),Y - writes $23 at $234 and $FE at $235
        [TestMethod]
        public void RunStackIndirectWithIndex()
        {
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);
            Assert.IsTrue(cpu.Flags.Carry);

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();

            Assert.AreEqual(2, cpu.A.Width);
            Assert.AreEqual(2, cpu.X.Width);

            // LDA #$234
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x234);
            cpu.ExecuteNext();
            Assert.AreEqual(0x234, cpu.A.Value);
            Assert.AreEqual(0, cpu.S.Value);

            // TCS - exchange accumulator with stack
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.TCS_Implied);
            cpu.ExecuteNext();
            Assert.AreEqual(0x234, cpu.S.Value);

            // LDA #$123
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x123);
            cpu.ExecuteNext();
            Assert.AreEqual(0x123, cpu.A.Value);

            // STA $237
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_Absolute);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x237);
            cpu.ExecuteNext();

            // LDA #$678
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x678);
            cpu.ExecuteNext();
            Assert.AreEqual(0x678, cpu.A.Value);

            // STA $239
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_Absolute);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x239);
            cpu.ExecuteNext();

            // LDY #$10
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDY_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x10);
            cpu.ExecuteNext();
            Assert.AreEqual(0x10, cpu.Y.Value);

            // LDA #$FE23
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xFE23);
            cpu.ExecuteNext();
            Assert.AreEqual(0xFE23, cpu.A.Value);
            // STA (3,s),y - store
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_StackRelativeIndirectIndexedWithY);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 3);
            cpu.ExecuteNext();
            Assert.AreEqual(0x23, MemMgr.RAM.ReadByte(0x133));
            Assert.AreEqual(0xFE, MemMgr.RAM.ReadByte(0x134));
        }

        /*
         * LDY #$98
         * CPY #0
         * - check that N flag is 1
         */
        [TestMethod]
        public void CompareIndexSetsNegative()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDY_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x98);
            cpu.ExecuteNext();

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CPY_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Negative); // most significan bit is set
            Assert.IsFalse(cpu.Flags.Zero);
            Assert.IsTrue(cpu.Flags.Carry); // no borrow required

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CPY_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x9A);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Negative); // most significan bit is set
            Assert.IsFalse(cpu.Flags.Zero);
            Assert.IsFalse(cpu.Flags.Carry); // borrow required
        }

        /*
         * LDA #$E9
         * SEC
         * SBC #$39
         * - carry should not be set
         * 
         * SEC
         * SBC #$C0
         * - carry should be set
         */
        [TestMethod]
        public void Substract()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xE9);
            cpu.ExecuteNext();

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x39);
            cpu.ExecuteNext();
            Assert.AreEqual(0xB0, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xc0);
            cpu.ExecuteNext();
            Assert.AreEqual(0xF0, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry);
        }

        /*
         * .asxs
             ldx #0
             lda #$e9
             sec
             sbc _foo,x
             bcc _clear
             lda #'s'
             bra _do
            _clear lda #'c'
            _do sta $afa100
             bra _do
            _foo .byte $39
            */
        [TestMethod]
        public void SubstractIndexed()
        {
            byte foo = 0x39;
            byte bar = 0x93;
            // This is a page 0 address
            byte foo_address = 0xA0;
            MemMgr.RAM.WriteByte(foo_address, foo);
            MemMgr.RAM.WriteByte(foo_address + 1, bar);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDX_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0);
            cpu.ExecuteNext();

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xE9);
            cpu.ExecuteNext();

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_AbsoluteIndexedWithX);
            MemMgr.RAM.WriteByte(cpu.PC + 1, foo_address);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);
        }

        /*
         * Store $EE55 at effective address $2_0203 + $125 = $2:0328
         * LDA #2
         * PHA
         * PLB
         * REP #$10 ; setxl
         * LDX #$125
         * LDY $203,b,X
         */
        [TestMethod]
        public void AbsoluteIndexedByBank()
        {
            MemMgr.RAM.WriteWord(0x2_0328, 0xEE55);

            // Go native
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);

            // Set A short (X is long)
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x20);
            cpu.ExecuteNext();
            Assert.AreEqual(1, cpu.A.Width);
            Assert.AreEqual(2, cpu.X.Width);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 2);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Value);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.PHA_StackImplied);
            cpu.ExecuteNext();

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.PLB_StackImplied);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.DataBank.Value);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDX_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x125);
            cpu.ExecuteNext();
            Assert.AreEqual(0x125, cpu.X.Value);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDY_AbsoluteIndexedWithX);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x203);
            cpu.ExecuteNext();
            Assert.AreEqual(0xEE55, cpu.Y.Value);
        }

        /*
         * http://www.6502.org/tutorials/65c816opcodes.html#6.1.2.2
         * LDA #$43
         * DBR $12
         * address $12ABCD = $9C
         * After BIT $ABCD, N=1 V=0, Z=1
         */
        [TestMethod]
        public void TestBit()
        {
            cpu.A.Value = 0x43;

            cpu.SetEmulationMode();
            cpu.DataBank.Value = 0x12;
            // set the value in memory
            MemMgr.RAM.WriteByte(0x12_ABCD, 0x9C);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.BIT_Absolute);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xABCD);

            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Negative);
            Assert.IsTrue(cpu.Flags.Zero);
            Assert.IsFalse(cpu.Flags.oVerflow);
        }

        /*
         * http://www.6502.org/tutorials/65c816opcodes.html#6.10.1
         *
         * Accumulator $1234
         * X is $ABCD
         * in emulation
         * After TXA, A contains $12CD
         * N=1, Z=0
         */
        [TestMethod]
        public void TestTransfer()
        {
            cpu.Flags.accumulatorShort = true;
            cpu.A.Value = 0x1234;
            cpu.X.Value = 0xABCD;

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.TXA_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Negative);
            Assert.IsFalse(cpu.Flags.Zero);
        }

        [TestMethod]
        public void CheckLargInx()
        {
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();

            Assert.AreEqual(2, cpu.X.Width);

            cpu.X.Value = 0xFFFF;
            Assert.AreEqual(0xFFFF, cpu.X.Value);
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.INX_Implied);
            cpu.ExecuteNext();
            Assert.AreEqual(0, cpu.X.Value);
        }

        /**
         * Bug reported by @dwsJason on 12/02/2023 in discord server - foenix_ide_dev
            clc
            xce
            rep #$30
            lda #$1234     ; A contain 34, and B contains 12
            sep #$20
            inc
            ; now B is 0, so if I rep #$30,  A is $0034, but it should still be $1234 
        */
        [TestMethod]
        public void CheckContentOfBIsValid()
        {
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();

            // LDA #$1234
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1234);
            cpu.ExecuteNext();

            // SEP #$20
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x20);
            cpu.ExecuteNext();

            // INC A
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.INC_Accumulator);
            cpu.ExecuteNext();

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();
            Assert.AreEqual(0x1235, cpu.A.Value);
        }

        /** 
         * Bug reported by @Minstrel Dragon on Discord
         * 
         * I'll perform the test in 6502 mode - Decimal Subtraction
         * 
         * sed
         * sec
         * lda #$55
         * sbc #$61
         * 
         * A register should contain #$94 
         */
        [TestMethod]
        public void SubstractBCDNegative()
        {
            ClearCarry();
            // SED - switch to decimal
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SED_Implied);
            cpu.ExecuteNext();

            // SEC - set the carry bit
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();

            // LDA #$55
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x55);
            cpu.ExecuteNext();

            // SBC #$61
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x61);
            cpu.ExecuteNext();

            // The result should be #$94
            Assert.AreEqual(0x94, cpu.A.Value);
        }

        /** 
         * Bug reported by @Minstrel Dragon on Discord
         * 
         * I'll perform the test in 6502 mode - Decimal Subtraction
         * Need double byte to perform $200 - 1 = $199
         * 
         */
        [TestMethod]
        public void SubstractBCDWord200minus1()
        {
            ClearCarry();
            // SED - switch to decimal
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SED_Implied);
            cpu.ExecuteNext();

            // SEC - set the carry bit
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();

            // LDA #0
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();

            // SBC #1
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();

            // The result should be #2
            Assert.AreEqual(0x99, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry);

            // So the carry should be forwarded to the next SBC operation
            // LDA #2
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x2);
            cpu.ExecuteNext();

            // SBC #0
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();
            // The result should be #2
            Assert.AreEqual(0x1, cpu.A.Value);
        }
        /** 
         * Bug reported by @Minstrel Dragon on Discord
         * 
         * I'll perform the test in 6502 mode - Decimal Subtraction
         * Need double byte to perform $100 - 1 = $99
         * 
         */
        [TestMethod]
        public void SubstractBCDWord100minus1()
        {
            ClearCarry();
            // SED - switch to decimal
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SED_Implied);
            cpu.ExecuteNext();

            // SEC - set the carry bit
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();

            // LDA #0
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();

            // SBC #1
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();

            // The result should be #2
            Assert.AreEqual(0x99, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry);

            // So the carry should be forwarded to the next SBC operation
            // LDA #1
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();

            // SBC #0
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();
            // The result should be #2
            Assert.AreEqual(0x0, cpu.A.Value);
        }

        [TestMethod]
        public void SubstractBCD55minus23()
        {
            ClearCarry();
            // SED - switch to decimal
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SED_Implied);
            cpu.ExecuteNext();

            // SEC - set the carry bit
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();

            // LDA #55
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x55);
            cpu.ExecuteNext();

            // SBC #23
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x23);
            cpu.ExecuteNext();

            // The result should be #2
            Assert.AreEqual(0x32, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry);
        }
    }
}
