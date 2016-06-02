using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class addReasonVM
    {
        public int ReasonID { get; set; }

        [Display(Name = "Reason : ")]
        public string Reason { get; set; }

        public int? TagID { get; set; }

        public bool isChecked { get; set; }

        public string tag { get; set; }

        public SearchedUserDetails UserDetails { get; set; }
    }
}