using System;
using System.Web.Http;
using Server.Models;

namespace Server.Controllers
{
    public class UserController : ApiController
    {
        // GetRank -->GET: api/User/elad
        public int Get(string username)
        {
            try
            {
                return Server.UserFacade.GetRank(username);
            }
            catch
            {
                return -1;
            }
             
        }
        // logout -->GET: api/User?username=elad&mode=logout
        //mode: logout | isloggedin
        public string Get(string username, string mode)
        {
            try
            {
                switch (mode)
                {
                    case "logout":
                        Server.UserFacade.Logout(username);
                        break;
                    case "isloggedin":
                        Server.UserFacade.IsUserLoggedInn(username);
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
        // DeleteUser -->GET: api/User?username=elad&passwordOrRank=123456&mod=delete
        //mode: delete | changerank | register
        public string Get(string username,string passwordOrRank ,string mode)
        {
            try
            {
                switch (mode)
                {
                    case "delete":
                        Server.UserFacade.DeleteUser(username, passwordOrRank);
                        break;
                    case "register":
                        Server.UserFacade.Register(username, passwordOrRank);
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
                var u = Server.UserFacade.GetUser(userName);
                ret.AvatarPath = u.AvatarPath;
                ret.Chips = u.ChipsAmount;
                ret.Email = u.Email;
                ret.Password = u.Password;
                ret.Rank = u.League;
                ret.Username = u.Username;
                ret.Wins = u.Wins;
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }


        // login -->POST: api/User
        public UserData Post([FromBody]UserData value)
        {
            var ret = new UserData();
            try
            {
                var u = Server.UserFacade.Login(value.Username, value.Password);
                ret.AvatarPath = u.AvatarPath;
                ret.Chips = u.ChipsAmount;
                ret.Email = u.Email;
                ret.Password = u.Password;
                ret.Rank = u.League;
                ret.Username = u.Username;
                ret.Wins = u.Wins;
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }
        //editUser --> POST: api/User?username=elad
        public UserData Post([FromBody]UserData value, string username)
        {
            var ret = new UserData();
            try
            {
               var u= Server.UserFacade.EditUser(username, value.Username, value.Password, value.AvatarPath, value.Email);
                if (u != null)
                {
                    ret.AvatarPath = u.AvatarPath;
                    ret.Chips = u.ChipsAmount;
                    ret.Email = u.Email;
                    ret.Password = u.Password;
                    ret.Rank = u.League;
                    ret.Username = u.Username;
                    ret.Wins = u.Wins;
                }
               
            }
            catch (Exception e)
            {
               ret.Message=e.Message;
            }

            return ret;
        }


        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
