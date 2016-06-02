using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class JoinEmployeeBOL
    {
        public int IsContract { get; set; }
        public int Employeecode { get; set; }
        public string EmailId { get; set; }
        public string UserName {get; set;}
        public int CandidateId { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
