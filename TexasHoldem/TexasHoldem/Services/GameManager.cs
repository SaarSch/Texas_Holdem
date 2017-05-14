using System;
using System.Collections.Generic;
using TexasHoldem.Game;
using TexasHoldem.GamePrefrences;

namespace TexasHoldem.Services
{
    public class GameManager
    {
        private readonly GameCenter _gameCenter;

        public GameManager()
        {
            _gameCenter = GameCenter.GetGameCenter();
        }

        public void SetLeagues()
        {
            _gameCenter.SetLeagues();
        }

        public Room CreateGame(string gameName, string username, string creatorName) // UC 5
        {
            return _gameCenter.CreateRoom(gameName, username, creatorName, new GamePreferences());
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

            return _gameCenter.CreateRoom(gameName, username, creatorName, gp);
        }

        public bool IsRoomExist(string roomName)
        {
            return _gameCenter.IsRoomExist(roomName);
        }

        public Room JoinGame(string username, string roomName, string playerName) // UC 6
        {
            return _gameCenter.AddUserToRoom(username, roomName, false, playerName);
        }

        public Room SpectateGame(string username, string roomName, string playerName) // UC 7
        {
            return _gameCenter.AddUserToRoom(username, roomName, true);
        }

        public Room LeaveGame(string username, string roomName, string playerName) // UC 8
        {
            return _gameCenter.RemoveUserFromRoom(username, roomName, playerName);
        }

        public List<string> FindGames(string username, string playerName, bool playerFlag, int potSize, bool potFlag,
            string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating, bool prefFlag, bool leagueFlag) // UC 11
        {
            return _gameCenter.FindGames(username, playerName, playerFlag, potSize, potFlag,
                gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers,
                spectating, prefFlag, leagueFlag);
        }

        public List<string> FindGames(string username) // UC 11 (Finds any available game)
        {
            return _gameCenter.FindGames(username, "", false, 0, false, "NoLimit", 0, 10, 4, 3, 7, false, false,
                false);
        }

        public Room StartGame(string gameName) // UC 12
        {
           return _gameCenter.GetRoom(gameName).StartGame();
        }

        public Room PlaceBet(string gameName,string player,int bet) // UC 13
        {
            return _gameCenter.GetRoom(gameName).SetBet(_gameCenter.GetRoom(gameName).GetPlayer(player), bet, false);
        }
      
        public Room Fold(string room, string userName)
        {
            return _gameCenter.GetRoom(room).Fold(_gameCenter.GetRoom(room).GetPlayer(userName));
        }

        public Room Call(string room, string userName)
        {
            return _gameCenter.GetRoom(room).Call(_gameCenter.GetRoom(room).GetPlayer(userName));
        }

        public void SetDefaultRank(string username, int rank) // UC 14
        {
            _gameCenter.SetDefaultRank(username, rank);
        }


        public void SetExpCriteria(string username, int exp) // UC 14
        {
            _gameCenter.SetExpCriteria(username, exp);
        }

        public void SetUserLeague(string username, string usernameToSet, int rank) // UC 14
        {
            _gameCenter.SetUserRank(username, usernameToSet, rank);
        }

        public void CalcLeague()
        {
            _gameCenter.SetLeagues();
        }

        public void PlayerWisper(string room, string username_sender, string username_reciver, string message)
        {
            _gameCenter.GetRoom(room).PlayerWisper(message, _gameCenter.GetRoom(room).GetPlayer(username_sender), _gameCenter.GetUser(username_reciver));
        }

        public void SpectatorWisper(string room, string username_sender, string username_reciver, string message)
        {
            _gameCenter.GetRoom(room).SpectatorWisper(message, _gameCenter.GetUser(username_sender), _gameCenter.GetUser(username_reciver));
        }

        public void PlayerSendMessege(string room, string username_sender, string username_reciver, string message)
        {
            _gameCenter.GetRoom(room).PlayerSendMessege(message, _gameCenter.GetRoom(room).GetPlayer(username_sender));
        }

        public void SpectatorsSendMessege(string room, string username_sender, string username_reciver, string message)
        {
            _gameCenter.GetRoom(room).SpectatorsSendMessege(message, _gameCenter.GetUser(username_sender));
        }

        public bool RestartGameCenter()
        {
            try
            {
                _gameCenter.DeleteAllRooms();
                _gameCenter.DeleteAllUsers();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
