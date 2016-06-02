using System;
using System.Net.Http;
using System.Web.Http;

namespace HRMS.Controllers
{
    public class AsyncController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Call(string method, string param)
        {
            string[] tokens = param.Split(new string[] { "?" }, StringSplitOptions.None);
            string[] tok = method.Split(new string[] { "/" }, StringSplitOptions.None);
            string[] paramControl = null;
            string[] reportid = null;
            if (tokens.Length > 1)
                paramControl = tokens[1].Split(new string[] { "=" }, StringSplitOptions.None);
            if (tokens.Length >= 2)
                reportid = tokens[2].Split(new string[] { "=" }, StringSplitOptions.None);
            return new HttpResponseMessage
            {
                Content = new StringContent(ExecuteMethod(tok[0], tokens[0], paramControl[1], Convert.ToInt32(reportid[1])))
            };
        }

        private string ExecuteMethod(string action, string param, string paramControl, int reportid)
        {
            var type = typeof(Executer);
            try
            {
                return type.GetMethod(action).Invoke(null, new object[] { param, paramControl, reportid }).ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}