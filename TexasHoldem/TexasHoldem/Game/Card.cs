using System;

public enum CardType
{
    Spades, //עלה
    Hearts,//לב
    Diamonds,// יהלום
    Clubs, // תלתן
}
public class Card : IComparable
{

    public int value;
    public CardType type;

    public Card(int value, CardType type)
    {
        if (value < 2 || value > 14)
        {
            Logger.Log(Severity.Error, "card value is illegal");
            throw new Exception("card value is illegal");
        }

        this.value = value;
        this.type = type;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        Card otherCard = obj as Card;
        if (otherCard != null)
            if (this.value.CompareTo(otherCard.value) == 0 && this.type.CompareTo(otherCard.type) == 0) return 0;
            else return -1;
        else return -1;

    }
}
