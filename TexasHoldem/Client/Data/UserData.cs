using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Client.Data
{
    public class UserData : INotifyPropertyChanged
    {
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

        private string _username;
        private string _password;
        private string _avatarPath;
        private string _email;
        private int _chips;
        private int _rank;
        public int Wins;
        public int Chips
        {
            get => _chips;
            set
            {
                _chips = value;
                OnPropertyChanged();
            }
        }
        public string Message;
        public List<string> Messages = new List<string>();
        public string token;

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
        public UserData()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}