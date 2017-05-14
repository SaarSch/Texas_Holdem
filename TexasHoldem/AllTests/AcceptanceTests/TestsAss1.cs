using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Bridges;
using TexasHoldem.GamePrefrences;

namespace AllTests.AcceptanceTests
{
    [TestClass]
    public class TestsAss1
    {
        private IBridge bridge;
        private readonly string legalPass = "123456789"; //legal password
        private readonly string legalPlayer = "eladkaminGameName"; //legal player name
        private readonly string legalUserName = "eladkamin"; //legal userName


        [TestInitialize]
        public void Initialize()
        {
            bridge = new ProxyBridge();
        }

        [TestCleanup()] // happens after each test
        public void Cleanup() 
        {
            bridge.RestartGameCenter();
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Good()
        {
            Assert.IsTrue(bridge.Register(legalUserName, legalPass));
            Assert.IsTrue(bridge.IsUserExist(legalUserName));
            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_ExistingUserName()
        {
            Assert.IsTrue(bridge.Register(legalUserName, legalPass));
            Assert.IsFalse(bridge.Register(legalUserName, legalPass));
            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalUserName()
        {
            Assert.IsFalse(bridge.Register("K", legalPass));
            Assert.IsFalse(bridge.IsUserExist("K"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalPass()
        {
            Assert.IsFalse(bridge.Register(legalUserName, "1"));
            Assert.IsFalse(bridge.IsUserExist(legalUserName));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersUserName()
        {
            Assert.IsFalse(bridge.Register("@123 !$%+_-", legalPass));
            Assert.IsFalse(bridge.IsUserExist("@123 !$%+_-"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersPass()
        {
            Assert.IsFalse(bridge.Register(legalUserName, "           "));
            Assert.IsFalse(bridge.IsUserExist(legalUserName));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Good()
        {
            bridge.Register(legalUserName, legalPass);

            Assert.IsTrue(bridge.Login(legalUserName, legalPass));
            Assert.IsTrue(bridge.IsLoggedIn(legalUserName, legalPass));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_UserNotExist()
        {
            Assert.IsFalse(bridge.Login(legalUserName, legalPass));
            Assert.IsFalse(bridge.IsLoggedIn(legalUserName, legalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_WrongPass()
        {
            bridge.Register(legalUserName, legalPass);

            Assert.IsFalse(bridge.Login(legalUserName, "notThePass1"));
            Assert.IsFalse(bridge.IsLoggedIn(legalUserName, legalPass));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_UserNameEmpty()
        {
            Assert.IsFalse(bridge.Login("", legalPass));
            Assert.IsFalse(bridge.IsLoggedIn("", legalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_SQLInjection()
        {
            bridge.Register(legalUserName, legalPass);

            Assert.IsFalse(bridge.Login(legalUserName, "OR 'a'='a'"));
            Assert.IsFalse(bridge.IsLoggedIn(legalUserName, legalPass));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLogOutFromTheSystem_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsTrue(bridge.LogOut(legalUserName));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsTrue(bridge.EditUsername(legalUserName, "thisIsEdited"));
            Assert.IsTrue(bridge.IsUserExist("thisIsEdited"));

            bridge.DeleteUser("thisIsEdited", legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Sad_UserNameExist()
        {
            bridge.Register("existingName", legalPass);
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditUsername(legalUserName, "existingName"));
            Assert.IsFalse(bridge.IsLoggedIn("existingName", legalPass));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser("existingName", legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Sad_IllegalUserName()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditUsername(legalUserName, "k"));
            Assert.IsFalse(bridge.IsLoggedIn("k", legalPass));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditUsername_Bad_IllegalCharacters()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditUsername(legalUserName, "@123 !$%+_-"));
            Assert.IsFalse(bridge.IsLoggedIn("@123 !$%+_-", legalPass));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditPassword_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsTrue(bridge.EditPassword(legalUserName, "9876543210"));

            bridge.LogOut(legalUserName);

            Assert.IsTrue(bridge.Login(legalUserName, "9876543210"));

            bridge.DeleteUser(legalUserName, "9876543210");
        }

        [TestMethod]
        public void TestEditPassword_Sad_IllegalPass()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditPassword(legalUserName, "0"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditPassword_Bad_IllegalCharacters()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditPassword(legalUserName, "              "));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsTrue(bridge.EditAvatar(legalUserName, "newavatar.jpg"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Sad_IllegalPic()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditAvatar(legalUserName, "newavatar.mp3"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Bad_InfectedPic()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.EditAvatar(legalUserName, "newVIRUSavatar.jpg"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsTrue(bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer));
            Assert.IsTrue(bridge.IsGameExist("Good Game Name"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalGameName()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.CreateNewGame("Illegal Game Name                  35", legalUserName, legalPlayer));
            Assert.IsFalse(bridge.IsGameExist("Illegal Game Name                  35"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void
            TestCreateNewTexasHoldemGame_Sad_IllegalNumberOfPlayers()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.CreateNewGameWithPrefrences("Good Game Name2", legalUserName, legalPlayer, "NoLimit", 1, 0, 4, 2, 11,
                true));
            Assert.IsFalse(bridge.IsGameExist("Good Game Name2"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Bad_IllegalCharactersInGameName()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.CreateNewGame("Illegal@#Game!@Name?)", legalUserName, legalPlayer));
            Assert.IsFalse(bridge.IsGameExist("Illegal@#Game!@Name?)"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Register("GoodName", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.CreateNewGame("Good Game Name1568", legalUserName, legalPlayer);
            bridge.LogOut(legalUserName);

            bridge.Login("GoodName", legalPass);

            var activeGames = bridge.findGames("GoodName", "", false, 0, false, "NoLimit", 0, 0, 5, 3, 4, false, false, true);

            Assert.IsTrue(bridge.JoinGame("GoodName", "Good Game Name1568", "imaplayer"));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser("GoodName", legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Sad_IllegalGame()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Register("GoodName", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.LogOut(legalUserName);

            bridge.Login("GoodName", legalPass);

            Assert.IsFalse(bridge.JoinGame(legalUserName, "gg11", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser("GoodName", legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_bad_MultiplejoiningAttemptsToFullRoom()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Register("GoodName", legalPass);
            bridge.Register("AnotherGoodName", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.LogOut(legalUserName);

            bridge.Login("AnotherGoodName", legalPass);
            bridge.JoinGame(legalUserName, "Good Game Name", legalPlayer);
            bridge.LogOut("AnotherGoodName");

            bridge.Login("GoodName", legalPass);

            Assert.IsFalse(bridge.JoinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.JoinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.JoinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.JoinGame(legalUserName, "Good Game Name", legalPlayer));
            Assert.IsFalse(bridge.JoinGame(legalUserName, "Good Game Name", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser("GoodName", legalPass);
            bridge.DeleteUser("AnotherGoodName", legalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Bad_IllegalGame()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.JoinGame(legalUserName, "", legalPlayer)); // the room shouldn't exist because it's name is illegal

            bridge.DeleteUser(legalUserName, legalPass);
        }


        [TestMethod]
        public void TestSpectateExistingGame_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Register("GoodName", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.LogOut(legalUserName);

            bridge.Login("GoodName", legalPass);

            var activeGames = bridge.findGames(legalUserName);

            Assert.IsTrue(bridge.SpectateGame("GoodName", "Good Game Name", "SEAN1234"));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser("GoodName", legalPass);
        }

        [TestMethod]
        public void TestSpectateExistingGame_Sad_IllegalGame()
        {
            bridge.Register(legalUserName, legalPass);

            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.SpectateGame(legalUserName, "gg12", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestSpectateExistingGame_Bad_IllegalChecters()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            Assert.IsFalse(bridge.SpectateGame(legalUserName, "Illegal)@#$%Game!@#$Name", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLeaveGame_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);
            bridge.CreateNewGame("Good Game Name123", legalUserName, legalPlayer);

            Assert.IsTrue(bridge.LeaveGame(legalUserName, "Good Game Name123", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestSaveTurn_Good()
        {
            //login and register 2 pleyers
            bridge.Register(legalUserName, legalPass);
            bridge.Register(legalUserName + "1", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.Login(legalUserName + "1", legalPass);
            //create and join to players to a game
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.JoinGame(legalUserName + "1", "Good Game Name", legalPlayer + "1");
            //play the game-round 1
            bridge.StartGame("Good Game Name");

            Assert.IsTrue(bridge.SaveTurn("Good Game Name", 1));
            Assert.IsFalse(bridge.SaveTurn("Good Game Name", 7));

            bridge.LeaveGame(legalUserName, "Good Game Name", legalPlayer);
            bridge.LeaveGame(legalUserName + "1", "Good Game Name", legalPlayer + "1");

            bridge.RestartGameCenter();
        }

        [TestMethod]
        public void TestFindGames_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);
            bridge.CreateNewGame("Good Game Name564", legalUserName, legalPlayer);
            bridge.CreateNewGame("Game Not In Rank", legalUserName, legalPlayer);

            var activeGames = bridge.findGames(legalUserName, "", false, 0, false, "NoLimit", 0, 0, 5, 3, 4, false, false, true);

            Assert.IsTrue(activeGames.Contains("Good Game Name564"));
            Assert.IsFalse(activeGames.Contains("Game Not In Rank"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestFindGames_Sad_NoGamesFound()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            var activeGames = bridge.findGames(legalUserName, "", false, 0, false, "NoLimit", 0, 0, 5, 3, 4, false, false, true);

            Assert.IsTrue(activeGames.Count == 0);

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestListActiveGames_Good()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            bridge.CreateNewGameWithPrefrences("Good Game Name777", legalUserName, legalPlayer, "NoLimit", 1, 10, 4, 2, 8, true);
            bridge.CreateNewGameWithPrefrences("Game Not In Rank777", legalUserName, legalPlayer, "NoLimit", 1, 10, 4, 2, 8, true);

            var activeGames = bridge.findGames(legalUserName);

            Assert.IsTrue(activeGames.Contains("Good Game Name777"));
            Assert.IsTrue(activeGames.Contains("Game Not In Rank777"));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestListActiveGames_Sad_NoGamesFound()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            var activeGames = bridge.findGames(legalUserName);

            Assert.IsTrue(activeGames.Count == 0);

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestGameFull_Good()
        {
            //login and register 2 players
            bridge.Register(legalUserName, legalPass);
            bridge.Register(legalUserName + "1", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.Login(legalUserName + "1", legalPass);
            //create and join to players to a game
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.JoinGame(legalUserName + "1", "Good Game Name", legalPlayer + "1");
            //play the game-round 1
            bridge.StartGame("Good Game Name");
            Assert.IsTrue(bridge.RaiseInGame(50, "Good Game Name", legalPlayer));
            Assert.IsTrue(bridge.CallInGame("Good Game Name", legalPlayer + "1"));

            //round 2-afterflop
            Assert.IsTrue(bridge.RaiseInGame(50, "Good Game Name", legalPlayer));
            Assert.IsTrue(bridge.CallInGame("Good Game Name", legalPlayer + "1"));

            //round 3-afterturn
            Assert.IsTrue(bridge.RaiseInGame(50, "Good Game Name", legalPlayer));
            Assert.IsTrue(bridge.CallInGame("Good Game Name", legalPlayer + "1"));

            //round 4-afterriver
            Assert.IsTrue(bridge.FoldInGame("Good Game Name", legalPlayer));
            bridge.LeaveGame(legalUserName, "Good Game Name", legalPlayer);


            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName + "1", legalPass);
        }

        [TestMethod]
        public void TestGameFull_Sad_IllegalBet()
        {
            //login and register 2 players
            bridge.Register(legalUserName, legalPass);
            bridge.Register(legalUserName + "1", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.Login(legalUserName + "1", legalPass);
            //create and join to players to a game
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.JoinGame(legalUserName + "1", "Good Game Name", legalPlayer + "1");
            //play the game-round 1
            bridge.StartGame("Good Game Name");
            Assert.IsFalse(bridge.RaiseInGame(Int32.MaxValue, "Good Game Name", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName + "1", legalPass);
        }

        [TestMethod]
        public void TestGameFull_Bad_illegalCharacters()
        {
            //login and register 2 players
            bridge.Register(legalUserName, legalPass);
            bridge.Register(legalUserName + "1", legalPass);

            bridge.Login(legalUserName, legalPass);
            bridge.Login(legalUserName + "1", legalPass);
            //create and join to players to a game
            bridge.CreateNewGame("Good Game Name", legalUserName, legalPlayer);
            bridge.JoinGame(legalUserName + "1", "Good Game Name", legalPlayer + "1");
            //play the game-round 1
            int e;
            int.TryParse("e", out e);
            bridge.StartGame("Good Game Name");
            Assert.IsFalse(bridge.RaiseInGame(e, "Good Game Name", legalPlayer));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName + "1", legalPass);

            bridge.RestartGameCenter();
        }
       


        [TestMethod]
        public void TestLeagueManagement_Good_changeDefultLeague_changeLeagueOfUser_changeXP()
        {
            bridge.Register(legalUserName, legalPass);
            

            bridge.Login(legalUserName, legalPass);

            bridge.SetUserRank(legalUserName,10);//max rank-can manage the league

            Assert.IsTrue(bridge.SetDefaultRank(legalUserName, 5));
            bridge.Register(legalUserName + "1", legalPass);//create new user
            Assert.AreEqual(-1,bridge.GetRank(legalUserName+"1"));//checks if the rank is 5

            Assert.IsTrue(bridge.SetUserLeague(legalUserName, legalUserName + "1", 1));
            Assert.AreEqual(1, bridge.GetRank(legalUserName + "1"));//checks if the rank had changed to 1

            Assert.IsTrue(bridge.SetExpCriteria(legalUserName,19));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName+"1", legalPass);
        }

        [TestMethod]
        public void TestLeagueManagement_sad_illegalRankLeagueOrExp()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            bridge.SetUserRank(legalUserName, 10);//max rank-can manage the league

            Assert.IsFalse(bridge.SetDefaultRank(legalUserName, 300));//the rank is not in range [0,10]

            bridge.Register(legalUserName + "1", legalPass);//create new user
            Assert.AreNotEqual(300, bridge.GetRank(legalUserName + "1"));//checks if the rank is not 300

            Assert.IsFalse(bridge.SetUserLeague(legalUserName, legalUserName + "1", 300));//the rank is not in range [0,10]
            Assert.AreNotEqual(300, bridge.GetRank(legalUserName + "1"));//checks if the rank had not changed to 300

            Assert.IsFalse(bridge.SetExpCriteria(legalUserName, 40));//the exp is not in range [5,20]

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName + "1", legalPass);
        }

        [TestMethod]
        public void TestLeagueManagement_sad_userDoesnotexist()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            bridge.SetUserRank(legalUserName, 10);//max rank-can manage the league

            Assert.IsFalse(bridge.SetUserLeague(legalUserName, legalUserName + "1", 1));

            bridge.DeleteUser(legalUserName, legalPass);
        }

        [TestMethod]
        public void TestLeagueManagement_sad_PermissionDenied()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            bridge.SetUserRank(legalUserName, 0);//min rank-can not manage the league

            Assert.IsFalse(bridge.SetDefaultRank(legalUserName, 10));
            bridge.Register(legalUserName + "1", legalPass);//create new user
            Assert.AreNotEqual(10, bridge.GetRank(legalUserName + "1"));//checks if the rank is not 10

            Assert.IsFalse(bridge.SetUserLeague(legalUserName, legalUserName + "1", 1));
            Assert.AreNotEqual(1, bridge.GetRank(legalUserName + "1"));//checks if the rank had not changed to 1

            Assert.IsFalse(bridge.SetExpCriteria(legalUserName, 18));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName + "1", legalPass);
        }


        [TestMethod]
        public void TestLeagueManagement_Bad()
        {
            bridge.Register(legalUserName, legalPass);
            bridge.Login(legalUserName, legalPass);

            bridge.SetUserRank(legalUserName, 10);//max rank-can manage the league
            var i = 'e';
            Assert.IsFalse(bridge.SetDefaultRank(legalUserName,i));

            Assert.IsFalse(bridge.SetUserLeague(legalUserName, legalUserName + "1", i));

            Assert.IsFalse(bridge.SetExpCriteria(legalUserName, i));

            bridge.DeleteUser(legalUserName, legalPass);
            bridge.DeleteUser(legalUserName + "1", legalPass);
        }
    }
}