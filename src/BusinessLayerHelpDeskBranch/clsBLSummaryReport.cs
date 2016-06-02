using System;
using V2.Helpdesk.Model;
using V2.Helpdesk.DataLayer;
using System.Data.SqlClient;
using System.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLSummaryReport.
	/// </summary>
	public class clsBLSummaryReport
	{
        
		DataLayer.clsDLSummaryReport objDLSummaryReport = new clsDLSummaryReport();
		public clsBLSummaryReport()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSummaryReport.cs", "clsBLSummaryReport", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet DepartmentDeatils(clsSummaryReport objSumaryReport)
		{
			try
			{
                return objDLSummaryReport.DepartmentDetails(objSumaryReport);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSummaryReport.cs", "DepartmentDeatils", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet EmployeeDetails(clsSummaryReport objSumaryReport)
		{
			try
			{
			
			return objDLSummaryReport.EmployeeDetails(objSumaryReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSummaryReport.cs", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet EmployeeDetails()
		{
			try
			{
			
			return objDLSummaryReport.EmployeeDetails();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSummaryReport.cs", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet DeptSummaryDetails(clsSummaryReport objSumaryReport)
		{
			try
			{
			
			return objDLSummaryReport.DeptSummaryDetails(objSumaryReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSummaryReport.cs", "DeptSummaryDetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet EmpSummaryDetails(clsSummaryReport objSumaryReport)
		{
			try
			{
			
			return objDLSummaryReport.EmpSummaryDetails(objSumaryReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSummaryReport.cs", "EmpSummaryDetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		
	}
}
