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
        // GET: api/Room
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Room/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Room  -------create root
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
                    if (value.creator_player_name == p.Name)
                    {
                       if(p.Hand[0]!=null) p1.player_hand[0] = p.Hand[0].ToString();
                       if (p.Hand[1] != null) p1.player_hand[1] = p.Hand[1].ToString();
                    }
                    ans.all_players[j] = p1;
                    j++;
                }
                return ans;
            }

            catch (Exception e)
            {
                ans.messege = e.Message;
                return ans;
            }

        }
        

        // PUT: api/Room/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Room/5
        public void Delete(int id)
        {
        }
    }
}
