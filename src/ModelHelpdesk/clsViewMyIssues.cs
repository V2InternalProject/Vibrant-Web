using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsViewMyIssues.
	/// </summary>
	public class clsViewMyIssues
	{
		#region private variable declaration
		private int _IssueAssignmentID;
		private int _EmployeeID;
		private int _ReportIssueID;
		private int _StatusID;
		private string _Cause;
		private string _Fix;
		private string _ReportedOn;
		private string _ReportedBy;
		private int _ProblemSeverityID;
		private int _ProblemPriorityID;
		private string _Description;
		private string _Connectionstring;
		private string _SelectedStatus;
		private string _SelectedEmployee;
		private int loginId;
		private int category; 
		private int issueMoveTo;
		private int issueMoveForm;
		private string reportIssueIDStr;
		private int subCategoryId;
		#endregion

		public clsViewMyIssues()
		{
		}

		# region public properties

		public int IssueAssignmentID
		{
			get {return _IssueAssignmentID;}
			set {_IssueAssignmentID = value;}
		}

		public int EmployeeID
		{
			get {return _EmployeeID;}
			set {_EmployeeID = value;}
		}

		public int ReportIssueID
		{
			get {return _ReportIssueID;}
			set {_ReportIssueID = value;}
		}

		public int StatusID
		{
			get {return _StatusID;}
			set {_StatusID = value;}
		}
		

		public int ProblemSeverityID
		{
			get {return _ProblemSeverityID;}
			set {_ProblemSeverityID = value;}
		}

		public int ProblemPriorityID
		{
			get {return _ProblemPriorityID;}
			set {_ProblemPriorityID = value;}
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

		public string ReportedOn
		{
			get {return _ReportedOn;}
			set {_ReportedOn = value;}
		}

		public string ReportedBy
		{
			get {return _ReportedBy;}
			set {_ReportedBy = value;}
		}

		public string Description
		{
			get {return _Description;}
			set {_Description = value;}
		}

		public string Connectionstring
		{
			get {return _Connectionstring;}
			set {_Connectionstring = value;}
		}
		public string SelectedStatus
		{
			get {return _SelectedStatus;}
			set {_SelectedStatus = value;}
		}
		public string SelectedEmployee
		{
			get {return _SelectedEmployee;}
			set {_SelectedEmployee = value;}
		}
		public int LoginId	
		{
			get {return loginId;}
			set {loginId = value;}
		}
		public int Category	
		{
			get {return category;}
			set {category = value;}
		}
		public int	IssueMoveFrom	
		{
			get {return issueMoveForm;}
			set {issueMoveForm = value;}
		}
		public int IssueMoveTo		
		{
			get {return issueMoveTo;}
			set {issueMoveTo = value;}
		}

		public string ReportIssueIDStr
		{
			get {return reportIssueIDStr;}
			set {reportIssueIDStr = value;}
		}
		public int SubCategoryId
		{
			get {return subCategoryId;}
			set {subCategoryId = value;}
		}
		
		#endregion
	}
}
