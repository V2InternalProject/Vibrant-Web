using System;
using System.Data;
using System.Data.SqlClient;
//using DataLayerHelpDesk;
using ModelHelpDeskBranch;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using DataLayerHelpDeskBranch;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLReportIssue.
	/// </summary>
	public class clsBLReportIssue
	{
		public clsBLReportIssue()
		{
			try
			{
			//
			// TODO: Add constructor logic here
			//
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "clsBLReportIssue", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet GetEmailID(int EmployeeID)
        {
            try
			{
                DataLayerHelpDeskBranch.clsDLReportIssue objClsDLReportIssue = new DataLayerHelpDeskBranch.clsDLReportIssue();
			return objClsDLReportIssue.GetEmailID(EmployeeID);
		    }
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
        }

		public DataSet GetSeverity()
		{
			try
			{
			clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
			return objClsDLReportIssue.GetSeverity();
		    }
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet GetSubCategory()
		{
			try
			{
			clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
            return objClsDLReportIssue.GetSubCategory();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
        
		public string GetCategoryName(clsReportIssue objclsReportIssue)
		{
			try
			{
			clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
			return objClsDLReportIssue.GetCategoryName(objclsReportIssue);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetCategoryName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

      

        public string GetAssignedToEmailID(clsReportIssue objclsReportIssue)
		{
			try
			{
			clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
            return objClsDLReportIssue.GetAssignedToEmailID(objclsReportIssue);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetAssignedToEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetPriority()
		{
			try
			{
			clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
			return objClsDLReportIssue.GetPriority();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
        public DataSet GetType()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetType();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetType", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public DataSet GetProblemSeverity()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetProblemSeverity();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetProblemSeverity", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
        public DataSet GetProjectName(string EmployeeCode)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetProjectName(EmployeeCode);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetProjectName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetProjectRole()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetProjectRole();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetProjectRole", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetResourcePool()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetResourcePool();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetResourcePool", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetReportingTo(int ProjectID)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetReportingTo(ProjectID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetReportingTo", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


		public DataSet InsertIssueDetails(clsReportIssue objClsReportIssue)
		{
			try
			{
			clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
			return objClsDLReportIssue.InsertIssueDetails(objClsReportIssue);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "InsertIssueDetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet GetSelectedProjectDetails(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetSelectedProjectDetails(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetSelectedProjectDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet InsertFile(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.InsertFile(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        public bool IsCCEmailExists(string CCEmailId)
        {
            try
            { 
              clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
              return objClsDLReportIssue.IsCCEmailExists(CCEmailId);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataSet getCategorySummary(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.getCategorySummary(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "getCategorySummary", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetProjectDates(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetSelectedProjectDetailsFromProjectID(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "getCategorySummary", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetSubCategoryByCategoryID(int categoryID)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetSubCategoryByCategoryID(categoryID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetSubCategoryByCategoryID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

	}
}
