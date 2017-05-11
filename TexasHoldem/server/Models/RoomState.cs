using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class RoomState
    {
        public string room_name;
        public bool is_on;
        public int pot;
        public string game_status;
        public string[] community_cards=new string[5];
        public Models.Player[] all_players=new Models.Player[5];
        public int min_players;
        public int max_players;
        public bool sepctating_allowed;
        public string messege;
    }
}