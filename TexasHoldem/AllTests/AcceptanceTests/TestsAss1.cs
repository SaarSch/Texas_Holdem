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
        private string legalPlayer = "eladkaminGameName"; //legal player name
       


        [TestInitialize]
        public void Initialize()
        {
            bridge = new ProxyBridge();
        }
        [TestMethod]
        public void TestRegisterToTheSystem_Good() 
        {
            Assert.IsTrue(bridge.register(legalUserName, legalPass));
            Assert.IsTrue(bridge.isUserExist(legalUserName));
            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_ExistingUserName()
        {
            Assert.IsTrue(bridge.register(legalUserName, legalPass));
            Assert.IsFalse(bridge.register(legalUserName, legalPass));
            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalUserName() 
        {
            Assert.IsFalse(bridge.register("K", legalPass));
            Assert.IsFalse(bridge.isUserExist("K"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalPass() 
        {
            Assert.IsFalse(bridge.register(legalUserName, "1"));
            Assert.IsFalse(bridge.isUserExist(legalUserName));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersUserName()
        {
            Assert.IsFalse(bridge.register("@123 !$%+_-", legalPass));
            Assert.IsFalse(bridge.isUserExist("@123 !$%+_-"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersPass() 
        {
            Assert.IsFalse(bridge.register(legalUserName, "           "));
            Assert.IsFalse(bridge.isUserExist(legalUserName));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Good() 
        {
            bridge.register(legalUserName, legalPass);

            Assert.IsTrue(bridge.login(legalUserName, legalPass));
            Assert.IsTrue(bridge.isLoggedIn(legalUserName, legalPass));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_UserNotExist()
        {
            Assert.IsFalse(bridge.login(legalUserName, legalPass));
            Assert.IsFalse(bridge.isLoggedIn(legalUserName, legalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_WrongPass() 
        {
            bridge.register(legalUserName, legalPass);

            Assert.IsFalse(bridge.login(legalUserName, "notThePass1"));
            Assert.IsFalse(bridge.isLoggedIn(legalUserName, legalPass));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_UserNameEmpty()
        {
            Assert.IsFalse(bridge.login("", legalPass));
            Assert.IsFalse(bridge.isLoggedIn("", legalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_SQLInjection() 
        {
            bridge.register(legalUserName, legalPass);

            Assert.IsFalse(bridge.login(legalUserName, "OR 'a'='a'"));
            Assert.IsFalse(bridge.isLoggedIn(legalUserName, legalPass));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLogOutFromTheSystem_Good() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.logOut(legalUserName));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Good() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.editUsername(legalUserName, "thisIsEdited"));
            Assert.IsTrue(bridge.isUserExist("thisIsEdited"));

            bridge.deleteUser("thisIsEdited", legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Sad_UserNameExist() 
        {
            bridge.register("existingName", legalPass);
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editUsername(legalUserName, "existingName"));
            Assert.IsFalse(bridge.isLoggedIn("existingName", legalPass));

            bridge.deleteUser(legalUserName, legalPass);
            bridge.deleteUser("existingName", legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Sad_IllegalUserName()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editUsername(legalUserName, "k"));
            Assert.IsFalse(bridge.isLoggedIn("k", legalPass));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Bad_IllegalCharacters() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editUsername(legalUserName, "@123 !$%+_-"));
            Assert.IsFalse(bridge.isLoggedIn("@123 !$%+_-", legalPass));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditPassword_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.editPassword(legalUserName, "9876543210"));

            bridge.logOut(legalUserName);

            Assert.IsTrue(bridge.login(legalUserName, "9876543210"));

            bridge.deleteUser(legalUserName, "9876543210");
        }

        [TestMethod]
        public void TestEditPassword_Sad_IllegalPass() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editPassword(legalUserName, "0"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditPassword_Bad_IllegalCharacters()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editPassword(legalUserName, "              "));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Good() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.editAvatar(legalUserName, "newavatar.jpg"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Sad_IllegalPic() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editAvatar(legalUserName, "newavatar.mp3"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Bad_InfectedPic() 
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.editAvatar(legalUserName, "newVIRUSavatar.jpg"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true));
            Assert.IsTrue(bridge.isGameExist("Good Game Name"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalGameName()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.createNewGame("Illegal Game Name                  35", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true));
            Assert.IsFalse(bridge.isGameExist("Illegal Game Name                  35"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalNumberOfPlayers() // TODO: remove? no need for number of players when creating a room
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true));
            Assert.IsFalse(bridge.isGameExist("Good Game Name"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Bad_IllegalCharactersInGameName()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            
            Assert.IsFalse(bridge.createNewGame("Illegal@#Game!@Name?)", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true));
            Assert.IsFalse(bridge.isGameExist("Illegal@#Game!@Name?)"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.logOut(legalUserName);

            bridge.login("GoodName", legalPass);

            IList activeGames = bridge.getActiveGames(bridge.getRank("GoodName"));

            Assert.IsTrue(bridge.joinGame(legalUserName, "Good Game Name", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
            bridge.deleteUser("GoodName", legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Sad_IllegalGame()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.logOut(legalUserName);

            bridge.login("GoodName", legalPass);

            Assert.IsFalse(bridge.joinGame(legalUserName, "gg11", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
            bridge.deleteUser("GoodName", legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_bad_MultiplejoiningAttemptsToFullRoom()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);
            bridge.register("AnotherGoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.logOut(legalUserName);

            bridge.login("AnotherGoodName", legalPass);
            bridge.joinGame(legalUserName, "Good Game Name", legalPlayer);
            bridge.logOut("AnotherGoodName");

            bridge.login("GoodName", legalPass);

            Assert.IsFalse(bridge.joinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.joinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.joinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.joinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.joinGame(legalUserName, "Good Game Name", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
            bridge.deleteUser("GoodName", legalPass);
            bridge.deleteUser("AnotherGoodName", legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Bad_IllegalGame()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.joinGame(legalUserName, "", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
        }




        [TestMethod]
        public void TestSpectateExistingGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.register("GoodName", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.logOut(legalUserName);

            bridge.login("GoodName", legalPass);

            IList activeGames = bridge.getActiveGames();

            Assert.IsTrue(bridge.spectateGame("GoodName", "Good Game Name", "SEAN1234"));

            bridge.deleteUser(legalUserName, legalPass);
            bridge.deleteUser("GoodName", legalPass);
        }

        [TestMethod]
        public void TestSpectateExistingGame_Sad_IllegalGame()
        {
            bridge.register(legalUserName, legalPass);
   
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.spectateGame(legalUserName, "gg12", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestSpectateExistingGame_Bad_IllegalChecters()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsFalse(bridge.spectateGame(legalUserName, "Illegal)@#$%Game!@#$Name", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLeaveGame_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name123", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);

            Assert.IsTrue(bridge.leaveGame(legalUserName, "Good Game Name123", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestReplayGame_Good()//TODO:::how to create a replay??
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            IList replayGames=bridge.getAllGamesReplay();

            Assert.IsTrue(bridge.leaveGame(legalUserName, "Good Game Name", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestSaveTurn_Good()//TODO:::how to create a game to save a turn??
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            Assert.IsTrue(bridge.leaveGame(legalUserName, "Good Game Name", legalPlayer));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestFindGames_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name789", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.createNewGame("Game Not In Rank", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.setRank("Game Not In Rank", 10);

            IList activeGames = bridge.getActiveGames(bridge.getRank(legalUserName));

            Assert.IsTrue(activeGames.Contains("Good Game Name789"));
            Assert.IsFalse(activeGames.Contains("Game Not In Rank"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestFindGames_Sad_NoGamesFound()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            IList activeGames = bridge.getActiveGames(bridge.getRank(legalUserName));

            Assert.IsTrue(activeGames.Count==0);

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestListActiveGames_Good()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);
            bridge.createNewGame("Good Game Name777", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.createNewGame("Game Not In Rank777", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.setRank("Game Not In Rank", 1);

            IList activeGames = bridge.getActiveGames();

            Assert.IsTrue(activeGames.Contains("Good Game Name777"));
            Assert.IsTrue(activeGames.Contains("Game Not In Rank777"));

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestListActiveGames_Sad_NoGamesFound()
        {
            bridge.register(legalUserName, legalPass);
            bridge.login(legalUserName, legalPass);

            IList activeGames = bridge.getActiveGames();

            Assert.IsTrue(activeGames.Count == 0);

            bridge.deleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestGameFull_Good()
        {
            //login and register 2 pleyers
            bridge.register(legalUserName, legalPass);
            bridge.register(legalUserName + "1", legalPass);

            bridge.login(legalUserName, legalPass);
            bridge.login(legalUserName + "1", legalPass);
            //create and join to players to a game
            bridge.createNewGame("Good Game Name", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 8, true);
            bridge.joinGame(legalUserName + "1", "Good Game Name",legalPlayer+"1");
            //play the game-round 1
            Assert.IsTrue(bridge.raiseingame(50, "Good Game Name", legalPlayer));
            Assert.IsTrue(bridge.callingame("Good Game Name", legalPlayer+"1"));
            //round 2
            Assert.IsTrue(bridge.foldingame("Good Game Name", legalPlayer));
            bridge.leaveGame(legalUserName, "Good Game Name", legalPlayer);


            bridge.deleteUser(legalUserName, legalPass);
            bridge.deleteUser(legalUserName+"1", legalPass);

        }


    }

}
