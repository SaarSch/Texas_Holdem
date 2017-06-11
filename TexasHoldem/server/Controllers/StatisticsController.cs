using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using server.Models;


namespace server.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET: api/Statistics?userName=elad
        public UserStat Get(string userName)
        {   
            var u= Server.Server.GameFacade.GetStat(userName);
            var ans = new UserStat
            {
                Username = u.Username,
                AvgCashGain = u.AvgCashGain,
                AvgGrossProfit = u.AvgGrossProfit,
                GrossProfit = u.GrossProfit,
                HighestCashGain = u.HighestCashGain,
                NumOfGames = u.NumOfGames,
                AvatarPath=u.AvatarPath
            };

            return ans;
        }

        //login GET: api/Statistics?userName=elad&password=12345678
        public UserStat Get(string userName,string password)
        {
            var u = Server.Server.GameFacade.WebLogin(userName, password);
            var ans = new UserStat
                {
                    Username = u.Username,
                    AvgCashGain = u.AvgCashGain,
                    AvgGrossProfit = u.AvgGrossProfit,
                    GrossProfit = u.GrossProfit,
                    HighestCashGain = u.HighestCashGain,
                    NumOfGames = u.NumOfGames,
                    AvatarPath=u.AvatarPath
            };
            return ans;
        }

    }
}
