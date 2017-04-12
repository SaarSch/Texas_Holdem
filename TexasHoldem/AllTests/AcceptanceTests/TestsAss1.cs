using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Bridges;

namespace AllTests.AcceptanceTests
{
    [TestClass]
    public class TestsAss1
    {
        private IBridge bridge;
        [TestMethod]
        public void TestRegisterToTheSystemGood()
        {
            Assert.IsTrue(bridge.register("EladKamin","123456789"));
            Assert.IsTrue(bridge.isUserExist("EladKamin"));
            Assert.IsTrue(bridge.deleteUser("EladKamin"));
        }
        [TestMethod]
        public void TestRegisterToTheSystemSad()
        {
            Assert.IsFalse(false);
        }
        [TestMethod]
        public void TestRegisterToTheSystemBad()
        {
            Assert.IsFalse(false);
        }
    }
}
