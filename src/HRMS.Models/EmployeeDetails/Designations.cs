namespace HRMS.Models
{
    public class Designations
    {
        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public bool isAdded { get; set; }

        public bool isValidMonth { get; set; }

        public bool isValidEntry { get; set; }
    }
}