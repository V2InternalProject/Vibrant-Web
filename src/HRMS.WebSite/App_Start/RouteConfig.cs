using System.Web.Mvc;
using System.Web.Routing;

namespace HRMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // This informs MVC Routing Engine to send any requests for .aspx page to the WebForms engine
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx");
            routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|gif|jpg)(/.*)?" });
            routes.MapRoute(
                 "Default",
                "{controller}/{action}/{id}",
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //  "EmployeeControllerIndex",
            //  "{controller}/{action}/{employeeId}",
            // new { controller = "EmployeeDetails", action = "Index", employeeId = UrlParameter.Optional }
            routes.MapRoute(
             "EmployeeControllerIndex",
             "{controller}/{action}/{employeeId}",
            new { controller = "EmployeeDetails", action = "Index", employeeId = UrlParameter.Optional }
          );
        }
    }
}