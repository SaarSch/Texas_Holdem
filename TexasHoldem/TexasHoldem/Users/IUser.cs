using System;
using System.Collections.Generic;

namespace TexasHoldem.Users
{
    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        string AvatarPath { get; set; }
        List<Tuple<string, string>> Notifications { get; set; }
        int League { get; set; }
        int Wins { get; set; }
        int ChipsAmount { get; set; }
        int NumOfGames { get; set; }
        int GrossProfit { get; set; }//cfir
        int AvgGrossProfit { get; set; }//cfir
        int HighestCashGain { get; set; }//cfir
        int AvgCashGain { get; set; }//cfir
        void AddNotification(string room, string notif);
        void RemoveNotification(string room, string notif);
    }
}