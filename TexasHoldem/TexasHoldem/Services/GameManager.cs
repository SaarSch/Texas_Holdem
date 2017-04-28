using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Services
{
    class GameManager
    {
        private readonly GameCenter gameCenter;

        public GameManager()
        {
            gameCenter = GameCenter.GetGameCenter();
        }

        public Room CreateGame(string roomName, string creatorUserName, string creatorName, GamePreferences gp) // UC 5
        {
            return gameCenter.CreateRoom(roomName, creatorUserName, creatorName, gp);
        }

        public bool IsRoomExist(string roomName)
        {
            return gameCenter.IsRoomExist(roomName);
        }

        public bool JoinGame(string username, string roomName, string playerName) // UC 6
        {
            try
            {
                gameCenter.AddUserToRoom(username, roomName, playerName, false);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool SpectateGame(string username, string roomName, string playerName) // UC 7
        {
            try
            {
                gameCenter.AddUserToRoom(username, roomName, playerName, true);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool LeaveGame(string username, string roomName, string playerName) // UC 8
        {
            try
            {
                gameCenter.RemoveUserFromRoom(username, roomName, playerName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public List<string> FindGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag) // UC 11
        {
            try
            {
                return gameCenter.FindGames(username, playerName, playerFlag, potSize, potFlag,
                    gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
                    spectating, prefFlag, leagueFlag);
            }
            catch (Exception e)
            {
                return new List<string>(); // no rooms found
            }
        }

        public List<string> FindGames(string username) // UC 11
        {
            try
            {
                return gameCenter.FindGames(username, "", false, 0, false, Gametype.NoLimit, 0, 10, 4, 3, 7, false, false,
                    false);
            }
            catch (Exception e)
            {
                return new List<string>(); // no rooms found
            }
            
        }

        public void PlayGame() // UC 12
        {
            
        }

        public void PlaceBets() // UC 13
        {
            
        }

        public bool restartGameCenter()
        {
            try
            {
                gameCenter.DeleteAllRooms();
                gameCenter.DeleteAllUsers();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
