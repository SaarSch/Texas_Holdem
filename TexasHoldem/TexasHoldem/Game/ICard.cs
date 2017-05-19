namespace TexasHoldem.Game
{
    public interface ICard
    {
        int Value { get; }
        CardType Type { get; }
        int CompareTo(object obj);
        string ToString();
    }
}