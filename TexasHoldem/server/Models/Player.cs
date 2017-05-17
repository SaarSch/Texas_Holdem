using System.Collections.Generic;

namespace server.Models
{
    public class Player
    {
        public string PlayerName;
        public int CurrentBet;
        public int ChipsAmount;
        public string Avatar;
        public string[] PlayerHand;
        public List<string> messages = new List<string>();

        public bool Equals(Player other)
        {
            if (messages.Count != other.messages.Count) return false;
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i] != other.messages[i]) return false;
            }
            if (PlayerHand.Length != other.PlayerHand.Length) return false;
            for (int i = 0; i < PlayerHand.Length; i++)
            {
                if (messages[i] != other.messages[i]) return false;
            }
            return PlayerName == other.PlayerName && CurrentBet == other.CurrentBet && ChipsAmount == other.ChipsAmount && Avatar == other.Avatar;
           
        }
    }
}