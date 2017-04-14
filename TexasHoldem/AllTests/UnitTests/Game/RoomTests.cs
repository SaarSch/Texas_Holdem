using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class RoomTests
    {
        [TestMethod]
        public void AddPlayerTest()
        {
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Assert.IsTrue(r.players.Count == 1);
            Player p1 = new Player("shachar", 0, new User("tom", "123", "aaa"));
            r.AddPlayer(p1);
            Assert.IsTrue(r.players.Count == 2);
        }

        [TestMethod]
        public void DealTwoTest()
        {
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Assert.IsTrue(r.players.Count == 1);
            Player p1 = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Player p3 = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Player p4 = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Player p5 = new Player("shachar", 0, new User("tom", "123", "aaa"));
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Assert.IsTrue(r.players.Count == 1);
            r.DealTwo();
            r.DealCommunityFirst();
            Assert.IsTrue(r.Deck.cards.Count == 47);
            Assert.IsNotNull(r.communityCards[0]);
            Assert.IsNotNull(r.communityCards[1]);
        }

        [TestMethod]
        public void DealCommunitySecondTest()
        {
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Assert.IsTrue(r.players.Count == 1);
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Assert.IsTrue(r.players.Count == 1);
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

        
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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);


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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);


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
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);


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
        public void HandCalculatorStraightStraightTest()
        {
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);


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
        public void HandCalculator3Of3OfTest()
        {
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);

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

            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Player p1 = new Player("shachar", 0, new User("tom", "123", "aaa"));
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

            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Player p1 = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Player p2 = new Player("shachar", 0, new User("tom", "123", "aaa"));
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

            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Player p1 = new Player("shachar", 0, new User("tom", "123", "aaa"));
            Player p2 = new Player("shachar", 0, new User("tom", "123", "aaa"));
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

            Player p = new Player("shachar", 600, new User("tom", "123", "aaa"));
            Room r = new Room("aa", p);
            Player p1 = new Player("shachar", 500, new User("tom", "123", "aaa"));
            Player p2 = new Player("shachar", 300, new User("tom", "123", "aaa"));
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
            r.CalcWinnersChips();

            Assert.IsTrue(p.ChipsAmount==1200);
        }
    }
}
