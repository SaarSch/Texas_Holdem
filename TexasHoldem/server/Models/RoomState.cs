namespace server.Models
{
    public class RoomState
    {
        public string RoomName;
        public bool IsOn;
        public int Pot;
        public string GameStatus;
        public string[] CommunityCards=new string[5];
        public Player[] AllPlayers;
        public int MinPlayers;
        public int MaxPlayers;
        public bool SepctatingAllowed;
        public string Messege;
        public string CurrentPlayer;
        public UserData[] spectators;
    }
}