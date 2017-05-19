using System;
using System.Collections.Generic;

namespace TexasHoldem.Users
{
    public interface IUser
    {
        List<Tuple<string, string>> Notifications { get; set; }
        int League { get; set; }
        int Wins { get; set; }
        int ChipsAmount { get; set; }
        int NumOfGames { get; set; }
        void AddNotification(string Room, string notif);
        void RemoveNotification(string Room, string notif);
        void SetPassword(string password);
        string GetPassword();
        void SetUsername(string username);
        string GetUsername();
        void SetAvatar(string avatarPath);
        string GetAvatar();
        void SetEmail(string email);
        string GetEmail();
    }
}