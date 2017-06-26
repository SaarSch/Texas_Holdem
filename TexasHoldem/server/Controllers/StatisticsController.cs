using System.Web.Http;
using server.Models;

namespace server.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET: api/Statistics?userName=elad
        public UserStat Get(string userName)
        {
            UserStat ans;
            var u = Server.Server.GameFacade.GetStat(userName);
            if (u == null)
                ans = new UserStat
                {
                    Username = "",
                    AvgCashGain = -1,
                    AvgGrossProfit = -1,
                    GrossProfit = -1,
                    HighestCashGain = -1,
                    NumOfGames = -1,
                    AvatarPath = ""
                };
            else
                ans = new UserStat
                {
                    Username = u.Username,
                    AvgCashGain = u.AvgCashGain,
                    AvgGrossProfit = u.AvgGrossProfit,
                    GrossProfit = u.GrossProfit,
                    HighestCashGain = u.HighestCashGain,
                    NumOfGames = u.NumOfGames,
                    AvatarPath = u.AvatarPath
                };

            return ans;
        }

        //login GET: api/Statistics?userName=elad&password=12345678
        public UserStat Get(string userName, string password)
        {
            UserStat ans;
            var u = Server.Server.GameFacade.WebLogin(userName, password);
            if (u == null)
                ans = new UserStat
                {
                    Username = "",
                    AvgCashGain = -1,
                    AvgGrossProfit = -1,
                    GrossProfit = -1,
                    HighestCashGain = -1,
                    NumOfGames = -1,
                    AvatarPath = ""
                };
            else
                ans = new UserStat
                {
                    Username = u.Username,
                    AvgCashGain = u.AvgCashGain,
                    AvgGrossProfit = u.AvgGrossProfit,
                    GrossProfit = u.GrossProfit,
                    HighestCashGain = u.HighestCashGain,
                    NumOfGames = u.NumOfGames,
                    AvatarPath = u.AvatarPath
                };
            return ans;
        }
    }
}