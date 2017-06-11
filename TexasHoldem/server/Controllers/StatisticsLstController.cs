using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using server.Models;

namespace server.Controllers
{
    public class StatisticsLstController : ApiController
    {
        // GET: api/StatisticsLst?kind=1
        //kind can be: 1|2|3
        //1=The total gross profit.
       //2=The highest cash gain in a game.
       //3=The number of games played.
        public List<UserStat> Get(int kind)
        {
            if (kind < 1 || kind > 3)
                return null;
            var lst = Server.Server.GameFacade.GetTopStat(kind);
            return lst.Select(u => new UserStat
                {
                    Username = u.Username,
                    AvgCashGain = u.AvgCashGain,
                    AvgGrossProfit = u.AvgGrossProfit,
                    GrossProfit = u.GrossProfit,
                    HighestCashGain = u.HighestCashGain,
                    NumOfGames = u.NumOfGames,
                    AvatarPath = u.AvatarPath
                })
                .ToList();
        }
    }
}
