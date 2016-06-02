namespace HRMS.Models
{
    public class EmpoyeeDependentDetails
    {
    }

    //Travel Details contains Passport and Visa Details
    public class EmployeeDependentTravelDetails
    {
        public int DependantId { get; set; }

        public string DependantName { get; set; }

        public string RelationType { get; set; }
    }
}