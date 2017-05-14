using System.Drawing;

namespace Client
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

        public UserData(string username, string password, string avatarPath, string email, int rank, int wins, int chips, string message)
        {
            Username = username;
            Password = password;
            AvatarPath = avatarPath;
            Email = email;
            Rank = rank;
            Wins = wins;
            Chips = chips;
            Message = message;
        }
    }
}