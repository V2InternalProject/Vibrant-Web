using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsCategoryWiseSearchReport.
	/// </summary>
	public class clsCategoryWiseSearchReport
	{
		public clsCategoryWiseSearchReport()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Private variables

		/// <summary>
		/// To store the categoryID
		/// </summary>
		private int intCategoryID;

		/// <summary>
		/// To store the categoryName
		/// </summary>
		private string strCategoryName;

		/// <summary>
		/// To store the employeeName
		/// </summary>
		private string strEmployeeName;
		
		/// <summary>
		/// To store the status ID
		/// </summary>
		private int intStatusID;
		
		/// <summary>
		/// To store the period for which report is requested
		/// </summary>
		private string strPeriod;

		/// <summary>
		/// To store the Date for which report is requested
		/// </summary>
		private string strDate;
		
		/// <summary>
		/// To store the Month for which report is requested
		/// </summary>
		private string strMonth;
		
		/// <summary>
		/// To store the Year for which report is requested
		/// </summary>
		private string strYear;

		/// <summary>
		/// To store the from-date for which report is requested
		/// </summary>
		private string strFromDate;

		/// <summary>
		/// To store the to-date for which report is requested
		/// </summary>
		private string strToDate;

		private int employeeID;
		
		#endregion

		#region Public Properties
		/// <summary>
		/// To GET/SET the Category ID
		/// </summary>
		public int CategoryID
		{
			get{return intCategoryID;}
			set{intCategoryID = value;}
		}

		/// <summary>
		/// To GET/SET the Category Names
		/// </summary>
		public string CategoryName
		{
			get{return strCategoryName;}
			set{strCategoryName = value;}
		}

		/// <summary>
		/// To GET/SET the Employee Names
		/// </summary>
		public string EmployeeName
		{
			get{return strEmployeeName;}
			set{strEmployeeName = value;}
		}

		/// <summary>
		/// To GET/SET the Status Id
		/// </summary>
		public int StatusID
		{
			get{return intStatusID;}
			set{intStatusID = value;}
		}

		/// <summary>
		/// To GET/SET the Period
		/// </summary>
		public string Period
		{
			get{return strPeriod;}
			set{strPeriod = value;}
		}

		/// <summary>
		/// To GET/SET the Date for which the report is requested
		/// </summary>
		public string Date
		{
			get{return strDate;}
			set{strDate = value;}
		}

		/// <summary>
		/// To GET/SET the Month for which the report is requested
		/// </summary>
		public string Month
		{
			get{return strMonth;}
			set{strMonth = value;}
		}

		/// <summary>
		/// To GET/SET the Year for which the report is requested
		/// </summary>
		public string Year
		{
			get{return strYear;}
			set{strYear = value;}
		}

		/// <summary>
		/// To GET/SET the From-date for which the report is requested
		/// </summary>
		public string FromDate
		{
			get{return strFromDate;}
			set{strFromDate = value;}
		}

		/// <summary>
		/// To GET/SET the To-date for which the report is requested
		/// </summary>
		public string ToDate
		{
			get{return strToDate;}
			set{strToDate = value;}
		}
		public int EmployeeID
		{
			get{return employeeID;}
			set{employeeID = value;}
		}
		#endregion
	}
}
