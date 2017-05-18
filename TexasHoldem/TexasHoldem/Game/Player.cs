using System;
using TexasHoldem.Exceptions;
using TexasHoldem.Loggers;
using TexasHoldem.Users;

namespace TexasHoldem.Game
{
    public class Player
    {
        public Card[] Hand  = new Card[2];
        public string Name;
        public int ChipsAmount;
        public int CurrentBet;
        public bool Folded;
        public HandStrength StrongestHand;
        public User User;
        public int PreviousRaise;
        public bool BetInThisRound;

        public Player(string name, User user)
        {
            User = user ?? throw new Exception("illegal User");
            Name = name ?? throw new Exception("illegal player name");
            Logger.Log(Severity.Action, "new player created for user:"+User.Username);
        }

        public void SetCards(Card first, Card second)
        {
            if (first == null || second == null)
            {
                var e = new Exception("can't put null cards in player hand");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            Hand[0] = first;
            Hand[1] = second;
            Logger.Log(Severity.Action,"user: " +User.Username+" player: "+Name+" got 2 cards: " +first.Value+", "+second.Value);
        }

        public void SetBet(int amount)
        {
            if (amount < 0 || amount > ChipsAmount)
            {
                Exception e = new IllegalBetException("bet must be greater then zero and less - equal to player chips");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            CurrentBet +=amount;
            ChipsAmount -= amount;
            PreviousRaise = amount;
            BetInThisRound = true;
            Logger.Log(Severity.Action, "User: " + User.Username + " player: " + Name + " set Bets= " + CurrentBet+ " current Chips Amount="+ ChipsAmount);
        }

        public void ClearBet() { CurrentBet = 0; }

        public void Fold() { Folded = true; }

        public void UndoFold() { Folded = false; }

        public override string ToString() { return "User name: " + User.Username + " Player name: " + Name+" Chip amount:"+ChipsAmount; }
    }
}
