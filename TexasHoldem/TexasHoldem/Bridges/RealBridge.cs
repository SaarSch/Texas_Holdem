using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.GamePrefrences;
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

        public bool Register(string userName, string pass)
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

        public bool IsUserExist(string userName)
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

        public bool DeleteUser(string username, string password)
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

        public bool Login(string userName, string pass)
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

        public bool IsLoggedIn(string userName, string pass)
        {
            return userManager.IsUserLoggedIn(userName, pass);
        }

        public bool LogOut(string userName)
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

        public bool EditUsername(string username, string newUsername)
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

        public bool EditPassword(string username, string newPass)
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

        public bool EditAvatar(string username, string newPath)
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

        public bool CreateNewGame(string gameName, string username, string creatorName)
        {
            try
            {
                gameManager.CreateGame(gameName, username, creatorName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool CreateNewGameWithPrefrences(string gameName, string username, string creatorName, string gameType, int buyInPolicy,
            int chipPolicy, int minBet, int minPlayers, int maxPlayer, bool spectating)
        {
            try
            {
                gameManager.CreateGameWithPreferences(gameName, username, creatorName, gameType, buyInPolicy,
                    chipPolicy, minBet, minPlayers, maxPlayer, spectating);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool IsGameExist(string gameName)
        {
            return gameManager.IsRoomExist(gameName);
        }

        public IList findGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
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

        public bool JoinGame(string username, string roomName, string playerName)
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

        public bool SpectateGame(string username, string roomName, string playerName)
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

        public bool LeaveGame(string username, string roomName, string playerName)
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

        public int GetRank(string userName)
        {
            return userManager.GetRank(userName);
        }

        public bool RaiseInGame(int raiseamount, string gamename, string playername)
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

        public bool CallInGame(string gamename, string playername)
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

        public bool FoldInGame(string gameName, string playerName)
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

        public bool SetExpCriteria(string username, int exp)
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

        public bool SetDefaultRank(string username, int rank)
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

        public bool SetUserLeague(string username, string usernameToSet, int rank)
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

        public bool SaveTurn(string roomName, int turnNum)
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

        public bool RestartGameCenter()
        {
            return gameManager.RestartGameCenter();
        }

        public bool StartGame(string roomName)
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

        public bool SetBet(string roomName, string PlayerName, int bet)
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

        public void SetUserRank(string legalUserName, int newrank)
        {
            userManager.ChangeRank(legalUserName, newrank);
        }
    }
}
