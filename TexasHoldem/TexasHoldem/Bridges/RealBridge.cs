using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Services;

namespace TexasHoldem.Bridges
{
    public class RealBridge:IBridge
    {
        private UserManager userManager;
        private GameManager gameManager;
        private ReplayManager replayManager;

        public RealBridge()
        {
            userManager = new UserManager();
            gameManager = new GameManager();
            replayManager = new ReplayManager();
        }

        public bool register(string userName, string pass)
        {
            try
            {
                userManager.Register(userName, pass);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool isUserExist(string userName)
        {
            try
            {
                userManager.Login(userName, userName);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Username does not exist!"))
                    return false;
            }
            return true;
        }

        public bool deleteUser(string username, string password)
        {
            try
            {
                userManager.DeleteUser(username, password);
            }
            catch (Exception e)
            {
               return false;
            }
            return true;
        }

        public bool login(string userName, string pass)
        {
            try
            {
                userManager.Login(userName, pass);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool isLoggedIn(string userName, string pass)
        {
            try
            {
                userManager.Login(userName, pass);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("This user is already logged in."))
                    return true;
            }
            return false;
           
        }

        public bool logOut(string userName)
        {
            try
            {
                userManager.Logout(userName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool editUsername(string username, string newUsername)
        {
            try
            {
                userManager.EditProfileUsername(username, newUsername);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool editPassword(string username, string newPass)
        {
            try
            {
                userManager.EditProfilePassword(username, newPass);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool editAvatar(string username, string newPath)
        {
            try
            {
                userManager.EditProfileAvatarPath(username, newPath);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool createNewGame(string gameName, int numOfPlayers)
        {
            throw new NotImplementedException();
        }

        public bool isGameExist(string gameName)
        {
            throw new NotImplementedException();
        }

        public ArrayList getActiveGames(int rank)
        {
            throw new NotImplementedException();
        }

        public bool joinGame(object activeGame)
        {
            throw new NotImplementedException();
        }

        public ArrayList getActiveGames()
        {
            throw new NotImplementedException();
        }

        public bool SpectateGame(object activeGame)
        {
            throw new NotImplementedException();
        }

        public bool leaveGame(string goodGameName)
        {
            throw new NotImplementedException();
        }

        public ArrayList getAllGamesReplay()
        {
            throw new NotImplementedException();
        }

        public int getRank(string userName)
        {
            throw new NotImplementedException();
        }

        public void setRank(string gameName, int rank)
        {
            throw new NotImplementedException();
        }
    }
}
