using FoenixIDE.MemoryLocations;
using FoenixIDE.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDETester
{
    [TestClass]
    public class OverflowTests
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
            cpu = new CPU(MemMgr, 14_000_000, false);
            cpu.SetEmulationMode();
            Assert.AreEqual(1, cpu.A.Width);
            Assert.AreEqual(1, cpu.X.Width);
        }

        // CLC
        [TestMethod]
        public void ClearCarry()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Carry);
        }

        // SEC
        [TestMethod]
        public void SetCarry()
        {
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Carry);
        }

        // LDA #$99
        [TestMethod]
        public void LoadAccumulatorWith99()
        {
            int PC = cpu.PC;

            // By default the CPU must be in 6502 emulation mode
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate); // LDA Immediate
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x99); // #$99

            cpu.ExecuteNext();
            Assert.AreEqual(0x99, cpu.A.Value);
            Assert.AreEqual(PC + 2, cpu.PC);
        }

        // CLC
        // LDA #$99  
        // ADC #$78
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
         #5   
              XCE
              SEC      ; 0 - 1 = -1, returns V = 0
              LDA #$00
              SBC #$01
        */
        [TestMethod]
        public void TestOverflowSBC5_16bit()
        {
            ClearCarry();
            // XCE
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);
            Assert.IsTrue(cpu.Flags.Carry);

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Width);

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0xFFFF, cpu.A.Value);
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

        /**
         * Taken from http://www.6502.org/tutorials/65c816opcodes.html#6.1.1.1
         #10   
              CLC
              XCE  ; switch to native
              REP #$30  ; set AX to 16 bit
              SEC      
              LDA #$0001 
              SBC #$2003  ; A must contain $DFFE
        */
        [TestMethod]
        public void TestOverflowSBC_16bit()
        {
            ClearCarry();
            // XCE
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);
            Assert.IsTrue(cpu.Flags.Carry);

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Width);

            // SEC
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);
            cpu.ExecuteNext();

            // LDA #$0001
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();

            // SBC #$2003
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x2003);
            cpu.ExecuteNext();

            Assert.AreEqual(0xDFFE, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Negative, "Negative should be true");
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
            Assert.IsFalse(cpu.Flags.Zero, "Zero should be false");
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
        }

        /**
        * Error reported by Phil
        * CLC
        * XCE
        * REP #$30
        * 
        * SEC
        * LDA #$AA
        * SBC #$78
        * 
        * Carry should be set
        */
        [TestMethod]
        public void TestOverflowSBC10()
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

            // SEC
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SEC_Implied);

            // LDA #$AA
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xAA);
            cpu.ExecuteNext();

            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x78);
            cpu.ExecuteNext();

            Assert.AreEqual(0x32, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
        }

        /**
         * Problem found when running BASIC816 - for loop=10 to 20:print loop:next
         * XCE
         * LDA #$b
         * CMP #$14
         * LDA #0
         * SBC #0 ; overflow should not be set here
         * BVC skip_eor
         * EOR #$8000
         * BMI ret_true
         */
        [TestMethod]
        public void TestOverflowSBC_BASIC816_FORLOOP()
        {
            ClearCarry();
            // XCE
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);
            Assert.IsTrue(cpu.Flags.Carry);

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Width);

            // LDA #$000b
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xb);
            cpu.ExecuteNext();

            // SBC #$2003
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.CMP_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x14);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Negative, "Negative should be true");
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be negative");

            // LDA #0
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();

            // SBC #0
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();

            Assert.AreEqual(0xFFFF, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Negative, "Negative should be true");
            Assert.IsFalse(cpu.Flags.oVerflow, "Overflow should be false");
            Assert.IsFalse(cpu.Flags.Zero, "Zero should be false");
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
        }

        /** 
         * More Overflow test, this time from http://www.6502.org/tutorials/vflag.html
         * 
              CLC      ; #1: 1 + 1 = 2, returns V = 0
              LDA #$01
              ADC #$01

              CLC      ; #2: 1 + -1 = 0, returns V = 0
              LDA #$01
              ADC #$FF

              CLC      ; #3: 127 + 1 = 128, returns V = 1
              LDA #$7F
              ADC #$01

              CLC      ; #4: -128 + -1 = -129, returns V = 1
              LDA #$80
              ADC #$FF

              SEC      ; #5: 0 - 1 = -1, returns V = 0
              LDA #$00
              SBC #$01

              SEC      ; #6: -128 - 1 = -129, returns V = 1
              LDA #$80
              SBC #$01

              SEC      ; #7: 127 - -1 = 128, returns V = 1
              LDA #$7F
              SBC #$FF
         */
        [TestMethod]
        public void Overflow_testTutorial1_8bits()
        {
            // #1:  1 + 1 = 2, returns V = 0
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Value, "1 + 1 = 2");
            Assert.IsFalse(cpu.Flags.Carry, "1 + 1 = 2 : Carry should be false");
            Assert.IsFalse(cpu.Flags.oVerflow, "1 + 1 = 2 : Overflow should be false");

            // #2:  1 + -1 = 0, returns V = 0
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0, cpu.A.Value, "1 + -1 = 0");
            Assert.IsTrue(cpu.Flags.Carry, "1 + -1 = 0 : Carry should be true");
            Assert.IsFalse(cpu.Flags.oVerflow, "1 + -1 = 0 : Overflow should be false");

            // #3: 127 + 1 = 128, returns V = 1
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x7F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value, "127 + 1 = 128");
            Assert.IsFalse(cpu.Flags.Carry, "127 + 1 = 128 : Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "127 + 1 = 128 : Overflow should be true");

            // #4: -128 + -1 = -129, returns V = 1
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x80);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7F, cpu.A.Value, "-128 + -1 = -129");
            Assert.IsTrue(cpu.Flags.Carry, "-128 + -1 = -129 : Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "-128 + -1 = -129 : Overflow should be true");

            // #5: 0 - 1 = -1, returns V = 0
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0xFF, cpu.A.Value, "0 - 1 = -1");
            Assert.IsFalse(cpu.Flags.Carry, "0 - 1 = -1 : Carry should be false");
            Assert.IsFalse(cpu.Flags.oVerflow, "0 - 1 = -1 : Overflow should be false");

            // #6: #6: -128 - 1 = -129, returns V = 1
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x80);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7F, cpu.A.Value, "-128 - 1 = -129");
            Assert.IsTrue(cpu.Flags.Carry, "-128 - 1 = -129 : Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "-128 - 1 = -129 : Overflow should be true");

            // #7: 127 - -1 = 128, returns V = 1
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x7F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value, "127 - -1 = 128");
            Assert.IsFalse(cpu.Flags.Carry, "127 - -1 = 128 : Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "127 - -1 = 128 Overflow should be true");
        }

        [TestMethod]
        public void Overflow_testTutorial1_16bits()
        {
            // Switch to 16 bit mode
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.XCE_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Emulation);
            Assert.IsTrue(cpu.Flags.Carry);

            // REP #$30
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.REP_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x30);
            cpu.ExecuteNext();

            // Test that Accumulator and Registers are wide
            Assert.AreEqual(2, cpu.A.Width);
            Assert.AreEqual(2, cpu.X.Width);

            // #1:  1 + 1 = 2, returns V = 0
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(2, cpu.A.Value, "1 + 1 = 2");
            Assert.IsFalse(cpu.Flags.Carry, "1 + 1 = 2 : Carry should be false");
            Assert.IsFalse(cpu.Flags.oVerflow, "1 + 1 = 2 : Overflow should be false");

            // #2:  1 + -1 = 0, returns V = 0
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xFFFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0, cpu.A.Value, "1 + -1 = 0");
            Assert.IsTrue(cpu.Flags.Carry, "1 + -1 = 0 : Carry should be true");
            Assert.IsFalse(cpu.Flags.oVerflow, "1 + -1 = 0 : Overflow should be false");

            // #3a: 127 + 1 = 128, returns V = 0
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x7F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value, "127 + 1 = 128");
            Assert.IsFalse(cpu.Flags.Carry, "127 + 1 = 128 : Carry should be false");
            Assert.IsFalse(cpu.Flags.oVerflow, "127 + 1 = 128 : Overflow should be false");

            // #3b: 32767 + 1 = 32768, returns V = 1
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x7FFF);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x8000, cpu.A.Value, "32767 + 1 = 32768");
            Assert.IsFalse(cpu.Flags.Carry, "32767 + 1 = 32768 : Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "32767 + 1 = 32768 : Overflow should be true");

            // #4: -32768 + -1 = -32769, returns V = 1
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x8000);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xFFFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7FFF, cpu.A.Value, "-32768 + -1 = -32769");
            Assert.IsTrue(cpu.Flags.Carry, "-32768 + -1 = -32769 : Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "-32768 + -1 = -32769 : Overflow should be true");

            // #5: 0 - 1 = -1, returns V = 0
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x0);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0xFFFF, cpu.A.Value, "0 - 1 = -1");
            Assert.IsFalse(cpu.Flags.Carry, "0 - 1 = -1 : Carry should be false");
            Assert.IsFalse(cpu.Flags.oVerflow, "0 - 1 = -1 : Overflow should be false");

            // #6: #6: -32768 - 1 = -32769, returns V = 1
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x8000);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x1);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7FFF, cpu.A.Value, "-128 - 1 = -129");
            Assert.IsTrue(cpu.Flags.Carry, "-128 - 1 = -129 : Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "-128 - 1 = -129 : Overflow should be true");

            // #7: 32767 - -1 = 32768, returns V = 1
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0x7FFF);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteWord(cpu.PC + 1, 0xFFFF);
            cpu.ExecuteNext();
            Assert.AreEqual(0x8000, cpu.A.Value, "127 - -1 = 128");
            Assert.IsFalse(cpu.Flags.Carry, "127 - -1 = 128 : Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "127 - -1 = 128 Overflow should be true");
        }

        /* 
         * Remember that ADC and SBC not only affect the carry flag, but they also use the value of the carry flag (i.e. the value before the ADC or SBC), and this will affect the result and will affect V. For example:
              SEC      ; Note: SEC, not CLC
              LDA #$3F ; #8: 63 + 64 + 1 = 128, returns V = 1
              ADC #$40

              CLC      ; Note: CLC, not SEC
              LDA #$C0 ; #9: -64 - 64 - 1 = -129, returns V = 1
              SBC #$40
        */
        [TestMethod]
        public void OverflowTestWithCarry()
        {
            //  #8: 63 + 64 + 1 = 128, returns V = 1
            SetCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x3F);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.ADC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x40);
            cpu.ExecuteNext();
            Assert.AreEqual(0x80, cpu.A.Value);
            Assert.IsFalse(cpu.Flags.Carry, "Carry should be false");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");

            //  #9: -64 - 64 - 1 = -129, returns V = 1
            ClearCarry();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.LDA_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0xC0);
            cpu.ExecuteNext();
            MemMgr.RAM.WriteByte(cpu.PC, OpcodeList.SBC_Immediate);
            MemMgr.RAM.WriteByte(cpu.PC + 1, 0x40);
            cpu.ExecuteNext();
            Assert.AreEqual(0x7f, cpu.A.Value);
            Assert.IsTrue(cpu.Flags.Carry, "Carry should be true");
            Assert.IsTrue(cpu.Flags.oVerflow, "Overflow should be true");
        }
    }
}
