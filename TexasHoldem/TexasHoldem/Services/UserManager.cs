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

        public void EditProfileUsername(string username, string password, string newusername) // UC 4
        {
            gameCenter.EditUser(username, password, newusername);
        }

        public void EditProfilePassword(string username, string password, string newPassword) // UC 4
        {
            gameCenter.EditUser(username, password, null, newPassword);
        }

        public void EditProfileEmail(string username, string password, string newEmail) // UC 4
        {
            gameCenter.EditUser(username, password, null, null, null, newEmail);
        }

        public void EditProfileAvatarPath(string username, string password, string newAvatarPath) // UC 4
        {
            gameCenter.EditUser(username, password, null, null, newAvatarPath);
        }

        public void DeleteUser(string username, string password)
        {
            gameCenter.DeleteUser(username, password);
        }
    }
}
