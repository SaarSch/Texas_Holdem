using System.Collections.Generic;

public class HandStrength
{
    public int handStrongessValue;
    public HandRank handrank;
    public List<Card> handCards;

    public HandStrength(int handValue, HandRank handRank, List<Card> cards)
	{
        handStrongessValue = handValue;
        handCards = cards;
        handrank = handRank;
    }


}
