using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TexasHoldem.Exceptions;
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
        River
    }

    public class Room : IRoom
    {
        public const int MinNameLength = 4;
        public const int MaxNameLength = 15;

        public Room(string name, IPlayer creator, GamePreferences gamePreferences)
        {
            if (!Regex.IsMatch(name, "^[a-zA-Z0-9 ]*$"))
            {
                Exception e = new IllegalRoomNameException("Room name contains illegal characters");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (name.Length > MaxNameLength || name.Length < MinNameLength)
            {
                Exception e = new IllegalRoomNameException("Room name must be between 4 and 15 characters long");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            //chipPolicy- amount of chips eace player is given, 0== all in.  
            //minBet- the minimum bet
            //buy-in- the minimum chip to join the game

            if (creator.User.ChipsAmount < gamePreferences.BuyInPolicy)

            {
                var e = new Exception("Player chips amount is low the the buy in");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            creator.User.ChipsAmount -= gamePreferences.BuyInPolicy;

            if (creator.User.ChipsAmount < gamePreferences.MinBet ||
                creator.User.ChipsAmount < gamePreferences.ChipPolicy && gamePreferences.ChipPolicy > 0)
            {
                creator.User.ChipsAmount += GamePreferences.BuyInPolicy;
                var e = new Exception("Player chips amount is too low to join");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (gamePreferences.GameType == Gametype.Limit && creator.User.ChipsAmount < 6 * gamePreferences.MinBet)
            {
                creator.User.ChipsAmount += GamePreferences.BuyInPolicy;
                var e = new Exception("Limit mode, player chips amount is too low to join");
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
			
            HandLogic = new HandLogic();
            Logger.Log(Severity.Action, "New room was created room  name=" + name + " rank=" + League);
        }

        public bool IsOn { get; set; }
        public List<IUser> SpectateUsers { get; set; } = new List<IUser>();
        public List<IPlayer> Players { get; set; } = new List<IPlayer>(8);
        public Deck Deck { get; set; } = new Deck();
        public Card[] CommunityCards { get; set; } = new Card[5];
        public string Name { get; set; }
        public int League { get; set; }
        public GamePreferences GamePreferences { get; set; }
        public bool Flop { get; set; }
        public int Pot { get; set; } = 0;
        public GameStatus GameStatus { get; set; }
        public int CurrentTurn { get; set; }
        public string CurrentWinners { get; set; }

        public HandLogic HandLogic { get; }

        public bool HasPlayer(string name)
        {
            foreach (var p in Players)
                if (p.Name == name)
                    return true;
            return false;
        }

        public Room AddPlayer(IPlayer p)
        {
            if (p == null)
            {
                var e = new Exception("Can't add a null player to the room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            foreach (var p1 in Players)
                if (p1.Name.Equals(p.Name))
                {
                    var e = new Exception("Can't join, player name is already exist");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
            foreach (var u in SpectateUsers)
                if (u.Username.Equals(p.User.Username))
                {
                    var e = new Exception("Can't join, player name is already exist");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
            if (IsOn)
            {
                var e = new Exception("Can't join, game is on");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (Players.Count + 1 > GamePreferences.MaxPlayers)
            {
                var e = new Exception("Room is full, can't add the player");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (p.User.League != League && p.User.League != -1)
            {
                var e = new Exception("Player is in diffrent league join");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if ( p.User.ChipsAmount < GamePreferences.BuyInPolicy)
            {
                var e = new Exception("Player chips amount is low than buy in");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            p.User.ChipsAmount -= GamePreferences.BuyInPolicy;


            if (p.User.ChipsAmount < GamePreferences.MinBet ||
                p.User.ChipsAmount < GamePreferences.ChipPolicy && GamePreferences.ChipPolicy > 0)
            {
                p.User.ChipsAmount += GamePreferences.BuyInPolicy;
                var e = new Exception("Player chips amount is too low to join");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (GamePreferences.GameType == Gametype.Limit && p.User.ChipsAmount < 6 * GamePreferences.MinBet)
            {
                p.User.ChipsAmount += GamePreferences.BuyInPolicy;
                var e = new Exception("Limit mode, player chips amount is too low to join");
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
            Logger.Log(Severity.Action, "New player joined the room: room name=" + Name + "player name=" + p.Name);
            return this;
        }

        public bool CanJoin(IUser user)
        {
            if (user == null)
            {
                return false;
            }
            if(this.IsInRoom(user.Username))
            {
                return false;
            }
            if (IsOn)
            {
                return false;
            }
            if (Players.Count + 1 > GamePreferences.MaxPlayers)
            {
                return false;
            }
            if (user.League != League && user.League != -1)
            {
                return false;
            }

            if (user.ChipsAmount < GamePreferences.BuyInPolicy)
            {
                return false;
            }

             var tmp = user.ChipsAmount - GamePreferences.BuyInPolicy;


            if (tmp < GamePreferences.MinBet ||
                (tmp < GamePreferences.ChipPolicy && GamePreferences.ChipPolicy > 0))
            {
                return false;
            }

            if (GamePreferences.GameType == Gametype.Limit && tmp < 6 * GamePreferences.MinBet)
            {
                return false;
            }

            return true;
        }

        public void Spectate(IUser user)
        {
            if (!GamePreferences.Spectating)
            {
                var e = new Exception("Can't spectate at this room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (user is null)
            {
                var e = new Exception("Can't add a null user to the room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            SpectateUsers.Add(user);
        }

        public IRoom ExitSpectator(IUser user)
        {
            if (user is null)
            {
                var e = new Exception("Can't add a null user to the room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (!SpectateUsers.Contains(user))
            {
                var e = new Exception("User is not spectate this room");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            SpectateUsers.Remove(user);
            return this;
        }

        public void DealTwo()
        {
            foreach (var p in Players)
            {
                p.SetCards(Deck.Draw(), Deck.Draw());
                Logger.Log(Severity.Action, "Player " + p.Name + " got 2 cards: " + p.Hand[0] + p.Hand[1]);
            }
        }

        public void DealCommunityFirst()
        {
            CommunityCards[0] = Deck.Draw();
            CommunityCards[1] = Deck.Draw();
            CommunityCards[2] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.Flop;
			
            Logger.Log(Severity.Action,
                "3 community cards dealed room name=" + Name + "community cards:" + CommunityCards[0] +
                CommunityCards[1] + CommunityCards[2]);
        }

        public void DealCommunitySecond()
        {
            CommunityCards[3] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.Turn;
			
            Logger.Log(Severity.Action,
                "1 community card dealed room name=" + Name + "community cards:" + CommunityCards[0] +
                CommunityCards[1] + CommunityCards[2] + CommunityCards[3]);
        }

        public void DealCommunityThird()
        {
            CommunityCards[4] = Deck.Draw();
            foreach (var p in Players) p.BetInThisRound = false;
            GameStatus = GameStatus.River;
			
            Logger.Log(Severity.Action,
                "1 community card dealed room name=" + Name + "community cards:" + CommunityCards[0] +
                CommunityCards[1] + CommunityCards[2] + CommunityCards[3] + CommunityCards[4]);
        }

        public Room StartGame()
        {
            foreach (var p in Players)
                if (!CanBeInRoom(p))
                {
                    var e = new Exception("Can not start because player " + p.Name + " can not be in this room!");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
            if (Players.Count < GamePreferences.MinPlayers)
            {
                var e = new Exception("Can not play with less than min players");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (IsOn)
            {
                var e = new Exception("Game has already started");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            for (var i = 0; i < 5; i++)
                CommunityCards[i] = null;
            foreach (var p in Players)
            {
                p.UndoFold();
                p.Hand[0] = null;
                p.Hand[1] = null;
                p.User.NumOfGames++;
                if (p.User.NumOfGames == 11)
                    p.User.League = p.User.Wins;
            }
            CurrentWinners = "";
            Pot = 0;
            GameStatus = GameStatus.PreFlop;
            IsOn = true;
            CurrentTurn = 0;
            var smallBlind = GamePreferences.MinBet / 2;
            Deck = new Deck();

            // 0 = dealer 1=small blind 2=big blind
            if (Players.Count == 2)
            {
                Logger.Log(Severity.Action,
                    "New game started in room " + Name + " dealer and small blind-" + Players[0] + "big blind-" +
                    Players[1]);
                SetBet(Players[0], smallBlind, true);
                SetBet(Players[1], GamePreferences.MinBet, true);
            }
            else
            {
                Logger.Log(Severity.Action,
                    "New game started in room" + Name + " dealer" + Players[0] + "small blind-" + Players[1] +
                    "big blind-" + Players[2] + PlayersToString(Players));
                SetBet(Players[1], smallBlind, true);
                SetBet(Players[2], GamePreferences.MinBet, true);
            }
            DealTwo();
            return this;
        }

        public Room Call(IPlayer p)
        {
            if (p == null)
            {
                var e = new Exception("Player can't be null");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (!Players.Contains(p))
            {
                var e = new Exception("Invalid player");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }


            var maxCips = 0;
            foreach (var p1 in Players) if (p1.CurrentBet > maxCips) maxCips = p1.CurrentBet;
            if (p.CurrentBet > maxCips)
            {
                var e = new Exception("Player can't call, he has the high bet!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            var callAmount = maxCips - p.CurrentBet;
            if (callAmount != 0)
            {
                SetBet(p, callAmount, true);
            }
            else
            {
                NextTurn();
                p.BetInThisRound = true;
            }


            var amount = p.CurrentBet;
            var allCall = true;

            foreach (var p1 in Players)
                if ((p1.CurrentBet != amount || !p1.BetInThisRound) && !p1.Folded)
                {
                    allCall = false;
                    break;
                }

            if (allCall)
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
                        CalcWinnersChips(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            return this;
        }

        public Room SetBet(IPlayer p, int bet, bool smallBlind)
        {
            var maxCips = 0;
            foreach (var p1 in Players) if (p1.CurrentBet > maxCips) maxCips = p1.CurrentBet;
            if (p == null)
            {
                var e = new Exception("Player can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (!Players.Contains(p))
            {
                var e = new Exception("Invalid player");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (bet + p.CurrentBet < maxCips)
            {
                var e = new Exception("Can not bet lower than the higest bet");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (bet < GamePreferences.MinBet && !smallBlind)
            {
                var e = new IllegalBetException("Can not bet less than min bet");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (!smallBlind && GamePreferences.GameType == Gametype.NoLimit && p.BetInThisRound &&
                p.PreviousRaise > bet) // no limit mode
            {
                var e = new IllegalBetException("Can not bet less than previous raise in no limit mode");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (!smallBlind && GamePreferences.GameType == Gametype.Limit) // limit mode
            {
                if (GameStatus == GameStatus.PreFlop || GameStatus == GameStatus.Flop) //pre flop & flop
                    if (bet != GamePreferences.MinBet)
                    {
                        var e =
                            new IllegalBetException("In pre flop/flop in limit mode bet must be equal to " + GamePreferences.MinBet);
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }

                if (GameStatus == GameStatus.Turn || GameStatus == GameStatus.River) //turn & river
                    if (bet  != GamePreferences.MinBet * 2)
                    {
                        var e = new IllegalBetException(
                            "In pre turn/river in limit mode bet must be equal to "+ GamePreferences.MinBet * 2);
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
            }

            if (!smallBlind && GamePreferences.GameType == Gametype.PotLimit) // limit pot
            {
                var pot = 0;
                foreach (var p1 in Players) pot += p1.CurrentBet;
                if (bet > pot)
                {
                    var e = new IllegalBetException("In limit pot mode bet must lower than " +pot);
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
            }

            Pot += bet;
            p.SetBet(bet);
			NextPlayer();
            return this;
        }

        public void ExitRoom(string player)
        {
            if (player == null)
            {
                var e = new Exception("Can not exit from room, player is invalid");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            var found = false;
            foreach (var p in Players) if (p.Name.Equals(player)) found = true;
            if (!found)
            {
                var e = new Exception("Can not exit from room, player is not found");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            for (var i = 0; i < Players.Count; i++)
                if (Players[i].Name.Equals(player))
                {
                    Players[i].User.ChipsAmount += Players[i].ChipsAmount;
                    for (var j = 0; j < Players[i].User.Notifications.Count; j++)
                        if (Players[i].User.Notifications[j].Item1 == Name)
                            Players[i].User.Notifications.Remove(Players[i].User.Notifications[j]);
                    if (!IsOn)
                    {
                        Players.Remove(Players[i]);
                    }
                    else
                    {
                        Players[i].Exit = true;
                        Fold(Players[i]);
                    }

                    break;
                }

            if (Players.Count == 0)
            {
                GameCenter.GetGameCenter().DeleteRoom(Name);
                Logger.Log(Severity.Action, "Last player exited, room closed");
            }
        }

        public List<IPlayer> Winners()
        {
            var winners = new List<IPlayer>();
            foreach (var p in Players)
                if (!p.Folded)
                {
                    // var hand = p.Hand.ToList();
                    var cardsmoq = new List<Card>();
                    var formoq = p.Hand.ToList();
                    foreach (var c in formoq)
                        cardsmoq.Add((Card) c);
                    cardsmoq.AddRange(CommunityCards.ToList());
                    p.StrongestHand = HandLogic.HandCalculator(cardsmoq);
                }
                else
                {
                    var cardsmoq = new List<Card>();
                    var formoq = p.Hand.ToList();
                    foreach (var c in formoq)
                        cardsmoq.Add((Card) c);
                    p.StrongestHand = new HandStrength(0, HandRank.Fold, cardsmoq);
                }
            var maxHand = 0;
            foreach (var p in Players)
                if (p.StrongestHand.HandStrongessValue > maxHand) maxHand = p.StrongestHand.HandStrongessValue;
            foreach (var p in Players) if (p.StrongestHand.HandStrongessValue == maxHand) winners.Add(p);
            return winners;
        }

        public void CalcWinnersChips(bool folded)
        {
            var winners = new List<IPlayer>();
            if (folded)
            {
                foreach (var p in Players)
                    if (!p.Folded) winners.Add(p);
            }
            else winners = Winners();
			
            Logger.Log(Severity.Action, "The winners in room " + Name + " is " + PlayersToString(winners));
            if (winners.Count > 1) CurrentWinners += "The winners are: ";
            else CurrentWinners += "The winner is: ";
            foreach (var p in winners)
            {
                CurrentWinners += p.Name + " ";
                p.User.Wins++;
            }
            var totalChips = 0;
            foreach (var p in Players) totalChips += p.CurrentBet;
            var chipsForPlayer = totalChips / winners.Count;
            foreach (var p in winners)
            {
                p.ChipsAmount += chipsForPlayer;
                p.User.GrossProfit += chipsForPlayer;
                p.User.AvgGrossProfit = p.User.GrossProfit / p.User.Wins;
                if (p.User.HighestCashGain < chipsForPlayer) p.User.HighestCashGain = p.User.HighestCashGain;
            }
            Logger.Log(Severity.Action, "Current status in room " + Name + " is " + PlayersToString(Players));

            CleanGame();
            IsOn = false;
            NextTurn();
        }

        public void CleanGame()
        {
            GameStatus = GameStatus.PreFlop;
            for (var i = 0; i < Players.Count; i++)
                if (Players[i].Exit)
                {
                    for (var j = 0; j < Players[i].User.Notifications.Count; j++)
                        if (Players[i].User.Notifications[j].Item1 == Name)
                            Players[i].User.Notifications.Remove(Players[i].User.Notifications[j]);
                    Players.Remove(Players[i]);
                }
            foreach (var p in Players)
            {
                p.User.AvgCashGain = p.User.GrossProfit / p.User.NumOfGames;
                p.CurrentBet = 0;
            }
            CurrentTurn = 0;
        }

        public void NextTurn()
        {
            var zero = Players[0];
            for (var i = 0; i < Players.Count - 1; i++) Players[i] = Players[i + 1];
            Players[Players.Count - 1] = zero;
            //_turn++;
        }

        public Room Fold(IPlayer p)
        {
            if (p == null)
            {
                var e = new Exception("Player can't be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            if (!Players.Contains(p))
            {
                var e = new Exception("Invalid player");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }
            if (p.Folded)
            {
                var e = new Exception("Player already folded");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            var folded = 0;
            foreach (var p1 in Players)
                if (p1.Folded)
                    folded++;
            if (Players.Count - 2 == folded)
            {
                p.Fold();
                Logger.Log(Severity.Action, "Player " + p.Name + " folded");
                CalcWinnersChips(true);
                return this;
            }

            p.Fold();
            Logger.Log(Severity.Action, "Player " + p.Name + " folded");
		
            NextPlayer();
            return this;
        }

        public IPlayer GetPlayer(string name)
        {
            IPlayer ans = null;
            if (name == null)
            {
                var e = new Exception("Name cant be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            var found = false;
            foreach (var p in Players) if (p.Name.Equals(name)) found = true;
            if (!found)
            {
                var e = new Exception("Player is not found");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            foreach (var p in Players) if (p.Name.Equals(name)) ans = p;

            return ans;
        }

        public IUser GetSpectator(string name)
        {
            IUser ans = null;
            if (name == null)
            {
                var e = new Exception("Name cant be null");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            var found = false;
            foreach (var s in SpectateUsers) if (s.Username.Equals(name)) found = true;
            if (!found)
            {
                var e = new Exception("Spectator is not found");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            foreach (var s in SpectateUsers) if (s.Username.Equals(name)) ans = s;

            return ans;
        }

        public bool IsInRoom(string name)
        {
            foreach (var p in Players)
                if (p.User.Username == name) return true;

            foreach (var u in SpectateUsers)
                if (u.Username == name) return true;

            return false;
        }

        public bool Isspectator(string username)
        {
            foreach (var u in SpectateUsers)
                if (u.Username == username) return true;

            return false;
        }

        private bool CanBeInRoom(IPlayer p)
        {
            if (p.ChipsAmount < GamePreferences.MinBet)
                return false;
            
            if (GamePreferences.GameType == Gametype.Limit && p.ChipsAmount < 6 * GamePreferences.MinBet)
            {
                return false;
            }
                
            return true;
        }

        public void ExitSpectator(string username)
        {
            for (var i = 0; i < SpectateUsers.Count; i++)
                if (SpectateUsers[i].Username == username)
                    SpectateUsers.Remove(SpectateUsers[i]);
        }

        private bool AllFold()
        {
            foreach (var p in Players)
                if (!p.Folded) return p.Folded;
            return true;
        }

        private void NextPlayer()
        {
            for (var j = 0; j < Players.Count - 1; j++)
            {
                var i = (CurrentTurn + 1 + j) % Players.Count;
                if (!Players[i].Folded)
                {
                    CurrentTurn = i;
                    break;
                }
            }
        }

        private string PlayersToString(List<IPlayer> players)
        {
            var playersNames = "Players:";
            foreach (var p in players) playersNames += p.ToString();
            return playersNames;
        }
    }
}