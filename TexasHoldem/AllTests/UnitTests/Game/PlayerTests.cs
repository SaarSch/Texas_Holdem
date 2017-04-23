using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllTests.UnitTests.Game
{
    [TestClass()]
    public class PlayerTests
    {
      
        [TestMethod()]
        public void SetCardsTest()
        {
            Player p = new Player("shachar", new User("tom1234555", "12345678", "aaa.png", "hello@gmail.com",50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            Assert.IsTrue(p.Hand.Length == 2);
            Assert.IsTrue(p.Hand[0].value==14&& p.Hand[0].type==CardType.Clubs&& p.Hand[1].value == 2 && p.Hand[1].type == CardType.Clubs);
        }

        [TestMethod()]
        public void SetBetTest()
        {
            Player p = new Player("shachar", new User("tom12346", "12345678", "aaa.png", "hello@gmail.com",50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.ChipsAmount = 50000;
            int chip = p.ChipsAmount;
            p.SetBet(500);
            Assert.IsTrue(p.CurrentBet == 500 && p.ChipsAmount + 500 == chip);
        }

        [TestMethod()]
        public void ClearBetTest()
        {
            Player p = new Player("shachar", new User("tom12347", "12345678", "aaa.jpeg", "hello@gmail.com",50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.ChipsAmount = 50000;
            p.SetBet(500);
            p.ClearBet();
            Assert.IsTrue(p.CurrentBet == 0);
        }

        [TestMethod()]
        public void FoldTest()
        {
            Player p = new Player("shachar", new User("tom12348", "12345678", "aaa.jpg", "hello@gmail.com",50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.Fold();
            Assert.IsTrue(p.Folded);

        }
    }
}