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
    }
}