using System;
using System.Data;
using DataLayerHelpDesk;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLCategoryWiseSearchReport.
	/// </summary>
	public class clsBLCategoryWiseSearchReport
	{
		clsDLCategoryWiseSearchReport objDLCategoryWiseSearchReport = new clsDLCategoryWiseSearchReport();

		public clsBLCategoryWiseSearchReport()
		{
			try
			{
			objDLCategoryWiseSearchReport = new clsDLCategoryWiseSearchReport();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategoryWiseSearchReport.cs", "clsBLCategoryWiseSearchReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getAllStatus()
		{
			try
			{
			return objDLCategoryWiseSearchReport.getAllStatus();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategoryWiseSearchReport.cs", "getAllStatus", ex.StackTrace);
				throw new V2Exceptions();
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
                return objDLCategoryWiseSearchReport.getYears();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategoryWiseSearchReport.cs", "getYears", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
		public DataSet GetCategoryWiseReport(clsCategoryWiseSearchReport objCategoryWiseSearchReport)
		{
			try
			{
			return objDLCategoryWiseSearchReport.GetCategoryWiseReport(objCategoryWiseSearchReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategoryWiseSearchReport.cs", "GetCategoryWiseReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
