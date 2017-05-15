using System;
using System.Collections.Generic;
using TexasHoldem.Exceptions;
using TexasHoldem.Loggers;

namespace TexasHoldem.Users
{
    public class UserLogic
    {
        public void SetLeagues(List<Tuple<User, bool>> _users)
        {
            double size = _users.Count / 10;
            var leagueSize = (int)Math.Ceiling(size);
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
                    if (u.Item1.Wins > maxWins && !done.Contains(u.Item1))
                    {
                        maxWins = u.Item1.Wins;
                        current = u.Item1;
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
                if (_users.Count == users + 1 && _users.Count % 2 == 1)
                {
                    league++;
                }
                maxWins = -1;
            }
        }

        public void Register(string username, string password, List<Tuple<User, bool>> _users)
        {
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {
                    var e = new IllegalUsernameException("ERROR in Register: Username already exists!");
                    Logger.Log(Severity.Error, e.Message);
                    throw e;
                }
            }

            _users.Add(new Tuple<User, bool>(new User(username, password, "default.png", "default@gmail.com", 5000), false));
            // USERNAME & PASSWORD CONFIRMED
            Logger.Log(Severity.Action, "Registration completed successfully!");
        }

        public User Login(string username, string password, List<Tuple<User, bool>> _users)
        {
            int i;
            User user = null;
            for (i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {
                    if (_users[i].Item1.GetPassword() == password)
                    {
                        if (_users[i].Item2)
                        {
                            var e = new IllegalUsernameException("ERROR in Login: This User is already logged in.");
                            Logger.Log(Severity.Error, e.Message);
                            throw e;
                        }
                        user = _users[i].Item1;
                        _users[i] = new Tuple<User, bool>(_users[i].Item1, true);
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

        public void Logout(string username, List<Tuple<User, bool>> _users)
        {
            int i;
            var exist = false;

            for (i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {
                    if (!_users[i].Item2)
                    {
                        var e = new IllegalUsernameException("ERROR in Logout: User is already logged off.");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                    exist = true;
                    _users[i] = new Tuple<User, bool>(_users[i].Item1, false);
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

        public void DeleteAllUsers(List<Tuple<User, bool>> _users)
        {
            _users.Clear();
        }

        public void EditUser(string username, string newUserName, string newPassword, string newAvatarPath, string newEmail, List<Tuple<User, bool>> _users)
        {
            var userExists = false;
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {
                    userExists = true;
                    if (!_users[i].Item2)
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
                                if (_users[j].Item1.GetUsername() == newUserName)
                                {
                                    var e = new IllegalUsernameException("ERROR in Edit Profile: New username already exists!");
                                    Logger.Log(Severity.Error, e.Message);
                                    throw e;
                                }
                            }
                            _users[i].Item1.SetUsername(newUserName);
                        }
                        if (newPassword != null)
                            _users[i].Item1.SetPassword(newPassword);
                        if (newAvatarPath != null)
                            _users[i].Item1.SetAvatar(newAvatarPath);
                        if (newEmail != null)
                            _users[i].Item1.SetEmail(newEmail);
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

        public User GetLoggedInUser(string username,List<Tuple<User, bool>> _users)
        {
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {
                    if (!_users[i].Item2)
                    {
                        var err = new IllegalUsernameException("ERROR in GetLoggedInUser: This User is not logged in.");
                        Logger.Log(Severity.Error, err.Message);
                        throw err;
                    }
                    return _users[i].Item1;
                }
            }
            var e = new IllegalUsernameException("ERROR in Edit Profile: This user doesn't exist.");
            Logger.Log(Severity.Error, e.Message);
            throw e;
        }

        public User GetUser(string username, List<Tuple<User, bool>> _users)
        {
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {

                    return _users[i].Item1;
                }
            }
            var e = new IllegalUsernameException("ERROR in GetUser: This User doesn't exist.");
            Logger.Log(Severity.Error, e.Message);
            throw e;
        }

        public void DeleteUser(string username, string password, List<Tuple<User, bool>> _users)
        {
            Tuple<User, bool> userToDelete = null;
            for (var i = 0; i < _users.Count; i++)
            {
                if (_users[i].Item1.GetUsername() == username)
                {
                    if (_users[i].Item1.GetPassword() == password)
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
    }
}
