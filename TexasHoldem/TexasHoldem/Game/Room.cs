using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TexasHoldem;
using TexasHoldem.GameReplay;

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
        HIGH_CARD,
        FOLD
    }

public enum gameStatus
{
    preFlop,
    flop,
    turn,
    river, 
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
    public int pot = 0;
    public readonly string gameReplay;
    private int turn = 1;
    public gameStatus gameStatus;

    public Room(String name, Player creator, GamePreferences gamePreferences)
    {

        if (name == null)
        {
            Logger.Log(Severity.Exception, "room name cant be null");
            throw new Exception("room name cant be null");
        }

        if (creator == null)
        {
            Logger.Log(Severity.Exception, "creator player cant be null");
            throw new Exception("creator player cant be null");
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

        this.gamePreferences = gamePreferences;
        players.Add(creator);
        if (!Regex.IsMatch(name, "^[a-zA-Z0-9 ]*$"))
        {
            Logger.Log(Severity.Error, "room name contains illegal characters");
            throw new Exception("room name contains illegal characters");
        }
        if (name.Length > 30 || name.Length < 4)
        {
            Logger.Log(Severity.Error, "room name must be between 4 and 30 characters long");
            throw new Exception("room name too short / long");
        }
        this.name = name;
        rank = creator.User.Rank;

        gameReplay = Replayer.CreateReplay();
        Logger.Log(Severity.Action, "new room was created room  name="+name+" rank="+rank );
    }

    public Room AddPlayer(Player p)
    {
        foreach(Player p1 in players)
        {
            if (p1.Name.Equals(p.Name))
            {
                Logger.Log(Severity.Exception, "cant join, player name is already exist");
                throw new Exception("cant join, player name is already exist");
            }
        }
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
        if (p.User.Rank < rank)
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
        Logger.Log(Severity.Action, "new player joined the room: room name=" + name + "player name=" + p.Name);
        return this;
    }
  
    public void Spectate(User user)
    {
        if (!gamePreferences.spectating)
        {

            Logger.Log(Severity.Exception, "cant spectate at this room");
            throw new Exception("cant spectate at this room");
        }

        if(user is null)
        {
            Logger.Log(Severity.Exception, "cant add a null user to the room");
            throw new Exception("null user");
        }
        spectateUsers.Add(user);
    }

    public void DealTwo()
    {
        foreach (Player p in players)
        {

            p.SetCards(Deck.Draw(), Deck.Draw());
            Logger.Log(Severity.Action, "player"+p.Name+"got 2 cards:" +p.Hand[0].ToString()+p.Hand[1].ToString());
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
        
            if (gameStatus!=gameStatus.preFlop)
            {
                Logger.Log(Severity.Error, "Already distributed first 3 community cards");
                throw new Exception("Already distributed community cards");
            }

        communityCards[0] = Deck.Draw();
        communityCards[1] = Deck.Draw();
        communityCards[2] = Deck.Draw();
        foreach (Player p in players) p.betInThisRound = false;
        gameStatus = gameStatus.flop;

        Replayer.Save(gameReplay, turn, players, pot, communityCards, "the flop");
        Logger.Log(Severity.Action, "3 community cards dealed room name=" + name + "community cards:" + communityCards[0].ToString() + communityCards[1].ToString() + communityCards[2].ToString());
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
      
            if (gameStatus!=gameStatus.flop)
            {
                Logger.Log(Severity.Error, "Already distributed 4 community cards");
                throw new Exception("Already distributed community cards");
            }
        communityCards[3] = Deck.Draw();
        foreach (Player p in players) p.betInThisRound = false;
        gameStatus = gameStatus.turn;

        Replayer.Save(gameReplay, turn, players, pot, communityCards, "the turn");
        Logger.Log(Severity.Action, "1 community card dealed room name=" + name + "community cards:"+ communityCards[0].ToString() + communityCards[1].ToString() + communityCards[2].ToString()+ communityCards[3].ToString());
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
        if (gameStatus != gameStatus.turn)
        {
            Logger.Log(Severity.Error, "Already distributed 5 community cards");
            throw new Exception("Already distributed community cards");
        }
        communityCards[4] = Deck.Draw();
        foreach (Player p in players) p.betInThisRound = false;
        gameStatus = gameStatus.river;

        Replayer.Save(gameReplay, turn, players, pot, communityCards, "the river");
        Logger.Log(Severity.Action, "1 community card dealed room name=" + name + "community cards:" + communityCards[0].ToString() + communityCards[1].ToString() + communityCards[2].ToString() + communityCards[3].ToString()+ communityCards[4].ToString());
    }

    public Room StartGame()
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

        foreach(Player p in players)
        {
            p.User.numOfGames++;
            if (p.User.numOfGames == 11)
            {
                p.User.Rank = p.User.wins;
            }
        }

        gameStatus = gameStatus.preFlop;
        IsOn = true;
        int smallBlind = gamePreferences.minBet / 2;
        Deck = new Deck();

        // 0 = dealer 1=small blind 2=big blind
        Replayer.Save(gameReplay, turn, players, pot, null, "start of turn");
        if (players.Count == 2)
        {
            Logger.Log(Severity.Action, "new game started in room " + name + " dealer and small blind-" + players[0].ToString()+ "big blind-"+players[1]);
            SetBet(players[0], smallBlind,true);
            SetBet(players[1], gamePreferences.minBet,false);
        }
        else
        {
            Logger.Log(Severity.Action, "new game started in room"+name+" dealer" + players[0].ToString()+ "small blind-" + players[1].ToString() + "big blind-" + players[2] +PlayersToString(players));
            SetBet(players[1], smallBlind,true);
            SetBet(players[2], gamePreferences.minBet,false);
        }
        DealTwo();
        return this;
    }

    public Room Call(Player p)
    {
        if (!players.Contains(p))
        {
            Logger.Log(Severity.Exception, "invalid player");
            throw new Exception("invalid player");
        }
        if (p == null)
        {
            Logger.Log(Severity.Exception, "player cant be null");
            throw new Exception("player cant be null");
        }

        int maxCips = 0;
        foreach (Player p1 in players) if (p1.CurrentBet > maxCips) maxCips = p1.CurrentBet;
        if (p.CurrentBet >maxCips)
        {
            Logger.Log(Severity.Exception, "player cant call, he have the high bet!");
            throw new Exception("player cant call, he have the high bet!");
        }
        int callAmount = maxCips - p.CurrentBet;
        SetBet(p, callAmount, false);

        int amount = p.CurrentBet;
        bool allCall = true;
        foreach (Player p1 in players) if (p1.CurrentBet != amount)
            {
                allCall = false;
                break;
            }

        if (allCall)
        {

            if (gameStatus==gameStatus.preFlop) DealCommunityFirst();

            else if (gameStatus == gameStatus.flop) DealCommunitySecond();

            else if (gameStatus == gameStatus.turn) DealCommunityThird();

            else if (gameStatus == gameStatus.river) CalcWinnersChips();
        }
        return this;
      }

    public Room SetBet(Player p, int bet,Boolean smallBlind)
    {
       if(!players.Contains(p))
        {
            Logger.Log(Severity.Exception, "invalid player");
            throw new Exception("invalid player");
        }
        if (p == null)
        {
            Logger.Log(Severity.Exception, "player cant be null");
            throw new Exception("player cant be null");
        }

        if ((bet < gamePreferences.minBet&&!smallBlind)||(smallBlind&&bet!=gamePreferences.minBet/2))
        {
            Logger.Log(Severity.Error, "cant bet less then min bet");
            throw new Exception("cant bet less then min bet");
        }

        if (gamePreferences.gameType == Gametype.NoLimit && p.betInThisRound && p.previousRaise > bet) // no limit mode
        {
            Logger.Log(Severity.Error, "cant bet less then previous raise in no limit mode");
            throw new Exception("cant bet less then previous raise in no limit mode");
        }

        if (gamePreferences.gameType == Gametype.limit) // limit mode
        {
            if (gameStatus==gameStatus.preFlop || gameStatus==gameStatus.flop)  //pre flop & flop
            {
               if(bet!= gamePreferences.minBet)
                {
                    Logger.Log(Severity.Error, "in pre flop/flop in limit mode bet must be equal to big blind");
                    throw new Exception("in pre flop/flop in limit mode bet must be equal to big blind");
                }
            }

            if (gameStatus==gameStatus.turn||gameStatus==gameStatus.river)  //turn & river
            {
                if (bet*2 != gamePreferences.minBet)
                {
                    Logger.Log(Severity.Error, "in pre turn/river in limit mode bet must be equal to 2*big blind");
                    throw new Exception("in pre turn/river in limit mode bet must be equal to 2*big blind");
                }
            }
        }

        if (gamePreferences.gameType == Gametype.PotLimit)// limit pot
        {
            int pot = 0;
            foreach (Player p1 in players) pot += p1.CurrentBet;
            if (bet > pot)
            {
                Logger.Log(Severity.Error, "in limit pot mode bet must lower then pot");
                throw new Exception("in limit pot mode bet must lower then pot");
            }
        }

        p.SetBet(bet);
        Replayer.Save(gameReplay, turn, players, pot, null, null);
        return this;
    }

    public void ExitRoom(String player)
    {
        if (IsOn)
        {
            Logger.Log(Severity.Error, "cant exit while game is on");
            throw new Exception("cant exit while game is on");
        }

        if (player == null)
        {
            Logger.Log(Severity.Exception, "cant exit from room player is invalid");
            throw new Exception("cant exit from room player is invalid");
        }

        Boolean found = false;
        foreach (Player p in players) if (p.Name.Equals(player)) found = true;
        if (!found)
        {
            Logger.Log(Severity.Exception, "cant exit from room player is not found");
            throw new Exception("cant exit from room player is not found");
        }

         foreach (Player p in players)
            if (p.Name.Equals(player))
            {
                p.User.chipsAmount += p.ChipsAmount;
                players.Remove(p);
                break;
            }

        if (players.Count == 0)
        {
            GameCenter.GetGameCenter().DeleteRoom(name);
            Logger.Log(Severity.Action, "last player exited, room closed");
            //Logger.Log(Severity.Exception, "cant exit from room, player is last player");
            //throw new Exception("cant exit from room, player is last player");
        }

    }

    public List<Player> Winners()
    {  
        List<Player> winners = new List<Player>();
        foreach (Player p in players)
        {
            if (!p.Folded)
            {
                List<Card> hand = p.Hand.ToList();
                hand.AddRange(communityCards.ToList());
                p.StrongestHand = HandCalculator(hand);
            }
            else p.StrongestHand = new HandStrength(0, HandRank.FOLD, p.Hand.ToList());

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
        if (communityCards[4] == null)
        {
            Logger.Log(Severity.Exception, "game is not over");
            throw new Exception("game is not over");
        }
        if (!IsOn)
        {
            Logger.Log(Severity.Error, "The game has not yet started");
            throw new Exception("The game has not yet started");
        }

        List<Player> winners = Winners();

        Replayer.Save(gameReplay, turn, players, pot, communityCards, "end of turn");
        Logger.Log(Severity.Action, "the winners in room" + name +"is"+PlayersToString(winners));
        foreach(Player p in winners)
        {
            p.User.wins++;
            if (p.User.wins == GameCenter.GetGameCenter().EXPCriteria && p.User.Rank < 10) // change to game center field
            {
                p.User.Rank++;
                p.User.wins = 0;   
            }
        }
        int totalChips = 0;
        foreach (Player p in players) totalChips += p.CurrentBet;
        int ChipsForPlayer = totalChips / winners.Count;
        foreach (Player p in winners) p.ChipsAmount += ChipsForPlayer;
        Logger.Log(Severity.Action, "current status in room" + name + "is" + PlayersToString(players));

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

    public void CleanGame()
    {
        foreach (Player p in players) { p.Hand[0] = null; p.Hand[1] = null; p.UndoFold();}
        for (int i = 0; i < 5; i++) communityCards[i] = null;
    }

    public void NextTurn()
    {
        Player zero = players[0];
        for(int i=0; i < players.Count-1; i++) players[i] = players[i + 1];
        players[players.Count-1] = zero;
        turn++;
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
                if (tempAscending == 4 && SumListCard(ascending) < SumListCard(TempOrderd))
                {
                    ascending = TempOrderd;
                }
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

    public int CalculateHandValue(List<Card> hand, int boost)
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

    public Room Fold(Player p)
    {
        Boolean allFolded = true;
        foreach(Player p1 in players)
            if (!p1.Folded)
            {
                allFolded = false;
                break;
            }
        if (allFolded)
        {
            Logger.Log(Severity.Exception, "player cant Fold, all players are folded");
            throw new Exception("player cant Fold, all players are folded");
        }
        if (p == null)
        {
            Logger.Log(Severity.Exception, "player cant be null");
            throw new Exception("player cant be null");
        }

        if (!players.Contains(p))
        {
            Logger.Log(Severity.Exception, "invalid player");
            throw new Exception("invalid player");
        }

        if (p.Folded)
        {
            Logger.Log(Severity.Exception, "player already folded");
            throw new Exception("player already folded");
        }

        p.Fold();
        Logger.Log(Severity.Action, "player "+p.Name+" folded");
        Replayer.Save(gameReplay, turn, players, pot, null, null);
        return this;
    }

    public Player GetPlayer(string name)
    {
        Player ans = null;
        if(name == null)
        {
            Logger.Log(Severity.Exception, "name cant be null");
            throw new Exception("player name be null");
        }

        Boolean found = false;
        foreach (Player p in players) if (p.Name.Equals(name)) found = true;
        if (!found)
        {
            Logger.Log(Severity.Exception, "player is not found");
            throw new Exception("player is not found");
        }

        foreach (Player p in players) if (p.Name.Equals(name)) ans = p;

        return ans;
    }

    


}
