using System;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace DataLayer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Class1
	{
		public Class1()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "Class1.cs", "Class1", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
	}
}
