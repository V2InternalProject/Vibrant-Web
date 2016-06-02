using System.Collections.Generic;

namespace HRMS.DAL
{
    public class AppraisalSection
    {
        public int sectionId { get; set; }
        public int yearId { get; set; }
        public int empId { get; set; }
        public int loggedUser { get; set; }
        public string sectionType { get; set; }
        public string commandType { get; set; }
        public string sectionTypeParser { get; set; }
        public string sectionName { get; set; }
        public string saveURL { get; set; }
        public string protocol { get; set; }
        public string isSubmit { get; set; }

        public Dictionary<string, AppraisalQuestions> questions { get; set; }
        public string errorMessage { get; set; }
        public Dictionary<string, Dictionary<string, string>> param { get; set; }
    }
}