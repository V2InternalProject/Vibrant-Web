using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfigureParametersforRoles
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<RoleLists> RoleList { get; set; }

        [Display(Name = "Total Records : ")]
        public int RecordsCount { get; set; }
    }

    public class RoleLists
    {
        public int RoleID { get; set; }

        public string RoleDescription { get; set; }
    }
}