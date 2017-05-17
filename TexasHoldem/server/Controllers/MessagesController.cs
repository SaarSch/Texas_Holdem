﻿using server.Models;
using System;
using System.Web.Http;

namespace server.Controllers
{
    public class MessagesController : ApiController
    {

        // GET: api/Messages/?room=moshe&sender=kaki&message=message sent to all&status=player
        public RoomState Get(string room, string sender, string message, string status) 
        {
            TexasHoldem.Game.Room r = null;
            var ans = new RoomState();
            try
            {
                if (status == "player")
                {
                    r = WebApiConfig.GameManger.PlayerSendMessege(room, sender, message);
                }
                else r = WebApiConfig.GameManger.SpectatorsSendMessege(room, sender, message);
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
            TexasHoldem.Game.Room r = null;
            var ans = new RoomState();
            try
            {
                if (status == "player")
                {
                    r = WebApiConfig.GameManger.PlayerWisper(room, sender, reciver, message);
                }
                else r = WebApiConfig.GameManger.SpectatorWisper(room, sender, reciver, message);
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
