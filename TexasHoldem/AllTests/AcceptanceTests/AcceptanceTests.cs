using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Bridges;
using TexasHoldem.Services;

namespace AllTests.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTests
    {
        private IBridge _bridge;
        private const string LegalPass = "123456789"; //legal password
        private const string LegalPlayer = "eladkaminGameName"; //legal player name
        private const string LegalUserName = "eladkamin"; //legal userName


        [TestInitialize]
        public void Initialize()
        {
            _bridge = new ProxyBridge();
        }

        [TestCleanup] // happens after each test
        public void Cleanup() 
        {
            _bridge.RestartGameCenter();
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Good()
        {
            Assert.IsTrue(_bridge.Register(LegalUserName, LegalPass));
            Assert.IsTrue(_bridge.IsUserExist(LegalUserName));
            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_ExistingUserName()
        {
            Assert.IsTrue(_bridge.Register(LegalUserName, LegalPass));
            Assert.IsFalse(_bridge.Register(LegalUserName, LegalPass));
            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalUserName()
        {
            Assert.IsFalse(_bridge.Register("K", LegalPass));
            Assert.IsFalse(_bridge.IsUserExist("K"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_IllegalPass()
        {
            Assert.IsFalse(_bridge.Register(LegalUserName, "1"));
            Assert.IsFalse(_bridge.IsUserExist(LegalUserName));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersUserName()
        {
            Assert.IsFalse(_bridge.Register("@123 !$%+_-", LegalPass));
            Assert.IsFalse(_bridge.IsUserExist("@123 !$%+_-"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Bad_IllegalCharactersPass()
        {
            Assert.IsFalse(_bridge.Register(LegalUserName, "           "));
            Assert.IsFalse(_bridge.IsUserExist(LegalUserName));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.Login(LegalUserName, LegalPass));
            Assert.IsTrue(_bridge.IsLoggedIn(LegalUserName, LegalPass));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_UserNotExist()
        {
            Assert.IsFalse(_bridge.Login(LegalUserName, LegalPass));
            Assert.IsFalse(_bridge.IsLoggedIn(LegalUserName, LegalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_WrongPass()
        {
            _bridge.Register(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.Login(LegalUserName, "notThePass1"));
            Assert.IsFalse(_bridge.IsLoggedIn(LegalUserName, LegalPass));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_UserNameEmpty()
        {
            Assert.IsFalse(_bridge.Login("", LegalPass));
            Assert.IsFalse(_bridge.IsLoggedIn("", LegalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Bad_SQLInjection()
        {
            _bridge.Register(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.Login(LegalUserName, "OR 'a'='a'"));
            Assert.IsFalse(_bridge.IsLoggedIn(LegalUserName, LegalPass));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestLogOutFromTheSystem_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.LogOut(LegalUserName));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditUsername_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.EditUsername(LegalUserName, "thisIsEdited"));
            Assert.IsTrue(_bridge.IsUserExist("thisIsEdited"));

            _bridge.DeleteUser("thisIsEdited", LegalPass);
        }

        [TestMethod]
        public void TestEditUsername_Sad_UserNameExist()
        {
            _bridge.Register("existingName", LegalPass);
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditUsername(LegalUserName, "existingName"));
            Assert.IsFalse(_bridge.IsLoggedIn("existingName", LegalPass));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser("existingName", LegalPass);
        }

        [TestMethod]
        public void TestEditUsername_Sad_IllegalUserName()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditUsername(LegalUserName, "k"));
            Assert.IsFalse(_bridge.IsLoggedIn("k", LegalPass));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditUsername_Bad_IllegalCharacters()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditUsername(LegalUserName, "@123 !$%+_-"));
            Assert.IsFalse(_bridge.IsLoggedIn("@123 !$%+_-", LegalPass));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditPassword_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.EditPassword(LegalUserName, "9876543210"));

            _bridge.LogOut(LegalUserName);

            Assert.IsTrue(_bridge.Login(LegalUserName, "9876543210"));

            _bridge.DeleteUser(LegalUserName, "9876543210");
        }

        [TestMethod]
        public void TestEditPassword_Sad_IllegalPass()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditPassword(LegalUserName, "0"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditPassword_Bad_IllegalCharacters()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditPassword(LegalUserName, "              "));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.EditAvatar(LegalUserName, "newavatar.jpg"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Sad_IllegalPic()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditAvatar(LegalUserName, "newavatar.mp3"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestEditAvatar_Bad_InfectedPic()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditAvatar(LegalUserName, "newVIRUSavatar.jpg"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer));
            Assert.IsTrue(_bridge.IsGameExist("Good Game Name"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalGameName()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.CreateNewGame("Illegal Game Name                  35", LegalUserName, LegalPlayer));
            Assert.IsFalse(_bridge.IsGameExist("Illegal Game Name                  35"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void
            TestCreateNewTexasHoldemGame_Sad_IllegalNumberOfPlayers()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.CreateNewGameWithPrefrences("Good Game Name2", LegalUserName, LegalPlayer, "NoLimit", 1, 0, 4, 2, 11,
                true));
            Assert.IsFalse(_bridge.IsGameExist("Good Game Name2"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Bad_IllegalCharactersInGameName()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.CreateNewGame("Illegal@#Game!@Name?)", LegalUserName, LegalPlayer));
            Assert.IsFalse(_bridge.IsGameExist("Illegal@#Game!@Name?)"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register("GoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name1568", LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("GoodName", LegalPass);

            Assert.IsTrue(_bridge.JoinGame("GoodName", "Good Game Name1568", "imaplayer"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser("GoodName", LegalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Sad_IllegalGame()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register("GoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("GoodName", LegalPass);

            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "gg11", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser("GoodName", LegalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_bad_MultiplejoiningAttemptsToFullRoom()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register("GoodName", LegalPass);
            _bridge.Register("AnotherGoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("AnotherGoodName", LegalPass);
            _bridge.JoinGame(LegalUserName, "Good Game Name", LegalPlayer);
            _bridge.LogOut("AnotherGoodName");

            _bridge.Login("GoodName", LegalPass);

            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "Good Game Name", LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "Good Game Name", LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "Good Game Name", LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "Good Game Name", LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "Good Game Name", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser("GoodName", LegalPass);
            _bridge.DeleteUser("AnotherGoodName", LegalPass);
        }

        [TestMethod]
        public void TestJoinExistingGame_Bad_IllegalGame()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "", LegalPlayer)); // the room shouldn't exist because it's name is illegal

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }


        [TestMethod]
        public void TestSpectateExistingGame_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register("GoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("GoodName", LegalPass);

            Assert.IsTrue(_bridge.SpectateGame("GoodName", "Good Game Name", "SEAN1234"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser("GoodName", LegalPass);
        }

        [TestMethod]
        public void TestSpectateExistingGame_Sad_IllegalGame()
        {
            _bridge.Register(LegalUserName, LegalPass);

            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.SpectateGame(LegalUserName, "gg12", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestSpectateExistingGame_Bad_IllegalChecters()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.SpectateGame(LegalUserName, "Illegal)@#$%Game!@#$Name", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestLeaveGame_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name123", LegalUserName, LegalPlayer);

            Assert.IsTrue(_bridge.LeaveGame(LegalUserName, "Good Game Name123", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestFindGames_Good()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name564", LegalUserName, LegalPlayer);
            _bridge.CreateNewGame("Game Not In Rank", LegalUserName, LegalPlayer);

            var rf = new RoomFilter {LeagueOnly = true};
            var activeGames = _bridge.FindGames(LegalUserName, rf);

            Assert.IsTrue(activeGames.Contains("Good Game Name564"));
            Assert.IsFalse(activeGames.Contains("Game Not In Rank"));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestFindGames_Sad_NoGamesFound()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            var rf = new RoomFilter { LeagueOnly = true };
            var activeGames = _bridge.FindGames(LegalUserName, rf);

            Assert.IsTrue(activeGames.Count == 0);

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestGameFull_Good()
        {
            //login and register 2 players
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register(LegalUserName + "1", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", "Good Game Name", LegalPlayer + "1");
            //play the game-round 1
            _bridge.StartGame("Good Game Name");
            Assert.IsTrue(_bridge.RaiseInGame(50, "Good Game Name", LegalPlayer));
            Assert.IsTrue(_bridge.CallInGame("Good Game Name", LegalPlayer + "1"));

            //round 2-afterflop
            Assert.IsTrue(_bridge.RaiseInGame(50, "Good Game Name", LegalPlayer));
            Assert.IsTrue(_bridge.CallInGame("Good Game Name", LegalPlayer + "1"));

            //round 3-afterturn
            Assert.IsTrue(_bridge.RaiseInGame(50, "Good Game Name", LegalPlayer));
            Assert.IsTrue(_bridge.CallInGame("Good Game Name", LegalPlayer + "1"));

            //round 4-afterriver
            Assert.IsTrue(_bridge.FoldInGame("Good Game Name", LegalPlayer));
            _bridge.LeaveGame(LegalUserName, "Good Game Name", LegalPlayer);


            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName + "1", LegalPass);
        }

        [TestMethod]
        public void TestGameFull_Sad_IllegalBet()
        {
            //login and register 2 players
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register(LegalUserName + "1", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", "Good Game Name", LegalPlayer + "1");
            //play the game-round 1
            _bridge.StartGame("Good Game Name");
            Assert.IsFalse(_bridge.RaiseInGame(Int32.MaxValue, "Good Game Name", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName + "1", LegalPass);
        }

        [TestMethod]
        public void TestGameFull_Bad_illegalCharacters()
        {
            //login and register 2 players
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Register(LegalUserName + "1", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame("Good Game Name", LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", "Good Game Name", LegalPlayer + "1");
            //play the game-round 1
            int e;
            int.TryParse("e", out e);
            _bridge.StartGame("Good Game Name");
            Assert.IsFalse(_bridge.RaiseInGame(e, "Good Game Name", LegalPlayer));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName + "1", LegalPass);

            _bridge.RestartGameCenter();
        }
       


        [TestMethod]
        public void TestLeagueManagement_Good_changeDefultLeague_changeLeagueOfUser_changeXP()
        {
            _bridge.Register(LegalUserName, LegalPass);
            

            _bridge.Login(LegalUserName, LegalPass);

            _bridge.SetUserRank(LegalUserName,10);//max rank-can manage the league

            //Assert.IsTrue(_bridge.SetDefaultRank(LegalUserName, 5));
            _bridge.Register(LegalUserName + "1", LegalPass);//create new user
            Assert.AreEqual(-1,_bridge.GetRank(LegalUserName+"1"));//checks if the rank is 5

            Assert.IsTrue(_bridge.SetUserLeague(LegalUserName, LegalUserName + "1", 1));
            Assert.AreEqual(1, _bridge.GetRank(LegalUserName + "1"));//checks if the rank had changed to 1

            Assert.IsTrue(_bridge.SetExpCriteria(LegalUserName,19));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName+"1", LegalPass);
        }

        [TestMethod]
        public void TestLeagueManagement_sad_illegalRankLeagueOrExp()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            _bridge.SetUserRank(LegalUserName, 10);//max rank-can manage the league

            //Assert.IsFalse(_bridge.SetDefaultRank(LegalUserName, 300));//the rank is not in range [0,10]

            _bridge.Register(LegalUserName + "1", LegalPass);//create new user
            Assert.AreNotEqual(300, _bridge.GetRank(LegalUserName + "1"));//checks if the rank is not 300

            Assert.IsFalse(_bridge.SetUserLeague(LegalUserName, LegalUserName + "1", 300));//the rank is not in range [0,10]
            Assert.AreNotEqual(300, _bridge.GetRank(LegalUserName + "1"));//checks if the rank had not changed to 300

            Assert.IsFalse(_bridge.SetExpCriteria(LegalUserName, 40));//the exp is not in range [5,20]

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName + "1", LegalPass);
        }

        [TestMethod]
        public void TestLeagueManagement_sad_userDoesnotexist()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            _bridge.SetUserRank(LegalUserName, 10);//max rank-can manage the league

            Assert.IsFalse(_bridge.SetUserLeague(LegalUserName, LegalUserName + "1", 1));

            _bridge.DeleteUser(LegalUserName, LegalPass);
        }

        [TestMethod]
        public void TestLeagueManagement_sad_PermissionDenied()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            _bridge.SetUserRank(LegalUserName, 0);//min rank-can not manage the league

            //Assert.IsFalse(_bridge.SetDefaultRank(LegalUserName, 10));
            _bridge.Register(LegalUserName + "1", LegalPass);//create new user
            Assert.AreNotEqual(10, _bridge.GetRank(LegalUserName + "1"));//checks if the rank is not 10

            Assert.IsFalse(_bridge.SetUserLeague(LegalUserName, LegalUserName + "1", 1));
            Assert.AreNotEqual(1, _bridge.GetRank(LegalUserName + "1"));//checks if the rank had not changed to 1

            Assert.IsFalse(_bridge.SetExpCriteria(LegalUserName, 18));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName + "1", LegalPass);
        }


        [TestMethod]
        public void TestLeagueManagement_Bad()
        {
            _bridge.Register(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            _bridge.SetUserRank(LegalUserName, 10);//max rank-can manage the league
            var i = 'e';
            //Assert.IsFalse(_bridge.SetDefaultRank(LegalUserName,i));

            Assert.IsFalse(_bridge.SetUserLeague(LegalUserName, LegalUserName + "1", i));

            Assert.IsFalse(_bridge.SetExpCriteria(LegalUserName, i));

            _bridge.DeleteUser(LegalUserName, LegalPass);
            _bridge.DeleteUser(LegalUserName + "1", LegalPass);
        }
    }
}