using System;

namespace HRMS.Orbitweb
{
    public partial class OrbitMastersTabs : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void HolidayList_Click(object sender, EventArgs e)
        {
            Response.Redirect("../OrbitWeb/HolidayList.aspx", false);
        }

        protected void ConfigSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("../OrbitWeb/ConfigItems.aspx", false);
        }

        protected void StatusMaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("../OrbitWeb/Status.aspx", false);
        }

        protected void MonthlyLeaveUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect("../OrbitWeb/MonthlyLeaveUpload.aspx", false);
        }

        protected void ShiftDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("../OrbitWeb/ShiftMaster.aspx", false);
        }
    }
}