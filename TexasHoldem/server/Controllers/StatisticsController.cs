using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace server.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET: api/Statistics
        public List<string> Get()
        {
            return Server.Server.GameFacade.GetRanks();
        }

        // GET: api/Statistics?userName=elad
        public List<string> Get(string userName)
        {
            return Server.Server.GameFacade.GetStasus(userName);
        }
    }
}
