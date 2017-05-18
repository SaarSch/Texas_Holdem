using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TexasHoldem.Exceptions;
using TexasHoldem.GameReplay;
using TexasHoldem.Loggers;
using TexasHoldem.Logics;
using TexasHoldem.Users;

namespace TexasHoldem.Game
{
    public enum GameStatus
    {
        PreFlop,
        Flop,
        Turn,
        River, 
    }

    public class Room
    {
        public int id;
        public bool IsOn;
        public List<User> SpectateUsers = new List<User>();
        public List<Player> Players = new List<Player>(8);
        public Deck Deck = new Deck();
        public Card[] CommunityCards = new Card[5];
        public string Name;
        public int League;
        public GamePreferences GamePreferences;
        public bool Flop;
        public int Pot = 0;
        public readonly string GameReplay;
        private int _turn = 1;
        public GameStatus GameStatus;
        public int CurrentTurn;

        public HandLogic HandLogic { get; }

        public const int MinNameLength = 4;
        public const int MaxNameLength = 30;

        public Room(string name, Player creator, GamePreferences gamePreferences)
        {
            if (name == null)
            {
                Exception e = new ArgumentException("room name can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (!Regex.IsMatch(name, "^[a-zA-Z0-9 ]*$"))
            {
                Exception e = new IllegalRoomNameException("room name contains illegal characters");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (name.Length > MaxNameLength || name.Length < MinNameLength)
            {
                Exception e = new IllegalRoomNameException("room name must be between 4 and 30 characters long");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (creator == null)
            {
                Exception e = new ArgumentException("creator player can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (gamePreferences == null)
            {
                Exception e = new ArgumentException("game preferences can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            //chipPolicy- amount of chips eace player is given, 0== all in.  
            //minBet- the minimum bet
            //buy-in- the minimum chip to join the game

            if (creator.User.ChipsAmount < gamePreferences.MinBet || (creator.User.ChipsAmount < gamePreferences.ChipPolicy && gamePreferences.ChipPolicy > 0) || creator.User.ChipsAmount < gamePreferences.BuyInPolicy)
            {
                var e = new Exception("player chips amount is too low to join");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (gamePreferences.ChipPolicy == 0)
            {
                creator.ChipsAmount = creator.User.ChipsAmount;
                creator.User.ChipsAmount = 0;
            }

            else
            {
                creator.ChipsAmount = gamePreferences.ChipPolicy;
                creator.User.ChipsAmount -= gamePreferences.ChipPolicy;
            }

            GamePreferences = gamePreferences;
            Players.Add(creator);
            Name = name;
            League = creator.User.League;

         //   GameReplay = Replayer.CreateReplay();
            HandLogic = new HandLogic();
            Logger.Log(Severity.Action, "new room was created room  name="+name+" rank="+League );
        }

        public bool HasPlayer(string name)
        {
            foreach (var p in Players)
            {
                if (p.Name == name)
                    return true;
            }
            return false;
        }

        public Room AddPlayer(Player p)
        {
            foreach(var p1 in Players)
            {
                if (p1.Name.Equals(p.Name))
                {
                    var e = new Exception("can't join, player name is already exist");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
            }
            foreach (var u in SpectateUsers)
            {
                if (u.GetUsername().Equals(p.Name))
                {
                    var e = new Exception("can't join, player name is already exist");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
            }
            if (IsOn)
            {
                var e = new Exception("can't join, game is on");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (p == null)
            {
                var e = new Exception("can't add a null player to the room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (Players.Count > GamePreferences.MaxPlayers)
            {
                var e = new Exception("room is full, can't add the player");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (p.User.League != League && p.User.League!=-1)
            {
                var e = new Exception("player is in diffrent league join");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (p.User.ChipsAmount < GamePreferences.MinBet || (p.User.ChipsAmount < GamePreferences.ChipPolicy && GamePreferences.ChipPolicy > 0)|| p.User.ChipsAmount<GamePreferences.BuyInPolicy)
            {
                var e = new Exception("player chips amount is too low to join");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (GamePreferences.ChipPolicy == 0)
            {
                p.ChipsAmount = p.User.ChipsAmount;
                p.User.ChipsAmount = 0;
            }
            else
            {
                p.ChipsAmount = GamePreferences.ChipPolicy;
                p.User.ChipsAmount -= GamePreferences.ChipPolicy;
            }
            Players.Add(p);
            Logger.Log(Severity.Action, "new player joined the room: room name=" + Name + "player name=" + p.Name);
            return this;
        }
  
        public void Spectate(User user)
        {
            if (!GamePreferences.Spectating)
            {
                var e = new Exception("can't spectate at this room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if(user is null)
            {
                var e = new Exception("can't add a null user to the room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            foreach (var p in Players)
            {
                if (p.Name == user.GetUsername())
                {
                    var e = new Exception("can't spectate at this room");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
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
                var e = new Exception("The game has not yet started, can't deal community first");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (AllFold())
            {
                var e = new Exception("all players folded, no need to deal community cards");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
        
            if (GameStatus!=GameStatus.PreFlop)
            {
                var e = new Exception("Already distributed first 3 community cards");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            CommunityCards[0] = Deck.Draw();
            CommunityCards[1] = Deck.Draw();
            CommunityCards[2] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.Flop;

         //   Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "the flop");
            Logger.Log(Severity.Action, "3 community cards dealed room name=" + Name + "community cards:" + CommunityCards[0] + CommunityCards[1] + CommunityCards[2]);
        }

        public void DealCommunitySecond()
        {
            if (!IsOn)
            {
                var e = new Exception("The game has not yet started, can't deal community second");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (AllFold())
            {
                var e = new Exception("all players folded, no need to deal community cards");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
      
            if (GameStatus!=GameStatus.Flop)
            {
                var e = new Exception("Already distributed 4 community cards");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            CommunityCards[3] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.Turn;

        //    Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "the turn");
            Logger.Log(Severity.Action, "1 community card dealed room name=" + Name + "community cards:"+ CommunityCards[0] + CommunityCards[1] + CommunityCards[2]+ CommunityCards[3]);
        }

        public void DealCommunityThird()
        {
            if (!IsOn)
            {
                var e = new Exception("The game has not yet started, can't deal community third");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (AllFold())
            {
                var e = new Exception("all players folded, no need to deal community cards");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (GameStatus != GameStatus.Turn)
            {
                var e = new Exception("Already distributed 5 community cards");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            CommunityCards[4] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.River;

        //    Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "the river");
            Logger.Log(Severity.Action, "1 community card dealed room name=" + Name + "community cards:" + CommunityCards[0] + CommunityCards[1] + CommunityCards[2] + CommunityCards[3]+ CommunityCards[4]);
        }

        public Room StartGame()
        {
            if (Players.Count < GamePreferences.MinPlayers)
            {
                var e = new Exception("can't play with less then min players");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (IsOn)
            {
                var e = new Exception("game has already started");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            foreach(var p in Players)
            {
                p.User.NumOfGames++;
                if (p.User.NumOfGames == 11)
                {
                    p.User.League = p.User.Wins;
                }
            }

            GameStatus = GameStatus.PreFlop;
            IsOn = true;
            CurrentTurn = 0;
            var smallBlind = GamePreferences.MinBet / 2;
            Deck = new Deck();

            // 0 = dealer 1=small blind 2=big blind
         //   Replayer.Save(GameReplay, _turn, Players, Pot, null, "start of turn");
            if (Players.Count == 2)
            {
                Logger.Log(Severity.Action, "new game started in room " + Name + " dealer and small blind-" + Players[0]+ "big blind-"+Players[1]);
                SetBet(Players[0], smallBlind,true);
                SetBet(Players[1], GamePreferences.MinBet,false);
            }
            else
            {
                Logger.Log(Severity.Action, "new game started in room"+Name+" dealer" + Players[0]+ "small blind-" + Players[1] + "big blind-" + Players[2] +PlayersToString(Players));
                SetBet(Players[1], smallBlind,true);
                SetBet(Players[2], GamePreferences.MinBet,false);
            }
            DealTwo();
            return this;
        }

        private void NextPlayer()
        {
         
           for (int j= 0; j < Players.Count-1; j++)
            {
                int i = (CurrentTurn+1+j) % Players.Count;
                if (!Players[i].Folded)
                {
                    CurrentTurn = i;
                    break;
                }
            }
        }

        public Room Call(Player p)
        {
            if (!Players.Contains(p))
            {
                var e = new Exception("invalid player");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (p == null)
            {
                var e = new Exception("player can't be null");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            var maxCips = 0;
            foreach (var p1 in Players) if (p1.CurrentBet > maxCips) maxCips = p1.CurrentBet;
            if (p.CurrentBet >maxCips)
            {
                var e = new Exception("player can't call, he has the high bet!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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
            NextPlayer();
            return this;
        }

        public Room SetBet(Player p, int bet, bool smallBlind)
        {
            if(!Players.Contains(p))
            {
                var e = new Exception("invalid player");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (p == null)
            {
                var e = new Exception("player can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if ((bet < GamePreferences.MinBet&&!smallBlind)||(smallBlind&&bet!=GamePreferences.MinBet/2))
            {
                var e = new IllegalBetException("can't bet less then min bet");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (GamePreferences.GameType == Gametype.NoLimit && p.BetInThisRound && p.PreviousRaise > bet) // no limit mode
            {
                var e = new IllegalBetException("can't bet less then previous raise in no limit mode");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (GamePreferences.GameType == Gametype.Limit) // limit mode
            {
                if (GameStatus==GameStatus.PreFlop || GameStatus==GameStatus.Flop)  //pre flop & flop
                {
                    if(bet!= GamePreferences.MinBet)
                    {
                        var e = new IllegalBetException("in pre flop/flop in limit mode bet must be equal to big blind");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                }

                if (GameStatus==GameStatus.Turn||GameStatus==GameStatus.River)  //turn & river
                {
                    if (bet*2 != GamePreferences.MinBet)
                    {
                        var e = new IllegalBetException("in pre turn/river in limit mode bet must be equal to 2*big blind");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                }
            }

            if (GamePreferences.GameType == Gametype.PotLimit)// limit pot
            {
                var pot = 0;
                foreach (var p1 in Players) pot += p1.CurrentBet;
                if (bet > pot)
                {
                    var e = new IllegalBetException("in limit pot mode bet must lower then pot");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
            }

            p.SetBet(bet);

        //    Replayer.Save(GameReplay, _turn, Players, Pot, null, null);

            return this;
        }

        public void ExitRoom(string player)
        {
            if (IsOn)
            {
                var e = new Exception("can't exit while game is on");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (player == null)
            {
                var e = new Exception("can't exit from room, player is invalid");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            var found = false;
            foreach (var p in Players) if (p.Name.Equals(player)) found = true;
            if (!found)
            {
                var e = new Exception("can't exit from room, player is not found");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
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
                    p.StrongestHand = HandLogic.HandCalculator(hand);
                }
                else p.StrongestHand = new HandStrength(0, HandRank.Fold, p.Hand.ToList());

            }
            var maxHand = 0;
            foreach (var p in Players) if (p.StrongestHand.HandStrongessValue > maxHand) maxHand = p.StrongestHand.HandStrongessValue;
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
 
            var winners = Winners();

        //    Replayer.Save(GameReplay, _turn, Players, Pot, CommunityCards, "end of turn");
            Logger.Log(Severity.Action, "the winners in room" + Name +"is"+PlayersToString(winners));
            foreach(var p in winners)
            {
                p.User.Wins++;
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

        public Room Fold(Player p)
        {
            
            if (p == null)
            {
                var e = new Exception("player can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (!Players.Contains(p))
            {
                var e = new Exception("invalid player");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (p.Folded)
            {
                var e = new Exception("player already folded");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            int folded = 0;
            foreach (var p1 in Players)
            {
                if (p1.Folded)
                {
                    folded++;
                }
            }
            if (Players.Count - 2 == folded) 
            {
                p.Fold();
                Logger.Log(Severity.Action, "player " + p.Name + " folded");
                Replayer.Save(GameReplay, _turn, Players, Pot, null, null);
                CalcWinnersChips();
                return this;
            }

            p.Fold();
            Logger.Log(Severity.Action, "player "+p.Name+" folded");

        //    Replayer.Save(GameReplay, _turn, Players, Pot, null, null);

            return this;
        }

        public Player GetPlayer(string name)
        {
            Player ans = null;
            if(name == null)
            {
                var e = new Exception("name cant be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            var found = false;
            foreach (var p in Players) if (p.Name.Equals(name)) found = true;
            if (!found)
            {
                var e = new Exception("player is not found");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            foreach (var p in Players) if (p.Name.Equals(name)) ans = p;

            return ans;
        }

    }
}