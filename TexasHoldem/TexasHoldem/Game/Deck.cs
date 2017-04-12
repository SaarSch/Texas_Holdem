using System;
using System.Collections.Generic;

public class Deck
{
	
       public List<Card> cards = new List<Card>(52);

        public Deck()
        {
            for (int i = 1; i < 14; i++) // init deck
            {
                for (int j = 1; j < 5; j++)
                {
                    cards.Add(new Card(i, (CardType)j));
                }
            }
            ShuffulDeck();
        }

        public void ShuffulDeck()
        {
            Random rnd = new Random();
            for (var i = 0; i < cards.Count; i++)
            {
                Swap(cards, i, rnd.Next(i, cards.Count));
            }
        }

        public static void Swap(List<Card> list, int i, int j)
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
           
            for(int i = 0; i < 52; i++)
            {
                if (cards[i].value == value && cards[i].type == type) return true;
            }
            return false;
   
        }


	}

