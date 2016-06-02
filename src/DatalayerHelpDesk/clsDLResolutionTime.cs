using System;
using System.Data;
using System.Data.SqlClient;
using ModelHelpdesk;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLResolutionTime.
	/// </summary>
	public class clsDLResolutionTime
	{
		string strConnectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		clsResolutionTime objclsResolutionTime =new clsResolutionTime ();
		public clsDLResolutionTime()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "clsDLResolutionTime", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getCategory()
		{
			try
			{
			DataSet dsCategory = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure,"sp_getCategoryMaster");
			return dsCategory;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "getCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getSubCategory(clsResolutionTime objclsResolutionTime )
		{
			try
			{
			SqlParameter[] sqlparam= new SqlParameter [1];

			sqlparam [0]=new SqlParameter ("@Categoryid",SqlDbType.Int);
			sqlparam [0].Value = objclsResolutionTime.CategoryID ;

			DataSet dsSubCategory = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure,"sp_getSubCategoryMaster",sqlparam);
			return dsSubCategory;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "getSubCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getProblemSeverity()
		{
			try
			{
			DataSet dsProblemSeverity = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure,"sp_GetProblemSeverityMaster");
			return dsProblemSeverity;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "getProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int addResolutionTime(clsResolutionTime objclsResolutionTime)
		{
			try
			{
			int rowsReturned = 0;
			SqlParameter [] sqlparam = new SqlParameter [5];
			
			sqlparam[0]=new SqlParameter ("@categoryid",SqlDbType.Int);
			sqlparam[0].Value = objclsResolutionTime.CategoryID;

			sqlparam[1]=new SqlParameter ("@subCategoryid",SqlDbType.Int);
			sqlparam[1].Value = objclsResolutionTime.SubCategoryID ;

			sqlparam[2]= new SqlParameter ("@problemSeverityid",SqlDbType.Int );
			sqlparam [2].Value = objclsResolutionTime.ProblemSeverityId ;

			sqlparam [3]=new SqlParameter ("@resolutionForGreen",SqlDbType.Int);
			sqlparam[3].Value=objclsResolutionTime.ResolutionForGreen;

			sqlparam[4]=new SqlParameter ("@resolutionForAmber",SqlDbType.Int);
			sqlparam[4].Value = objclsResolutionTime.ResolutionForAmber ;

			rowsReturned = SqlHelper.ExecuteNonQuery (strConnectionString,CommandType.StoredProcedure,"sp_AddResolutionTime",sqlparam);
			return rowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "addResolutionTime", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public int updateResolutionTime (clsResolutionTime objclsResolutionTime)
		{
			try
			{
			int rowsReturned = 0;
			SqlParameter [] sqlparam = new SqlParameter[6];
			
			sqlparam[0]=new SqlParameter ("@categoryid",SqlDbType.Int);
			sqlparam[0].Value = objclsResolutionTime.CategoryID;

			sqlparam[1]=new SqlParameter ("@subCategoryid",SqlDbType.Int);
			sqlparam[1].Value = objclsResolutionTime.SubCategoryID ;

			sqlparam[2]= new SqlParameter ("@problemSeverityid",SqlDbType.Int );
			sqlparam [2].Value = objclsResolutionTime.ProblemSeverityId ;

			sqlparam [3]=new SqlParameter ("@resolutionForGreen",SqlDbType.Int);
			sqlparam[3].Value=objclsResolutionTime.ResolutionForGreen;

			sqlparam[4]=new SqlParameter ("@resolutionForAmber",SqlDbType.Int);
			sqlparam[4].Value = objclsResolutionTime.ResolutionForAmber ;
			
			sqlparam[5] = new SqlParameter ("@resolutionid",SqlDbType.Int);
			sqlparam[5].Value = objclsResolutionTime.ResolutionID;

			rowsReturned = SqlHelper.ExecuteNonQuery (strConnectionString,CommandType.StoredProcedure,"sp_UpdateResolutionTime",sqlparam);
			return rowsReturned;

		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "updateResolutionTime", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public int deleteResolutionTime(clsResolutionTime objclsResolutionTime)
		{
			try
			{
			int rowsReturned = 0;
			SqlParameter [] sqlparam = new SqlParameter[1];

			sqlparam[0]= new SqlParameter("@resolutionID",SqlDbType.Int);
			sqlparam[0].Value = objclsResolutionTime.ResolutionID;
			rowsReturned=SqlHelper.ExecuteNonQuery(strConnectionString,CommandType.StoredProcedure,"sp_DeleteResoltuionTime",sqlparam);
			return rowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "deleteResolutionTime", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet IsDuplicateResolution (clsResolutionTime  objclsResolutionTime )
		{
			try
			{
			DataSet dsIsDuplicateResolution ;
			SqlParameter [] sqlparam = new SqlParameter[4];
		
			
			sqlparam[0]= new SqlParameter("@subCategoryID",SqlDbType.Int);
			sqlparam[0].Value= objclsResolutionTime.SubCategoryID;
			sqlparam[1]= new SqlParameter ("@problemSeverityid",SqlDbType.Int );
			sqlparam[1].Value = objclsResolutionTime.ProblemSeverityId ;
			sqlparam[2] = new SqlParameter("@categoryid",SqlDbType.Int);
			sqlparam[2].Value =objclsResolutionTime.CategoryID;
			sqlparam[3]=new SqlParameter ("@resolutionid",SqlDbType.Int);
			sqlparam[3].Value=objclsResolutionTime.ResolutionID;


			dsIsDuplicateResolution = SqlHelper.ExecuteDataset(strConnectionString,CommandType.StoredProcedure,"get_IsDuplicateResolution",sqlparam);
			return dsIsDuplicateResolution;

		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "IsDuplicateResolution", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getResolutionTime()
		{
			try
			{
			DataSet dsGetResolutionTime = SqlHelper.ExecuteDataset (strConnectionString,CommandType.StoredProcedure, "sp_GetResolutionTime");
			return dsGetResolutionTime;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTime.cs", "getResolutionTime", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		

	}
}
