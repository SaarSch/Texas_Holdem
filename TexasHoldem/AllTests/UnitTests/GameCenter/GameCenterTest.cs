using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Game;
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
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                _gc.Login("seanoch123", "seanoch123");
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                IPreferences gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                var p = new List<Predicate<Room>> {room => room.HasPlayer("player1")};
                var ans = _gc.FindGames(p);
                if (ans.Count == 1 && ans[0].Name == "MyRoom1")
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
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                _gc.Login("seanoch123", "seanoch123");
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                IPreferences gp = new GamePreferences();
                gp = new ModifiedBuyInPolicy(0, gp);
                gp = new ModifiedMinBet(5, gp);
                gp = new ModifiedMinPlayers(3, gp);
                gp = new ModifiedMaxPlayers(4, gp);
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
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                _gc.Login("seanoch123", "seanoch123");
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                IPreferences gp = new GamePreferences();
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
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                _gc.Login("seanoch123", "seanoch123");
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                IPreferences gp = new GamePreferences();
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
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_DeleteRoom()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                var before = false;
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                IPreferences gp = new GamePreferences();
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
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                _gc.Login("seanoch123", "seanoch123");
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                //1 0 4 2 8
                IPreferences gp = new GamePreferences();
                gp = new ModifiedBuyInPolicy(0,gp);
                gp = new ModifiedMinBet(5,gp);
                gp = new ModifiedMinPlayers(3,gp);
                gp = new ModifiedMaxPlayers(4,gp);
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                //Gametype.PotLimit, 0, 0, 5, 2, 10, false
                gp = new GamePreferences();
                gp = new ModifiedGameType(Gametype.PotLimit, gp);
                gp = new ModifiedBuyInPolicy(0, gp);
                gp = new ModifiedMinBet(5, gp);
                gp = new ModifiedMinPlayers(2,gp);
                gp = new ModifiedMaxPlayers(10,gp);
                gp = new ModifiedSpectating(false, gp);
                _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                var p = new List<Predicate<Room>> {room => room.GamePreferences.GetGameType() == Gametype.PotLimit};
                var ans = _gc.FindGames(p);
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
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                _gc.Login("seanoch123", "seanoch123");
                //Gametype.NoLimit, 0, 10, 5, 3, 4, true
                //1 0 4 2 8
                IPreferences gp = new GamePreferences();
                gp = new ModifiedBuyInPolicy(0, gp);
                gp = new ModifiedChipPolicy(10, gp);
                gp = new ModifiedMinBet(5, gp);
                gp = new ModifiedMinPlayers(3, gp);
                gp = new ModifiedMaxPlayers(4, gp);
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
            _gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }
    }
}