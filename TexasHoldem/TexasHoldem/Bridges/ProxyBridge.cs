using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool register(string username, string pass)
        {
            return  real==null || real.register(username, pass);
            
        }

        public bool isUserExist(string username)
        {
            return real == null || real.isUserExist(username);
        }

        public bool deleteUser(string username, string password)
        {
            return real == null || real.deleteUser(username, password);
        }

        public bool login(string username, string pass)
        {
            return real == null || real.login(username, pass);
        }

        public bool isLoggedIn(string username, string pass)
        {
            return real == null || real.isLoggedIn(username, pass);
        }

        public bool logOut(string username)
        {
            return real == null || real.logOut(username);
        }

        public bool editUsername(string username, string newusername)
        {
            return real == null || real.editUsername(username, newusername);
        }

        public bool editPassword(string username, string newPass)
        {
            return real == null || real.editPassword(username, newPass);
        }

        public bool editAvatar(string username, string newPath)
        {
            return real == null || real.editAvatar(username, newPath);
        }

        public bool createNewGame(string gameName, string username, string creatorName, string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayer, Boolean spectating)
        {
            return real == null || real.createNewGame(gameName, username, creatorName, gameType, buyInPolicy,  chipPolicy,  minBet,  minPlayers,  maxPlayer,  spectating);
        }

        public bool isGameExist(string gameName)
        {
            return real == null || real.isGameExist(gameName);
        }

        public IList findGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
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

        public bool joinGame(string username, string roomName, string playerName)
        {
            return real == null || real.joinGame(username, roomName, playerName);
        }

        public bool spectateGame(string username, string roomName, string playerName)
        {
            return real == null || real.spectateGame(username, roomName, playerName);
        }

        public bool leaveGame(string username, string roomName, string playerName)
        {
            return real == null || real.leaveGame(username, roomName, playerName);
        }

        public IList getAllGameReplays()
        {
            IList s;
            if (real != null)
                s = real.getAllGameReplays();
            else
            {
                s = new List<string>();
                s.Add("Good Game Name");
            }
            return s;
        }

        public int getRank(string username)
        {
            return real?.getRank(username) ?? 0;
        }
       

        public bool raiseInGame(int raiseamount, string gamename, string playername)
        {
            return real == null || real.raiseInGame(raiseamount, gamename, playername);
        }

        public bool callInGame(string gamename, string playername)
        {
            return real == null || real.callInGame(gamename, playername);
        }

        public bool foldInGame(string goodGameName, string legalPlayer)
        {
            return real == null || real.foldInGame(goodGameName, legalPlayer);
        }

        public bool restartGameCenter()
        {
            return real == null || real.restartGameCenter();
        }
    }
}
