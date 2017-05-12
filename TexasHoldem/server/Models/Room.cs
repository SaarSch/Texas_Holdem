using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class Room
    {
        public string room_name;
        public string game_type;
        public int rank;
        public int buy_in_policy;
        public int min_bet;
        public int min_players;
        public int max_players;
        public bool sepctating_allowed;
    }
}