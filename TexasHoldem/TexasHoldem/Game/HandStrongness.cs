using System.Collections.Generic;

public class HandStrongness
{
    public int handStrongessValue;
    public HandRank handrank;
    public List<Card> handCards;

    public HandStrongness(int handValue, HandRank handRank, List<Card> cards)
	{
        handStrongessValue = handValue;
        handCards = cards;
        handrank = handRank;
    }


}
