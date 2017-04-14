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
        public bool register(string s1, string s2)
        {
           return true;
        }

        public bool isUserExist(string s)
        {
            return true;
        }

        public bool deleteUser(string s)
        {
            return true;
        }

        public bool login(string s1, string s2)
        {
            return true;
        }

        public bool isLoggedIn(string s)
        {
            return true;
        }

        public bool logOut(string s)
        {
            return true;
        }

        public bool editUserName(string s)
        {
            return true;
        }

        public bool editPassword(string s)
        {
            return true;
        }

        public bool editAvatar(string s)
        {
            return true;
        }

        public bool createNewGame(string gameName, int numOfPlayers)
        {
            return true;
        }

        public bool isGameExist(string gameName)
        {
            return true;
        }

        public ArrayList getActiveGames()
        {
            ArrayList s = new ArrayList();
            s.Add("Good Game Name");
            return s;
        }

        public bool joinGame(object activeGame)
        {
            return true;
        }

        public ArrayList getAllActiveGames()
        {
            ArrayList s = new ArrayList();
            s.Add("Good Game Name");
            return s;
        }

        public bool SpectateGame(object activeGame)
        {
            return true;
        }

        public bool leaveGame(string goodGameName)
        {
            return true;
        }

        public ArrayList getAllGamesReplay()
        {
            ArrayList s = new ArrayList();
            s.Add("Good Game Name");
            return s;
        }
    }
}
