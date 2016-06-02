using System;

namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for clsSummaryReport.
	/// </summary>
	public class clsSummaryReport
	{
		#region private variable declaration
		private int issueAssignmentID;
		private int employeeID;
		private int categoryID;
		private int reportIssueID;
		private int statusID;
		private string cause;
		private string fix;
		private string reportedOn;
		private string reportedBy;
		private int problemSeverityID;
		private int problemPriorityID;
		private string description;
		private string connectionstring;
		private int selectedStatus;
		private string selectedEmployee;
		private int loginId;
		private int category; 
		private int issueMoveTo;
		private int issueMoveForm;
		private string reportIssueIDStr;
		private int subCategoryId;
		private DateTime reportIssueDate;
		private DateTime reportCloseDate;
		#endregion
		
		
		public clsSummaryReport()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		# region public properties

		public int IssueAssignmentID
		{
			get {return issueAssignmentID;}
			set {issueAssignmentID = value;}
		}

		public int EmployeeID
		{
			get {return employeeID;}
			set {employeeID = value;}
		}

		public int ReportIssueID
		{
			get {return reportIssueID;}
			set {reportIssueID = value;}
		}

		public int StatusID
		{
			get {return statusID;}
			set {statusID = value;}
		}
		

		public int ProblemSeverityID
		{
			get {return problemSeverityID;}
			set {problemSeverityID = value;}
		}

		public int ProblemPriorityID
		{
			get {return problemPriorityID;}
			set {problemPriorityID = value;}
		}

		public string Cause
		{
			get {return cause;}
			set {cause = value;}
		}
		public string Fix
		{
			get {return fix;}
			set {fix = value;}
		}

		public string ReportedOn
		{
			get {return reportedOn;}
			set {reportedOn = value;}
		}

		public string ReportedBy
		{
			get {return reportedBy;}
			set {reportedBy = value;}
		}

		public string Description
		{
			get {return description;}
			set {description = value;}
		}

		public string Connectionstring
		{
			get {return connectionstring;}
			set {connectionstring = value;}
		}
		public int SelectedStatus
		{
			get {return selectedStatus;}
			set {selectedStatus = value;}
		}
		public string SelectedEmployee
		{
			get {return selectedEmployee;}
			set {selectedEmployee = value;}
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
		public int CategoryID	
		{
			get {return categoryID;}
			set {categoryID = value;}
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
		public DateTime ReportIssueDate
		{
			get {return reportIssueDate;}
			set {reportIssueDate = value;}
		}
		public DateTime ReportCloseDate
		{
			get{return reportCloseDate;}
			set{reportCloseDate = value;}
		}
		
		#endregion
	}
}
