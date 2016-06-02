using HRMS.DAL;
using System;
using System.Web;

//using System.Web.SessionState;
using System.Web.Security;

namespace HRMS.Helper
{
    public static class HRMSHelper
    {
        public static string Decrypt(string id, out bool isAuthorize)
        {
            try
            {
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string decryptedEmployeeId = string.Empty;
                decryptedEmployeeId = Commondal.Decrypt(Convert.ToString(id), true);
                decryptedEmployeeId = decryptedEmployeeId.Replace(HttpContext.Current.Session["SecurityKey"].ToString(), "");
                isAuthorize = true;
                return decryptedEmployeeId;
            }
            catch (System.Exception ex)
            {
                isAuthorize = false;
                return string.Empty;
            }
        }

        public static int LoggedInEmployeeId()
        {
            try
            {
                EmployeeDAL EmployeeDAL = new EmployeeDAL();
                return EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}