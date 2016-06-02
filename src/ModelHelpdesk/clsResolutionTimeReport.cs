using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsResolutionTimeReport.
	/// </summary>
	public class clsResolutionTimeReport
	{
		public clsResolutionTimeReport()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region Private variables

		//private string strFromDate;
		//private string strToDate;
		private DateTime dtFromMonth;
		//private int intFromYear;
		private DateTime dtToMonth;
		//private int intToYear;
		private int intEmployeeID;
		private int intPriorityID;
		private int intSeverityID;
		private int intIssueHealth;

		#endregion

		#region Public Properties
		
		/// <summary>
		/// To get set the value for From Month
		/// </summary>
		public DateTime FromMonth
		{
			get{return dtFromMonth;}
			set{dtFromMonth = value;}
		}

		/*/// <summary>
		/// To get set the value for From Year
		/// </summary>
		public int FromYear
		{
			get{return intFromYear;}
			set{intFromYear = value;}
		}*/

		/// <summary>
		/// To get set the value for To Month
		/// </summary>
		public DateTime ToMonth
		{
			get{return dtToMonth;}
			set{dtToMonth = value;}
		}

		/*/// <summary>
		/// To get set the value for To Year
		/// </summary>
		public int ToYear
		{
			get{return intToYear;}
			set{intToYear = value;}
		}*/

		/// <summary>
		/// To get set the value for Employee ID
		/// </summary>
		public int EmployeeID
		{
			get{return intEmployeeID;}
			set{intEmployeeID = value;}
		}

		/// <summary>
		/// To get set the value for Priority ID
		/// </summary>
		public int PriorityID
		{
			get{return intPriorityID;}
			set{intPriorityID = value;}
		}

		/// <summary>
		/// To get set the value for Severity ID
		/// </summary>
		public int SeverityID
		{
			get{return intSeverityID;}
			set{intSeverityID = value;}
		}
			
		/// <summary>
		/// To get set the value for Issue Health ID
		/// </summary>
		public int IssueHealth
		{
			get{return intIssueHealth;}
			set{intIssueHealth = value;}
		}
		#endregion
		
	}
}