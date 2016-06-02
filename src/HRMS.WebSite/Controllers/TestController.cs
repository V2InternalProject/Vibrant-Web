using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace HRMS.Controllers
{
    public class TestController : ApiController
    {
        [System.Web.Mvc.HttpGet]
        public HttpResponseMessage Show(int id)
        {
            DAL.EmployeeDAL empClient = new DAL.EmployeeDAL();

            List<HRMS.Models.EmployeeListDetails> empList = new List<Models.EmployeeListDetails>();
            empList = empClient.GetEmployeeList();

            var response = Request.CreateResponse();
            response.Content = new ObjectContent(typeof(List<HRMS.Models.EmployeeListDetails>), empList, new JsonMediaTypeFormatter());
            //set headers on the "response"
            return response;
        }

        //[HttpPost]
        //public HttpResponseMessage Save(Employee employee,string from)
        //{
        //    var response = Request.CreateResponse();
        //    response.Content = new ObjectContent(typeof(Employee), new Employee(), new JsonMediaTypeFormatter());
        //    //set headers on the "response"
        //    return response;
        //}
    }
}