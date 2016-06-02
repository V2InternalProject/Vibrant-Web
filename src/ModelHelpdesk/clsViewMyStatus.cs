using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsViewMyStatus.
	/// </summary>
	public class clsViewMyStatus
	{
		public clsViewMyStatus()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Private variables

		private int intIssueID;
		private int intLoginId;
		private string strEmailID;
		private string strPassword;
		private string strComments;
		private int intStatusID;
		private int intSubCategoryID;
		
		#endregion

		#region
		public int IssueID
		{
			get{return intIssueID;}
			set{intIssueID = value;}
		}

		public int LoginID
		{
			get{return intLoginId;}
			set{intLoginId = value;}
		}

		public string EmailID
		{
			get{return strEmailID;}
			set{strEmailID = value;}
		}

		public string Password
		{
			get{return strPassword;}
			set{strPassword = value;}
		}

		public string Comments
		{
			get{return strComments;}
			set{strComments = value;}
		}

		public int StatusID
		{
			get{return intStatusID;}
			set{intStatusID = value;}
		}

		public int SubCategoryID
		{
			get{return intSubCategoryID;}
			set{intSubCategoryID = value;}
		}
		#endregion
	}
}
