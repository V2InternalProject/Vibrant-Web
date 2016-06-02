using HRMS.common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using V2.Orbit.BusinessLayer;

namespace HRMS.Controllers
{
    public class AppraisalAdminController : ApiController
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AppraisalReview objApr = new AppraisalReview();
        private readonly HRMSResponse objResponse = new HRMSResponse();
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        [HttpGet]
        public HttpResponseMessage GetAllSectionList()
        {
            var response = Request.CreateResponse();
            try
            {
                var SectionsList = objApr.GetAllSectionList();

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<SectionList>)SectionsList;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get sectionlist.";
                log.Error("Error during get appraisal list : GetAllSectionList() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllQuestionList()
        {
            var response = Request.CreateResponse();
            try
            {
                var QuestionsList = objApr.GetAllQuestionList();

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<QuestionList>)QuestionsList;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get questionlist.";
                log.Error("Error during get appraisal list : GetAllQuestionList() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllYear()
        {
            var response = Request.CreateResponse();
            try
            {
                var YearList = objApr.GetAllYear();

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<yearList>)YearList;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get yearlist.";
                log.Error("Error during get appraisal list : GetAllYear() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetYearSectionList(int? ID)
        {
            var response = Request.CreateResponse();
            try
            {
                var YearSectionList = objApr.GetYearSectionList(ID);

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<yearSectionMapping>)YearSectionList;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get yearsectionlist.";
                log.Error("Error during get appraisal list : GetYearSectionList() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetMappingId(int? yearId, int sectionId)
        {
            var response = Request.CreateResponse();
            try
            {
                var MappingId = objApr.GetMappingId(yearId, sectionId);

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<Mepping>)MappingId;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get MappingId.";
                log.Error("Error during get appraisal list : GetMappingId() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllSectionOfYear(int? Id)
        {
            var response = Request.CreateResponse();
            try
            {
                var SectionsList = objApr.GetAllSectionOfYear(Id);

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<SectionList>)SectionsList;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get sectionlist.";
                log.Error("Error during get appraisal list : GetAllSectionOfYear() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetYearQuestionList(int? yearId, int? sectionId)
        {
            var response = Request.CreateResponse();
            try
            {
                var YearQuestionList = objApr.GetYearQuestionList(yearId, sectionId);

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (List<finalMapping>)YearQuestionList;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get yearquestionlist.";
                log.Error("Error during get appraisal list : GetYearQuestionList() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage EmployeeList(int? Id)
        {
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                if (Id == loggedinUser)
                {
                    var EmployeeList = objApr.EmployeeList(Id);

                    objResponse.Status = "Success";
                    objResponse.Message = "";
                    objResponse.Data = (List<employee>)EmployeeList;
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/error/index/You are not authorized for this action");
                }
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Error while get EmployeeList.";
                log.Error("Error during get appraisal list : EmployeeList() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateSections(SectionList section)
        {
            var response = Request.CreateResponse();
            if (section != null)
            {
                List<SectionList> sections = new List<SectionList>();
                sections.Add(section);
                try
                {
                    objApr.SaveUpdateSections(sections);

                    objResponse.Status = "Success";
                    objResponse.Message = "Data Saved...";
                    objResponse.Data = "";
                }
                catch (Exception ex)
                {
                    objResponse.Status = "Error";
                    objResponse.Message = "Failed to save data. Please contact support /administrator";
                    log.Error("Error occured saving data : SaveUpdateSections() ", ex);
                }
                finally
                {
                    //set headers on the "response"
                    response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
                }
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateQuestions(QuestionList question)
        {
            var response = Request.CreateResponse();
            if (question != null)
            {
                List<QuestionList> questions = new List<QuestionList>();
                questions.Add(question);
                try
                {
                    objApr.SaveUpdateQuestions(questions);

                    objResponse.Status = "Success";
                    objResponse.Message = "Data Saved...";
                    objResponse.Data = "";
                }
                catch (Exception ex)
                {
                    objResponse.Status = "Error";
                    objResponse.Message = "Failed to save data. Please contact support /administrator";
                    log.Error("Error occured saving : SaveUpdateQuestions() ", ex);
                }
                finally
                {
                    //set headers on the "response"
                    response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
                }
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveYearSectionMapping(yearSectionMapping data)
        {
            var response = Request.CreateResponse();
            if (data != null)
            {
                List<yearSectionMapping> mappingData = new List<yearSectionMapping>();
                mappingData.Add(data);
                try
                {
                    objApr.SaveYearSectionMapping(mappingData);

                    objResponse.Status = "Success";
                    objResponse.Message = "Data Saved...";
                    objResponse.Data = "";
                }
                catch (Exception ex)
                {
                    objResponse.Status = "Error";
                    objResponse.Message = "Failed to save data. Please contact support /administrator";
                    log.Error("Error occured saving : SaveYearSectionMapping() ", ex);
                }
                finally
                {
                    //set headers on the "response"
                    response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
                }
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveMapping(finalMapping data)
        {
            var response = Request.CreateResponse();
            if (data != null)
            {
                List<finalMapping> mappingData = new List<finalMapping>();
                mappingData.Add(data);
                try
                {
                    objApr.SaveMapping(mappingData);

                    objResponse.Status = "Success";
                    objResponse.Message = "Data Saved...";
                    objResponse.Data = "";
                }
                catch (Exception ex)
                {
                    objResponse.Status = "Error";
                    objResponse.Message = "Failed to save data. Please contact support /administrator";
                    log.Error("Error occured saving : SaveMapping() ", ex);
                }
                finally
                {
                    //set headers on the "response"
                    response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
                }
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdateSectionOrder(List<yearSectionMapping> lstOrder)
        {
            var response = Request.CreateResponse();
            try
            {
                objApr.UpdateSectionOrder(lstOrder);

                objResponse.Status = "Success";
                objResponse.Message = "Data Saved...";
                objResponse.Data = "";
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Failed to save data. Please contact support /administrator";
                log.Error("Error occured saving : UpdateSectionOrder() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdateQuestionOrder(List<finalMapping> lstOrder)
        {
            var response = Request.CreateResponse();
            try
            {
                objApr.UpdateQuestionOrder(lstOrder);

                objResponse.Status = "Success";
                objResponse.Message = "Data Saved...";
                objResponse.Data = "";
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.Message = "Failed to save data. Please contact support /administrator";
                log.Error("Error occured saving : UpdateQuestionOrder() ", ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdateSectionMapping(yearSectionMapping data)
        {
            var response = Request.CreateResponse();
            if (data != null)
            {
                List<yearSectionMapping> mappingData = new List<yearSectionMapping>();
                mappingData.Add(data);
                try
                {
                    objApr.UpdateSectionMapping(mappingData);

                    objResponse.Status = "Success";
                    objResponse.Message = "Data Saved...";
                    objResponse.Data = "";
                }
                catch (Exception ex)
                {
                    objResponse.Status = "Error";
                    objResponse.Message = "Failed to save data. Please contact support /administrator";
                    log.Error("Error occured saving : UpdateSectionMapping() ", ex);
                }
                finally
                {
                    //set headers on the "response"
                    response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
                }
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdateQuestionMapping(finalMapping data)
        {
            var response = Request.CreateResponse();
            if (data != null)
            {
                List<finalMapping> mappingData = new List<finalMapping>();
                mappingData.Add(data);
                try
                {
                    objApr.UpdateQuestionMapping(mappingData);

                    objResponse.Status = "Success";
                    objResponse.Message = "Data Saved...";
                    objResponse.Data = "";
                }
                catch (Exception ex)
                {
                    objResponse.Status = "Error";
                    objResponse.Message = "Failed to save data. Please contact support /administrator";
                    log.Error("Error occured saving : UpdateQuestionMapping() ", ex);
                }
                finally
                {
                    //set headers on the "response"
                    response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
                }
            }
            return response;
        }
    }
}