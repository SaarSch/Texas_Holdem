using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Game;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void AllCardsExist()
        {
            var d = new Deck();
            for (var j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Clubs));
            for (var j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Diamonds));
            for (var j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Hearts));
            for (var j = 2; j < 15; j++) Assert.IsTrue(d.Contains(j, CardType.Spades));
        }

        [TestMethod]
        public void ShuffleTest()
        {
            var d = new Deck();
            var testA = d.Cards.ToArray();
            d.Shuffle();
            var testB = d.Cards.ToArray();

            var ans = false;
            for (var i = 0; i < 52; i++) if (testA[i] != testB[i]) ans = true;
            Assert.IsTrue(ans);
        }


        [TestMethod]
        public void DrawTest()
        {
            var d = new Deck();
            var test = d.Draw();
            Assert.IsTrue(d.Cards.Count == 51);
            Assert.IsTrue(!d.Contains(test.Value, test.Type));
        }

        [TestMethod]
        public void ComperCardTest()
        {
            var d = new Card(2, CardType.Clubs);
            var d1 = new Card(2, CardType.Clubs);
            Assert.IsTrue(d.CompareTo(d1) == 0);
        }

        [TestMethod]
        public void ComperCardTestFalse()
        {
            var d = new Card(3, CardType.Clubs);
            var d1 = new Card(2, CardType.Clubs);
            Assert.IsTrue(d.CompareTo(d1) == -1);
        }
    }
}