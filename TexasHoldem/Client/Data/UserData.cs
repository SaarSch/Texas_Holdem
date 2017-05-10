using System.Drawing;

namespace Client
{
    public class UserData
    {
        public string username;
        public string password;
        public string avatarPath;
        public string email;
        public int Rank;
        public int wins;
        public int chips;
        public string message;

        public UserData(string username, string password, string avatarPath, string email, int rank, int wins, int chips, string message)
        {
            this.username = username;
            this.password = password;
            this.avatarPath = avatarPath;
            this.email = email;
            Rank = rank;
            this.wins = wins;
            this.chips = chips;
            this.message = message;
        }
    }
}