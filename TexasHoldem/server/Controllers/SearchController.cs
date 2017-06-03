using System;
using System.Linq;
using System.Web.Http;
using Server.Models;
using server;

namespace Server.Controllers
{
    public class SearchController : ApiController
        {
            public RoomList Post([FromBody] RoomFilter value,string token)
        {
            var ret = new RoomList();
            try
            {
                Server.CheckToken(token);
                var rooms = Server.GameFacade.FindGames(Crypto.Decrypt(value.User), new TexasHoldem.Game.RoomFilter(
                    value.PlayerName, value.PotSize, value.LeagueOnly, value.GameType, value.BuyInPolicy,
                    value.ChipPolicy, value.MinBet, value.MinPlayers, value.MaxPlayers,
                    value.SpectatingAllowed));
                ret.Rooms = new Room[rooms.Count];
                for (var i = 0; i<rooms.Count; i++)
                {
                    ret.Rooms[i] = new Room
                    {
                        RoomName = rooms.ElementAt(i).Name,
                        GameType = rooms.ElementAt(i).GamePreferences.GameType.ToString(),
                        League = rooms.ElementAt(i).League,
                        BuyInPolicy = rooms.ElementAt(i).GamePreferences.BuyInPolicy,
                        MinBet = rooms.ElementAt(i).GamePreferences.MinBet,
                        MinPlayers = rooms.ElementAt(i).GamePreferences.MinPlayers,
                        MaxPlayers = rooms.ElementAt(i).GamePreferences.MaxPlayers,
                        SpectatingAllowed = rooms.ElementAt(i).GamePreferences.Spectating,
                        ChipPolicy = rooms.ElementAt(i).GamePreferences.ChipPolicy
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
