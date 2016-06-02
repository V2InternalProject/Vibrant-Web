using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class HRMBOL
    {
        public int RRFID { get; set; }
       // public string RecruiterName { get; set; }
        public int RecruiterID { get; set; }
        public int AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public int SLAType { get; set; }
    }
}
