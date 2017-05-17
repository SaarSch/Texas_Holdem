using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Services
{
    public class RoomFilter
    {
        public string PlayerName;
        public int? PotSize;
        public bool? LeagueOnly;
        public string GameType;
        public int? BuyInPolicy;
        public int? ChipPolicy;
        public int? MinBet;
        public int? MinPlayers;
        public int? MaxPlayers;
        public bool? SepctatingAllowed;


        public RoomFilter(string player_name, int? pot_size, bool? league_only, string game_type, int? buy_in_policy,
            int? chip_policy, int? min_bet,
            int? min_players, int? max_players, bool? sepctating_allowed)
        {
            this.PlayerName = player_name;
            this.PotSize = pot_size;
            this.LeagueOnly = league_only;
            this.GameType = game_type;
            this.BuyInPolicy = buy_in_policy;
            this.ChipPolicy = chip_policy;
            this.MinBet = min_bet;
            this.MinPlayers = min_players;
            this.MaxPlayers = max_players;
            this.SepctatingAllowed = sepctating_allowed;
        }

        public RoomFilter()
        {
        }
    }
}
