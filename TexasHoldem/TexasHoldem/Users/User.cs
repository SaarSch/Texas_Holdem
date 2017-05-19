using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TexasHoldem.Exceptions;
using TexasHoldem.Loggers;

namespace TexasHoldem.Users
{
    public class User : IUser
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value.Length > PasswordLengthMax || value.Length < PasswordLengthMin)
                {
                    Exception e = new IllegalPasswordException("Illegal username! Length must be between 8 and 12.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }

                for (var i = 0; i < value.Length; i++)
                {
                    if (value[i] == ' ')
                    {
                        Exception e = new IllegalUsernameException("Illegal username! Space is not allowed.");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                }

                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (value.Length > PasswordLengthMax || value.Length < PasswordLengthMin)
                {
                    Exception e = new IllegalPasswordException("Illegal password! Length must be between 8 and 12.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }

                var hasNonLetterChar = false;
                int i;

                for (i = 0; i < value.Length && !hasNonLetterChar; i++)
                {
                    if (value[i] == ' ')
                    {
                        Exception e = new IllegalPasswordException("Illegal password! Space is not allowed.");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                    if (!char.IsLetter(value[i]))
                    {
                        hasNonLetterChar = true;
                    }
                }

                if (!hasNonLetterChar)
                {
                    Exception e = new IllegalPasswordException("Illegal password! Must contain at least 1 non-letter character.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }

                _password = value;
            }
        }

        private string _avatarPath;
        public string AvatarPath
        {
            get => _avatarPath;
            set
            {
                if (value.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1
                    || (!value.EndsWith(".png") && !value.EndsWith(".jpg") && !value.EndsWith(".jpeg"))
                    || value.Contains("virus")
                    || value.Contains("VIRUS"))
                {
                    Exception e = new IllegalAvatarException("Illegal avatar file! Must be a legal image.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
                _avatarPath = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (Regex.IsMatch(value, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z")
                    && Regex.IsMatch(value, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*"))
                {
                    _email = value;
                }
                else
                {
                    Exception e = new IllegalAvatarException("Illegal email! must be in format: aaa@bbb.ccc.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
            }
        }

        public List<Tuple<string,string>> Notifications { get; set; }
        public int League { get; set; }
        public int Wins { get; set; }
        public int ChipsAmount { get; set; }
        public int NumOfGames { get; set; }

        public const int PasswordLengthMin = 8;
        public const int PasswordLengthMax = 12;

        public User(string username, string password, string avatarPath, string email, int chipsAmount)
        {
            NumOfGames = 0;
            Wins = 0;
            Username = username;
            Password = password;
            AvatarPath = avatarPath;
            Email = email;
            Notifications = new List<Tuple<string, string>>();
            ChipsAmount = chipsAmount;
        }

        public void AddNotification(string room, string notif)
        {
            if (notif == "")
            {
                throw new Exception("notification is empty.");
            }

            Notifications.Add(new Tuple<string, string>(room,notif));
        }

        public void RemoveNotification(string room, string notif)
        {
            if (notif == "")
            {
                throw new Exception("notification is empty.");
            }
            Tuple<string, string> p = null;
            foreach (var p1 in Notifications)
            {
                if (p1.Item1 == room && p1.Item2 == notif)
                    p = p1;
            }

            Notifications.Remove(p);
        }
    }
}