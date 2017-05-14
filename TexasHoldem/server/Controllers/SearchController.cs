using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using server.Models;

namespace server.Controllers
{
    public class SearchController : ApiController
    {
        public RoomList Post([FromBody] RoomFilter value)
        {
            List<Room> rooms;
            RoomList ret = new RoomList();
            try
            {
                rooms = WebApiConfig.gameManger.FindGames(value.user, new TexasHoldem.Services.RoomFilter(
                    value.player_name, value.pot_size, value.league_only, value.game_type, value.buy_in_policy,
                    value.chip_policy, value.min_bet, value.min_players, value.max_players,
                    value.sepctating_allowed));
                ret.rooms = new server.Models.Room[rooms.Count];
                for (int i = 0; i < rooms.Count; i++)
                {
                    ret.rooms[i] = new server.Models.Room();
                    ret.rooms[i].room_name = rooms.ElementAt(i).name;
                    ret.rooms[i].game_type = rooms.ElementAt(i).gamePreferences.gameType.ToString();
                    ret.rooms[i].rank = rooms.ElementAt(i).rank;
                    ret.rooms[i].buy_in_policy = rooms.ElementAt(i).gamePreferences.buyInPolicy;
                    ret.rooms[i].min_bet = rooms.ElementAt(i).gamePreferences.minBet;
                    ret.rooms[i].min_players = rooms.ElementAt(i).gamePreferences.minPlayers;
                    ret.rooms[i].max_players = rooms.ElementAt(i).gamePreferences.maxPlayers;
                    ret.rooms[i].sepctating_allowed = rooms.ElementAt(i).gamePreferences.spectating;
                }
            }
            catch (Exception e)
            {
                ret.message = e.Message;
            }

            return ret;
        }
    }
}
