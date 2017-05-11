using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class Player
    {
        public string player_name;
        public int current_bet;
        public int chips_amount;
        public string avatar;
        public string[] player_hand;
    }
}