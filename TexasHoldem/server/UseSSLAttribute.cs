using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace server
{
    public class UseSSLAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var msg = new HttpResponseMessage();
                msg.StatusCode = HttpStatusCode.Forbidden;
                msg.ReasonPhrase = "SSL Needed!";
                actionContext.Response = msg;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}