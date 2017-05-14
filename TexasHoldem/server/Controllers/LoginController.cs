using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class LoginController : ApiController
    {
        // POST: api/Login
        public UserData Post([FromBody]UserData value)
        {
            User u;
            UserData ret = new UserData();
            try
            {
                u=WebApiConfig.userManger.Login(value.username, value.password);
                ret.avatarPath = u.GetAvatar();
                ret.chips = u.chipsAmount;
                ret.email = u.GetEmail();
                ret.password = u.GetPassword();
                ret.Rank = u.league;
                ret.username = u.GetUsername();
                ret.wins = u.wins;
            }
            catch (Exception e)
            {
                ret.message= e.Message;
            }

            return ret;
        }
        // POST: api/Login?username=elad
        public string Post([FromBody]UserData value,string username)
        {
            try
            {
                WebApiConfig.userManger.EditUser(username, value.username,value.password,value.avatarPath,value.email);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "";
        }
    }
}
