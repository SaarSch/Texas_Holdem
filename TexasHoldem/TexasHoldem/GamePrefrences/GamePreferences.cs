using System;
using TexasHoldem.Loggers;

namespace TexasHoldem.GamePrefrences
{
    public enum Gametype
    {
        Limit, 
        NoLimit,
        PotLimit, 
    }

    public class GamePreferences : IPreferences
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
                    Logger.Log(Severity.Error, "buy in policy can't be negative");
                    throw new Exception("buy in policy can't be negative");
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
                    Logger.Log(Severity.Error, "Chip policy can't be negative");
                    throw new Exception("Chip policy can't be negative");
                }
                if (_minBet > value && value > 0)
                {
                    Logger.Log(Severity.Error, "min bet can't be higher then chip policy");
                    throw new Exception("min bet can't be higher then chip policy");
                }
                _chipPolicy = value;
            }
        }
        public int MinBet
        {
            get => _minBet;
            set
            {
                if (value < 2)
                {
                    Logger.Log(Severity.Error, "Minimum bet can't be less then 2");
                    throw new Exception("Minimum bet can't be less then 2");
                }
                if (value > _chipPolicy && _chipPolicy > 0)
                {
                    Logger.Log(Severity.Error, "min bet can't be higher then chip policy");
                    throw new Exception("min bet can't be higher then chip policy");
                }
                _minBet = value;
            }
        }
        public int MinPlayers
        {
            get => _minPlayers;
            set
            {
                if (value < 2)
                {
                    Logger.Log(Severity.Error, "Minimum players can't be less then 2");
                    throw new Exception("Minimum players can't be less then 2");
                }
                _minPlayers = value;
            }
        }
        public int MaxPlayers
        {
            get => _maxPlayers;
            set
            {
                if (value > 10)
                {
                    Logger.Log(Severity.Error, "Maximum players can't be more then 10");
                    throw new Exception("Maximum players can't be more then 10");
                }
                _maxPlayers = value;
            }
        }
        public bool Spectating { get; set; }

        public GamePreferences()
        {
            SetGameType(Gametype.NoLimit);
            SetBuyInPolicy(1);
            SetChipPolicy(0);
            SetMinBet(4);
            SetMinPlayers(2);
            SetMaxPlayers(8);
            SetSpectating(true);
        }

        public void SetBuyInPolicy(int bp)
        {
            BuyInPolicy = bp;
        }

        public void SetChipPolicy(int cp)
        {
            ChipPolicy = cp;
        }

        public void SetGameType(Gametype gt)
        {
            GameType = gt;
        }

        public void SetMaxPlayers(int mp)
        {
            MaxPlayers = mp;
        }

        public void SetMinPlayers(int mp)
        {
            MinPlayers = mp;
        }

        public void SetMinBet(int mb)
        {
            MinBet = mb;
        }

        public void SetSpectating(bool s)
        {
            Spectating = s;
        }

        public int GetBuyInPolicy()
        {
            return BuyInPolicy;
        }

        public int GetChipPolicy()
        {
            return ChipPolicy;
        }

        public Gametype GetGameType()
        {
            return GameType;
        }

        public int GetMaxPlayers()
        {
            return MaxPlayers;
        }

        public int GetMinPlayers()
        {
            return MinPlayers;
        }

        public int GetMinBet()
        {
            return MinBet;
        }

        public bool GetSpectating()
        {
            return Spectating;
        }
    }
}