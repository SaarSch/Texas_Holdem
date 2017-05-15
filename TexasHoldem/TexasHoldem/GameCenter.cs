using System;
using System.Collections.Generic;
using TexasHoldem.Exceptions;
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
            return _instance ?? (_instance = new GameCenter());
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
                    var e = new IllegalUsernameException("ERROR in Register: Username already exists!");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
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
                            var e = new IllegalUsernameException("ERROR in Login: This User is already logged in.");
                            Logger.Log(Severity.Error, e.Message);
                            throw e;
                        }
                        user = _users[i].First;
                        _users[i].Second = true;
                    }
                    else
                    {
                        var e = new IllegalPasswordException("ERROR in Login: Wrong password!");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                }
            }

            if (user == null)
            {
                var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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
                        var e = new IllegalUsernameException("ERROR in Logout: User is already logged off.");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                    exist = true;
                    _users[i].Second = false;
                }
            }

            if (!exist)
            {
                var e = new IllegalUsernameException("ERROR in Logout: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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
                        var e = new IllegalUsernameException("ERROR in Edit Profile: This User is not logged in.");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                    try
                    {
                        if (newUserName != null)
                        {
                            for (var j = 0; j < _users.Count; j++)
                            {
                                if (_users[j].First.GetUsername() == newUserName)
                                {
                                    var e = new IllegalUsernameException("ERROR in Edit Profile: New username already exists!");
                                    Logger.Log(Severity.Error, e.Message);
                                    throw e;
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
                        var e = new Exception("ERROR in Edit Profile: Invalid new user details!");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                }
            }

            if (!userExists)
            {
                var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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
                        var err = new IllegalUsernameException("ERROR in GetLoggedInUser: This User is not logged in.");
                        Logger.Log(Severity.Error, err.Message);
                        throw err;
                    }
                    return _users[i].First;
                }
            }
            var e = new IllegalUsernameException("ERROR in Edit Profile: This user doesn't exist.");
            Logger.Log(Severity.Error, e.Message);
            throw e;
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
            var e = new IllegalUsernameException("ERROR in GetUser: This User doesn't exist.");
            Logger.Log(Severity.Error, e.Message);
            throw e;
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
                        var e = new IllegalPasswordException("ERROR in Edit Profile: Wrong password!");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                }
            }

            if (userToDelete == null)
            {
                var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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
                    var err = new IllegalRoomNameException("ERROR in CreateRoom: room name already taken!");
                    Logger.Log(Severity.Error, err.Message);
                    throw err;
                }
                var newRoom = new Room(roomName, p, gp);
                Rooms.Add(newRoom);
                Logger.Log(Severity.Action, "Room " + newRoom.Name + " created successfully by " + creator + "!");
                return newRoom;
            }
            var e = new IllegalUsernameException("ERROR in CreateRoom: Username does not exist!");
            Logger.Log(Severity.Error, e.Message);
            throw e;
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
            var e = new IllegalRoomNameException("Error in GetRoom: Room " + roomName + " doesn't exist!");
            Logger.Log(Severity.Error, e.Message);
            throw e;
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
                var e = new IllegalRoomNameException("Error in AddUserToRoom: Room " + roomName + " doesn't exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
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
                var e = new IllegalRoomNameException("Error in RemoveUserFromRoom: Room " + roomName + " doesn't exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            return room;
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
            var e = new IllegalRoomNameException("Room " + roomName + " does not exist!");
            Logger.Log(Severity.Error, e.Message);
            throw e;
        }

        public List<Room> FindGames(List<Predicate<Room>> predicates)
        {
            var ans = new List<Room>();
            string roomsFound = "";
            foreach (var r in Rooms)
            {
                var toAdd = true;
                foreach (var p in predicates)
                {
                    if (!p.Invoke(r))
                    {
                        toAdd = false;
                    }
                }
                if (toAdd)
                {
                    ans.Add(r);
                }
            }

            if (ans.Count > 0)
            {
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
                Logger.Log(Severity.Action, "Found the following game rooms: " + roomsFound);

            }
            else
            {
                var e = new Exception("ERROR in FindGames: No game rooms found.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            return ans;
        }

        public string GetReplayFilename(string roomName) // TODO : change to CSV file (and move to Replayer?)
        {
            return GetRoom(roomName).GameReplay;
        }

    }
}