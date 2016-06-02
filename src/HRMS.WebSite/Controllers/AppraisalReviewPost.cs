using System.Collections.Generic;

namespace HRMS.Controllers
{
    public class AppraisalReviewPost
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public Dictionary<string, Dictionary<string, string>> param { get; set; }
        public object Data { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Questions { get; set; }
        public string Appraiser1Comment { get; set; }
        public string Appraiser2Comment { get; set; }
    }

    public class AppraisalReviewList
    {
        public Appraisee Appriasee { get; set; }
        public List<AppraisalReviewPost> sections { get; set; }
    }

    public class Appraisee
    {
        public string FullName { get; set; }
        public int EmployeeCode { get; set; }
    }
}