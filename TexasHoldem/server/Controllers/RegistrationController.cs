using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                WebApiConfig.userManger.Register(value.username, value.password);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
    }
}
