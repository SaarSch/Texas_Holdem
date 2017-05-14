using System.Collections;
using System.Collections.Generic;

namespace TexasHoldem.Bridges
{
    public class ProxyBridge : IBridge
    {
        private readonly IBridge _real;
        public ProxyBridge()
        {
            //real = null;
            _real = new RealBridge();
        }
        public bool Register(string username, string pass)
        {
            return  _real==null || _real.Register(username, pass);
            
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

        public bool CreateNewGameWithPrefrences(string gameName, string username, string creatorName, string gameType, int buyInPolicy,
            int chipPolicy, int minBet, int minPlayers, int maxPlayer, bool spectating)
        {
            return _real == null || _real.CreateNewGameWithPrefrences(gameName, username, creatorName, gameType, buyInPolicy,
            chipPolicy, minBet, minPlayers, maxPlayer, spectating);
        }

        public bool IsGameExist(string gameName)
        {
            return _real == null || _real.IsGameExist(gameName);
        }

        public IList findGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag)
        {
            IList s;
            if (_real != null)
                s = _real.findGames(username, playerName, playerFlag, potSize, potFlag,
                gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
            spectating, prefFlag, leagueFlag);
            else
            {
                s = new List<string> {"Good Game Name"};
            }
            return s;
        }

        public IList findGames(string username)
        {
            var s = _real != null ? _real.findGames(username) : new List<string> {"Good Game Name"};
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

        public bool SetExpCriteria(string username, int exp)
        {
            return _real == null || _real.SetExpCriteria(username, exp);
        }

        public bool SetDefaultRank(string username, int rank)
        {
            return _real == null || _real.SetDefaultRank(username, rank);
        }

        public bool SetUserLeague(string username, string usernameToSet, int rank)
        {
            return _real == null || _real.SetUserLeague(username, usernameToSet, rank);
        }

        public bool SaveTurn(string roomName, int turnNum)
        {
            return _real == null || _real.SaveTurn(roomName, turnNum);
        }

        public bool RestartGameCenter()
        {
            return _real == null || _real.RestartGameCenter();
        }

        public bool StartGame(string roomName)
        {
            return _real == null || _real.StartGame(roomName);
        }

        public bool SetBet(string roomName, string playerName, int bet)
        {
            return _real == null || _real.SetBet(roomName, playerName, bet);
        }

        public void SetUserRank(string legalUserName, int newrank)
        {
            _real?.SetUserRank(legalUserName, newrank);
        }
    }
}
