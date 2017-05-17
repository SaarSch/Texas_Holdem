using System.Collections.Generic;
using System.Web.Http;
using Server.Models;

namespace Server.Controllers
{
    public class ReplayController : ApiController
    {
        // GET: api/Replay?RoomName=="KAKI"&&Player="moshe"
        public IEnumerable<RoomState> Get(string roomName, string player)
        {
            List<RoomState> ans = null;
            if(RoomController.Replays.ContainsKey(roomName) && RoomController.Replays[roomName].ContainsKey(player))
            ans = RoomController.Replays[roomName][player];
            return ans;
        }

      
    }
}
