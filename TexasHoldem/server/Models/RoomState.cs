namespace server.Models
{
    public class RoomState
    {
        public string RoomName;
        public bool IsOn;
        public int Pot;
        public string GameStatus;
        public string[] CommunityCards = new string[5];
        public Player[] AllPlayers;
        public int MinPlayers;
        public int MaxPlayers;
        public bool SepctatingAllowed;
        public string Messege;
        public string CurrentPlayer;
        public UserData[] spectators;

        public bool Equals(RoomState other)
        {
            if (other == null) return false;
            for(int i = 0; i < 5; i++)
            {
                if (CommunityCards[i] != other.CommunityCards[i])
                {
                    return false;
                }
            }
            if (AllPlayers.Length != other.AllPlayers.Length) return false;
            for (int i = 0; i < AllPlayers.Length; i++)
            {
                if (AllPlayers[i] != null)
                {
                    if (!AllPlayers[i].Equals(other.AllPlayers[i]))
                    {
                        return false;
                    }
                }
            
            }
            if (spectators.Length != other.spectators.Length) return false;
            for (int i = 0; i < spectators.Length; i++)
            {
                if (spectators[i] != null)
                {
                    if (!spectators[i].Equals(other.spectators[i]))
                    {
                        return false;
                    }
                }
            }

            return  RoomName == other.RoomName && IsOn == other.IsOn && Pot == other.Pot && GameStatus == other.GameStatus &&
                    MinPlayers == other.MinPlayers&&MaxPlayers==other.MaxPlayers&&SepctatingAllowed==other.SepctatingAllowed&&Messege==other.Messege&&CurrentPlayer==other.CurrentPlayer;
         }
    }
}

 