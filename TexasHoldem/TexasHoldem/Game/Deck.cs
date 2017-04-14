using System;
using System.Collections.Generic;

public class Deck
{
	
       public List<Card> cards = new List<Card>(52);

        public Deck()
        {
            for (int i = 2; i < 15; i++) // init deck
            {
                for (int j = 1; j < 5; j++)
                {
                    cards.Add(new Card(i, (CardType)j-1));
                }
            }
        Shuffle();
        }

        public void Shuffle()
        {
            Random rnd = new Random();
            for (var i = 0; i < cards.Count; i++)
            {
                Swap(cards, i, rnd.Next(i, cards.Count));
            }
        }

        private void Swap(List<Card> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public Card Draw()
        {
            if (cards.Count < 1) throw new Exception("Deck is empty");
            Card Temp = cards[0];
            cards.RemoveAt(0);
            return Temp;
        }

        public Boolean Contains(int value, CardType type)
        {
           
            for(int i = 0; i < this.cards.Count; i++)
            {
                if (cards[i].value == value && cards[i].type == type) return true;
            }
            return false;
   
        }
	}

