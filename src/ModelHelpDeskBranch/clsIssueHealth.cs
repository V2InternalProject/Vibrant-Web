using System;

namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for clsIssueHealth.
	/// </summary>
	public class clsIssueHealth
	{
		public clsIssueHealth()
		{
			//
			// TODO: Add constructor logic here
			//
			
		}
		# region Variable Declaretion 
		private int emplyeeId;
		#endregion 

		# region Public Property  
		public int EmployeeID
		{
			get {return emplyeeId;}
			set {emplyeeId = value;}
		}

		#endregion 
	}
}
