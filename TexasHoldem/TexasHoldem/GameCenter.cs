using System;
using System.Collections.Generic;
using TexasHoldem.GameCenterHelpers;

namespace TexasHoldem
{
    public class GameCenter
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("aaaa");
            Console.ReadLine();
        }

        // Implementation according to the Singleton Pattern
        private static GameCenter instance = null;
        private List<Pair<User, bool>> Users;
        public List<Room> Rooms;
        public int EXPCriteria { get; private set; }
        public int DefaultRank { get; private set; }

        private GameCenter()
        {
            Users = new List<Pair<User, bool>>();
            Rooms = new List<Room>();
            EXPCriteria = 10;
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

        public void DeleteAllUsers()
        {
            Users.Clear();
        }

        public void DeleteAllRooms()
        {
            Rooms.Clear();
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
                        Logger.Log(Severity.Error, "ERROR in GetLoggedInUser: This user is not logged in.");
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

        public User GetUser(string username)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].First.GetUsername() == username)
                {

                        return Users[i].First;
                }
            }
            Logger.Log(Severity.Error, "ERROR in GetUser: This user doesn't exist.");
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

        public Room CreateRoom(string roomName, string username, string creator, Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers,
            bool spectating)
        {
            GamePreferences gp = new GamePreferences(gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers, spectating);
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
            if (roomFound != null)
            {
                return roomFound;
            }
            else
            {
                Logger.Log(Severity.Error, "Error in GetRoom: Room " + roomName + " doesn't exist!");
                throw new Exception("Room " + roomName + " doesn't exist!");
            }
        }

        public bool IsRoomExist(string roomName)
        {
            try
            {
                GetRoom(roomName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void AddUserToRoom(string username, string roomName, bool isSpectator, string playerName = "")
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
                if (isSpectator)
                {
                    room.Spectate(user);
                }
                else
                { 
                    
                room.AddPlayer(new Player(playerName, user));
                }
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
                    room.ExitRoom(playerName);
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

        public void SetDefaultRank(string username, int rank)
        {
            User context = null;
            try
            {
                context = GetLoggedInUser(username);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (context.Rank != 10)
            {
                Logger.Log(Severity.Error, "ERROR in SetDefaultRank: Only highest rank users can update the default rank!");
                throw new Exception("Only highest rank users can update the default rank!");
            }
            if (rank < 0 || rank > 10)
            {
                Logger.Log(Severity.Error, "ERROR in SetDefaultRank: Default rank must be an integer in [0,10]!");
                throw new Exception("Default rank must be an integer in [0,10]!");
            }
            this.DefaultRank = rank;
            Logger.Log(Severity.Action, username + " changed the default rank to " + rank + "!");
        }

        public void SetExpCriteria(string username, int exp)
        {
            User context = null;
            try
            {
                context = GetLoggedInUser(username);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (context.Rank != 10)
            {
                Logger.Log(Severity.Error, "ERROR in SetEXPCriteria: Only highest rank users can update the EXP criteria!");
                throw new Exception("Only highest rank users can update the EXP criteria!");
            }
            if (exp < 5 || exp > 20)
            {
                Logger.Log(Severity.Error, "ERROR in SetDefaultRank: EXP criteria must be an integer in [5,20]!");
                throw new Exception("EXP criteria must be an integer in [5, 20]!");
            }
            this.EXPCriteria = exp;
            Logger.Log(Severity.Action, username + " changed the EXP criteria to " + exp + "!");
        }

        public void SetUserRank(string username, string usernameToSet, int rank)
        {
            User context = null;
            User toSetRank = null;
            try
            {
                context = GetLoggedInUser(username);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (context.Rank != 10)
            {
                Logger.Log(Severity.Error, "ERROR in SetUserRank: Only highest rank users can set other users' rank!");
                throw new Exception("Only highest rank users can set other users' rank!");
            }
            if (rank < 0 || rank > 10)
            {
                Logger.Log(Severity.Error, "ERROR in SetUserRank: Default rank must be an integer in [0,10]!");
                throw new Exception("Default rank must be an integer in [0,10]!");
            }
            try
            {
                toSetRank = GetUser(usernameToSet);
            }
            catch (Exception e)
            {
                throw e;
            }
            toSetRank.Rank = rank;
            Logger.Log(Severity.Action, username + " changed the rank of " + usernameToSet + " to " + rank + "!");
        }

        public void DeleteRoom(string roomName)
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].name == roomName)
                {
                    Rooms.RemoveAt(i);
                    Logger.Log(Severity.Action, "Room " + roomName + " deleted successfully!");
                    return;
                }
            }
            Logger.Log(Severity.Error, "Room " + roomName + " does not exist!");
            throw new Exception("ERROR in DeleteRoom: Room "+ roomName + " does not exist!");
        }

        // changes from List<Room>
        public List<string> FindGames(string contextUser, string playerName, bool playerFlag, int potSize, bool potFlag, 
            Gametype gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers, bool spectating,
            bool prefFlag, bool leagueFlag)
        {
            GamePreferences gp = new GamePreferences(gameType, buyInPolicy, chipPolicy, minBet, minPlayers, maxPlayers, spectating);
            List<Room> ans = new List<Room>();
            string roomsFound = "";
            User context = null;
            try
            {
                context = GetLoggedInUser(contextUser);
            }
            catch (Exception e)
            {
                throw e;
            }
            for (int i = 0; i < Rooms.Count; i++)
            {
                bool passed = true;

                if (playerFlag)
                {
                    bool found = false;
                    for (int j = 0; j < Rooms[i].players.Count; j++)
                    {
                        if (Rooms[i].players[j].Name == playerName)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        passed = false;
                    }
                }

                if (potFlag)
                {
                    if (Rooms[i].pot != potSize)
                    {
                        passed = false;
                    }
                }

                if (leagueFlag)
                {
                    if (Rooms[i].rank != context.Rank)
                    {
                        passed = false;
                    }
                }

                if (prefFlag)
                {
                    if (Rooms[i].gamePreferences.minPlayers != gp.minPlayers)
                    {
                        passed = false;
                    }
                    if (Rooms[i].gamePreferences.maxPlayers != gp.maxPlayers)
                    {
                        passed = false;
                    }
                    if (Rooms[i].gamePreferences.buyInPolicy != gp.buyInPolicy)
                    {
                        passed = false;
                    }
                    if (Rooms[i].gamePreferences.chipPolicy != gp.chipPolicy)
                    {
                        passed = false;
                    }
                    if (Rooms[i].gamePreferences.minBet != gp.minBet)
                    {
                        passed = false;
                    }
                    if (Rooms[i].gamePreferences.gameType != gp.gameType)
                    {
                        passed = false;
                    }
                    if (Rooms[i].gamePreferences.spectating != gp.spectating)
                    {
                        passed = false;
                    }
                }

                if (passed)
                {
                    ans.Add(Rooms[i]);
                }
            }

            for (int i = 0; i < ans.Count; i++)
            {
                if (i < ans.Count - 1)
                {
                    roomsFound = roomsFound + ans[i].name + ", ";
                }
                else
                {
                    roomsFound = roomsFound + ans[i].name + ".";
                }
                
            }

            if (ans.Count > 0)
            {
                Logger.Log(Severity.Action, "Found the following game rooms: " + roomsFound);
                // added
                List<string> stringList = new List<string>();
                foreach (Room r in ans)
                {
                    stringList.Add(r.name);
                }
                return stringList;
                //return ans;
            }
            else
            {
                Logger.Log(Severity.Error, "ERROR in FindGames: No game rooms found.");
                throw new Exception("No game rooms found.");
            }
        }

        public string GetReplayFilename(string roomName) // TODO : change to CSV file (and move to Replayer?)
        {
            return GetRoom(roomName).gameReplay;
        }
    }
}