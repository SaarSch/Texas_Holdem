using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Bridges
{
    public class ProxyBridge : IBridge
    {
        private IBridge real;
        public ProxyBridge()
        {
            real = null;
        }
        public bool register(string userName, string pass)
        {
            return  real==null || real.register(userName, pass);
            
        }

        public bool isUserExist(string userName)
        {
            return real == null || real.isUserExist(userName);
        }

        public bool deleteUser(string userName)
        {
            return real == null || real.deleteUser(userName);
        }

        public bool login(string userName, string pass)
        {
            return real == null || real.login(userName, pass);
        }

        public bool isLoggedIn(string userName)
        {
            return real == null || real.isLoggedIn(userName);
        }

        public bool logOut(string userName)
        {
            return real == null || real.logOut(userName);
        }

        public bool editUserName(string userName)
        {
            return real == null || real.editUserName(userName);
        }

        public bool editPassword(string pass)
        {
            return real == null || real.editPassword(pass);
        }

        public bool editAvatar(string path)
        {
            return real == null || real.editAvatar(path);
        }

        public bool createNewGame(string gameName, int numOfPlayers)
        {
            return real == null || real.createNewGame(gameName,numOfPlayers);
        }

        public bool isGameExist(string gameName)
        {
            return real == null || real.isGameExist(gameName);
        }

        public ArrayList getActiveGames(int rank)
        {
            ArrayList s = new ArrayList();
            if (real != null)
                s = real.getActiveGames(rank);
            else
                s.Add("Good Game Name");
            return s;
        }

        public ArrayList getActiveGames()
        {
            ArrayList s = new ArrayList();

            if (real != null)
                s = real.getActiveGames();
            else
                s.Add("Good Game Name");
            return s;
        }

        public bool joinGame(object activeGame)
        {
            return real == null || real.joinGame(activeGame);
        }

        public bool SpectateGame(object activeGame)
        {
            return real == null || real.SpectateGame(activeGame);
        }

        public bool leaveGame(string goodGameName)
        {
            return real == null || real.leaveGame(goodGameName);
        }

        public ArrayList getAllGamesReplay()
        {
            ArrayList s = new ArrayList();
            if (real != null)
                s = real.getAllGamesReplay();
            else
                s.Add("Good Game Name");
            return s;
        }

        public int getRank(string userName)
        {
            return real?.getRank(userName) ?? 0;
        }

        public void setRank(string gameName, int rank)
        {
            real?.setRank(gameName,rank);
        }
    }
}
