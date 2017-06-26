namespace Client.Data
{
    public class RoomState
    {
        public Player[] AllPlayers;
        public string[] CommunityCards = new string[5];
        public string CurrentPlayer;
        public string CurrentWinners;
        public string GameStatus;
        public bool IsOn;
        public int MaxPlayers;
        public string Messege;
        public int MinPlayers;
        public int Pot;
        public string RoomName;
        public bool SepctatingAllowed;
        public UserData[] Spectators;
    }
}