using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TexasHoldem.Game;
using TexasHoldem.Loggers;
using TexasHoldem.Notifications;
using TexasHoldem.Users;

namespace TexasHoldem.Logics
{
    public class MessageLogic
    {
        public void CheckMessage(string message, object sender)
        {
            if (message is null)
            {
                var e = new Exception("cant send null message");
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

        public IRoom PlayerSendMessage(string message, IPlayer sender, IRoom r)
        {
            CheckMessage(message, sender);
            if (!r.Players.Contains(sender))
            {
                var e = new Exception("sender dose not exist");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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

        public IRoom SpectatorsSendMessage(string message, IUser sender, IRoom r)
        {
            CheckMessage(message, sender);
            if (!r.SpectateUsers.Contains(sender))
            {
                Logger.Log(Severity.Error, "sender dose not exist");
                throw new Exception("sender dose not exist");
            }
            var roomUsers = new List<IUser>();
            roomUsers.AddRange(r.SpectateUsers);
            Notifier.Instance.Notify(roomUsers, r.Name, sender.Username + ": " + message);
            return r;
        }

        public IRoom SpectatorWhisper(string message, IUser sender, IUser reciver, IRoom r)
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
            var roomUsers = new List<IUser> { sender, reciver };
            Notifier.Instance.Notify(roomUsers, r.Name, sender.Username + ": " + message);
            return r;
        }

        public IRoom PlayerWhisper(string message, IPlayer sender, IUser reciver, IRoom r)
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
            var roomUsers = new List<IUser> { reciver, sender.User };
            Notifier.Instance.Notify(roomUsers, r.Name, sender.Name + ": " + message);
            return r;
        }

        private bool IsUserIsPlayer(IUser u, IRoom r)
        {
            foreach (var p in r.Players)
            {
                if (p.User == u) return true;
            }
            return false;
        }

        public void NotifyRoom(string message, IRoom r)
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
    }
}
