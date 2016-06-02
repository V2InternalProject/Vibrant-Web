using BLL;
using BOL;
using HRMS.common;
using HRMS.DAL;
using HRMS.Models;
using log4net;
using MailActivity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using V2.Orbit.BusinessLayer;

namespace HRMS.Controllers
{
    public class AppraisalReviewController : ApiController
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AppraisalReview objApr = new AppraisalReview();
        private readonly HRMSResponse objResponse = new HRMSResponse();
        private Random incident = new Random();

        #region General Actions

        /// <summary>
        /// Returns list of menu options
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Menu(int empId)
        {
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                var menu = objApr.Menu(empId, loggedinUser);

                response.Content = new ObjectContent(typeof(List<HRMS.Models.Menu>), menu.MenuList, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while generating menu. Please contact support /administrator with incidentID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error while generating menu. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        ///     Returns current active appraisal year. There should be always only single appraisal year active.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppraisalYear()
        {
            var response = Request.CreateResponse();
            try
            {
                var apprYear = objApr.GetAppraisalYear();

                objResponse.Status = "Success";
                objResponse.Message = "";
                objResponse.Data = (Dictionary<int, string>)apprYear;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.IncidentID = incident.Next(10000, 99999);
                objResponse.Message = "No Appraisal Year is active. Please contact support /administrator with incidentID : " + objResponse.IncidentID;
                log.Error("No Appraisal Year is active. : GetAppraisalYear() with incident ID : " + objResponse.IncidentID, ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        #endregion General Actions

        #region AppraisalIntitiate

        /// <summary>
        ///     Returns list of Probable candidates for appraisal
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppraisalList()
        {
            var response = Request.CreateResponse();
            try
            {
                var AppraiseeList = objApr.GetAppraisalList();
                response.Content = new ObjectContent(typeof(List<ProbableList>), AppraiseeList, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Appraisal candidate list not available. Please contact support /administrator with incidentID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Appraisal candidate list not available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Provides details for the Appraisee
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppraiseeDetails([FromUri]int empId)
        {
            var response = Request.CreateResponse();
            try
            {
                var AppraiseeDetails = objApr.GetAppraiseeDetails(empId);
                response.Content = new ObjectContent(typeof(AppraiseeDetails), AppraiseeDetails, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Appraisee details not available. Please contact support /administrator with incidentID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Appraisee details not available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Returns candidate list ready to initiate appraisal process
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ReadytoInitiate()
        {
            var response = Request.CreateResponse();
            try
            {
                var dsSavedAppraisalList = objApr.GetInitiationList();
                response.Content = new ObjectContent(typeof(List<ProbableList>), dsSavedAppraisalList, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : ReadytoInitiate() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No candidates available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Returns candidates for whom Appraisal process is initiated
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Initiated()
        {
            var response = Request.CreateResponse();
            try
            {
                var dsSavedAppraisalList = objApr.GetInitiated();
                response.Content = new ObjectContent(typeof(List<ProbableList>), dsSavedAppraisalList, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : Initiated() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No candidates available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Freezed()
        {
            var response = Request.CreateResponse();
            try
            {
                var dsFreezedAppraisalList = objApr.GetFreezed();

                response.Content = new ObjectContent(typeof(List<ProbableList>), dsFreezedAppraisalList, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : Freezed() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No candidates available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        ///     Submits appraisees along with setup list
        /// </summary>
        /// <param name="lstAppr"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SaveAppraisalList(List<ProbableList> lstAppr)
        {
            var flag = 1;
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objApr.SaveAppraisalList(lstAppr, "", loggedinUser, "Submit");
                response.Content = new ObjectContent(typeof(int), flag, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving data : SaveAppraisalList() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, "Failed to save candidate setup. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Initiates Appraisal Process for selected list
        /// </summary>
        /// <param name="lstAppr"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Initiate(List<ProbableList> lstAppr)
        {
            var flag = 1;
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objApr.SaveAppraisalList(lstAppr, "", loggedinUser, "initiate");
                response.Content = new ObjectContent(typeof(int), flag, new JsonMediaTypeFormatter());

                SendInitiationMail(lstAppr);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving data : Initiate() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, "Failed to initiate candidate setup. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        private void SendInitiationMail(List<ProbableList> lstAppr)
        {
            try
            {
                EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
                DataSet dsGetMailInfo = new DataSet();
                EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
                EmailActivity objEmailActivity = new EmailActivity();

                char[] separator = new char[] { ';' };
                string failureMsg = "\r\nCandidates initiated but mail sending failed for : ";
                foreach (ProbableList itemAppr in lstAppr)
                {
                    objEmailActivityBOL.ToID = objEmailActivityBOL.ToID + itemAppr.EID.ToString() + ";";
                    //objApr.getMailID(itemAppr.EID);
                }
                objEmailActivityBOL.FromAddress = "smtp-relay@v2solutions.com";
                objEmailActivityBOL.CCID = ConfigurationManager.AppSettings["HR-ADMIN"].ToString();
                objEmailActivityBOL.EmailTemplateName = "Appraisal Initiate";

                dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
                objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
                objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                objEmailActivityBOL.Body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
                objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);

                try
                {
                    objEmailActivity.SendMail(objEmailActivityBOL);
                }
                catch (System.Exception ex)
                {
                    failureMsg = failureMsg + objEmailActivityBOL.ToAddress;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Freezes appraisal process for selected candidates
        /// </summary>
        /// <param name="lstAppr"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Freeze(FreezeInitiation data)
        {
            var flag = 1;
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objApr.SaveAppraisalList(data.lstAppr, data.message, loggedinUser, "freeze");
                response.Content = new ObjectContent(typeof(int), flag, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving data : Freeze() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, "Failed to  freeze appraisal process. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Reinitiates appraisal for selected candidates from the same step wher eit was freezed
        /// </summary>
        /// <param name="lstAppr"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UnFreeze(List<ProbableList> lstAppr)
        {
            var flag = 1;
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objApr.SaveAppraisalList(lstAppr, "", loggedinUser, "unfreeze");
                response.Content = new ObjectContent(typeof(int), flag, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving data : UnFreeze() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, "Failed to  unfreeze appraisal process. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Cancels initiation for selected candidates.
        /// </summary>
        /// <param name="lstAppr"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelInitiate(List<ProbableList> lstAppr)
        {
            var flag = 1;
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objApr.SaveAppraisalList(lstAppr, "", loggedinUser, "cancelled");

                response.Content = new ObjectContent(typeof(int), flag, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving data : CancelInitiate() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, "Failed to  cancel appraisal process. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        ///     Returns list of Employees
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSetupList()
        {
            var response = Request.CreateResponse();
            try
            {
                var setupList = objApr.GetSetupList();

                response.Content = new ObjectContent(typeof(SetupList), setupList, new JsonMediaTypeFormatter());
                objResponse.Data = (SetupList)setupList;
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving data : GetSetupList() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Setup data not available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        #endregion AppraisalIntitiate

        #region Appraisal Review

        /// <summary>
        ///     Returns section based on section id
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Section(int sectionId, int empID)
        {
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                var appraisalSection = objApr.Section(sectionId, empID, loggedinUser);
                if (!string.IsNullOrEmpty(appraisalSection.errorMessage))
                    throw new Exception("No data retrieved.");
                response.Content = new ObjectContent(typeof(AppraisalSection), appraisalSection, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                if (ex.Message.ToLower().Contains("no data retrieved"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error in section. : " + ex.Message + "; incidentID : " + num));
                log.Error("Error while generating section : Section() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error in section. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appraisalSectionPost"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Save([FromUri] int sectionID, [FromUri] string sectionTypeParser, [FromUri]int empID, [FromBody]object Data)
        {
            var response = Request.CreateResponse();
            try
            {
                AppraisalSectionPost appraisalSectionPost = new AppraisalSectionPost();
                appraisalSectionPost.SectionId = sectionID;
                appraisalSectionPost.SectionTypeParser = sectionTypeParser;
                appraisalSectionPost.EmpId = empID;
                if (sectionTypeParser == "JA") appraisalSectionPost.Data = JArray.Parse(Data.ToString());
                else if (sectionTypeParser == "JO") appraisalSectionPost.Data = JObject.Parse(Data.ToString());

                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                appraisalSectionPost.LoggedUser = loggedinUser;
                VibrantHttpResponse status = objApr.saveAppraisalSection(appraisalSectionPost);

                response.Content = new ObjectContent(typeof(VibrantHttpResponse), status, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while saving section : Save() with incident ID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error in section. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        /// Submits Appraisal Form and forwards control to person on next stage.
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Submit([FromUri]int empId)
        {
            var response = Request.CreateResponse();
            try
            {
                AppraisalSectionPost objSubmit = new AppraisalSectionPost();
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objSubmit.EmpId = empId;
                objSubmit.LoggedUser = loggedinUser;

                var status = objApr.Submit(objSubmit);

                response.Content = new ObjectContent(typeof(VibrantHttpResponse), status, new JsonMediaTypeFormatter());

                if (status.Message.ToLower().Contains("success") || status.Message.ToLower().Contains("appraisal submitted to closure") || status.Message.ToLower().Contains("appraisal escalated"))
                    SendSubmitMail(loggedinUser, empId, status.Message);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Submit failed : Submit() with incident ID : " + num, ex);
                if (ex.Message.ToLower().Contains("fill all mendatory sections"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Submit failed. Please fill all mendatory sections."));
                else if (ex.Message.ToLower().Contains("you have already submitted appraisal to next stage"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Submit failed. You have already submitted appraisal to next stage."));
                else if (ex.Message.Contains("Appraisal Submitted to next stage but failed to send mail"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Appraisal submitted successfully but failed to send mail to next stage stake holders. incidentID : " + num));
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Submit failed. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        private void SendSubmitMail(int loggedinUser, int empId, string msg)
        {
            var objApr = new Appraisal();
            EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
            DataSet dsGetMailInfo = new DataSet();
            EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
            EmailActivity objEmailActivity = new EmailActivity();

            char[] separator = new char[] { ';' };

            if (msg.ToLower().Contains("appraisal submitted to closure"))
                dsGetMailInfo = objApr.GetMailDetails(loggedinUser, empId, "complete");
            else
                dsGetMailInfo = objApr.GetMailDetails(loggedinUser, empId, "");

            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.CCID = ConfigurationManager.AppSettings["HR-ADMIN"].ToString();

            //string[] cc = ConfigurationManager.AppSettings["HR-ADMIN"].ToString().Split(';');

            //for (int i = 0; i < cc.Length; i++)
            //{
            //    if (cc[i] != "")
            //    {
            //        objEmailActivityBOL.CCAddress[i] =cc[i];
            //    }
            //}

            objEmailActivityBOL.EmailTemplateName = (dsGetMailInfo.Tables[0].Rows[0]["EmailTemplateName"].ToString());

            // dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());

            try
            {
                objEmailActivity.SendSubmitMail(objEmailActivityBOL, (dsGetMailInfo.Tables[0].Rows[0]["toName"].ToString()), (dsGetMailInfo.Tables[0].Rows[0]["EmployeeName"].ToString()), (dsGetMailInfo.Tables[0].Rows[0]["EmployeeCode"].ToString()));
            }
            catch (System.Exception ex)
            {
                throw new Exception("Appraisal Submitted to next stage but failed to send mail.");
            }
        }

        /// <summary>
        /// Returns Appraisal details based on stage
        /// </summary>
        /// <param name="appraisalSection"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppraisalData(int sectionId, string sectionTypeParser, int empID)
        {
            var response = Request.CreateResponse();
            try
            {
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

                AppraisalSectionPost appraisalSection = new AppraisalSectionPost
                {
                    EmpId = empID,
                    SectionId = sectionId,
                    LoggedUser = loggedinUser,
                    SectionTypeParser = sectionTypeParser
                };
                response.Content = new ObjectContent(typeof(AppraisalSectionPost), objApr.GetAppraisalData(appraisalSection), new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Data not available. Please contact support /administrator with incidentID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not available. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Self()
        {
            var response = Request.CreateResponse();
            try
            {
                int i = 0;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.IncidentID = incident.Next(10000, 99999);
                objResponse.Message = "Section not generated. Please contact support /administrator with incidentID : " + objResponse.IncidentID;
                log.Error("Error while generating section : GetAppraisalList() with incident ID : " + objResponse.IncidentID, ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Review()
        {
            var response = Request.CreateResponse();
            try
            {
                int i = 0;
            }
            catch (Exception ex)
            {
                objResponse.Status = "Error";
                objResponse.IncidentID = incident.Next(10000, 99999);
                objResponse.Message = "Section not generated. Please contact support /administrator with incidentID : " + objResponse.IncidentID;
                log.Error("Error while generating section : GetAppraisalList() with incident ID : " + objResponse.IncidentID, ex);
            }
            finally
            {
                //set headers on the "response"
                response.Content = new ObjectContent(typeof(HRMSResponse), objResponse, new JsonMediaTypeFormatter());
            }
            return response;
        }

        #endregion Appraisal Review

        #region Dump Code

        //[HttpGet]
        //public HttpResponseMessage Assignments(int section)
        //{
        //    try
        //    {
        //        object pd =
        //            new
        //            {
        //                questionid = 1,
        //                questiontext = "Project Definition",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object achv =
        //            new
        //            {
        //                questionid = 2,
        //                questiontext = "Achievement",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = false,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object sd =
        //            new
        //            {
        //                questionid = 3,
        //                questiontext = "Start Date",
        //                datatype = "date",
        //                control = "datepicker",
        //                isrequired = true,
        //                validation =
        //                    new { testvalue = "noFutureDate", failureMessage = "Start Date can not be a future date." }
        //            };
        //        object ed =
        //            new
        //            {
        //                questionid = 4,
        //                questiontext = "End Date",
        //                datatype = "date",
        //                control = "datepicker",
        //                isrequired = false,
        //                validation =
        //                    new
        //                    {
        //                        testvalue = "postSD",
        //                        failureMessage = "End date must be a later date than Start Date."
        //                    }
        //            };
        //        object pm =
        //            new
        //            {
        //                questionid = 5,
        //                questiontext = "Project Manager Name",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object am =
        //            new
        //            {
        //                questionid = 6,
        //                questiontext = "Appreciation Mail",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };

        //        object data =
        //            new
        //            {
        //                sectionid = 1,
        //                yearid = 2,
        //                sectiontype = "V",
        //                questions = new { pd, achv, sd, ed, pm, am },
        //                param = new { }
        //            };

        //        //@"param"": {""pd"": [],""achv"": [],""sd"": [],""ed"": [],""pm"": [],""am"": []}}}";
        //        //string apform = @"{""data"": {""sectionid"": 1,""yearid"": 2,""sectiontype"": ""V"",""questions"": [{""pd"": {""questionid"": 1,""questiontext"": ""Project Definition"",""seq"": 1,""dataType"": ""text"",""controlType"": ""text"",""isRequired"": ""1"",""validation"": {""testvalue"": """",""failureMessage"": """"}},""achv"": {""questionid"": 2,""questiontext"": ""Achievement"",""seq"": 2,""dataType"": ""text"",""controlType"": ""text"",""isRequired"": ""0"",""validation"": {""testvalue"": """",""failureMessage"": """"}},""sd"": {""questionid"": 3,""questiontext"": ""Start Date"",""seq"": 3,""dataType"": ""date"",""controlType"": ""datepicker"",""isRequired"": ""1"",""validation"": {""testvalue"": ""isDate"",""failureMessage"": ""Enter Valid Date""},{""testvalue"": ""noFutureDate"",""failureMessage"": ""Start Date can not be a future date.""}},""ed"": {""questionid"": 4,""questiontext"": ""End Date"",""seq"": 4,""dataType"": ""date"",""controlType"": ""datepicker"",""isRequired"": ""0"",""validation"": {""testvalue"": ""isDate"",""failureMessage"": ""Enter Valid Date""},{""testvalue"": ""postsd"",""failureMessage"": ""End date can not be prior than Start Date.""}},""pm"": {""questionid"": 5,""questiontext"": ""Project Manager Name"",""seq"": 5,""dataType"": ""int"",""controlType"": ""dropdown"",""isRequired"": ""1"",""validation"": {""testvalue"": """",""failureMessage"": """"}},""am"": {""questionid"": 6,""questiontext"": ""Appreciation mail""""seq"": 6,""dataType"": ""text"",""controlType"": ""text"",""isRequired"": ""0"",""validation"": {""testvalue"": """",""failureMessage"": """"}},}],""param"": {""pd"": [],""achv"": [],""sd"": [],""ed"": [],""pm"": [],""am"": []}}}";
        //        var response = Request.CreateResponse();
        //        response.Content = new ObjectContent(typeof(object), data, new JsonMediaTypeFormatter());
        //        //set headers on the "response"
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error during retrieveing appraisal candidate list : GetAppraisalList() ", ex);
        //        return null;
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage Factors(int section)
        //{
        //    try
        //    {
        //        object data = null;
        //        object sn =
        //            new
        //            {
        //                questionid = 1,
        //                questiontext = "S No",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = false,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object ar =
        //            new
        //            {
        //                questionid = 2,
        //                questiontext = "Area",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = false,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object ac =
        //            new
        //            {
        //                questionid = 3,
        //                questiontext = "Appraisee's Comment",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object a1c =
        //            new
        //            {
        //                questionid = 4,
        //                questiontext = "Appraiser 1 Comment",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object a2c =
        //            new
        //            {
        //                questionid = 5,
        //                questiontext = "Appraiser 2 Comment",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };
        //        object rc =
        //            new
        //            {
        //                questionid = 6,
        //                questiontext = "Reviewer 1 Comment",
        //                datatype = "text",
        //                control = "text",
        //                isrequired = true,
        //                validation = new { testvalue = "", failureMessage = "" }
        //            };

        //        if (section == 1)
        //            data =
        //                new
        //                {
        //                    sectionid = 2,
        //                    yearid = 2,
        //                    sectiontype = "FR",
        //                    questions = new { sn, ar, ac },
        //                    param =
        //                        new
        //                        {
        //                            sn = new { r1 = "A", r2 = "B", r3 = "C", r4 = "D", r5 = "E" },
        //                            ar =
        //                                new
        //                                {
        //                                    r1 = "Facilitating Factors (self)",
        //                                    r2 = "Facilitating Factors (environment)",
        //                                    r3 = "Inhibiting Factors (Self related)",
        //                                    r4 = "Inhibiting Factors (Environment Related)",
        //                                    r5 = "Support Expected/ Required from Organization in Future"
        //                                }
        //                        }
        //                };

        //        if (section == 2)
        //            data =
        //                new
        //                {
        //                    sectionid = 2,
        //                    yearid = 2,
        //                    sectiontype = "FR",
        //                    questions = new { sn, ar, ac, a1c },
        //                    param =
        //                        new
        //                        {
        //                            sn = new { r1 = "A", r2 = "B", r3 = "C", r4 = "D", r5 = "E" },
        //                            ar =
        //                                new
        //                                {
        //                                    r1 = "Facilitating Factors (self)",
        //                                    r2 = "Facilitating Factors (environment)",
        //                                    r3 = "Inhibiting Factors (Self related)",
        //                                    r4 = "Inhibiting Factors (Environment Related)",
        //                                    r5 = "Support Expected/ Required from Organization in Future"
        //                                }
        //                        }
        //                };

        //        if (section == 3)
        //            data =
        //                new
        //                {
        //                    sectionid = 2,
        //                    yearid = 2,
        //                    sectiontype = "FR",
        //                    questions = new { sn, ar, ac, a1c, a2c, rc },
        //                    param =
        //                        new
        //                        {
        //                            sn = new { r1 = "A", r2 = "B", r3 = "C", r4 = "D", r5 = "E" },
        //                            ar =
        //                                new
        //                                {
        //                                    r1 = "Facilitating Factors (self)",
        //                                    r2 = "Facilitating Factors (environment)",
        //                                    r3 = "Inhibiting Factors (Self related)",
        //                                    r4 = "Inhibiting Factors (Environment Related)",
        //                                    r5 = "Support Expected/ Required from Organization in Future"
        //                                }
        //                        }
        //                };
        //        //@"param"": {""pd"": [],""achv"": [],""sd"": [],""ed"": [],""pm"": [],""am"": []}}}";
        //        //string apform = @"{""data"": {""sectionid"": 1,""yearid"": 2,""sectiontype"": ""V"",""questions"": [{""pd"": {""questionid"": 1,""questiontext"": ""Project Definition"",""seq"": 1,""dataType"": ""text"",""controlType"": ""text"",""isRequired"": ""1"",""validation"": {""testvalue"": """",""failureMessage"": """"}},""achv"": {""questionid"": 2,""questiontext"": ""Achievement"",""seq"": 2,""dataType"": ""text"",""controlType"": ""text"",""isRequired"": ""0"",""validation"": {""testvalue"": """",""failureMessage"": """"}},""sd"": {""questionid"": 3,""questiontext"": ""Start Date"",""seq"": 3,""dataType"": ""date"",""controlType"": ""datepicker"",""isRequired"": ""1"",""validation"": {""testvalue"": ""isDate"",""failureMessage"": ""Enter Valid Date""},{""testvalue"": ""noFutureDate"",""failureMessage"": ""Start Date can not be a future date.""}},""ed"": {""questionid"": 4,""questiontext"": ""End Date"",""seq"": 4,""dataType"": ""date"",""controlType"": ""datepicker"",""isRequired"": ""0"",""validation"": {""testvalue"": ""isDate"",""failureMessage"": ""Enter Valid Date""},{""testvalue"": ""postsd"",""failureMessage"": ""End date can not be prior than Start Date.""}},""pm"": {""questionid"": 5,""questiontext"": ""Project Manager Name"",""seq"": 5,""dataType"": ""int"",""controlType"": ""dropdown"",""isRequired"": ""1"",""validation"": {""testvalue"": """",""failureMessage"": """"}},""am"": {""questionid"": 6,""questiontext"": ""Appreciation mail""""seq"": 6,""dataType"": ""text"",""controlType"": ""text"",""isRequired"": ""0"",""validation"": {""testvalue"": """",""failureMessage"": """"}},}],""param"": {""pd"": [],""achv"": [],""sd"": [],""ed"": [],""pm"": [],""am"": []}}}";
        //        var response = Request.CreateResponse();
        //        response.Content = new ObjectContent(typeof(object), data, new JsonMediaTypeFormatter());
        //        //set headers on the "response"
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error during retrieveing appraisal candidate list : GetAppraisalList() ", ex);
        //        return null;
        //    }
        //}

        #endregion Dump Code

        [HttpGet]
        public HttpResponseMessage For(int empId)
        {
            /*
             Get menu
             * get section for each Menu
             * Get data for each Menu
             * Create Section Wise data
             *      -> section name
             *      -> section data
             *      -> review Comment
             *      Create JSON Object
             *      Send to data Base

             */

            var response = Request.CreateResponse();
            try
            {
                List<AppraisalReviewPost> _data = new List<AppraisalReviewPost>();
                var loggedinUser = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                var menu = objApr.Menu(empId, loggedinUser);
                int sectionId = 0;
                foreach (var menuItem in menu.MenuList)
                {
                    var menuUrlArr = menuItem.URL.Split('/');
                    if (!string.IsNullOrEmpty(Convert.ToString(menuUrlArr[menuUrlArr.Length - 2])) && Convert.ToString(menuUrlArr[menuUrlArr.Length - 2]).ToLower() != "for")
                        sectionId = int.Parse(menuUrlArr[menuUrlArr.Length - 2]);

                    var appraisalSection = objApr.Section(sectionId, empId, loggedinUser);

                    var appraisalSectionPost = new AppraisalSectionPost
               {
                   EmpId = empId,
                   SectionId = sectionId,
                   LoggedUser = loggedinUser,
                   SectionTypeParser = appraisalSection.sectionTypeParser
               };
                    if (appraisalSection.sectionTypeParser != null)
                    {
                        var data = objApr.GetAppraisalData(appraisalSectionPost);

                        _data.Add(new AppraisalReviewPost
                        {
                            Data = data.Data,
                            param = appraisalSection.param,
                            SectionName = appraisalSection.sectionName,
                            Questions = GetQuestions(appraisalSection.questions),
                            SectionId = appraisalSection.sectionId,
                            Type = appraisalSection.sectionTypeParser,
                            Appraiser1Comment = "",
                            Appraiser2Comment = ""
                        });
                    }
                }

                AppraiseeDetails objAppraiseeDetail = objApr.GetAppraiseeDetails(empId);
                AppraisalReviewList reviewPost = new AppraisalReviewList() { sections = _data, Appriasee = new Appraisee { FullName = objAppraiseeDetail.Employeename, EmployeeCode = Convert.ToInt32(objAppraiseeDetail.EmployeeCode) } };
                response.Content = new ObjectContent(typeof(AppraisalReviewList), reviewPost, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error while generating menu. Please contact support /administrator with incidentID : " + num, ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error while generating menu. Please contact support /administrator with incidentID : " + num));
            }
            return response;
        }

        private Dictionary<string, string> GetQuestions(Dictionary<string, AppraisalQuestions> questions)
        {
            Dictionary<string, string> _list = new Dictionary<string, string>();

            var questionsOrdered = questions.OrderBy(x => x.Value.seq);

            foreach (var current in questionsOrdered)
                _list.Add(current.Key, current.Value.questionText);
            return _list;
        }
    }
}