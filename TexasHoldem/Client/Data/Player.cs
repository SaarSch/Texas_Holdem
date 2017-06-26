using System.Collections.Generic;

namespace Client.Data
{
    public class Player
    {
        public string Avatar;
        public int ChipsAmount;
        public int CurrentBet;
        public List<string> Messages = new List<string>();
        public string[] PlayerHand;
        public string PlayerName;
    }
}