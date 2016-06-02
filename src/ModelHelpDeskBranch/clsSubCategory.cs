using System;

namespace V2.Helpdesk.Model
{
	/// <summary>
	/// Summary description for clsSubCategory.
	/// </summary>
	public class clsSubCategory
	{
		public clsSubCategory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Private Variables

		private int intSubCategoryID;
		private string strSubCategory;
		private int intnewCategory;
		private string strnewSubCategory;
		private int intIsActive;
		private int intAddCategory;
		private string strAddSubCategories;
		private int strAddIsActive;

		#endregion



		#region public properties

		public int SubCategoryID
		{
			get{return intSubCategoryID;}
			set{intSubCategoryID = value;}
		}

		public string SubCategory
		{
			get{return strSubCategory;}
			set{strSubCategory = value;}
		}

		public int NewCategory
		{
			get{return intnewCategory;}
			set{intnewCategory = value;}
		}

		public string NewSubCategory
		{
			get{return strnewSubCategory;}
			set{strnewSubCategory = value;}
		}

		public int IsActive
		{
			get{return intIsActive;}
			set{intIsActive = value;}
		}

		public int AddCategory
		{
			get{return intAddCategory;}
			set{intAddCategory = value;}
		}

		public string AddSubCategories
		{
			get{return strAddSubCategories;}
			set{strAddSubCategories = value;}
		}
		
		public int AddIsActive
		{
			get{return strAddIsActive;}
			set{strAddIsActive = value;}
		}

		#endregion
	}
}
