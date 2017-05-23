using System.Collections.Generic;

namespace Client.Data
{
    public class Player
    {
        public string PlayerName;
        public int CurrentBet;
        public int ChipsAmount;
        public string Avatar;
        public string[] PlayerHand;
        public List<string> Messages = new List<string>();
    }
}
