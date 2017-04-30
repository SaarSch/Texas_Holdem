using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Services;

namespace TexasHoldem.Bridges
{
    public class RealBridge:IBridge
    {
        private UserManager userManager;
        private GameManager gameManager;
        private ReplayManager replayManager;

        public RealBridge()
        {
            userManager = new UserManager();
            gameManager = new GameManager();
            replayManager = new ReplayManager();
        }

        public bool register(string userName, string pass)
        {
            try
            {
                userManager.Register(userName, pass);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool isUserExist(string userName)
        {
            try
            {
                userManager.Login(userName, userName);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Username does not exist!"))
                    return false;
            }
            return true;
        }

        public bool deleteUser(string username, string password)
        {
            try
            {
                userManager.DeleteUser(username, password);
            }
            catch (Exception e)
            {
               return false;
            }
            return true;
        }

        public bool login(string userName, string pass)
        {
            try
            {
                userManager.Login(userName, pass);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool isLoggedIn(string userName, string pass)
        {
            return userManager.IsUserLoggedIn(userName, pass);
        }

        public bool logOut(string userName)
        {
            try
            {
                userManager.Logout(userName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool editUsername(string username, string newUsername)
        {
            try
            {
                userManager.EditProfileUsername(username, newUsername);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool editPassword(string username, string newPass)
        {
            try
            {
                userManager.EditProfilePassword(username, newPass);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool editAvatar(string username, string newPath)
        {
            try
            {
                userManager.EditProfileAvatarPath(username, newPath);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool createNewGame(string gameName, string username, string creatorName, string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayer, Boolean spectating)
        {
            Gametype d = Gametype.NoLimit;
            switch (gameType)
            {
                case "NoLimit":
                    d=Gametype.NoLimit;
                    break;
                case "limit":
                    d = Gametype.limit;
                    break;
                case "PotLimit":
                    d = Gametype.PotLimit;
                    break;
            }
            try
            {
                gameManager.CreateGame(gameName, username, creatorName, d, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayer, spectating);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool isGameExist(string gameName)
        {
            return gameManager.IsRoomExist(gameName);
        }

        public IList findGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag)
        {
            try
            {
                return gameManager.FindGames(username, playerName, playerFlag, potSize, potFlag,
                    gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
                    spectating, prefFlag, leagueFlag);
            }
            catch (Exception e)
            {
                return new List<string>();
            }
            
        }

        public IList findGames(string username)
        {
            try
            { 
                return gameManager.FindGames(username);
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }

        public bool joinGame(string username, string roomName, string playerName)
        {
            try
            {
                gameManager.JoinGame(username, roomName, playerName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool spectateGame(string username, string roomName, string playerName)
        {
            try
            {
                gameManager.SpectateGame(username, roomName, playerName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool leaveGame(string username, string roomName, string playerName)
        {
            try
            {
                gameManager.LeaveGame(username, roomName, playerName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public int getRank(string userName)
        {
            return userManager.GetRank(userName);
        }

        public bool raiseInGame(int raiseamount, string gamename, string playername)
        {
            try
            {
                gameManager.PlaceBet(gamename, playername, raiseamount);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool callInGame(string gamename, string playername)
        {
            try
            {
                gameManager.Call(gamename, playername);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool foldInGame(string gameName, string playerName)
        {
            try
            {
                gameManager.Fold(gameName, playerName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool setExpCriteria(string username, int exp)
        {
            try
            {
                gameManager.SetExpCriteria(username, exp);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool setDefaultRank(string username, int rank)
        {
            try
            {
                gameManager.SetDefaultRank(username, rank);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool setUserLeague(string username, string usernameToSet, int rank)
        {
            try
            {
                gameManager.SetUserLeague(username, usernameToSet, rank);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool saveTurn(string roomName, int turnNum)
        {
            try
            {
                replayManager.SaveTurn(roomName, turnNum);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool restartGameCenter()
        {
            return gameManager.restartGameCenter();
        }

        public bool startGame(string roomName)
        {
            try
            {
                gameManager.StartGame(roomName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool setBet(string roomName, string PlayerName, int bet)
        {
            try
            {
                gameManager.PlaceBet(roomName,PlayerName,bet);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void setUserRank(string legalUserName, int newrank)
        {
            userManager.ChangeRank(legalUserName, newrank);
        }
    }
}
