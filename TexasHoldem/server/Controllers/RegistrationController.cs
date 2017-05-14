using server.Models;
using System;
using System.Web.Http;

namespace server.Controllers
{
    public class RegistrationController : ApiController
    {
        // POST: api/Registration
        public string Post([FromBody]UserData value)
        {
            try
            {
                WebApiConfig.UserManger.Register(value.Username, value.Password);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
    }
}
