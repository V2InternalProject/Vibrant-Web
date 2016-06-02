using System;

namespace HRMS.ModelHelpdesk
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
	#endregion
	}
}
