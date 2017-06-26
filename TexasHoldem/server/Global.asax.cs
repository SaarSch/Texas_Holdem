using System.Web;
using System.Web.Http;

namespace Server
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(global::Server.Server.Register);
        }
    }
}