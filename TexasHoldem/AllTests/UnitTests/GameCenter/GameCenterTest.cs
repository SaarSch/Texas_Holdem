using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TexasHoldem.Game;
using TexasHoldem.Users;

namespace AllTests.UnitTests.GameCenter
{
    [TestClass]
    public class GameCenterTest
    {
        private readonly TexasHoldem.GameCenter _gc = TexasHoldem.GameCenter.GetGameCenter();
        private readonly UserLogic _ul = new UserLogic();
        private List<Tuple<IUser, bool>> _u;
        private List<Tuple<IUser, bool>> moqlist;
        [TestInitialize]
        public void Initialize()
        {
            _u=new List<Tuple<IUser, bool>>();
            moqlist = new List<Tuple<IUser, bool>>();
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_full()
        {
            var i = 0;
            setMoqUsers(20,false);
            _ul.SetLeagues(_u);
            foreach (var u in _u)
            {
                Assert.IsTrue(u.Item1.League == Math.Floor((double)i / 2) + 1);
                i++;
            }
        }

        private void setMoqUsers(int num,bool b)
        {
            _u.Clear();
            for (var i = 0; i < num; i++)
            {
                var userMock = new Mock<IUser>();
                userMock.SetupAllProperties();
                userMock.Setup(us => us.GetPassword()).Returns("12345678");
                userMock.Setup(us => us.GetUsername()).Returns("aaaaaaa" + i);
                userMock.Setup(us => us.Wins).Returns(i);
                userMock.Setup(us => us.ChipsAmount).Returns(50000);
                _u.Add(new Tuple<IUser, bool>(userMock.Object, b));
            }
        }
        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_full1()
        {
            var count = 0;
            setMoqUsers(60, false);
            _ul.SetLeagues(_u);
            foreach (var u in _u)
            {
                Assert.IsTrue(u.Item1.League == Math.Floor((double)count / 6) + 1);
                count++;
            }
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_not_Full()
        {
            var count = 0;
            setMoqUsers(8, false);
            _ul.SetLeagues(_u);
            foreach (var u in _u)
            {
                Assert.IsTrue(u.Item1.League == Math.Floor((double)count / 2) + 7);
                count++;
            }
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_Od()
        {
            setMoqUsers(21, false);
            _ul.SetLeagues(_u);
            for (var i = 1; i < 21; i++)
            {
                Assert.IsTrue(_u[i].Item1.League == Math.Ceiling((double)i / 2));
            }
            Assert.IsTrue(_u[0].Item1.League == 1);
        }


        [TestMethod]
        public void GameCenter_Register_UsernameWithSpaces()
        {
            
            var succ = true;
            
            try
            {
                _ul.Register("1234 5", "ssssssss", moqlist);
            }
            catch
            {
                succ = false;
            }
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Register_PasswordWithOnlyLetters()
        {
            var succ = true;
            try
            {
                _ul.Register("seanocheri", "sssssssss", moqlist);
            }
            catch
            {
                succ = false;
            }
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Register_OK()
        {
            var succ = true;
            try
            {
                _ul.Register("seanocheri", "123sean123", moqlist);
            }
            catch
            {
                succ = false;
            }
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_Register_SameOneTwice()
        {
            var succ = true;
            _ul.Register("example123", "123exm123", moqlist);
            try
            {
                _ul.Register("example123", "123exm123", moqlist);
            }
            catch
            {
                succ = false;
            }
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Login_WrongPassword()
        {
            var succ = true;
            setMoqUsers(1, false);
            try
            {
                _ul.Login("aaaaaaa0", "12345679", _u);
            }
            catch
            {
                succ = false;
            }
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Login_OK()
        {
            var succ = true;
            setMoqUsers(1, false);
            try
            {
                _ul.Login("aaaaaaa0", "12345678", _u);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                succ = false;
            }
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_Logout_OK()
        {
            var succ = true;
            setMoqUsers(1,true);
            try
            {
                _ul.Logout("aaaaaaa0", _u);
            }
            catch
            {
                succ = false;
            }
            Assert.IsTrue(succ);
        }

        private IRoom createMockRoom(string name,string p)
        {
            var roomMock = new Mock<IRoom>();
            IRoom ans;
            IPlayer ipl;
            List<IPlayer> l=new List<IPlayer>(8);
            var pl = new Mock<IPlayer>();
            pl.SetupAllProperties();
            ipl = pl.Object;
            ipl.Name = p;
            roomMock.SetupAllProperties();
            
            ans = roomMock.Object;
            l.Add(ipl);
            //ans.Players=new List<IPlayer>();
            ans.Name = name;
            ans.Players = l;
            roomMock.Setup(r => r.HasPlayer(It.IsAny<string>())).Returns<string>(s => mockHasPlayer(s, ans));
            //ans.Players.Add(ipl);
            //ans = roomMock.Object;
            return ans;
        }
        private bool mockHasPlayer(string name,IRoom i)
        {
            foreach (var p in i.Players)
            {
                if (p.Name == name)
                    return true;
            }
            return false;
        }
        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer()
        {
            var succ = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            IRoom mockr2 = createMockRoom("MyRoom2", "player2");
            //Gametype.NoLimit, 0, 0, 5, 3, 4, true
            //var gp = new GamePreferences();
            _gc.Rooms.Add(mockr1);
            _gc.Rooms.Add(mockr2);

            var p = new List<Predicate<IRoom>>
            { room => room.HasPlayer("player1")};

            var ans = _gc.FindGames(p);
            if (ans.Count == 1 && ans[0].Name == "MyRoom1")
                    succ = true;

            _gc.Rooms.Remove(mockr2);
            _gc.Rooms.Remove(mockr1);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer_Null()
        {
            var succ = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            IRoom mockr2 = createMockRoom("MyRoom2", "player2");
            _gc.Rooms.Add(mockr1);
            _gc.Rooms.Add(mockr2);
            try
            {
                //var gp = new GamePreferences
                var p = new List<Predicate<IRoom>>
                { room => room.HasPlayer(null)};
                _gc.FindGames(p);
            }
            catch
            {
                succ = true;
            }
            _gc.Rooms.Remove(mockr2);
            _gc.Rooms.Remove(mockr1);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPot()
        {
            var succ = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            IRoom mockr2 = createMockRoom("MyRoom2", "player2");
            _gc.Rooms.Add(mockr1);
            _gc.Rooms.Add(mockr2);

            mockr2.Pot = 5;
            var p = new List<Predicate<IRoom>> {room => room.Pot == 5};
            var ans = _gc.FindGames(p);
                if (ans.Count == 1 && ans[0].Name == "MyRoom2")
                    succ = true;

            _gc.Rooms.Remove(mockr2);
            _gc.Rooms.Remove(mockr1);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_NoFilter()
        {
            var succ = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            IRoom mockr2 = createMockRoom("MyRoom2", "player2");
            _gc.Rooms.Add(mockr1);
            _gc.Rooms.Add(mockr2);

            mockr2.Pot = 5;
            var p = new List<Predicate<IRoom>>();
            var ans = _gc.FindGames(p);
            if (ans.Count == 2)
                succ = true;

            _gc.Rooms.Remove(mockr2);
            _gc.Rooms.Remove(mockr1);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_DeleteRoom()
        {
            var succ = false;
            var before = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            _gc.Rooms.Add(mockr1);

            if (_gc.Rooms.Count == 1)
                before = true;
            _gc.DeleteRoom("MyRoom1");
            if (_gc.Rooms.Count == 0 && before)
                succ = true;

            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_GameType()
        {
            var succ = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            IRoom mockr2 = createMockRoom("MyRoom2", "player2");

            //Gametype.NoLimit, 0, 0, 5, 3, 4, true
            var gp = new GamePreferences
                {
                    BuyInPolicy = 0,
                    MinBet = 5,
                    MinPlayers = 3,
                    MaxPlayers = 4
                };
            mockr1.GamePreferences = gp;
            _gc.Rooms.Add(mockr1);
            //Gametype.PotLimit, 0, 0, 5, 2, 10, false
            gp = new GamePreferences
                {
                    GameType = Gametype.PotLimit,
                    BuyInPolicy = 0,
                    MinBet = 5,
                    MinPlayers = 2,
                    MaxPlayers = 10,
                    Spectating = false
                };
            mockr2.GamePreferences = gp;
            _gc.Rooms.Add(mockr2);

            var p = new List<Predicate<IRoom>>
            { room => room.GamePreferences.GameType == Gametype.PotLimit};
            var ans = _gc.FindGames(p);
            if (ans.Count == 1)
               succ = true;

            _gc.Rooms.Remove(mockr2);
            _gc.Rooms.Remove(mockr1);
            Assert.IsTrue(succ);
        }


        [TestMethod]
        public void GameCenter_FindGames_PotAndPlayer()
        {
            var succ = false;
            IRoom mockr1 = createMockRoom("MyRoom1", "player1");
            IRoom mockr2 = createMockRoom("MyRoom2", "player2");
            IRoom mockr3 = createMockRoom("MyRoom3", "player2");
            _gc.Rooms.Add(mockr1);
            _gc.Rooms.Add(mockr2);
            _gc.Rooms.Add(mockr3);

            mockr2.Pot = 5;
            var p = new List<Predicate<IRoom>>
                { room => room.HasPlayer("player2"), room => room.Pot == 5};
            var ans = _gc.FindGames(p);
            if (ans.Count == 1 && ans[0].Name == "MyRoom2")
                succ = true;

            _gc.Rooms.Remove(mockr2);
            _gc.Rooms.Remove(mockr1);
            _gc.Rooms.Remove(mockr3);
            Assert.IsTrue(succ);
        }
    }
}