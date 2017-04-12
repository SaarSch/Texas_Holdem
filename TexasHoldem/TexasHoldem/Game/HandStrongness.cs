using System;
using System.Collections.Generic;

public class HandStrongness
{
    public int _handValue;
    public HandRank _handRank;
    public List<Card> _cards;

    public HandStrongness(int handValue, HandRank handRank, List<Card> cards)
	{
        HandValue = handValue;
        Cards = cards;
        HandRank = handRank;
    }


}
