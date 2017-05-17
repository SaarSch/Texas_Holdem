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

        public void Notify(List<User> users, string room, string msg)
        {
            if(msg == "")
            {
                var e = new Exception("message is empty.");
                Logger.Log(Severity.Exception, e.Message);
                throw e;
            }

            var notif = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + msg;

            foreach (var u in users)
            {
                u.AddNotification(room ,notif);
            }
        }
    }
}
