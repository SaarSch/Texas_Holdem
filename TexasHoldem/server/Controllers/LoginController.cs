using server.Models;
using System;
using System.Web.Http;

namespace server.Controllers
{
    public class LoginController : ApiController
    {
        // POST: api/Login
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
                ret.Message= e.Message;
            }

            return ret;
        }
        // POST: api/Login?username=elad
        public string Post([FromBody]UserData value,string username)
        {
            try
            {
                WebApiConfig.UserManger.EditUser(username, value.Username,value.Password,value.AvatarPath,value.Email);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "";
        }
    }
}
