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
        //mode: delete | changerank
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
