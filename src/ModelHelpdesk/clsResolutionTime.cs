using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsResolutionTime.
	/// </summary>
	public class clsResolutionTime
	{
		public clsResolutionTime()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		private int resolutionid;
		private int categoryid;
		private int subCategoryid;
		private int problemSeverityid;
		private string resolutionForGreen;
		private string resolutionForAmber;

		public int ResolutionID
		{
			get {return resolutionid;}
			set {resolutionid=value;}
		}
		public int CategoryID
		{
			get{return categoryid; }
			set{categoryid = value;}
		}
		public int SubCategoryID
		{
			get{return subCategoryid; }
			set{subCategoryid =value;}
		}
		public int ProblemSeverityId
		{
			get {return problemSeverityid;}
			set {problemSeverityid=value;}
		}
		public string ResolutionForGreen
		{
			get{return resolutionForGreen ;}
			set{resolutionForGreen =value;}
		}
		public string ResolutionForAmber
		{
			get{return resolutionForAmber;}
			set{resolutionForAmber =value;}
		}

	}
}
