using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.GameCenterHelpers;

namespace TexasHoldem
{
    public class GameCenter
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            Console.ReadLine();
        }

        private static GameCenter Instance = null;
        private List<Pair<ProxyUser,bool>> Users;

        public GameCenter()
        {
            Users = new List<Pair<ProxyUser, bool>>();
        }

        public static GameCenter GetGameCenter()
        {
            if (Instance == null)
            {
                return new GameCenter();
            }

            return Instance;
        }

        public void Register(string username, string password)
        {
            Boolean hasNonLetterChar = false;
            int i;

            // PASSWORD CHECK

            if (password.Length > 12 || password.Length < 8)
            {
                throw new Exception("Illegal password! Length must be between 8 and 12.");
            }

            for (i = 0; i < password.Length; i++)
            {
                if (password[i] == ' ')
                {
                    throw new Exception("Illegal password! Space is not allowed.");
                }
                if (!Char.IsLetter(password[i]))
                {
                    hasNonLetterChar = true;
                }
            }

            if (!hasNonLetterChar)
            {
                throw new Exception("Illegal password! Must contain at least 1 non-letter character.");
            }

            // USERNAME CHECK

            if (username.Length > 12 || username.Length < 8)
            {
                throw new Exception("Illegal username! Length must be between 8 and 12.");
            }

            for (i = 0; i < username.Length; i++)
            {
                if (username[i] == ' ')
                {
                    throw new Exception("Illegal username! Space is not allowed.");
                }
            }

            for (i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.getUsername() == username)
                {
                    throw new Exception("Username already exists!");
                }
            }

            // USERNAME & PASSWORD CONFIRMED
            Users.Add(new Pair<ProxyUser, bool>(new ProxyUser(username, password), false));
        }

        public ProxyUser Login(string username, string password)
        {
            int i;
            ProxyUser user = null;
            for (i = 0; i < Users.Count; i++)
            {
                if(Users[i].First.getUsername() == username)
                {
                    if(Users[i].First.getPassword() == password)
                    {
                        if (Users[i].Second)
                        {
                            throw new Exception("This user is already logged in.");
                        }
                        else
                        {
                            user = Users[i].First;
                            Users[i].Second = true;
                        }
                    }
                    else
                    {
                        throw new Exception("Wrong password!");
                    }
                }
            }

            if (user == null)
            {
                throw new Exception("Username does not exist!");
            }

            return user;
            
        }

        public void Logout(string username)
        {
            int i;
            bool exist = false;

            for (i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.getUsername() == username)
                {
                    if (!Users[i].Second)
                    {
                        throw new Exception("User is already logged off.");
                    }
                    else
                    {
                        exist = true;
                        Users[i].Second = false;
                    }

                }
            }

            if (!exist)
            {
                throw new Exception("Username does not exist!");
            }
        }
    }
}
