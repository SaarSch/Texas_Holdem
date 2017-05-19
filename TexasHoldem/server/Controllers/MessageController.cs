using System;
using System.Web.Http;
using Server.Models;
using IRoom = TexasHoldem.Game.IRoom;

namespace Server.Controllers
{
    public class MessageController : ApiController
    {

        // GET: api/Messages/?room=moshe&sender=kaki&message=message sent to all&status=player
        public RoomState Get(string room, string sender, string message, string status) 
        {
            IRoom r = null;
            var ans = new RoomState();
            try
            {
                r = status == "player" ? Server.GameFacade.PlayerSendMessage(room, sender, message) : Server.GameFacade.SpectatorsSendMessage(room, sender, message);
            }
            catch (Exception e)
            {
                ans.Messege = e.Message;
            }
            if (r != null) RoomController.CreateRoomState(sender, r, ans);
            return ans;
        }
        
        // GET: api/Messages/?room=moshe&sender=kaki&reciver=sean&message=message&status=player   wisper
        public RoomState Get(string room, string sender, string reciver, string message, string status)
        {
            IRoom r = null;
            var ans = new RoomState();
            try
            {
                r = status == "player" ? Server.GameFacade.PlayerWhisper(room, sender, reciver, message) : Server.GameFacade.SpectatorWhisper(room, sender, reciver, message);
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
