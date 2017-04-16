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

        public void Register(string userName, string password) // UC 1
        {
            gameCenter.Register(userName, password);
        }

        public User Login(string userName, string password) // UC 2
        {
            return gameCenter.Login(userName, password);
        }

        public void Logout(string userName) // UC 3
        {
            gameCenter.Logout(userName);
        }

        public void EditProfile(string userName, string password, string newUsername, string newPassword, string newEmail, string newAvatarPath) // UC 4
        {
            gameCenter.EditUser(userName, password, newUsername, newPassword, newAvatarPath, newEmail);
        }
    }
}
