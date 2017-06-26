using System.Collections.Generic;

namespace Server.Models
{
    public class UserData
    {
        public string AvatarPath;
        public int Chips;
        public string Email;
        public string Message;
        public List<string> Messages = new List<string>();
        public string Password;
        public int Rank;
        public string token;
        public string Username;
        public int Wins;

        public bool Equals(UserData other)
        {
            if (Messages.Count != other.Messages.Count) return false;
            for (var i = 0; i < Messages.Count; i++)
                if (Messages[i] != other.Messages[i]) return false;


            return Username == other.Username && Password == other.Password && AvatarPath == other.AvatarPath &&
                   Email == other.Email && Rank == other.Rank
                   && Wins == other.Wins && Chips == other.Chips && Message == other.Message;
        }
    }
}