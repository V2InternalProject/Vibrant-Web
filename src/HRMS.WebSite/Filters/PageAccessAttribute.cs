using HRMS.Extensions;
using HRMS.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication3.Filters
{
    public class PageAccessAttribute : ActionFilterAttribute
    {
        public string PageName { get; set; }
        public JArray RoleLinkInjector { get; set; }

        //public string RoleLink { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session["AccessRights"];// provide USER ID session variable
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

                    //var test = pageAccessList.Distinct().OrderByDescending(t => t.Display).ToList();
                    // var test = pageAccessList.GroupBy(x => x.Url).Select(y => y.First()).ToList();

                    filterContext.HttpContext.Session["PageLevelAccess"] = pageAccessList.ToList(); //add code here

                    base.OnActionExecuting(filterContext);
                }
                else if (PageName == "Generate Invoice")
                {
                    var pageAccessList =
                        (from t in roles
                         where (from r in roles
                                where r.Section == "Invoicing"
                                select r.Section).Contains(t.Section)
                         select new MenuModel
                         {
                             ClassName = t.Action == PageName ? "selected" : "",
                             Display = t.Action,
                             Url = "/" + t.ControllerName + "/" + t.ActionKey,
                         }).GroupBy(x => x.Url).Select(y => y.First()).ToList();

                    filterContext.HttpContext.Session["PageLevelAccess"] = pageAccessList.ToList(); //add code here

                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectResult("/error/index/You are not authorized to access " + PageName);
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/error/index/Session Expired");
            }
        }

        //public List<MenuModel> RoleLinkInjectionProcessor(List<MenuModel> menus, JArray injectionList)
        //{
        //    var user=HttpContext.Current.User;

        //    foreach (var menu in menus)
        //    {
        //        menu.Url += injectionList.Where(t => t["ul"] == menu.Url && user.IsInRole(t["rl"])).First()["qs"];
        //    }
        //    return menus;
        //}
    }
}