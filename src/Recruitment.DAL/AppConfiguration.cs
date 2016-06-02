using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DAL
{
   public  class AppConfiguration
    {

        public AppConfiguration()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            }
        }

        public static string OrbitConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["OrbitConnectionString"].ConnectionString;
            }
        }

        public static string RoleConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["RoleConnectionString"].ConnectionString;
            }
        }

    }
}
