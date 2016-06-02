namespace HRMS.Models
{
    public class FeedbackChkListVM
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string DeptStage { get; set; }
        public int Count { get; set; }
        public bool isChecked { get; set; }
        //public List<string> checkListFor { get; set; }

        public string CheckList { get; set; }
    }
}