using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class DefaultController : ApiController
    {
        // GET: api/Default
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public Class1 Get(string id)
        {
            Class1 a = new Class1();
            a.aa = id;
            a.b = 2;
            return a;
        }



        // POST: api/Default
        public string Post([FromBody]Class1 value,int i)
        {
            return "bulbul";
        }
        public int Post([FromBody]Class1 value)
        {
            return 2;
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
