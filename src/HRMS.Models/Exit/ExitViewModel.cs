using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ExitViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<SeperationReasons> seperationReason { get; set; }

        //[DisplayName("Reason For : ")]
        public List<SeperationForCheckList> seperationCheckList { get; set; }

        public List<SeperationReasons> reasonDetails { get; set; }

        public List<ReasonDetail> ReasonList { get; set; }

        public int? CountRecord { get; set; }

        public List<TagList> ReasonFor { get; set; }

        public int ReasonID { get; set; }

        [Required(ErrorMessage = "Reason field is required")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string QuestionnaireDescription { get; set; }

        public int? TagID { get; set; }

        [Required(ErrorMessage = "Reason For field is required")]
        public string tag { get; set; }

        public int QuestionnaireID { get; set; }

        [Required(ErrorMessage = "CheckList is required.")]
        public string QuestionnaireName { get; set; }

        [Required(ErrorMessage = "Reason field is required")]
        public int RevisionID { get; set; }

        public int RevisionNo { get; set; }
        public bool IsExisted { get; set; }
        public bool IsEdited { get; set; }
        public string ExistingReason { get; set; }
    }

    public class ReasonDetail
    {
        public int QuestionnaireID { get; set; }
        public int RevisionID { get; set; }
        public int RevisionNo { get; set; }
        public string Reason { get; set; }
    }

    public class SeperationReasons
    {
        public int ReasonID { get; set; }

        [Display(Name = "Reason : ")]
        public string Reason { get; set; }

        [Display(Name = "Reason Description: ")]
        public string QuestionnaireDescription { get; set; }

        public int? TagID { get; set; }

        public bool isChecked { get; set; }

        public string tag { get; set; }
    }

    public class TagList
    {
        public int? TagID { get; set; }

        public string Tag { get; set; }
    }

    public class SeperationForCheckList
    {
        public int QuestionnaireID { get; set; }
        public string QuestionnaireName { get; set; }
        public string QuestionnaireDescription { get; set; }
        public int RevisionID { get; set; }
        public bool isChecked { get; set; }
    }
}