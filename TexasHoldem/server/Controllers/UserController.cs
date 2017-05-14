using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // logout -->GET: api/User/username
        public string Get(string username)
        {
            try
            {
                WebApiConfig.UserManger.Logout(username);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
        // DeleteUser -->GET: api/User?username=elad&passwordOrRank=123456&mod=delete
        //mod: delete | isloggedin | changerank
        public string Get(string username,string passwordOrRank ,string mode)
        {
            try
            {
                switch (mode)
                {
                    case "delete":
                        WebApiConfig.UserManger.DeleteUser(username, passwordOrRank);
                        break;
                    case "isloggedin":
                        WebApiConfig.UserManger.IsUserLoggedInn(username, passwordOrRank);
                        break;
                    case "changerank":
                        WebApiConfig.UserManger.ChangeRank(username,Int32.Parse(passwordOrRank) );
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
        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
