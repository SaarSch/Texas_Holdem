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
        public string[] community_cards;
        public Player[] all_players;
        public int min_players;
        public int max_players;
        public bool sepctating_allowed;
        public string messege;
    }
}