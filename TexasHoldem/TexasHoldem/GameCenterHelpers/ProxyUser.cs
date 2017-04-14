using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.GameCenterHelpers
{
    public class ProxyUser
    {
        private string username;
        private string password;
        public ProxyUser(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string getUsername()
        {
            return this.username;
        }

        public string getPassword()
        {
            return this.password;
        }

        public override string ToString()
        {
            return username + " " + password;
        }
    }
}
