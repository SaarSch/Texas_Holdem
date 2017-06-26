namespace Server.Models
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

        public bool Equals(RoomState other)
        {
            if (other == null) return false;
            for (var i = 0; i < 5; i++)
                if (CommunityCards[i] != other.CommunityCards[i])
                    return false;
            if (AllPlayers.Length != other.AllPlayers.Length) return false;
            for (var i = 0; i < AllPlayers.Length; i++)
                if (AllPlayers[i] != null)
                    if (!AllPlayers[i].Equals(other.AllPlayers[i]))
                        return false;
            if (Spectators.Length != other.Spectators.Length) return false;
            for (var i = 0; i < Spectators.Length; i++)
                if (Spectators[i] != null)
                    if (!Spectators[i].Equals(other.Spectators[i]))
                        return false;

            return RoomName == other.RoomName && IsOn == other.IsOn && Pot == other.Pot &&
                   GameStatus == other.GameStatus &&
                   MinPlayers == other.MinPlayers && MaxPlayers == other.MaxPlayers &&
                   SepctatingAllowed == other.SepctatingAllowed && Messege == other.Messege &&
                   CurrentPlayer == other.CurrentPlayer;
        }
    }
}