using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Bridges;

namespace AllTests.AcceptanceTests
{
    [TestClass]
    public class TestsAss1
    {
        private IBridge bridge;
        private string legalUserName = "eladkamin"; //legal userName
        private string legalPass = "123456789"; //legal password

        [TestInitialize]
        public void Initialize()
        {
            bridge = new ProxyBridge();
        }
        [TestMethod]
        public void TestRegisterToTheSystem_Good() //legal userName and legal Pass
        {
            Assert.IsTrue(bridge.register(legalUserName, legalPass));
            Assert.IsTrue(bridge.isUserExist(legalUserName));
            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_ExistingUserName() //illegal userName or illegal Pass or userName exists
        {
            Assert.IsTrue(bridge.register(legalUserName, legalPass));
            Assert.IsFalse(bridge.register(legalUserName, legalPass));
            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalUserName() //illegal userName or illegal Pass or userName exists
        {
            Assert.IsFalse(bridge.register("K", legalPass));
            Assert.IsFalse(bridge.isUserExist("K"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalPass() //illegal userName or illegal Pass or userName exists
        {
            Assert.IsFalse(bridge.register(legalUserName, "1"));
            Assert.IsFalse(bridge.isUserExist(legalUserName));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersUserName() //illegal characters in the userName or in the Pass
        {
            Assert.IsFalse(bridge.register("@123 !$%+_-", legalPass));
            Assert.IsFalse(bridge.isUserExist("@123 !$%+_-"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersPass() //illegal characters in the userName or in the Pass
        {
            Assert.IsFalse(bridge.register(legalUserName, "           "));
            Assert.IsFalse(bridge.isUserExist(legalUserName));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Good() //existing user information and the user is logged in 
        {
            bridge.register(legalUserName, legalPass);

            Assert.IsTrue(bridge.login(legalUserName, legalPass));
            Assert.IsTrue(bridge.isLoggedIn(legalUserName));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_UserNotExist() //user does not exist or password is wrong
        {
            Assert.IsFalse(bridge.login(legalUserName, legalPass));
            Assert.IsFalse(bridge.isLoggedIn(legalUserName));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_WrongPass() //user does not exist or password is wrong
        {
            bridge.register(legalUserName, legalPass);

            Assert.IsFalse(bridge.login(legalUserName, "notThePass1"));
            Assert.IsFalse(bridge.isLoggedIn(legalUserName));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_UserNameEmpty() //Username field is empty or SQL Injection in password field
        {
            Assert.IsFalse(bridge.login("", legalPass));
            Assert.IsFalse(bridge.isLoggedIn(""));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_SQLInjection() //Username field is empty or SQL Injection in password field
        {
            bridge.register(legalUserName, legalPass);

            Assert.IsFalse(bridge.login(legalUserName, "OR 'a'='a'"));
            Assert.IsFalse(bridge.isLoggedIn(legalUserName));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestLogOutToTheSystem_Good() //logout
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.logOut(legalUserName));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditUserName_Good() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.editUserName("thisIsEdited"));
            Assert.IsTrue(bridge.isUserExist("thisIsEdited"));

            bridge.deleteUser("thisIsEdited");
        }

        [TestMethod]
        public void TestEditUserName_Sad_UserNameExist() 
        {
            bridge.register("existingName", legalPass);
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editUserName("existingName"));
            Assert.IsFalse(bridge.isLoggedIn("existingName"));

            bridge.deleteUser(legalUserName);
            bridge.deleteUser("existingName");
        }

        [TestMethod]
        public void TestEditUserName_Sad_IllegalUserName()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editUserName("k"));
            Assert.IsFalse(bridge.isLoggedIn("k"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditUserName_Bad_IllegalCharacters() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editUserName("@123 !$%+_-"));
            Assert.IsFalse(bridge.isLoggedIn("@123 !$%+_-"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditPassword_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.editPassword("9876543210"));

            bridge.logOut(legalUserName);

            Assert.IsTrue(bridge.login(legalUserName, "9876543210"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditPassword_Sad_IllegalPass() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editPassword("0"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditPassword_Bad_IllegalCharacters()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editPassword("              "));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditAvatar_Good() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.editAvatar("newavatar.jpg"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditAvatar_Sad_IllegalPic() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editAvatar("newavatar.mp3"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestEditAvatar_Bad_InfectedPic() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editAvatar("newVIRUSavatar.jpg"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.createNewGame("Good Game Name",6));
            Assert.IsTrue(bridge.isGameExist("Good Game Name"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalGameName()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.createNewGame("Illegal Game Name                  35", 6));
            Assert.IsFalse(bridge.isGameExist("Illegal Game Name                  35"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalNumberOfPlayers()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.createNewGame("Good Game Name", 10000000));
            Assert.IsFalse(bridge.isGameExist("Good Game Name"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Bad_IllegalCharactersInGameName()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            
            Assert.IsFalse(bridge.createNewGame("Illegal@#Game!@Name?)", 6));
            Assert.IsFalse(bridge.isGameExist("Illegal@#Game!@Name?)"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestJoinExistingGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 6);
            bridge.logOut(legalUserName);

            bridge.login("GoodName", legalPass);

            ArrayList activeGames = bridge.getActiveGames(bridge.getRank("GoodName"));

            Assert.IsTrue(bridge.joinGame(activeGames[0]));

            bridge.deleteUser(legalUserName);
            bridge.deleteUser("GoodName");
        }

        [TestMethod]
        public void TestJoinExistingGame_Sad_IllegalGame()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 6);
            bridge.logOut(legalUserName);

            bridge.login("GoodName", legalPass);

            Assert.IsFalse(bridge.joinGame("gg12"));

            bridge.deleteUser(legalUserName);
            bridge.deleteUser("GoodName");
        }

        [TestMethod]
        public void TestJoinExistingGame_bad_MultiplejoiningAttemptsToFullRoom()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);
            bridge.register("AnotherGoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 2);
            bridge.logOut(legalUserName);

            bridge.login("AnotherGoodName", legalPass);
            bridge.joinGame("Good Game Name");
            bridge.logOut("AnotherGoodName");

            bridge.login("GoodName", legalPass);

            Assert.IsFalse(bridge.joinGame("Good Game Name"));
            Assert.IsFalse(bridge.joinGame("Good Game Name"));
            Assert.IsFalse(bridge.joinGame("Good Game Name"));
            Assert.IsFalse(bridge.joinGame("Good Game Name"));
            Assert.IsFalse(bridge.joinGame("Good Game Name"));

            bridge.deleteUser(legalUserName);
            bridge.deleteUser("GoodName");
            bridge.deleteUser("AnotherGoodName");
        }

        [TestMethod]
        public void TestJoinExistingGame_Bad_IllegalGame()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.joinGame(""));

            bridge.deleteUser(legalUserName);
        }




        [TestMethod]
        public void TestSpectateExistingGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 6);
            bridge.logOut(legalUserName);

            bridge.login("GoodName", legalPass);

            ArrayList activeGames = bridge.getActiveGames();

            Assert.IsTrue(bridge.SpectateGame(activeGames[0]));

            bridge.deleteUser(legalUserName);
            bridge.deleteUser("GoodName");
        }

        [TestMethod]
        public void TestSpectateExistingGame_Sad_IllegalGame()
        {
            bridge.register(legalUserName, legalPass);
   
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.SpectateGame("gg12"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestJoinExistingGame_Bad_IllegalChecters()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.SpectateGame("Illegal)@#$%Game!@#$Name"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestLeaveGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 6);

            Assert.IsTrue(bridge.leaveGame("Good Game Name"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestReplayGame_Good()//TODO:::how to create a replay??
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            ArrayList replayGames=bridge.getAllGamesReplay();

            Assert.IsTrue(bridge.leaveGame("Good Game Name"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestSaveTurn_Good()//TODO:::how to create a game to save a turn??
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.leaveGame("Good Game Name"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestFindGames_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 6);
            bridge.createNewGame("Game Not In Rank", 6);
            bridge.setRank("Game Not In Rank", 10);

            ArrayList activeGames = bridge.getActiveGames(bridge.getRank(legalUserName));

            Assert.IsTrue(activeGames.Contains("Good Game Name"));
            Assert.IsFalse(activeGames.Contains("Game Not In Rank"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestFindGames_Sad_NoGamesFound()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            ArrayList activeGames = bridge.getActiveGames(bridge.getRank(legalUserName));

            Assert.IsTrue(activeGames.Count==0);

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestListActiveGames_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", 6);
            bridge.createNewGame("Game Not In Rank", 6);
            bridge.setRank("Game Not In Rank", 10);

            ArrayList activeGames = bridge.getActiveGames();

            Assert.IsTrue(activeGames.Contains("Good Game Name"));
            Assert.IsTrue(activeGames.Contains("Game Not In Rank"));

            bridge.deleteUser(legalUserName);
        }

        [TestMethod]
        public void TestListActiveGames_Sad_NoGamesFound()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            ArrayList activeGames = bridge.getActiveGames();

            Assert.IsTrue(activeGames.Count == 0);

            bridge.deleteUser(legalUserName);
        }



    }

}
