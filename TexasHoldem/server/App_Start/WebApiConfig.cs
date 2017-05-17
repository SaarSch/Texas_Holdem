using System;
using System.Web.Http;
using TexasHoldem.Services;

namespace Server
{
    public static class WebApiConfig
    {
        public static DateTime ChangeLeagues;
        public static GameFacade GameFacade= new GameFacade();
        public static UserFacade UserFacade = new UserFacade();
        public static ReplayFacade ReplayFacade = new ReplayFacade();

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
