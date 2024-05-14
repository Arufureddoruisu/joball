using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            Response.Clear();
            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "NotFound";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        routeData.Values["action"] = "NotFound";
                        break;
                }
            }

            routeData.Values["exception"] = exception;
            Server.ClearError();

            IController errorController = new JobPortal.Controllers.ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
