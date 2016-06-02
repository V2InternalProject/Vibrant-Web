using System;
using System.IO;
using System.Text;


namespace V2.CommonServices.FileLogger
{
	// Logger class using singleton pattern
	[Serializable]
	public enum LogType
	{
		Info = 1,
		Warning = 2,
		Error = 3,
		DEBUG = 4
	}
	[Serializable]
	public delegate void CannotWriteToLogFileExceptionHandler(string ExceptionMessage);

	[Serializable]
	public class FileLog
	{
		public event CannotWriteToLogFileExceptionHandler CannotWriteToLogFile;

		// Static members are lazily initialized.
		// .NET guarantees thread safety for static initialization
		private static readonly FileLog instance = new FileLog();

		// Privates
		private bool isReady = false;
		private StreamWriter swLog;
		private string strLogFile;

		// Constructors
		private FileLog()
		{
			this.strLogFile = GetNewLogFilename();
			openFile();
			_writelog("");
			closeFile();
		}

		private void openFile()
		{
			try
			{
				swLog = File.AppendText(strLogFile);
				isReady = true;
			}
			catch (System.Exception ex)
			{
				isReady = false;
			}
		}

		private void closeFile()
		{
			if (isReady)
			{
				try
				{
					swLog.Close();
				}
				catch
				{
				}
			}
		}

		private static string GetNewLogFilename()
		{
			//string fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			//return fileName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

			string fileName = AppDomain.CurrentDomain.BaseDirectory + "Log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

			if (Directory.Exists(fileName))
			{
				return fileName;
			}
			else
			{
				try
				{
					Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Log");
					return fileName;
				}
				catch (System.Exception ex)
				{

					return AppDomain.CurrentDomain.BaseDirectory + "Log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
				}
			}
		}

		public static FileLog GetLogger()
		{
			return instance;
		}

		public void WriteLine(LogType logtype, string message, string pageorClass, string methodName, string exceptionStack)
		{
			string stub = DateTime.Now.ToString("yyyy-MM-dd @ HH:mm:ss  ");
			switch (logtype)
			{
				case LogType.Info:
					stub += "Informational , ";
					break;
				case LogType.Warning:
					stub += "Warning       , ";
					break;
				case LogType.Error:
					stub += "FATAL ERROR   , ";
					break;
				case LogType.DEBUG:
					stub += "DEBUG         , ";
					break;
			}
			stub += message + " " + pageorClass + " " + methodName + "  " + exceptionStack;

			openFile();
			_writelog(stub);
			closeFile();
			//Console.WriteLine(stub);
		}

		public void WriteStrings(LogType logtype, string message, string pageorClass, string methodName, string exceptionStack)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(message);
			//sb.Append(exception);
			WriteLine(logtype, sb.ToString(), pageorClass, methodName, exceptionStack);
		}

		private void _writelog(string msg)
		{
			if (isReady)
			{
				swLog.WriteLine(msg);
			}
			else
			{
				CannotWriteToLogFile(msg);
				//throw new Exception("Error Cannot write to log file.");                
			}
		}
	}
}









#region old Code 19sept07

//using System;
//using System.IO;
//using System.Text;


//namespace V2.CommonServices.FileLogger
//{
//    // Logger class using singleton pattern
    
//    public enum LogType
//    {
//        Info = 1,
//        Warning = 2,
//        Error = 3,
//        DEBUG = 4
//    }

//    public delegate void CannotWriteToLogFileExceptionHandler(string ExceptionMessage);
    
    
//    public class FileLog 
//    {
//        public event CannotWriteToLogFileExceptionHandler CannotWriteToLogFile;

//        // Static members are lazily initialized.
//        // .NET guarantees thread safety for static initialization
//        private static readonly FileLog instance = new FileLog();

//        // Privates
//        private bool isReady = false;
//        private StreamWriter swLog;
//        private string strLogFile;

//        // Constructors
//        private FileLog()
//        {
//            this.strLogFile = GetNewLogFilename();
//            openFile();
//            _writelog("");
//            closeFile();
//        }

//        private void openFile()
//        {
//            try
//            {
//                swLog = File.AppendText(strLogFile);
//                isReady = true;
//            }
//            catch
//            {
//                isReady = false;
//            }
//        }

//        private void closeFile()
//        {
//            if (isReady)
//            {
//                try
//                {
//                    swLog.Close();
//                }
//                catch
//                {
//                }
//            }
//        }

//        private static string GetNewLogFilename()
//        {
//            //string fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
//            //return fileName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

//            string fileName = AppDomain.CurrentDomain.BaseDirectory  + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

//            if (Directory.Exists(fileName))
//            {
//                return fileName;
//            }
//            else
//            {
//                try
//                {
//                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Log");
//                    return fileName;
//                }
//                catch
//                {
//                    return AppDomain.CurrentDomain.BaseDirectory + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
//                }
//            }	            
//        }

//        public static FileLog GetLogger()
//        {
//            return instance;
//        }

//        public void WriteLine(LogType logtype,string pageorClass,string methodName,string message)
//        {
//            string stub = DateTime.Now.ToString("yyyy-MM-dd @ HH:mm:ss  ");
//            switch (logtype)
//            {
//                case LogType.Info:
//                    stub += "Informational , ";
//                    break;
//                case LogType.Warning:
//                    stub += "Warning       , ";
//                    break;
//                case LogType.Error:
//                    stub += "FATAL ERROR   , ";
//                    break;
//                case LogType.DEBUG:
//                    stub += "DEBUG         , ";
//                    break;
//            }
//            stub += pageorClass + " " + methodName + "  " + message; 
           
//            openFile();
//            _writelog(stub);
//            closeFile();
//            //Console.WriteLine(stub);
//        }

//        public void WriteStrings(LogType logtype,string pageorClass,string method, string message)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append(message);
//            //sb.Append(exception);
//            WriteLine(logtype,pageorClass,method, sb.ToString());
//        }

//        private void _writelog(string msg)
//        {
//            if (isReady)
//            {
//                swLog.WriteLine(msg);
//            }
//            else
//            {
//                CannotWriteToLogFile(msg);
//                //throw new Exception("Error Cannot write to log file.");                
//            }
//        }
//    }
//}

#endregion