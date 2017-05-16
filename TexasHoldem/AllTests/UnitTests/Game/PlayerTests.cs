using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TexasHoldem.Game;
using TexasHoldem.Users;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void SetCardsTest()
        {
            var userMock = new Mock<IUser>();
            var card1Mock = new Mock<ICard>();
            var card2Mock = new Mock<ICard>();

            card1Mock.Setup(card=>card.Value).Returns(14);
            card1Mock.Setup(card => card.Type).Returns(CardType.Clubs);

            card2Mock.Setup(card => card.Value).Returns(2);
            card2Mock.Setup(card => card.Type).Returns(CardType.Clubs);

            userMock.Setup(user => user.GetUsername()).Returns("tom1234555");
            userMock.Setup(user => user.GetPassword()).Returns("12345678");
            userMock.Setup(user => user.GetAvatar()).Returns("aaa.png");
            userMock.Setup(user => user.GetEmail()).Returns("hello@gmail.com");

            var p = new Player("shachar", userMock.Object);
            p.SetCards(card1Mock.Object, card2Mock.Object);
            Assert.IsTrue(p.Hand.Length == 2);
            Assert.IsTrue(p.Hand[0].Value == 14 && p.Hand[0].Type == CardType.Clubs && p.Hand[1].Value == 2 &&
                          p.Hand[1].Type == CardType.Clubs);
        }

        [TestMethod]
        public void SetCardsTestFalse()
        {
            var p = new Player("shachar", new User("tom1234555", "12345678", "aaa.png", "hello@gmail.com", 50000));
            try
            {
                p.SetCards(null, new Card(2, CardType.Clubs));
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("can't put null cards in player hand"));
            }
        }

        [TestMethod]
        public void SetBetTest()
        {
            var p = new Player("shachar", new User("tom12346", "12345678", "aaa.png", "hello@gmail.com", 50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.ChipsAmount = 50000;
            var chip = p.ChipsAmount;
            p.SetBet(500);
            Assert.IsTrue(p.CurrentBet == 500 && p.ChipsAmount + 500 == chip);
        }

        [TestMethod]
        public void SetBetTestFlase()
        {
            var p = new Player("shachar", new User("tom12346", "12345678", "aaa.png", "hello@gmail.com", 50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.ChipsAmount = 50000;
            try
            {
                p.SetBet(-500);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("bet must be greater then zero and less - equal to player chips"));
            }
        }

        [TestMethod]
        public void ClearBetTest()
        {
            var p = new Player("shachar", new User("tom12347", "12345678", "aaa.jpeg", "hello@gmail.com", 50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.ChipsAmount = 50000;
            p.SetBet(500);
            p.ClearBet();
            Assert.IsTrue(p.CurrentBet == 0);
        }

        [TestMethod]
        public void FoldTest()
        {
            var p = new Player("shachar", new User("tom12348", "12345678", "aaa.jpg", "hello@gmail.com", 50000));
            p.SetCards(new Card(14, CardType.Clubs), new Card(2, CardType.Clubs));
            p.Fold();
            Assert.IsTrue(p.Folded);
        }
    }
}