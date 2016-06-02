using System.Web.Http;

namespace HRMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{method}/{param}"
            //);
            config.Routes.MapHttpRoute("fileapi", "api/{controller}/{action}"
                );

            config.Routes.MapHttpRoute("Appraisalapi1", "api/{controller}/{action}/{sectionTypeParser}/{sectionId}/for/{empID}"
                    );
            config.Routes.MapHttpRoute("Appraisalapi", "api/{controller}/{action}/{sectionId}/for/{empID}"
                    );
            config.Routes.MapHttpRoute(
                name: "newfileapi",
                routeTemplate: "api/{controller}/{action}/{empId}"
            );
            config.Routes.MapHttpRoute(
                name: "newfileapiId",
                routeTemplate: "api/{controller}/{action}/for/{ID}"
            );
            config.Routes.MapHttpRoute(
                name: "twoparamfileapi",
                routeTemplate: "api/{controller}/{action}/{YearID}/{sectionId}",
                defaults: "api/{controller}/{action}/{YearID}/{sectionId}"
            );
        }
    }
}