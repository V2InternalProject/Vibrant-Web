using System;
using System.Data;
using System.Data.SqlClient;
using ModelHelpdesk;
using DataLayerHelpDesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLIssueAssignment.
	/// </summary>
	public class clsBLIssueAssignment
	{
		
		clsDLIssueAssignment objDLIssueAssignment = new clsDLIssueAssignment();

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
				throw new V2Exceptions();
			}
		}


		public bool UpdateIssueByLoginUser(clsIssueAssignment objIssueAssignment,string name1)
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
				throw new V2Exceptions();
			}
		}

		public DataSet FileName(clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions();
			}
		}
		public bool IssueUpdateBySuperAdmin(clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions();
			}
		}

		public bool IssueAssignmentBySuperAdmin(clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions();
			}
		}

        public DataSet ChangeCategoryOfIssue(clsIssueAssignment objIssueAssignment)
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
                throw new V2Exceptions();
            }
        }
		public string getFromEmailID(clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions();
			}
		}

		public string getToEmailID(clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions();
			}
		}

		public string getEmployeeEmailID(clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions();
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
                throw new V2Exceptions();
            }
        }


	}
}
