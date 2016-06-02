using System;

namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for ProblemPriority.
	/// </summary>
	public class ProblemPriority
	{
		#region private variable declaration
			private int _ProblemPriorityID;
			private string _ProblemPriority;
			private bool _isActive;
		#endregion

		public ProblemPriority()
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

		public bool isActive
		{
			get {return _isActive;}
			set {_isActive = value;}
		}

		#endregion
	}
}
