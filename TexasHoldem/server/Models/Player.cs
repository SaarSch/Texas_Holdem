using System.Collections.Generic;

namespace Server.Models
{
    public class Player
    {
        public string PlayerName;
        public int CurrentBet;
        public int ChipsAmount;
        public string Avatar;
        public string[] PlayerHand;
        public List<string> Messages = new List<string>();

        public bool Equals(Player other)
        {
            if (Messages.Count != other.Messages.Count) return false;
            for (var i = 0; i < Messages.Count; i++)
            {
                if (Messages[i] != other.Messages[i]) return false;
            }
            if (PlayerHand.Length != other.PlayerHand.Length) return false;
            for (var i = 0; i < PlayerHand.Length; i++)
            {
                if (PlayerHand[i] != other.PlayerHand[i]) return false;
            }
            return PlayerName == other.PlayerName && CurrentBet == other.CurrentBet && ChipsAmount == other.ChipsAmount && Avatar == other.Avatar;
           
        }
    }
}