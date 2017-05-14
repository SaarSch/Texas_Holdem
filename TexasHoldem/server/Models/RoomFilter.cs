namespace server.Models
{
    public class RoomFilter
    {
        public string user;
        public string player_name;
        public int? pot_size;
        public bool? league_only;
        public string game_type;
        public int? buy_in_policy;
        public int? chip_policy;
        public int? min_bet;
        public int? min_players;
        public int? max_players;
        public bool? sepctating_allowed;
    }
}