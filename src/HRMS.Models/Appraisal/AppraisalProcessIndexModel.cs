using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class AppraisalProcessIndexModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<ActionCount> ActionsCountList { get; set; }
    }

    public class ActionCount
    {
        public int EmployeeId { get; set; }

        public int? Appraiser1 { get; set; }

        public int? Reviewer1 { get; set; }

        public int? Appraiser2 { get; set; }

        public int? Reviewer2 { get; set; }

        public int? GroupHead { get; set; }

        public int? StageId { get; set; }

        public DateTime? IDFInitiatedOn { get; set; }

        public DateTime? IDfFrozenOn { get; set; }

        public DateTime? AppraisalYearFrozenOn { get; set; }
    }
}