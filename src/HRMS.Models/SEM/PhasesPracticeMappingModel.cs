using System.Collections.Generic;

namespace HRMS.Models
{
    public class PhasesPracticeMappingModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<PracticeDetails> PracticeDetailsList { get; set; }
        public List<PhasesPracticeMapping> PhasePracticeMapping { get; set; }
        public int PracticeID { get; set; }
    }

    public class PracticeDetails
    {
        public int PracticeID { get; set; }
        public string PracticeName { get; set; }
    }

    public class PhasesPracticeMapping
    {
        public int PhaseID { get; set; }
        public string PhaseName { get; set; }
        public double PercentageEfforts { get; set; }
        public int OrderNumber { get; set; }
        public bool IsSelected { get; set; }
    }
}