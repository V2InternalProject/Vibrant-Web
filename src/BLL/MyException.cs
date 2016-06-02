using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class MyException : Exception
    {
        public MyException(string message,  Exception innerException)
            : base(message, innerException)
        {
           
        }

        
    } 

}
