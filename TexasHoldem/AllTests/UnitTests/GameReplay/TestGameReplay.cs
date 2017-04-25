using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.GameReplay;

namespace AllTests.UnitTests.GameReplay
{
    [TestClass]
    public class TestGameReplay
    {
        [TestMethod]
        public void CreateReplay_CreateAReplayFile_FileCreated()
        {
            string path = Directory.GetCurrentDirectory() + "\\" + Replayer.CreateReplay();
            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
        }

        [TestMethod]
        public void Save_SaveValidTurn_FileChanged()
        {
            string filename = Replayer.CreateReplay();
            string path = Directory.GetCurrentDirectory() + "\\" + filename;
            long fileLength = 0;
            long fileLengthAfter = 0;
            if (File.Exists(path))
            {
                fileLength = new FileInfo(path).Length;
            }

            Player p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            Player p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            List<Player> players = new List<Player>{p1,p2};

            Replayer.Save(filename, 5, players, 100, null, "a comment");

            Assert.AreNotEqual(fileLength, fileLengthAfter);
            File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
        }
    }
}
