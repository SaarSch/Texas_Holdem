using System.Collections;
using System.Collections.Generic;

namespace TexasHoldem.Bridges
{
    public class ProxyBridge : IBridge
    {
        private readonly IBridge _real;

        public ProxyBridge()
        {
            _real = new RealBridge();
        }

        public bool Register(string username, string pass)
        {
            return _real == null || _real.Register(username, pass);
        }

        public bool IsGameExist(string gameName)
        {
            return _real == null || _real.IsGameExist(gameName);
        }

        public bool IsUserExist(string username)
        {
            return _real == null || _real.IsUserExist(username);
        }

        public bool DeleteUser(string username, string password)
        {
            return _real == null || _real.DeleteUser(username, password);
        }

        public bool Login(string username, string pass)
        {
            return _real == null || _real.Login(username, pass);
        }

        public bool IsLoggedIn(string username, string pass)
        {
            return _real == null || _real.IsLoggedIn(username, pass);
        }

        public bool LogOut(string username)
        {
            return _real == null || _real.LogOut(username);
        }

        public bool EditUsername(string username, string newusername)
        {
            return _real == null || _real.EditUsername(username, newusername);
        }

        public bool EditPassword(string username, string newPass)
        {
            return _real == null || _real.EditPassword(username, newPass);
        }

        public bool EditAvatar(string username, string newPath)
        {
            return _real == null || _real.EditAvatar(username, newPath);
        }

        public bool CreateNewGame(string gameName, string username, string creatorName)
        {
            return _real == null || _real.CreateNewGame(gameName, username, creatorName);
        }

        public bool CreateNewGameWithPrefrences(string gameName, string username, string creatorName, string gameType,
            int buyInPolicy,
            int chipPolicy, int minBet, int minPlayers, int maxPlayer, bool spectating)
        {
            return _real == null || _real.CreateNewGameWithPrefrences(gameName, username, creatorName, gameType,
                       buyInPolicy,
                       chipPolicy, minBet, minPlayers, maxPlayer, spectating);
        }


        public IList FindGames(string username)
        {
            var s = _real != null ? _real.FindGames(username) : new List<string> {"Good Game Name"};
            return s;
        }

        public IList FindGamesWithFilter(string legalUserName, bool leagueOnly, string gameType, int buyInPolicy, int chipPolicy,
            int minBet, int minPlayers, bool sepctatingAllowed)
        {
            var s = _real != null ? _real.FindGamesWithFilter(legalUserName, leagueOnly, gameType, buyInPolicy, chipPolicy,
            minBet, minPlayers, sepctatingAllowed) : new List<string> { "Good Game Name" };
            return s;
        }

        public bool JoinGame(string username, string roomName, string playerName)
        {
            return _real == null || _real.JoinGame(username, roomName, playerName);
        }

        public bool SpectateGame(string username, string roomName, string playerName)
        {
            return _real == null || _real.SpectateGame(username, roomName, playerName);
        }

        public bool LeaveGame(string username, string roomName, string playerName)
        {
            return _real == null || _real.LeaveGame(username, roomName, playerName);
        }

        public int GetRank(string username)
        {
            return _real?.GetRank(username) ?? 0;
        }


        public bool RaiseInGame(int raiseamount, string gamename, string playername)
        {
            return _real == null || _real.RaiseInGame(raiseamount, gamename, playername);
        }

        public bool CallInGame(string gamename, string playername)
        {
            return _real == null || _real.CallInGame(gamename, playername);
        }

        public bool FoldInGame(string gameName, string playerName)
        {
            return _real == null || _real.FoldInGame(gameName, playerName);
        }

        public bool SendMessageToEveryone(string roomName, bool isSpectator, string senderPlayerName, string message)
        {
            return _real == null || _real.SendMessageToEveryone(roomName, isSpectator, senderPlayerName, message);
        }

        public bool SendWhisper(string roomName, bool isSpectator, string senderPlayerName, string receiverPlayerName,
            string message)
        {
            return _real == null || _real.SendWhisper(roomName, isSpectator, senderPlayerName, receiverPlayerName,
                       message);
        }

        public IList GetMessages(string roomName, string username)
        {
            var s = _real != null ? _real.GetMessages(roomName, username) : new List<string>();
            return s;
        }

        public bool RestartGameCenter(bool deleteUsers)
        {
            return _real == null || _real.RestartGameCenter(deleteUsers);
        }

        public bool StartGame(string roomName)
        {
            return _real == null || _real.StartGame(roomName);
        }

        public bool SetBet(string roomName, string playerName, int bet)
        {
            return _real == null || _real.SetBet(roomName, playerName, bet);
        }

        public bool GetStat(string userName)
        {
            return _real == null || _real.GetStat(userName);
        }

        public IList GetLead20(int i)
        {
            var s = _real != null ? _real.GetLead20(i) : new List<string>();
            return s;
        }
    }
}