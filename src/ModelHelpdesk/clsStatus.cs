using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsStatus.
	/// </summary>
	public class clsStatus
	{
		#region private variable declaration
		private int _StatusID;
		private string _Status;
		private string _Connectionstring;
		#endregion

		public clsStatus()
		{
		}

		# region public properties

		public int StatusID
		{
			get {return _StatusID;}
			set {_StatusID = value;}
		}

		public string StatusName
		{
			get {return _Status;}
			set {_Status = value;}
		}

		public string Connectionstring
		{
			get {return _Connectionstring;}
			set {_Connectionstring = value;}
		}

		#endregion

	}
}
