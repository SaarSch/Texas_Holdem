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
            var roomUsers = new List<IUser>();

            foreach (var p in r.Players) roomUsers.Add(p.User);
            roomUsers.AddRange(r.SpectateUsers);
            Notifier.Instance.Notify(roomUsers, r.Name, message);
        }

        public Room PlayerSendMessege(string message, IPlayer sender, Room r)
        {
            if (message is null)
            {
                Logger.Log(Severity.Error, "cant send null mesege");
                throw new Exception("cant send null message");
            }
            if (!CheckMessageValidity(message))
            {
                Logger.Log(Severity.Error, "cant send empty message / curses");
                throw new Exception("cant send empty message / curses");
            }
            if (sender is null)
            {
                Logger.Log(Severity.Error, "sender cant be null");
                throw new Exception("sender cant be null");
            }
            if (!r.Players.Contains(sender))
            {
                Logger.Log(Severity.Error, "sender dose not exist");
                throw new Exception("sender dose not exist");
            }
            var roomUsers = new List<IUser>();
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
            if (message is null)
            {
                Logger.Log(Severity.Error, "cant send null mesege");
                throw new Exception("cant send null message");
            }
            if (!CheckMessageValidity(message))
            {
                Logger.Log(Severity.Error, "cant send empty message / curses");
                throw new Exception("cant send empty message / curses");
            }
            if (sender is null)
            {
                Logger.Log(Severity.Error, "sender cant be null");
                throw new Exception("sender cant be null");
            }
            if (!r.SpectateUsers.Contains(sender))
            {
                Logger.Log(Severity.Error, "sender dose not exist");
                throw new Exception("sender dose not exist");
            }
            var roomUsers = new List<IUser>();
            roomUsers.AddRange(r.SpectateUsers);
            Notifier.Instance.Notify(roomUsers, r.Name, sender.GetUsername() + ": " + message);
            return r;
        }

        public Room SpectatorWisper(string message, User sender, User reciver, Room r)
        {
            if (message is null)
            {
                Logger.Log(Severity.Error, "cant send null mesege");
                throw new Exception("cant send null message");
            }
            if (!CheckMessageValidity(message))
            {
                Logger.Log(Severity.Error, "cant send empty message / curses");
                throw new Exception("cant send empty message / curses");
            }
            if (sender is null)
            {
                Logger.Log(Severity.Error, "sender cant be null");
                throw new Exception("sender cant be null");
            }
            if (reciver is null)
            {
                Logger.Log(Severity.Error, "reciver cant be null");
                throw new Exception("reciver cant be null");
            }
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
            var roomUsers = new List<IUser> { reciver };
            Notifier.Instance.Notify(roomUsers, r.Name, sender.GetUsername() + ": " + message);
            return r;
        }

        public Room PlayerWisper(string message, IPlayer sender, User reciver, Room r)
        {
            if (message is null)
            {
                Logger.Log(Severity.Error, "cant send null mesege");
                throw new Exception("cant send null message");
            }
            if (!CheckMessageValidity(message))
            {
                Logger.Log(Severity.Error, "cant send empty message / curses");
                throw new Exception("cant send empty message / curses");
            }
            if (sender is null)
            {
                Logger.Log(Severity.Error, "sender cant be null");
                throw new Exception("sender cant be null");
            }
            if (reciver is null)
            {
                Logger.Log(Severity.Error, "reciver cant be null");
                throw new Exception("reciver cant be null");
            }
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
            var roomUsers = new List<IUser> { reciver };
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
