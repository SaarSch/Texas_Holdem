using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TexasHoldem.Game;
using TexasHoldem.Logics;
using TexasHoldem.Users;

namespace AllTests.UnitTests.GameCenter
{
    [TestClass]
    public class GameCenterTest
    {
        private static readonly TexasHoldem.GameCenter _gc = TexasHoldem.GameCenter.GetGameCenter("TestDatabase");
	    private static readonly UserLogic _ul = _gc.UserLogic;
		private List<Tuple<IUser, bool>> _u;
        [TestInitialize]
        public void Initialize()
        {
            _u=new List<Tuple<IUser, bool>>();
	        registerMoqUsers(60);
			_ul.UpdateDB();
		}

	    [TestCleanup] // happens after each test
	    public void Cleanup()
	    {
		    _ul.DeleteAllUsers(_u);
	    }

		[TestMethod]
        public void GameCenter_SetLeagues_all_leagues_full()
        {
            var i = 0;
            _ul.SetLeagues(_u);
            foreach (var u in _u)
            {
                Assert.AreEqual(u.Item1.League, Math.Floor((double)i / 6) + 1);
                i++;
            }
        }

	    private void registerMoqUsers(int num)
	    {
			for (int i = 0; i < num; i++)
			{
				_ul.Register("aaaaaaa" + i, "12345678", _u);
				_u[i].Item1.Wins = i;
				_u[i].Item1.ChipsAmount = 50000;
				_u[i].Item1.League = 0;
			}
		}

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_full1()
        {
            var count = 0;
            _ul.SetLeagues(_u);
            foreach (var u in _u)
            {
                Assert.AreEqual(u.Item1.League, Math.Floor((double)count / 6) + 1);
                count++;
            }
        }
        [TestMethod]
        public void GameCenter_GetStat()
        {
            var ans = _ul.GetStat("aaaaaaa0");
            Assert.AreEqual(ans.Username, "aaaaaaa0");
            Assert.AreEqual(ans.Password, "12345678");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GameCenter_GetStat_fail()
        {
            var ans = _ul.GetStat("123");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GameCenter_GetTopStat_fail()
        {
            _gc.Users.AddRange(_u);
            var ans = _gc.GetTopStat(8);
        }
        [TestMethod]
        public void GameCenter_GetTopStat1()
        {
            var i = 0;
            _gc.Users.AddRange(_u);
            foreach (var u in _u)
            {
                u.Item1.GrossProfit = i++;
            }
            var ans = _gc.GetTopStat(1);
            for (var j = 0; j < ans.Count; j++)
            {
                Assert.AreEqual(ans[j].GrossProfit, --i);
            }
        }
        [TestMethod]
        public void GameCenter_GetTopStat2()
        {
            int i = 0;
            _gc.Users.AddRange(_u);
            foreach (var u in _u)
            {
                u.Item1.HighestCashGain = i++;
            }
            List<IUser> ans = _gc.GetTopStat(2);
            for (int j = 0; j < ans.Count; j++)
            {
                Assert.AreEqual(ans[j].HighestCashGain, --i);
            }
        }
        [TestMethod]
        public void GameCenter_GetTopStat3()
        {
            int i = 0;
            _gc.Users.AddRange(_u);
            foreach (var u in _u)
            {
                u.Item1.NumOfGames = i++;
            }
            List<IUser> ans = _gc.GetTopStat(3);
            for (int j = 0; j < ans.Count; j++)
            {
                Assert.AreEqual(ans[j].NumOfGames,--i);
            }
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_not_Full()
        {
            var count = 0;
			_ul.DeleteAllUsers(_u);
			registerMoqUsers(8);
            _ul.SetLeagues(_u);
            foreach (var u in _u)
            {
                Assert.AreEqual(Math.Floor((double)count / 2) + 7, u.Item1.League);
                count++;
            }
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_Odd()
        {
			_ul.Register("aaaaaaa60", "12345678", _u);
	        IUser user = _u.First(u => u.Item1.Username == "aaaaaaa60").Item1;
	        user.Wins = 60;
	        user.ChipsAmount = 50000;
	        user.League = 0;
			_ul.SetLeagues(_u);
            for (var i = 1; i < 61; i++)
            {
                Assert.AreEqual(_u[i].Item1.League, Math.Ceiling((double)i / 6));
            }
            Assert.AreEqual(_u[0].Item1.League, 1);
        }


        [TestMethod]
        public void GameCenter_Register_UsernameWithSpaces()
        {
            
            var succ = true;
            
            try
            {
                _ul.Register("1234 5", "ssssssss", _u);
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
                _ul.Register("seanocheri", "sssssssss", _u);
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
                _ul.Register("seanocheri", "123sean123", _u);
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
            _ul.Register("example123", "123exm123", _u);
            try
            {
                _ul.Register("example123", "123exm123", _u);
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
	        _ul.Login("aaaaaaa0", "12345678", _u);

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