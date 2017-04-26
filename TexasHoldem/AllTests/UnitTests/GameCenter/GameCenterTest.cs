using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem;

namespace AllTests
{
    [TestClass]
    public class GameCenterTest
    {
        GameCenter gc = GameCenter.GetGameCenter();

        [TestMethod]
        public void GameCenter_Register_UsernameWithSpaces()
        {
            bool succ = true;
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
            bool succ = true;
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
            bool succ = true;
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
            bool succ = true;
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
            bool succ = true;
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
            bool succ = true;
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
            bool succ = true;
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
            bool succ = false;
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
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_PermissionDenied()
        {
            bool succ = false;
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
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_IllegalContext()
        {
            bool succ = false;
            User check = null;
            try
            {
                gc.SetDefaultRank("login1234", 3);
                gc.Register("check1234", "321exm321");
                check = gc.Login("check1234", "321exm321");
                if (check.Rank == 2)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetDefaultRank_IllegalRank()
        {
            bool succ = false;
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
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_OK()
        {
            bool succ = false;
            User context = null;

            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.SetExpCriteria("login1234", 6);
                if (gc.EXPCriteria == 6)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_PermissionDenied()
        {
            bool succ = false;
            User context = null;

            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                gc.SetExpCriteria("login1234", 6);
                if (gc.EXPCriteria == 6)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_IllegalEXP()
        {
            bool succ = false;
            User context = null;

            try
            {
                gc.Register("login1234", "123exm1234");
                context = gc.Login("login1234", "123exm1234");
                context.Rank = 10;
                gc.SetExpCriteria("login1234", 4);
                if (gc.EXPCriteria == 4)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetEXP_IllegalContext()
        {
            bool succ = false;

            try
            {
                gc.SetExpCriteria("login1234", 6);
                if (gc.EXPCriteria == 6)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_OK()
        {
            bool succ = false;
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
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_IllegalContext()
        {
            bool succ = false;
            User context = null;
            User user_to_set = null;
            try
            {
                gc.Register("test9876", "123exm9876");
                user_to_set = gc.Login("test9876", "123exm9876");
                gc.SetUserRank("login1234", "test9876", 6);
                if (user_to_set.Rank == 6)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_PermissionDenied()
        {
            bool succ = false;
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
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_SetUserRank_IllegalRank()
        {
            bool succ = false;
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
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllUsers();
            Assert.IsFalse(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPlayer()
        {
            bool succ = false;
            List<Room> ans = null;
            GamePreferences pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
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
                ans = gc.FindGames("login1234", "player1", true, 0, false, pref, false, false);
                if (ans.Count == 1 && ans[0].name == "MyRoom1")
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_SearchPot()
        {
            bool succ = false;
            List<Room> ans = null;
            Room r = null;
            GamePreferences pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
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
                ans = gc.FindGames("login1234", "player1", false, 5, true, pref, false, false);
                if (ans.Count == 1 && ans[0].name == "MyRoom2")
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_NoFilter()
        {
            bool succ = false;
            List<Room> ans = null;
            Room r = null;
            GamePreferences pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
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
                ans = gc.FindGames("login1234", "player1", false, 5, false, pref, false, false);
                if (ans.Count == 2)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_DeleteRoom()
        {
            bool succ = false;
            List<Room> ans = null;
            Room r = null;
            GamePreferences pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            try
            {
                bool before = false;
                gc.Register("login1234", "123exm1234");
                gc.Login("login1234", "123exm1234");
                gc.CreateRoom("MyRoom1", "login1234", "player1", pref);
                if (gc.GetAllRooms().Count == 1)
                {
                    before = true;
                }
                gc.DeleteRoom("MyRoom1");
                if (gc.GetAllRooms().Count == 0 && before)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_Preferences()
        {
            bool succ = false;
            List<Room> ans = null;
            Room r = null;
            GamePreferences pref1 = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
            GamePreferences pref2 = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 2, 10, false);
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
                ans = gc.FindGames("login1234", "player1", false, 5, false, pref1, true, false);
                if (ans.Count == 1)
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_InLeague()
        {
            bool succ = false;
            User user = null;
            List<Room> ans = null;
            Room r = null;
            GamePreferences pref = new GamePreferences(Gametype.NoLimit, 0, 0, 5, 3, 4, true);
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
                ans = gc.FindGames("login1234", "player1", false, 5, false, pref, false, true);
                if (ans.Count == 1 && ans[0].name=="MyRoom2")
                if (ans.Count == 1 && ans[0].name=="MyRoom2")
                {
                    succ = true;
                }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }

        [TestMethod]
        public void GameCenter_FindGames_PotAndPlayer()
        {
            bool succ = false;
            List<Room> ans = null;
            Room r = null;
            GamePreferences pref = new GamePreferences(Gametype.NoLimit, 0, 10, 5, 3, 4, true);
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
                ans = gc.FindGames("login1234", "player2", true, 5, true, pref, false, false);
                if (ans.Count == 1 && ans[0].name == "MyRoom2")
                    if (ans.Count == 1 && ans[0].name == "MyRoom2")
                    {
                        succ = true;
                    }
            }
            catch
            { }
            gc.DeleteAllRooms();
            gc.DeleteAllUsers();
            Assert.IsTrue(succ);
        }
    }
}
