using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class RRFApproverBOL
    {
        public string RRFNo { get; set; }
        public int RRFID { get; set; }
        public int RequestedBy { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ExpectedClosureDate { get; set; }
        public int RRFForDU { get; set; }
        public int RRFForDT { get; set; }
        public string ProjectName { get; set; }
        public int Designation { get; set; }
        public int IndicativePanel1 { get; set; }
        public int IndicativePanel2 { get; set; }
        public int IndicativePanel3 { get; set; }
        public int ResourcePool { get; set; }
        public int EmployeementType { get; set; }
        public int PositionsRequired { get; set; }
        public bool IsReplacement { get; set; }
        public int ReplacementFor { get; set; }
        public string KeySkills { get; set; }
        public int Experience { get; set; }
        public string BusinessJustification { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal BudgetPerVacancy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int ApprovedBy { get; set; }
        public int ApprovalStatus { get; set; }
        public int RRFStatus { get; set; }
        public string Comments { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Role { get; set; }
        public bool IsBillable { get; set; }
        public int SLAForSkill { get; set; }
    }
}