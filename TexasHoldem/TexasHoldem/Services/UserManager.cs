using System;
using System.Collections.Generic;
using TexasHoldem.Users;

namespace TexasHoldem.Services
{
    public class UserManager
    {
        private readonly GameCenter _gameCenter;

        public UserManager()
        {
            _gameCenter = GameCenter.GetGameCenter();
        }

        public void Register(string username, string password) // UC 1
        {
            _gameCenter.Register(username, password);
        }

        public User Login(string username, string password) // UC 2
        {
            return _gameCenter.Login(username, password);
        }

        public void Logout(string username) // UC 3
        {
            _gameCenter.Logout(username);
        }

        public void EditProfileUsername(string username, string newusername) // UC 4
        {
            _gameCenter.EditUser(username, newusername, null, null, null);
        }

        public void EditProfilePassword(string username, string newPassword) // UC 4
        {
            _gameCenter.EditUser(username, null, newPassword, null, null);
        }

        public void EditProfileEmail(string username, string newEmail) // UC 4
        {
            _gameCenter.EditUser(username, null, null, null, newEmail);
        }

        public void EditProfileAvatarPath(string username, string newAvatarPath) // UC 4
        {
            _gameCenter.EditUser(username, null, null, newAvatarPath, null);
        }
        public void EditUser(string username, string newusername, string newPassword, string newAvatarPath, string newEmail)
        {
            _gameCenter.EditUser(username, newusername, newPassword, newAvatarPath, newEmail);
        }
        public void DeleteUser(string username, string password)
        {
            _gameCenter.DeleteUser(username, password);
        }

        public bool IsUserLoggedIn(string userName, string pass)
        {
            try
            {
                _gameCenter.GetLoggedInUser(userName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void IsUserLoggedInn(string userName)
        {
                _gameCenter.GetLoggedInUser(userName);
        }

        public int GetRank(string userName)
        {
           return _gameCenter.GetUser(userName).League;
        }

        public List<string> GetMessages(string username, string roomName)
        {
            return _gameCenter.GetMessages(username, roomName);
        }
    }
}
