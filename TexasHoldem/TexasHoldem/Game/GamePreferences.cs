using System;
using TexasHoldem.Loggers;

namespace TexasHoldem.Game
{
    public enum Gametype
    {
        Limit, 
        NoLimit,
        PotLimit, 
    }

    public class GamePreferences
    {
        private int _buyInPolicy;
        private int _chipPolicy;
        private int _minBet;
        private int _minPlayers;
        private int _maxPlayers;

        public Gametype GameType { get; set; }
        public int BuyInPolicy
        {
            get => _buyInPolicy;
            set
            {
                if (value < 0)
                {
                    var e = new Exception("buy in policy can't be negative");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                _buyInPolicy = value;
            }
        }
        public int ChipPolicy
        {
            get => _chipPolicy;
            set
            {
                if (value < 0)
                {
                    var e = new Exception("Chip policy can't be negative");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                if (_minBet > value && value > 0)
                {
                    var e = new Exception("min bet can't be higher then chip policy");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                _chipPolicy = value;
            }
        }
        public int MinBet
        {
            get => _minBet;
            set
            {
                if (value < MinBetValue)
                {
                    var e = new Exception("Minimum bet can't be less then 2");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                if (value > _chipPolicy && _chipPolicy > 0)
                {
                    var e = new Exception("min bet can't be higher then chip policy");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                _minBet = value;
            }
        }
        public int MinPlayers
        {
            get => _minPlayers;
            set
            {
                if (value < MinPlayersValue)
                {
                    var e = new Exception("Minimum players can't be less then 2");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                _minPlayers = value;
            }
        }
        public int MaxPlayers
        {
            get => _maxPlayers;
            set
            {
                if (value > MaxPlayersValue)
                {
                    var e = new Exception("Maximum players can't be more then 10");
                    Logger.Log(Severity.Exception, e.Message);
                    throw e;
                }
                _maxPlayers = value;
            }
        }
        public bool Spectating { get; set; }

        public const int MinBetValue = 2;
        public const int MinPlayersValue = 2;
        public const int MaxPlayersValue = 10;

        public GamePreferences()
        {
            GameType = Gametype.NoLimit;
            BuyInPolicy = 1;
            ChipPolicy = 0;
            MinBet = 4;
            MinPlayers = 2;
            MaxPlayers = 8;
            Spectating = true;
        }
    }
}