using System.Collections.Generic;

namespace TexasHoldem.Game
{
    public class HandStrength
    {
        public int HandStrongessValue;
        public HandRank Handrank;
        public List<Card> HandCards;

        public HandStrength(int handValue, HandRank handRank, List<Card> cards)
        {
            HandStrongessValue = handValue;
            HandCards = cards;
            Handrank = handRank;
        }


    }
}
