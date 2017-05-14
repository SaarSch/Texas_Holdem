using System;
using System.Collections.Generic;
using TexasHoldem.Game;
using TexasHoldem.GameCenterHelpers;
using TexasHoldem.GamePrefrences;
using TexasHoldem.Loggers;
using TexasHoldem.Users;

namespace TexasHoldem
{
    public class GameCenter
    {
        private static void Main()
        {   
            Console.WriteLine("aaaa");
            Console.ReadLine();
        }

        // Implementation according to the Singleton Pattern
        private static GameCenter _instance;
        private readonly List<Pair<User, bool>> _users;
        public List<Room> Rooms;
        public int ExpCriteria { get; private set; }
        public int DefaultRank { get; private set; }

        public const int MinRank = 0;
        public const int MaxRank = 10;
        public const int MinCriteria = 5;
        public const int MaxCriteria = 20;

        private GameCenter()
        {
            _users = new List<Pair<User, bool>>();
            Rooms = new List<Room>();
            ExpCriteria = 10;
        }

        public static GameCenter GetGameCenter()
        {
            if (_instance == null)
            {
                _instance = new GameCenter();
                return _instance;
            }

            return _instance;
        }

        public void SetLeagues()
        {
            double size = _users.Count / 10;
            var leagueSize =(int)Math.Ceiling(size);
            if (leagueSize == 0) leagueSize = 1;
            var currentSize = 0;
            var league = 10;
            var users = 0;
            var done = new List<User>();
            User current = null;
            var maxWins = -1;
            for (var i = 0; i < _users.Count; i++)
            {
                foreach (var u in _users)
                {
                    if (u.First.Wins > maxWins && !done.Contains(u.First))
                    {
                        maxWins = u.First.Wins;
                        current = u.First;
                    }
                }
                if (current != null)
                {
                    current.League = league;
                    currentSize++;
                    done.Add(current);
                }
                users++;
                if ((currentSize == leagueSize && leagueSize >= 2) || (currentSize > leagueSize))
                {
                    currentSize = 0;
                    league--;
                }
                if (_users.Count == users + 1&& _users.Count%2==1)
                {
                    league++;
                }
                maxWins = -1; 
            }
        }

        public void Register(string username, string password)
        {
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].First.GetUsername() == username)
                {
                    Logger.Log(Severity.Error, "ERROR in Register: Username already exists!");
                    throw new Exception("Username already exists!");
                }
            }

            _users.Add(new Pair<User, bool>(new User(username, password, "default.png", "default@gmail.com", 5000), false));
            // USERNAME & PASSWORD CONFIRMED
            Logger.Log(Severity.Action, "Registration completed successfully!");
        }

        public User Login(string username, string password)
        {
            int i;
            User user = null;
            for (i = 0; i < _users.Count; i++)
            {
                if(_users[i].First.GetUsername() == username)
                {
                    if(_users[i].First.GetPassword() == password)
                    {
                        if (_users[i].Second)
                        {
                            Logger.Log(Severity.Error, "ERROR in Login: This user is already logged in.");
                            throw new Exception("This user is already logged in.");
                        }
                        user = _users[i].First;
                        _users[i].Second = true;
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
            Logger.Log(Severity.Action, username + " logged in successfully!");

            return user;
            
        }

        public void Logout(string username)
        {
            int i;
            var exist = false;

            for (i = 0; i < _users.Count; i++)
            {
                if (_users[i].First.GetUsername() == username)
                {
                    if (!_users[i].Second)
                    {
                        Logger.Log(Severity.Action, "ERROR in Logout: User is already logged off.");
                        throw new Exception("User is already logged off.");
                    }
                    exist = true;
                    _users[i].Second = false;
                }
            }

            if (!exist)
            {
                Logger.Log(Severity.Action, "ERROR in Logout: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
            Logger.Log(Severity.Action, username + " logged out successfully!");
        } 

        public void DeleteAllUsers()
        {
            _users.Clear();
        }

        public void DeleteAllRooms()
        {
            Rooms.Clear();
        }

        public void EditUser(string username, string newUserName, string newPassword, string newAvatarPath, string newEmail)
        {
            var userExists = false;
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].First.GetUsername() == username)
                {
                    userExists = true;
                    if (!_users[i].Second)
                    {
                        Logger.Log(Severity.Error, "ERROR in Edit Profile: This user is not logged in.");
                        throw new Exception("This user is not logged in.");
                    }
                    try
                    {
                        if (newUserName != null)
                        {
                            for (var j = 0; j < _users.Count; j++)
                            {
                                if (_users[j].First.GetUsername() == newUserName)
                                {
                                    Logger.Log(Severity.Error, "ERROR in Edit Profile: New username already exists!");
                                    throw new Exception("New username already exists!");
                                }
                            }
                            _users[i].First.SetUsername(newUserName);
                        }
                        if (newPassword != null)
                            _users[i].First.SetPassword(newPassword);
                        if (newAvatarPath != null)
                            _users[i].First.SetAvatar(newAvatarPath);
                        if (newEmail != null)
                            _users[i].First.SetEmail(newEmail);
                    }
                    catch (Exception)
                    {
                        Logger.Log(Severity.Error, "ERROR in Edit Profile: Invalid new user details!"); // TODO specific?
                        throw;
                    }
                }
            }

            if (!userExists)
            {
                Logger.Log(Severity.Error, "ERROR in Login: Username does not exist!");
                throw new Exception("Username does not exist!");
            }
            Logger.Log(Severity.Action, username + "'s profile edited successfully!");
        }

        public User GetLoggedInUser(string username)
        {
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].First.GetUsername() == username)
                {
                    if (!_users[i].Second)
                    {
                        Logger.Log(Severity.Error, "ERROR in GetLoggedInUser: This user is not logged in.");
                        throw new Exception("This user is not logged in.");
                    }
                    return _users[i].First;
                }
            }
            Logger.Log(Severity.Error, "ERROR in Edit Profile: This user doesn't exist.");
            throw new Exception("This user doesn't exist.");
        }

        public User GetUser(string username)
        {
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].First.GetUsername() == username)
                {

                        return _users[i].First;
                }
            }
            Logger.Log(Severity.Error, "ERROR in GetUser: This user doesn't exist.");
            throw new Exception("This user doesn't exist.");
        }

        public void DeleteUser(string username, string password)
        {
            Pair<User, bool> userToDelete = null;
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].First.GetUsername() == username)
                {
                    if (_users[i].First.GetPassword() == password)
                    {
                        userToDelete = _users[i];
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
            _users.Remove(userToDelete);
            Logger.Log(Severity.Action, "User: " + username + " deleted successfully!");
        }

        public Room CreateRoom(string roomName, string username, string creator, IPreferences gp)
        {
            var user = GetLoggedInUser(username);
            if (user != null)
            {
                var p = new Player(creator, user);
                if (IsRoomExist(roomName))
                {
                    Logger.Log(Severity.Error, "ERROR in CreateRoom: room name already taken!");
                    throw new Exception("Room name already taken!");
                }
                var newRoom = new Room(roomName, p, gp);
                Rooms.Add(newRoom);
                Logger.Log(Severity.Action, "Room " + newRoom.Name + " created successfully by " + creator + "!");
                return newRoom;
            }
            Logger.Log(Severity.Error, "ERROR in CreateRoom: Username does not exist!");
            throw new Exception("Username does not exist!");
        }

        public Room GetRoom(string roomName)
        {
            Room roomFound = null;
            for (var i = 0; i < Rooms.Count && roomFound == null; i++)
            {
                if (Rooms[i].Name == roomName)
                    roomFound = Rooms[i];
            }
            if (roomFound != null)
            {
                return roomFound;
            }
            Logger.Log(Severity.Error, "Error in GetRoom: Room " + roomName + " doesn't exist!");
            throw new Exception("Room " + roomName + " doesn't exist!");
        }

        public bool IsRoomExist(string roomName)
        {
            try
            {
                GetRoom(roomName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Room AddUserToRoom(string username, string roomName, bool isSpectator, string playerName = "")
        {
            var room = GetRoom(roomName);
            var user = GetLoggedInUser(username);

            if (room != null)
            {
                if (isSpectator)
                {
                    room.Spectate(user);
                }
                else
                { 
                    
                return room.AddPlayer(new Player(playerName, user));
                }
            }
            else
            {
                Logger.Log(Severity.Error, "Error in AddUserToRoom: Room " + roomName + " doesn't exist!");
                throw new Exception("Room " + roomName + " doesn't exist!");
            }
            return room;
        }

        public Room RemoveUserFromRoom(string username, string roomName, string playerName)
        {
            var room = GetRoom(roomName);
            GetLoggedInUser(username);

            if (room != null)
            {
                room.ExitRoom(playerName);
            }
            else
            {
                Logger.Log(Severity.Error, "Error in RemoveUserFromRoom: Room " + roomName + " doesn't exist!");
                throw new Exception("Room " + roomName + " doesn't exist!");
            }
            return room;
        }

        public void SetDefaultRank(string username, int rank)
        {
            var context = GetLoggedInUser(username);

            if (context.League != MaxRank)
            {
                Logger.Log(Severity.Error, "ERROR in SetDefaultRank: Only highest rank users can update the default rank!");
                throw new Exception("Only highest rank users can update the default rank!");
            }
            if (rank < MinRank || rank > MaxRank)
            {
                Logger.Log(Severity.Error, "ERROR in SetDefaultRank: Default rank must be an integer in [0,10]!");
                throw new Exception("Default rank must be an integer in [0,10]!");
            }
            DefaultRank = rank;
            Logger.Log(Severity.Action, username + " changed the default rank to " + rank + "!");
        }

        public void SetExpCriteria(string username, int exp)
        {
            var context = GetLoggedInUser(username);

            if (context.League != MaxRank)
            {
                Logger.Log(Severity.Error, "ERROR in SetEXPCriteria: Only highest rank users can update the EXP criteria!");
                throw new Exception("Only highest rank users can update the EXP criteria!");
            }
            if (exp < MinCriteria || exp > MaxCriteria)
            {
                Logger.Log(Severity.Error, "ERROR in SetDefaultRank: EXP criteria must be an integer in [5,20]!");
                throw new Exception("EXP criteria must be an integer in [5, 20]!");
            }
            ExpCriteria = exp;
            Logger.Log(Severity.Action, username + " changed the EXP criteria to " + exp + "!");
        }

        public void SetUserRank(string username, string usernameToSet, int rank)
        {
            var context = GetLoggedInUser(username);

            if (context.League != MaxRank)
            {
                Logger.Log(Severity.Error, "ERROR in SetUserRank: Only highest rank users can set other users' rank!");
                throw new Exception("Only highest rank users can set other users' rank!");
            }
            if (rank < MinRank || rank > MaxRank)
            {
                Logger.Log(Severity.Error, "ERROR in SetUserRank: Default rank must be an integer in [0,10]!");
                throw new Exception("Default rank must be an integer in [0,10]!");
            }
            var toSetRank = GetUser(usernameToSet);
            toSetRank.League = rank;
            toSetRank = GetUser(usernameToSet);
            toSetRank.League = rank;
            Logger.Log(Severity.Action, username + " changed the rank of " + usernameToSet + " to " + rank + "!");
        }

        public void DeleteRoom(string roomName)
        {
            for (var i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].Name == roomName)
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
            string gameType, int buyInPolicy, int chipPolicy, int minBet, int minPlayers, int maxPlayers, bool spectating,
            bool prefFlag, bool leagueFlag)
        {
            IPreferences gp = new GamePreferences();
            gp = new ModifiedGameType((Gametype)Enum.Parse(typeof(Gametype), gameType), gp);
            gp = new ModifiedBuyInPolicy(buyInPolicy, gp);
            gp = new ModifiedChipPolicy(chipPolicy, gp);
            gp = new ModifiedMinBet(minBet, gp);
            gp = new ModifiedMinPlayers(minPlayers, gp);
            gp = new ModifiedMaxPlayers(maxPlayers, gp);
            gp = new ModifiedSpectating(spectating, gp);

            var ans = new List<Room>();
            var roomsFound = "";
            var context = GetLoggedInUser(contextUser);

            for (var i = 0; i < Rooms.Count; i++)
            {
                var passed = true;

                if (playerFlag)
                {
                    var found = false;
                    for (var j = 0; j < Rooms[i].Players.Count; j++)
                    {
                        if (Rooms[i].Players[j].Name == playerName)
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
                    if (Rooms[i].Pot != potSize)
                    {
                        passed = false;
                    }
                }

                if (leagueFlag)
                {
                    if (Rooms[i].League != context.League)
                    {
                        passed = false;
                    }
                }

                if (prefFlag)
                {
                    if (Rooms[i].GamePreferences.GetMinPlayers() != gp.GetMinPlayers())
                    {
                        passed = false;
                    }
                    if (Rooms[i].GamePreferences.GetMaxPlayers() != gp.GetMaxPlayers())
                    {
                        passed = false;
                    }
                    if (Rooms[i].GamePreferences.GetBuyInPolicy() != gp.GetBuyInPolicy())
                    {
                        passed = false;
                    }
                    if (Rooms[i].GamePreferences.GetChipPolicy() != gp.GetChipPolicy())
                    {
                        passed = false;
                    }
                    if (Rooms[i].GamePreferences.GetMinBet() != gp.GetMinBet())
                    {
                        passed = false;
                    }
                    if (Rooms[i].GamePreferences.GetGameType() != gp.GetGameType())
                    {
                        passed = false;
                    }
                    if (Rooms[i].GamePreferences.GetSpectating() != gp.GetSpectating())
                    {
                        passed = false;
                    }
                }

                if (passed)
                {
                    ans.Add(Rooms[i]);
                }
            }

            for (var i = 0; i < ans.Count; i++)
            {
                if (i < ans.Count - 1)
                {
                    roomsFound = roomsFound + ans[i].Name + ", ";
                }
                else
                {
                    roomsFound = roomsFound + ans[i].Name + ".";
                }
                
            }

            if (ans.Count > 0)
            {
                Logger.Log(Severity.Action, "Found the following game rooms: " + roomsFound);
                // added
                var stringList = new List<string>();
                foreach (var r in ans)
                {
                    stringList.Add(r.Name);
                }
                return stringList;
                //return ans;
            }
            Logger.Log(Severity.Error, "ERROR in FindGames: No game rooms found.");
            throw new Exception("No game rooms found.");
        }

        public string GetReplayFilename(string roomName) // TODO : change to CSV file (and move to Replayer?)
        {
            return GetRoom(roomName).GameReplay;
        }

    }
}