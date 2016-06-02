using System.Collections.Generic;

namespace HRMS.Models
{
    public class AppraisalDesignationsModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<AppraisalDesignation> AppraisalDesignations { get; set; }
    }

    public class AppraisalDesignation
    {
        public int? ParameterID { get; set; }

        public int? DesignationID { get; set; }

        public string Designation { get; set; }

        public bool Checked { get; set; }
    }
}