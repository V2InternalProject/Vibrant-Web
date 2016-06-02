using HRMS.Extensions;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Recruitment.UserControl
{
    public partial class RecruitementHeader : System.Web.UI.UserControl
    {
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            var session = HttpContext.Current.Session["AccessRights"];// provide USER ID session variable
            if (session != null)
            {
                var roles = (List<AccessRightMapping>)session;

                var pageAccessList =
                      (from t in roles
                       where (from r in roles
                              where r.Section == "Talent Acquisition"
                              select r.Section).Contains(t.Section)
                       select new MenuModel
                       {
                           ActionName = t.Action
                       }).GroupBy(x => x.ActionName).Select(y => y.First()).ToList();

                HttpContext.Current.Session["SmarttrackActions"] = pageAccessList.ToList();

                var smartt = HttpContext.Current.Session["SmarttrackActions"];
                var smarttack = (List<MenuModel>)smartt;

                MenuModel RecruiterRole = smarttack.Find(item => item.ActionName == "Recruiter");
                MenuModel HRMRole = smarttack.Find(item => item.ActionName == "HRM List");
                MenuModel RequestorRole = smarttack.Find(item => item.ActionName == "Requestor");
                MenuModel ApproverRole = smarttack.Find(item => item.ActionName == "RRF Approver");
                MenuModel InterviewerRole = smarttack.Find(item => item.ActionName == "Interviewer");
                MenuModel MasterTableRole = smarttack.Find(item => item.ActionName == "Master Table");
                MenuModel CandidateRole = smarttack.Find(item => item.ActionName == "Candidate");

                Session["RecruiterRole"] = null;
                Session["HRMRole"] = null;
                Session["RequestorRole"] = null;
                Session["ApproverRole"] = null;
                Session["InterviewerRole"] = null;
                Session["MasterTableRole"] = null;
                Session["CandidateRole"] = null;

                if (RecruiterRole != null)
                {
                    Session["RecruiterRole"] = RecruiterRole.ActionName;
                }
                if (HRMRole != null)
                {
                    Session["HRMRole"] = HRMRole.ActionName;
                }
                if (RequestorRole != null)
                {
                    Session["RequestorRole"] = RequestorRole.ActionName;
                }
                if (ApproverRole != null)
                {
                    Session["ApproverRole"] = ApproverRole.ActionName;
                }
                if (InterviewerRole != null)
                {
                    Session["InterviewerRole"] = InterviewerRole.ActionName;
                }
                if (MasterTableRole != null)
                {
                    Session["MasterTableRole"] = MasterTableRole.ActionName;
                }
                if (CandidateRole != null)
                {
                    Session["CandidateRole"] = CandidateRole.ActionName;
                }

                if (Session["HRMRole"] != null)
                {
                    HRM.Visible = true;
                    HRMTemp.Visible = true;
                }
                if (Session["RecruiterRole"] != null)
                {
                    Recruiter.Visible = true;
                    RecruiterTemp.Visible = true;
                }
                if (Session["RequestorRole"] != null)
                {
                    RRFRequestor.Visible = true;
                    RRFRequestorTemp.Visible = true;
                }
                if (Session["ApproverRole"] != null)
                {
                    RRFApprover.Visible = true;
                    RRFApproverTemp.Visible = true;
                }
                if (Session["InterviewerRole"] != null)
                {
                    Interviewer.Visible = true;
                    InterviewerTemp.Visible = true;
                }
                if (Session["MasterTableRole"] != null)
                {
                    masterTable.Visible = true;
                    masterTableTemp.Visible = true;
                }
                if (Session["CandidateRole"] != null)
                {
                    Candidate.Visible = true;
                    CandidateTemp.Visible = true;
                }
            }
        }

        protected void HRM_Click(object sender, EventArgs e)
        {
            string PageName = "HRM List";
            objpagelevel.PageLevelAccess(PageName);

            Session["Title"] = "HRM RRFList";
            Session["HeaderCheck"] = Session["Title"];
            Response.Redirect("../Recruitment/RRFList.aspx", false);
        }

        protected void RRFRequestor_Click(object sender, EventArgs e)
        {
            string PageName = "Requestor";
            objpagelevel.PageLevelAccess(PageName);

            Session["Title"] = "RRF List";
            Session["HeaderCheck"] = Session["Title"];
            Response.Redirect("../Recruitment/RRFList.aspx", false);
        }

        protected void RRFApprover_Click(object sender, EventArgs e)
        {
            string PageName = "RRF Approver";
            objpagelevel.PageLevelAccess(PageName);

            Session["Title"] = "RRF Approver List";
            Session["HeaderCheck"] = Session["Title"];
            Response.Redirect("../Recruitment/RRFList.aspx", false);
        }

        protected void masterTable_Click(object sender, EventArgs e)
        {
            string PageName = "Master Table";
            objpagelevel.PageLevelAccess(PageName);

            Session["HeaderCheck"] = "master";
            Response.Redirect("../Recruitment/MastersTable.aspx", false);
        }

        protected void Interviewer_Click(object sender, EventArgs e)
        {
            string PageName = "Interviewer";
            objpagelevel.PageLevelAccess(PageName);

            Session["HeaderCheck"] = "Interview";
            Response.Redirect("../Recruitment/Interviewer.aspx", false);
        }

        protected void Candidate_Click(object sender, EventArgs e)
        {
            string PageName = "Candidate";
            objpagelevel.PageLevelAccess(PageName);

            Session["HeaderCheck"] = "Candidate";
            Response.Redirect("../Recruitment/Candidate.aspx", false);
        }

        protected void Recruiter_Click(object sender, EventArgs e)
        {
            string PageName = "Recruiter";
            objpagelevel.PageLevelAccess(PageName);

            Session["HeaderCheck"] = "Recruiter";
            Response.Redirect("../Recruitment/Recruiter.aspx", false);
        }
    }
}