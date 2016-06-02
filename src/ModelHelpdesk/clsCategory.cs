using System;

namespace ModelHelpdesk
{
	/// <summary>
	/// Summary description for clsCategory.
	/// </summary>
	public class clsCategory
	{
		public clsCategory()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region private variables
		private int intCategoryID;
		private string strCategory;
		private int intIsActive;
		private string strNewCategory;
		private string strAddCategories;
		private int intaddIdActive;
		private string employeeId;
		#endregion

		#region public properties
		public int CategoryID
		{
			get{return intCategoryID;}
			set{intCategoryID = value;}
		}

		public string Category
		{
			get{return strCategory;}
			set{strCategory = value;}
		}

		public int isActive
		{
			get{return intIsActive;}
			set{intIsActive = value;}
		}
		
		public string NewCategory
		{
			get{return strNewCategory;}
			set{strNewCategory = value;}
		}

		public string AddCategories
		{
			get{return strAddCategories;}
			set{strAddCategories = value;}
		}

		public int AddIdActive
		{
			get{return intaddIdActive;}
			set{intaddIdActive = value;}
		}	 
		public string EmployeeId
		{
			get{return employeeId;}
			set{employeeId = value;}
		}
		#endregion
		
	}
}
