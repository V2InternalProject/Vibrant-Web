using System;

namespace ModelHelpDeskBranch
{
	/// <summary>
	/// Summary description for clsReportIssue.
	/// </summary>
	public class clsReportIssue
	{
		public clsReportIssue()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Private Variables

        private int intReportIssueID;
		private string strName;
		private string strEmailID;
		private string strCCEmailID;
		private string strPhoneExtension;
		private string strSeatingLocation;
		private string strUploadedFileName;
		private string strUploadedFileExtension;
		private string strDescription;		
		private int intSubCategoryID;
        private int intTypeID;
		private int intSeverityID;
		private int intPriorityID;
		private int intStatusID;
		private string strAssignedTo;
        private int intEmployeeID;
        private int intProjectNameId;
        private int intProjectRoleId;
        private int intWorkHours;
        private int intNumberOfResources;
        private int intResourcePoolId;
        private int intReportingToId;
        private DateTime? dtFromDate;
        private DateTime? dtToDate;
			
		#endregion

		#region Public Properties

		public int ReportIssueID
		{
			get{return intReportIssueID;}
			set{intReportIssueID = value;}
		}

        public int EmployeeID
        {
            get { return intEmployeeID; }
            set { intEmployeeID = value; }
        }
		public string Name
		{
			get{return strName;}
			set{strName = value;}
		}

		public string EmailID
		{
			get{return strEmailID;}
			set{strEmailID = value;}
		}

		public string CCEmailID
		{
			get{return strCCEmailID;}
			set{strCCEmailID = value;}
		}
        public int Type
        {
            get { return intTypeID; }
            set { intTypeID = value; }
        }

		public string PhoneExtension
		{
			get{return strPhoneExtension;}
			set{strPhoneExtension = value;}
		}
		
		public string SeatingLocation
		{
			get{return strSeatingLocation;}
			set{strSeatingLocation = value;}
		}

		public int SubCategoryID
		{
			get{return intSubCategoryID;}
			set{intSubCategoryID = value;}
		}

		public int SeverityID
		{
			get{return intSeverityID;}
			set{intSeverityID = value;}
		}

		public int PriorityID
		{
			get{return intPriorityID;}
			set{intPriorityID = value;}
		}

		public string UploadedFileName
		{
			get{return strUploadedFileName;}
			set{strUploadedFileName = value;}
		}

		public string UploadedFileExtension
		{
			get{return strUploadedFileExtension;}
			set{strUploadedFileExtension = value;}
		}
		
		public string Description
		{
			get{return strDescription;}
			set{strDescription = value;}
		}

		public int StatusID
		{
			get{return intStatusID;}
			set{intStatusID = value;}
		}

		public string AssignedTo
		{
			get{return strAssignedTo;}
			set{strAssignedTo = value;}
		}

        public int ProjectNameId
        {
            get { return intProjectNameId; }
            set { intProjectNameId = value; }
        }

        public int ProjectRoleId
        {
            get { return intProjectRoleId; }
            set { intProjectRoleId = value; }
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

        public int ResourcePoolId
        {
            get { return intResourcePoolId; }
            set { intResourcePoolId = value; }
        }

        public int ReportingToId
        {
            get { return intReportingToId; }
            set { intReportingToId = value; }
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
