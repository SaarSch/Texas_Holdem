using System.Collections.Generic;

namespace TexasHoldem.Game
{
    public enum HandRank
    {
        RoyalFlush,
        StraightFlush,
        FourOfAKind,
        FullHouse,
        Flush,
        Straight,
        ThreeOfAKind,
        TwoPair,
        Pair,
        HighCard,
        Fold
    }

    public class HandStrength
    {
        public List<Card> HandCards;
        public HandRank Handrank;
        public int HandStrongessValue;

        public HandStrength(int handValue, HandRank handRank, List<Card> cards)
        {
            HandStrongessValue = handValue;
            HandCards = cards;
            Handrank = handRank;
        }
    }
}