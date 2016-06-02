using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    internal class ConfigurationSeparationCheckListViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<ConfigSeperationCheckList> ConfigSeperationCheckList { get; set; }

        //[DisplayName("Reason For : ")]

        public List<ConfigSeperationCheckList> reasonDetails { get; set; }

        public int? CountRecord { get; set; }

        public List<TagList> ReasonFor { get; set; }

        public int QuestionnaireID { get; set; }

        public string QuestionnaireName { get; set; }

        public string QuestionnaireDescription { get; set; }

        public int? TagID { get; set; }

        public string tag { get; set; }
    }

    public class ConfigSeperationCheckList
    {
        public int QuestionnaireID { get; set; }

        [Display(Name = "Reason : ")]
        public string QuestionnaireName { get; set; }

        public int? TagID { get; set; }

        public bool isChecked { get; set; }

        public string tag { get; set; }
    }

    //public class TagList
    //{
    //    public int? TagID { get; set; }

    //    public string Tag { get; set; }
    //}
}