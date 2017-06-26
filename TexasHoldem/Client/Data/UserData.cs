using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Data
{
    public class UserData : INotifyPropertyChanged
    {
        private string _avatarPath;
        private int _chips;
        private string _email;
        private string _password;
        private int _rank;

        private string _username;
        public string Message;
        public List<string> Messages = new List<string>();
        public string token;
        public int Wins;

        public UserData(string username, string password, string avatarPath, string email, int rank, int wins,
            int chips, string message)
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

        public UserData()
        {
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string AvatarPath
        {
            get => _avatarPath;
            set
            {
                _avatarPath = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public int Rank
        {
            get => _rank;
            set
            {
                _rank = value;
                OnPropertyChanged();
            }
        }

        public int Chips
        {
            get => _chips;
            set
            {
                _chips = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}