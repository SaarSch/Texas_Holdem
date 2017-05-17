using System.Web.Http;
using TexasHoldem.Services;

namespace server
{
    public static class WebApiConfig
    {
        public static GameFacade GameManger= new GameFacade();
        public static UserFacade UserManger = new UserFacade();
        public static ReplayFacade ReplayManger = new ReplayFacade();

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
