using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalParametersModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<AppraisalParameterMaster> AppraisalParameterMaster { get; set; }

        [Display(Name = "Total Records : ")]
        public int ParameterRecordsCount { get; set; }

        public int AppraisalYearID { get; set; }
    }

    public class AppraisalParameterMaster
    {
        public int ParameterID { get; set; }

        public string Parameter { get; set; }

        public string ParameterDescription { get; set; }

        public int? OrderNo { get; set; }

        public int? ParameterCategoryID { get; set; }

        public bool AppraisalParameterChecked { get; set; }

        public int AppraisalYearID { get; set; }
    }
}