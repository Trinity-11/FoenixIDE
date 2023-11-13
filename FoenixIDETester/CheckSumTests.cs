using FoenixIDE.Simulator.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoenixIDETester
{
    [TestClass]
    public class CheckSumTests
    {
        [TestMethod]
        public void TestCheckSumWithKnownValue()
        {
            byte chksum = FakeFATSDCardDevice.LFNCheckSum("ADLIBA~1RAD");
            Assert.AreEqual(0xab, chksum);
        }
    }
}
