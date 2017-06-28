using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Services;

namespace TexasHoldem.Bridges
{
    public class RealBridge : IBridge
    {
        private readonly GameFacade _gameManager;
        private readonly UserFacade _userManager;

        public RealBridge()
        {
            _userManager = new UserFacade("TestDatabase"); // Acceptance tests use a seperate test-db
            _gameManager = new GameFacade("TestDatabase");
        }

        public bool Register(string userName, string pass)
        {
            try
            {
                _userManager.Register(userName, pass);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool IsUserExist(string userName)
        {
            try
            {
                _userManager.Login(userName, userName);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("ERROR in Login: Username does not exist!"))
                    return false;
            }
            return true;
        }

        public bool DeleteUser(string username, string password)
        {
            try
            {
                _userManager.DeleteUser(username, password);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Login(string userName, string pass)
        {
            try
            {
                _userManager.Login(userName, pass);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsLoggedIn(string userName, string pass)
        {
            return _userManager.IsUserLoggedIn(userName, pass);
        }

        public bool LogOut(string userName)
        {
            try
            {
                _userManager.Logout(userName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool EditUsername(string username, string newUsername)
        {
            try
            {
                _userManager.EditProfileUsername(username, newUsername);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool EditPassword(string username, string newPass)
        {
            try
            {
                _userManager.EditProfilePassword(username, newPass);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool EditAvatar(string username, string newPath)
        {
            try
            {
                _userManager.EditProfileAvatarPath(username, newPath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CreateNewGame(string gameName, string username, string creatorName)
        {
            try
            {
                _gameManager.CreateGame(gameName, username, creatorName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CreateNewGameWithPrefrences(string gameName, string username, string creatorName, string gameType,
            int buyInPolicy,
            int chipPolicy, int minBet, int minPlayers, int maxPlayer, bool spectating)
        {
            try
            {
                _gameManager.CreateGameWithPreferences(gameName, username, creatorName, gameType, buyInPolicy,
                    chipPolicy, minBet, minPlayers, maxPlayer, spectating);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsGameExist(string gameName)
        {
            return _gameManager.IsRoomExist(gameName);
        }

        public IList FindGames(string username)
        {
            try
            {
                var tmp = _gameManager.FindGames(username);
                var ans = new List<string>();
                foreach (var r in tmp)
                    ans.Add(r.Name);
                return ans;
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }

        public bool JoinGame(string username, string roomName, string playerName)
        {
            try
            {
                _gameManager.JoinGame(username, roomName, playerName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool SpectateGame(string username, string roomName, string playerName)
        {
            try
            {
                _gameManager.SpectateGame(username, roomName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool LeaveGame(string username, string roomName, string playerName)
        {
            try
            {
                _gameManager.LeaveGame(username, roomName, playerName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public int GetRank(string userName)
        {
            return _userManager.GetRank(userName);
        }

        public bool RaiseInGame(int raiseamount, string gamename, string playername)
        {
            try
            {
                _gameManager.PlaceBet(gamename, playername, raiseamount);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CallInGame(string gamename, string playername)
        {
            try
            {
                _gameManager.Call(gamename, playername);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FoldInGame(string gameName, string playerName)
        {
            try
            {
                _gameManager.Fold(gameName, playerName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool SendMessageToEveryone(string roomName, bool isSpectator, string senderPlayerName, string message)
        {
            try
            {
                if (isSpectator)
                    _gameManager.SpectatorsSendMessage(roomName, senderPlayerName, message);
                else
                    _gameManager.PlayerSendMessage(roomName, senderPlayerName, message);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool SendWhisper(string roomName, bool isSpectator, string senderPlayerName, string receiverPlayerName,
            string message)
        {
            try
            {
                if (isSpectator)
                    _gameManager.SpectatorWhisper(roomName, senderPlayerName, receiverPlayerName, message);
                else
                    _gameManager.PlayerWhisper(roomName, senderPlayerName, receiverPlayerName, message);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public IList GetMessages(string roomName, string username)
        {
            return _userManager.GetMessages(username, roomName);
        }

        public bool RestartGameCenter(bool deleteUsers)
        {
            return _gameManager.RestartGameCenter(deleteUsers);
        }

        public bool StartGame(string roomName)
        {
            try
            {
                _gameManager.StartGame(roomName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool SetBet(string roomName, string playerName, int bet)
        {
            try
            {
                _gameManager.PlaceBet(roomName, playerName, bet);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool GetStat(string userName)
        {

           return _gameManager.GetStat(userName)!=null;

        }

        public IList GetLead20(int i)
        {
                var tmp = _gameManager.GetTopStat(i);
                return tmp?.Select(r => r.Username).ToList() ?? new List<string>();
        }
    }
}