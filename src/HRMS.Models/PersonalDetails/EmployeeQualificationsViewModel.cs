using System.Collections.Generic;

namespace HRMS.Models
{
    public class EmployeeQualificationsViewModel
    {
        public int? EmployeeID { get; set; }

        public List<EmployeeQualifications> EmployeeQualificationsList { get; set; }

        public EmployeeQualifications NewEmployeeQualification { get; set; }

        public List<DegreeListClass> DegreeList { get; set; }

        public List<TypeListClass> TypeList { get; set; }

        public List<QualificationListClass> QualificationList { get; set; }

        public List<YearListClass> YearList { get; set; }                       // Added Year Drop Down List

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public int bithDate { get; set; }

        public int QualificationID { get; set; }
        public int DegreeID { get; set; }
        public int YearID { get; set; }
        public int TypeID { get; set; }
    }

    public class YearListClass
    {
        public int YearID { get; set; }
        public string Year { get; set; }
    }

    public class DegreeListClass
    {
        public int DegreeID { get; set; }
        public string Degree { get; set; }
    }

    public class TypeListClass
    {
        public int TypeID { get; set; }
        public string Type { get; set; }
    }

    public class QualificationListClass
    {
        public int QualificationID { get; set; }
        public string Qualification { get; set; }
    }
}