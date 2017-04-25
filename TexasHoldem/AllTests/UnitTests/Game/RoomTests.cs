using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class RoomTests
    {
        private User u = new User("tom12345", "12345678", "aaa.png", "hello@gmail.com",50000);
        private User u1 = new User("tom12346", "12345678", "bbb.png", "hello1@gmail.com",50000);
        private User u2 = new User("tom12347", "12345678", "ccc.png", "hello3@gmail.com",50000);
        private GamePreferences  gp= new GamePreferences(Gametype.NoLimit,1,0,4,2,8,true);

        [TestMethod]
        public void AddPlayerTest()
        {
            Player p = new Player("shachar" ,u);
            Room r = new Room("aa", p, gp);
            Assert.IsTrue(r.players.Count == 1);
            Player p1 = new Player("shachar1", u1);
            r.AddPlayer(p1);
            Assert.IsTrue(r.players.Count == 2);
        }

        [TestMethod]
        public void GamePreferencesTest()
        {
            try
            {
                GamePreferences gp = new GamePreferences(Gametype.NoLimit, -8, 0, 4, 2, 8, true);
            }

            catch(Exception e)
            {
                Assert.IsTrue(e.Message.Equals("buy in policy  cant be negativ"));
            }
        }

        [TestMethod]
        public void GamePreferencesTest1()
        {
            try
            {
                GamePreferences gp = new GamePreferences(Gametype.NoLimit, 1, -8, 4, 2, 8, true);
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Chip policy value cant be negativ"));
            }
            try
            {
                GamePreferences gp = new GamePreferences(Gametype.NoLimit, 1, 2, 1, 2, 8, true);
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Minimum bet cant be less then 2"));
            }
            try
            {
                GamePreferences gp = new GamePreferences(Gametype.NoLimit, 1, 2, 4, 1, 8, true);
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Minimum players cant be less then 2"));
            }
            try
            {
                GamePreferences gp = new GamePreferences(Gametype.NoLimit, 1, 2, 4, 2, 15, true);
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("Maximum players cant be more then 9"));
            }
            try
            {
                GamePreferences gp = new GamePreferences(Gametype.NoLimit, 1, 2, 4, 2, 8, true);
            }

            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("min bet cant be higher the chip policy"));
            }
        }

        [TestMethod]
        public void RemovePlayerTest()
        {
            Player p = new Player("shachar1", u);
            Room r = new Room("aa", p, gp);
            Assert.IsTrue(r.players.Count == 1);
            Player p1 = new Player("shachar", u1);
            r.AddPlayer(p1);
            Assert.IsTrue(r.players.Count == 2);
            r.ExitRoom("shachar");
            Assert.IsTrue(r.players.Count == 1);
        }

        [TestMethod]
        public void DealTwoTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);
            Assert.IsTrue(r.players.Count == 1);
            Player p1 = new Player("shachar3", u1);
            Player p3 = new Player("shachar2",  new User("tom12345f", "12345678", "gggg.png", "hello@gmail.com",50000));
            Player p4 = new Player("shachar14",  new User("tom12345g", "12345678", "eeee.png", "hello@gmail.com", 50000));
            Player p5 = new Player("shachar4",  new User("tom12345h", "12345678", "jgjg.png", "hello@gmail.com", 50000));
            r.AddPlayer(p1);
            r.AddPlayer(p3);
            r.AddPlayer(p4);
            r.AddPlayer(p5);
            Assert.IsTrue(r.players.Count == 5);
            r.DealTwo();
            foreach(Player p2 in r.players)
            {
                Assert.IsTrue(p.Hand.Length == 2);
            }
        }

        [TestMethod]
        public void DealCommunityFirstTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p,gp);
            Assert.IsTrue(r.players.Count == 1);
            r.IsOn = true;
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.cards.Count == 47);
            Assert.IsNotNull(r.communityCards[0]);
            Assert.IsNotNull(r.communityCards[1]);
        }

        [TestMethod]
        public void DealCommunitySecondTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p,gp);
            Assert.IsTrue(r.players.Count == 1);
            r.IsOn = true;
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.cards.Count == 47);
            Assert.IsNotNull(r.communityCards[0]);
            Assert.IsNotNull(r.communityCards[1]);
            r.DealCommunitySecond();
            Assert.IsTrue(r.Deck.cards.Count == 46);
            Assert.IsNotNull(r.communityCards[2]);
        }

        [TestMethod]
        public void DealCommunityThirdTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p,gp);
            Assert.IsTrue(r.players.Count == 1);
            r.IsOn = true;
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.cards.Count == 47);
            Assert.IsNotNull(r.communityCards[0]);
            Assert.IsNotNull(r.communityCards[1]);
            Assert.IsNotNull(r.communityCards[2]);
            r.DealCommunitySecond();
            Assert.IsTrue(r.Deck.cards.Count == 46);
            Assert.IsNotNull(r.communityCards[3]);
            r.DealCommunityThird();
            Assert.IsTrue(r.Deck.cards.Count == 45);
            Assert.IsNotNull(r.communityCards[4]);
        }

        [TestMethod]
        public void HandCalculatorRoyalStraightTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p,gp);
            List < Card > win= new List<Card>();// royal flush
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(13, CardType.Clubs));
            win.Add(new Card(12, CardType.Clubs));
            win.Add(new Card(11, CardType.Clubs));
            win.Add(new Card(10, CardType.Clubs));
            win.Add(new Card(9, CardType.Clubs));
            win.Add(new Card(8, CardType.Clubs));

            List<Card> loss = new List<Card>();// straight flush
            loss.Add(new Card(3, CardType.Clubs));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(5, CardType.Clubs));
            loss.Add(new Card(6, CardType.Clubs));
            loss.Add(new Card(7, CardType.Clubs));
            loss.Add(new Card(8, CardType.Clubs));
            loss.Add(new Card(9, CardType.Clubs));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraight4OfTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);
            List<Card> win = new List<Card>();// straight flush
            win.Add(new Card(7, CardType.Clubs));
            win.Add(new Card(2, CardType.Clubs));
            win.Add(new Card(3, CardType.Clubs));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(5, CardType.Clubs));
            win.Add(new Card(6, CardType.Clubs));
            win.Add(new Card(7, CardType.Clubs));

            List<Card> loss = new List<Card>();// 4 of a kind
            loss.Add(new Card(6, CardType.Clubs));
            loss.Add(new Card(3, CardType.Clubs));
            loss.Add(new Card(12, CardType.Clubs));
            loss.Add(new Card(10, CardType.Spades));
            loss.Add(new Card(10, CardType.Clubs));
            loss.Add(new Card(10, CardType.Hearts));
            loss.Add(new Card(10, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculator4OfFullTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);
            List<Card> win = new List<Card>();//4 of a kind
            win.Add(new Card(7, CardType.Clubs));
            win.Add(new Card(7, CardType.Spades));
            win.Add(new Card(7, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(5, CardType.Clubs));
            win.Add(new Card(6, CardType.Clubs));
            win.Add(new Card(14, CardType.Clubs));

            List<Card> loss = new List<Card>();//full house
            loss.Add(new Card(6, CardType.Clubs));
            loss.Add(new Card(6, CardType.Hearts));
            loss.Add(new Card(6, CardType.Diamonds));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(2, CardType.Clubs));
            loss.Add(new Card(10, CardType.Hearts));
            loss.Add(new Card(8, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorFullFlushTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);

            List<Card> win = new List<Card>();//full house
            win.Add(new Card(6, CardType.Clubs));
            win.Add(new Card(6, CardType.Hearts));
            win.Add(new Card(6, CardType.Diamonds));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(2, CardType.Clubs));
            win.Add(new Card(10, CardType.Hearts));
            win.Add(new Card(8, CardType.Diamonds));

            List<Card> loss = new List<Card>();//flush
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(8, CardType.Diamonds));
            loss.Add(new Card(11, CardType.Diamonds));
            loss.Add(new Card(2, CardType.Diamonds));
            loss.Add(new Card(9, CardType.Diamonds));
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorFlushStraightTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);

            List<Card> win = new List<Card>();//flush
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(8, CardType.Diamonds));
            win.Add(new Card(11, CardType.Diamonds));
            win.Add(new Card(2, CardType.Diamonds));
            win.Add(new Card(9, CardType.Diamonds));
            win.Add(new Card(13, CardType.Diamonds));
            win.Add(new Card(4, CardType.Diamonds));

            List<Card> loss = new List<Card>();//straight
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(5, CardType.Diamonds));
            loss.Add(new Card(6, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraight3OfTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);

            List<Card> win = new List<Card>();//straight
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(5, CardType.Diamonds));
            win.Add(new Card(6, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));

            List<Card> loss = new List<Card>();//3 of a kind
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(14, CardType.Clubs));
            loss.Add(new Card(5, CardType.Diamonds));
            loss.Add(new Card(14, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculator3Of2PairsTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);

            List<Card> win = new List<Card>();//3 of a kind
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(5, CardType.Diamonds));
            win.Add(new Card(14, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));

            List<Card> loss = new List<Card>();//2 pairs
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(14, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(7, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));
            
            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculator2PairsPairTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);

            List<Card> win = new List<Card>();//2 pairs
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(7, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();//pair
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(14, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void BHandCalculatorPairHighCardTest()
        {
            Player p = new Player("shachar", new User("tom12345a", "12345678", "aaa.png", "hello@gmail.com",50000));
            Room r = new Room("aa", p, gp);

        
            List<Card> win = new List<Card>();//pair
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();// high card
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(13, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorPairPairTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();//pair
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();// pair
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(13, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculator2Pair2PairTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();// 2 pair
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(13, CardType.Spades));
            win.Add(new Card(13, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();//  2 pair
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(13, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(2, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorHighHighTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();//high card
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(10, CardType.Clubs));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();// high card
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(8, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorHighHighTest1()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.limit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp1);


            List<Card> win = new List<Card>();//high card
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(10, CardType.Clubs));
            win.Add(new Card(2, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();// high card
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(8, CardType.Clubs));
            loss.Add(new Card(2, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void SetBetTest()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.limit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp1);
            try { r.SetBet(null, 1000); }
            catch(Exception e)
            {
                Assert.IsTrue(e.Message.Equals("player cant be null"));
            }
     
        }

        [TestMethod]
        public void SetBetTest2()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.limit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp1);
            try { r.SetBet(p, 0); }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("cant bet less then min bet"));
            }

        }

        [TestMethod]
        public void SetBetTest3()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.NoLimit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp1);
            try { p.previousRaise = 30; p.betInThisRound = true; r.SetBet(p, 10); }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("cant bet less then previous raise in no limit mode"));
            }

        }

        [TestMethod]
        public void SetBetTest4()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.limit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp1);
            try { r.SetBet(p, 120); }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("in pre flop/flop in limit mode bet must be equal to big blind"));
            }

        }

        [TestMethod]
        public void SetBetTest5()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.limit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp1);
            r.communityCards[0] = new Card(5, CardType.Clubs);
            r.communityCards[1] = new Card(5, CardType.Clubs);
            r.communityCards[2] = new Card(5, CardType.Clubs);
            r.communityCards[3] = new Card(5, CardType.Clubs);
            try { r.SetBet(p, 120); }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("in pre turn/river in limit mode bet must be equal to 2*big blind"));
            }

        }

        [TestMethod]
        public void SetBetTest6()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.PotLimit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Player p2 = new Player("shachar1", u);
            Room r = new Room("aa", p, gp1);
            r.AddPlayer(p2);
            p2.CurrentBet = 500;
            try { r.SetBet(p, 600); }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("in limit pot mode bet must lower then pot"));
            }

        }

        [TestMethod]
        public void SetBetTest7()
        {
            GamePreferences gp1 = new GamePreferences(Gametype.PotLimit, 1, 30, 10, 3, 8, true);
            Player p = new Player("shachar", u);
            Player p2 = new Player("shachar2", u);
            Room r = new Room("aa", p, gp1);
            r.AddPlayer(p2);
            p.ChipsAmount = 60000;
            p2.CurrentBet = 500;
            r.SetBet(p, 300);
            Assert.IsTrue(p.CurrentBet == 300);

        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();//straight
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(13, CardType.Clubs));
            win.Add(new Card(12, CardType.Spades));
            win.Add(new Card(11, CardType.Hearts));
            win.Add(new Card(12, CardType.Diamonds));
            win.Add(new Card(10, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();//straight
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(7, CardType.Clubs));
            loss.Add(new Card(6, CardType.Spades));
            loss.Add(new Card(5, CardType.Hearts));
            loss.Add(new Card(4, CardType.Diamonds));
            loss.Add(new Card(3, CardType.Clubs));
            loss.Add(new Card(2, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest1()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();//straight
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(13, CardType.Clubs));
            win.Add(new Card(12, CardType.Clubs));
            win.Add(new Card(11, CardType.Clubs));
            win.Add(new Card(12, CardType.Clubs));
            win.Add(new Card(10, CardType.Clubs));
            win.Add(new Card(9, CardType.Clubs));

            List<Card> loss = new List<Card>();//straight
            loss.Add(new Card(8, CardType.Diamonds));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(6, CardType.Diamonds));
            loss.Add(new Card(5, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Diamonds));
            loss.Add(new Card(3, CardType.Diamonds));
            loss.Add(new Card(2, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest2()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();//straight
            win.Add(new Card(2, CardType.Clubs));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(12, CardType.Clubs));
            win.Add(new Card(11, CardType.Clubs));
            win.Add(new Card(13, CardType.Clubs));
            win.Add(new Card(10, CardType.Clubs));
            win.Add(new Card(9, CardType.Clubs));

            List<Card> loss = new List<Card>();//straight
            loss.Add(new Card(8, CardType.Diamonds));
            loss.Add(new Card(7, CardType.Diamonds));
            loss.Add(new Card(6, CardType.Diamonds));
            loss.Add(new Card(5, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Diamonds));
            loss.Add(new Card(3, CardType.Diamonds));
            loss.Add(new Card(2, CardType.Diamonds));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest3()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);


            List<Card> win = new List<Card>();//straight
            win.Add(new Card(10, CardType.Spades));
            win.Add(new Card(9, CardType.Spades));
            win.Add(new Card(6, CardType.Diamonds));
            win.Add(new Card(5, CardType.Diamonds));
            win.Add(new Card(4, CardType.Hearts));
            win.Add(new Card(3, CardType.Clubs));
            win.Add(new Card(2, CardType.Hearts));

            List<Card> loss = new List<Card>();//straight
            loss.Add(new Card(12, CardType.Spades));
            loss.Add(new Card(11, CardType.Diamonds));
            loss.Add(new Card(6, CardType.Diamonds));
            loss.Add(new Card(5, CardType.Diamonds));
            loss.Add(new Card(3, CardType.Spades));
            loss.Add(new Card(3, CardType.Diamonds));
            loss.Add(new Card(3, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void HandCalculator3Of3OfTest()
        {
            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);

            List<Card> win = new List<Card>();//3 of a kind
            win.Add(new Card(14, CardType.Diamonds));
            win.Add(new Card(14, CardType.Clubs));
            win.Add(new Card(14, CardType.Spades));
            win.Add(new Card(3, CardType.Hearts));
            win.Add(new Card(7, CardType.Diamonds));
            win.Add(new Card(4, CardType.Clubs));
            win.Add(new Card(9, CardType.Hearts));

            List<Card> loss = new List<Card>();//3 of a kind
            loss.Add(new Card(13, CardType.Diamonds));
            loss.Add(new Card(13, CardType.Clubs));
            loss.Add(new Card(13, CardType.Spades));
            loss.Add(new Card(3, CardType.Hearts));
            loss.Add(new Card(14, CardType.Diamonds));
            loss.Add(new Card(4, CardType.Clubs));
            loss.Add(new Card(9, CardType.Hearts));

            Assert.IsTrue(r.HandCalculator(win).handStrongessValue > r.HandCalculator(loss).handStrongessValue);
        }

        [TestMethod]
        public void WinnersTest()
        {

            Player p = new Player("shachar", u);
            Room r = new Room("aa", p,gp);
            Player p1 = new Player("shachar1", u1);

            r.AddPlayer(p1);
            r.communityCards[0]= new Card(13, CardType.Diamonds);
            r.communityCards[1] = new Card(13, CardType.Spades);
            r.communityCards[2] = new Card(14, CardType.Hearts);
            r.communityCards[3] = new Card(3, CardType.Clubs);
            r.communityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0]= new Card(13, CardType.Clubs);
            p.Hand[1] = new Card(13, CardType.Hearts);

            p1.Hand[0]  = new Card(12, CardType.Clubs);
            p1.Hand[1] = new Card(12, CardType.Hearts);

            Assert.IsTrue (r.Winners().Count == 1&&r.Winners()[0]==p);
        }

        [TestMethod]
        public void WinnersSameHandRankTest()
        {

            Player p = new Player("shachar", u);
            Room r = new Room("aa", p,gp);
            Player p1 = new Player("shachar2", u1);
            Player p2 = new Player("shachar1", u2);
            r.AddPlayer(p1);
            r.communityCards[0] = new Card(13, CardType.Diamonds);
            r.communityCards[1] = new Card(11, CardType.Spades);
            r.communityCards[2] = new Card(14, CardType.Hearts);
            r.communityCards[3] = new Card(3, CardType.Clubs);
            r.communityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(2, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(5, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);

            r.AddPlayer(p2);

            Assert.IsTrue(r.Winners().Count == 1&& r.Winners()[0] == p1);
        }

        [TestMethod]
        public void WinnersTieTest()
        {

            Player p = new Player("shachar", u);
            Room r = new Room("aa", p, gp);
            Player p1 = new Player("shachar2",  u1);
            Player p2 = new Player("shachar3", u2);
            r.AddPlayer(p1);
            r.communityCards[0] = new Card(13, CardType.Diamonds);
            r.communityCards[1] = new Card(11, CardType.Spades);
            r.communityCards[2] = new Card(14, CardType.Hearts);
            r.communityCards[3] = new Card(3, CardType.Clubs);
            r.communityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(5, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(5, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);

            r.AddPlayer(p2);

            Assert.IsTrue(r.Winners().Count == 2 && r.Winners()[0]==p && r.Winners()[1] == p1);
        }

        [TestMethod]
        public void ChipsTest()
        {

            Player p = new Player("shachar1", u);
            Room r = new Room("aa", p, gp);
            Player p1 = new Player("shachar2", u1);
            Player p2 = new Player("shachar3", u2);
            r.AddPlayer(p1);
            r.AddPlayer(p2);
            r.communityCards[0] = new Card(13, CardType.Diamonds);
            r.communityCards[1] = new Card(11, CardType.Spades);
            r.communityCards[2] = new Card(14, CardType.Hearts);
            r.communityCards[3] = new Card(3, CardType.Clubs);
            r.communityCards[4] = new Card(4, CardType.Diamonds);

            p.Hand[0] = new Card(5, CardType.Diamonds);
            p.Hand[1] = new Card(13, CardType.Spades);

            p1.Hand[0] = new Card(2, CardType.Clubs);
            p1.Hand[1] = new Card(13, CardType.Hearts);

            p2.Hand[0] = new Card(7, CardType.Clubs);
            p2.Hand[1] = new Card(7, CardType.Hearts);

            p.SetBet(300);
            p1.SetBet(300);
            p2.SetBet(300);
            Player temp1 = r.players[0];
            r.IsOn = true;
            r.CalcWinnersChips();

            Assert.IsTrue(p.ChipsAmount==50600);
        }

        [TestMethod]
        public void NextTurnTest()
        {

            Player p = new Player("shachar1", u);
            Room r = new Room("aa", p, gp);
            Player p1 = new Player("shachar2", u1);
            Player p2 = new Player("shachar3", u2);
            r.AddPlayer(p1);
            r.AddPlayer(p2);
            r.communityCards[0] = new Card(13, CardType.Diamonds);
            r.communityCards[1] = new Card(11, CardType.Spades);
            r.communityCards[2] = new Card(14, CardType.Hearts);
            r.communityCards[3] = new Card(3, CardType.Clubs);
            r.communityCards[4] = new Card(4, CardType.Diamonds);

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

            Assert.IsTrue(r.players[0]==p1&&r.players[1]==p2&&r.players[2]==p);
        }

        [TestMethod]
        public void SmallBigBlindTest()
        {

            Player p = new Player("shachar1", u);
            Room r = new Room("aa", p,gp);
            Player p1 = new Player("shachar2", u1);
            Player p2 = new Player("shachar3", u2);
            r.AddPlayer(p1);
            r.AddPlayer(p2);
            r.StartGame();


            Assert.IsTrue(r.players[0].CurrentBet==0&& r.players[1].CurrentBet == 2&& r.players[2].CurrentBet == 4);
        }

        [TestMethod]
        public void SmallBigBlind2PlayersTest()
        {

            Player p = new Player("shachar1" , u);
            Room r = new Room("aa", p,gp);
            Player p1 = new Player("shachar2", u1);
            Player p2 = new Player("shachar3" , u2);
            r.AddPlayer(p1);
            r.StartGame();


            Assert.IsTrue(r.players[0].CurrentBet == 2 && r.players[1].CurrentBet == 4);
        }

        [TestMethod]
        public void SpectateTest()
        {

            Player p = new Player("shachar1", u);
            Room r = new Room("aa", p,gp);
            r.Spectate(u);
            Assert.IsTrue(r.spectateUsers.Contains(u));
        }

        [TestMethod]
        public void NotifyTest()
        {

            string message = "wow you are so cool!";
            User yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
            User kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
            Player p = new Player("shachar1", yossi);
            Room r = new Room("aa", p, gp);
            Player p1 = new Player("shachar2", kobi);
            r.AddPlayer(p1);
            r.NotifyRoom(message);

            foreach (Player p2 in r.players)
            {
                Assert.AreEqual(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + message,
                    p2.User.Notifications[0]);
            }
        }
    }
}
