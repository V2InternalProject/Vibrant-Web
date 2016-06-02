using System;

namespace HRMS.HelpDesk.controls
{
    public partial class HelpDeskReportsTabs : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void IssueMembership_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/MemberWiseSearchReport.aspx", false);
        }

        protected void IssueDepartmentwise_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/CategoryWiseSearchReport.aspx", false);
        }

        protected void ResolutionTime_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/ResolutionTimeReport.aspx", false);
        }

        protected void SeverityWise_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/IssueStatusReport.aspx", false);
        }

        protected void SummaryReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/SummaryReport.aspx", false);
        }

        protected void SLAReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/SLAReport.aspx", false);
        }
    }
}