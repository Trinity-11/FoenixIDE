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
                RAM = new MemoryRAM(0, 3 * 0x20_0000)
            };
            cpu = new CPU(MemMgr);
            cpu.SetEmulationMode();
            Assert.AreEqual(1, cpu.A.Width);
            Assert.AreEqual(1, cpu.X.Width);
        }

        // LDA #$99
        [TestMethod]
        public void LoadAccumulatorWith99()
        {
            // By default the CPU must be in 6502 emulation mode
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x99); // #$99
            int PC = cpu.PC;
            cpu.ExecuteNext();
            Assert.AreEqual(0x99, cpu.A.Value);
            Assert.AreEqual(PC + 2, cpu.PC);
        }
        // CLC
        [TestMethod]
        public void ClearCarry()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Carry);
        }
        // LDA #$99
        // CLC
        // ADC #$78
        // -- overflow and carry are set.
        // Unsigned: $99 + $78 = $11  ==> carry set
        // Signed: -$67 + $78 = $11 ==> overflow false
        [TestMethod]
        public void LoadCheckCarrySetForOverflowAbsolute()
        {
            LoadAccumulatorWith99();
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x78);
            cpu.ExecuteNext();
            Assert.AreEqual(0x11, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
        }
        // LDA #$99
        // CLC
        // ADC $56
        // Unsigned: $99 + $78 = $11 => carry set
        // Signed: -$67 + $78 = $11 ==> overflow false
        [TestMethod]
        public void LoadCheckCarrySetForOverflowDirectPage()
        {
            LoadAccumulatorWith99();
            // Write a value that will cause an overflow in the addition - A is $99
            MemMgr.RAM.WriteByte(0x56, 0x78);
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_DirectPage);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x56);
            cpu.ExecuteNext();
            Assert.AreEqual(0x11, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
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

        /**
         *   Taken from http://www.6502.org/tutorials/vflag.html
        #1   CLC; 1 + 1 = 2, returns C = 0, V = 0
             LDA #$01
             ADC #$01
        */
        [TestMethod]
        public void TestOverflowADC1()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 1);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 1);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry);
            Assert.IsFalse(cpu.Flags.oVerflow);
        }
        /**
         *   Taken from http://www.6502.org/tutorials/vflag.html
         #2   CLC; 1 + -1 = 0, returns C = 1, V = 0
              LDA #$01
              ADC #$FF
        */
        [TestMethod]
        public void TestOverflowADC2()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 1);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
        }
        /**
         *   Taken from http://www.6502.org/tutorials/vflag.html
         #3   CLC; 127 + 1 = 128, returns C = 0, V = 1
              LDA #$7F
              ADC #$01
           */
        [TestMethod]
        public void TestOverflowADC3()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x7F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }

        /**
         *   Taken from http://www.6502.org/tutorials/vflag.html
         #4   CLC; -128 + -1 = -129, returns C = 1, V = 1
              LDA #$80
              ADC #$FF
        */
        [TestMethod]
        public void TestOverflowADC4()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x80);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7F, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }

        /**
         *   Taken from http://www.6502.org/tutorials/vflag.html
         #5   SEC      ; 0 - 1 = -1, returns V = 0
              LDA #$00
              SBC #$01
        */
        [TestMethod]
        public void TestOverflowSBC5()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0xFF, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
        }

        /**
         *   Taken from http://www.6502.org/tutorials/vflag.html
         #6   SEC      ; -128 - 1 = -129, returns V = 1
              LDA #$80
              SBC #$01

                */
        [TestMethod]
        public void TestOverflowSBC6()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x80);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7F, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }

        /**
         * Taken from http://www.6502.org/tutorials/vflag.html
         #7  SEC      ; 127 - -1 = 128, returns V = 1
             LDA #$7F
             SBC #$FF
        */
        [TestMethod]
        public void TestOverflowSBC7()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x7F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }
        /**
         *  Taken from http://www.6502.org/tutorials/vflag.html
         #8   SEC      ; Note: SEC, not CLC
              LDA #$3F ; 63 + 64 + 1 = 128, returns V = 1
              ADC #$40
        */
        [TestMethod]
        public void TestOverflowADC8()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x3F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x40);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }
        /**
         * Taken from http://www.6502.org/tutorials/vflag.html
         #9   CLC      ; Note: CLC, not SEC
              LDA #$C0 ; -64 - 64 - 1 = -129, returns V = 1
              SBC #$40
        */
        [TestMethod]
        public void TestOverflowSBC9()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xC0);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x40);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7F, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }
    }
}
