namespace HRMS.DAL
{
    public class AppraisalSectionPost
    {
        public int SectionId { get; set; }
        public string SectionTypeParser { get; set; }
        public int EmpId { get; set; }
        public int LoggedUser { get; set; }
        public int YearId { get; set; }
        public object Data { get; set; }
    }
}