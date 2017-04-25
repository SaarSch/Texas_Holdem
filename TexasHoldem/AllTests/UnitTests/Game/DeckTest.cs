using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void AllCardsExist()
        {
            Deck d = new Deck();
            for (int j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Clubs));
            for (int j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Diamonds));
            for (int j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Hearts));
            for (int j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Spades));
        }

        [TestMethod]
        public void ShuffleTest()
        {
            Deck d = new Deck();
            Card[] testA = d.cards.ToArray();
            d.Shuffle();
            Card[] testB = d.cards.ToArray();

            Boolean ans = false;
            for (int i = 0; i < 52; i++) if (testA[i] != testB[i]) ans = true;
            Assert.IsTrue(ans);
        }

        
        [TestMethod]
        public void DrawTest()
        {
            Deck d = new Deck();
            Card test = d.Draw();
            Assert.IsTrue(d.cards.Count == 51);
            Assert.IsTrue(!d.Contains(test.value, test.type));
     
        }

        [TestMethod]
        public void ComperCardTest()
        {
            Card d = new Card(2, CardType.Clubs);
            Card d1 = new Card(2, CardType.Clubs);
            Assert.IsTrue(d.CompareTo(d1) == 0);

        }

        [TestMethod]
        public void ComperCardTestFalse()
        {
            Card d = new Card(3, CardType.Clubs);
            Card d1 = new Card(2, CardType.Clubs);
            Assert.IsTrue(d.CompareTo(d1) == -1);

        }
    }
}
