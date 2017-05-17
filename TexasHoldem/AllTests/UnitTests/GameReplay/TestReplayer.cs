using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Game;
using TexasHoldem.GameReplay;
using TexasHoldem.Users;

namespace AllTests.UnitTests.GameReplay
{
    [TestClass]
    public class TestReplayer
    {
        [TestMethod]
        public void CreateReplay_CreateAReplayFile_FileCreated()
        {
            var path = Directory.GetCurrentDirectory() + "\\" + Replayer.CreateReplay();
            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
            File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
        }

        [TestMethod]
        public void Save_SaveWithoutCommunity_FileChanged()
        {
            var filename = Replayer.CreateReplay();
            var path = Directory.GetCurrentDirectory() + "\\" + filename;
            long fileLength = 0;
            long fileLengthAfter = 0;

            if (File.Exists(path))
                fileLength = new FileInfo(path).Length;

            var p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            var p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            var players = new List<IPlayer> {p1, p2};

            Replayer.Save(filename, 5, players, 100, null, null);
            if (File.Exists(path))
                fileLengthAfter = new FileInfo(path).Length;

            Assert.AreNotEqual(fileLength, fileLengthAfter);
            File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
            File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
        }

        [TestMethod]
        public void Save_SaveValidEntry_FileChanged()
        {
            var filename = Replayer.CreateReplay();
            var path = Directory.GetCurrentDirectory() + "\\" + filename;
            long fileLength = 0;
            long fileLengthAfter = 0;

            if (File.Exists(path))
                fileLength = new FileInfo(path).Length;

            var p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            var p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            p1.Folded = true;
            var players = new List<IPlayer> {p1, p2};
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            Replayer.Save(filename, 5, players, 100, community, "a comment");
            if (File.Exists(path))
                fileLengthAfter = new FileInfo(path).Length;

            Assert.AreNotEqual(fileLength, fileLengthAfter);
            File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
            File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
        }

        [TestMethod]
        public void Save_SaveWithoutFilename_ExceptionThrown()
        {
            var p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            var p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            p1.Folded = true;
            var players = new List<IPlayer> {p1, p2};
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            try
            {
                Replayer.Save(null, 5, players, 100, community, "a comment");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [TestMethod]
        public void Save_SaveWithInvalidFilename_ExceptionThrown()
        {
            var p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            var p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            p1.Folded = true;
            var players = new List<IPlayer> {p1, p2};
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            try
            {
                Replayer.Save("gfdsgfds.csv", 5, players, 100, community, "a comment");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [TestMethod]
        public void Save_SaveWithInvalidRound_ExceptionThrown()
        {
            var filename = Replayer.CreateReplay();
            var p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            var p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            p1.Folded = true;
            var players = new List<IPlayer> {p1, p2};
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            try
            {
                Replayer.Save(filename, 0, players, 100, community, "a comment");
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
            }
        }

        [TestMethod]
        public void Save_SaveWithoutPlayerList_ExceptionThrown()
        {
            var filename = Replayer.CreateReplay();
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            try
            {
                Replayer.Save(filename, -5, null, 100, community, "a comment");
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
            }
        }

        [TestMethod]
        public void Save_SaveWithEmptyPlayerList_ExceptionThrown()
        {
            var filename = Replayer.CreateReplay();
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            try
            {
                Replayer.Save(filename, -5, new List<IPlayer>(), 100, community, "a comment");
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
            }
        }

        [TestMethod]
        public void Save_SaveWithInvalidPot_ExceptionThrown()
        {
            var filename = Replayer.CreateReplay();
            var p1 = new Player("yossi", new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000));
            var p2 = new Player("kobi", new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000));
            p1.CurrentBet = 100;
            p2.CurrentBet = 150;
            p1.Folded = true;
            var players = new List<IPlayer> {p1, p2};
            Card[] community =
            {
                new Card(2, CardType.Clubs), null, new Card(5, CardType.Diamonds), new Card(10, CardType.Hearts),
                new Card(12, CardType.Spades)
            };

            try
            {
                Replayer.Save(filename, -5, players, -100, community, "a comment");
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\" + filename);
                File.Delete(Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt");
            }
        }
    }
}