using System;
using System.Collections.Generic;
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

    private List<Player> players; //change to list of Games?
    private List<Room> rooms; //do we need this?

    public User(string username, string password, string avatarPath, string email)
    {
        wins = 0;
        Rank = 0;
        SetUsername(username);
        SetPassword(password);
        SetAvatar(avatarPath);
        SetEmail(email);
        Notifications = new List<string>();
        players = new List<Player>();
        rooms = new List<Room>();
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
        // TODO: real set, with type checking
        this.avatarPath = avatarPath;
    }

    public string GetAvatar()
    {
        return avatarPath;
    }

    public void SetEmail(string email)
    {
        // TODO: real set, with email validity check
        this.email = email;
    }

    public string GetEmail()
    {
        return email;
    }
}
