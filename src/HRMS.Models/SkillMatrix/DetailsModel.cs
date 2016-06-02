namespace HRMS.Models.SkillMatrix
{
    public class DetailsModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? EmployeeCode { get; set; }

        public int? ResourcePoolID { get; set; }

        public string ResourcePoolName { get; set; }

        public int? Id { get; set; }

        public int? ToolId { get; set; }

        public string SkillName { get; set; }

        public string Rating { get; set; }
        public string NewRating { get; set; }

        public int? RatingId { get; set; }

        public int? ProjectEmployeeRoleID { get; set; }

        public int? ProjectSkillMatrixFormStatus { get; set; }  //added by sat  //added by sat

        public string Remark { get; set; }
    }
}