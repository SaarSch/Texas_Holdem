using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TexasHoldem.Services;

namespace server
{
    public static class WebApiConfig
    {
        public static GameManager gameManger= new GameManager();
        public static UserManager userManger = new UserManager();
        public static ReplayManager replayManger = new ReplayManager();

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
