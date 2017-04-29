using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TexasHoldem.Services
{
    class UserManager
    {
        private readonly GameCenter gameCenter;

        public UserManager()
        {
            gameCenter = GameCenter.GetGameCenter();
        }

        public void Register(string username, string password) // UC 1
        {
            gameCenter.Register(username, password);
        }

        public User Login(string username, string password) // UC 2
        {
            return gameCenter.Login(username, password);
        }

        public void Logout(string username) // UC 3
        {
            gameCenter.Logout(username);
        }

        public void EditProfileUsername(string username, string newusername) // UC 4
        {
            gameCenter.EditUser(username, newusername, null, null, null);
        }

        public void EditProfilePassword(string username, string newPassword) // UC 4
        {
            gameCenter.EditUser(username, null, newPassword, null, null);
        }

        public void EditProfileEmail(string username, string newEmail) // UC 4
        {
            gameCenter.EditUser(username, null, null, null, newEmail);
        }

        public void EditProfileAvatarPath(string username, string newAvatarPath) // UC 4
        {
            gameCenter.EditUser(username, null, null, newAvatarPath, null);
        }

        public void DeleteUser(string username, string password)
        {
            gameCenter.DeleteUser(username, password);
        }

        public bool IsUserLoggedIn(string userName, string pass)
        {
            try
            {
                gameCenter.GetLoggedInUser(userName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public int GetRank(string userName)
        {
           return gameCenter.GetUser(userName).Rank;
        }

        public void ChangeRank(string userName, int rank)
        {
            gameCenter.GetUser(userName).Rank = rank;
        }
    }
}
