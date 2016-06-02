namespace HRMS.Models
{
    public class AddProjectModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public CustomerAddress ProjectOwners { get; set; }
        public CustomerContact ProjectReviewers { get; set; }
        public CustomerContract ProjectDocuments { get; set; }

        //public AddCustomerAddress AddCustomerAddressinvoice { get; set; }
        //public AddContact AddContact { get; set; }
        //public AddContract AddContract { get; set; }
        public CustomerContract IRApprovers { get; set; }

        public CustomerContract IRGenerators { get; set; }
    }
}