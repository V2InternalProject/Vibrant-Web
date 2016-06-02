using System;
using System.IO;
using System.Text;


namespace V2.CommonServices.Exceptions
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    [Serializable]
    public class V2Exceptions : Exception
    {
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public V2Exceptions()
            : base()
        {
        }
        public V2Exceptions(string Message)
            : base(Message)
        {
            log.Error(Message);
        }

        public V2Exceptions(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
            log.Error(Message, InnerException);
        }


    }
}

