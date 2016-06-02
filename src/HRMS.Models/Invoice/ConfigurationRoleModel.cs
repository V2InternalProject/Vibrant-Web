using System.Collections.Generic;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class ConfigurationRoleModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string UserName { get; set; }

        //public List<DocumentCategoryList> DocumentCategoryLists { get; set; }

        public List<MainRoleDetails> MainRoleLists { get; set; }

        public int RoleID { get; set; }

        public string RoleDescription { get; set; }

        public string DocumentCategory { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<string> SelectedItemId { get; set; }

        public bool ProjectCreator { get; set; }

        public bool ResourceAllocator { get; set; }

        public bool IRGenerator { get; set; }

        public bool IRApprover { get; set; }

        public bool IRFinanceApprover { get; set; }

        public string DocumentCategoryAccess { get; set; }

        public bool TimesheetToBeFilled { get; set; }

        public int[] SelectedItems { get; set; }
    }

    public class DocumentCategoryList
    {
        public int DocumentCategoryID { get; set; }

        public string DocumentCategory { get; set; }
    }

    public class MainRoleDetails
    {
        public int RoleID { get; set; }

        public string RoleDescription { get; set; }

        public bool ProjectCreator { get; set; }

        public bool ResourceAllocator { get; set; }

        public bool IRGenerator { get; set; }

        public bool IRApprover { get; set; }

        public bool IRFinanceApprover { get; set; }

        public bool TimesheetToBeFilled { get; set; }

        public int Category { get; set; }

        public string CategoryID { get; set; }

        public string DocumentCategoryAccess { get; set; }
    }
}