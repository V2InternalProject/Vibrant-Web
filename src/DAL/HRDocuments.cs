namespace HRMS.DAL
{
    public partial class Tbl_HR_Documents : IDocuments
    {
        public bool IsParent
        {
            get { return true; }
        }
    }

    public partial class Tbl_HR_DocumentDetail : IDocuments
    {
        public bool IsParent
        {
            get { return false; }
        }
    }
}