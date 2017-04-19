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
            Assert.AreEqual(false, succ);
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
            Assert.AreEqual(false, succ);
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
            Assert.AreEqual(true, succ);
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
            Assert.AreEqual(false, succ);
        }

        [TestMethod]
        public void GameCenter_Login_WrongPassword()
        {
            bool succ = true;
            try
            {
                gc.Login("seanocheri", "123sean143");
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
            Assert.IsTrue(succ);
        }
    }
}
