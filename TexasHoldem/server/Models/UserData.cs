using System.Collections.Generic;

namespace server.Models
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
        public List<string> messages = new List<string>();

        public bool Equals(UserData other)
        {
            if (messages.Count != other.messages.Count) return false;
            for(int i = 0; i < messages.Count; i++)
            {
                if (messages[i] != other.messages[i]) return false;
            }


            return Username == other.Username && Password == other.Password && AvatarPath == other.AvatarPath && Email == other.Email && Rank == other.Rank
                && Wins ==other.Wins && Chips == other.Chips && Message == other.Message;
        }
    }
}