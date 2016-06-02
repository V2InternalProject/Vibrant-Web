using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class RRFListBOL
    {
        public int UserID { get; set; }
        public int RRFID { get; set; }
        public string RoleType { get; set; }
        public int RRFStatus { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string RRFNo { get; set; }
    }
}
