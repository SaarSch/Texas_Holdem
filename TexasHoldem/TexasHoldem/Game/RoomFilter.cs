namespace TexasHoldem.Game
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


        public RoomFilter(string playerName, int? potSize, bool? leagueOnly, string gameType, int? buyInPolicy,
            int? chipPolicy, int? minBet,
            int? minPlayers, int? maxPlayers, bool? sepctatingAllowed)
        {
            PlayerName = playerName;
            PotSize = potSize;
            LeagueOnly = leagueOnly;
            GameType = gameType;
            BuyInPolicy = buyInPolicy;
            ChipPolicy = chipPolicy;
            MinBet = minBet;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            SepctatingAllowed = sepctatingAllowed;
        }

        public RoomFilter()
        {
        }
    }
}
