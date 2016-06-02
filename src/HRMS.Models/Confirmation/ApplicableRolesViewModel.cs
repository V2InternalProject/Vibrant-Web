using System.Collections.Generic;

namespace HRMS.Models
{
    public class ApplicableRolesViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<ApplicableRole> ApplicableRoles { get; set; }
    }

    public class ApplicableRole
    {
        public int? CompetencyID { get; set; }

        public int? RoleID { get; set; }

        public string Role { get; set; }

        public bool Checked { get; set; }
    }
}