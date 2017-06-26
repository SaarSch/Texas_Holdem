using System;
using TexasHoldem.Exceptions;
using TexasHoldem.Loggers;
using TexasHoldem.Users;

namespace TexasHoldem.Game
{
    public class Player : IPlayer
    {
        public Player(string name, IUser user)
        {
            User = user ?? throw new Exception("illegal User");
            Name = name ?? throw new Exception("illegal player name");
            Exit = false;
            Logger.Log(Severity.Action, "new player created for user:" + User.Username);
        }

        public ICard[] Hand { get; set; } = new ICard[2];
        public string Name { get; set; }
        public int ChipsAmount { get; set; }
        public int CurrentBet { get; set; }
        public bool Folded { get; set; }
        public HandStrength StrongestHand { get; set; }
        public IUser User { get; set; }
        public int PreviousRaise { get; set; }
        public bool BetInThisRound { get; set; }
        public bool Exit { get; set; }

        public void SetCards(ICard first, ICard second)
        {
            if (first == null || second == null)
            {
                var e = new Exception("can't put null cards in player hand");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            Hand[0] = first;
            Hand[1] = second;
            Logger.Log(Severity.Action,
                "user: " + User.Username + " player: " + Name + " got 2 cards: " + first.Value + ", " + second.Value);
        }

        public void SetBet(int amount)
        {
            if (amount < 0 || amount > ChipsAmount)
            {
                Exception e = new IllegalBetException("bet must be greater then zero and less - equal to player chips");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            CurrentBet += amount;
            ChipsAmount -= amount;
            PreviousRaise = amount;
            BetInThisRound = true;
            Logger.Log(Severity.Action,
                "User: " + User.Username + " player: " + Name + " set Bets= " + CurrentBet + " current Chips Amount=" +
                ChipsAmount);
        }

        public void ClearBet()
        {
            CurrentBet = 0;
            PreviousRaise = 0;
            Folded = false;
        }

        public void Fold()
        {
            Folded = true;
        }

        public void UndoFold()
        {
            Folded = false;
        }

        public override string ToString()
        {
            return "User name: " + User.Username + " Player name: " + Name + " Chip amount:" + ChipsAmount;
        }
    }
}