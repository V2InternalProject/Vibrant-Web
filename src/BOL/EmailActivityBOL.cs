using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class EmailActivityBOL
    {
        public string ToID { get; set; }
       
        public int FromID { get; set; }
        public string EmailTemplateName { get; set; }
        public string CCID { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string[] ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string[] CCAddress { get; set; }
        public string RRFNo { get; set; }
        public string Position { get; set; }
        public string skills { get; set; }
        public string CandidateName { get; set; }
    }
}
