using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TexasHoldem.Loggers;
using TexasHoldem.Notifications;
using TexasHoldem.Users;

namespace TexasHoldem.Game
{
    public class MessageLogic
    {
        public void NotifyRoom(string message, Room r)
        {
            if (message is null)
            {
                var e = new Exception("can't send null message");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            var roomUsers = new List<User>();

            foreach (var p in r.Players) roomUsers.Add(p.User);
            roomUsers.AddRange(r.SpectateUsers);
            Notifier.Instance.Notify(roomUsers, r.Name, message);
        }

        public void CheckMessage(string message, object sender)
        {
            if (message is null)
            {
                var e = new Exception("cant send null mesege");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            if (!CheckMessageValidity(message))
            {
                var e = new Exception("cant send empty message / curses");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (sender is null)
            {
                var e = new Exception("sender cant be null");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
        }
        public void CheckMessage(string message, object sender, object reciver)
        {
            CheckMessage(message, sender);
            if (reciver is null)
            {
                var e = new Exception("sender cant be null");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
        }

        public Room PlayerSendMessege(string message, Player sender, Room r)
        {
            CheckMessage(message, sender);
            if (!r.Players.Contains(sender))
            {
                var e = new Exception("sender dose not exist");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            var roomUsers = new List<User>();
            foreach (var p in r.Players) roomUsers.Add(p.User);
            roomUsers.AddRange(r.SpectateUsers);
            Notifier.Instance.Notify(roomUsers, r.Name, " " + sender.Name + ": " + message);
            return r;
        }

        private bool CheckMessageValidity(string message)
        {
            var wordFilter = new Regex("(fuck|shit|pussy)");
            return message != "" && !wordFilter.IsMatch(message);
        }

        public Room SpectatorsSendMessege(string message, User sender, Room r)
        {
            CheckMessage(message, sender);
            if (!r.SpectateUsers.Contains(sender))
            {
                Logger.Log(Severity.Error, "sender dose not exist");
                throw new Exception("sender dose not exist");
            }
            var roomUsers = new List<User>();
            roomUsers.AddRange(r.SpectateUsers);
            Notifier.Instance.Notify(roomUsers, r.Name, sender.GetUsername() + ": " + message);
            return r;
        }

        public Room SpectatorWisper(string message, User sender, User reciver, Room r)
        {
            CheckMessage(message, sender,reciver);
            if (!r.SpectateUsers.Contains(sender))
            {
                Logger.Log(Severity.Error, "sender dose not exist");
                throw new Exception("sender dose not exist");
            }
            if (!r.SpectateUsers.Contains(reciver))
            {
                Logger.Log(Severity.Error, "reciver dose not exist");
                throw new Exception("reciver dose not exist");
            }
            var roomUsers = new List<User> { reciver };
            Notifier.Instance.Notify(roomUsers, r.Name, sender.GetUsername() + ": " + message);
            return r;
        }

        public Room PlayerWisper(string message, Player sender, User reciver, Room r)
        {
            CheckMessage(message, sender, reciver);         
            if (!r.Players.Contains(sender))
            {
                Logger.Log(Severity.Error, "sender dose not exist");
                throw new Exception("sender dose not exist");
            }
            if (!r.SpectateUsers.Contains(reciver) && !IsUserIsPlayer(reciver, r))
            {
                Logger.Log(Severity.Error, "reciver dose not exist");
                throw new Exception("reciver dose not exist");
            }
            var roomUsers = new List<User> { reciver };
            Notifier.Instance.Notify(roomUsers, r.Name, sender.Name + ": " + message);
            return r;
        }

        private bool IsUserIsPlayer(User u, Room r)
        {
            foreach (var p in r.Players)
            {
                if (p.User == u) return true;
            }
            return false;
        }
    }
}
