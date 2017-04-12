using System;
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
    }
}
