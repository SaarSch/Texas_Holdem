using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TexasHoldem;

public class User
{
    private string username;
    private string password;
    private string avatarPath;
    private string email;
 //   public bool Online { get; set; }
    public List<string> Notifications { get; set; }
    public int Rank;
    public int wins;
    public int chipsAmount;

    private List<Player> players; //change to list of Games?
    private List<Room> rooms; //do we need this?

    public User(string username, string password, string avatarPath, string email, int chipsAmount)
    {
        wins = 0;
        Rank = GameCenter.GetGameCenter().DefaultRank;
        SetUsername(username);
        SetPassword(password);
        SetAvatar(avatarPath);
        SetEmail(email);
        Notifications = new List<string>();
        players = new List<Player>();
        rooms = new List<Room>();
        this.chipsAmount = chipsAmount;
    }

    public void AddNotification(string notif)
    {
        if (notif == "")
        {
            throw new Exception("notification is empty.");
        }

        Notifications.Add(notif);
    }

    public void RemoveNotification(string notif)
    {
        if (notif == "")
        {
            throw new Exception("notification is empty.");
        }

        Notifications.Remove(notif);
    }

    public void SetPassword(string password)
    {
        if (password.Length > 12 || password.Length < 8)
        {
            Logger.Log(Severity.Error, "ERROR: Illegal password! Length must be between 8 and 12.");
            throw new Exception("Illegal password! Length must be between 8 and 12.");
        }

        bool hasNonLetterChar = false;
        int i;

        for (i = 0; i < password.Length && !hasNonLetterChar; i++)
        {
            if (password[i] == ' ')
            {
                Logger.Log(Severity.Error, "ERROR in Register: Illegal password! Space is not allowed.");
                throw new Exception("Illegal password! Space is not allowed.");
            }
            if (!Char.IsLetter(password[i]))
            {
                hasNonLetterChar = true;
            }
        }

        if (!hasNonLetterChar)
        {
            Logger.Log(Severity.Error, "ERROR in Register: Illegal password!  Must contain at least 1 non-letter character.");
            throw new Exception("Illegal password! Must contain at least 1 non-letter character.");
        }

        this.password = password;
    }

    public string GetPassword()
    {
        return password;
    }

    public void SetUsername(string username)
    {
        if (username.Length > 12 || username.Length < 8)
        {
            Logger.Log(Severity.Error, "ERROR: Illegal username!Length must be between 8 and 12.");
            throw new Exception("Illegal username! Length must be between 8 and 12.");
        }

        for (int i = 0; i < username.Length; i++)
        {
            if (username[i] == ' ')
            {
                Logger.Log(Severity.Error, "ERROR: Illegal username! Space is not allowed.");
                throw new Exception("Illegal username! Space is not allowed.");
            }
        }

        this.username = username;
    }

    public string GetUsername()
    {
        return username;
    }

    public void SetAvatar(string avatarPath)
    {
        if (avatarPath.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1
            || (!avatarPath.EndsWith(".png") && !avatarPath.EndsWith(".jpg") && !avatarPath.EndsWith(".jpeg"))
            || avatarPath.Contains("virus")
            ||  avatarPath.Contains("VIRUS")) // TODO: add more?
        {
            Logger.Log(Severity.Error, "ERROR: Illegal avatar file! Must be a legal image");
            throw new Exception("Illegal avatar file! Must be a legal image");
        }
        else
        {
            this.avatarPath = avatarPath;
        }
    }

    public string GetAvatar()
    {
        return avatarPath;
    }

    public void SetEmail(string email)
    {
        if (Regex.IsMatch(email, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z")
               && Regex.IsMatch(email, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*"))
        {
            this.email = email;
        }
        else
        {
            Logger.Log(Severity.Error, "ERROR: Illegal email! must be in format: aaa@bbb.ccc");
            throw new Exception("Illegal email! must be in format: aaa@bbb.ccc.");
        }
        
    }

    public string GetEmail()
    {
        return email;
    }
}