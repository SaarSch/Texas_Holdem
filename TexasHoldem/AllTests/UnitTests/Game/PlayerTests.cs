using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TexasHoldem.Bridges;
using TexasHoldem.Game;
using TexasHoldem.Users;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class PlayerTests
    {
        private IUser _userMock ;
        private ICard _card1Mock ;
        private ICard _card2Mock ;

        [TestInitialize]
        public void Initialize()
        {
            var userMock = new Mock<IUser>();
            var card1Mock = new Mock<ICard>();
            var card2Mock = new Mock<ICard>();

            card1Mock.Setup(card => card.Value).Returns(14);
            card1Mock.Setup(card => card.Type).Returns(CardType.Clubs);

            card2Mock.Setup(card => card.Value).Returns(2);
            card2Mock.Setup(card => card.Type).Returns(CardType.Clubs);

            userMock.Setup(user => user.Username).Returns("tom1234555");
            userMock.Setup(user => user.Password).Returns("12345678");
            userMock.Setup(user => user.AvatarPath).Returns("aaa.png");
            userMock.Setup(user => user.Email).Returns("hello@gmail.com");

            _userMock = userMock.Object;
            _card1Mock = card1Mock.Object;
            _card2Mock = card2Mock.Object;
        }
        [TestMethod]
        public void SetCardsTest()
        {
            var p = new Player("shachar", _userMock);
            p.SetCards(_card1Mock, _card2Mock);
            Assert.IsTrue(p.Hand.Length == 2);
            Assert.IsTrue(p.Hand[0].Value == 14 && p.Hand[0].Type == CardType.Clubs && p.Hand[1].Value == 2 &&
                          p.Hand[1].Type == CardType.Clubs);
        }

        [TestMethod]
        public void SetCardsTestFalse()
        {
            var p = new Player("shachar", _userMock);
            try
            {
                p.SetCards(null, _card2Mock);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Equals("can't put null cards in player hand"));
            }
        }

        [TestMethod]
        public void SetBetTest()
        {
            var p = new Player("shachar", _userMock);
            p.SetCards(_card1Mock, _card2Mock);
            p.ChipsAmount = 50000;
            var chip = p.ChipsAmount;
            p.SetBet(500);
            Assert.IsTrue(p.CurrentBet == 500 && p.ChipsAmount + 500 == chip);
        }

        [TestMethod]
        public void SetBetTestFlase()
        {
            var p = new Player("shachar", _userMock);
            p.SetCards(_card1Mock, _card2Mock);
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
            var p = new Player("shachar", _userMock);
            p.SetCards(_card1Mock, _card2Mock);
            p.ChipsAmount = 50000;
            p.SetBet(500);
            p.ClearBet();
            Assert.IsTrue(p.CurrentBet == 0);
        }

        [TestMethod]
        public void FoldTest()
        {
            var p = new Player("shachar", _userMock);
            p.SetCards(_card1Mock, _card2Mock);
            p.Fold();
            Assert.IsTrue(p.Folded);
        }
    }
}