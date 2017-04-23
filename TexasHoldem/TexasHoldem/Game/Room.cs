using System;
using System.Collections.Generic;
using System.Linq;

public enum HandRank
    {
        ROYAL_FLUSH,
        STRAIGHT_FLUSH,
        FOUR_OF_A_KIND,
        FULL_HOUSE,
        FLUSH,
        STRAIGHT,
        THREE_OF_A_KIND,
        TWO_PAIR,
        PAIR,
        HIGH_CARD
    }

public class Room
{
    public Boolean IsOn = false;
    public List<User> spectateUsers = new List<User>();
    public List<Player> players = new List<Player>(8);
    public Deck Deck = new Deck();
    public Card[] communityCards = new Card[5];
    public string name;
    public int rank;
    public GamePreferences gamePreferences;
    public Boolean flop;

    public Room(String name, Player creator, GamePreferences gamePreferences)
    {

        if (name == null)
        {
            Logger.Log(Severity.Exception, "room name cant be null");
            throw new Exception("room name cant be null");
        }

        if (creator == null)
        {
            Logger.Log(Severity.Exception, "creator palyer cant be null");
            throw new Exception("creator palyer cant be null");
        }

        if (gamePreferences == null)
        {
            Logger.Log(Severity.Exception, "game Preferences cant be null");
            throw new Exception("game Preferences cant be null");
        }
        //chipPolicy- amount of chips eace player is given, 0== all in.  
        //minBet- the minimum bet
        //buy-in- the minimum chip to join the game

        if (creator.User.chipsAmount < gamePreferences.minBet || (creator.User.chipsAmount < gamePreferences.chipPolicy && gamePreferences.chipPolicy > 0) || creator.User.chipsAmount < gamePreferences.buyInPolicy)
        {
            Logger.Log(Severity.Error, "player chips amount is too low to join");
            throw new Exception("player chips amount is too low to join");
        }

        if (gamePreferences.chipPolicy == 0)
        {
            creator.ChipsAmount = creator.User.chipsAmount;
            creator.User.chipsAmount = 0;
        }

        else
        {
            creator.ChipsAmount = gamePreferences.chipPolicy;
            creator.User.chipsAmount -= gamePreferences.chipPolicy;
        }

        players.Add(creator);
        this.name = name;
        rank = creator.User.Rank;
        Logger.Log(Severity.Action, "new room was created room  name="+name+" rank="+rank );
    }

    public void AddPlayer(Player p)
    {
        if (IsOn)
        {
            Logger.Log(Severity.Exception, "cant join, game is on");
            throw new Exception("cant join, game is on");
        }
        if (p == null)
        {
            Logger.Log(Severity.Exception, "cant add a null player to the room");
            throw new Exception("illegal Player");
        }
        if (players.Count > gamePreferences.maxPlayers)
        {
            Logger.Log(Severity.Exception, "room is full, cant add the player");
            throw new Exception("room is full");
        }
        if (p.User.Rank<rank)
        {
            Logger.Log(Severity.Error, "player rank is too low to join");
            throw new Exception("player rank is too low to join");
        }
        if (p.User.chipsAmount < gamePreferences.minBet || (p.User.chipsAmount < gamePreferences.chipPolicy && gamePreferences.chipPolicy > 0)|| p.User.chipsAmount<gamePreferences.buyInPolicy)
        {
            Logger.Log(Severity.Error, "player chips amount is too low to join");
            throw new Exception("player chips amount is too low to join");
        }

        if (gamePreferences.chipPolicy == 0)
        {
            p.ChipsAmount = p.User.chipsAmount;
            p.User.chipsAmount = 0;
        }
        else
        {
            p.ChipsAmount = gamePreferences.chipPolicy;
            p.User.chipsAmount -= gamePreferences.chipPolicy;
        }
        players.Add(p);
        Logger.Log(Severity.Action, "new player joined the room: room name=" + name +"player name="+p.Name);
    }

    public void Spectate(User user)
    {
        if (!gamePreferences.spectating)
        {

            Logger.Log(Severity.Exception, "cant spectat at this room");
            throw new Exception("cant spectat at this room");
        }
        if(user is null)
        {
            Logger.Log(Severity.Exception, "cant add a null user to the room");
            throw new Exception("null user");
        }
        spectateUsers.Add(user);
    }

    private void DealTwo()
    {
        foreach (Player p in players)
        {
            p.SetCards(Deck.Draw(), Deck.Draw());
        }
    }

    private bool AllFold()
    {
        bool ans = true;
        foreach (Player p in players)
        {
            if (!p.Folded) return p.Folded;
        }
        return ans;
    }


    public void DealCommunityFirst()
    {
        if (!IsOn)
        {
            Logger.Log(Severity.Exception, "The game has not yet started cant deal community first");
            throw new Exception("The game has not yet started cant deal community first");
        }
        if (AllFold())
        {
            Logger.Log(Severity.Error, "all players folded no need to deal community cards");
            throw new Exception("all players folded");
        }
        for (int i = 0; i < 5; i++)
            if (communityCards[i] != null)
            {
                Logger.Log(Severity.Error, "Already distributed first 3 community cards");
                throw new Exception("Already distributed community cards");
            }

        communityCards[0] = Deck.Draw();
        communityCards[1] = Deck.Draw();
        communityCards[2] = Deck.Draw();
        foreach (Player p in players) p.betInThisRound = false;
        Logger.Log(Severity.Action, "3 commuinty cards deald room name=" + name + "community cards:" + communityCards[0].ToString() + communityCards[1].ToString() + communityCards[2].ToString());
    }

    public void DealCommunitySecond()
    {
        if (!IsOn)
        {
            Logger.Log(Severity.Exception, "The game has not yet started cant deal community second");
            throw new Exception("The game has not yet started cant deal community second");
        }
        if (AllFold())
        {
            Logger.Log(Severity.Error, "all players folded no need to deal community cards");
            throw new Exception("all players folded");
        }
        for (int i = 3; i < 5; i++)
            if (communityCards[i] != null)
            {
                Logger.Log(Severity.Error, "Already distributed 4 community cards");
                throw new Exception("Already distributed community cards");
            }
        communityCards[3] = Deck.Draw();
        foreach (Player p in players) p.betInThisRound = false;
        Logger.Log(Severity.Action, "1 commuinty cards deald room name=" + name + "community cards:"+ communityCards[0].ToString() + communityCards[1].ToString() + communityCards[2].ToString()+ communityCards[3].ToString());
    }

    public void DealCommunityThird()
    {
        if (!IsOn)
        {
            Logger.Log(Severity.Exception, "The game has not yet started cant deal community third");
            throw new Exception("The game has not yet started cant deal community third");
        }
        if (AllFold())
        {
            Logger.Log(Severity.Error, "all players folded no need to deal community cards");
            throw new Exception("all players folded");
        }
        if (communityCards[4] != null)
        {
            Logger.Log(Severity.Error, "Already distributed 5 community cards");
            throw new Exception("Already distributed community cards");
        }
        communityCards[4] = Deck.Draw();
        foreach (Player p in players) p.betInThisRound = false;
        Logger.Log(Severity.Action, "1 commuinty cards deald room name=" + name + "community cards:" + communityCards[0].ToString() + communityCards[1].ToString() + communityCards[2].ToString() + communityCards[3].ToString()+ communityCards[4].ToString());
    }

    public void StartGame()
    {
        if (players.Count < gamePreferences.minPlayers)
        {
            Logger.Log(Severity.Error, "cant play with less then min players");
            throw new Exception("cant play with less then min players");
        }
        if (IsOn)
        {
            Logger.Log(Severity.Exception, "game is already started");
            throw new Exception("game alerady started");
        }

        IsOn = true;
        int smallBlind = gamePreferences.minBet / 2;
        Deck = new Deck();

        // 0 = dealer 1=small blind 2=big blind
        if (players.Count == 2)
        {
            Logger.Log(Severity.Action, "new game started in room " + name + " dealer and small blind-" + players[0].ToString()+ "big blind-"+players[1]);
            players[0].SetBet(smallBlind);
            players[1].SetBet(gamePreferences.minBet);
        }
        else
        {
            Logger.Log(Severity.Action, "new game started in room"+name+" dealer" + players[0].ToString()+ "small blind-" + players[1].ToString() + "big blind-" + players[2] +PlayersToString(players));
            players[1].SetBet(smallBlind);
            players[2].SetBet(gamePreferences.minBet);
        }
        DealTwo();
    }

    public void SetBet(Player p, int bet)
    {
        if(p == null)
        {
            Logger.Log(Severity.Exception, "player cant be null");
            throw new Exception("player cant be null");
        }

        if (bet < gamePreferences.minBet)
        {
            Logger.Log(Severity.Error, "cant bet less then min bet");
            throw new Exception("cant bet less then min bet");
        }

        if (gamePreferences.gameType == Gametype.NoLimit && p.betInThisRound && p.previousRaise<bet) // no limit mode
        {
            Logger.Log(Severity.Error, "cant bet less then previous raise in no limit mode");
            throw new Exception("cant bet less then previous raise in no limit mode");
        }




        p.SetBet(bet);

    }

    public void ExitRoom(Player p)
    {
        if (IsOn)
        {
            Logger.Log(Severity.Error, "cant exit while game is on");
            throw new Exception("cant exit while game is on");
        }

        if (p == null||!players.Contains(p))
        {
            Logger.Log(Severity.Exception, "cant exit from room player is invalid");
            throw new Exception("cant exit from room player is invalid");
        }

        p.User.chipsAmount += p.ChipsAmount;
        players.Remove(p);
    }

    public List<Player> Winners()
    {
        if (communityCards[4] == null)
        {
            Logger.Log(Severity.Exception, "game is not over");
            throw new Exception("game is not over");
        }
        if (players.Count < 2)
        {
            Logger.Log(Severity.Error, "cant play with less the 2 players");
            throw new Exception("cant play with less then 2 players");
        }
        
        List<Player> winners = new List<Player>();
        foreach (Player p in players)
        {
            List<Card> hand = p.Hand.ToList();
            hand.AddRange(communityCards.ToList());
            p.StrongestHand = HandCalculator(hand);
        }
        int maxHand = 0;
        foreach (Player p in players)  if (p.StrongestHand.handStrongessValue > maxHand) maxHand = p.StrongestHand.handStrongessValue;
        foreach (Player p in players) if (p.StrongestHand.handStrongessValue == maxHand) winners.Add(p);
        return winners;
    }

    private string PlayersToString(List<Player> players)
    {
        string playersNames = "Players:";
        foreach (Player p in players) playersNames += p.ToString();
        return playersNames;
    }

    public void CalcWinnersChips()
    {
        List<Player> winners = Winners();
        Logger.Log(Severity.Action, "the winners in room" + name +"is"+PlayersToString(winners));
        foreach(Player p in winners)
        {
            p.User.wins++;
            if (p.User.wins == 10) // change to game center field
            {
                p.User.Rank++;
                p.User.wins = 0;   
            }
        }
        int totalChips = 0;
        foreach (Player p in players) totalChips += p.CurrentBet;
        int ChipsForPlayer = totalChips / winners.Count;
        foreach (Player p in winners) p.ChipsAmount += ChipsForPlayer;
        Logger.Log(Severity.Action, "cuurent status in room" + name + "is" + PlayersToString(players));

        CleanGame(); 
        IsOn = false;
        NextTurn();
    }

    public void NotifyRoom(string message)
    {
        if (message is null)
        {
            Logger.Log(Severity.Error, "cant send null mesege");
            throw new Exception("cant send null message");
        }
        List<User> roomUsers = new List<User>();

        foreach (Player p in players) roomUsers.Add(p.User);
        roomUsers.AddRange(spectateUsers);
        Notifier.Instance.Notify(roomUsers, message);
    }

    private void CleanGame()
    {
        foreach (Player p in players) { p.Hand[0] = null; p.Hand[1] = null; p.UndoFold();}
        for (int i = 0; i < 5; i++) communityCards[i] = null;
    }
    private void NextTurn()
    {
        Player zero = players[0];
        for(int i=0; i < players.Count-1; i++) players[i] = players[i + 1];
        players[players.Count-1] = zero;
    }

    public HandStrength HandCalculator(List<Card> cards)
    {

        int handValue = 0;
        HandRank handRank;
        List<Card> hand = new List<Card>();

        List<Card> orderByValue = cards.OrderBy(card => card.value).ToList();

        int boost = (int)Math.Pow(10, 6);


        //Look for simillar cards:
        List<Card> threesList = new List<Card>();
        List<Card> pairsList = new List<Card>();
        List<Card> foursList = new List<Card>();
        int i = 0;
        while (i < 6)
        {
            if (orderByValue.ElementAt(i).value == orderByValue.ElementAt(i + 1).value)
            {
                if (i == 5 || orderByValue.ElementAt(i + 2).value != orderByValue.ElementAt(i).value)
                {
                    pairsList.AddRange(orderByValue.GetRange(i, 2));
                    i = i + 2;
                    continue;
                }
                if (i < 4 && orderByValue.ElementAt(i + 3).value == orderByValue.ElementAt(i).value)
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
        List<Card> sameShapeList = new List<Card>();
        sameShapeList = cards.Where(card => card.type == CardType.Clubs).OrderBy(card => card.value).ToList();
        if (sameShapeList.Count < 5) sameShapeList = cards.Where(card => card.type == CardType.Diamonds).OrderBy(card => card.value).ToList();
        if (sameShapeList.Count < 5) sameShapeList = cards.Where(card => card.type == CardType.Hearts).OrderBy(card => card.value).ToList();
        if (sameShapeList.Count < 5) sameShapeList = cards.Where(card => card.type == CardType.Spades).OrderBy(card => card.value).ToList();

        //Look for ascending
        List<Card> ascending = new List<Card>();
        for (int j = 0; j < 6; j++)
        {
            
            for (int q = j + 1; q < 7; q++)
            {
                List<Card> TempOrderd = new List<Card>();
                TempOrderd.AddRange(orderByValue);
                TempOrderd.RemoveAt(q);
                TempOrderd.RemoveAt(j);
               
                int tempAscending = 0;
                for (int m = 0; m < 4; m++)
                {
                    if (TempOrderd[m].value + 1 == TempOrderd[m + 1].value) tempAscending++;
                }
                if (tempAscending == 4 && SumListCard(ascending) < SumListCard(TempOrderd)) ascending = TempOrderd;
            }
        }

        //Decide Hand
        List<Card> temp;
        temp = IsStraightFlush(ascending, sameShapeList);
        if (temp != null)
        {
            hand = temp;
            if (hand.ElementAt(0).value == 10) handRank = HandRank.ROYAL_FLUSH;
            else handRank = HandRank.STRAIGHT_FLUSH;
            handValue = CalculateHandValue(hand, 8 * boost);
        }
        else if (foursList.Count == 4)
        {
            handRank = HandRank.FOUR_OF_A_KIND;
            orderByValue.RemoveAll(card => foursList.Contains(card));
            hand.Add(orderByValue.ElementAt(2));
            hand.AddRange(foursList);
            handValue = CalculateHandValue(hand, 7 * boost);
        }
        else if (threesList.Count == 3 && pairsList.Count >= 2)
        {
            handRank = HandRank.FULL_HOUSE;
            hand.AddRange(pairsList.GetRange(pairsList.Count - 2, 2));
            hand.AddRange(threesList);
            handValue = CalculateHandValue(hand, 6 * boost);
        }
        else if (sameShapeList.Count >= 5)
        {
            hand.AddRange(sameShapeList.GetRange(sameShapeList.Count - 5, 5));
            handRank = HandRank.FLUSH;
            handValue = CalculateHandValue(hand, 5 * boost);
        }
        else if (ascending.Count >= 5)
        {
            hand.AddRange(ascending.GetRange(ascending.Count - 5, 5));
            handRank = HandRank.STRAIGHT;
            handValue = CalculateHandValue(hand, 4 * boost);
        }
        else if (threesList.Count == 3)
        {
            handRank = HandRank.THREE_OF_A_KIND;
            orderByValue.RemoveAll(card => threesList.Contains(card));
            hand.AddRange(orderByValue.GetRange(2, 2));
            hand.AddRange(threesList);
            handValue = CalculateHandValue(hand, 3 * boost);
        }
        else if (pairsList.Count == 4)
        {
            handRank = HandRank.TWO_PAIR;
            orderByValue.RemoveAll(card => pairsList.Contains(card));
            hand.Add(orderByValue.ElementAt(2));
            hand.AddRange(pairsList);
            handValue = CalculateHandValue(hand, 2 * boost);
        }
        else if (pairsList.Count == 2)
        {
            handRank = HandRank.PAIR;
            orderByValue.RemoveAll(card => pairsList.Contains(card));
            hand.AddRange(orderByValue.GetRange(2, 3));
            hand.AddRange(pairsList);
            handValue = CalculateHandValue(hand, boost);
        }
        else
        {
            handRank = HandRank.HIGH_CARD;
            hand.AddRange(orderByValue.GetRange(2, 5));
            handValue = CalculateHandValue(hand, 0);
        }
        return new HandStrength(handValue, handRank, hand);
    }

    private int CalculateHandValue(List<Card> hand, int boost)
    {
        int ans = boost;
        for (int i = 0; i < 5; i++)
        {
            ans = ans + (int)Math.Pow(10, i) * hand.ElementAt(i).value;
        }
        return ans;
    }

    private List<Card> IsStraightFlush(List<Card> ascending, List<Card> similarShape)
    {
        if (ascending.Count < 5 || similarShape.Count < 5) return null;
        if (ascending.Count == 5)
        {
            return IsStrightFlushHelper(ascending, similarShape);
        }
        if (ascending.Count == 6)
        {
            List<Card> leftCut = new List<Card>();
            leftCut.AddRange(ascending);
            List<Card> rightCut = new List<Card>();
            rightCut.AddRange(ascending);

            leftCut.RemoveAt(0);
            rightCut.RemoveAt(5);
            leftCut = IsStrightFlushHelper(leftCut, similarShape);
            rightCut = IsStrightFlushHelper(rightCut, similarShape);
            if (leftCut == null) return rightCut;
            return leftCut;
        }
        if (ascending.Count == 7)
        {
            List<Card> leftCut = new List<Card>();
            leftCut.AddRange(ascending);
            List<Card> rightCut = new List<Card>();
            rightCut.AddRange(ascending);
            List<Card> bothCut = new List<Card>();
            bothCut.AddRange(ascending);

            leftCut.RemoveRange(0, 2);
            rightCut.RemoveRange(5, 2);
            bothCut.RemoveAt(0);
            bothCut.RemoveAt(5);
            leftCut = IsStrightFlushHelper(leftCut, similarShape);
            rightCut = IsStrightFlushHelper(rightCut, similarShape);
            bothCut = IsStrightFlushHelper(bothCut, similarShape);
            if (bothCut == null && leftCut == null) return rightCut;
            if (leftCut == null) return bothCut;
            return leftCut;
        }
        return null;
    }

    private List<Card> IsStrightFlushHelper(List<Card> ascending, List<Card> similarShape)
    {
        List<Card> hand = new List<Card>();
        hand.AddRange(ascending);
        ascending.RemoveAll(card => similarShape.Contains(card));
        if (ascending.Count == 0) return hand;
        return null;
    }

    private int SumListCard(List<Card> cards)
    {
        int sum = 0;
        for (int i = 0; i < cards.Count;i++) sum += cards[i].value;
        return sum;
    }
}
