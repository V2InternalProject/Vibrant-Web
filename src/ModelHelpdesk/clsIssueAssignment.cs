using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsIssueAssignment.
	/// </summary>
	public class clsIssueAssignment
	{
		#region private variable declaration
		private int _IssueAssignmentID;
		private int _StatusID;
		private int _ReportIssueID;
		private string _Cause;
		private string _Fix;
		private int _EmployeeID;
        private int _TypeID;
		private string _Connectionstring;
		private string _ItDeptEmailID;
		private int _SubCategory;
        private int _SubCategoryID;
		private string _AddComment;
		#endregion

		public clsIssueAssignment()
		{
		}

		# region public properties

		public int IssueAssignmentID
		{
			get {return _IssueAssignmentID;}
			set {_IssueAssignmentID = value;}
		}

		public int StatusID
		{
			get {return _StatusID;}
			set {_StatusID = value;}
		}

        public int TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

		public int ReportIssueID
		{
			get {return _ReportIssueID;}
			set {_ReportIssueID = value;}
		}

		public string Cause
		{
			get {return _Cause;}
			set {_Cause = value;}
		}

		public string Fix
		{
			get {return _Fix;}
			set {_Fix = value;}
		}

		public int EmployeeID
		{
			get {return _EmployeeID;}
			set {_EmployeeID = value;}
		}

		public string Connectionstring
		{
			get {return _Connectionstring;}
			set {_Connectionstring = value;}
		}

		public string ItDeptEmailID
		{
			get {return _ItDeptEmailID;}
			set {_ItDeptEmailID = value;}
		}
		public int SubCategory
		{
			get {return _SubCategory;}
			set {_SubCategory = value;}
		}
        public int SubCategoryID
        {
            get { return _SubCategoryID; }
            set { _SubCategoryID = value; }
        }
		public string AddComment 
		{
			get{return _AddComment;}
			set {_AddComment=value;}
		}

		#endregion

	}
}
