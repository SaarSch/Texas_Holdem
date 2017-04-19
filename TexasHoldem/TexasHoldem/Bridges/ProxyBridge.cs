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
            //real = null;
            real = new RealBridge();
        }
        public bool register(string username, string pass)
        {
            return  real==null || real.register(username, pass);
            
        }

        public bool isUserExist(string username)
        {
            return real == null || real.isUserExist(username);
        }

        public bool deleteUser(string username, string password)
        {
            return real == null || real.deleteUser(username, password);
        }

        public bool login(string username, string pass)
        {
            return real == null || real.login(username, pass);
        }

        public bool isLoggedIn(string username, string pass)
        {
            return real == null || real.isLoggedIn(username, pass);
        }

        public bool logOut(string username)
        {
            return real == null || real.logOut(username);
        }

        public bool editUsername(string username, string newusername)
        {
            return real == null || real.editUsername(username, newusername);
        }

        public bool editPassword(string username, string newPass)
        {
            return real == null || real.editPassword(username, newPass);
        }

        public bool editAvatar(string username, string newPath)
        {
            return real == null || real.editAvatar(username, newPath);
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

        public int getRank(string username)
        {
            return real?.getRank(username) ?? 0;
        }

        public void setRank(string gameName, int rank)
        {
            real?.setRank(gameName,rank);
        }
    }
}
