using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TexasHoldem.Game;
using TexasHoldem.Users;

namespace AllTests.UnitTests.Game
{
    [TestClass]
    public class RoomTests
    {
        private readonly GamePreferences _gp = new GamePreferences();
        private IUser _u ;
        private IUser _u1 ;
        private IUser _u2 ;
        private IUser _u3;
        private IUser _u4;
        private IPlayer _p;
        private IPlayer _p1;
        private IPlayer _p2;
        private IPlayer _p3;
        private IPlayer _p4;
        private ICard _card1Mock;
        private ICard _card2Mock;

        private void mokupdatehand(ICard d1, ICard d2, IPlayer p)
        {
            p.Hand[0] = d1;
            p.Hand[1] = d2;
        }

        private void mokSetBet(int amount, IPlayer p)
        {
            p.CurrentBet += amount;
            p.ChipsAmount -= amount;
            p.PreviousRaise = amount;
            p.BetInThisRound = true;
        }

        private void mokAddNotification(string Room, string notif, IUser u)
        {
            u.Notifications.Add(new Tuple<string, string>(Room, notif));
        }

        [TestInitialize]
        public void Initialize()
        {
            var userMock = new Mock<IUser>();
            var userMock1 = new Mock<IUser>();
            var userMock2 = new Mock<IUser>();
            var userMock3 = new Mock<IUser>();
            var userMock4 = new Mock<IUser>();
            var card1Mock = new Mock<ICard>();
            var card2Mock = new Mock<ICard>();
            var pMock = new Mock<IPlayer>();
            var p1Mock = new Mock<IPlayer>();
            var p2Mock = new Mock<IPlayer>();
            var p3Mock = new Mock<IPlayer>();
            var p4Mock = new Mock<IPlayer>();

            card1Mock.Setup(card => card.Value).Returns(14);
            card1Mock.Setup(card => card.Type).Returns(CardType.Clubs);

            card2Mock.Setup(card => card.Value).Returns(2);
            card2Mock.Setup(card => card.Type).Returns(CardType.Clubs);

            userMock.Setup(user => user.GetUsername()).Returns("tom12345");
            userMock.Setup(user => user.GetPassword()).Returns("12345678");
            userMock.Setup(user => user.GetAvatar()).Returns("aaa.png");
            userMock.Setup(user => user.GetEmail()).Returns("hello@gmail.com");
            userMock.Setup(user => user.ChipsAmount).Returns(50000);
            userMock.SetupProperty(user => user.Notifications);
            _u = userMock.Object;
            _u.Notifications = new List<Tuple<string, string>>();
            userMock.Setup(user => user.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((Room, notif) => mokAddNotification(Room, notif, _u));

            userMock1.Setup(user => user.GetUsername()).Returns("tom12346");
            userMock1.Setup(user => user.GetAvatar()).Returns("bbb.png");
            userMock1.Setup(user => user.GetEmail()).Returns("hello1@gmail.com");
            userMock1.Setup(user => user.ChipsAmount).Returns(50000);
            userMock1.Setup(user => user.GetPassword()).Returns("12345678");
            userMock1.SetupProperty(user => user.Notifications);
            _u1 = userMock1.Object;
            _u1.Notifications = new List<Tuple<string, string>>();
            userMock1.Setup(user => user.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((Room, notif) => mokAddNotification(Room, notif, _u1));

            userMock2.Setup(user => user.GetUsername()).Returns("tom12347");
            userMock2.Setup(user => user.GetAvatar()).Returns("ccc.png");
            userMock2.Setup(user => user.GetEmail()).Returns("hello3@gmail.com");
            userMock2.Setup(user => user.ChipsAmount).Returns(50000);
            userMock2.Setup(user => user.GetPassword()).Returns("12345678");
            userMock2.SetupProperty(user => user.Notifications);
            _u2 = userMock2.Object;
            _u2.Notifications = new List<Tuple<string, string>>();
            userMock2.Setup(user => user.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((Room, notif) => mokAddNotification(Room, notif, _u2));

            userMock3.Setup(user => user.GetUsername()).Returns("tom12348");
            userMock3.Setup(user => user.GetAvatar()).Returns("ccc.png");
            userMock3.Setup(user => user.GetEmail()).Returns("hello3@gmail.com");
            userMock3.Setup(user => user.ChipsAmount).Returns(50000);
            userMock3.Setup(user => user.GetPassword()).Returns("12345678");
            userMock3.SetupProperty(user => user.Notifications);
            _u3 = userMock3.Object;
            _u3.Notifications = new List<Tuple<string, string>>();
            userMock3.Setup(user => user.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((Room, notif) => mokAddNotification(Room, notif, _u3));

            userMock4.Setup(user => user.GetUsername()).Returns("tom12349");
            userMock4.Setup(user => user.GetAvatar()).Returns("ccc.png");
            userMock4.Setup(user => user.GetEmail()).Returns("hello3@gmail.com");
            userMock4.Setup(user => user.ChipsAmount).Returns(50000);
            userMock4.Setup(user => user.GetPassword()).Returns("12345678");
            userMock4.SetupProperty(user => user.Notifications);
            _u4 = userMock4.Object;
            _u4.Notifications = new List<Tuple<string, string>>();
            userMock4.Setup(user => user.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((Room, notif) => mokAddNotification(Room, notif, _u4));

            pMock.Setup(plyer => plyer.Name).Returns("shachar");
            pMock.Setup(plyer => plyer.User).Returns(_u);
            pMock.SetupProperty(plyer => plyer.Hand);
            pMock.SetupProperty(plyer => plyer.PreviousRaise);
            pMock.SetupProperty(plyer => plyer.BetInThisRound);
            pMock.SetupProperty(plyer => plyer.CurrentBet);
            pMock.SetupProperty(plyer => plyer.ChipsAmount);
            pMock.SetupProperty(plyer => plyer.StrongestHand);
            pMock.SetupProperty(plyer => plyer.Folded);
            _p = pMock.Object;
            _p.Hand = new ICard[2];
            pMock.Setup(plyer => plyer.SetCards(It.IsAny<ICard>(), It.IsAny<ICard>()))
                .Callback<ICard, ICard>((d1, d2) => mokupdatehand(d1, d2, _p));
            pMock.Setup(plyer => plyer.SetBet(It.IsAny<int>())).Callback<int>(amount => mokSetBet(amount, _p));

            p1Mock.Setup(plyer => plyer.Name).Returns("shachar1");
            p1Mock.Setup(plyer => plyer.User).Returns(_u1);
            p1Mock.SetupProperty(plyer => plyer.Hand);
            p1Mock.SetupProperty(plyer => plyer.PreviousRaise);
            p1Mock.SetupProperty(plyer => plyer.BetInThisRound);
            p1Mock.SetupProperty(plyer => plyer.CurrentBet);
            p1Mock.SetupProperty(plyer => plyer.ChipsAmount);
            p1Mock.SetupProperty(plyer => plyer.StrongestHand);
            p1Mock.SetupProperty(plyer => plyer.Folded);
            _p1 = p1Mock.Object;
            _p1.Hand = new ICard[2];
            p1Mock.Setup(plyer => plyer.SetCards(It.IsAny<ICard>(), It.IsAny<ICard>()))
                .Callback<ICard, ICard>((d1, d2) => mokupdatehand(d1, d2, _p1));
            p1Mock.Setup(plyer => plyer.SetBet(It.IsAny<int>())).Callback<int>(amount => mokSetBet(amount, _p1));

            p2Mock.Setup(plyer => plyer.Name).Returns("shachar2");
            p2Mock.Setup(plyer => plyer.User).Returns(_u2);
            p2Mock.SetupProperty(plyer => plyer.Hand);
            p2Mock.SetupProperty(plyer => plyer.PreviousRaise);
            p2Mock.SetupProperty(plyer => plyer.BetInThisRound);
            p2Mock.SetupProperty(plyer => plyer.CurrentBet);
            p2Mock.SetupProperty(plyer => plyer.ChipsAmount);
            p2Mock.SetupProperty(plyer => plyer.StrongestHand);
            p2Mock.SetupProperty(plyer => plyer.Folded);
            _p2 = p2Mock.Object;
            _p2.Hand = new ICard[2];
            p2Mock.Setup(plyer => plyer.SetCards(It.IsAny<ICard>(), It.IsAny<ICard>()))
                .Callback<ICard, ICard>((d1, d2) => mokupdatehand(d1, d2, _p2));
            p2Mock.Setup(plyer => plyer.SetBet(It.IsAny<int>())).Callback<int>(amount => mokSetBet(amount, _p2));

            p3Mock.Setup(plyer => plyer.Name).Returns("shachar3");
            p3Mock.Setup(plyer => plyer.User).Returns(_u3);
            p3Mock.SetupProperty(plyer => plyer.Hand);
            p3Mock.SetupProperty(plyer => plyer.PreviousRaise);
            p3Mock.SetupProperty(plyer => plyer.BetInThisRound);
            p3Mock.SetupProperty(plyer => plyer.CurrentBet);
            p3Mock.SetupProperty(plyer => plyer.ChipsAmount);
            p3Mock.SetupProperty(plyer => plyer.StrongestHand);
            p3Mock.SetupProperty(plyer => plyer.Folded);
            _p3 = p3Mock.Object;
            _p3.Hand = new ICard[2];
            p3Mock.Setup(plyer => plyer.SetCards(It.IsAny<ICard>(), It.IsAny<ICard>()))
                .Callback<ICard, ICard>((d1, d2) => mokupdatehand(d1, d2, _p3));
            p3Mock.Setup(plyer => plyer.SetBet(It.IsAny<int>())).Callback<int>(amount => mokSetBet(amount, _p3));

            p4Mock.Setup(plyer => plyer.Name).Returns("shachar4");
            p4Mock.Setup(plyer => plyer.User).Returns(_u4);
            p4Mock.SetupProperty(plyer => plyer.Hand);
            p4Mock.SetupProperty(plyer => plyer.PreviousRaise);
            p4Mock.SetupProperty(plyer => plyer.BetInThisRound);
            p4Mock.SetupProperty(plyer => plyer.CurrentBet);
            p4Mock.SetupProperty(plyer => plyer.ChipsAmount);
            p4Mock.SetupProperty(plyer => plyer.StrongestHand);
            p4Mock.SetupProperty(plyer => plyer.Folded);
            _p4 = p4Mock.Object;
            _p4.Hand = new ICard[2];
            p4Mock.Setup(plyer => plyer.SetCards(It.IsAny<ICard>(), It.IsAny<ICard>()))
                .Callback<ICard, ICard>((d1, d2) => mokupdatehand(d1, d2, _p4));
            p4Mock.Setup(plyer => plyer.SetBet(It.IsAny<int>())).Callback<int>(amount => mokSetBet(amount, _p4));

            _card1Mock = card1Mock.Object;
            _card2Mock = card2Mock.Object;
        }

        [TestMethod]
        public void AddPlayerTest()
        {
            var r = new Room("aaaa", _p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            r.AddPlayer(_p1);
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
            var r = new Room("aaaa", _p1, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            r.AddPlayer(_p);
            Assert.IsTrue(r.Players.Count == 2);
            r.ExitRoom("shachar");
            Assert.IsTrue(r.Players.Count == 1);
        }

        [TestMethod]
        public void DealTwoTest()
        {
            var r = new Room("aaaa", _p, _gp);
            Assert.IsTrue(r.Players.Count == 1);
            r.AddPlayer(_p1);
            r.AddPlayer(_p2);
            r.AddPlayer(_p3);
            r.AddPlayer(_p4);
            Assert.IsTrue(r.Players.Count == 5);
            r.DealTwo();
            foreach (var p in r.Players)
                Assert.IsTrue(p.Hand.Length == 2);
        }

        [TestMethod]
        public void DealCommunityFirstTest()
        {
            var r = new Room("aaaa", _p, _gp);
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
            var r = new Room("aaaa", _p, _gp);
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
           // var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);

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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);

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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);

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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);

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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);

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
            //var p = new Player("shachar", new User("tom12345a", "12345678", "aaa.png", "hello@gmail.com", 50000));
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, gp1);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, gp1);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, gp1);
            try
            {
                r.SetBet(_p, 0, false);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, gp1);
            try
            {
                _p.PreviousRaise = 30;
                _p.BetInThisRound = true;
                r.SetBet(_p, 10, false);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, gp1);
            try
            {
                r.SetBet(_p, 120, false);
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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, gp1)
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
                r.SetBet(_p, 120, false);
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

            //var p = new Player("shachar", _u);
            //var p2 = new Player("shachar1", _u);
            var r = new Room("aaaa", _p, gp);
            r.AddPlayer(_p1);
            _p1.CurrentBet = 500;
            try
            {
                r.SetBet(_p, 600, false);
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

            //var p = new Player("shachar", _u);
            //var p2 = new Player("shachar2", _u);
            var r = new Room("aaaa", _p, gp);
            r.AddPlayer(_p1);
            _p.ChipsAmount = 60000;
            _p1.CurrentBet = 500;
            r.SetBet(_p, 300, false);
            Assert.IsTrue(_p.CurrentBet == 300);
        }

        [TestMethod]
        public void HandCalculatorStraightStraightTest()
        {
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);


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
           // var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
            
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
           // var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
            //var p1 = new Player("shachar1", _u1);

            r.AddPlayer(_p1);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(13, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            _p.Hand[0] = new Card(13, CardType.Clubs);
            _p.Hand[1] = new Card(13, CardType.Hearts);

            _p1.Hand[0] = new Card(12, CardType.Clubs);
            _p1.Hand[1] = new Card(12, CardType.Hearts);

            Assert.IsTrue(r.Winners().Count == 1 && r.Winners()[0] == _p);
        }

        [TestMethod]
        public void WinnersSameHandRankTest()
        {
          //  var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
           // var p1 = new Player("shachar2", _u1);
            //var p2 = new Player("shachar1", _u2);
            r.AddPlayer(_p2);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            _p.Hand[0] = new Card(2, CardType.Diamonds);
            _p.Hand[1] = new Card(13, CardType.Spades);

            _p2.Hand[0] = new Card(5, CardType.Clubs);
            _p2.Hand[1] = new Card(13, CardType.Hearts);

            _p1.Hand[0] = new Card(7, CardType.Clubs);
            _p1.Hand[1] = new Card(7, CardType.Hearts);

            r.AddPlayer(_p1);

            Assert.IsTrue(r.Winners().Count == 1 && r.Winners()[0] == _p2);
        }

        [TestMethod]
        public void WinnersTieTest()
        {
            //var p = new Player("shachar", _u);
            var r = new Room("aaaa", _p, _gp);
            //var p1 = new Player("shachar2", _u1);
            //var p2 = new Player("shachar3", _u2);
            r.AddPlayer(_p2);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            _p.Hand[0] = new Card(5, CardType.Diamonds);
            _p.Hand[1] = new Card(13, CardType.Spades);

            _p2.Hand[0] = new Card(5, CardType.Clubs);
            _p2.Hand[1] = new Card(13, CardType.Hearts);

            _p3.Hand[0] = new Card(7, CardType.Clubs);
            _p3.Hand[1] = new Card(7, CardType.Hearts);

            r.AddPlayer(_p3);

            Assert.IsTrue(r.Winners().Count == 2 && r.Winners()[0] == _p && r.Winners()[1] == _p2);
        }

        [TestMethod]
        public void ChipsTest()
        {
            //var p = new Player("shachar1", _u);
            var r = new Room("aaaa", _p, _gp);
            //var p1 = new Player("shachar2", _u1);
            //var p2 = new Player("shachar3", _u2);
            r.AddPlayer(_p1);
            r.AddPlayer(_p2);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            _p.Hand[0] = new Card(5, CardType.Diamonds);
            _p.Hand[1] = new Card(13, CardType.Spades);

            _p1.Hand[0] = new Card(2, CardType.Clubs);
            _p1.Hand[1] = new Card(13, CardType.Hearts);

            _p2.Hand[0] = new Card(7, CardType.Clubs);
            _p2.Hand[1] = new Card(7, CardType.Hearts);

            _p.SetBet(300);
            _p1.SetBet(300);
            _p2.SetBet(300);
            r.IsOn = true;
            r.CalcWinnersChips();

            Assert.IsTrue(_p.ChipsAmount == 50600);
        }

        [TestMethod]
        public void NextTurnTest()
        {
            //var p = new Player("shachar1", _u);
            var r = new Room("aaaa", _p, _gp);
            //var p1 = new Player("shachar2", _u1);
            //var p2 = new Player("shachar3", _u2);
            r.AddPlayer(_p1);
            r.AddPlayer(_p2);
            r.CommunityCards[0] = new Card(13, CardType.Diamonds);
            r.CommunityCards[1] = new Card(11, CardType.Spades);
            r.CommunityCards[2] = new Card(14, CardType.Hearts);
            r.CommunityCards[3] = new Card(3, CardType.Clubs);
            r.CommunityCards[4] = new Card(4, CardType.Diamonds);

            _p.Hand[0] = new Card(5, CardType.Diamonds);
            _p.Hand[1] = new Card(13, CardType.Spades);

            _p1.Hand[0] = new Card(2, CardType.Clubs);
            _p1.Hand[1] = new Card(13, CardType.Hearts);

            _p2.Hand[0] = new Card(7, CardType.Clubs);
            _p2.Hand[1] = new Card(7, CardType.Hearts);
            r.IsOn = true;
            _p.SetBet(300);
            _p1.SetBet(300);
            _p2.SetBet(300);


            r.CalcWinnersChips();

            Assert.IsTrue(r.Players[0] == _p1 && r.Players[1] == _p2 && r.Players[2] == _p);
        }

        [TestMethod]
        public void SmallBigBlindTest()
        {
            //var p = new Player("shachar1", _u);
            var r = new Room("aaaa", _p, _gp);
            //var p1 = new Player("shachar2", _u1);
            //var p2 = new Player("shachar3", _u2);
            r.AddPlayer(_p1);
            r.AddPlayer(_p2);
            r.StartGame();


            Assert.IsTrue(r.Players[0].CurrentBet == 0 && r.Players[1].CurrentBet == 2 && r.Players[2].CurrentBet == 4);
        }

        [TestMethod]
        public void SmallBigBlind2PlayersTest()
        {
        //    var p = new Player("shachar1", _u);
            var r = new Room("aaaa", _p, _gp);
          //  var p1 = new Player("shachar2", _u1);
            r.AddPlayer(_p1);
            r.StartGame();


            Assert.IsTrue(r.Players[0].CurrentBet == 2 && r.Players[1].CurrentBet == 4);
        }

        [TestMethod]
        public void SpectateTest()
        {
        //    var p = new Player("shachar1", _u);
            var r = new Room("aaaa", _p, _gp);
            r.Spectate(_u);
            Assert.IsTrue(r.SpectateUsers.Contains(_u));
        }

        [TestMethod]
        public void NotifyTest()
        {
            var ml = new MessageLogic();
            const string message = "wow you are so cool!";
            //var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
            //var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
            //var p = new Player("shachar1", yossi);
            var r = new Room("aaaa", _p, _gp);
            //var p1 = new Player("shachar2", kobi);
            r.AddPlayer(_p1);
            ml.NotifyRoom(message,r);

            foreach (var p2 in r.Players)
                Assert.AreEqual(
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + message,
                    p2.User.Notifications[0].Item2);
        }
    }
}