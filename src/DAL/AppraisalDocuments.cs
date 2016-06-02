namespace HRMS.DAL
{
    public partial class tbl_ApprisalDocuments : IAppraisalDocuments
    {
        public bool IsParent
        {
            get { return true; }
        }
    }

    public partial class tbl_ApprisalDocumentDetail : IAppraisalDocuments
    {
        public bool IsParent
        {
            get { return false; }
        }
    }
}