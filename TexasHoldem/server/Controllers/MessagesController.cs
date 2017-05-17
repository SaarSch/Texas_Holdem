using server.Models;
using System;
using System.Web.Http;

namespace server.Controllers
{
    public class MessagesController : ApiController
    {

        // GET: api/Messages/?room=moshe&sender=kaki&message=message sent to all
        public RoomState Get(string room, string sender, string message) 
        {
            TexasHoldem.Game.Room r = null;
            var ans = new RoomState();
            try
            {
                r = WebApiConfig.GameManger.PlayerSendMessege(room, sender, message);
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if (r != null) RoomController.CreateRoomState(sender, r, ans);
            return ans;
        }
        
        // GET: api/Messages/?room=moshe&sender=kaki&reciver=sean&message=message   wisper
        public RoomState Get(string room, string sender, string reciver, string message)
        {
            TexasHoldem.Game.Room r = null;
            var ans = new RoomState();
            try
            {
                r = WebApiConfig.GameManger.PlayerWisper(room, sender, reciver ,message);
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if (r != null) RoomController.CreateRoomState(sender, r, ans);
            return ans;
        }

    }
}
