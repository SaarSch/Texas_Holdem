using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Controllers;
using Server.Models;

namespace AllTests.UnitTests
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var test = "i am password";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }


        [TestMethod]
        public void TestMethod2()
        {
            var test = "1";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var test = "cfir3210";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var test = "1234567890";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var test = "!@#$%^&*()_+********1234";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var test = "!@#$%^&*()_+**1234**asdasd**23452**asdasd";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var test = 1001;
            var encrypt = Crypto.Encrypt("" + test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, Convert.ToInt32(dec));
        }

        [TestMethod]
        public void TestMethod9()
        {
            var test = "9876543210";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var test = "";
            var encrypt = Crypto.Encrypt(test);
            var dec = Crypto.Decrypt(encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod11()
        {
            RoomController.SaveReplay(new List<RoomState>(), "PlayerName", "roomName");
        }

        [TestMethod]
        public void TestMethod12()
        {
            var ansLists = new List<string>();
            var AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null
                ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
                : AppDomain.CurrentDomain.BaseDirectory;
            foreach (var f in Directory.GetFiles(AppDataPath))
                if (f.EndsWith(".json") && f.Contains("User_Name" + "PlayerName"))
                {
                    var split = f.Split('#');
                    var date = "Date: " + split[1];
                    var roomName = " Room name: " + split[2].Substring(0, split[2].Length - 5);
                    ansLists.Add(date + roomName);
                }
        }

        [TestMethod]
        public void TestMethod13()
        {
            var ans = new List<RoomState>();
            var AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null
                ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
                : AppDomain.CurrentDomain.BaseDirectory;
            foreach (var f in Directory.GetFiles(AppDataPath))
                if (f.EndsWith(".json") && f.Contains("User_Name" + "sean1111") && f.Contains("sdfhj") &&
                    f.Contains("21_06_2017 07_10_42"))
                {
                    var stream1 = new MemoryStream();
                    using (var fs = File.OpenRead(f))
                    {
                        fs.CopyTo(stream1);
                    }
                    stream1.Position = 0;
                    var ser = new DataContractJsonSerializer(typeof(List<RoomState>));
                    var p2 = (List<RoomState>) ser.ReadObject(stream1);
                }
        }
    }
}