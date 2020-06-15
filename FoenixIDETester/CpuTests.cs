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
        MemoryManager mgr;

        [TestInitialize]
        public void Setup()
        {
            mgr = new MemoryManager
            {
                RAM = new MemoryRAM(0, 3 * 0x1_0000),  // Only setup 3K of RAM - this should be tons to test our CPU
                CODEC = new CodecRAM(1025,1),
                INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4)
            };
            cpu = new CPU(mgr);
            cpu.SetEmulationMode();
            Assert.AreEqual(1, cpu.A.Width);
            Assert.AreEqual(1, cpu.X.Width);
        }

        // LDA #$99
        [TestMethod]
        public void LoadAccumulatorWith99()
        {
            // By default the CPU must be in 6502 emulation mode
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            mgr.RAM.WriteByte(cpu.PC + 1, 0x99); // #$99
            int PC = cpu.PC;
            cpu.ExecuteNext();
            Assert.AreEqual(0x99, cpu.A.Value);
            Assert.AreEqual(PC + 2, cpu.PC);
        }
        // CLC
        [TestMethod]
        public void ClearCarry()
        {
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Carry);
        }
        // LDA #$99
        // CLC
        // ADC #$78
        // -- overflow and carry are set
        [TestMethod]
        public void LoadCheckCarrySetForOverflowAbsolute()
        {
            LoadAccumulatorWith99();
            ClearCarry();
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0x78);
            cpu.ExecuteNext();
            Assert.AreEqual(0x11, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.oVerflow);
            Assert.IsTrue(cpu.Flags.Carry);
        }
        // LDA #$99
        // CLC
        // ADC $56
        // 
        [TestMethod]
        public void LoadCheckCarrySetForOverflowDirectPage()
        {
            LoadAccumulatorWith99();
            // Write a value that will cause an overflow in the addition - A is $99
            mgr.RAM.WriteByte(0x56, 0x78);
            ClearCarry();
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_DirectPage);
            mgr.RAM.WriteByte(cpu.PC + 1, 0x56);
            cpu.ExecuteNext();
            Assert.AreEqual(0x11, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.oVerflow);
            Assert.IsTrue(cpu.Flags.Carry);
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
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            mgr.RAM.WriteByte(cpu.PC + 1, 1); // #1
            cpu.ExecuteNext();
            // sta z_B
            byte z_B = 0x20;
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_DirectPage);
            mgr.RAM.WriteByte(cpu.PC + 1, z_B);
            cpu.ExecuteNext();
            // lda #255
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            mgr.RAM.WriteByte(cpu.PC + 1, 255); // #255
            cpu.ExecuteNext();
            
            // adc z_B
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_DirectPage);
            mgr.RAM.WriteByte(cpu.PC + 1, z_B);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Zero);
            Assert.IsTrue(cpu.Flags.Carry);

            // sta z_C
            byte z_C = 0x21;
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_DirectPage);
            mgr.RAM.WriteByte(cpu.PC + 1, z_C);
            cpu.ExecuteNext();
            Assert.AreEqual(0, mgr.RAM.ReadByte(z_C));
        }

        // CLC
        // XCE
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
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);
            Assert.IsTrue(cpu.Flags.Carry);
            Assert.AreEqual(2, cpu.A.Width);
            Assert.AreEqual(2, cpu.X.Width);

            // LDA #$234
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x234);
            cpu.ExecuteNext();
            Assert.AreEqual(0x234, cpu.A.Value);
            Assert.AreEqual(0, cpu.S.Value);

            // TCS - exchange accumulator with stack
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.TCS_Implied);
            cpu.ExecuteNext();
            Assert.AreEqual(0x234, cpu.S.Value);

            // LDA #$123
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x123);
            cpu.ExecuteNext();
            Assert.AreEqual(0x123, cpu.A.Value);

            // STA $237
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_Absolute);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x237);
            cpu.ExecuteNext();

            // LDA #$678
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x678);
            cpu.ExecuteNext();
            Assert.AreEqual(0x678, cpu.A.Value);

            // STA $239
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_Absolute);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x239);
            cpu.ExecuteNext();

            // LDY #$10
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDY_Immediate);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x10);
            cpu.ExecuteNext();
            Assert.AreEqual(0x10, cpu.Y.Value);

            // LDA #$FE23
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteWord(cpu.PC + 1, 0xFE23);
            cpu.ExecuteNext();
            Assert.AreEqual(0xFE23, cpu.A.Value);
            // STA (3,s),y - store
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.STA_StackRelativeIndirectIndexedWithY);
            mgr.RAM.WriteByte(cpu.PC + 1, 3);
            cpu.ExecuteNext();
            Assert.AreEqual(0x23, mgr.RAM.ReadByte(0x133));
            Assert.AreEqual(0xFE, mgr.RAM.ReadByte(0x134));
        }

        /*
         * LDY #$98
         * CPY #0
         * - check that N flag is 1
         */
        [TestMethod]
        public void CompareIndexSetsNegative()
        {
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDY_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0x98);
            cpu.ExecuteNext();

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.CPY_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Negative); // most significan bit is set
            Assert.IsFalse(cpu.Flags.Zero);
            Assert.IsTrue(cpu.Flags.Carry); // no borrow required

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.CPY_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0x9A);
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
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0xE9);
            cpu.ExecuteNext();

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0x39);
            cpu.ExecuteNext();
            Assert.AreEqual(0xE9 - 0x39, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0xc0);
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
            mgr.RAM.WriteByte(foo_address, foo);
            mgr.RAM.WriteByte(foo_address + 1, bar);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDX_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0);
            cpu.ExecuteNext();

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0xE9);
            cpu.ExecuteNext();

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_AbsoluteIndexedWithX);
            mgr.RAM.WriteByte(cpu.PC + 1, foo_address);
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
            mgr.RAM.WriteWord(0x2_0328, 0xEE55);

            // Go native
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);

            // Set A short (X is long)
            mgr.RAM.WriteByte(cpu.PC, OpcodeList.SEP_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 0x20);
            cpu.ExecuteNext();
            Assert.AreEqual(1, cpu.A.Width);
            Assert.AreEqual(2, cpu.X.Width);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            mgr.RAM.WriteByte(cpu.PC + 1, 2);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Value);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.PHA_StackImplied);
            cpu.ExecuteNext();

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.PLB_StackImplied);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.DataBank.Value);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDX_Immediate);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x125);
            cpu.ExecuteNext();
            Assert.AreEqual(0x125, cpu.X.Value);

            mgr.RAM.WriteByte(cpu.PC, OpcodeList.LDY_AbsoluteIndexedWithX);
            mgr.RAM.WriteWord(cpu.PC + 1, 0x203);
            cpu.ExecuteNext();
            Assert.AreEqual(0xEE55, cpu.Y.Value);
        }

    }
}
