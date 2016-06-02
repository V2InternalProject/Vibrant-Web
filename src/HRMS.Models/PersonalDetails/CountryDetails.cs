using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.PersonalDetails
{
    public class CountryDetails
    {
        [Required]
        public int CountryId { get; set; }

        [Required]
        public string CountryName { get; set; }
    }
}