using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.GamePrefrences;

namespace TexasHoldem.Services
{
    public class GameManager
    {
        private readonly GameCenter gameCenter;

        public GameManager()
        {
            gameCenter = GameCenter.GetGameCenter();
        }

        public Room CreateGame(string gameName, string username, string creatorName) // UC 5
        {
            return gameCenter.CreateRoom(gameName, username, creatorName, new GamePreferences());
        }

        public Room CreateGameWithPreferences(string gameName, string username, string creatorName, string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers, bool spectating)
        {
            IPreferences gp = new GamePreferences();
            gp = new ModifiedGameType((Gametype)Enum.Parse(typeof(Gametype), gameType), gp);
            gp = new ModifiedBuyInPolicy(buyInPolicy, gp);
            gp = new ModifiedChipPolicy(chipPolicy, gp);
            gp = new ModifiedMinBet(minBet, gp);
            gp = new ModifiedMinPlayers(minPlayers, gp);
            gp = new ModifiedMaxPlayers(maxPlayers, gp);
            gp = new ModifiedSpectating(spectating, gp);

            return gameCenter.CreateRoom(gameName, username, creatorName, gp);
        }

        public bool IsRoomExist(string roomName)
        {
            return gameCenter.IsRoomExist(roomName);
        }

        public void JoinGame(string username, string roomName, string playerName) // UC 6
        {
            gameCenter.AddUserToRoom(username, roomName, false, playerName);
        }

        public void SpectateGame(string username, string roomName, string playerName) // UC 7
        {
            gameCenter.AddUserToRoom(username, roomName, true);
        }

        public void LeaveGame(string username, string roomName, string playerName) // UC 8
        {
            gameCenter.RemoveUserFromRoom(username, roomName, playerName);
        }

        public List<string> FindGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag) // UC 11
        {
            return gameCenter.FindGames(username, playerName, playerFlag, potSize, potFlag,
                gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
                spectating, prefFlag, leagueFlag);
        }

        public List<string> FindGames(string username) // UC 11 (Finds any available game)
        {
            return gameCenter.FindGames(username, "", false, 0, false, "NoLimit", 0, 10, 4, 3, 7, false, false,
                false);
        }

        public Room StartGame(string gameName) // UC 12
        {
           return gameCenter.GetRoom(gameName).StartGame();
        }

        public Room PlaceBet(string gameName,string player,int bet) // UC 13
        {
            return gameCenter.GetRoom(gameName).SetBet(gameCenter.GetRoom(gameName).GetPlayer(player), bet, false);
        }
      
        public Room Fold(string room, string userName)
        {
            return gameCenter.GetRoom(room).Fold(gameCenter.GetRoom(room).GetPlayer(userName));
        }

        public Room Call(string room, string userName)
        {
            return gameCenter.GetRoom(room).Call(gameCenter.GetRoom(room).GetPlayer(userName));
        }

        public void SetDefaultRank(string username, int rank) // UC 14
        {
            gameCenter.SetDefaultRank(username, rank);
        }
      
        public void SetExpCriteria(string username, int exp) // UC 14
        {
            gameCenter.SetExpCriteria(username, exp);
        }

        public void SetUserLeague(string username, string usernameToSet, int rank) // UC 14
        {
            gameCenter.SetUserRank(username, usernameToSet, rank);
        }

        public bool RestartGameCenter()
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
