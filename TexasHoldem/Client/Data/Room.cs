using System.Drawing;

namespace Client
{
    public class Room
    {
        public string room_name { get; set; }
        public string game_type { get; set; }
        public int rank { get; set; }
        public int buy_in_policy { get; set; }
        public int min_bet { get; set; }
        public int min_players { get; set; }
        public int max_players { get; set; }
        public bool sepctating_allowed { get; set; }
    }
}