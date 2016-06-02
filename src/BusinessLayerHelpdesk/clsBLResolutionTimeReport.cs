using System;
using System.Data;
using DataLayerHelpDesk;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLResolutionTimeReport.
	/// </summary>
	public class clsBLResolutionTimeReport
	{
		clsDLResolutionTimeReport objClsDLResolutionTimeReport = new clsDLResolutionTimeReport();
		public clsBLResolutionTimeReport()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTimeReport.cs", "clsBLResolutionTimeReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getProblemPriority()
		{
			try
			{
			return objClsDLResolutionTimeReport.getProblemPriority();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTimeReport.cs", "getProblemPriority", ex.StackTrace);
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
                return objClsDLResolutionTimeReport.getYears();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTimeReport.cs", "getYears", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

		public DataSet getProblemSeverity()
		{
			try
			{
			return objClsDLResolutionTimeReport.getProblemSeverity();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTimeReport.cs", "getProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        public DataSet getResolutionTimeDetails(clsResolutionTimeReport objClsResolutionTimeReport, int superAdmin)
		{
			try
			{
                return objClsDLResolutionTimeReport.getResolutionTimeDetails(objClsResolutionTimeReport, superAdmin);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTimeReport.cs", "getResolutionTimeDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getGraphDetails(clsResolutionTimeReport objClsResolutionTimeReport)
		{
			try
			{
			return objClsDLResolutionTimeReport.getGraphDetails(objClsResolutionTimeReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTimeReport.cs", "getGraphDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
