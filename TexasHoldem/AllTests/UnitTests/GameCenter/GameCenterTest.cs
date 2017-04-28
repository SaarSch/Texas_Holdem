using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllTests.UnitTests.GameCenter
{
    [TestClass]
    public class GameCenterTest
    {
        private readonly TexasHoldem.GameCenter gc = TexasHoldem.GameCenter.GetGameCenter();

        [TestMethod]
        public void GameCenter_Register_UsernameWithSpaces()
        {
            var succ = true;
            try
            {
                gc.Register("1234 5", "ssssssss");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Register_PasswordWithOnlyLetters()
        {
            var succ = true;
            try
            {
                gc.Register("seanocheri", "sssssssss");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Register_OK()
        {
            var succ = true;
            try
            {
                gc.Register("seanocheri", "123sean123");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_Register_SameOneTwice()
        {
            var succ = true;
            try
            {
                gc.Register("example123", "123exm123");
            }
            catch
            {
            }
            try
            {
                gc.Register("example123", "123exm123");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Login_WrongPassword()
        {
            var succ = true;
            try
            {
                gc.Register("seanocheri", "123sean123");
                gc.Login("seanocheri", "123sean143");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_Login_OK()
        {
            var succ = true;
            try
            {
                gc.Register("login123", "123exm123");
                gc.Login("login123", "123exm123");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_Logout_OK()
        {
            var succ = true;
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Logout("login1234");
            }
            catch
            {
                succ = false;
            }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_OK()
        {
            var succ = false;
            User context = null;
            User check = null;
            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.SetDefaultRank("login1234", 2);
                gc.Register("check1234", "321exm321");
                check = gc.Login("check1234", "321exm321");
                if (check.Rank == 2)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_PermissionDenied()
        {
            var succ = false;
            User context = null;
            User check = null;
            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                gc.SetDefaultRank("login1234", 3);
                gc.Register("check1234", "321exm321");
                check = gc.Login("check1234", "321exm321");
                if (check.Rank == 3)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_IllegalContext()
        {
            var succ = false;
            User check = null;
            try
            {
                gc.SetDefaultRank("login1234", 3);
                gc.Register("check1234", "321exm321");
                check = gc.Login("check1234", "321exm321");
                if (check.Rank == 2)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_IllegalRank()
        {
            var succ = false;
            User context = null;
            User check = null;
            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.SetDefaultRank("login1234", -2);
                gc.Register("check1234", "321exm321");
                check = gc.Login("check1234", "321exm321");
                if (check.Rank == -2)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_OK()
        {
            var succ = false;
            User context = null;

            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.SetExpCriteria("login1234", 6);
                if (gc.EXPCriteria == 6)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_PermissionDenied()
        {
            var succ = false;
            User context = null;

            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                gc.SetExpCriteria("login1234", 6);
                if (gc.EXPCriteria == 6)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_IllegalEXP()
        {
            var succ = false;
            User context = null;

            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.SetExpCriteria("login1234", 4);
                if (gc.EXPCriteria == 4)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_IllegalContext()
        {
            var succ = false;

            try
            {
                gc.SetExpCriteria("login1234", 6);
                if (gc.EXPCriteria == 6)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_OK()
        {
            var succ = false;
            User context = null;
            User user_to_set = null;
            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.Register("test9876", "123exm9876");
                user_to_set = gc.Login("test9876", "123exm9876");
                gc.SetUserRank("login1234", "test9876", 6);
                if (user_to_set.Rank == 6)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_IllegalContext()
        {
            var succ = false;
            User context = null;
            User user_to_set = null;
            try
            {
                gc.Register("test9876", "123exm9876");
                user_to_set = gc.Login("test9876", "123exm9876");
                gc.SetUserRank("login1234", "test9876", 6);
                if (user_to_set.Rank == 6)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_PermissionDenied()
        {
            var succ = false;
            User context = null;
            User user_to_set = null;
            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                gc.Register("test9876", "123exm9876");
                user_to_set = gc.Login("test9876", "123exm9876");
                gc.SetUserRank("login1234", "test9876", 6);
                if (user_to_set.Rank == 6)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_IllegalRank()
        {
            var succ = false;
            User context = null;
            User user_to_set = null;
            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.Register("test9876", "123exm9876");
                user_to_set = gc.Login("test9876", "123exm9876");
                gc.SetUserRank("login1234", "test9876", 11);
                if (user_to_set.Rank == 11)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer()
        {
            var succ = false;
            List<string> ans = null;
            var pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", pref);
                gc.CreateRoom("MyRoom2", "login1234", "player2", pref);
                ans = gc.FindGames("login1234", "player1", true, 0, false, Gametype.NoLimit, 0, 0, 5, 3, 4, true, false, false);
                if (ans.Count == 1 && ans[0] == "MyRoom1")
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPot()
        {
            var succ = false;
            List<string> ans = null;
            Room r = null;
            var pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", pref);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", pref);
                r.pot = 5;
                ans = gc.FindGames("login1234", "player1", false, 5, true, Gametype.NoLimit, 0, 0, 5, 3, 4, true, false, false);
                if (ans.Count == 1 && ans[0] == "MyRoom2")
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_NoFilter()
        {
            var succ = false;
            List<string> ans = null;
            Room r = null;
            var pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", pref);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", pref);
                r.pot = 5;
                ans = gc.FindGames("login1234", "player1", false, 5, false, Gametype.NoLimit, 0, 0, 5, 3, 4, true, false, false);
                if (ans.Count == 2)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_DeleteRoom()
        {
            var succ = false;
            List<Room> ans = null;
            Room r = null;
            var pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                var before = false;
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.CreateRoom("MyRoom1", "login1234", "player1", pref);
                if (gc.Rooms.Count == 1)
                    before = true;
                gc.DeleteRoom("MyRoom1");
                if (gc.Rooms.Count == 0 && before)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_Preferences()
        {
            var succ = false;
            List<string> ans = null;
            Room r = null;
            var pref1 = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            var pref2 = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 2, 10, false);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", pref1);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", pref2);
                r.pot = 5;
                ans = gc.FindGames("login1234", "player1", false, 5, false, Gametype.NoLimit, 0, 0, 5, 3, 4, true, true, false);
                if (ans.Count == 1)
                    succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_InLeague()
        {
            var succ = false;
            User user = null;
            List<string> ans = null;
            Room r = null;
            var pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                user = gc.Login("seanoch123", "seanoch123");
                user.Rank = 5;
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", pref);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", pref);
                r.pot = 5;
                ans = gc.FindGames("login1234", "player1", false, 5, false, Gametype.NoLimit, 0, 0, 5, 3, 4, true, false, true);
                if (ans.Count == 1 && ans[0] == "MyRoom2")
                    if (ans.Count == 1 && ans[0] == "MyRoom2")
                        succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_PotAndPlayer()
        {
            var succ = false;
            List<string> ans = null;
            Room r = null;
            var pref = new GamePreferences(Gametype.NoLimit, 0, 10, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.Register("seanoch123", "seanoch123");
                gc.Login("seanoch123", "seanoch123");
                gc.CreateRoom("MyRoom1", "seanoch123", "player1", pref);
                r = gc.CreateRoom("MyRoom2", "login1234", "player2", pref);
                r.pot = 5;
                gc.CreateRoom("MyRoom3", "login1234", "player2", pref);
                ans = gc.FindGames("login1234", "player2", true, 5, true, Gametype.NoLimit, 0, 10, 5, 3, 4, true, false, false);
                if (ans.Count == 1 && ans[0] == "MyRoom2")
                    if (ans.Count == 1 && ans[0] == "MyRoom2")
                        succ = true;
            }
            catch
            {
            }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }
    }
}