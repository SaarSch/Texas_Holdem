using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.GamePrefrences;

namespace TexasHoldem.Bridges
{
    public class ProxyBridge : IBridge
    {
        private IBridge real;
        public ProxyBridge()
        {
            //real = null;
            real = new RealBridge();
        }
        public bool Register(string username, string pass)
        {
            return  real==null || real.Register(username, pass);
            
        }

        public bool IsUserExist(string username)
        {
            return real == null || real.IsUserExist(username);
        }

        public bool DeleteUser(string username, string password)
        {
            return real == null || real.DeleteUser(username, password);
        }

        public bool Login(string username, string pass)
        {
            return real == null || real.Login(username, pass);
        }

        public bool IsLoggedIn(string username, string pass)
        {
            return real == null || real.IsLoggedIn(username, pass);
        }

        public bool LogOut(string username)
        {
            return real == null || real.LogOut(username);
        }

        public bool EditUsername(string username, string newusername)
        {
            return real == null || real.EditUsername(username, newusername);
        }

        public bool EditPassword(string username, string newPass)
        {
            return real == null || real.EditPassword(username, newPass);
        }

        public bool EditAvatar(string username, string newPath)
        {
            return real == null || real.EditAvatar(username, newPath);
        }

        public bool CreateNewGame(string gameName, string username, string creatorName)
        {
            return real == null || real.CreateNewGame(gameName, username, creatorName);
        }

        public bool CreateNewGameWithPrefrences(string gameName, string username, string creatorName, string gameType, int buyInPolicy,
            int chipPolicy, int minBet, int minPlayers, int maxPlayer, bool spectating)
        {
            return real == null || real.CreateNewGameWithPrefrences(gameName, username, creatorName, gameType, buyInPolicy,
            chipPolicy, minBet, minPlayers, maxPlayer, spectating);
        }

        public bool IsGameExist(string gameName)
        {
            return real == null || real.IsGameExist(gameName);
        }

        public IList findGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag)
        {
            IList s;
            if (real != null)
                s = real.findGames(username, playerName, playerFlag, potSize, potFlag,
                gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
            spectating, prefFlag, leagueFlag);
            else
            {
                s = new List<string>();
                s.Add("Good Game Name");
            }
            return s;
        }

        public IList findGames(string username)
        {
            IList s;
            if (real != null)
                s = real.findGames(username);
            else
            {
                s = new List<string>();
                s.Add("Good Game Name");
            }
            return s;
        }

        public bool JoinGame(string username, string roomName, string playerName)
        {
            return real == null || real.JoinGame(username, roomName, playerName);
        }

        public bool SpectateGame(string username, string roomName, string playerName)
        {
            return real == null || real.SpectateGame(username, roomName, playerName);
        }

        public bool LeaveGame(string username, string roomName, string playerName)
        {
            return real == null || real.LeaveGame(username, roomName, playerName);
        }

        public int GetRank(string username)
        {
            return real?.GetRank(username) ?? 0;
        }
       

        public bool RaiseInGame(int raiseamount, string gamename, string playername)
        {
            return real == null || real.RaiseInGame(raiseamount, gamename, playername);
        }

        public bool CallInGame(string gamename, string playername)
        {
            return real == null || real.CallInGame(gamename, playername);
        }

        public bool FoldInGame(string gameName, string playerName)
        {
            return real == null || real.FoldInGame(gameName, playerName);
        }

        public bool SetExpCriteria(string username, int exp)
        {
            return real == null || real.SetExpCriteria(username, exp);
        }

        public bool SetDefaultRank(string username, int rank)
        {
            return real == null || real.SetDefaultRank(username, rank);
        }

        public bool SetUserLeague(string username, string usernameToSet, int rank)
        {
            return real == null || real.SetUserLeague(username, usernameToSet, rank);
        }

        public bool SaveTurn(string roomName, int turnNum)
        {
            return real == null || real.SaveTurn(roomName, turnNum);
        }

        public bool RestartGameCenter()
        {
            return real == null || real.RestartGameCenter();
        }

        public bool StartGame(string roomName)
        {
            return real == null || real.StartGame(roomName);
        }

        public bool SetBet(string roomName, string PlayerName, int bet)
        {
            return real == null || real.SetBet(roomName, PlayerName, bet);
        }

        public void SetUserRank(string legalUserName, int newrank)
        {
            real?.SetUserRank(legalUserName, newrank);
        }
    }
}
