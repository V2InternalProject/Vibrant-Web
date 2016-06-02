namespace HRMS.DAL
{
    public class AppraisalQuestions
    {
        public int questionId { get; set; }
        public string questionText { get; set; }
        public int seq { get; set; }
        public string dataType { get; set; }
        public string controlType { get; set; }
        public bool isRequired { get; set; }
        public QuestionValidation validation { get; set; }
    }

    public class QuestionValidation
    {
        public string testValue { get; set; }
        public string failureMessage { get; set; }
    }
}