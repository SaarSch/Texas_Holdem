﻿using System;
using System.Collections.Generic;

public class Notifier
{
    private static Notifier instance;
    private Notifier() { }
    public static Notifier Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Notifier();
            }
            return instance;
        }
    }

    public void Notify(List<User> users, string msg)
    {
        if(msg == "")
        {
            throw new Exception("message is empty.");
        }

        string notif = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + msg;

        foreach (User u in users)
        {
            u.AddNotification(notif);

            /*
            if (u.Online)
            {
                u.ShowNotification(notif);
                u.RemoveNotification(notif);
            }
            */
        }
    }
}