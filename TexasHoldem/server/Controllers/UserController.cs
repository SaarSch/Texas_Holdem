using System;
using System.Linq;
using System.Web.Http;
using Server.Models;

namespace Server.Controllers
{
    public class UserController : ApiController
    {
        // logout -->GET: api/User?username=elad&mode=logout
        //mode: logout | isloggedin
        public string Get(string username, string mode, string token)
        {
            try
            {
                Server.CheckToken(token);
                switch (mode)
                {
                    case "logout":
                        if (Server.UserFacade.Logout(Crypto.Decrypt(username)))
                            Server.GuidDic.Remove(token);
                        break;
                    case "isloggedin":
                        Server.UserFacade.IsUserLoggedInn(Crypto.Decrypt(username));
                        break;
                    default:
                        throw new Exception("comunication error: unkown mode");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("you are logged in too long, please log out"))
                    if (Server.UserFacade.Logout(Crypto.Decrypt(username)))
                        Server.GuidDic.Remove(token);
                return e.Message;
            }
            return "";
        }

        // DeleteUser -->GET: api/User?username=elad&passwordOrRank=123456&mod=delete
        //mode: delete  | register
        public string Get(string username, string passwordOrRank, string mode, string token)
        {
            try
            {
                switch (mode)
                {
                    case "delete":
                        Server.CheckToken(token);
                        Server.UserFacade.DeleteUser(Crypto.Decrypt(username), Crypto.Decrypt(passwordOrRank));
                        break;
                    case "register":
                        Server.UserFacade.Register(Crypto.Decrypt(username), Crypto.Decrypt(passwordOrRank));
                        break;
                    default:
                        throw new Exception("comunication error: unkown mode");
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }

        public UserData Put(string userName)
        {
            var ret = new UserData();
            try
            {
                var u = Server.UserFacade.GetUser(Crypto.Decrypt(userName));
                ret.AvatarPath = u.AvatarPath;
                ret.Chips = u.ChipsAmount;
                ret.Email = Crypto.Encrypt(u.Email);
                ret.Password = Crypto.Encrypt(u.Password);
                ret.Rank = u.League;
                ret.Username = Crypto.Encrypt(u.Username);
                ret.Wins = u.Wins;
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }


        // login -->POST: api/User
        public UserData Post([FromBody] UserData value)
        {
            var ret = new UserData();
            try
            {
                var u = Server.UserFacade.Login(Crypto.Decrypt(value.Username), Crypto.Decrypt(value.Password));
                ret.AvatarPath = u.AvatarPath;
                ret.Chips = u.ChipsAmount;
                ret.Email = Crypto.Encrypt(u.Email);
                ret.Password = Crypto.Encrypt(u.Password);
                ret.Rank = u.League;
                ret.Username = Crypto.Encrypt(u.Username);
                ret.Wins = u.Wins;
                var g = Guid.NewGuid();
                while (g.ToString().Contains('&') || g.ToString().Contains('+') || g.ToString().Contains('=') ||
                       g.ToString().Contains(':')
                       || g.ToString().Contains('/') || g.ToString().Contains('?') || g.ToString().Contains('#') ||
                       g.ToString().Contains('@') || g.ToString().Contains('!')
                       || g.ToString().Contains('$') || g.ToString().Contains('\'') || g.ToString().Contains('(') ||
                       g.ToString().Contains(')')
                       || g.ToString().Contains('*') || g.ToString().Contains(',') || g.ToString().Contains(';'))
                    g = Guid.NewGuid();
                ret.token = g.ToString();
                Server.GuidDic.Add(g.ToString(), new Tuple<string, DateTime>(value.Username, DateTime.Now));
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }

        //editUser --> POST: api/User?username=elad
        public UserData Post([FromBody] UserData value, string username, string token)
        {
            var ret = new UserData();
            try
            {
                Server.CheckToken(token);
                var u = Server.UserFacade.EditUser(Crypto.Decrypt(username), Crypto.Decrypt(value.Username),
                    Crypto.Decrypt(value.Password), value.AvatarPath, Crypto.Decrypt(value.Email));
                if (u != null)
                {
                    ret.AvatarPath = u.AvatarPath;
                    ret.Chips = u.ChipsAmount;
                    ret.Email = Crypto.Encrypt(u.Email);
                    ret.Password = Crypto.Encrypt(u.Password);
                    ret.Rank = u.League;
                    ret.Username = Crypto.Encrypt(u.Username);
                    ret.Wins = u.Wins;
                }
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }
    }
}