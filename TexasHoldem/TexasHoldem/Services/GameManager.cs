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

        public Room CreateGame(string roomName, string creatorUserName, string creatorName) // UC 5
        {
            return gameCenter.CreateRoom(roomName, creatorUserName, creatorName);
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

        public void SaveGame() // UC 11
        {
            
        }

        public List<Room> ListActiveGames(int rank) // UC 12
        {
            return null;
        }

        public List<Room> ListActiveGames() // UC 12
        {
            return null; // -1
        }

        public void PlayGame() // UC 13
        {
            
        }

        public void PlaceBets() // UC 14
        {
            
        }
    }
}
