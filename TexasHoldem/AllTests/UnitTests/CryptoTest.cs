using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Controllers;
using Server.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

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
            string test = "1234567890";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod7()
        {
            string test = "!@#$%^&*()_+********1234";
            string encrypt = Crypto.Encrypt(test);
            string dec = Crypto.Decrypt(encrypt);
            Assert.AreNotEqual(test, encrypt);
            Assert.AreEqual(test, dec);
        }

        [TestMethod]
        public void TestMethod8()
        {
            string test = "!@#$%^&*()_+**1234**asdasd**23452**asdasd";
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
            string test = "9876543210";
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
           RoomController.SaveReaply(new List<RoomState>(),"PlayerName","roomName");
        }

        [TestMethod]
        public void TestMethod12()
        {
            List<string> ansLists = new List<string>();
            string AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString() : AppDomain.CurrentDomain.BaseDirectory;
            foreach (string f in Directory.GetFiles(AppDataPath))
            {
                if (f.EndsWith(".json") && f.Contains("User_Name" + "PlayerName"))
                {
                    string[] split = f.Split('#');
                    string date = "Date: " + split[1];
                    string roomName = " Room name: " + split[2].Substring(0,split[2].Length-5);
                    ansLists.Add(date + roomName);
                }

            }
        }

        [TestMethod]
        public void TestMethod13()
        {
            List<RoomState> ans = new List<RoomState>();
            string AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString() : AppDomain.CurrentDomain.BaseDirectory;
            foreach (string f in Directory.GetFiles(AppDataPath))
            {
                if (f.EndsWith(".json") && f.Contains("User_Name" + "sean1111") && f.Contains("sdfhj") && f.Contains("21_06_2017 07_10_42"))
                {
                    MemoryStream stream1 = new MemoryStream();
                    using (FileStream fs = File.OpenRead(f))
                    {
                        fs.CopyTo(stream1);
                    }
                    stream1.Position = 0;
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<RoomState>));
                    List<RoomState> p2 = (List<RoomState>)ser.ReadObject(stream1);
                }
            }
        }

    }
}
