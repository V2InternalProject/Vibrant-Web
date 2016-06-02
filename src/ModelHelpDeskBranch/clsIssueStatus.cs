using System;


namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for clsIssueStatus.
	/// </summary>
	public class clsIssueStatus
	{
		#region Private Variable Declaration
		private string _connectionString;
		private DateTime _reportIssuedate;
		private int _problemSeverityId;
		private DateTime _startMonth;
		private DateTime _endMonth;
		private int employeeId;
		private int category;
		#endregion


		
		public clsIssueStatus()
		{
		}
		#region Public Variable Declaration
		public DateTime StartMonth
		{
			get {return  _startMonth;}
			set{ _startMonth = value;}
		}
		public DateTime EndMonth
		{
			get {return  _endMonth;}
			set{_endMonth = value;}
		}
		public string connectionString
		{
			get {return  _connectionString;}
			set{ _connectionString = value;}
		}
		public DateTime reportIssueDate
		{
			get {return _reportIssuedate; }
			set {_reportIssuedate = value;}
	
		}
		public int problemSeverityId
		{
			get {return _problemSeverityId;}
			set {_problemSeverityId = value;}
		}
		public int EmployeeId
		{
			get {return employeeId;}
			set {employeeId = value;}
		}
		public int Category
		{
			get {return category;}
			set {category = value;}
		}
		#endregion	
	}
}
