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
                RAM = new MemoryRAM(0, 1024),  // Only setup 1K of RAM - this should be tons to test our CPU
                CODEC = new CodecRAM(1025,1),
                INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4)
            };
            cpu = new CPU(mgr);
            cpu.SetEmulationMode();
        }

        // LDA #$99
        [TestMethod]
        public void LoadAccumulatorWith99()
        {
            // By default the CPU must be in 6502 emulation mode
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.LDA_Immediate); // LDA Immediate
            mgr.RAM.WriteByte(cpu.PC.Value + 1, 0x99); // #$99
            int PC = cpu.PC.Value;
            cpu.ExecuteNext();
            Assert.AreEqual(0x99, cpu.A.Value);
            Assert.AreEqual(PC + 2, cpu.GetLongPC());
        }
        [TestMethod]
        public void ClearCarry()
        {
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.CLC_Implied);
            cpu.ExecuteNext();
            Assert.IsFalse(cpu.Flags.Carry);
        }

        [TestMethod]
        public void LoadCheckCarrySetForOverflowAbsolute()
        {
            LoadAccumulatorWith99();
            ClearCarry();
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.ADC_Immediate);
            mgr.RAM.WriteByte(cpu.PC.Value + 1, 0x78);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.oVerflow);
            Assert.IsTrue(cpu.Flags.Carry);
        }

        [TestMethod]
        public void LoadCheckCarrySetForOverflowDirectPage()
        {
            LoadAccumulatorWith99();
            // Write a value that will cause an overflow in the addition - A is $99
            mgr.RAM.WriteByte(0x56, 0x78);
            ClearCarry();
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.ADC_DirectPage);
            mgr.RAM.WriteByte(cpu.PC.Value + 1, 0x56);
            cpu.ExecuteNext();
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
            cpu.SetLongPC(0);
            // lda #1
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.LDA_Immediate); // LDA Immediate
            mgr.RAM.WriteByte(cpu.PC.Value + 1, 1); // #1
            cpu.ExecuteNext();
            // sta z_B
            byte z_B = 0x20;
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.STA_DirectPage);
            mgr.RAM.WriteByte(cpu.PC.Value + 1, z_B);
            cpu.ExecuteNext();
            // lda #255
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.LDA_Immediate); // LDA Immediate
            mgr.RAM.WriteByte(cpu.PC.Value + 1, 255); // #255
            cpu.ExecuteNext();
            
            // adc z_B
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.ADC_DirectPage);
            mgr.RAM.WriteByte(cpu.PC.Value + 1, z_B);
            cpu.ExecuteNext();
            Assert.IsTrue(cpu.Flags.Zero);
            Assert.IsTrue(cpu.Flags.Carry);

            // sta z_C
            byte z_C = 0x21;
            mgr.RAM.WriteByte(cpu.PC.Value, OpcodeList.STA_DirectPage);
            mgr.RAM.WriteByte(cpu.PC.Value + 1, z_C);
            cpu.ExecuteNext();
            Assert.AreEqual(0, mgr.RAM.ReadByte(z_C));
        }
    }
}
