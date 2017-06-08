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
        // GET: api/Statistics
        public List<UserStat> Get()
        {
            List<UserStat> ans =new List<UserStat>();
           // return Server.Server.GameFacade.GetRanks();
            return ans;
        }

        // GET: api/Statistics?userName=elad
        public UserStat Get(string userName)
        {
            //return Server.Server.GameFacade.GetStat(userName);
            UserStat ans=new UserStat();

            return ans;
        }

        // GET: api/Statistics?userName=elad&password=12345678
        public UserStat Get(string userName,string password)
        {
            UserStat ans = new UserStat();

            return ans;
        }

    }
}
