using System;
using System.Data;
using System.Data.SqlClient;
using V2.Helpdesk.Model;
using V2.Helpdesk.DataLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLIssueAssignment.
	/// </summary>
	public class clsBLIssueAssignment
	{
		
		DataLayer.clsDLIssueAssignment objDLIssueAssignment = new clsDLIssueAssignment();

		public clsBLIssueAssignment()
		{
			try
			{
			objDLIssueAssignment = new clsDLIssueAssignment();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "clsBLIssueAssignment", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public bool UpdateIssueByLoginUser(Model.clsIssueAssignment objIssueAssignment,string name1)
		{
			try
			{
				if (objDLIssueAssignment.UpdateIssueByLoginUser(objIssueAssignment,name1))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "UpdateIssueByLoginUser", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet FileName(Model.clsIssueAssignment objIssueAssignment)
		{
			try
			{
			return objDLIssueAssignment.FileName(objIssueAssignment);
				
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "FileName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public bool IssueUpdateBySuperAdmin(Model.clsIssueAssignment objIssueAssignment)
		{
			try
			{
				if(objDLIssueAssignment.IssueUpdateBySuperAdmin(objIssueAssignment))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "IssueUpdateBySuperAdmin", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public bool IssueAssignmentBySuperAdmin(Model.clsIssueAssignment objIssueAssignment)
		{
			try
			{
				if(objDLIssueAssignment.IssueAssignmentBySuperAdmin(objIssueAssignment))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "IssueAssignmentBySuperAdmin", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet ChangeCategoryOfIssue(Model.clsIssueAssignment objIssueAssignment)
        {
            try
            {
                return objDLIssueAssignment.ChangeCategoryOfIssue(objIssueAssignment);
               
               
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "ChangeCategoryOfIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
		public string getFromEmailID(Model.clsIssueAssignment objIssueAssignment)
		{
			try
			{
			return objDLIssueAssignment.getFromEmailID(objIssueAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "getFromEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public string getToEmailID(Model.clsIssueAssignment objIssueAssignment)
		{
			try
			{
			return objDLIssueAssignment.getToEmailID(objIssueAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "getToEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public string getEmployeeEmailID(Model.clsIssueAssignment objIssueAssignment)
		{
			try
			{
			return objDLIssueAssignment.getEmployeeEmailID(objIssueAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "getEmployeeEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


        public string GetIssueRaiserEmailID(int intReportIssueID)
        {
            try
            {
                return objDLIssueAssignment.GetIssueRaiserEmailID(intReportIssueID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "getEmployeeEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public string getUserName(Model.clsIssueAssignment objIssueAssignment)
        {
            try
            {
                return objDLIssueAssignment.getUserName(objIssueAssignment);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueAssignment.cs", "getUserName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

	}
}
