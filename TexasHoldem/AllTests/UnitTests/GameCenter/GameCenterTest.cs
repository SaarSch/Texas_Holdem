using System;
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
                gc.SetEXPCriteria("login1234", 6);
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
                gc.SetEXPCriteria("login1234", 6);
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
                gc.SetEXPCriteria("login1234", 4);
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
                gc.SetEXPCriteria("login1234", 6);
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
    }
}
