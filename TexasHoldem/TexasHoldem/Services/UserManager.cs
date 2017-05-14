using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.Users;

namespace TexasHoldem.Services
{
    public class UserManager
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
        public void EditUser(string username, string newusername, string newPassword, string newAvatarPath, string newEmail)
        {
            gameCenter.EditUser(username, newusername, newPassword, newAvatarPath, newEmail);
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

        public void IsUserLoggedInn(string userName, string pass)
        {
                gameCenter.GetLoggedInUser(userName);
        }

        public int GetRank(string userName)
        {
           return gameCenter.GetUser(userName).League;
        }

        public void ChangeRank(string userName, int rank)
        {
            gameCenter.GetUser(userName).League = rank;
        }
    }
}
