using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.GamePrefrences;

namespace AllTests.UnitTests.GameCenter
{
    [TestClass]
    public class GameCenterTest
    {
        private readonly TexasHoldem.GameCenter _gc = TexasHoldem.GameCenter.GetGameCenter();

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
                context.Rank = 10;
                _gc.SetDefaultRank("login1234", 2);
                _gc.Register("check1234", "321exm321");
                var check = _gc.Login("check1234", "321exm321");
                if (check.Rank == -1)
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
                if (check.Rank == 3)
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
                if (check.Rank == 2)
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
                context.Rank = 10;
                _gc.SetDefaultRank("login1234", -2);
                _gc.Register("check1234", "321exm321");
                var check = _gc.Login("check1234", "321exm321");
                if (check.Rank == -2)
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
                context.Rank = 10;
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
                context.Rank = 10;
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
                context.Rank = 10;
                _gc.Register("test9876", "123exm9876");
                var userToSet = _gc.Login("test9876", "123exm9876");
                _gc.SetUserRank("login1234", "test9876", 6);
                if (userToSet.Rank == 6)
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
                if (userToSet.Rank == 6)
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
                if (userToSet.Rank == 6)
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
                context.Rank = 10;
                _gc.Register("test9876", "123exm9876");
                var userToSet = _gc.Login("test9876", "123exm9876");
                _gc.SetUserRank("login1234", "test9876", 11);
                if (userToSet.Rank == 11)
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
                var gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                var ans = _gc.FindGames("login1234", "player1", true, 0, false, "NoLimit", 0, 0, 5, 3, 4, true, false,
                    false);
                if (ans.Count == 1 && ans[0] == "MyRoom1")
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
                var gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                var ans = _gc.FindGames("login1234", "player1", false, 5, true, "NoLimit", 0, 0, 5, 3, 4, true, false,
                    false);
                if (ans.Count == 1 && ans[0] == "MyRoom2")
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
                GamePreferences gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                var ans = _gc.FindGames("login1234", "player1", false, 5, false, "NoLimit", 0, 0, 5, 3, 4, true,
                    false, false);
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
                GamePreferences gp = new GamePreferences();
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
        public void GameCenter_FindGames_Preferences()
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

                //Gametype.NoLimit, 0, 0, 5, 2, 10, false
                //Gametype.NoLimit, 1, 0, 4, 2, 8, true
                gp = new GamePreferences();
                gp = new ModifiedBuyInPolicy(0, gp);
                gp = new ModifiedMinBet(5, gp);
                gp = new ModifiedMaxPlayers(10, gp);
                gp = new ModifiedSpectating(false, gp);

                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                var ans = _gc.FindGames("login1234", "player1", false, 5, false, "NoLimit", 0, 0, 5, 3, 4, true, true, false);
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
        public void GameCenter_FindGames_InLeague()
        {
            var succ = false;
            _gc.DeleteAllRooms();
            _gc.DeleteAllUsers();
            try
            {
                _gc.Register("login1234", "123exm1234");
                _gc.Login("login1234", "123exm1234");
                _gc.Register("seanoch123", "seanoch123");
                var user = _gc.Login("seanoch123", "seanoch123");
                user.Rank = 5;
                //Gametype.NoLimit, 0, 0, 5, 3, 4, true
                var gp = new GamePreferences();
                _gc.CreateRoom("MyRoom1", "seanoch123", "player1", gp);
                var r = _gc.CreateRoom("MyRoom2", "login1234", "player2", gp);
                r.Pot = 5;
                var ans = _gc.FindGames("login1234", "player1", false, 5, false, "NoLimit", 0, 0, 5, 3, 4, true, false, true);
                if (ans.Count == 1 && ans[0] == "MyRoom2")
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
                var ans = _gc.FindGames("login1234", "player2", true, 5, true, "NoLimit", 0, 10, 5, 3, 4, true, false, false);
                if (ans.Count == 1 && ans[0] == "MyRoom2")
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