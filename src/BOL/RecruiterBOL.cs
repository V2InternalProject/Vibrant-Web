using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace BOL
{
    public class RecruiterBOL
    {
        public int UserID { get; set; }
        public string RRFCode { get; set; }
        public string RoleType { get; set; }
        //public string Requestor { get; set; }
        //public string Position { get; set; }
        //public DateTime RequestedDate { get; set; }
        //public DateTime ExpectedClosureDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string RRFID { get; set; }
        public DateTime RRFAcceptedDate { get; set; }
    }
}
