namespace TexasHoldem.Game
{
    public class RoomFilter
    {
        public int? BuyInPolicy;
        public int? ChipPolicy;
        public string GameType;
        public bool? LeagueOnly;
        public int? MaxPlayers;
        public int? MinBet;
        public int? MinPlayers;
        public string PlayerName;
        public int? PotSize;
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