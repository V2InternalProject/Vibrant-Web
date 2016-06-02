using System.Collections.Generic;

namespace HRMS.Models
{
    public class AccessRightsViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<AccessRightsNodeDetails> AccessRightsNodeList { get; set; }
    }

    public class AccessRightsNodeDetails
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<AccessRightsNodeMapping> AccessRightsList { get; set; }
        public List<Nodes> NodesList { get; set; }
        public List<AccessRights> AccessList { get; set; }
    }

    public class Nodes
    {
        public int NodeID { get; set; }
        public string NodeName { get; set; }
    }

    public class AccessRights
    {
        public int AccessRightID { get; set; }
        public string AccessRightName { get; set; }
    }

    public class AccessRightsNodeMapping
    {
        //public int ID { get; set; }
        public int AccessRightID { get; set; }

        public string AccessRightName { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
    }

    public class UpdateAccessRightsNodeMapping
    {
        public int AccessRightID { get; set; }
        public int NodeID { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
    }

    public class ViewableNodesForEmployee
    {
        public int AccessRightID { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public bool CanView { get; set; }
    }

    public class PageRights
    {
        public int PageId { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}