using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public enum Gametype
    {
        limit, 
        NoLimit,
        PotLimit, 
    }

    public class GamePreferences
    {
        public Gametype gameType;
        public int buyInPolicy;
        public int chipPolicy;
        public int minBet;
        public int minPlayers;
        public int maxPlayers;
        public Boolean spectating;


        public GamePreferences(Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayer, Boolean spectating)
        {
            

            if(buyInPolicy < 0)
            {
                    Logger.Log(Severity.Error, "buy in policy cant be negativ");
                    throw new Exception("buy in policy  cant be negativ");
            }

            if (chipPolicy < 0)
            {
                Logger.Log(Severity.Error, "Chip policy cant be negativ");
                throw new Exception("Chip policy value cant be negativ");
            }

            if (minBet < 2)
            {
                Logger.Log(Severity.Error, "Minimum bet cant be less then 2");
                throw new Exception("Minimum bet cant be less then 2");
            }

            if (minPlayers < 2)
            {
                Logger.Log(Severity.Error, "Minimum players cant be less then 2");
                throw new Exception("Minimum players cant be less then 2");
            }

            if (maxPlayer > 10)
            {
                Logger.Log(Severity.Error, "Maximum players cant be more then 9");
                throw new Exception("Maximum players cant be more then 9");
            }

            if (minBet > chipPolicy &&chipPolicy>0)
            {
                Logger.Log(Severity.Error, "min bet cant be higher the chip policy");
                throw new Exception("min bet cant be higher the chip policy");
            }

            this.gameType= gameType;
            this.buyInPolicy = buyInPolicy;
            this.chipPolicy = chipPolicy;
            this.minBet = minBet;
            this.minPlayers = minPlayers;
            this.maxPlayers = maxPlayer;
            this.spectating = spectating;
        }
    }
