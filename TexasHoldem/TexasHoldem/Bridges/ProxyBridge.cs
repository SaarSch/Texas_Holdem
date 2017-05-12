using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Services;

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


        public IList findGames(string username, RoomFilter filter)
        {
            IList s;
            if (real != null)
                s = real.findGames(username, filter);
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

        public bool foldInGame(string gameName, string playerName)
        {
            return real == null || real.foldInGame(gameName, playerName);
        }

        public bool setExpCriteria(string username, int exp)
        {
            return real == null || real.setExpCriteria(username, exp);
        }

        public bool setDefaultRank(string username, int rank)
        {
            return real == null || real.setDefaultRank(username, rank);
        }

        public bool setUserLeague(string username, string usernameToSet, int rank)
        {
            return real == null || real.setUserLeague(username, usernameToSet, rank);
        }

        public bool saveTurn(string roomName, int turnNum)
        {
            return real == null || real.saveTurn(roomName, turnNum);
        }

        public bool restartGameCenter()
        {
            return real == null || real.restartGameCenter();
        }

        public bool startGame(string roomName)
        {
            return real == null || real.startGame(roomName);
        }

        public bool setBet(string roomName, string PlayerName, int bet)
        {
            return real == null || real.setBet(roomName, PlayerName, bet);
        }

        public void setUserRank(string legalUserName, int newrank)
        {
            real?.setUserRank(legalUserName, newrank);
        }
    }
}
