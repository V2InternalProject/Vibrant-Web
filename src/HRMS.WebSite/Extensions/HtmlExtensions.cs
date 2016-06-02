using HRMS.Extensions;
using HRMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString HorizontalMenu(this HtmlHelper helper, List<MenuModel> menuList)
        {
            var link = "<a href='{0}' class='{1}'>{2}</a>";
            StringBuilder builder = new StringBuilder();
            if (menuList != null)
            {
                foreach (var current in menuList)
                {
                    builder.Append(string.Format(
                        link,
                        current.Url,
                        current.ClassName,
                        current.Display
                        ));
                }
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString PMSMenu(this HtmlHelper helper)
        {
            var roles = (List<AccessRightMapping>)HttpContext.Current.Session["AccessRights"];
            var pmsList = roles.Where(p => p.Area == "PMS").ToList();

            var link = "<li class='{3}'><a href='{0}' class='{1}'>{2}</a></li>";
            StringBuilder builder = new StringBuilder();
            if (pmsList != null)
            {
                foreach (var current in pmsList)
                {
                    builder.Append(string.Format(
                        link,
                        "/" + current.ControllerName + "/" + current.ActionKey,
                        "pms",
                        current.Action,
                        "submenu1"
                        ));
                }
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}