using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TexasHoldem.Game;

namespace server.Controllers
{
    public class SearchController : ApiController
        {
            public RoomList Post([FromBody] RoomFilter value)
        {
            List<TexasHoldem.Game.Room> rooms;
            RoomList ret = new RoomList();
            try
            {
                rooms = WebApiConfig.GameManger.FindGames(value.User, new TexasHoldem.Services.RoomFilter(
                    value.PlayerName, value.PotSize, value.LeagueOnly, value.GameType, value.BuyInPolicy,
                    value.ChipPolicy, value.MinBet, value.MinPlayers, value.MaxPlayers,
                    value.SpectatingAllowed));
                ret.Rooms = new server.Models.Room[rooms.Count];
                for (int i = 0; i<rooms.Count; i++)
                {
                    ret.Rooms[i] = new server.Models.Room();
                    ret.Rooms[i].RoomName = rooms.ElementAt(i).Name;
                    ret.Rooms[i].GameType = rooms.ElementAt(i).GamePreferences.GetGameType().ToString();
                    ret.Rooms[i].League = rooms.ElementAt(i).League;
                    ret.Rooms[i].BuyInPolicy = rooms.ElementAt(i).GamePreferences.GetBuyInPolicy();
                    ret.Rooms[i].MinBet = rooms.ElementAt(i).GamePreferences.GetMinBet();
                    ret.Rooms[i].MinPlayers = rooms.ElementAt(i).GamePreferences.GetMinPlayers();
                    ret.Rooms[i].MaxPlayers = rooms.ElementAt(i).GamePreferences.GetMaxPlayers();
                    ret.Rooms[i].SepctatingAllowed = rooms.ElementAt(i).GamePreferences.GetSpectating();
                }
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }
    }
}
