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
        private List<Room> Rooms;
        
        private GameCenter()
        {
            Users = new List<Pair<User, bool>>();
            Rooms = new List<Room>();
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
                Users.Add(new Pair<User, bool>(new User(username, password, "default.png", "default@gmail.com", 5000), false));
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

        public void EditUser(string username, string newUserName, string newPassword, string newAvatarPath, string newEmail)
        {
            bool userExists = false;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == username)
                {
                    userExists = true;
                    if (!Users[i].Second)
                    {
                        Logger.Log(Severity.Error, "ERROR in Edit Profile: This user is not logged in.");
                        throw new Exception("This user is not logged in.");
                    }
                    try
                    {
                        if (newUserName != null)
                        {
                            for (int j = 0; j < Users.Count; j++)
                            {
                                if (Users[j].First.GetUsername() == newUserName)
                                {
                                    Logger.Log(Severity.Error, "ERROR in Edit Profile: New username already exists!");
                                    throw new Exception("New username already exists!");
                                }
                            }
                            Users[i].First.SetUsername(newUserName);
                        }
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

        public User GetLoggedInUser(string username)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == username)
                {
                    if (!Users[i].Second)
                    {
                        Logger.Log(Severity.Error, "ERROR in Edit Profile: This user is not logged in.");
                        throw new Exception("This user is not logged in.");
                    }
                    else
                    {
                        return Users[i].First;
                    }
                }
            }
            Logger.Log(Severity.Error, "ERROR in Edit Profile: This user doesn't exist.");
            throw new Exception("This user doesn't exist.");
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

        public Room CreateRoom(string roomName, string username, string creator,GamePreferences gp)
        {
            User user = GetLoggedInUser(username);
            if (user != null)
            {
                Player p = new Player(creator, user);
                if (IsRoomExist(roomName))
                {
                    Logger.Log(Severity.Error, "ERROR in CreateRoom: room name already taken!");
                    throw new Exception("Room name already taken!");
                }
                else
                {
                    Room newRoom = new Room(roomName, p,gp);
                    Rooms.Add(newRoom);
                    Logger.Log(Severity.Action, "Room " + newRoom.name + " created successfully by " + creator + "!");
                    return newRoom;
                }
            }
            else
            {
                Logger.Log(Severity.Error, "ERROR in CreateRoom: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
        }

        public Room GetRoom(string roomName)
        {
            Room roomFound = null;
            for (int i = 0; i < Rooms.Count && roomFound == null; i++)
            {
                if (Rooms[i].name == roomName)
                    roomFound = Rooms[i];
            }
            return roomFound;
        }

        public bool IsRoomExist(string roomName)
        {
            return GetRoom(roomName) != null;
        }

        public void AddUserToRoom(string username, string roomName, string playerName, bool isSpectator)
        {
            Room room = null;
            User user = null;
            try
            {
                room = GetRoom(roomName);
                user = GetLoggedInUser(username);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (room != null)
            {
                room.AddPlayer(new Player(playerName, user)/*, isSpectator*/);
            }
            else
            {
                Logger.Log(Severity.Error, "Error in AddUserToRoom: Room " + roomName + " doesn't exist!");
                throw new Exception("Room " + roomName + " doesn't exist!");
            }
        }

        public void RemoveUserFromRoom(string username, string roomName, string playerName)
        {
            Room room = null;
            User user = null;
            try
            {
                room = GetRoom(roomName);
                user = GetLoggedInUser(username);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (room != null)
            {
                try
                {
                    room.RemovePlayer(playerName);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                Logger.Log(Severity.Error, "Error in RemoveUserFromRoom: Room " + roomName + " doesn't exist!");
                throw new Exception("Room " + roomName + " doesn't exist!");
            }
        }

        public List<Room> GetAllRooms()
        {
            return Rooms;
        }
    }
}
