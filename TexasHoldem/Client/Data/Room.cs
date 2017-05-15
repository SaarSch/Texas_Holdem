using System.Drawing;

namespace Client
{
    public class Room
    {
        public string RoomName { get; set; }
        public string GameType { get; set; }
        public int League { get; set; }
        public int BuyInPolicy { get; set; }
        public int MinBet { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool SpectatingAllowed { get; set; }
    }
}