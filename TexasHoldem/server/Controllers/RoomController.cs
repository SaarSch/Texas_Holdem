using server.Models;
using System;
using System.Web.Http;
using TexasHoldem.Game;
using Player = server.Models.Player;
using Room = TexasHoldem.Game.Room;

namespace server.Controllers
{
    public class RoomController : ApiController
    {
        // Put: /api/Room?game_name=moshe&player_name=kaki
        public RoomState Put(string gameName, string playerName) //get current status
        {
            IRoom r = null;
            var ans = new RoomState();
            try
            {
                r = WebApiConfig.GameManger.RoomStatus(gameName);
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if (r != null) CreateRoomState(playerName, r, ans);
            return ans;
        }


        // GET: /api/Room?game_name=moshe&player_name=kaki
        public RoomState GET(string gameName, string playerName) //start game
        {
            Room r = null;
            var ans = new RoomState();
            try
            {
                r = WebApiConfig.GameManger.StartGame(gameName);
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if (r != null) CreateRoomState(playerName, r, ans);
            return ans;
        }

        // GET: /api/Room?user_name=sean&game_name=moshe&player_name=kaki&option=join
        public RoomState GET(string userName ,string gameName, string playerName, string option)// join/spectate game// leave game
        {
            IRoom r = null;
            var ans = new RoomState();
            try
            {
                switch (option)
                {
                    case "join":
                        r = WebApiConfig.GameManger.JoinGame(userName, gameName, playerName);
                        break;
                    case "spectate":
                        r = WebApiConfig.GameManger.SpectateGame(userName, gameName, playerName);
                        break;
                    case "leave":
                        r = WebApiConfig.GameManger.LeaveGame(userName, gameName, playerName);
                        break;
                }
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if (r != null) CreateRoomState(playerName, r, ans);
            return ans;
        }


        // GET: /api/Room?game_name=moshe&player_name=kaki&bet=100
        public RoomState GET(string gameName, string playerName, int bet) //palce bet
        {
            IRoom r = null;
            var ans = new RoomState();
            try
            {
                r = WebApiConfig.GameManger.PlaceBet(gameName, playerName, bet);
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if(r!=null) CreateRoomState(playerName, r, ans);
            return ans;
        }


        // GET: /api/Room?game_name=moshe&player_name=kaki&option=call 
        public RoomState GET(string gameName, string playerName, string option) //call / fold 
        {
            IRoom r = null;
            var ans = new RoomState();
            try
            {
                switch (option)
                {
                    case "fold":
                        r = WebApiConfig.GameManger.Fold(gameName, playerName);
                        break;
                    case "call":
                        r = WebApiConfig.GameManger.Call(gameName, playerName);
                        break;
                }
                if (r != null) CreateRoomState(playerName, r, ans);
                return ans;
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            return ans;
        }

        // POST: api/Room  -------create room
        public RoomState Post([FromBody]Models.Room value)
        {
            var ans = new RoomState();
            try
            {
                IRoom r = WebApiConfig.GameManger.CreateGameWithPreferences(value.RoomName, value.CreatorUserName, value.CreatorPlayerName, value.GameType, value.BuyInPolicy, value.ChipPolicy, value.MinBet, value.MinPlayers, value.MaxPlayers, value.SepctatingAllowed);
                if (r != null) CreateRoomState(value.CreatorPlayerName, r, ans);
                return ans;
            }

            catch (Exception e)
            {
                ans.Messege = e.Message;
                return ans;
            }

        }

        public static void CreateRoomState(string player, IRoom r, RoomState ans)
        {
            try
            {
                var spectator = false;
                foreach (var u in r.SpectateUsers)
                {
                    if (u.GetUsername() == player) spectator = true;
                }

                ans.RoomName = r.Name;
                ans.IsOn = r.IsOn;
                ans.Pot = r.Pot;
                ans.GameStatus = r.GameStatus.ToString();
                ans.CommunityCards = new string[5];
                ans.AllPlayers = new Player[r.Players.Count];
                for (var i = 0; i < 5; i++)
                {
                    if (r.CommunityCards[i] == null) break;
                    ans.CommunityCards[i] = r.CommunityCards[i].ToString();
                }
                var j = 0;
                foreach (var p in r.Players)
                {
                    var p1 = new Player
                    {
                        PlayerName = p.Name,
                        CurrentBet = p.CurrentBet,
                        ChipsAmount = p.ChipsAmount,
                        Avatar = p.User.GetAvatar(),
                        PlayerHand = new string[2]
                    };
                    if (player == p.Name&&r.IsOn)
                    {
                        if (p.Hand[0] != null) p1.PlayerHand[0] = p.Hand[0].ToString();
                        if (p.Hand[1] != null) p1.PlayerHand[1] = p.Hand[1].ToString();
                        foreach (var pa in p.User.Notifications)
                        {
                            if (pa.Item1 == r.Name)
                            {
                                p1.messages.Add(pa.Item2);
                            }
                        }
                    }
                    else if(!r.IsOn)
                    {
                        if (p.Hand[0] != null) p1.PlayerHand[0] = p.Hand[0].ToString();
                        if (p.Hand[1] != null) p1.PlayerHand[1] = p.Hand[1].ToString();
                        if(player == p.Name)
                        {
                            foreach (var pa in p.User.Notifications)
                            {
                                if (pa.Item1 == r.Name)
                                {
                                    p1.messages.Add(pa.Item2);
                                }
                            }
                        }
                    }
                    ans.AllPlayers[j] = p1;
                    j++;
                }   
                
                ans.spectators = new UserData[r.SpectateUsers.Count];
                var u1 = new UserData();
                foreach(var u in r.SpectateUsers)
                {
                    u1.Username = u.GetUsername();
                    if (spectator&& player==u.GetUsername())
                    {     
                        foreach (var pa in u.Notifications)
                        {
                            if (pa.Item1 == r.Name)
                            {
                                u1.messages.Add(pa.Item2);
                            }
                        }
                    }
                }

            }


            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
        }


    }
}
