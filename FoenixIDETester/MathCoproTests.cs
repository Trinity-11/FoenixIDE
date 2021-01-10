using FoenixIDE.MemoryLocations;
using FoenixIDE.Simulator.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDETester
{
    [TestClass]
    public class MathCoproTests
    {
        MathCoproRegister copro;

        [TestInitialize]
        public void Setup()
        {
            copro = new MathCoproRegister(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1);
        }

        [TestMethod]
        public void UnsignedMultiplication1()
        {
            copro.WriteWord(0, 2600);
            copro.WriteWord(2, 6300);
            int lo = copro.ReadWord(4);
            int hi = copro.ReadWord(6);
            int result = (hi << 16) + lo;
            Assert.AreEqual(6300 * 2600, result);
        }

        [TestMethod]
        public void SignedMultiplication1()
        {
            copro.WriteWord(8, -1);
            copro.WriteWord(10, 6300);
            int lo = copro.ReadWord(12);
            int hi = copro.ReadWord(14);
            int result = (hi << 16) + lo;
            Assert.AreEqual(6300 * -1, result);
        }

        [TestMethod]
        public void UnsignedDivision1()
        {
            copro.WriteWord(0x10, 2600);
            copro.WriteWord(0x12, 6300);
            int quotient = copro.ReadWord(0x14);
            int remainder = copro.ReadWord(0x16);
            
            Assert.AreEqual(6300 / 2600, quotient);
            Assert.AreEqual(6300 % 2600, remainder);
        }

        [TestMethod]
        public void SignedDivision1()
        {
            copro.WriteWord(0x18, -2600);
            copro.WriteWord(0x1a, -6300);
            short quotient = (short)copro.ReadWord(0x1c);
            short remainder = (short)copro.ReadWord(0x1e);

            Assert.AreEqual(-6300 / -2600, quotient);
            Assert.AreEqual(-6300 % -2600, remainder);
        }
    }
}
