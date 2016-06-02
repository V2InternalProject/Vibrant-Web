
using System.Text;
using System.Collections.Generic;
using System;
using ModelHelpdesk;
using DataLayerHelpDesk;
using System.Data.SqlClient;
using System.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	
	public class clsBLSLAReport
	{
		clsDLSLAReport objDLSLAReport = new clsDLSLAReport();
		public clsBLSLAReport()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSLAReport.cs", "clsBLSLAReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        public DataSet DepartmentDeatils(clsSLAReport objSLAReport)
		{
			try
			{
                return objDLSLAReport.DepartmentDetails(objSLAReport);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSLAReport.cs", "DepartmentDeatils", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet EmployeeDetails(clsSLAReport objSLAReport)
		{
			try
			{
			
			return objDLSLAReport.EmployeeDetails(objSLAReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSLAReport.cs", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet EmployeeDetails()
		{
			try
			{
			
			return objDLSLAReport.EmployeeDetails();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSLAReport.cs", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet DeptSLADetails(clsSLAReport objSLAReport)
		{
			try
			{
			
			return objDLSLAReport.DeptSLADetails(objSLAReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSLAReport.cs", "DeptSummaryDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet EmpSummaryDetails(clsSLAReport objSLAReport)
		{
			try
			{
			
			return objDLSLAReport.EmpSummaryDetails(objSLAReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSLAReport.cs", "EmpSummaryDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		
	}
}
