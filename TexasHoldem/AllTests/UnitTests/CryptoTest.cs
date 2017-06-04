using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

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


        [TestMethod]
        public void TestMethod2()
        {
            string test = "1";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod3()
        {
            string test = "cfir3210";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod4()
        {
            string test = "123456789";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod7()
        {
            string test = "!@#$%^&*()_+********";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod8()
        {
            string test = "!@#$%^&*()_+**1234**asdasd**23452**";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod5()
        {
            int test = 1001;
            string encrypt = Crypto.Encrypt(""+test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, Convert.ToInt32(dec));
        }

        [TestMethod]
        public void TestMethod9()
        {
            string test = "sean1111";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod10()
        {
            string test = "";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod11()
        {
            string test = "";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(null);
            Assert.AreEqual(test, dec);
        }

    }
}
