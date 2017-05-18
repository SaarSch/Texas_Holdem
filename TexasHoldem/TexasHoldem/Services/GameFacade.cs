using System;
using System.Collections.Generic;
using TexasHoldem.Game;
using TexasHoldem.Logics;

namespace TexasHoldem.Services
{
    public class GameFacade
    {
        private readonly GameCenter _gameCenter;
        private readonly MessageLogic _messageLogic;
        private readonly UserLogic _userLogic;

        public GameFacade()
        {
            _gameCenter = GameCenter.GetGameCenter();
            _messageLogic = new MessageLogic();
            _userLogic = new UserLogic();
        }

        public Room RoomStatus(string roomName)
        {
            return _gameCenter.GetRoom(roomName);
        }

        public void SetLeagues()
        {
            _userLogic.SetLeagues(_gameCenter.Users);
        }

        public Room CreateGame(string gameName, string username, string creatorName) // UC 5
        {
            return _gameCenter.CreateRoom(gameName, username, creatorName, new GamePreferences());
        }

        public Room CreateGameWithPreferences(string gameName, string username, string creatorName, string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers, bool spectating)
        {
            var gp = new GamePreferences();
            gp.GameType = (Gametype) Enum.Parse(typeof(Gametype),gameType);
            gp.BuyInPolicy = buyInPolicy;
            gp.ChipPolicy = chipPolicy;
            gp.MinBet = minBet;
            gp.MinPlayers = minPlayers;
            gp.MaxPlayers = maxPlayers;
            gp.Spectating = spectating;

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

        public List<Room> FindGames(string username, RoomFilter r)  // UC 11 (Finds any available game)
        {
            var context = _userLogic.GetUser(username, _gameCenter.Users);

            var predicates = new List<Predicate<Room>>();

            if (context.League != -1 && r.LeagueOnly != null && r.LeagueOnly.Value)
            {
                predicates.Add(room => room.League == context.League);
            }
            if (r.PlayerName != null)
            {
                predicates.Add(room => room.HasPlayer(r.PlayerName));
            }
            if (r.PotSize != null)
            {
                predicates.Add(room => room.Pot == r.PotSize.Value);
            }
            if (r.GameType != null)
            {
                predicates.Add(room => room.GamePreferences.GameType.ToString() == r.GameType);
            }
            if (r.BuyInPolicy != null)
            {
                predicates.Add(room => room.GamePreferences.BuyInPolicy == r.BuyInPolicy.Value);
            }
            if (r.ChipPolicy != null)
            {
                predicates.Add(room => room.GamePreferences.ChipPolicy == r.ChipPolicy.Value);
            }
            if (r.MinBet != null)
            {
                predicates.Add(room => room.GamePreferences.MinBet == r.MinBet.Value);
            }
            if (r.MinPlayers != null)
            {
                predicates.Add(room => room.GamePreferences.MinPlayers == r.MinPlayers.Value);
            }
            if (r.MaxPlayers != null)
            {
                predicates.Add(room => room.GamePreferences.MaxPlayers == r.MaxPlayers.Value);
            }
            if (r.SepctatingAllowed != null)
            {
                predicates.Add(room => room.GamePreferences.Spectating == r.SepctatingAllowed.Value);
            }

            return _gameCenter.FindGames(predicates);
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

        public void CalcLeague()
        {
            _userLogic.SetLeagues(_gameCenter.Users);
        }

        public Room PlayerWhisper(string room, string playernameSender, string usernameReceiver, string message)
        {
            return _messageLogic.PlayerWhisper(message, _gameCenter.GetRoom(room).GetPlayer(playernameSender), _userLogic.GetUser(usernameReceiver, _gameCenter.Users), _gameCenter.GetRoom(room));
        }

        public Room SpectatorWhisper(string room, string usernameSender, string usernameReceiver, string message)
        {
            return _messageLogic.SpectatorWhisper(message, _userLogic.GetUser(usernameSender, _gameCenter.Users), _userLogic.GetUser(usernameReceiver, _gameCenter.Users), _gameCenter.GetRoom(room));
        }

        public Room PlayerSendMessage(string room, string playerNameSender, string message)
        {
            return _messageLogic.PlayerSendMessage(message, _gameCenter.GetRoom(room).GetPlayer(playerNameSender),_gameCenter.GetRoom(room));
        }

        public Room SpectatorsSendMessage(string room, string usernameSender, string message)
        {
            return _messageLogic.SpectatorsSendMessage(message, _userLogic.GetUser(usernameSender, _gameCenter.Users), _gameCenter.GetRoom(room));
        }

        public bool RestartGameCenter()
        {
            try
            {
                _gameCenter.DeleteAllRooms();
                _userLogic.DeleteAllUsers(_gameCenter.Users);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
