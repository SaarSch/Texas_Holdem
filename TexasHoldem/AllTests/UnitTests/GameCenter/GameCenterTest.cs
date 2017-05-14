using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.GamePrefrences;

namespace AllTests.UnitTests.GameCenter
{
    [TestClass]
    public class GameCenterTest
    {
        private readonly TexasHoldem.GameCenter _gc = TexasHoldem.GameCenter.GetGameCenter();

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_full()
        {
            for(var i = 0; i < 20; i++)
            {
                _gc.Register("aaaaaaa" + i, "12345678");
                _gc.GetUser("aaaaaaa" + i).Wins = i;
            }
            _gc.SetLeagues();
            for(var i = 0; i < 20; i++)
            {
                Assert.IsTrue(_gc.GetUser("aaaaaaa" + i).League == Math.Floor((double)i / 2) + 1);
            }
            _gc.DeleteAllUsers();
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_full1()
        {
            for (var i = 0; i < 60; i++)
            {
                _gc.Register("aaaaaaa" + i, "12345678");
                _gc.GetUser("aaaaaaa" + i).Wins = i;
            }
            _gc.SetLeagues();
            for (var i = 0; i < 60; i++)
            {
                Assert.IsTrue(_gc.GetUser("aaaaaaa" + i).League == Math.Floor((double)i / 6) + 1);
            }
            _gc.DeleteAllUsers();
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_not_Full()
        {
            for (var i = 0; i < 8; i++)
            {
                _gc.Register("aaaaaaa" + i, "12345678");
                _gc.GetUser("aaaaaaa" + i).Wins = i;
            }
            _gc.SetLeagues();
            for (var i = 0; i < 8; i++)
            {
                Assert.IsTrue(_gc.GetUser("aaaaaaa" + i).League == Math.Floor((double)i / 2) + 7);
            }
            _gc.DeleteAllUsers();
        }

        [TestMethod]
        public void GameCenter_SetLeagues_all_leagues_Od()
        {
            for (var i = 0; i < 21; i++)
            {
                _gc.Register("aaaaaaa" + i, "12345678");
                _gc.GetUser("aaaaaaa" + i).Wins = i;
            }
            _gc.SetLeagues();
            for (var i = 1; i < 21; i++)
            {
                Assert.IsTrue(_gc.GetUser("aaaaaaa" + i).League == Math.Ceiling((double)i / 2));
            }
            Assert.IsTrue(_gc.GetUser("aaaaaaa" + 0).League == 1);
            _gc.DeleteAllUsers();
        }


        [TestMethod]
        public void GameCenter_Register_UsernameWithSpaces()
        {
            var succ = true;
            try
            {
                _gc.Register("1234 5", "ssssssss");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Register_PasswordWithOnlyLetters()
        {
            var succ = true;
            try
            {
                _gc.Register("seanocheri", "sssssssss");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Register_OK()
        {
            var succ = true;
            try
            {
                _gc.Register("seanocheri", "123sean123");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_Register_SameOneTwice()
        {
            var succ = true;
            try
            {
                _gc.Register("example123", "123exm123");
            }
            catch
            {
                // ignored
            }
            try
            {
                _gc.Register("example123", "123exm123");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Login_WrongPassword()
        {
            var succ = true;
            try
            {
                _gc.Register("seanocheri", "123sean123");
                _gc.Login("seanocheri", "123sean143");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Login_OK()
        {
            var succ = true;
            try
            {
                _gc.Register("login123", "123exm123");
                _gc.Login("login123", "123exm123");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_Logout_OK()
        {
            var succ = true;
            try
            {
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Logout("login1234");
            }
            catch
            {
                succ = false;
            }
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_OK()
        {
            var succ = false;
            try
            {
                _gc.Register("login1234", "123exm1234");
                var context = _gc.Login("login1234", "123exm1234");
                context.League = 10;
                _gc.SetDefaultRank("login1234", 2);
                _gc.Register("check1234", "321exm321");
                var check = _gc.Login("check1234", "321exm321");
                if (check.League == -1)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_PermissionDenied()
        {
            var succ = false;
            try
            {
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.SetDefaultRank("login1234", 3);
                _gc.Register("check1234", "321exm321");
                var check = _gc.Login("check1234", "321exm321");
                if (check.League == 3)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_IllegalContext()
        {
            var succ = false;
            try
            {
                _gc.SetDefaultRank("login1234", 3);
                _gc.Register("check1234", "321exm321");
                var check = _gc.Login("check1234", "321exm321");
                if (check.League == 2)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_IllegalRank()
        {
            var succ = false;
            try
            {
                _gc.Register("login1234", "123exm1234");
                var context = _gc.Login("login1234", "123exm1234");
                context.League = 10;
                _gc.SetDefaultRank("login1234", -2);
                _gc.Register("check1234", "321exm321");
                var check = _gc.Login("check1234", "321exm321");
                if (check.League == -2)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_OK()
        {
            var succ = false;

            try
            {
                _gc.Register("login1234", "123exm1234");
                var context = _gc.Login("login1234", "123exm1234");
                context.League = 10;
                _gc.SetExpCriteria("login1234", 6);
                if (_gc.ExpCriteria == 6)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_PermissionDenied()
        {
            var succ = false;

            try
            {
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.SetExpCriteria("login1234", 6);
                if (_gc.ExpCriteria == 6)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_IllegalEXP()
        {
            var succ = false;

            try
            {
                _gc.Register("login1234", "123exm1234");
                var context = _gc.Login("login1234", "123exm1234");
                context.League = 10;
                _gc.SetExpCriteria("login1234", 4);
                if (_gc.ExpCriteria == 4)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_IllegalContext()
        {
            var succ = false;

            try
            {
                _gc.SetExpCriteria("login1234", 6);
                if (_gc.ExpCriteria == 6)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_OK()
        {
            var succ = false;
            try
            {
                _gc.Register("login1234", "123exm1234");
                var context = _gc.Login("login1234", "123exm1234");
                context.League = 10;
                _gc.Register("test9876", "123exm9876");
                var userToSet = _gc.Login("test9876", "123exm9876");
                _gc.SetUserRank("login1234", "test9876", 6);
                if (userToSet.League == 6)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_IllegalContext()
        {
            var succ = false;
            try
            {
                _gc.Register("test9876", "123exm9876");
                var userToSet = _gc.Login("test9876", "123exm9876");
                _gc.SetUserRank("login1234", "test9876", 6);
                if (userToSet.League == 6)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_PermissionDenied()
        {
            var succ = false;
            try
            {
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("test9876", "123exm9876");
                var userToSet = _gc.Login("test9876", "123exm9876");
                _gc.SetUserRank("login1234", "test9876", 6);
                if (userToSet.League == 6)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_IllegalRank()
        {
            var succ = false;
            try
            {
                _gc.Register("login1234", "123exm1234");
                var context = _gc.Login("login1234", "123exm1234");
                context.League = 10;
                _gc.Register("test9876", "123exm9876");
                var userToSet = _gc.Login("test9876", "123exm9876");
                _gc.SetUserRank("login1234", "test9876", 11);
                if (userToSet.League == 11)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                gc.CreateRoom("MyRoom2", "login1234", "player2", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                List<Predicate<Room>> p = new List<Predicate<Room>>();
                p.Add(room => room.hasPlayer("player1"));
                ans = gc.FindGames(p);
                if (ans.Count == 1 && ans[0].name == "MyRoom1")
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer_Null()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                gc.CreateRoom("MyRoom2", "login1234", "player2", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                List<Predicate<Room>> p = new List<Predicate<Room>>();
                p.Add(room => room.hasPlayer(null));
                gc.FindGames(p);
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPot()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                r.pot = 5;
                List<Predicate<Room>> p = new List<Predicate<Room>>();
                p.Add(room => room.pot==5);
                ans = gc.FindGames(p);
                if (ans.Count == 1 && ans[0].name == "MyRoom2")
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_NoFilter()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                r.pot = 5;
                List<Predicate<Room>> p = new List<Predicate<Room>>();
                ans = gc.FindGames(p);
                if (ans.Count == 2)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_DeleteRoom()
        {
            var succ = false;
            List<Room> ans = null;
            Room r = null;
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                var before = false;
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.CreateRoom("MyRoom1", "login1234", "player1", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                if (gc.Rooms.Count == 1)
                    before = true;
                gc.DeleteRoom("MyRoom1");
                if (gc.Rooms.Count == 0 && before)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_GameType()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", Gametype.NoLimit, 0, 0, 5, 3, 4, true);
                gc.CreateRoom("MyRoom2", "login1234", "player2", Gametype.PotLimit, 0, 0, 5, 2, 10, false);
                List<Predicate<Room>> p = new List<Predicate<Room>>();
                p.Add(room => room.gamePreferences.gameType == Gametype.PotLimit);
                ans = gc.FindGames(p);
                if (ans.Count == 1)
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }


        [TestMethod]
        public void GameCenter_FindGames_PotAndPlayer()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", Gametype.NoLimit, 0, 10, 5, 3, 4, true);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", Gametype.NoLimit, 0, 10, 5, 3, 4, true);
                r.pot = 5;
                gc.CreateRoom("MyRoom3", "login1234", "player2", Gametype.NoLimit, 0, 10, 5, 3, 4, true);
                List<Predicate<Room>> p = new List<Predicate<Room>>();
                p.Add(room => room.hasPlayer("player2"));
                p.Add(room => room.pot == 5);
                ans = gc.FindGames(p);
                if (ans.Count == 1 && ans[0].name == "MyRoom2")
                    succ = true;
            }
            catch
            {
                // ignored
            }
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }
    }
}