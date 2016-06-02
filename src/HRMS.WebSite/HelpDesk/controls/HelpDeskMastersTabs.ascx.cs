using System;

namespace HRMS.HelpDesk.controls
{
    public partial class HelpDeskMastersTabs : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void UserMaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/ViewEmployeeDetails.aspx", false);
        }

        protected void CategoryMaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/subCategoryMaster.aspx", false);
        }

        protected void SeverityMaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/ProblemSeverityMaster.aspx", false);
        }

        protected void ResolutionTimeMaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/ResolutionTimeMaster.aspx", false);
        }
    }
}