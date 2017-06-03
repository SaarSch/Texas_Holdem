using System;
using System.Collections.Generic;
using System.Web.Http;
using TexasHoldem.Services;

namespace Server
{
    public static class Server
    {
        public static DateTime ChangeLeagues;
        public static GameFacade GameFacade= new GameFacade();
        public static UserFacade UserFacade = new UserFacade();
        public static ReplayFacade ReplayFacade = new ReplayFacade();
        public static Dictionary<Guid, Tuple<string, DateTime>> GuidDic = new Dictionary<Guid, Tuple<string, DateTime>>();

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

        public static void CheckToken(string token)
        {
            if (!GuidDic.ContainsKey(new Guid(token)))
            {
                throw new Exception("token:" + token + "does not exist!!");
            }
            else if(GuidDic[new Guid(token)].Item2.AddHours(8)<DateTime.Now)
            {
                throw new Exception("you are logged in too long, please log out");
            }
        }
    }
}
