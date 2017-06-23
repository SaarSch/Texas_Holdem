using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Exceptions;
using TexasHoldem.Game;
using TexasHoldem.Loggers;
using TexasHoldem.Users;

namespace TexasHoldem.Logics
{
    public class UserLogic
    {
	    private DatabaseContext db;

	    public UserLogic(string dbName = "TexasDatabase")
	    {
		    db = new DatabaseContext(dbName);
	    }

        public IUser WebLogin(string username, string password)//i will delete it later
        {
            var query = db.Users.Where(u => u.Username == username);

            if (!query.Any())
            {
                Logger.Log(Severity.Error, "ERROR in Login: Username does not exist!");
                return null;
            }
			password= Crypto.Encrypt(password);
			query = query.Where(u => u.Password == password);

            if (!query.Any())
            {
                Logger.Log(Severity.Error, "ERROR in Login: Wrong password!");
                return null;
            }
            IUser fuser = query.FirstOrDefault();
            return fuser;
        }

        public List<Tuple<IUser, bool>> GetAllUsers()
        {
            List<Tuple<IUser, bool>> ans=new List<Tuple<IUser, bool>>();
            foreach (var u in db.Users)
            {
                ans.Add(new Tuple<IUser, bool>(u,false));
            }
            return ans;
        }

        public IUser GetStat(string userName)
        {
            var query = db.Users.Where(u => u.Username == userName);

            if (!query.Any())
            {
                Logger.Log(Severity.Error, "ERROR in GetLoggedInUser: Username does not exist!");
                return null;
            }
            return query.First();
        }

        public void SetLeagues(List<Tuple<IUser, bool>> users)
        {
            var tempUsers = users.Where(user => user.Item1.League != -1).ToList();
            double size = tempUsers.Count / 10;
            var leagueSize = (int)Math.Ceiling(size);
            if (leagueSize == 0) leagueSize = 1;
            var currentSize = 0;
            var league = 10;
            var usersNum = 0;
            var done = new List<IUser>();
            IUser current = null;
            var maxWins = -1;
            for (var i = 0; i < tempUsers.Count; i++)
            {
                foreach (var u in tempUsers)
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
                    var query = db.Users.First(u => u.Username == current.Username);
                    query.League = league;
                    currentSize++;
                    done.Add(current);
                }
                usersNum++;
                if ((currentSize == leagueSize && leagueSize >= 2) || (currentSize > leagueSize))
                {
                    currentSize = 0;
                    league--;
                }
                if (tempUsers.Count == usersNum + 1 && tempUsers.Count % 2 == 1)
                {
                    league++;
                }
                maxWins = -1;
            }
            db.SaveChanges();
        }

        public void Register(string username, string password, List<Tuple<IUser, bool>> users)
        {
            
	        var query = from u in db.Users
						where u.Username == username
						select u;
	        if (query.Any())
	        {
				var e = new IllegalUsernameException("ERROR in Register: Username already exists!");
		        Logger.Log(Severity.Error, e.Message);
		        throw e;
			}
            User.CheckPassWord(password);
            password = Crypto.Encrypt(password);

            User newUser = new User(username, password, "Resources/profilePicture.png", "default@gmail.com", 5000);
	        db.Users.Add(newUser);
	        db.SaveChanges();
            users.Add(new Tuple<IUser, bool>(newUser, false));
            // USERNAME & PASSWORD CONFIRMED
            Logger.Log(Severity.Action, "Registration completed successfully!");
        }

        public IUser Login(string username, string password, List<Tuple<IUser, bool>> users)
        {

            password = Crypto.Encrypt(password);
            var query = db.Users.Where(u => u.Username == username);


            if (!query.Any())
            {
                var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            query = query.Where(u => u.Password == password);


            if (!query.Any())
            {
                var e = new IllegalPasswordException("ERROR in Login: Wrong password!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
          
            Tuple<IUser, bool> foundUser = users.SingleOrDefault(t => t.Item1.Username == username && t.Item1.Password == password);
            if (foundUser == null)
            {
                var e = new IllegalUsernameException("ERROR in Login: itsss nullll.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            if (foundUser.Item2)
            {
                var e = new IllegalUsernameException("ERROR in Login: This User is already logged in.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            
            IUser fuser = foundUser.Item1;
            users.Remove(foundUser);
            users.Add(new Tuple<IUser, bool>(fuser,true));
            //users.Where(u => u.Item1.Username == username).ToList().ForEach(i => i = new Tuple<IUser, bool>(foundUser.Item1, true));
            Logger.Log(Severity.Action, username + " logged in successfully!");

            return fuser;
        }


        public bool Logout(string username, List<Tuple<IUser, bool>> users)
        {
            //int i;
            //var exist = false;
            var query = db.Users.Where(u => u.Username == username);

            if (!query.Any())
            {
                var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }


            Tuple<IUser, bool> foundUser = users.Single(t => t.Item1.Username == username);
            if (!foundUser.Item2)
            {
                var e = new IllegalUsernameException("ERROR in Logout: User is already logged off.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }

            var userRooms = GameCenter.GetGameCenter().Rooms.Where(r => r.IsInRoom(username)).ToList();
            foreach (Room r in userRooms)
            {
                r.ExitRoom(username);
            }

            var userRoomsSpectate = GameCenter.GetGameCenter().Rooms.Where(r => r.Isspectator(username)).ToList();
            foreach (Room r in userRoomsSpectate)
            {
                r.ExitSpectator(username);
            }

            IUser fuser = foundUser.Item1;
            users.Remove(foundUser);
            users.Add(new Tuple<IUser, bool>(fuser, false));
           // users.Where(u => u.Item1.Username == username).ToList().ForEach(i=>i= new Tuple<IUser, bool>(foundUser.Item1, false));
            //for (i = 0; i < users.Count; i++)
            //{
            //    if (users[i].Item1.Username == username)
            //    {
            //        if (!users[i].Item2)
            //        {
            //            var e = new IllegalUsernameException("ERROR in Logout: User is already logged off.");
            //            Logger.Log(Severity.Error, e.Message);
            //            throw e;
            //        }
            //        exist = true;
            //        users[i] = new Tuple<IUser, bool>(users[i].Item1, false);
            //    }
            //}

            //if (!exist)
            //{
            //    var e = new IllegalUsernameException("ERROR in Logout: Username does not exist!");
            //    Logger.Log(Severity.Error, e.Message);
            //    throw e;
            //}
            Logger.Log(Severity.Action, username + " logged out successfully!");
            return true;
        }

        public void DeleteAllUsers(List<Tuple<IUser, bool>> users)
        {
            foreach (var u in users)
            {
                db.Users.Remove((User)u.Item1);
            }

            users.Clear();
            db.SaveChanges();
        }

        public IUser EditUser(string username, string newUserName, string newPassword, string newAvatarPath, string newEmail, List<Tuple<IUser, bool>> users)
        {

            if (newEmail != "")
            {
                User.CanSetMail(newEmail);
            }
            if (newUserName != "")
            {
                User.CanSetUserName(newUserName);
            }
            if (newPassword != "")
            {
                User.CanSetPass(newPassword);
                newPassword = Crypto.Encrypt(newPassword);
            }
            //IUser ans= null;
            var query = db.Users.Where(u => u.Username == username);

            if (!query.Any())
            {
                var e = new IllegalUsernameException("ERROR in Edit Profile: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            var user= db.Users.First(u => u.Username == username);
            Tuple<IUser, bool> foundUser = users.Single(t => t.Item1.Username == username);
            IUser ans = foundUser.Item1;
            if (!foundUser.Item2)
            {
                var e = new IllegalUsernameException("ERROR in Edit Profile: This User is not logged in.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            try
            {
                if (newUserName != "" && newUserName != username)
                {
                    query = db.Users.Where(u => u.Username == newUserName);
                    if (query.Any())
                    {
                        var e = new IllegalUsernameException("ERROR in Edit Profile: New username already exists!");
                        Logger.Log(Severity.Error, e.Message);
                        throw e;
                    }
                    ans=new User(ans,newUserName);
                    users.Remove(foundUser);
                    db.Users.Remove(user);
                    users.Add(new Tuple<IUser, bool>(ans, true));
                    foundUser = users.Single(t => t.Item1.Username == newUserName);
                    db.Users.Add((User)ans);
                    db.SaveChanges();
                    user = db.Users.First(u => u.Username == newUserName);
                    //user.Username = newUserName;
                }
                if (newPassword != "")
                {
                    ans.Password = newPassword;//check if it changes
                    user.Password = newPassword;
                }
                  
                if (newAvatarPath != "")
                {
                    ans.AvatarPath = newAvatarPath;
                    user.AvatarPath = newAvatarPath;
                }
                   
                if (newEmail != "")
                {
                    ans.Email = newEmail;
                    user.Email = newEmail;
                }
                   
            }
            catch (Exception ex)
            {
                
                var e = new Exception("ERROR in Edit Profile: Invalid new user details!"+ex.Message);
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            //var userExists = false;
            //for (var i = 0; i < users.Count; i++)
            //{
            //    if (users[i].Item1.Username == username)
            //    {
            //        ans = users[i].Item1;
            //        userExists = true;
            //        if (!users[i].Item2)
            //        {
            //            var e = new IllegalUsernameException("ERROR in Edit Profile: This User is not logged in.");
            //            Logger.Log(Severity.Error, e.Message);
            //            throw e;
            //        }
            //        try
            //        {
            //            if (newUserName != null&&newUserName!=username)
            //            {
            //                for (var j = 0; j < users.Count; j++)
            //                {
            //                    if (users[j].Item1.Username == newUserName)
            //                    {
            //                        var e = new IllegalUsernameException("ERROR in Edit Profile: New username already exists!");
            //                        Logger.Log(Severity.Error, e.Message);
            //                        throw e;
            //                    }
            //                }
            //                users[i].Item1.Username = newUserName;
            //            }
            //            if (newPassword != null)
            //                users[i].Item1.Password = newPassword;
            //            if (newAvatarPath != null)
            //                users[i].Item1.AvatarPath = newAvatarPath;
            //            if (newEmail != null)
            //                users[i].Item1.Email = newEmail;
            //        }
            //        catch (Exception)
            //        {
            //            var e = new Exception("ERROR in Edit Profile: Invalid new user details!");
            //            Logger.Log(Severity.Error, e.Message);
            //            throw e;
            //        }
            //    }
            //}

            //if (!userExists)
            //{
            //    var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
            //    Logger.Log(Severity.Error, e.Message);
            //    throw e;
            //}
            Logger.Log(Severity.Action, username + "'s profile edited successfully!");
            db.SaveChanges();
            return ans;
        }

        public IUser GetLoggedInUser(string username,List<Tuple<IUser, bool>> users)
        {
            var query = db.Users.Where(u => u.Username == username);

            if (!query.Any())
            {
                var e = new IllegalUsernameException("ERROR in GetLoggedInUser: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            Tuple<IUser, bool> foundUser = users.Single(t => t.Item1.Username == username);
            IUser ans = foundUser.Item1;
            if (!foundUser.Item2)
            {
                var e = new IllegalUsernameException("ERROR in GetLoggedInUser: This User is not logged in.");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            return ans;
            //for (var i = 0; i < users.Count; i++)
            //{
            //    if (users[i].Item1.Username == username)
            //    {
            //        if (!users[i].Item2)
            //        {
            //            var err = new IllegalUsernameException("ERROR in GetLoggedInUser: This User is not logged in.");
            //            Logger.Log(Severity.Error, err.Message);
            //            throw err;
            //        }
            //        return users[i].Item1;
            //    }
            //}
            //var e = new IllegalUsernameException("ERROR in Edit Profile: This user doesn't exist.");
            //Logger.Log(Severity.Error, e.Message);
            //throw e;
        }

        public IUser GetUser(string username)
        {
            var query = db.Users.Where(u => u.Username == username);

            if (!query.Any())
            {
                var e = new IllegalUsernameException("ERROR in GetLoggedInUser: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            return query.First();
        }

        public IUser GetUser(string username, List<Tuple<IUser, bool>> users)
        {
            return GetUser(username);
            //var query = db.Users.Where(u => u.Username == username);

            //if (!query.Any())
            //{
            //    var e = new IllegalUsernameException("ERROR in GetLoggedInUser: Username does not exist!");
            //    Logger.Log(Severity.Error, e.Message);
            //    throw e;
            //}
            ////Tuple<IUser, bool> foundUser = users.Single(t => t.Item1.Username == username);
            ////IUser ans = foundUser.Item1;
            //return query.First();
            //for (var i = 0; i < users.Count; i++)
            //{
            //    if (users[i].Item1.Username == username)
            //    {

            //        return users[i].Item1;
            //    }
            //}
            //var e = new IllegalUsernameException("ERROR in GetUser: This User doesn't exist.");
            //Logger.Log(Severity.Error, e.Message);
            //throw e;
        }

        public void DeleteUser(string username, string password, List<Tuple<IUser, bool>> users)
        {
            password = Crypto.Encrypt(password);
            var query = db.Users.Where(u => u.Username == username);

            if (!query.Any())
            {
                var e = new IllegalUsernameException("ERROR in DeleteUser: Username does not exist!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            query = query.Where(u => u.Password == password);

            if (!query.Any())
            {
                var e = new IllegalPasswordException("ERROR in DeleteUser: Wrong password!");
                Logger.Log(Severity.Error, e.Message);
                throw e;
            }
            //Tuple<IUser, bool> userToDelete = null;
            //for (var i = 0; i < users.Count; i++)
            //{
            //    if (users[i].Item1.Username == username)
            //    {
            //        if (users[i].Item1.Username == password)
            //        {
            //            userToDelete = users[i];
            //        }
            //        else
            //        {
            //            var e = new IllegalPasswordException("ERROR in Edit Profile: Wrong password!");
            //            Logger.Log(Severity.Error, e.Message);
            //            throw e;
            //        }
            //    }
            //}

            //if (userToDelete == null)
            //{
            //    var e = new IllegalUsernameException("ERROR in Login: Username does not exist!");
            //    Logger.Log(Severity.Error, e.Message);
            //    throw e;
            //}
            Tuple<IUser, bool> foundUser = users.Single(t => t.Item1.Username == username && t.Item1.Password == password);
            users.Remove(foundUser);
            db.Users.Remove(query.First());
            db.SaveChanges();
            Logger.Log(Severity.Action, "User: " + username + " deleted successfully!");
        }

	    public void UpdateDB()
	    {
		    db.SaveChanges();
	    }
    }
}
