namespace server.Models
{
    public class Room
    {
        public string RoomName;
        public string CreatorUserName;
        public string CreatorPlayerName;
        public string GameType;
        public int ChipPolicy;
        public int BuyInPolicy;
        public int MinBet;
        public int MinPlayers;
        public int MaxPlayers;
        public bool SepctatingAllowed;
    }
}