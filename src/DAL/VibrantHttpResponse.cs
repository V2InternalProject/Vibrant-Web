using System;

namespace HRMS.DAL
{
    public class VibrantHttpResponse
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}