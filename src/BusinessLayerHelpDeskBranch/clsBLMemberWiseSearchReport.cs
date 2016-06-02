using System;
using System.Data;
using V2.Helpdesk.Model;
using V2.Helpdesk.DataLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLMemberWiseSearchReport.
	/// </summary>
	public class clsBLMemberWiseSearchReport
	{
		clsDLMemberWiseSearchReport objClsDLMemberWiseSearchReport = new clsDLMemberWiseSearchReport();
		public clsBLMemberWiseSearchReport()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "clsBLMemberWiseSearchReport", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        /// <summary>
        /// Get years to bind dropdown
        /// </summary>
        /// <returns></returns>
        public DataSet getYears()
        {
            try
            {
                //clsDLMemberWiseSearchReport objClsDLMemberWiseSearchReport = new clsDLMemberWiseSearchReport();
                return objClsDLMemberWiseSearchReport.getYears();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "getYears", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

		public DataSet getAllEmployees()
		{
			try
			{
			//clsDLMemberWiseSearchReport objClsDLMemberWiseSearchReport = new clsDLMemberWiseSearchReport();
			return objClsDLMemberWiseSearchReport.getAllEmployees();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "getAllEmployees", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getAllEmployee(clsMemberWiseSearchReport objclsMemberWiseSearchReport)
		{
			try
			{
			//clsDLMemberWiseSearchReport objClsDLMemberWiseSearchReport = new clsDLMemberWiseSearchReport();
			return objClsDLMemberWiseSearchReport.getAllEmployee(objclsMemberWiseSearchReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "getAllEmployee", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet GetStatus()
        {
            try
            {
                return objClsDLMemberWiseSearchReport.GetStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "GetStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

		public DataSet getAllStatus()
		{
			try
			{
			//clsDLMemberWiseSearchReport objClsDLMemberWiseSearchReport = new clsDLMemberWiseSearchReport();
			return objClsDLMemberWiseSearchReport.getAllStatus();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "getAllStatus", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet GetMemberWiseReport(clsMemberWiseSearchReport objClsMemberWiseSearchReport, int superAdmin)
		{
			try
			{
                return objClsDLMemberWiseSearchReport.GetMemberWiseReport(objClsMemberWiseSearchReport, superAdmin);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLMemberWiseSearchReport.cs", "GetMemberWiseReport", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		
	}
}
