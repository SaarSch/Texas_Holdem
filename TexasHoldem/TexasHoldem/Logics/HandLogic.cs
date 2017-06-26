using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Game;

namespace TexasHoldem.Logics
{
    public class HandLogic
    {
        public HandStrength HandCalculator(List<Card> cards)
        {
            int handValue;
            HandRank handRank;
            var hand = new List<Card>();
            var orderByValue = cards.OrderBy(card => card.Value).ToList();
            var boost = (int) Math.Pow(10, 6);

            //Look for simillar cards:
            var threesList = new List<Card>();
            var pairsList = new List<Card>();
            var foursList = new List<Card>();
            var i = 0;
            while (i < 6)
            {
                if (orderByValue.ElementAt(i).Value == orderByValue.ElementAt(i + 1).Value)
                {
                    if (i == 5 || orderByValue.ElementAt(i + 2).Value != orderByValue.ElementAt(i).Value)
                    {
                        pairsList.AddRange(orderByValue.GetRange(i, 2));
                        i = i + 2;
                        continue;
                    }
                    if (i < 4 && orderByValue.ElementAt(i + 3).Value == orderByValue.ElementAt(i).Value)
                    {
                        foursList.AddRange(orderByValue.GetRange(i, 4));
                        break;
                    }
                    threesList.AddRange(orderByValue.GetRange(i, 3));
                    i = i + 3;
                    continue;
                }
                i++;
            }
            if (pairsList.Count == 6) pairsList.RemoveRange(0, 2);
            if (threesList.Count == 6) threesList.RemoveRange(0, 3);

            //Look for simillar shape:
            var sameShapeList = cards.Where(card => card.Type == CardType.Clubs).OrderBy(card => card.Value).ToList();
            if (sameShapeList.Count < 5)
                sameShapeList = cards.Where(card => card.Type == CardType.Diamonds)
                    .OrderBy(card => card.Value)
                    .ToList();
            if (sameShapeList.Count < 5)
                sameShapeList = cards.Where(card => card.Type == CardType.Hearts).OrderBy(card => card.Value).ToList();
            if (sameShapeList.Count < 5)
                sameShapeList = cards.Where(card => card.Type == CardType.Spades).OrderBy(card => card.Value).ToList();

            //Look for ascending
            var ascending = new List<Card>();
            for (var j = 0; j < 6; j++)
            for (var q = j + 1; q < 7; q++)
            {
                var tempOrderd = new List<Card>();
                tempOrderd.AddRange(orderByValue);
                tempOrderd.RemoveAt(q);
                tempOrderd.RemoveAt(j);

                var tempAscending = 0;
                for (var m = 0; m < 4; m++)
                    if (tempOrderd[m].Value + 1 == tempOrderd[m + 1].Value) tempAscending++;
                if (tempAscending == 4 && SumListCard(ascending) < SumListCard(tempOrderd))
                    ascending = tempOrderd;
            }

            //Decide Hand
            var temp = IsStraightFlush(ascending, sameShapeList);
            if (temp != null)
            {
                hand = temp;
                handRank = hand.ElementAt(0).Value == 10 ? HandRank.RoyalFlush : HandRank.StraightFlush;
                handValue = CalculateHandValue(hand, 8 * boost);
            }
            else if (foursList.Count == 4)
            {
                handRank = HandRank.FourOfAKind;
                orderByValue.RemoveAll(card => foursList.Contains(card));
                hand.Add(orderByValue.ElementAt(2));
                hand.AddRange(foursList);
                handValue = CalculateHandValue(hand, 7 * boost);
            }
            else if (threesList.Count == 3 && pairsList.Count >= 2)
            {
                handRank = HandRank.FullHouse;
                hand.AddRange(pairsList.GetRange(pairsList.Count - 2, 2));
                hand.AddRange(threesList);
                handValue = CalculateHandValue(hand, 6 * boost);
            }
            else if (sameShapeList.Count >= 5)
            {
                hand.AddRange(sameShapeList.GetRange(sameShapeList.Count - 5, 5));
                handRank = HandRank.Flush;
                handValue = CalculateHandValue(hand, 5 * boost);
            }
            else if (ascending.Count >= 5)
            {
                hand.AddRange(ascending.GetRange(ascending.Count - 5, 5));
                handRank = HandRank.Straight;
                handValue = CalculateHandValue(hand, 4 * boost);
            }
            else if (threesList.Count == 3)
            {
                handRank = HandRank.ThreeOfAKind;
                orderByValue.RemoveAll(card => threesList.Contains(card));
                hand.AddRange(orderByValue.GetRange(2, 2));
                hand.AddRange(threesList);
                handValue = CalculateHandValue(hand, 3 * boost);
            }
            else
            {
                switch (pairsList.Count)
                {
                    case 4:
                        handRank = HandRank.TwoPair;
                        orderByValue.RemoveAll(card => pairsList.Contains(card));
                        hand.Add(orderByValue.ElementAt(2));
                        hand.AddRange(pairsList);
                        handValue = CalculateHandValue(hand, 2 * boost);
                        break;
                    case 2:
                        handRank = HandRank.Pair;
                        orderByValue.RemoveAll(card => pairsList.Contains(card));
                        hand.AddRange(orderByValue.GetRange(2, 3));
                        hand.AddRange(pairsList);
                        handValue = CalculateHandValue(hand, boost);
                        break;
                    default:
                        handRank = HandRank.HighCard;
                        hand.AddRange(orderByValue.GetRange(2, 5));
                        handValue = CalculateHandValue(hand, 0);
                        break;
                }
            }
            return new HandStrength(handValue, handRank, hand);
        }

        public int CalculateHandValue(List<Card> hand, int boost)
        {
            var ans = boost;
            for (var i = 0; i < 5; i++)
                ans = ans + (int) Math.Pow(10, i) * hand.ElementAt(i).Value;
            return ans;
        }

        private List<Card> IsStraightFlush(List<Card> ascending, List<Card> similarShape)
        {
            if (ascending.Count < 5 || similarShape.Count < 5) return null;
            return ascending.Count == 5 ? IsStrightFlushHelper(ascending, similarShape) : null;
        }

        private List<Card> IsStrightFlushHelper(List<Card> ascending, List<Card> similarShape)
        {
            var hand = new List<Card>();
            hand.AddRange(ascending);
            ascending.RemoveAll(similarShape.Contains);
            return ascending.Count == 0 ? hand : null;
        }

        private int SumListCard(List<Card> cards)
        {
            var sum = 0;
            for (var i = 0; i < cards.Count; i++) sum += cards[i].Value;
            return sum;
        }
    }
}