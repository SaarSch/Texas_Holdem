using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TexasHoldem.GamePrefrences;
using TexasHoldem.GameReplay;
using TexasHoldem.Loggers;
using TexasHoldem.Notifications;
using TexasHoldem.Users;

namespace TexasHoldem.Game
{
    public enum HandRank
    {
        RoyalFlush,
        StraightFlush,
        FourOfAKind,
        FullHouse,
        Flush,
        Straight,
        ThreeOfAKind,
        TwoPair,
        Pair,
        HighCard,
        Fold
    }

    public enum GameStatus
    {
        PreFlop,
        Flop,
        Turn,
        River, 
    }

    public class Room
    {
        public bool IsOn;
        public List<User> SpectateUsers = new List<User>();
        public List<Player> Players = new List<Player>(8);
        public Deck Deck = new Deck();
        public Card[] CommunityCards = new Card[5];
        public string Name;
        public int league;
        public IPreferences GamePreferences;
        public bool Flop;
        public int Pot = 0;
        public readonly string GameReplay;
        private int _turn = 1;
        public GameStatus GameStatus;

        public const int MinNameLength = 4;
        public const int MaxNameLength = 30;

        public Room(string name, Player creator, IPreferences gamePreferences)
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

            if (creator.User.ChipsAmount < gamePreferences.GetMinBet() || (creator.User.ChipsAmount < gamePreferences.GetChipPolicy() && gamePreferences.GetChipPolicy() > 0) || creator.User.ChipsAmount < gamePreferences.GetBuyInPolicy())
            {
                Logger.Log(Severity.Error, "player chips amount is too low to join");
                throw new Exception("player chips amount is too low to join");
            }

            if (gamePreferences.GetChipPolicy() == 0)
            {
                creator.ChipsAmount = creator.User.ChipsAmount;
                creator.User.ChipsAmount = 0;
            }

            else
            {
                creator.ChipsAmount = gamePreferences.GetChipPolicy();
                creator.User.ChipsAmount -= gamePreferences.GetChipPolicy();
            }

            GamePreferences = gamePreferences;
            Players.Add(creator);
            if (!Regex.IsMatch(name, "^[a-zA-Z0-9 ]*$"))
            {
                Logger.Log(Severity.Error, "room name contains illegal characters");
                throw new Exception("room name contains illegal characters");
            }
            if (name.Length > MaxNameLength || name.Length < MinNameLength)
            {
                Logger.Log(Severity.Error, "room name must be between 4 and 30 characters long");
                throw new Exception("room name too short / long");
            }
            Name = name;
            league = creator.User.league;

            GameReplay = Replayer.CreateReplay();
            Logger.Log(Severity.Action, "new room was created room  name="+name+" rank="+league );
        }

        public Room AddPlayer(Player p)
        {
            foreach(var p1 in Players)
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
            if (Players.Count > GamePreferences.GetMaxPlayers())
            {
                Logger.Log(Severity.Exception, "room is full, cant add the player");
                throw new Exception("room is full");
            }
            if (p.User.league != league     && p.User.league!=-1)
        {
            Logger.Log(Severity.Error, "player is in diffrent league join");
            throw new Exception("player is in diffrent league join");
        }
            if (p.User.ChipsAmount < GamePreferences.GetMinBet() || (p.User.ChipsAmount < GamePreferences.GetChipPolicy() && GamePreferences.GetChipPolicy() > 0)|| p.User.ChipsAmount<GamePreferences.GetBuyInPolicy())
            {
                Logger.Log(Severity.Error, "player chips amount is too low to join");
                throw new Exception("player chips amount is too low to join");
            }

            if (GamePreferences.GetChipPolicy() == 0)
            {
                p.ChipsAmount = p.User.ChipsAmount;
                p.User.ChipsAmount = 0;
            }
            else
            {
                p.ChipsAmount = GamePreferences.GetChipPolicy();
                p.User.ChipsAmount -= GamePreferences.GetChipPolicy();
            }
            Players.Add(p);
            Logger.Log(Severity.Action, "new player joined the room: room name=" + Name + "player name=" + p.Name);
            return this;
        }
  
        public void Spectate(User user)
        {
            if (!GamePreferences.GetSpectating())
            {

                Logger.Log(Severity.Exception, "cant spectate at this room");
                throw new Exception("cant spectate at this room");
            }

            if(user is null)
            {
                Logger.Log(Severity.Exception, "cant add a null user to the room");
                throw new Exception("null user");
            }
            SpectateUsers.Add(user);
        }

        public void DealTwo()
        {
            foreach (var p in Players)
            {

                p.SetCards(Deck.Draw(), Deck.Draw());
                Logger.Log(Severity.Action, "player"+p.Name+"got 2 cards:" +p.Hand[0]+p.Hand[1]);
            }
        }

        private bool AllFold()
        {
            foreach (var p in Players)
            {
                if (!p.Folded) return p.Folded;
            }
            return true;
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
        
            if (GameStatus!=GameStatus.PreFlop)
            {
                Logger.Log(Severity.Error, "Already distributed first 3 community cards");
                throw new Exception("Already distributed community cards");
            }

            CommunityCards[0] = Deck.Draw();
            CommunityCards[1] = Deck.Draw();
            CommunityCards[2] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.Flop;

            Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "the flop");
            Logger.Log(Severity.Action, "3 community cards dealed room name=" + Name + "community cards:" + CommunityCards[0] + CommunityCards[1] + CommunityCards[2]);
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
      
            if (GameStatus!=GameStatus.Flop)
            {
                Logger.Log(Severity.Error, "Already distributed 4 community cards");
                throw new Exception("Already distributed community cards");
            }
            CommunityCards[3] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.Turn;

            Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "the turn");
            Logger.Log(Severity.Action, "1 community card dealed room name=" + Name + "community cards:"+ CommunityCards[0] + CommunityCards[1] + CommunityCards[2]+ CommunityCards[3]);
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
            if (GameStatus != GameStatus.Turn)
            {
                Logger.Log(Severity.Error, "Already distributed 5 community cards");
                throw new Exception("Already distributed community cards");
            }
            CommunityCards[4] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.River;

            Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "the river");
            Logger.Log(Severity.Action, "1 community card dealed room name=" + Name + "community cards:" + CommunityCards[0] + CommunityCards[1] + CommunityCards[2] + CommunityCards[3]+ CommunityCards[4]);
        }

        public Room StartGame()
        {
            if (Players.Count < GamePreferences.GetMinPlayers())
            {
                p.User.league = p.User.wins;
                Logger.Log(Severity.Error, "cant play with less then min players");
                throw new Exception("cant play with less then min players");
            }
            if (IsOn)
            {
                Logger.Log(Severity.Exception, "game is already started");
                throw new Exception("game alerady started");
            }

            foreach(var p in Players)
            {
                p.User.NumOfGames++;
                if (p.User.NumOfGames == 11)
                {
                    p.User.league = p.User.Wins;
                }
            }

            GameStatus = GameStatus.PreFlop;
            IsOn = true;
            var smallBlind = GamePreferences.GetMinBet() / 2;
            Deck = new Deck();

            // 0 = dealer 1=small blind 2=big blind
            Replayer.Save(GameReplay, _turn, Players, Pot, null, "start of turn");
            if (Players.Count == 2)
            {
                Logger.Log(Severity.Action, "new game started in room " + Name + " dealer and small blind-" + Players[0]+ "big blind-"+Players[1]);
                SetBet(Players[0], smallBlind,true);
                SetBet(Players[1], GamePreferences.GetMinBet(),false);
            }
            else
            {
                Logger.Log(Severity.Action, "new game started in room"+Name+" dealer" + Players[0]+ "small blind-" + Players[1] + "big blind-" + Players[2] +PlayersToString(Players));
                SetBet(Players[1], smallBlind,true);
                SetBet(Players[2], GamePreferences.GetMinBet(),false);
            }
            DealTwo();
            return this;
        }

        public Room Call(Player p)
        {
            if (!Players.Contains(p))
            {
                Logger.Log(Severity.Exception, "invalid player");
                throw new Exception("invalid player");
            }
            if (p == null)
            {
                Logger.Log(Severity.Exception, "player cant be null");
                throw new Exception("player cant be null");
            }

            var maxCips = 0;
            foreach (var p1 in Players) if (p1.CurrentBet > maxCips) maxCips = p1.CurrentBet;
            if (p.CurrentBet >maxCips)
            {
                Logger.Log(Severity.Exception, "player cant call, he have the high bet!");
                throw new Exception("player cant call, he have the high bet!");
            }
            var callAmount = maxCips - p.CurrentBet;
            SetBet(p, callAmount, false);

            var amount = p.CurrentBet;
            var allCall = true;
            foreach (var p1 in Players) if (p1.CurrentBet != amount)
            {
                allCall = false;
                break;
            }

            if (allCall)
            {
                switch (GameStatus)
                {
                    case GameStatus.PreFlop:
                        DealCommunityFirst();
                        break;
                    case GameStatus.Flop:
                        DealCommunitySecond();
                        break;
                    case GameStatus.Turn:
                        DealCommunityThird();
                        break;
                    case GameStatus.River:
                        CalcWinnersChips();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return this;
        }

        public Room SetBet(Player p, int bet, bool smallBlind)
        {
            if(!Players.Contains(p))
            {
                Logger.Log(Severity.Exception, "invalid player");
                throw new Exception("invalid player");
            }
            if (p == null)
            {
                Logger.Log(Severity.Exception, "player cant be null");
                throw new Exception("player cant be null");
            }

            if ((bet < GamePreferences.GetMinBet()&&!smallBlind)||(smallBlind&&bet!=GamePreferences.GetMinBet()/2))
            {
                Logger.Log(Severity.Error, "cant bet less then min bet");
                throw new Exception("cant bet less then min bet");
            }

            if (GamePreferences.GetGameType() == Gametype.NoLimit && p.BetInThisRound && p.PreviousRaise > bet) // no limit mode
            {
                Logger.Log(Severity.Error, "cant bet less then previous raise in no limit mode");
                throw new Exception("cant bet less then previous raise in no limit mode");
            }

            if (GamePreferences.GetGameType() == Gametype.Limit) // limit mode
            {
                if (GameStatus==GameStatus.PreFlop || GameStatus==GameStatus.Flop)  //pre flop & flop
                {
                    if(bet!= GamePreferences.GetMinBet())
                    {
                        Logger.Log(Severity.Error, "in pre flop/flop in limit mode bet must be equal to big blind");
                        throw new Exception("in pre flop/flop in limit mode bet must be equal to big blind");
                    }
                }

                if (GameStatus==GameStatus.Turn||GameStatus==GameStatus.River)  //turn & river
                {
                    if (bet*2 != GamePreferences.GetMinBet())
                    {
                        Logger.Log(Severity.Error, "in pre turn/river in limit mode bet must be equal to 2*big blind");
                        throw new Exception("in pre turn/river in limit mode bet must be equal to 2*big blind");
                    }
                }
            }

            if (GamePreferences.GetGameType() == Gametype.PotLimit)// limit pot
            {
                var pot = 0;
                foreach (var p1 in Players) pot += p1.CurrentBet;
                if (bet > pot)
                {
                    Logger.Log(Severity.Error, "in limit pot mode bet must lower then pot");
                    throw new Exception("in limit pot mode bet must lower then pot");
                }
            }

            p.SetBet(bet);
            Replayer.Save(GameReplay, _turn, Players, Pot, null, null);
            return this;
        }

        public void ExitRoom(string player)
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

            var found = false;
            foreach (var p in Players) if (p.Name.Equals(player)) found = true;
            if (!found)
            {
                Logger.Log(Severity.Exception, "cant exit from room player is not found");
                throw new Exception("cant exit from room player is not found");
            }

            foreach (var p in Players)
                if (p.Name.Equals(player))
                {
                    p.User.ChipsAmount += p.ChipsAmount;
                    Players.Remove(p);
                    break;
                }

            if (Players.Count == 0)
            {
                GameCenter.GetGameCenter().DeleteRoom(Name);
                Logger.Log(Severity.Action, "last player exited, room closed");
                //Logger.Log(Severity.Exception, "cant exit from room, player is last player");
                //throw new Exception("cant exit from room, player is last player");
            }

        }

        public List<Player> Winners()
        {  
            var winners = new List<Player>();
            foreach (var p in Players)
            {
                if (!p.Folded)
                {
                    var hand = p.Hand.ToList();
                    hand.AddRange(CommunityCards.ToList());
                    p.StrongestHand = HandCalculator(hand);
                }
                else p.StrongestHand = new HandStrength(0, HandRank.Fold, p.Hand.ToList());

            }
            var maxHand = 0;
            foreach (var p in Players)  if (p.StrongestHand.HandStrongessValue > maxHand) maxHand = p.StrongestHand.HandStrongessValue;
            foreach (var p in Players) if (p.StrongestHand.HandStrongessValue == maxHand) winners.Add(p);
            return winners;
        }

        private string PlayersToString(List<Player> players)
        {
            var playersNames = "Players:";
            foreach (var p in players) playersNames += p.ToString();
            return playersNames;
        }

        public void CalcWinnersChips()
        {
            if (CommunityCards[4] == null)
            {
                Logger.Log(Severity.Exception, "game is not over");
                throw new Exception("game is not over");
            }
            if (!IsOn)
            {
                Logger.Log(Severity.Error, "The game has not yet started");
                throw new Exception("The game has not yet started");
            }

            var winners = Winners();

            Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "end of turn");
            Logger.Log(Severity.Action, "the winners in room" + Name +"is"+PlayersToString(winners));
            foreach(var p in winners)
            {
                p.User.Wins++;
                if (p.User.Wins == GameCenter.GetGameCenter().ExpCriteria && p.User.Rank < GameCenter.MaxRank)
                {
                    p.User.Rank++;
                    p.User.Wins = 0;   
                }
            }
            var totalChips = 0;
            foreach (var p in Players) totalChips += p.CurrentBet;
            var chipsForPlayer = totalChips / winners.Count;
            foreach (var p in winners) p.ChipsAmount += chipsForPlayer;
            Logger.Log(Severity.Action, "current status in room" + Name + "is" + PlayersToString(Players));

            CleanGame(); 
            IsOn = false;
            NextTurn();
        }

        public void NotifyRoom(string message)
        {
            p.User.wins++;
            if (message is null)
            {
                Logger.Log(Severity.Error, "cant send null mesege");
                throw new Exception("cant send null message");
            }
            var roomUsers = new List<User>();

            foreach (var p in Players) roomUsers.Add(p.User);
            roomUsers.AddRange(SpectateUsers);
            Notifier.Instance.Notify(roomUsers, message);
        }

        public void CleanGame()
        {
            foreach (var p in Players) { p.Hand[0] = null; p.Hand[1] = null; p.UndoFold();}
            for (var i = 0; i < 5; i++) CommunityCards[i] = null;
        }

        public void NextTurn()
        {
            var zero = Players[0];
            for(var i=0; i < Players.Count-1; i++) Players[i] = Players[i + 1];
            Players[Players.Count-1] = zero;
            _turn++;
        }

        public HandStrength HandCalculator(List<Card> cards)
        {
            int handValue;
            HandRank handRank;
            var hand = new List<Card>();
            var orderByValue = cards.OrderBy(card => card.Value).ToList();
            var boost = (int)Math.Pow(10, 6);

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
            if (sameShapeList.Count < 5) sameShapeList = cards.Where(card => card.Type == CardType.Diamonds).OrderBy(card => card.Value).ToList();
            if (sameShapeList.Count < 5) sameShapeList = cards.Where(card => card.Type == CardType.Hearts).OrderBy(card => card.Value).ToList();
            if (sameShapeList.Count < 5) sameShapeList = cards.Where(card => card.Type == CardType.Spades).OrderBy(card => card.Value).ToList();

            //Look for ascending
            var ascending = new List<Card>();
            for (var j = 0; j < 6; j++)
            {
            
                for (var q = j + 1; q < 7; q++)
                {
                    var tempOrderd = new List<Card>();
                    tempOrderd.AddRange(orderByValue);
                    tempOrderd.RemoveAt(q);
                    tempOrderd.RemoveAt(j);
               
                    var tempAscending = 0;
                    for (var m = 0; m < 4; m++)
                    {
                        if (tempOrderd[m].Value + 1 == tempOrderd[m + 1].Value) tempAscending++;
                    }
                    if (tempAscending == 4 && SumListCard(ascending) < SumListCard(tempOrderd))
                    {
                        ascending = tempOrderd;
                    }
                }
            }

            //Decide Hand
            var temp = IsStraightFlush(@ascending, sameShapeList);
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
            else switch (pairsList.Count)
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
            return new HandStrength(handValue, handRank, hand);
        }

        public int CalculateHandValue(List<Card> hand, int boost)
        {
            var ans = boost;
            for (var i = 0; i < 5; i++)
            {
                ans = ans + (int)Math.Pow(10, i) * hand.ElementAt(i).Value;
            }
            return ans;
        }

        private List<Card> IsStraightFlush(List<Card> ascending, List<Card> similarShape)
        {
            if (ascending.Count < 5 || similarShape.Count < 5) return null;
            return @ascending.Count == 5 ? IsStrightFlushHelper(@ascending, similarShape) : null;
        }

        private List<Card> IsStrightFlushHelper(List<Card> ascending, List<Card> similarShape)
        {
            var hand = new List<Card>();
            hand.AddRange(ascending);
            ascending.RemoveAll(similarShape.Contains);
            if (ascending.Count == 0) return hand;
            return null;
        }

        private int SumListCard(List<Card> cards)
        {
            var sum = 0;
            for (var i = 0; i < cards.Count;i++) sum += cards[i].Value;
            return sum;
        }

        public Room Fold(Player p)
        {
            var allFolded = true;
            foreach(var p1 in Players)
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

            if (!Players.Contains(p))
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
            Replayer.Save(GameReplay, _turn, Players, Pot, null, null);
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

            var found = false;
            foreach (var p in Players) if (p.Name.Equals(name)) found = true;
            if (!found)
            {
                Logger.Log(Severity.Exception, "player is not found");
                throw new Exception("player is not found");
            }

            foreach (var p in Players) if (p.Name.Equals(name)) ans = p;

            return ans;
        }

    


    }
}