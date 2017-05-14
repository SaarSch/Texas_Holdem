using server.Models;
using System;
using System.Web.Http;
using Room = TexasHoldem.Game.Room;

namespace server.Controllers
{
    public class RoomController : ApiController
    {
        // GET: /api/Room?game_name=moshe&player_name=kaki
        public RoomState GET(String gameName, String playerName) //start game
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
            Room r = null;
            var ans = new RoomState();
            try
            {
                if (option == "join")
                {
                    r = WebApiConfig.GameManger.JoinGame(userName, gameName, playerName);
                }
                else if(option == "spectate")
                {
                    r = WebApiConfig.GameManger.SpectateGame(userName, gameName, playerName);    
                }
                else if (option == "leave")
                {
                    r = WebApiConfig.GameManger.LeaveGame(userName, gameName, playerName);
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
            Room r = null;
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
            Room r = null;
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
                Room r = WebApiConfig.GameManger.CreateGameWithPreferences(value.RoomName, value.CreatorUserName, value.CreatorPlayerName, value.GameType, value.BuyInPolicy, value.ChipPolicy, value.MinBet, value.MinPlayers, value.MaxPlayers, value.SepctatingAllowed);
                if (r != null) CreateRoomState(value.CreatorPlayerName, r, ans);
                return ans;
            }

            catch (Exception e)
            {
                ans.Messege = e.Message;
                return ans;
            }

        }

        private void CreateRoomState(string player, Room r, RoomState ans)
        {
            try
            {
                ans.RoomName = r.Name;
                ans.IsOn = r.IsOn;
                ans.Pot = r.Pot;
                ans.GameStatus = r.GameStatus.ToString();
                ans.CommunityCards = new string[5];
                ans.AllPlayers = new Models.Player[r.Players.Count];
                for (var i = 0; i < 5; i++)
                {
                    if (r.CommunityCards[i] == null) break;
                    ans.CommunityCards[i] = r.CommunityCards[i].ToString();
                }
                var j = 0;
                foreach (var p in r.Players)
                {
                    var p1 = new Models.Player
                    {
                        PlayerName = p.Name,
                        CurrentBet = p.CurrentBet,
                        ChipsAmount = p.ChipsAmount,
                        Avatar = p.User.GetAvatar(),
                        PlayerHand = new string[2]
                    };
                    if (player == p.Name)
                    {
                        if (p.Hand[0] != null) p1.PlayerHand[0] = p.Hand[0].ToString();
                        if (p.Hand[1] != null) p1.PlayerHand[1] = p.Hand[1].ToString();
                    }
                    ans.AllPlayers[j] = p1;
                    j++;
                }
            }

            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
        }


    }
}
