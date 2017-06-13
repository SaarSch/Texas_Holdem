using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TexasHoldem.Exceptions;
using TexasHoldem.Loggers;
using System.ComponentModel.DataAnnotations;

namespace TexasHoldem.Users
{
    public class User : IUser
    {
        public int GrossProfit { get; set; }//cfir
        public int AvgGrossProfit { get; set; }//cfir
        public int HighestCashGain { get; set; }//cfir
        public int AvgCashGain { get; set; }//cfir
        private string _username;
	    [Key]
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
        public static void CanSetPass(string value)
        {  
            if (value.Length > PasswordLengthMax || value.Length < PasswordLengthMin)
            {
                Exception e = new IllegalPasswordException("Illegal password! Length must be between 8 and 12.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            var hasNonLetterChar = false;
            for (int i = 0; i < value.Length && !hasNonLetterChar; i++)
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
        }

        public static void CanSetMail(string value)
        {
            if (Regex.IsMatch(value, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z")
                   && Regex.IsMatch(value, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*"))
            {
                return;
            }
            else
            {
                Exception e = new IllegalAvatarException("Illegal email! must be in format: aaa@bbb.ccc.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }     
        }

        public static void CanSetUserName(string value)
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
        }

        private string _password;
        public string Password
        {
	        get => _password;//Crypto.Decrypt(_password);

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

	            _password = value; //Crypto.Encrypt(value);
            }
        }

        private string _avatarPath = "Resources/profilePicture.png";
        public string AvatarPath
        {
            get => _avatarPath;
            set
            {
                if ((!value.EndsWith(".png") && !value.EndsWith(".jpg") && !value.EndsWith(".jpeg"))
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
        private IUser ans;

        public User() // TODO: remove? used for queries

	    {
			League = -1;
		    NumOfGames = 0;
		    Wins = 0;
		    Username = "abcd1234";
		    Password = "abcd1234";
		    AvatarPath = "default.png";
		    Email = "mail@gmail.com";
		    Notifications = new List<Tuple<string, string>>();
		    ChipsAmount = 5000;
	        NumOfGames=0;

	        GrossProfit=0;
	        AvgGrossProfit=0;
	        HighestCashGain=0;
	        AvgCashGain=0;
        }

        public User(string username, string password, string avatarPath, string email, int chipsAmount)
        {
            League = -1;
            NumOfGames = 0;
            Wins = 0;
            Username = username;
            Password = password;
            AvatarPath = avatarPath;
            Email = email;
            Notifications = new List<Tuple<string, string>>();
            ChipsAmount = chipsAmount;

            GrossProfit = 0;
            AvgGrossProfit = 0;
            HighestCashGain = 0;
            AvgCashGain = 0;
        }

        public User(IUser ans,string username)
        {
            League = ans.League;
            NumOfGames = ans.NumOfGames;
            Wins = ans.Wins;
            Username = username;
            Password = ans.Password;
            AvatarPath = ans.AvatarPath;
            Email = ans.Email;
            Notifications = new List<Tuple<string, string>>();
            Notifications.AddRange(ans.Notifications);
            ChipsAmount = ans.ChipsAmount;

            GrossProfit = ans.GrossProfit;
            AvgGrossProfit = ans.AvgGrossProfit;
            HighestCashGain = ans.HighestCashGain;
            AvgCashGain = ans.AvgCashGain;
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

        public override bool Equals(Object o)
        {
            if (!(o is User))
                return false;
            User other = (User)o;
            if (other.Username == this.Username && other.Password == this.Password)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return (int)(this.Username[0]) + (int)(this.Username[1]) + (int)(this.Password[0]) + (int)(this.Username[1]);
        //    return base.GetHashCode();
        }
    }
}