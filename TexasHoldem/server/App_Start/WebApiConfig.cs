using System.Web.Http;
using TexasHoldem.Services;

namespace server
{
    public static class WebApiConfig
    {
        public static GameManager GameManger= new GameManager();
        public static UserManager UserManger = new UserManager();
        public static ReplayManager ReplayManger = new ReplayManager();

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
