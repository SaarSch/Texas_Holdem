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

        // Implementation according to the Singleton Pattern
        private static GameCenter Instance = null;
        private List<Pair<User,bool>> Users;

        public GameCenter()
        {
            Users = new List<Pair<User, bool>>();
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
                Logger.Log(Severity.Error, "ERROR in Register: Illegal password! Length must be between 8 and 12.");
                throw new Exception("Illegal password! Length must be between 8 and 12.");
            }

            for (i = 0; i < password.Length; i++)
            {
                if (password[i] == ' ')
                {
                    Logger.Log(Severity.Error, "ERROR in Register: Illegal password! Space is not allowed.");
                    throw new Exception("Illegal password! Space is not allowed.");
                }
                if (!Char.IsLetter(password[i]))
                {
                    hasNonLetterChar = true;
                }
            }

            if (!hasNonLetterChar)
            {
                Logger.Log(Severity.Error, "ERROR in Register: Illegal password!  Must contain at least 1 non-letter character.");
                throw new Exception("Illegal password! Must contain at least 1 non-letter character.");
            }

            // USERNAME CHECK

            if (username.Length > 12 || username.Length < 8)
            {
                Logger.Log(Severity.Error, "ERROR in Register: Illegal username!Length must be between 8 and 12.");
                throw new Exception("Illegal username! Length must be between 8 and 12.");
            }

            for (i = 0; i < username.Length; i++)
            {
                if (username[i] == ' ')
                {
                    Logger.Log(Severity.Error, "ERROR in Register: Illegal username! Space is not allowed.");
                    throw new Exception("Illegal username! Space is not allowed.");
                }
            }

            for (i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.Username == username)
                {
                    Logger.Log(Severity.Error, "ERROR in Register: Username already exists!");
                    throw new Exception("Username already exists!");
                }
            }

            // USERNAME & PASSWORD CONFIRMED
            Logger.Log(Severity.Action, "Registration completed successfully!");
            Users.Add(new Pair<User, bool>(new User(username, password, ""), false));
        }

        public User Login(string username, string password)
        {
            int i;
            User user = null;
            for (i = 0; i < Users.Count; i++)
            {
                if(Users[i].First.Username == username)
                {
                    if(Users[i].First.Password == password)
                    {
                        if (Users[i].Second)
                        {
                            Logger.Log(Severity.Error, "ERROR in Login: This user is already logged in.");
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
                        Logger.Log(Severity.Error, "ERROR in Login: Wrong password!");
                        throw new Exception("Wrong password!");
                    }
                }
            }

            if (user == null)
            {
                Logger.Log(Severity.Error, "ERROR in Login: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
            else
            {
                Logger.Log(Severity.Action, username + " logged in successfully!");
            }

            return user;
            
        }

        public void Logout(string username)
        {
            int i;
            bool exist = false;

            for (i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.Username == username)
                {
                    if (!Users[i].Second)
                    {
                        Logger.Log(Severity.Action, "ERROR in Logout: User is already logged off.");
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
                Logger.Log(Severity.Action, "ERROR in Logout: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
            else
            {
                Logger.Log(Severity.Action, username + " logged out successfully!");
            }
        }
    }
}
