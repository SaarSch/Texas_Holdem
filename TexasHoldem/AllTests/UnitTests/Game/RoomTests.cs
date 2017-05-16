using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Game;
using TexasHoldem.Users;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class RoomTests
    {
        private readonly GamePreferences _gp = new GamePreferences();
        private readonly User _u = new User("tom12345", "12345678", "aaa.png", "hello@gmail.com", 50000);
        private readonly User _u1 = new User("tom12346", "12345678", "bbb.png", "hello1@gmail.com", 50000);
        private readonly User _u2 = new User("tom12347", "12345678", "ccc.png", "hello3@gmail.com", 50000);

        [TestMethod]
        public void AddPlayerTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            var p1 = new Player("shachar1", _u1);
            r.AddPlayer(p1);
            Assert.IsTrue(r.Players.Count == 2);
        }

        [TestMethod]
        public void GamePreferencesTest()
        {
            try
            {
                //Gametype.NoLimit, -8, 0, 4, 2, 8, true
                new GamePreferences {BuyInPolicy = -8};
                Assert.Fail();
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("buy in policy can't be negative"));
            }
        }

        [TestMethod]
        public void GamePreferencesTest1()
        {
            try
            {
                //Gametype.NoLimit, 1, -8, 4, 2, 8, true
                new GamePreferences { ChipPolicy = -8 };
                Assert.Fail();
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Chip policy can't be negative"));
            }
            try
            {
                //Gametype.NoLimit, 1, 2, 1, 2, 8, true
                new GamePreferences { MinBet = 1 };
                Assert.Fail();
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Minimum bet can't be less then 2"));
            }
            try
            {
                //Gametype.NoLimit, 1, 4, 4, 1, 8, true
                new GamePreferences { MinPlayers = 1 };
                Assert.Fail();
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Minimum players can't be less then 2"));
            }
            try
            {
                //Gametype.NoLimit, 1, 4, 4, 2, 15, true
                new GamePreferences { MaxPlayers = 15 };
                Assert.Fail();
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Maximum players can't be more then 10"));
            }
            try
            {
                //Gametype.NoLimit, 1, 2, 4, 2, 8, true
                new GamePreferences { ChipPolicy = 2, MinBet = 4 };
                Assert.Fail();
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("min bet can't be higher then chip policy"));
            }
        }

        [TestMethod]
        public void RemovePlayerTest()
        {
            var p = new Player("shachar1", _u);
            var r = new Room("aaaa", p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            var p1 = new Player("shachar", _u1);
            r.AddPlayer(p1);
            Assert.IsTrue(r.Players.Count == 2);
            r.ExitRoom("shachar");
            Assert.IsTrue(r.Players.Count == 1);
        }

        [TestMethod]
        public void DealTwoTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            var p1 = new Player("shachar3", _u1);
            var p3 = new Player("shachar2", new User("tom12345f", "12345678", "gggg.png", "hello@gmail.com", 50000));
            var p4 = new Player("shachar14", new User("tom12345g", "12345678", "eeee.png", "hello@gmail.com", 50000));
            var p5 = new Player("shachar4", new User("tom12345h", "12345678", "jgjg.png", "hello@gmail.com", 50000));
            r.AddPlayer(p1);
            r.AddPlayer(p3);
            r.AddPlayer(p4);
            r.AddPlayer(p5);
            Assert.IsTrue(r.Players.Count == 5);
            r.DealTwo();
            foreach (var unused in r.Players)
                Assert.IsTrue(p.Hand.Length == 2);
        }

        [TestMethod]
        public void DealCommunityFirstTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            r.IsOn = true;
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.Cards.Count == 47);
            Assert.IsNotNull(r.CommunityCards[0]);
            Assert.IsNotNull(r.CommunityCards[1]);
        }

        [TestMethod]
        public void DealCommunitySecondTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            r.IsOn = true;
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.Cards.Count == 47);
            Assert.IsNotNull(r.CommunityCards[0]);
            Assert.IsNotNull(r.CommunityCards[1]);
            r.DealCommunitySecond();
            Assert.IsTrue(r.Deck.Cards.Count == 46);
            Assert.IsNotNull(r.CommunityCards[2]);
        }

        [TestMethod]
        public void DealCommunityThirdTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            r.IsOn = true;
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.Cards.Count == 47);
            Assert.IsNotNull(r.CommunityCards[0]);
            Assert.IsNotNull(r.CommunityCards[1]);
            Assert.IsNotNull(r.CommunityCards[2]);
            r.DealCommunitySecond();
            Assert.IsTrue(r.Deck.Cards.Count == 46);
            Assert.IsNotNull(r.CommunityCards[3]);
            r.DealCommunityThird();
            Assert.IsTrue(r.Deck.Cards.Count == 45);
            Assert.IsNotNull(r.CommunityCards[4]);
        }

        [TestMethod]
        public void HandCalculatorRoyalStraightTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            var win = new List<Card>
            {
                new Card(14, CardType.Clubs),
                new Card(13, CardType.Clubs),
                new Card(12, CardType.Clubs),
                new Card(11, CardType.Clubs),
                new Card(10, CardType.Clubs),
                new Card(9, CardType.Clubs),
                new Card(8, CardType.Clubs)
            }; // royal flush

            var loss = new List<Card>
            {
                new Card(3, CardType.Clubs),
                new Card(4, CardType.Clubs),
                new Card(5, CardType.Clubs),
                new Card(6, CardType.Clubs),
                new Card(7, CardType.Clubs),
                new Card(8, CardType.Clubs),
                new Card(9, CardType.Clubs)
            }; // straight flush

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraight4OfTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            var win = new List<Card>
            {
                new Card(7, CardType.Clubs),
                new Card(2, CardType.Clubs),
                new Card(3, CardType.Clubs),
                new Card(4, CardType.Clubs),
                new Card(5, CardType.Clubs),
                new Card(6, CardType.Clubs),
                new Card(7, CardType.Clubs)
            }; // straight flush

            var loss = new List<Card>
            {
                new Card(6, CardType.Clubs),
                new Card(3, CardType.Clubs),
                new Card(12, CardType.Clubs),
                new Card(10, CardType.Spades),
                new Card(10, CardType.Clubs),
                new Card(10, CardType.Hearts),
                new Card(10, CardType.Diamonds)
            }; // 4 of a kind

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculator4OfFullTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            var win = new List<Card>
            {
                new Card(7, CardType.Clubs),
                new Card(7, CardType.Spades),
                new Card(7, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(5, CardType.Clubs),
                new Card(6, CardType.Clubs),
                new Card(14, CardType.Clubs)
            }; //4 of a kind

            var loss = new List<Card>
            {
                new Card(6, CardType.Clubs),
                new Card(6, CardType.Hearts),
                new Card(6, CardType.Diamonds),
                new Card(2, CardType.Spades),
                new Card(2, CardType.Clubs),
                new Card(10, CardType.Hearts),
                new Card(8, CardType.Diamonds)
            }; //full house

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorFullFlushTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);

            var win = new List<Card>
            {
                new Card(6, CardType.Clubs),
                new Card(6, CardType.Hearts),
                new Card(6, CardType.Diamonds),
                new Card(2, CardType.Spades),
                new Card(2, CardType.Clubs),
                new Card(10, CardType.Hearts),
                new Card(8, CardType.Diamonds)
            }; //full house

            var loss = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(8, CardType.Diamonds),
                new Card(11, CardType.Diamonds),
                new Card(2, CardType.Diamonds),
                new Card(9, CardType.Diamonds),
                new Card(13, CardType.Diamonds),
                new Card(4, CardType.Diamonds)
            }; //flush

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorFlushStraightTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);

            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(8, CardType.Diamonds),
                new Card(11, CardType.Diamonds),
                new Card(2, CardType.Diamonds),
                new Card(9, CardType.Diamonds),
                new Card(13, CardType.Diamonds),
                new Card(4, CardType.Diamonds)
            }; //flush

            var loss = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(4, CardType.Clubs),
                new Card(5, CardType.Diamonds),
                new Card(6, CardType.Hearts),
                new Card(7, CardType.Diamonds)
            }; //straight

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraight3OfTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);

            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(4, CardType.Clubs),
                new Card(5, CardType.Diamonds),
                new Card(6, CardType.Hearts),
                new Card(7, CardType.Diamonds)
            }; //straight

            var loss = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(14, CardType.Clubs),
                new Card(5, CardType.Diamonds),
                new Card(14, CardType.Hearts),
                new Card(7, CardType.Diamonds)
            }; //3 of a kind

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculator3Of2PairsTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);

            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(14, CardType.Clubs),
                new Card(5, CardType.Diamonds),
                new Card(14, CardType.Hearts),
                new Card(7, CardType.Diamonds)
            }; //3 of a kind

            var loss = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(7, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //2 pairs

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculator2PairsPairTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);

            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(7, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //2 pairs

            var loss = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //pair

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void BHandCalculatorPairHighCardTest()
        {
            var p = new Player("shachar", new User("tom12345a", "12345678", "aaa.png", "hello@gmail.com", 50000));
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //pair

            var loss = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(13, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; // high card

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorPairPairTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //pair

            var loss = new List<Card>
            {
                new Card(13, CardType.Diamonds),
                new Card(13, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; // pair

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculator2Pair2PairTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(13, CardType.Spades),
                new Card(13, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; // 2 pair

            var loss = new List<Card>
            {
                new Card(13, CardType.Diamonds),
                new Card(13, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(2, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //  2 pair

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorHighHighTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(10, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //high card

            var loss = new List<Card>
            {
                new Card(13, CardType.Diamonds),
                new Card(8, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; // high card

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorHighHighTest1()
        {
            //Gametype.Limit, 1, 30, 10, 3, 8, true
            var gp1 = new GamePreferences();
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, gp1);


            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(10, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //high card

            var loss = new List<Card>
            {
                new Card(13, CardType.Diamonds),
                new Card(8, CardType.Clubs),
                new Card(2, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; // high card

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void SetBetTest()
        {
            //Gametype.Limit, 1, 30, 10, 3, 8, true
            var gp1 = new GamePreferences();
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, gp1);
            try
            {
                r.SetBet(null, 1000,false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("invalid player"));
            }
        }

        [TestMethod]
        public void SetBetTest2()
        {
            //Gametype.Limit, 1, 30, 10, 3, 8, true
            var gp1 = new GamePreferences();
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, gp1);
            try
            {
                r.SetBet(p, 0, false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("can't bet less then min bet"));
            }
        }

        [TestMethod]
        public void SetBetTest3()
        {
            //Gametype.NoLimit, 1, 30, 10, 3, 8, true
            var gp1 = new GamePreferences();
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, gp1);
            try
            {
                p.PreviousRaise = 30;
                p.BetInThisRound = true;
                r.SetBet(p, 10, false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("can't bet less then previous raise in no limit mode"));
            }
        }

        [TestMethod]
        public void SetBetTest4()
        {
            //Gametype.Limit, 1, 30, 10, 3, 8, true
            var gp1 = new GamePreferences();
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, gp1);
            try
            {
                r.SetBet(p, 120, false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("in pre flop/flop in limit mode bet must be equal to big blind"));
            }
        }

        [TestMethod]
        public void SetBetTest5()
        {
            //Gametype.Limit, 1, 30, 10, 3, 8, true
            var gp1 = new GamePreferences();
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, gp1)
            {
                CommunityCards =
                {
                    [0] = new Card(5, CardType.Clubs),
                    [1] = new Card(5, CardType.Clubs),
                    [2] = new Card(5, CardType.Clubs),
                    [3] = new Card(5, CardType.Clubs)
                }
            };
            try
            {
                r.SetBet(p, 120, false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("in pre flop/flop in limit mode bet must be equal to big blind"));
            }
        }

        [TestMethod]
        public void SetBetTest6()
        {
            var gp = new GamePreferences
            {
                GameType = Gametype.PotLimit,
                ChipPolicy = 30,
                MinBet = 10,
                MinPlayers = 3
            };

            var p = new Player("shachar", _u);
            var p2 = new Player("shachar1", _u);
            var r = new Room("aaaa", p, gp);
            r.AddPlayer(p2);
            p2.CurrentBet = 500;
            try
            {
                r.SetBet(p, 600, false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("in limit pot mode bet must lower then pot"));
            }
        }

        [TestMethod]
        public void SetBetTest7()
        {
            var gp = new GamePreferences
            {
                GameType = Gametype.PotLimit,
                ChipPolicy = 30,
                MinBet = 10,
                MinPlayers = 3
            };

            var p = new Player("shachar", _u);
            var p2 = new Player("shachar2", _u);
            var r = new Room("aaaa", p, gp);
            r.AddPlayer(p2);
            p.ChipsAmount = 60000;
            p2.CurrentBet = 500;
            r.SetBet(p, 300, false);
            Assert.IsTrue(p.CurrentBet == 300);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(13, CardType.Clubs),
                new Card(12, CardType.Spades),
                new Card(11, CardType.Hearts),
                new Card(12, CardType.Diamonds),
                new Card(10, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //straight

            var loss = new List<Card>
            {
                new Card(13, CardType.Diamonds),
                new Card(7, CardType.Clubs),
                new Card(6, CardType.Spades),
                new Card(5, CardType.Hearts),
                new Card(4, CardType.Diamonds),
                new Card(3, CardType.Clubs),
                new Card(2, CardType.Hearts)
            }; //straight

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest1()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(14, CardType.Clubs),
                new Card(13, CardType.Clubs),
                new Card(12, CardType.Clubs),
                new Card(11, CardType.Clubs),
                new Card(12, CardType.Clubs),
                new Card(10, CardType.Clubs),
                new Card(9, CardType.Clubs)
            }; //straight

            var loss = new List<Card>
            {
                new Card(8, CardType.Diamonds),
                new Card(7, CardType.Diamonds),
                new Card(6, CardType.Diamonds),
                new Card(5, CardType.Diamonds),
                new Card(4, CardType.Diamonds),
                new Card(3, CardType.Diamonds),
                new Card(2, CardType.Diamonds)
            }; //straight

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest2()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(2, CardType.Clubs),
                new Card(14, CardType.Clubs),
                new Card(12, CardType.Clubs),
                new Card(11, CardType.Clubs),
                new Card(13, CardType.Clubs),
                new Card(10, CardType.Clubs),
                new Card(9, CardType.Clubs)
            }; //straight

            var loss = new List<Card>
            {
                new Card(8, CardType.Diamonds),
                new Card(7, CardType.Diamonds),
                new Card(6, CardType.Diamonds),
                new Card(5, CardType.Diamonds),
                new Card(4, CardType.Diamonds),
                new Card(3, CardType.Diamonds),
                new Card(2, CardType.Diamonds)
            }; //straight

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest3()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);


            var win = new List<Card>
            {
                new Card(10, CardType.Spades),
                new Card(9, CardType.Spades),
                new Card(6, CardType.Diamonds),
                new Card(5, CardType.Diamonds),
                new Card(4, CardType.Hearts),
                new Card(3, CardType.Clubs),
                new Card(2, CardType.Hearts)
            }; //straight

            var loss = new List<Card>
            {
                new Card(12, CardType.Spades),
                new Card(11, CardType.Diamonds),
                new Card(6, CardType.Diamonds),
                new Card(5, CardType.Diamonds),
                new Card(3, CardType.Spades),
                new Card(3, CardType.Diamonds),
                new Card(3, CardType.Hearts)
            }; //straight

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void HandCalculator3Of3OfTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);

            var win = new List<Card>
            {
                new Card(14, CardType.Diamonds),
                new Card(14, CardType.Clubs),
                new Card(14, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(7, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //3 of a kind

            var loss = new List<Card>
            {
                new Card(13, CardType.Diamonds),
                new Card(13, CardType.Clubs),
                new Card(13, CardType.Spades),
                new Card(3, CardType.Hearts),
                new Card(14, CardType.Diamonds),
                new Card(4, CardType.Clubs),
                new Card(9, CardType.Hearts)
            }; //3 of a kind

            Assert.IsTrue(r.HandLogic.HandCalculator(win).HandStrongessValue > r.HandLogic.HandCalculator(loss).HandStrongessValue);
        }

        [TestMethod]
        public void WinnersTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar1", _u1);

            r.AddPlayer(p1);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(13, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(13, CardType.Clubs);
            p.Hand[1] = new Card(13, CardType.Hearts);

            p1.Hand[0] = new Card(12, CardType.Clubs);
            p1.Hand[1] = new Card(12, CardType.Hearts);

            Assert.IsTrue(r.Winners().Count == 1 && r.Winners()[0] == p);
        }

        [TestMethod]
        public void WinnersSameHandRankTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", _u1);
            var p2 = new Player("shachar1", _u2);
            r.AddPlayer(p1);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(2, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(5, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);

            r.AddPlayer(p2);

            Assert.IsTrue(r.Winners().Count == 1 && r.Winners()[0] == p1);
        }

        [TestMethod]
        public void WinnersTieTest()
        {
            var p = new Player("shachar", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", _u1);
            var p2 = new Player("shachar3", _u2);
            r.AddPlayer(p1);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(5, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(5, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);

            r.AddPlayer(p2);

            Assert.IsTrue(r.Winners().Count == 2 && r.Winners()[0] == p && r.Winners()[1] == p1);
        }

        [TestMethod]
        public void ChipsTest()
        {
            var p = new Player("shachar1", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", _u1);
            var p2 = new Player("shachar3", _u2);
            r.AddPlayer(p1);
            r.AddPlayer(p2);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(5, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(2, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);

            p.SetBet(300);
            p1.SetBet(300);
            p2.SetBet(300);
            r.IsOn = true;
            r.CalcWinnersChips();

            Assert.IsTrue(p.ChipsAmount == 50600);
        }

        [TestMethod]
        public void NextTurnTest()
        {
            var p = new Player("shachar1", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", _u1);
            var p2 = new Player("shachar3", _u2);
            r.AddPlayer(p1);
            r.AddPlayer(p2);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(5, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(2, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);
            r.IsOn = true;
            p.SetBet(300);
            p1.SetBet(300);
            p2.SetBet(300);


            r.CalcWinnersChips();

            Assert.IsTrue(r.Players[0] == p1 && r.Players[1] == p2 && r.Players[2] == p);
        }

        [TestMethod]
        public void SmallBigBlindTest()
        {
            var p = new Player("shachar1", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", _u1);
            var p2 = new Player("shachar3", _u2);
            r.AddPlayer(p1);
            r.AddPlayer(p2);
            r.StartGame();


            Assert.IsTrue(r.Players[0].CurrentBet == 0 && r.Players[1].CurrentBet == 2 && r.Players[2].CurrentBet == 4);
        }

        [TestMethod]
        public void SmallBigBlind2PlayersTest()
        {
            var p = new Player("shachar1", _u);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", _u1);
            r.AddPlayer(p1);
            r.StartGame();


            Assert.IsTrue(r.Players[0].CurrentBet == 2 && r.Players[1].CurrentBet == 4);
        }

        [TestMethod]
        public void SpectateTest()
        {
            var p = new Player("shachar1", _u);
            var r = new Room("aaaa", p, _gp);
            r.Spectate(_u);
            Assert.IsTrue(r.SpectateUsers.Contains(_u));
        }

        [TestMethod]
        public void NotifyTestPlayerToAllRoom()
        {
            var ml = new MessageLogic();
            const string message = "wow you are so cool!";
            var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
            var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
            var p = new Player("shachar1", yossi);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", kobi);
            r.AddPlayer(p1);
            ml.PlayerSendMessege(message,p1,r);

            foreach (var p2 in r.Players)
                Assert.AreEqual(
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ":  "+p1.Name+": " + message,
                    p2.User.Notifications[0].Item2);
        }
        [TestMethod]
        public void NotifyTestPlayerWisper()
        {
            var ml = new MessageLogic();
            const string message = "wow you are so cool!";
            var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
            var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
            var p = new Player("shachar1", yossi);
            var r = new Room("aaaa", p, _gp);
            var p1 = new Player("shachar2", kobi);
            r.AddPlayer(p1);
            ml.PlayerWisper(message, p1,p.User,r);
                Assert.AreEqual(
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + p1.Name + ": " + message,
                    p.User.Notifications[0].Item2);
        }

        [TestMethod]
        public void NotifyTestPlayerWisperFail()
        {
            try
            {
                var ml = new MessageLogic();
                const string message = null;
                var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
                var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
                var p = new Player("shachar1", yossi);
                var r = new Room("aaaa", p, _gp);
                var p1 = new Player("shachar2", kobi);
                r.AddPlayer(p1);
                ml.PlayerWisper(message, p1, p.User, r);
               
            }
            catch(Exception e)
            {
                Assert.AreEqual(
                   e.Message, "cant send null message");
            }
        }

        [TestMethod]
        public void NotifyTestPlayerWisperFail2()
        {
            try
            {
                var ml = new MessageLogic();
                const string message = null;
                var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
                var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
                var p = new Player("shachar1", yossi);
                var r = new Room("aaaa", p, _gp);
                var p1 = new Player("shachar2", kobi);
                r.AddPlayer(p1);
                ml.PlayerWisper("aa", null, p.User, r);

            }
            catch (Exception e)
            {
                Assert.AreEqual(
                   e.Message, "sender cant be null");
            }
        }

        [TestMethod]
        public void NotifyTestPlayerWisperFail3()
        {
            try
            {
                var ml = new MessageLogic();
                const string message = null;
                var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
                var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
                var p = new Player("shachar1", yossi);
                var r = new Room("aaaa", p, _gp);
                var p1 = new Player("shachar2", kobi);
                r.AddPlayer(p1);
                ml.PlayerWisper("", p1, p.User, r);

            }
            catch (Exception e)
            {
                Assert.AreEqual(
                   e.Message, "cant send empty message / curses");
            }
        }

        [TestMethod]
        public void NotifyTestPlayerWisperFail4()
        {
            try
            {
                var ml = new MessageLogic();
                const string message = null;
                var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
                var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
                var p = new Player("shachar1", yossi);
                var r = new Room("aaaa", p, _gp);
                var p1 = new Player("shachar2", kobi);
                r.AddPlayer(p1);
                ml.PlayerWisper("fuck", p1, p.User, r);

            }
            catch (Exception e)
            {
                Assert.AreEqual(
                   e.Message, "cant send empty message / curses");
            }
        }
    }
} 