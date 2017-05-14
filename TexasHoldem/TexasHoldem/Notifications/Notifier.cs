using System;
using System.Collections.Generic;
using TexasHoldem.Loggers;
using TexasHoldem.Users;

namespace TexasHoldem.Notifications
{
    public class Notifier
    {
        private static Notifier _instance;
        private Notifier() { }
        public static Notifier Instance => _instance ?? (_instance = new Notifier());

        public void Notify(List<User> users, string msg)
        {
            if(msg == "")
            {
                Logger.Log(Severity.Exception, "message is empty.");
                throw new Exception("message is empty.");
            }

            var notif = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + msg;

            foreach (var u in users)
            {
                u.AddNotification(notif);
            }
        }
    }
}
