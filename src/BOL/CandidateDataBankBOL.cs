using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class CandidateDataBankBOL
    {
        public int CandidateID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Years { get; set; }
        public int UptoYears { get; set; }
        public int Qualifications { get; set; }
        public int NoticePeriod { get; set; }
        public int Status { get; set; }

        //public int TotalWorkExp { get; set; }
        //public int RelevantWorkExp { get; set; }
        //public int HighestQualification { get; set; }
        //public string Skills { get; set; }
    }
}
