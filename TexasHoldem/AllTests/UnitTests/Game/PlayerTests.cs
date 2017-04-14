using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class PlayerTests
    {
      
        [TestMethod()]
        public void SetCardsTest()
        {
            Player p = new Player("shachar", 0, new User("tom", "123", "aaa"));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            Assert.IsTrue(p.Hand.Length == 2);
            Assert.IsTrue(p.Hand[0].value==14&& p.Hand[0].type==CardType.Clubs&& p.Hand[1].value == 2 && p.Hand[1].type == CardType.Clubs);
        }

        [TestMethod()]
        public void SetBetTest()
        {
            Player p = new Player("shachar", 600, new User("tom", "123", "aaa"));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            int chip = p.ChipsAmount;
            p.SetBet(500);
            Assert.IsTrue(p.CurrentBet == 500 && p.ChipsAmount + 500 == chip);
        }

        [TestMethod()]
        public void ClearBetTest()
        {
            Player p = new Player("shachar", 600, new User("tom", "123", "aaa"));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.SetBet(500);
            p.ClearBet();
            Assert.IsTrue(p.CurrentBet == 0);
        }

        [TestMethod()]
        public void FoldTest()
        {
            Player p = new Player("shachar", 600, new User("tom", "123", "aaa"));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.Fold();
            Assert.IsTrue(p.Folded);

        }
    }
}