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
        private static GameCenter instance = null;
        private List<Pair<User,bool>> Users;

        private GameCenter()
        {
            Users = new List<Pair<User, bool>>();
        }

        public static GameCenter GetGameCenter()
        {
            if (instance == null)
            {
                instance = new GameCenter();
                return instance;
            }

            return instance;
        }

        public void Register(string username, string password)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == username)
                {
                    Logger.Log(Severity.Error, "ERROR in Register: Username already exists!");
                    throw new Exception("Username already exists!");
                }
            }

            try
            {
                Users.Add(new Pair<User, bool>(new User(username, password, "", ""), false));
            }
            catch (Exception e)
            {
                throw e;
            }
            // USERNAME & PASSWORD CONFIRMED
            Logger.Log(Severity.Action, "Registration completed successfully!");
        }

        public User Login(string username, string password)
        {
            int i;
            User user = null;
            for (i = 0; i < Users.Count; i++)
            {
                if(Users[i].First.GetUsername() == username)
                {
                    if(Users[i].First.GetPassword() == password)
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
                if (Users[i].First.GetUsername() == username)
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

        public void EditUser(string username, string password, string newUserName = null, string newPassword = null,
            string newAvatarPath = null, string newEmail = null)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == newUserName)
                {
                    Logger.Log(Severity.Error, "ERROR in Edit Profile: New username already exists!");
                    throw new Exception("New username already exists!");
                }
            }

            bool userExists = false;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == username)
                {
                    userExists = true;
                    if (Users[i].First.GetPassword() == password)
                    {
                        try
                        {
                            if (newUserName != null)
                                Users[i].First.SetUsername(newUserName);
                            if (newPassword != null)
                                Users[i].First.SetPassword(newPassword);
                            if (newAvatarPath != null)
                                Users[i].First.SetAvatar(newAvatarPath);
                            if (newEmail != null)
                                Users[i].First.SetEmail(newEmail);
                        }
                        catch (Exception e)
                        {
                            Logger.Log(Severity.Error, "ERROR in Edit Profile: Invalid new user details!"); // TODO specific?
                            throw e;
                        }
                    }
                    else
                    {
                        Logger.Log(Severity.Error, "ERROR in Edit Profile: Wrong password!");
                        throw new Exception("Wrong password!");
                    }
                }
            }

            if (!userExists)
            {
                Logger.Log(Severity.Error, "ERROR in Login: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
            else
            {
                Logger.Log(Severity.Action, username + "'s profile edited successfully!");
            }
        }

        public void DeleteUser(string username, string password)
        {
            Pair<User, bool> userToDelete = null;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == username)
                {
                    if (Users[i].First.GetPassword() == password)
                    {
                        userToDelete = Users[i];
                    }
                    else
                    {
                        Logger.Log(Severity.Error, "ERROR in Edit Profile: Wrong password!");
                        throw new Exception("Wrong password!");
                    }
                }
            }

            if (userToDelete == null)
            {
                Logger.Log(Severity.Error, "ERROR in Login: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
            else
            {
                Users.Remove(userToDelete);
                Logger.Log(Severity.Action, "User: " + username + " deleted successfully!");
            }
        }
    }
}
