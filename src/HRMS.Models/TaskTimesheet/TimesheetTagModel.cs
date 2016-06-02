using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class TimesheetTagModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<ActiveProjectList> TagProjectList { get; set; }
        public int? ProjectID { get; set; }
        public string UserName { get; set; }
        public string SelectedTagName { get; set; }
        public DateTime? TagStartDate { get; set; }
        public DateTime? TagEndDate { get; set; }
        public string TagLevel { get; set; }
        public int TagNameId { get; set; }
        public string TagName { get; set; }
        public string HiddenTagLevel { get; set; }
    }

    public class ActiveProjectList
    {
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
    }
}