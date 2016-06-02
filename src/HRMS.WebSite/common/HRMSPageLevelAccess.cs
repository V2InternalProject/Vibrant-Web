using HRMS.Extensions;
using HRMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS
{
    public class HRMSPageLevelAccess
    {
        public void PageLevelAccess(string PageName)
        {
            var session = HttpContext.Current.Session["AccessRights"];// provide USER ID session variable
            if (session != null)
            {
                var roles = (List<AccessRightMapping>)session;
                if (roles.Where(t => t.Action == PageName).Any())
                {
                    var pageAccessList =
                        (from t in roles
                         where (from r in roles
                                where r.Action == PageName
                                select r.Section).Contains(t.Section)
                         select new MenuModel
                         {
                             ClassName = t.Action == PageName ? "selected" : "",
                             Display = t.Action,
                             Url = "/" + t.ControllerName + "/" + t.ActionKey,
                         }).GroupBy(x => x.Url).Select(y => y.First()).ToList();

                    HttpContext.Current.Session["PageLevelAccess"] = pageAccessList.ToList(); //add code here
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/error/index/You are not authorized to access " + PageName);
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/error/index/Session Expired");
            }
        }
    }
}