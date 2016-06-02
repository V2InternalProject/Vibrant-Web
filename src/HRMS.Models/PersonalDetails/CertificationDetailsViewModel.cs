using System.Collections.Generic;

namespace HRMS.Models
{
    public class CertificationDetailsViewModel
    {
        public int? EmployeeID { get; set; }

        public List<CertificationDetails> CertificationsList { get; set; }

        public CertificationDetails NewCertification { get; set; }

        public List<Certification> CertificationNameList { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public string CertificationName { get; set; }

        public int SelectedCertificationID { get; set; }
    }

    public class Certification
    {
        public int CertificationID { get; set; }
        public string CertificationName { get; set; }
    }
}