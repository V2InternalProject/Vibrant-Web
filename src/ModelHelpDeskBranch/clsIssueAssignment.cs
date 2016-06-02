using System;

namespace V2.Helpdesk.Model
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
        private int _ProblemSeverity;
		private string _Connectionstring;
		private string _ItDeptEmailID;
		private int _SubCategory;
        private int _SubCategoryID;
		private string _AddComment;
        private int intWorkHours;
        private int intNumberOfResources;
        private DateTime? dtFromDate;
        private DateTime? dtToDate;
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

        public int ProblemSeverity
        {
            get { return _ProblemSeverity; }
            set { _ProblemSeverity = value; }
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
        public int WorkHours
        {
            get { return intWorkHours; }
            set { intWorkHours = value; }
        }

        public int NumberOfResources
        {
            get { return intNumberOfResources; }
            set { intNumberOfResources = value; }
        }
        public DateTime? FromDate
        {
            get { return dtFromDate; }
            set { dtFromDate = value; }
        }

        public DateTime? ToDate
        {
            get { return dtToDate; }
            set { dtToDate = value; }
        }

		#endregion

	}
}
