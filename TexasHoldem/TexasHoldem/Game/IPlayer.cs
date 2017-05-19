using TexasHoldem.Users;

namespace TexasHoldem.Game
{
    public interface IPlayer
    {
        ICard[] Hand { get; set; }
        string Name { get; set; }
        int ChipsAmount { get; set; }
        int CurrentBet { get; set; }
        bool Folded { get; set; }
        HandStrength StrongestHand { get; set; }
        IUser User { get; set; }
        int PreviousRaise { get; set; }
        bool BetInThisRound { get; set; }
        void SetCards(ICard first, ICard second);
        void SetBet(int amount);
        void ClearBet();
        void Fold();
        void UndoFold();
        string ToString();
    }
}