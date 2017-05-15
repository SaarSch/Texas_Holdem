using server.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace server.Controllers
{
    public class SearchController : ApiController
        {
            public RoomList Post([FromBody] RoomFilter value)
        {
            var ret = new RoomList();
            try
            {
                var rooms = WebApiConfig.GameManger.FindGames(value.User, new TexasHoldem.Services.RoomFilter(
                    value.PlayerName, value.PotSize, value.LeagueOnly, value.GameType, value.BuyInPolicy,
                    value.ChipPolicy, value.MinBet, value.MinPlayers, value.MaxPlayers,
                    value.SpectatingAllowed));
                ret.Rooms = new Room[rooms.Count];
                for (var i = 0; i<rooms.Count; i++)
                {
                    ret.Rooms[i] = new Room
                    {
                        RoomName = rooms.ElementAt(i).Name,
                        GameType = rooms.ElementAt(i).GamePreferences.GetGameType().ToString(),
                        League = rooms.ElementAt(i).League,
                        BuyInPolicy = rooms.ElementAt(i).GamePreferences.GetBuyInPolicy(),
                        MinBet = rooms.ElementAt(i).GamePreferences.GetMinBet(),
                        MinPlayers = rooms.ElementAt(i).GamePreferences.GetMinPlayers(),
                        MaxPlayers = rooms.ElementAt(i).GamePreferences.GetMaxPlayers(),
                        SepctatingAllowed = rooms.ElementAt(i).GamePreferences.GetSpectating()
                    };
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
