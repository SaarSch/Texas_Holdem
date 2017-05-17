using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class ReplayController : ApiController
    {
        // GET: api/Replay
        public IEnumerable<RoomState> Get(string RoomName, string Player)
        {
            List<RoomState> ans = null;
            if(RoomController.Replays.ContainsKey(RoomName) && RoomController.Replays[RoomName].ContainsKey(Player))
            ans = RoomController.Replays[RoomName][Player];
            return ans;
        }

      
    }
}
