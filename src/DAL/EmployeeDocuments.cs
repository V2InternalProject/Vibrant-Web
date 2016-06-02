namespace HRMS.DAL
{
    public partial class Tbl_Employee_Documents : IDocuments
    {
        public bool IsParent
        {
            get { return true; }
        }
    }

    public partial class Tbl_Employee_DocumentDetail : IDocuments
    {
        public bool IsParent
        {
            get { return false; }
        }
    }
}