using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using server;

namespace AllTests.UnitTests
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string test = "i am password";
            string encrypt =Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }
    }
}
