using System;

namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for clsLogin.
	/// </summary>
	public class clsLogin
	{
		#region private variable declaration
		private int _EmployeeID;
		private string _Password;
		private string _Connectionstring;
		#endregion

		public clsLogin()
		{
		}

		# region public properties

		public int EmployeeID
		{
			get {return _EmployeeID;}
			set {_EmployeeID = value;}
		}

		public string Password
		{
			get {return _Password;}
			set {_Password = value;}
		}

		public string Connectionstring
		{
			get {return _Connectionstring;}
			set {_Connectionstring = value;}
		}

		#endregion

	}
}
