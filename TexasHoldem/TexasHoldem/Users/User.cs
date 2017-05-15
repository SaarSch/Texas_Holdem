using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TexasHoldem.Exceptions;
using TexasHoldem.Loggers;

namespace TexasHoldem.Users
{
    public class User
    {
        private string _username;
        private string _password;
        private string _avatarPath;
        private string _email;

        public List<Tuple<string,string>> Notifications { get; set; }
        public int League=-1;
        public int Wins;
        public int ChipsAmount;
        public int NumOfGames;

        public const int PasswordLengthMin = 8;
        public const int PasswordLengthMax = 12;

        public User(string username, string password, string avatarPath, string email, int chipsAmount)
        {
            NumOfGames = 0;
            Wins = 0;
            SetUsername(username);
            SetPassword(password);
            SetAvatar(avatarPath);
            SetEmail(email);
            Notifications = new List<Tuple<string, string>>();
            ChipsAmount = chipsAmount;
        }

        public void AddNotification(string Room, string notif)
        {
            if (notif == "")
            {
                throw new Exception("notification is empty.");
            }

            Notifications.Add(new Tuple<string, string>(Room,notif));
        }

        public void RemoveNotification(string Room, string notif)
        {
            if (notif == "")
            {
                throw new Exception("notification is empty.");
            }
            Tuple<string, string> p = null;
            foreach (var p1 in Notifications)
            {
                if (p1.Item1 == Room && p1.Item2 == notif)
                    p = p1;
            }

            Notifications.Remove(p);
        }

        public void SetPassword(string password)
        {
            if (password.Length > PasswordLengthMax || password.Length < PasswordLengthMin)
            {
                Exception e = new IllegalPasswordException("Illegal password! Length must be between 8 and 12.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            var hasNonLetterChar = false;
            int i;

            for (i = 0; i < password.Length && !hasNonLetterChar; i++)
            {
                if (password[i] == ' ')
                {
                    Exception e = new IllegalPasswordException("Illegal password! Space is not allowed.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
                if (!char.IsLetter(password[i]))
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

            _password = password;
        }

        public string GetPassword()
        {
            return _password;
        }

        public void SetUsername(string username)
        {
            if (username.Length > PasswordLengthMax || username.Length < PasswordLengthMin)
            {
                Exception e = new IllegalPasswordException("Illegal UserName! Length must be between 8 and 12.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            for (var i = 0; i < username.Length; i++)
            {
                if (username[i] == ' ')
                {
                    Exception e = new IllegalUsernameException("Illegal username! Space is not allowed.");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
            }

            _username = username;
        }

        public string GetUsername()
        {
            return _username;
        }

        public void SetAvatar(string avatarPath)
        {
            if (avatarPath.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1
                || (!avatarPath.EndsWith(".png") && !avatarPath.EndsWith(".jpg") && !avatarPath.EndsWith(".jpeg"))
                || avatarPath.Contains("virus")
                ||  avatarPath.Contains("VIRUS")) // TODO: add more?
            {
                Exception e = new IllegalAvatarException("Illegal avatar file! Must be a legal image.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            _avatarPath = avatarPath;
        }

        public string GetAvatar()
        {
            return _avatarPath;
        }

        public void SetEmail(string email)
        {
            if (Regex.IsMatch(email, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z")
                && Regex.IsMatch(email, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*"))
            {
                _email = email;
            }
            else
            {
                Exception e = new IllegalAvatarException("Illegal email! must be in format: aaa@bbb.ccc.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
        
        }

        public string GetEmail()
        {
            return _email;
        }
    }
}