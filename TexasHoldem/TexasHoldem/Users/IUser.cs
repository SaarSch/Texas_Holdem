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
        void AddNotification(string room, string notif);
        void RemoveNotification(string room, string notif);
    }
}