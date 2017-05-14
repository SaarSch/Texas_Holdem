using System;
using TexasHoldem.Loggers;

namespace TexasHoldem.Game
{
    public enum CardType
    {
        Spades, //עלה
        Hearts, //לב
        Diamonds, //יהלום
        Clubs, //תלתן
    }
    public class Card : IComparable
    {
        public int Value { get; }
        public CardType Type { get; }
        public const int MinValue = 2;
        public const int MaxValue = 14;

        public Card(int value, CardType type)
        {
            if (value < MinValue || value > MaxValue)
            {
                Logger.Log(Severity.Error, "card value is illegal");
                throw new Exception("card value is illegal");
            }

            Value = value;
            Type = type;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var otherCard = obj as Card;
            if (otherCard == null) return -1;
            if (Value.CompareTo(otherCard.Value) == 0 && Type.CompareTo(otherCard.Type) == 0) return 0;
            return -1;
        }

        public override string ToString()
        {
            return "(" + Value + Type + ")";
        }
    }
}