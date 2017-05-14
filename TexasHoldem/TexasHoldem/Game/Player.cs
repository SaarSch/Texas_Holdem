using System;
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
            if (name == null)
            {
                Logger.Log(Severity.Exception, "cant create player with null name");
                throw new Exception("illegal player name");
            }
            if(user == null)
            {
                Logger.Log(Severity.Exception, "cant create player with null User");
                throw new Exception("illegal User");
            }
            this.User = user;
            this.Name = name;
            Logger.Log(Severity.Action, "new player created for user:"+User.GetUsername());
        }

        public void SetCards(Card first, Card second)
        {
            if (first == null || second == null)
            {
                Logger.Log(Severity.Exception, "cant put null cards int player hand");
                throw new Exception("can't put null cards");
            }
            Hand[0] = first;
            Hand[1] = second;
            Logger.Log(Severity.Action,"user: " +User.GetUsername()+" player: "+Name+" got 2 cards: " +first.Value+", "+second.Value);
        }

        public void SetBet(int amount)
        {
            if (amount < 0 || amount > ChipsAmount)
            {
                Logger.Log(Severity.Error, "bet must be greater then zero and less-equal to player chips");
                throw new Exception("illegael bet");
            }
            CurrentBet +=amount;
            ChipsAmount -= amount;
            PreviousRaise = amount;
            BetInThisRound = true;
            Logger.Log(Severity.Action, "User: " + User.GetUsername() + " player: " + Name + " set Bets= " + CurrentBet+ " current Chips Amount="+ ChipsAmount);
        }

        public void ClearBet() { CurrentBet = 0; }

        public void Fold() { Folded = true; }

        public void UndoFold() { Folded = false; }

        public override string ToString() { return "User name: " + User.GetUsername() + " Player name: " + Name+" Chip amount:"+ChipsAmount; }
    }
}
