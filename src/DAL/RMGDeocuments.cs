namespace HRMS.DAL
{
    public partial class Tbl_RMG_Documents : IDocuments
    {
        public bool IsParent
        {
            get { return true; }
        }
    }

    public partial class Tbl_RMG_DocumentDetail : IDocuments
    {
        public bool IsParent
        {
            get { return false; }
        }
    }
}