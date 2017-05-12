using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Services
{
    public class GameManager
    {
        private readonly GameCenter gameCenter;

        public GameManager()
        {
            gameCenter = GameCenter.GetGameCenter();
        }

        public Room CreateGame(string roomName, string creatorUserName, string creatorName, Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating) // UC 5
        {
            return gameCenter.CreateRoom(roomName, creatorUserName, creatorName, gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
            spectating);
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

        public List<Room> FindGames(string username, RoomFilter r)  // UC 11 (Finds any available game)
        {
            User context = gameCenter.GetUser(username);

            List<Predicate<Room>> predicates = new List<Predicate<Room>>();

            if (context.Rank != -1 && r.LeagueOnly != null && r.LeagueOnly.Value == true)
            {
                predicates.Add(room => room.rank == context.Rank);
            }
            if (r.PlayerName != null)
            {
                predicates.Add(room => room.hasPlayer(r.PlayerName));
            }
            if (r.PotSize != null)
            {
                predicates.Add(room => room.pot == r.PotSize.Value);
            }
            if (r.GameType != null)
            {
                predicates.Add(room => room.gamePreferences.gameType.ToString() == r.GameType);
            }
            if (r.BuyInPolicy != null)
            {
                predicates.Add(room => room.gamePreferences.buyInPolicy == r.BuyInPolicy.Value);
            }
            if (r.ChipPolicy != null)
            {
                predicates.Add(room => room.gamePreferences.chipPolicy == r.ChipPolicy.Value);
            }
            if (r.MinPlayers != null)
            {
                predicates.Add(room => room.gamePreferences.minPlayers == r.MinPlayers.Value);
            }
            if (r.MaxPlayers != null)
            {
                predicates.Add(room => room.gamePreferences.maxPlayers == r.MaxPlayers.Value);
            }
            if (r.SepctatingAllowed != null)
            {
                predicates.Add(room => room.gamePreferences.spectating == r.SepctatingAllowed.Value);
            }

            return gameCenter.FindGames(predicates);
        }

        public void StartGame(string gameName) // UC 12
        {
            gameCenter.GetRoom(gameName).StartGame();
        }

        public void PlaceBet(string gameName,string player,int bet) // UC 13
        {
            gameCenter.GetRoom(gameName).SetBet(gameCenter.GetRoom(gameName).GetPlayer(player), bet, false);
        }
      
        public void Fold(string room, string userName)
        {
            gameCenter.GetRoom(room).Fold(gameCenter.GetRoom(room).GetPlayer(userName));
        }

        public void Call(string room, string userName)
        {
            gameCenter.GetRoom(room).Call(gameCenter.GetRoom(room).GetPlayer(userName));
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
