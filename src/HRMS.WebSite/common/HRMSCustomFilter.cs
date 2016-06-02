using HRMS.Models;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace HRMS.common
{
    public class HRMSCustomFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            // If the browser session or authentication session has expired...
            if (HttpContext.Current.Session[SessionFilter.EncryptedLoggedinEmployeeId] == null || !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //ValidateSession(SessionFilter.OnActionExecuting);
                ValidateSession();
            }
            HttpRequestBase request = filterContext.HttpContext.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            // If the browser session or authentication session has expired...
            if (HttpContext.Current.Session[SessionFilter.EncryptedLoggedinEmployeeId] == null || !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //ValidateSession(SessionFilter.OnActionExecuted);
                ValidateSession();
            }
            HttpRequestBase request = filterContext.HttpContext.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }

        public void ValidateSession()
        //public void ValidateSession(string filterType)
        {
            HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Your session has expired due to inactivity. Please log back in and try again.'); window.location.href='../../Account/LogOff';</SCRIPT>");
            //if (filterType == SessionFilter.OnActionExecuting)
            //{
            //    ActionExecutingContext filterContext = new ActionExecutingContext();
            //    filterContext.Result = new RedirectResult(System.Configuration.ConfigurationManager.AppSettings["Log-OffURL"]);
            //}
            //else if (filterType == SessionFilter.OnActionExecuted)
            //{
            //    ActionExecutedContext filterContext = new ActionExecutedContext();
            //    filterContext.Result = new RedirectResult(System.Configuration.ConfigurationManager.AppSettings["Log-OffURL"]);
            //}
        }
    }
}