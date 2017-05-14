using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using server.Models;

namespace server.Controllers
{
    public class UserController : ApiController
    {
        // GetRank -->GET: api/User/elad
        public int Get(string username)
        {
            try
            {
                return WebApiConfig.UserManger.GetRank(username);
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
                        WebApiConfig.UserManger.Logout(username);
                        break;
                    case "isloggedin":
                        WebApiConfig.UserManger.IsUserLoggedInn(username);
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
                        WebApiConfig.UserManger.DeleteUser(username, passwordOrRank);
                        break;
                    case "changerank":
                        WebApiConfig.UserManger.ChangeRank(username,Int32.Parse(passwordOrRank) );
                        break;
                    case "register":
                        WebApiConfig.UserManger.Register(username, passwordOrRank);
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
        // login -->POST: api/Login
        public UserData Post([FromBody]UserData value)
        {
            var ret = new UserData();
            try
            {
                var u = WebApiConfig.UserManger.Login(value.Username, value.Password);
                ret.AvatarPath = u.GetAvatar();
                ret.Chips = u.ChipsAmount;
                ret.Email = u.GetEmail();
                ret.Password = u.GetPassword();
                ret.Rank = u.League;
                ret.Username = u.GetUsername();
                ret.Wins = u.Wins;
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
            }

            return ret;
        }
        //editUser --> POST: api/Login?username=elad
        public string Post([FromBody]UserData value, string username)
        {
            try
            {
                WebApiConfig.UserManger.EditUser(username, value.Username, value.Password, value.AvatarPath, value.Email);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "";
        }


        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
