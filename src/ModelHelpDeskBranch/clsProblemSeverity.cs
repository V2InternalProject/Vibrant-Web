using System;


namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for clsProblemSeverity.
	/// </summary>
	public class clsProblemSeverity
	{
		#region private variable declaration
		private int _ProblemSeverityID;
		private string _ProblemSeverity;
		private int _isActive;
		private string _Connectionstring;
		#endregion

		public clsProblemSeverity()
		{
		}

		# region public properties

		public int ProblemSeverityID
		{
			get {return _ProblemSeverityID;}
			set {_ProblemSeverityID = value;}
		}

		public string ProblemSeverityName
		{
			get {return _ProblemSeverity;}
			set {_ProblemSeverity = value;}
		}

		public int isActive
		{
			get {return _isActive;}
			set {_isActive = value;}
		}

		public string Connectionstring
		{
			get {return _Connectionstring;}
			set {_Connectionstring = value;}
		}

		#endregion

	}
}
