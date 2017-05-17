using System.Collections.Generic;

namespace Server.Models
{
    public class UserData
    {
        public string Username;
        public string Password;
        public string AvatarPath;
        public string Email;
        public int Rank;
        public int Wins;
        public int Chips;
        public string Message;
        public List<string> Messages = new List<string>();

        public bool Equals(UserData other)
        {
            if (Messages.Count != other.Messages.Count) return false;
            for(var i = 0; i < Messages.Count; i++)
            {
                if (Messages[i] != other.Messages[i]) return false;
            }


            return Username == other.Username && Password == other.Password && AvatarPath == other.AvatarPath && Email == other.Email && Rank == other.Rank
                && Wins ==other.Wins && Chips == other.Chips && Message == other.Message;
        }
    }
}