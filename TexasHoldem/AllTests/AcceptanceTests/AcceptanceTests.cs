using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Bridges;
using TexasHoldem.Game;
using TexasHoldem.Services;

namespace AllTests.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTests
    {
        private IBridge _bridge;
        private const string LegalPass = "123456789";
        private const string LegalPlayer = "eladkaminGameName";
        private const string LegalUserName = "eladkamin";
        private const string LegalRoomName = "Good Room Name";

        [TestInitialize]
        public void Initialize()
        {
            _bridge = new ProxyBridge();

			_bridge.Register(LegalUserName, LegalPass);
			for (int i = 0; i < 10; i++)
	        {
				_bridge.Register(LegalUserName + i, LegalPass);
			}
        }

        [TestCleanup] // happens after each test
        public void Cleanup() 
        {
            _bridge.RestartGameCenter();
		}

        [TestMethod]
        public void TestRegisterToTheSystem_Good()
        {
            Assert.IsTrue(_bridge.Register(LegalUserName + "gf", LegalPass));
            Assert.IsTrue(_bridge.IsUserExist(LegalUserName + "gf"));
        }

        [TestMethod]
        public void TestRegisterToTheSystem_Sad_ExistingUserName()
        {
	        Assert.IsFalse(_bridge.Register(LegalUserName, LegalPass));
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
            Assert.IsFalse(_bridge.Register(LegalUserName + "5343", "1"));
            Assert.IsFalse(_bridge.IsUserExist(LegalUserName + "5343"));
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
            Assert.IsFalse(_bridge.Register(LegalUserName + "abcdssaef", "           "));
            Assert.IsFalse(_bridge.IsUserExist(LegalUserName + "abcdssaef"));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Good()
        {
            Assert.IsTrue(_bridge.Login(LegalUserName, LegalPass));
            Assert.IsTrue(_bridge.IsLoggedIn(LegalUserName, LegalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_UserNotExist()
        {
            Assert.IsFalse(_bridge.Login(LegalUserName + "abcdef", LegalPass));
            Assert.IsFalse(_bridge.IsLoggedIn(LegalUserName + "abcdef", LegalPass));
        }

        [TestMethod]
        public void TestLoginToTheSystem_Sad_WrongPass()
        {
            Assert.IsFalse(_bridge.Login(LegalUserName, "notThePass1"));
            Assert.IsFalse(_bridge.IsLoggedIn(LegalUserName, LegalPass));
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
            Assert.IsFalse(_bridge.Login(LegalUserName, "OR 'a'='a'"));
	        Assert.IsFalse(_bridge.IsLoggedIn(LegalUserName, LegalPass));
        }

        [TestMethod]
        public void TestLogOutFromTheSystem_Good()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.LogOut(LegalUserName));
        }

        [TestMethod]
        public void TestEditUsername_Good()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.EditUsername(LegalUserName, "thisIsEdited"));
            Assert.IsTrue(_bridge.IsUserExist("thisIsEdited"));
        }

        [TestMethod]
        public void TestEditUsername_Sad_UserNameExist()
        {
            _bridge.Register("existingName", LegalPass);
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditUsername(LegalUserName, "existingName"));
            Assert.IsFalse(_bridge.IsLoggedIn("existingName", LegalPass));
        }

        [TestMethod]
        public void TestEditUsername_Sad_IllegalUserName()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditUsername(LegalUserName, "k"));
            Assert.IsFalse(_bridge.IsLoggedIn("k", LegalPass));
        }

        [TestMethod]
        public void TestEditUsername_Bad_IllegalCharacters()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditUsername(LegalUserName, "@123 !$%+_-"));
            Assert.IsFalse(_bridge.IsLoggedIn("@123 !$%+_-", LegalPass));
        }

        [TestMethod]
        public void TestEditPassword_Good()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.EditPassword(LegalUserName, "9876543210"));

            _bridge.LogOut(LegalUserName);

            Assert.IsTrue(_bridge.Login(LegalUserName, "9876543210"));
        }

        [TestMethod]
        public void TestEditPassword_Sad_IllegalPass()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditPassword(LegalUserName, "0"));
        }

        [TestMethod]
        public void TestEditPassword_Bad_IllegalCharacters()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditPassword(LegalUserName, "              "));
        }

        [TestMethod]
        public void TestEditAvatar_Good()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.EditAvatar(LegalUserName, "newavatar.jpg"));
        }

        [TestMethod]
        public void TestEditAvatar_Sad_IllegalPic()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.EditAvatar(LegalUserName, "newavatar.mp3"));
        }

        [TestMethod]
        public void TestEditAvatar_Bad_InfectedPic()
        {
            _bridge.Login(LegalUserName, LegalPass);

	        Assert.IsFalse(_bridge.EditAvatar(LegalUserName, "newVIRUSavatar.jpg"));
		}

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Good()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsTrue(_bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer));
            Assert.IsTrue(_bridge.IsGameExist(LegalRoomName));
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Sad_IllegalGameName()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.CreateNewGame("Illegal Game Name                  35", LegalUserName, LegalPlayer));
            Assert.IsFalse(_bridge.IsGameExist("Illegal Game Name                  35"));

            
        }

        [TestMethod]
        public void
            TestCreateNewTexasHoldemGame_Sad_IllegalNumberOfPlayers()
        {
            
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.CreateNewGameWithPrefrences("Good Game Name2", LegalUserName, LegalPlayer, "NoLimit", 1, 0, 4, 2, 11,
                true));
            Assert.IsFalse(_bridge.IsGameExist("Good Game Name2"));

            
        }

        [TestMethod]
        public void TestCreateNewTexasHoldemGame_Bad_IllegalCharactersInGameName()
        {
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.CreateNewGame("Illegal@#Game!@Name?)", LegalUserName, LegalPlayer));
            Assert.IsFalse(_bridge.IsGameExist("Illegal@#Game!@Name?)"));
        }

        [TestMethod]
        public void TestJoinExistingGame_Good()
        {
            _bridge.Register("GoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name1568", LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("GoodName", LegalPass);

            Assert.IsTrue(_bridge.JoinGame("GoodName", "Good Game Name1568", "imaplayer"));
        }

        [TestMethod]
        public void TestJoinExistingGame_Sad_IllegalGame()
        {
            
            _bridge.Register("GoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("GoodName", LegalPass);

            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "gg11", LegalPlayer));
        }

        [TestMethod]
        public void TestJoinExistingGame_bad_MultiplejoiningAttemptsToFullRoom()
        {
            
            _bridge.Register("GoodName", LegalPass);
            _bridge.Register("AnotherGoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("AnotherGoodName", LegalPass);
            _bridge.JoinGame(LegalUserName, LegalRoomName, LegalPlayer);
            _bridge.LogOut("AnotherGoodName");

            _bridge.Login("GoodName", LegalPass);

            Assert.IsFalse(_bridge.JoinGame(LegalUserName, LegalRoomName, LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, LegalRoomName, LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, LegalRoomName, LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, LegalRoomName, LegalPlayer));
            Assert.IsFalse(_bridge.JoinGame(LegalUserName, LegalRoomName, LegalPlayer));
        }

        [TestMethod]
        public void TestJoinExistingGame_Bad_IllegalGame()
        {
            
            _bridge.Login(LegalUserName, LegalPass);

            Assert.IsFalse(_bridge.JoinGame(LegalUserName, "", LegalPlayer)); // the room shouldn't exist because it's name is illegal

            
        }


        [TestMethod]
        public void TestSpectateExistingGame_Good()
        {
            _bridge.Register("GoodName", LegalPass);

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.LogOut(LegalUserName);

            _bridge.Login("GoodName", LegalPass);

            Assert.IsTrue(_bridge.SpectateGame("GoodName", LegalRoomName, "SEAN1234"));
        }

        [TestMethod]
        public void TestSpectateExistingGame_Sad_IllegalGame()
        {
            _bridge.Login(LegalUserName, LegalPass);
            Assert.IsFalse(_bridge.SpectateGame(LegalUserName, "gg12", LegalPlayer));
        }

        [TestMethod]
        public void TestSpectateExistingGame_Bad_IllegalChecters()
        {
            _bridge.Login(LegalUserName, LegalPass);
            Assert.IsFalse(_bridge.SpectateGame(LegalUserName, "Illegal)@#$%Game!@#$Name", LegalPlayer));
        }

        [TestMethod]
        public void TestLeaveGame_Good()
        {
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name123", LegalUserName, LegalPlayer);

            Assert.IsTrue(_bridge.LeaveGame(LegalUserName, "Good Game Name123", LegalPlayer));
        }

        [TestMethod]
        public void TestFindGames_Good()
        {
            
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.CreateNewGame("Good Game Name564", LegalUserName, LegalPlayer);
            _bridge.CreateNewGame("Game Not In Rank", LegalUserName, LegalPlayer);

            var rf = new RoomFilter {LeagueOnly = true , GameType = "NoLimit", BuyInPolicy = 1, ChipPolicy = 0, MinBet = 4, MinPlayers = 2, SepctatingAllowed = true};
            var activeGames = _bridge.FindGames(LegalUserName, rf);

            Assert.IsTrue(activeGames.Contains("Good Game Name564"));
            Assert.IsFalse(activeGames.Contains("Game Not In Rank"));

            
        }

        [TestMethod]
        public void TestFindGames_Sad_NoGamesFound()
        {
            
            _bridge.Login(LegalUserName, LegalPass);

            var rf = new RoomFilter { LeagueOnly = true };
            var activeGames = _bridge.FindGames(LegalUserName, rf);

            Assert.IsTrue(activeGames.Count == 0);

            
        }

        [TestMethod]
        public void TestGameFull_Good()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            //play the game-round 1
            _bridge.StartGame(LegalRoomName);
            Assert.IsTrue(_bridge.RaiseInGame(50, LegalRoomName, LegalPlayer));
            Assert.IsTrue(_bridge.CallInGame(LegalRoomName, LegalPlayer + "1"));

            //round 2-afterflop
            Assert.IsTrue(_bridge.RaiseInGame(50, LegalRoomName, LegalPlayer));
            Assert.IsTrue(_bridge.CallInGame(LegalRoomName, LegalPlayer + "1"));

            //round 3-afterturn
            Assert.IsTrue(_bridge.RaiseInGame(50, LegalRoomName, LegalPlayer));
            Assert.IsTrue(_bridge.CallInGame(LegalRoomName, LegalPlayer + "1"));

            //round 4-afterriver
            Assert.IsTrue(_bridge.FoldInGame(LegalRoomName, LegalPlayer));
	        _bridge.LeaveGame(LegalUserName, LegalRoomName, LegalPlayer);

        }

        [TestMethod]
        public void TestGameFull_Sad_IllegalBet()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            //play the game-round 1
            _bridge.StartGame(LegalRoomName);
            Assert.IsFalse(_bridge.RaiseInGame(Int32.MaxValue, LegalRoomName, LegalPlayer));
        }

        [TestMethod]
        public void TestGameFull_Bad_illegalCharacters()
        {
            //login 2 players
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            //play the game-round 1
            int e;
            int.TryParse("e", out e);
            _bridge.StartGame(LegalRoomName);
            Assert.IsFalse(_bridge.RaiseInGame(e, LegalRoomName, LegalPlayer));
        }

        [TestMethod]
        public void TestMessageStream_Good_PlayerToEveryone()
        {
            //login 2 players
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            _bridge.Login(LegalUserName + "2", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            _bridge.SpectateGame(LegalUserName + "2", LegalRoomName, LegalPlayer + "2");

            _bridge.SendMessageToEveryone(LegalRoomName, false, LegalPlayer, "Hiiii!");

            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "1");
            Assert.AreEqual(1, messages.Count);
            messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "2");
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Good_PlayerToPlayer()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");

            _bridge.SendWhisper(LegalRoomName, false, LegalPlayer, LegalPlayer + "1", "Hiiii!");
            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "1");
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Good_PlayerToSpectator()
        {
            //login 2 players
            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.SpectateGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");

            _bridge.SendWhisper(LegalRoomName, false, LegalPlayer, LegalUserName + "1", "Hiiii!");
            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "1");
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Good_SpectatorToEveryone()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            _bridge.Login(LegalUserName + "2", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.SpectateGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            _bridge.SpectateGame(LegalUserName + "2", LegalRoomName, LegalPlayer + "2");

            _bridge.SendMessageToEveryone(LegalRoomName, true, LegalUserName + "2", "Hiiii!");

            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName); // player
            Assert.AreEqual(0, messages.Count);
            messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "2"); // spectator
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Good_SpectatorToSpectator()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            _bridge.Login(LegalUserName + "2", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.SpectateGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            _bridge.SpectateGame(LegalUserName + "2", LegalRoomName, LegalPlayer + "2");

            _bridge.SendWhisper(LegalRoomName, true, LegalUserName + "1", LegalUserName + "2", "Hiiii!");
            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "2");
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Bad_SpectatorToPlayer()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.SpectateGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");

            _bridge.SendWhisper(LegalRoomName, true, LegalPlayer + "1", LegalPlayer, "Hiiii!");
            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "1");
	        Assert.AreEqual(0, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Sad_EmptyMessage()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            _bridge.Login(LegalUserName + "2", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            _bridge.SpectateGame(LegalUserName + "2", LegalRoomName, LegalPlayer + "2");

            _bridge.SendMessageToEveryone(LegalRoomName, false, LegalPlayer, "");

            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "1");
            Assert.AreEqual(0, messages.Count);
            messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "2");
            Assert.AreEqual(0, messages.Count);
        }

        [TestMethod]
        public void TestMessageStream_Sad_MaliciousMessage()
        {
            //login 2 players

            _bridge.Login(LegalUserName, LegalPass);
            _bridge.Login(LegalUserName + "1", LegalPass);
            _bridge.Login(LegalUserName + "2", LegalPass);
            //create and join to players to a game
            _bridge.CreateNewGame(LegalRoomName, LegalUserName, LegalPlayer);
            _bridge.JoinGame(LegalUserName + "1", LegalRoomName, LegalPlayer + "1");
            _bridge.SpectateGame(LegalUserName + "2", LegalRoomName, LegalPlayer + "2");

            _bridge.SendMessageToEveryone(LegalRoomName, false, LegalPlayer, "fuck SHIT ^%^%#^$wegw^%et$");

            var messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "1");
            Assert.AreEqual(0, messages.Count);
            messages = _bridge.GetMessages(LegalRoomName, LegalUserName + "2");
            Assert.AreEqual(0, messages.Count);
        }


    }
}