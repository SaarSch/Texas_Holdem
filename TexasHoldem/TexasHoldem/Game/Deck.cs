using System;
using System.Collections.Generic;
using TexasHoldem.Loggers;

namespace TexasHoldem.Game
{
    public class Deck
    {
        public List<Card> Cards { get; } = new List<Card>(52);

        public Deck()
        {
            for (var i = Card.MinValue; i <= Card.MaxValue; i++) // init deck
            {
                for (var j = 1; j < 5; j++)
                {
                    Cards.Add(new Card(i, (CardType)j-1));
                }
            }
            Shuffle();
        }

        public void Shuffle()
        {
            var rnd = new Random();
            for (var i = 0; i < Cards.Count; i++)
            {
                Swap(Cards, i, rnd.Next(i, Cards.Count));
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
            if (Cards.Count < 1)
            {
                Logger.Log(Severity.Exception, "Deck is empty cant draw card");
                throw new Exception("Deck is empty");
            }
            Card temp = Cards[0];
            Cards.RemoveAt(0);
            return temp;
        }

        public Boolean Contains(int value, CardType type)
        {
           
            for(int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i].Value == value && Cards[i].Type == type) return true;
            }
            return false;
   
        }
    }
}

