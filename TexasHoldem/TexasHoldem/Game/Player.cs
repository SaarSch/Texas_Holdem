using System;

public class Player
{

        public Card[] hand = new Card[2];
        public string name;
        public int chipsAmount;
        public int currentBet = 0;
        public Boolean folded =false;
        public HandStrongness strongestHand;
        public User user;



        public Player(string name, int chips ,User user)
        {
            if (name == null) throw new Exception("illegal player name");
            if (chips < 0) throw new Exception("illegal amount of chips");
            if(user==null) throw new Exception("illegal User");

            this.name = name;
            this.user=user;
            this.chipsAmount=chips;
        }

        public int CurrentBet { get => currentBet; set => currentBet = value; }

        public void setCards(Card first, Card second)
        {
            if (first == null || second == null) throw new Exception("can't put null cards");
            hand[0] = first;
            hand[1] = second;
        }

        public void SetBet(int amount)
        {
            if (amount < 0 || amount > chipsAmount) throw new Exception("illegael bet");
            currentBet +=amount;
            chipsAmount -= amount;
        }

        public void ClearBet() { currentBet = 0; }

        public void Fold() { folded = true; }
   }
