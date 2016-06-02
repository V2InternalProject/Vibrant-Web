using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsSubCategoryAssignment.
	/// </summary>
	public class clsSubCategoryAssignment
	{
		public clsSubCategoryAssignment()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Private Variables
		private int intEmployeeID;
		private int intCategoryID;
		private int intIsSysAdmin;
		private string strEmployeeName;
		private string strEmployeeEmailID;
		private int intIsActive;
		private string strSubCategoryID;
		private int intSubCategoryAssignmentID;
		private int suerAdminEmployeeId;
		#endregion

		#region Properties
		public int EmployeeID
		{
			get{return intEmployeeID;}
			set{intEmployeeID = value;}
		}

		public int CategoryID
		{
			get{return intCategoryID;}
			set{intCategoryID = value;}
		}

		public int IsSysAdmin
		{
			get{return intIsSysAdmin;}
			set{intIsSysAdmin = value;}
		}

		public string EmployeeName
		{
			get{return strEmployeeName;}
			set{strEmployeeName = value;}
		}

		public string EmployeeEmailID
		{
			get{return strEmployeeEmailID;}
			set{strEmployeeEmailID = value;}
		}

		public int IsActive
		{
			get{return intIsActive;}
			set{intIsActive = value;}
		}

		public string SubCategoryID
		{
			get{return strSubCategoryID;}
			set{strSubCategoryID = value;}
		}

		public int SubCategoryAssignmentID
		{
			get{return intSubCategoryAssignmentID;}
			set{intSubCategoryAssignmentID = value;}
		}
		public int SuerAdminEmployeeId
		{
			get{return suerAdminEmployeeId;}
			set{suerAdminEmployeeId = value;}
		}
		#endregion
	}
}
