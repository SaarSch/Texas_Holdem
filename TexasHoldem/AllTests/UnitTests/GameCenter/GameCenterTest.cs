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

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer()
        {
            var succ = false;
            setMoqUsers(2, true);
            _gc.Users.Add(_u[0]);
            _gc.Users.Add(_u[1]);

            //Gametype.NoLimit, 0, 0, 5, 3, 4, true
            var gp = new GamePreferences();
            _gc.CreateRoom("MyRoom1", "aaaaaaa0", "player1", gp);
            _gc.CreateRoom("MyRoom2", "aaaaaaa1", "player2", gp);
            var p = new List<Predicate<Room>> {room => room.HasPlayer("player1")};
            var ans = _gc.FindGames(p);
            if (ans.Count == 1 && ans[0].Name == "MyRoom1")
                    succ = true;

            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer_Null()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            try
            {
                _ul.Register("login1234", "123exm1234", _gc.Users);
                _ul.Login("login1234", "123exm1234", _gc.Users);
                _ul.Register("seanoch123", "seanoch123", _gc.Users);
                _ul.Login("seanoch123", "seanoch123", _gc.Users);
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                var gp = new GamePreferences
                {
                    BuyInPolicy = 0,
                    MinBet = 5,
                    MinPlayers = 3,
                    MaxPlayers = 4
                };
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                var p = new List<Predicate<Room>> {room => room.HasPlayer(null)};
                _gc.FindGames(p);
            }
            catch
            {
                succ = true;
            }
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPot()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            try
            {
                _ul.Register("login1234", "123exm1234", _gc.Users);
                _ul.Login("login1234", "123exm1234", _gc.Users);
                _ul.Register("seanoch123", "seanoch123", _gc.Users);
                _ul.Login("seanoch123", "seanoch123", _gc.Users);
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                var gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                var p = new List<Predicate<Room>> {room => room.Pot == 5};
                var ans = _gc.FindGames(p);
                if (ans.Count == 1 && ans[0].Name == "MyRoom2")
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_NoFilter()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            try
            {
                _ul.Register("login1234", "123exm1234", _gc.Users);
                _ul.Login("login1234", "123exm1234", _gc.Users);
                _ul.Register("seanoch123", "seanoch123", _gc.Users);
                _ul.Login("seanoch123", "seanoch123", _gc.Users);
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                var gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                var p = new List<Predicate<Room>>();
                var ans = _gc.FindGames(p);
                if (ans.Count == 2)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_DeleteRoom()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            try
            {
                var before = false;
                _ul.Register("login1234", "123exm1234", _gc.Users);
                _ul.Login("login1234", "123exm1234", _gc.Users);
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                var gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "login1234", "player1", gp);
                if (_gc.Rooms.Count == 1)
                    before = true;
                _gc.DeleteRoom("MyRoom1");
                if (_gc.Rooms.Count == 0 && before)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_GameType()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            try
            {
                _ul.Register("login1234", "123exm1234", _gc.Users);
                _ul.Login("login1234", "123exm1234", _gc.Users);
                _ul.Register("seanoch123", "seanoch123", _gc.Users);
                _ul.Login("seanoch123", "seanoch123", _gc.Users);
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                //1 0 4 2 8
                var gp = new GamePreferences
                {
                    BuyInPolicy = 0,
                    MinBet = 5,
                    MinPlayers = 3,
                    MaxPlayers = 4
                };
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
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
                _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                var p = new List<Predicate<Room>> {room => room.GamePreferences.GameType == Gametype.PotLimit};
                var ans = _gc.FindGames(p);
                if (ans.Count == 1)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }


        [TestMethod]
        public void GameCenter_FindGames_PotAndPlayer()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            try
            {
                _ul.Register("login1234", "123exm1234", _gc.Users);
                _ul.Login("login1234", "123exm1234", _gc.Users);
                _ul.Register("seanoch123", "seanoch123", _gc.Users);
                _ul.Login("seanoch123", "seanoch123", _gc.Users);
                //Gametype.NoLimit, 0, 10, 5, 3, 4, true
                //1 0 4 2 8
                var gp = new GamePreferences
                {
                    BuyInPolicy = 0,
                    ChipPolicy = 10,
                    MinBet = 5,
                    MinPlayers = 3,
                    MaxPlayers = 4
                };
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                _gc.CreateRoom("MyRoom3", "login1234", "player2", gp);
                var p = new List<Predicate<Room>> {room => room.HasPlayer("player2"), room => room.Pot == 5};
                var ans = _gc.FindGames(p);
                if (ans.Count == 1 && ans[0].Name == "MyRoom2")
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _ul.DeleteAllUsers(_gc.Users);
            Assert.IsTrue(succ);
        }
    }
}