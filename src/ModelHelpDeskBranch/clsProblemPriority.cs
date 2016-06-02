using System;

namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for ProblemPriority.
	/// </summary>
	public class clsProblemPriority
	{
		#region private variable declaration
			private int _ProblemPriorityID;
			private string _ProblemPriority;
			private int _GreenResolutionHours;
			private int _AmberResolutionHours;
			private int _isActive;
			private string _Connectionstring;
		#endregion

		public clsProblemPriority()
		{
		}

		# region public properties

		public int ProblemPriorityID
		{
			get {return _ProblemPriorityID;}
			set {_ProblemPriorityID = value;}
		}

		public string ProblemPriorityName
		{
			get {return _ProblemPriority;}
			set {_ProblemPriority = value;}
		}

		public int GreenResolutionHours
		{
			get {return _GreenResolutionHours;}
			set {_GreenResolutionHours = value;}
		}

		public int AmberResolutionHours
		{
			get {return _AmberResolutionHours;}
			set {_AmberResolutionHours = value;}
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
