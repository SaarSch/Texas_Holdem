using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class RoomController : ApiController
    {
        // GET: /api/Room?game_name=moshe&player_name=kaki&option=call 
        public RoomState GET(string game_name, string player_name, string option)
        {
            Room r = null;
            RoomState ans = new RoomState();
            try
            {
                if (option == "fold")
                {
                    r = WebApiConfig.gameManger.Fold(game_name, player_name);

                }
                else if (option == "call")
                {
                    r = WebApiConfig.gameManger.Call(game_name, player_name);
                }
                CreateRoomState(player_name, r, ans);
                return ans;
            }

            catch (Exception e)
            {
                ans.messege = e.Message;
            }
            return ans;

        }

        // POST: api/Room  -------create room
        public RoomState Post([FromBody]Models.Room value)
        {
            RoomState ans = new RoomState();
            try
            {
                Gametype gp = (Gametype)0;
                if (value.game_type == Gametype.limit.ToString()) gp = Gametype.limit;
                else if (value.game_type == Gametype.PotLimit.ToString()) gp = Gametype.PotLimit;
                else if (value.game_type == Gametype.NoLimit.ToString()) gp = Gametype.NoLimit;
                Room r = WebApiConfig.gameManger.CreateGame(value.room_name, value.creator_user_name, value.creator_player_name, gp, value.buy_in_policy, value.chip_policy, value.min_bet, value.min_players, value.max_players, value.sepctating_allowed);
                CreateRoomState(value.creator_player_name, r, ans);
                return ans;
            }

            catch (Exception e)
            {
                ans.messege = e.Message;
                return ans;
            }

        }

        private void CreateRoomState(string player, Room r, RoomState ans)
        {
            try
            {
                ans.room_name = r.name;
                ans.is_on = r.IsOn;
                ans.pot = r.pot;
                ans.game_status = r.gameStatus.ToString();
                ans.community_cards = new string[5];
                ans.all_players = new Models.Player[r.players.Count];
                for (int i = 0; i < 5; i++)
                {
                    if (r.communityCards[i] == null) break;
                    ans.community_cards[i] = r.communityCards[i].ToString();
                }
                int j = 0;
                foreach (Player p in r.players)
                {
                    Models.Player p1 = new Models.Player();
                    p1.player_name = p.Name;
                    p1.current_bet = p.CurrentBet;
                    p1.chips_amount = p.ChipsAmount;
                    p1.avatar = p.User.GetAvatar();
                    p1.player_hand = new string[2];
                    if (player == p.Name)
                    {
                        if (p.Hand[0] != null) p1.player_hand[0] = p.Hand[0].ToString();
                        if (p.Hand[1] != null) p1.player_hand[1] = p.Hand[1].ToString();
                    }
                    ans.all_players[j] = p1;
                    j++;
                }
            }

            catch (Exception e)
            {
                ans.messege = e.Message;
            }
        }


    }
}
