using System;
using System.Collections.Generic;
using TexasHoldem.Logics;
using TexasHoldem.Users;

namespace TexasHoldem.Services
{
    public class UserFacade
    {
        private readonly GameCenter _gameCenter;
        private readonly UserLogic _userLogic;

        public UserFacade()
        {
            _gameCenter = GameCenter.GetGameCenter();
            _userLogic = new UserLogic();
        }

        public void Register(string username, string password) // UC 1
        {
            _userLogic.Register(username, password, _gameCenter.Users);
        }

        public IUser Login(string username, string password) // UC 2
        {
            return _userLogic.Login(username, password, _gameCenter.Users);
        }

        public void Logout(string username) // UC 3
        {
            _userLogic.Logout(username, _gameCenter.Users);
        }

        public void EditProfileUsername(string username, string newusername) // UC 4
        {
            _userLogic.EditUser(username, newusername, null, null, null, _gameCenter.Users);
        }

        public void EditProfilePassword(string username, string newPassword) // UC 4
        {
            _userLogic.EditUser(username, null, newPassword, null, null, _gameCenter.Users);
        }

        public void EditProfileEmail(string username, string newEmail) // UC 4
        {
            _userLogic.EditUser(username, null, null, null, newEmail, _gameCenter.Users);
        }

        public void EditProfileAvatarPath(string username, string newAvatarPath) // UC 4
        {
            _userLogic.EditUser(username, null, null, newAvatarPath, null, _gameCenter.Users);
        }
        public void EditUser(string username, string newusername, string newPassword, string newAvatarPath, string newEmail)
        {
            _userLogic.EditUser(username, newusername, newPassword, newAvatarPath, newEmail, _gameCenter.Users);
        }
        public void DeleteUser(string username, string password)
        {
            _userLogic.DeleteUser(username, password, _gameCenter.Users);
        }

        public bool IsUserLoggedIn(string userName, string pass)
        {
            try
            {
                _userLogic.GetLoggedInUser(userName, _gameCenter.Users);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void IsUserLoggedInn(string userName)
        {
                _userLogic.GetLoggedInUser(userName, _gameCenter.Users);
        }

        public int GetRank(string userName)
        {
           return _userLogic.GetUser(userName, _gameCenter.Users).League;
        }

        public List<string> GetMessages(string username, string roomName)
        {
            return _gameCenter.GetMessages(username, roomName);
        }
    }
}
