using System;

public class Player
{

    public Card[] Hand  = new Card[2];
    public string Name;
    public int ChipsAmount;
    public int CurrentBet = 0;
    public Boolean Folded = false;
    public HandStrength StrongestHand;
    public User User;



    public Player(string name, int chips ,User user)
        {
        if (chips < 0) throw new Exception("illegal amount of chips");
        Name = name ?? throw new Exception("illegal player name");
        User = user ?? throw new Exception("illegal User");
        ChipsAmount = chips;
        }

        public void SetCards(Card first, Card second)
        {
            if (first == null || second == null) throw new Exception("can't put null cards");
            Hand[0] = first;
            Hand[1] = second;
        }

        public void SetBet(int amount)
        {
            if (amount < 0 || amount > ChipsAmount) throw new Exception("illegael bet");
            CurrentBet +=amount;
            ChipsAmount -= amount;
        }

        public void ClearBet() { CurrentBet = 0; }

        public void Fold() { Folded = true; }
   }
