using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HRMS.DAL
{
    public class SemDAL
    {
        private WSEMDBEntities dbContext = new WSEMDBEntities();
        private HRMSDBEntities dbContextHRMS = new HRMSDBEntities();

        public List<SEMViewModel> CustomerDetailRecord(string searchText, int page, int rows, out int totalCount)
        {
            List<SEMViewModel> CustomerRecords = new List<SEMViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(searchText))
                    searchText = searchText.Trim();

                CustomerRecords = (from cust in dbContext.tbl_PM_Customer
                                   where cust.CustomerName.StartsWith(searchText)
                                   select new SEMViewModel
                                     {
                                         CustomerId = cust.Customer,
                                         CustomerName = cust.CustomerName,
                                         Region = (from relation in dbContext.tbl_CNF_RegionMaster
                                                   where relation.RegionID == cust.RegionID
                                                   select relation.RegionName).FirstOrDefault(),
                                         ContractSigningDate = cust.DateSigned,
                                         ContractValidityDate = cust.ContractValidityDate
                                     }).OrderBy(x => x.CustomerName).ToList();

                totalCount = CustomerRecords.Count();

                return CustomerRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PMSProjectDetailsViewModel> ProjectReviewerDetailsRecord(int ProjectID, int page, int rows, out int totalCount)
        {
            try
            {
                List<PMSProjectDetailsViewModel> ProjectReviewerDetails = new List<PMSProjectDetailsViewModel>();

                var ProjectReviewerDetail = dbContext.GetProjectReviewerDetails_sp(ProjectID);
                //if (ProjectReviewerDetail.Count() > 0)
                //{
                ProjectReviewerDetails = (from reviewerdetails in ProjectReviewerDetail
                                          select new PMSProjectDetailsViewModel
                                          {
                                              ProjectReviewerId = reviewerdetails.ProjectReviewerId,
                                              ProjectID = reviewerdetails.ProjectId,
                                              EmployeeId = reviewerdetails.EmployeeId,
                                              EmployeeName = reviewerdetails.EmployeeName,
                                              PMSProjectStartDate = reviewerdetails.FromDate,
                                              PMSProjectEndDate = reviewerdetails.ToDate,
                                              RoleDescription = reviewerdetails.RoleDescription
                                          }).ToList();
                //}
                totalCount = ProjectReviewerDetails.Count();
                return ProjectReviewerDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PMSProjectDetailsViewModel> ProjectReviewerDetailsforEmail(int ProjectID)
        {
            try
            {
                List<PMSProjectDetailsViewModel> ProjectReviewerDetails = new List<PMSProjectDetailsViewModel>();

                var ProjectReviewerDetail = dbContext.GetProjectReviewerDetails_sp(ProjectID);
                //if (ProjectReviewerDetail.Count() > 0)
                //{
                ProjectReviewerDetails = (from reviewerdetails in ProjectReviewerDetail
                                          select new PMSProjectDetailsViewModel
                                          {
                                              ProjectReviewerId = reviewerdetails.ProjectReviewerId,
                                              ProjectID = reviewerdetails.ProjectId,
                                              EmployeeId = reviewerdetails.EmployeeId,
                                              EmployeeName = reviewerdetails.EmployeeName,
                                              PMSProjectStartDate = reviewerdetails.FromDate,
                                              PMSProjectEndDate = reviewerdetails.ToDate,
                                              RoleDescription = reviewerdetails.RoleDescription
                                          }).ToList();
                //}
                return ProjectReviewerDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RevisionList> GetApprooveRevisionProjectDetails(int? ProjectID)
        {
            try
            {
                int ProjID = ProjectID.HasValue ? ProjectID.Value : 0;
                var auditTrail = dbContext.GetRevisionApprovalFeildList_SP(ProjID);
                List<RevisionList> AuditTrailList = new List<RevisionList>();
                AuditTrailList = (from s in auditTrail
                                  select new RevisionList
                                {
                                    NewValue = s.Value,
                                    OldValue = s.OldValue,
                                    FeildName = s.FieldName
                                }).ToList();
                return AuditTrailList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PMSProjectDetailsViewModel> getManagerRevisionComment(int? projectId)
        {
            try
            {
                var ManagerRevisionComments = dbContext.GetManagerRevisionComments_SP(projectId);
                List<PMSProjectDetailsViewModel> PMSProjectDetails = (from s in ManagerRevisionComments
                                                                      select new PMSProjectDetailsViewModel
                                                                      {
                                                                          ManagerRevisionComment = s.SendersComment
                                                                      }).ToList();
                return PMSProjectDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PMSProjectDetailsViewModel> getfieldlabellist(int? ProjectID)
        {
            int? projID = ProjectID;
            var FeildNames = dbContext.getfieldlabellistWithRevisionStatus_SP(projID);

            List<PMSProjectDetailsViewModel> FeildName = (from feilds in FeildNames
                                                          select new PMSProjectDetailsViewModel
                                                         {
                                                             FeildName = feilds.FieldName
                                                         }).ToList();
            //(from change in dbContext.tbl_PM_AuditTrail
            // where change.ProjectID == ProjectID && (change.FieldName == "End Date" || change.FieldName == "Work(Hours)")
            // orderby change.LogID ascending
            // select change.FieldName).ToList();

            return FeildName;
        }

        public List<PMSProjectDetailsViewModel> approvalStatusIdList(int? ProjectID)
        {
            int? projID = ProjectID;
            var RevisionStatusIDs = dbContext.getfieldlabellistWithRevisionStatus_SP(projID);

            List<PMSProjectDetailsViewModel> RevisionStatusID = (from feilds in RevisionStatusIDs
                                                                 select new PMSProjectDetailsViewModel
                                                                 {
                                                                     RevisionStaus = feilds.RevisionStatusID.HasValue ? feilds.RevisionStatusID.Value : 0
                                                                 }).ToList();
            //List<int?> RevisionStatusID = (from change in dbContext.tbl_PM_AuditTrail
            //                               where change.ProjectID == ProjectID && (change.FieldName == "End Date" || change.FieldName == "Work(Hours)")
            //                               orderby change.LogID ascending
            //                               select change.RevisionStatusID).ToList();
            return RevisionStatusID;
        }

        public Tuple<bool, bool> saveApproveDetailsIntrail(int? ProjectID, string btnClick, string ApprovalComment, int employeeId, bool IsEndDateChanged, bool IsWorkHourChanged)
        {
            try
            {
                int? ProjId = ProjectID;
                string buttonClick = btnClick;
                string ApprovalComments = ApprovalComment;
                string IsEndDateChange = string.Empty;
                string IsWorkHourChange = string.Empty;
                DateTime? oldEndDate = null;
                DateTime? newEndDate = null;
                bool resetStatus = false;
                if (IsEndDateChanged == true)
                    IsEndDateChange = "True";
                else
                    IsEndDateChange = "False";
                if (IsWorkHourChanged == true)
                    IsWorkHourChange = "True";
                else
                    IsWorkHourChange = "False";
                SearchedUserDetails EmployeeDetails = GetEmployeeDetails(employeeId);
                string UserName = null;
                if (EmployeeDetails != null)
                    UserName = EmployeeDetails.UserName;

                List<RevisionList> fieldList = GetApprooveRevisionProjectDetails(ProjectID);

                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                dbContext.ApproveRejectRevisionStatus_SP(ProjId, btnClick, ApprovalComments, UserName, IsEndDateChange, IsWorkHourChange, Result);

                bool status = Convert.ToBoolean(Result.Value);
                ObjectParameter Results = new ObjectParameter("Result", typeof(int));
                dbContext.AddUpdateRevisionComments_SP(ProjectID, null, null, "Update", UserName, ApprovalComment, Results);

                bool statusSecond = Convert.ToBoolean(Results.Value);
                if (status == true && statusSecond == true)
                    status = true;
                else
                    status = false;

                foreach (var item in fieldList)
                {
                    if (item.FeildName == "End Date")
                    {
                        oldEndDate = Convert.ToDateTime(item.OldValue);
                        newEndDate = Convert.ToDateTime(item.NewValue);
                    }
                }
                if (IsEndDateChanged == true && status == true && statusSecond == true && newEndDate > oldEndDate)
                {
                    ObjectParameter ResetResult = new ObjectParameter("ResetResult", typeof(int));
                    dbContext.ResetResourceAllocationFromHistory_SP(ProjectID, oldEndDate, newEndDate, ResetResult);
                    resetStatus = Convert.ToBoolean(Result.Value);
                }
                return new Tuple<bool, bool>(status, resetStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveProjectReviewersDetail(PMSProjectDetailsViewModel model, DateTime ToDate)
        {
            try
            {
                int projectId = model.ProjectID.HasValue ? model.ProjectID.Value : 0;
                int employeeId = model.EmployeeId;
                DateTime? fromDate = DateTime.Now;//model.PMSProjectStartDate;
                DateTime? toDate = ToDate;// model.PMSProjectEndDate;
                int? roleId = model.RoleId;
                int projReviewerId = 0;
                ObjectParameter ProjectReviewerId = new ObjectParameter("ProjectReviewerId", typeof(int));
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));

                if (model.ProjectReviewerId != 0)
                {
                    //Customer = CusContantDetails.CustomerIds;
                    //dbContext.AddUpdateCustomerContacts_SP(Customer, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "UPDATE", Output, CustID);
                    projReviewerId = model.ProjectReviewerId;
                    dbContext.AddUpdateReviewerDetails_SP(projReviewerId, projectId, employeeId, fromDate, toDate, roleId, "UPDATE", Result, ProjectReviewerId);
                }
                else
                {
                    dbContext.AddUpdateReviewerDetails_SP(projReviewerId, projectId, employeeId, fromDate, toDate, roleId, "INSERT", Result, ProjectReviewerId);
                    //dbContext.AddUpdateCustomerContacts_SP(Cusid, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "INSERT", Output, CustID);
                }
                bool status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getRoleIdByDesription(string Description)
        {
            try
            {
                int roleId = dbContext.tbl_PM_RoleSem.Where(x => x.RoleDescription == Description).Select(x => x.RoleID).FirstOrDefault();
                return roleId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProjectReviewers> GetProjectReviewersName()
        {
            try
            {
                //List<ProjectReviewers> ProjectReviewers = new List<ProjectReviewers>()
                //{
                //    new ProjectReviewers {EmployeeId=511,EmployeeName="Anjan Chatterji"},
                //    new ProjectReviewers {EmployeeId=806,EmployeeName="Raj thakkar"}
                //};
                var projectReviewersList = dbContext.GetEmployeeListWithEmployeeName_SP();
                List<ProjectReviewers> ProjectReviewers = (from reviewer in projectReviewersList
                                                           orderby reviewer.EmployeeName ascending
                                                           select new ProjectReviewers
                                                           {
                                                               EmployeeId = reviewer.employeeid.HasValue ? reviewer.employeeid.Value : 0,
                                                               EmployeeName = reviewer.EmployeeName
                                                           }).ToList();
                return ProjectReviewers;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProjectReviewers> GetProjectIRApproverName()
        {
            try
            {
                var projectIRApproverList = dbContext.GetProjectIRApprovers_SP();
                List<ProjectReviewers> ProjectIRApprover = (from reviewer in projectIRApproverList
                                                            orderby reviewer.EmployeeName ascending
                                                            select new ProjectReviewers
                                                            {
                                                                EmployeeId = reviewer.employeeid.HasValue ? reviewer.employeeid.Value : 0,
                                                                EmployeeName = reviewer.EmployeeName
                                                            }).ToList();
                return ProjectIRApprover;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProjectReviewers> GetProjectIRFinanceApproverName()
        {
            try
            {
                var projectIRFinanceApproverList = dbContext.GetProjectIRFinanceApprovers_SP();
                List<ProjectReviewers> ProjectIRFinanceReviewers = (from reviewer in projectIRFinanceApproverList
                                                                    orderby reviewer.EmployeeName ascending
                                                                    select new ProjectReviewers
                                                                    {
                                                                        EmployeeId = reviewer.employeeid.HasValue ? reviewer.employeeid.Value : 0,
                                                                        EmployeeName = reviewer.EmployeeName
                                                                    }).ToList();
                return ProjectIRFinanceReviewers;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckIfEmployeeisReviewer(int employeeid)
        {
            try
            {
                bool status;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                int empID = employeeid;
                dbContext.CheckifEmployeeIsReviewer_SP(empID, Output);
                return status = Convert.ToBoolean(Output.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckIfEmployeeisReviewerForParticularProject(int? projectID, int employeeID)
        {
            try
            {
                bool status;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                int empID = employeeID;
                dbContext.CheckifEmployeeIsReviewerForPerticularProject_SP(projectID, empID, Output);
                return status = Convert.ToBoolean(Output.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int geteEmployeeIDFromSEMDatabase(string employeeCode)
        {
            try
            {
                int employeeid = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == employeeCode).FirstOrDefault().EmployeeID;
                return employeeid;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetRoleByEmployeeID(int employeeId)
        {
            try
            {
                ObjectParameter Output = new ObjectParameter("role", typeof(string));
                string employeeRole;
                int employeeID = employeeId;
                dbContext.GetEmployeeRoleByEmployeeId_SP(employeeID, Output);
                return employeeRole = (Output.Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public v_tbl_PM_Customer GetCustomerDetails(int? Customerid)
        {
            try
            {
                v_tbl_PM_Customer CustDetails = dbContext.v_tbl_PM_Customer.Where(ed => ed.Customer == Customerid).FirstOrDefault();
                return CustDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CountryDetailsListSEM> GetTravelCountryDetails()
        {
            List<CountryDetailsListSEM> conutries = new List<CountryDetailsListSEM>();
            HRMSDBEntities dbContext = new HRMSDBEntities();
            try
            {
                conutries = (from country in dbContext.tbl_PM_CountryMaster
                             orderby country.CountryName
                             select new CountryDetailsListSEM
                             {
                                 CountryId = country.CountryID,
                                 CountryName = country.CountryName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        public List<TypeOfContactListSEM> GetTypeOfContactList()
        {
            List<TypeOfContactListSEM> typeofcontacts = new List<TypeOfContactListSEM>();
            WSEMDBEntities WSEMdbContext = new WSEMDBEntities();
            try
            {
                var typeofcontactlist = WSEMdbContext.GetTypeOfContactDetails_SP();
                typeofcontacts = (from c in typeofcontactlist
                                  select new TypeOfContactListSEM
                                  {
                                      ContactTypeID = c.ContactTypeID,
                                      ContactTypeName = c.ContactTypeName
                                  }).ToList();
                return typeofcontacts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExternalMarketSegmentation> GetExtMktSegmentList()
        {
            List<ExternalMarketSegmentation> conutries = new List<ExternalMarketSegmentation>();
            WSEMDBEntities dbContext = new WSEMDBEntities();
            try
            {
                conutries = (from country in dbContext.tbl_PM_DomainMaster
                             orderby country.DomainName
                             select new ExternalMarketSegmentation
                             {
                                 ExtMaktSegID = country.DomainID,
                                 ExtMaktSeg = country.DomainName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        public List<Region> GetRegionTypeList()
        {
            List<Region> conutries = new List<Region>();
            WSEMDBEntities dbContext = new WSEMDBEntities();
            try
            {
                conutries = (from country in dbContext.tbl_CNF_RegionMaster
                             orderby country.RegionName
                             select new Region
                             {
                                 RegionID = country.RegionID,
                                 RegionNames = country.RegionName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        public List<ProjectNamesListDetails> GetProjectNamesList()
        {
            List<ProjectNamesListDetails> projectNames = new List<ProjectNamesListDetails>();

            var projectList = dbContext.GetProjectNames_SP();

            projectNames = (from s in projectList
                            select new ProjectNamesListDetails
                            {
                                ProjectCode = s.ProjectCode,
                                ProjectName = s.ProjectName
                            }).ToList();

            return projectNames.ToList();
        }

        public List<PMSApprovalStatusListDetails> GetPMSApprovalStatusList()
        {
            List<PMSApprovalStatusListDetails> approvalStatus = new List<PMSApprovalStatusListDetails>();

            var approvalStatusList = dbContext.GetPMSApprovalStatus_SP();

            approvalStatus = (from s in approvalStatusList
                              select new PMSApprovalStatusListDetails
                              {
                                  PMSApprovalStatusID = s.ApprovalStatusID,
                                  PMSApprovalStatus = s.ApprovalStatus
                              }).ToList();

            return approvalStatus.ToList();
        }

        public List<PMSProjectStatusListDetails> GetProjectStatusList()
        {
            List<PMSProjectStatusListDetails> ProjectStatus = new List<PMSProjectStatusListDetails>();

            var ProjectStatusList = dbContext.GetPMSProjectStatus_SP();

            ProjectStatus = (from s in ProjectStatusList
                             select new PMSProjectStatusListDetails
                             {
                                 PMSProjectStatusID = Convert.ToInt16(s.ProjectStatusID),
                                 PMSProjectStatus = s.ProjectStatus
                             }).ToList();

            return ProjectStatus.ToList();
        }

        public List<PMSOrganizationUnitListDetails> GetPMSOrganizationUnitList()
        {
            List<PMSOrganizationUnitListDetails> organizationUnit = new List<PMSOrganizationUnitListDetails>();

            var organizationUnitList = dbContext.GetPMSOrganizationUnit_SP();

            organizationUnit = (from s in organizationUnitList
                                select new PMSOrganizationUnitListDetails
                                {
                                    PMSOrganizationUnitID = s.LocationID,
                                    PMSOrganizationUnit = s.Location
                                }).ToList();

            return organizationUnit.ToList();
        }

        public List<PMSDeliveryUnitListDetails> GetPMSDeliveryUnitList()
        {
            List<PMSDeliveryUnitListDetails> deliveryUnit = new List<PMSDeliveryUnitListDetails>();

            var deliveryUnitList = dbContext.GetPMSDeliveryUnit_SP();

            deliveryUnit = (from s in deliveryUnitList
                            select new PMSDeliveryUnitListDetails
                            {
                                PMSDeliveryUnitID = s.ResourcePoolID,
                                PMSDeliveryUnit = s.ResourcePoolName
                            }).ToList();

            return deliveryUnit.ToList();
        }

        public List<PMSDeliveryTeamListDetails> GetPMSDeliveryTeamList()
        {
            List<PMSDeliveryTeamListDetails> deliveryTeam = new List<PMSDeliveryTeamListDetails>();

            var deliveryTeamList = dbContext.GetPMSDeliveryTeam_SP();

            deliveryTeam = (from s in deliveryTeamList
                            select new PMSDeliveryTeamListDetails
                            {
                                PMSDeliveryTeamID = s.GroupID,
                                PMSDeliveryTeam = s.GroupName
                            }).ToList();

            return deliveryTeam.ToList();
        }

        public List<PMSCustomerListDetails> GetPMSCustomerList()
        {
            List<PMSCustomerListDetails> customer = new List<PMSCustomerListDetails>();

            var customerList = dbContext.GetPMSCustomer_SP();

            customer = (from s in customerList
                        select new PMSCustomerListDetails
                        {
                            PMSCustomerID = s.Customer,
                            PMSCustomer = s.CustomerName
                        }).ToList();

            return customer.ToList();
        }

        public Tuple<bool, int, bool> SaveCustomerDetail(AddCustomerModel custdetail)
        {
            bool status = false;
            int CustomerId = 0;
            bool isAbbreviatedNameExist = false;
            var customerDetails = dbContext.GetPMSCustomer_SP();
            List<AddCustomerModel> abbreviatedNameExist = (from a in customerDetails
                                                           where a.CustomerID.ToLower() == custdetail.AbbreviatedName.ToLower()
                                                           select new AddCustomerModel
                                                           {
                                                               AbbreviatedName = a.CustomerID
                                                           }).ToList();
            string City = custdetail.City;
            string EmailID = custdetail.EmailAddress;
            string PinCode = custdetail.ZipCode;
            string Address = custdetail.Address;
            string CustomerID = custdetail.AbbreviatedName;
            int? Customer = 0;
            string CustomerName = custdetail.CustomerName;

            string State = custdetail.State;
            string Country = custdetail.Countrynames;
            string Phone = custdetail.AlternatePhoneNumber;
            string FaxNumber = custdetail.FaxNumber;
            string MobileNumber = custdetail.PhoneNumber;
            string HomePhone = custdetail.PhoneNumber;
            DateTime? DateSigned = custdetail.ContractSigningDate;
            DateTime? ContractValidityDate = custdetail.ContractValidityDate;
            int? CreditPeriod = custdetail.CreditPeriod;
            int? BusinessType = custdetail.ExtMaktSegName;
            int? RegionID = custdetail.RegionName;

            ObjectParameter CustID = new ObjectParameter("CustID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            if (custdetail.CutomerIds != 0)
            {
                Customer = custdetail.CutomerIds;

                dbContext.AddUpdateCustomerDetails_SP(Customer, CustomerID, CustomerName, Address, City, State, Country, PinCode, Phone, FaxNumber, EmailID, MobileNumber, HomePhone, DateSigned, CreditPeriod, ContractValidityDate, BusinessType, RegionID, "UPDATE", Output, CustID);
            }
            else if (custdetail.CutomerIds == 0 && abbreviatedNameExist.Count == 0)
            {
                dbContext.AddUpdateCustomerDetails_SP(Customer, CustomerID, CustomerName, Address, City, State, Country, PinCode, Phone, FaxNumber, EmailID, MobileNumber, HomePhone, DateSigned, CreditPeriod, ContractValidityDate, BusinessType, RegionID, "INSERT", Output, CustID);
            }
            else
            {
                status = false;
                CustomerId = custdetail.CutomerIds;
                isAbbreviatedNameExist = true;
                return new Tuple<bool, int, bool>(status, CustomerId, isAbbreviatedNameExist);
            }

            status = Convert.ToBoolean(Output.Value);

            if (custdetail.CutomerIds == 0)
            {
                CustomerId = Convert.ToInt32(CustID.Value);
            }
            else
            {
                CustomerId = custdetail.CutomerIds;
            }

            return new Tuple<bool, int, bool>(status, CustomerId, isAbbreviatedNameExist);
        }

        public bool SaveInvoiceAddressDetail(AddCustomerAddress InvoiceDetails, int CustInvoiceId, int CountryID, bool SameAddess)
        {
            string City = InvoiceDetails.City;
            string EmailId = InvoiceDetails.EmailId;
            string ZipCode = InvoiceDetails.ZipCode;
            string Details = InvoiceDetails.Details;
            int? Customer = 0;
            string State = InvoiceDetails.State;
            string Country = Convert.ToString(CountryID);
            string PhoneNumber = InvoiceDetails.PhoneNumber;
            string Address = InvoiceDetails.Address;
            int Cusid = CustInvoiceId;
            bool SameAddessval = SameAddess;

            ObjectParameter CustID = new ObjectParameter("CustID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            if (InvoiceDetails.CutomerIds != 0)
            {
                Customer = InvoiceDetails.CutomerIds;
                dbContext.AddUpdateInvoiceAddress_SP(Customer, Address, Country, State, City, ZipCode, PhoneNumber, EmailId, Details, SameAddessval, "UPDATE", Output, CustID);
            }
            else
            {
                dbContext.AddUpdateInvoiceAddress_SP(Cusid, Address, Country, State, City, ZipCode, PhoneNumber, EmailId, Details, SameAddessval, "INSERT", Output, CustID);
            }
            bool status = Convert.ToBoolean(Output.Value);
            return status;
        }

        public List<AddCustomerAddress> AddreessInvoiceDetailRecord(int CustomerID, int page, int rows, out int totalCount)
        {
            List<AddCustomerAddress> CustomerRecords = new List<AddCustomerAddress>();

            HRMSDBEntities HrmsDbContext = new HRMSDBEntities();
            try
            {
                var customerList = dbContext.GetInvoiceAddressDetails_SP();

                CustomerRecords = (from s in customerList
                                   where s.CustID == CustomerID
                                   select new AddCustomerAddress
                                   {
                                       CutomerIds = s.CustomerId,
                                       CustID = s.CustID,
                                       Address = s.Address,
                                       City = s.City,
                                       PhoneNumber = s.PhoneNumber,
                                       Country = (from relation in HrmsDbContext.tbl_PM_CountryMaster
                                                  where relation.CountryID == s.Country
                                                  select relation.CountryName).FirstOrDefault(),
                                       Details = s.Details,
                                       State = s.State,
                                       ZipCode = s.ZipCode,
                                       EmailId = s.EmailId,
                                       SameAddess = s.SameAddess
                                   }).ToList();

                totalCount = CustomerRecords.Count();
                return CustomerRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PMSProjectGroupListDetails> GetPMSProjectGroupList()
        {
            List<PMSProjectGroupListDetails> projectGroup = new List<PMSProjectGroupListDetails>();

            var projectGroupList = dbContext.GetPMSProjectGroup_SP();

            projectGroup = (from s in projectGroupList
                            select new PMSProjectGroupListDetails
                            {
                                PMSProjectGroupID = s.ProjectGroupID,
                                PMSProjectGroup = s.ProjectGroupName
                            }).ToList();

            return projectGroup.ToList();
        }

        public List<PMSProjectStatusListDetails> GetPMSProjectStatusList()
        {
            List<PMSProjectStatusListDetails> projectStatus = new List<PMSProjectStatusListDetails>();

            var projectStatusList = dbContext.GetPMSProjectStatus_SP();

            projectStatus = (from s in projectStatusList
                             select new PMSProjectStatusListDetails
                             {
                                 PMSProjectStatusID = s.ProjectStatusID,
                                 PMSProjectStatus = s.ProjectStatus
                             }).ToList();

            return projectStatus.ToList();
        }

        public List<PMSProjectCurrencyListDetails> GetPMSProjectCurrencyList()
        {
            List<PMSProjectCurrencyListDetails> projectCurrency = new List<PMSProjectCurrencyListDetails>();

            var projectCurrencyList = dbContext.GetPMSProjectCurrency_SP();

            projectCurrency = (from s in projectCurrencyList
                               select new PMSProjectCurrencyListDetails
                               {
                                   PMSProjectCurrencyID = s.CurrencyID,
                                   PMSProjectCurrency = s.CurrencyName
                               }).ToList();

            return projectCurrency.ToList();
        }

        public List<PMSPracticeListDetails> GetPMSPracticeList()
        {
            List<PMSPracticeListDetails> practice = new List<PMSPracticeListDetails>();

            var practiceList = dbContext.GetPMSProjectPracticeType_SP();

            practice = (from s in practiceList
                        select new PMSPracticeListDetails
                        {
                            PMSPracticeID = s.TypeID,
                            PMSPractice = s.ProjectType
                        }).ToList();

            return practice.ToList();
        }

        public List<PMSLifeCycleListDetails> GetPMSLifeCycleList()
        {
            List<PMSLifeCycleListDetails> lifeCycle = new List<PMSLifeCycleListDetails>();

            var lifeCycleList = dbContext.GetPMSProjectLifeCycle_SP();

            lifeCycle = (from s in lifeCycleList
                         select new PMSLifeCycleListDetails
                         {
                             PMSLifeCycleID = s.ProjectLifeCycleID,
                             PMSLifeCycle = s.ProjectLifeCycle
                         }).ToList();

            return lifeCycle.ToList();
        }

        public List<PMSCommercialDetailsTypeListDetails> GetPMSCommercialDetailsTypeList()
        {
            List<PMSCommercialDetailsTypeListDetails> commercialDetailsType = new List<PMSCommercialDetailsTypeListDetails>();

            var commercialDetailsTypeList = dbContext.GetPMSProjectCommercialDetailsType_SP();

            commercialDetailsType = (from s in commercialDetailsTypeList
                                     select new PMSCommercialDetailsTypeListDetails
                                     {
                                         PMSCommercialDetailsTypeID = s.ContractTypeID,
                                         PMSCommercialDetailsType = s.NodeLabel
                                     }).ToList();

            return commercialDetailsType.ToList();
        }

        public List<PMSBusinessGroupListDetails> GetPMSBusinessGroupList()
        {
            List<PMSBusinessGroupListDetails> businessGroup = new List<PMSBusinessGroupListDetails>();

            var businessGroupList = dbContext.GetPMSBusinessGroups_SP();

            businessGroup = (from s in businessGroupList
                             select new PMSBusinessGroupListDetails
                             {
                                 PMSBusinessGroupID = s.BusinessGroupID,
                                 PMSBusinessGroup = s.BusinessGroup
                             }).ToList();

            return businessGroup.ToList();
        }

        public List<CustomerContact> CustomerContactsRecord(int CustomerID, int page, int rows, out int totalCount)
        {
            List<CustomerContact> CustomerRecords = new List<CustomerContact>();
            SemDAL dal = new SemDAL();
            //var typeofcontactlist = dal.GetTypeOfContactList();
            try
            {
                var customerList = dbContext.GetCustomerContactDetails_SP(CustomerID);

                CustomerRecords = (from s in customerList
                                   //where s.CustomerID == CustomerID
                                   select new CustomerContact
                                   {
                                       ContactPerson = s.ContactPerson,
                                       CustomerIds = s.CustomerContactID,
                                       CustID = s.CustomerID,
                                       EmailID = s.EMailID,
                                       FaxNumber = s.Fax,
                                       MobileNumber = s.Mobile,
                                       OnlineContact = s.OnlineContact,
                                       PhoneNumber = s.Phone,
                                       TypeofContact = s.Position
                                   }).ToList();

                totalCount = CustomerRecords.Count();
                return CustomerRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tbl_PM_Project GetPMSProjectDetails(int? ProjectID)
        {
            try
            {
                var projectDetails = dbContext.GetPMSProjectDetails_SP(ProjectID);

                tbl_PM_Project data = new tbl_PM_Project();

                foreach (var d in projectDetails)
                {
                    data.ProjectCode = d.ProjectCode;
                    data.ProjectName = d.ProjectName;
                    data.ProjectID = d.ProjectID;
                    data.ShortJobTitle = d.ShortJobTitle;
                    data.ProjectGroupID = d.ProjectGroupID;
                    data.Description = d.Description;
                    data.ProjectStatusID = d.ProjectStatusID;
                    data.BillingCurrencyID = d.BillingCurrencyID;
                    data.BusinessGroupID = d.BusinessGroupID;
                    data.ContractType = d.ContractType;
                    data.ProjectTypeID = d.ProjectTypeID;
                    data.ActualStartDate = d.ActualStartDate;
                    data.ActualEndDate = d.ActualEndDate.HasValue ? d.ActualEndDate.Value : d.ExpectedEndDate;
                    data.EstimatedEfforts = d.EstimatedEfforts;
                    data.ExpectedDuration = d.ExpectedDuration;
                    data.CustomerID = d.CustomerID;
                    data.Billable = d.Billable;
                    data.LocationID = d.LocationID;
                    data.ResourcePoolID = d.ResourcePoolID;
                    data.ResourceGroupID = d.ResourceGroupID;
                    data.LifeCycleID = d.LifeCycleID;
                    data.ApprovalStatusID = d.ApprovalStatusID;
                    data.RevisionStatusID = d.RevisionStatusID;
                    data.CreatedBy = d.CreatedBy;
                }
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PMSProjectDetailsViewModel> GetPMSProjectDetailForApproval(int ProjectID)
        {
            var projectDetails = dbContext.GetPMSProjectDetails_SP(ProjectID);
            List<PMSProjectDetailsViewModel> project = (from p in projectDetails
                                                        select new PMSProjectDetailsViewModel
                                                        {
                                                            ProjectID = p.ProjectID,
                                                            PMSProjectWorkHours = p.EstimatedEfforts.ToString(),
                                                            PMSProjectEndDate = p.ActualEndDate,
                                                            ApprovalStaus = p.ApprovalStatusID.HasValue ? p.ApprovalStatusID.Value : 0,
                                                            PMSCommercialDetailsType = p.ContractType,
                                                            PMSBusinessGroup = p.BusinessGroupID,
                                                            PMSOrganizationUnit = p.LocationID,
                                                            PMSDeliveryUnit = p.ResourcePoolID,
                                                            PMSDeliveryTeam = p.ResourceGroupID,
                                                            PMSProjectGroup = p.ProjectGroupID,
                                                            PMSProjectStatusID = p.ProjectStatusID,
                                                            PMSProjectStatus = p.ProjectStatus,
                                                            PMSProjectCurrency = p.BillingCurrencyID,
                                                            PMSPractice = p.ProjectTypeID,
                                                            PMSProjectDurationDays = p.ExpectedDuration,
                                                            PMSCustomer = p.CustomerID,
                                                            PMSLifeCycle = p.LifeCycleID,
                                                            PMSProjectBillableStatus = p.Billable,
                                                            ProjectName = p.ProjectName,
                                                            ApprovalStatusID = p.ApprovalStatusID
                                                        }).ToList();
            return project;
        }

        public bool SaveCustomeContactDetail(CustomerContact CusContantDetails, int CustInvoiceId)
        {
            string CustomerName = CusContantDetails.ContactPerson;
            string ContactPerson = CusContantDetails.ContactPerson;
            string Mobile = CusContantDetails.MobileNumber;
            string EMailID = CusContantDetails.EmailID;
            string OnlineContact = CusContantDetails.OnlineContact;
            string Position = CusContantDetails.TypeofContact;
            string Fax = CusContantDetails.FaxNumber;
            string Phone = CusContantDetails.PhoneNumber;

            int? Customer = 0;
            int Cusid = CustInvoiceId;

            ObjectParameter CustID = new ObjectParameter("CustID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));

            if (CusContantDetails.CustomerIds != 0)
            {
                Customer = CusContantDetails.CustomerIds;
                dbContext.AddUpdateCustomerContacts_SP(Customer, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "UPDATE", Output, CustID);
            }
            else
            {
                dbContext.AddUpdateCustomerContacts_SP(Cusid, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "INSERT", Output, CustID);
            }
            bool status = Convert.ToBoolean(Output.Value);
            return status;
        }

        public List<PMSProjectDetailsViewModel> ProjectDetailRecord(int ddlApprovalStatusID, int ddlProjectlStatusID, string searchText, string loggedInUserEmployeeCode, bool IsProjectApprover, string role, int page, int rows, out int totalCount)
        {
            List<PMSProjectDetailsViewModel> ProjectRecords = new List<PMSProjectDetailsViewModel>();
            try
            {
                var ProjectDetails = dbContext.LoadGridPMSProjectDetails_SP(ddlApprovalStatusID, ddlProjectlStatusID, searchText, loggedInUserEmployeeCode, IsProjectApprover, role);
                ProjectRecords = (from project in ProjectDetails
                                  select new PMSProjectDetailsViewModel
                                  {
                                      ProjectID = project.ProjectID,
                                      ProjectCode = project.ProjectCode,
                                      ProjectName = project.ProjectName,
                                      PMSProjectStartDate = project.ActualStartDate,
                                      PMSProjectEndDate = project.ActualEndDate,
                                      ApprovalStatusID = project.ApprovalStatusID,
                                      RevisionStaus = project.RevisionStatusID.HasValue ? project.RevisionStatusID.Value : 0,
                                      PMSProjectStatus = project.ProjectStatus,
                                      OrganizationUnitName = project.OrganizationUnit,
                                      DeliveryTeamName = project.DeliveryTeam,
                                      DeliveryUnitName = project.DeliveryUnit,
                                      PracticeName = project.Practice,
                                      PMSApprovalStatus = project.ApprovalStatus
                                  }).ToList();

                foreach (var item in ProjectRecords)
                {
                    if (item.RevisionStaus == 4)
                    {
                        item.PMSRevisionStatus = ApprovalStatus.RevisionPendingApproval;
                    }
                    else if (item.RevisionStaus == 5)
                    {
                        item.PMSRevisionStatus = ApprovalStatus.RevisionApproved;
                    }
                    else if (item.RevisionStaus == 6)
                    {
                        item.PMSRevisionStatus = ApprovalStatus.RevisionRejected;
                    }
                }
                totalCount = ProjectRecords.Count();
                return ProjectRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool savePMSProjectDetailsForApproval(PMSProjectDetailsViewModel projectDetail)
        {
            //string WorkHours = projectDetail.PMSProjectWorkHours;
            //DateTime? EndDate = projectDetail.PMSProjectEndDate;

            //ObjectParameter ProjID = new ObjectParameter("ProjID", typeof(int));
            ObjectParameter Result = new ObjectParameter("Result", typeof(int));

            EmployeeDAL EmployeeDal = new EmployeeDAL();
            HRMS_tbl_PM_Employee employeeDetails = EmployeeDal.GetEmployeeDetailsByEmployeeCode(Membership.GetUser().UserName);
            string EmployeeName = null;
            if (employeeDetails != null)
                EmployeeName = employeeDetails.UserName;
            dbContext.AddUpdateProjectDetailsForApproval_SP(EmployeeName, projectDetail.FeildName, projectDetail.ProjectID, projectDetail.NewValue, projectDetail.RevisionStaus, projectDetail.OldValue, Result);

            bool status = Convert.ToBoolean(Result.Value);

            return status;
        }

        public bool SaveQuestionsDetailsForRevision(int? projectId, string stringToBeSaved, string UserName)
        {
            try
            {
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                dbContext.AddUpdateRevisionComments_SP(projectId, stringToBeSaved, UserName, "Add", null, null, Result);
                bool status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveUpdatedRevisionValues(PMSProjectDetailsViewModel projectDetail)
        {
            try
            {
                bool status = false;
                tbl_PM_Project project = dbContext.tbl_PM_Project.Where(x => x.ProjectID == projectDetail.ProjectID).FirstOrDefault();
                if (project != null)
                {
                    project.ActualEndDate = projectDetail.PMSProjectEndDate;
                    project.EstimatedEfforts = Convert.ToDouble(projectDetail.PMSProjectWorkHours);
                    project.RevisionStatusID = projectDetail.RevisionStaus;
                    dbContext.SaveChanges();
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, int?, bool, bool> SavePMSProjectDetail(PMSProjectDetailsViewModel projectDetail)
        {
            string AbbreviatedName = string.Empty;
            string ProjectName = string.Empty;
            string Description = string.Empty;
            int? ProjectID = 0;
            if (!string.IsNullOrEmpty(projectDetail.AbbreviatedName))
                AbbreviatedName = projectDetail.AbbreviatedName.Trim();
            else
                AbbreviatedName = projectDetail.AbbreviatedName;
            string ProjectCode = projectDetail.ProjectCode;
            if (!string.IsNullOrEmpty(projectDetail.ProjectName))
                ProjectName = projectDetail.ProjectName.Trim();
            else
                ProjectName = projectDetail.ProjectName;
            int? ProjectGroupID = projectDetail.PMSProjectGroup;
            if (!string.IsNullOrEmpty(projectDetail.PMSProjectDescription))
                Description = projectDetail.PMSProjectDescription.Trim();
            else
                Description = projectDetail.PMSProjectDescription;
            string ProjectStatus = projectDetail.PMSProjectStatus;
            int? ProjectCurrencyID = projectDetail.PMSProjectCurrency;
            int? CommercialDetailsID = projectDetail.PMSCommercialDetailsType;
            int? PracticeID = projectDetail.PMSPractice;
            int? DurationDays = projectDetail.PMSProjectDurationDays;
            int? BusinessGroupID = projectDetail.PMSBusinessGroup;
            string WorkHours = projectDetail.PMSProjectWorkHours;
            DateTime? StartDate = projectDetail.PMSProjectStartDate;
            DateTime? EndDate = projectDetail.PMSProjectEndDate;
            int? CustomerID = projectDetail.PMSCustomer;
            int? OrganizationUnitID = projectDetail.PMSOrganizationUnit;
            int? LifeCycleID = projectDetail.PMSLifeCycle;
            int? DeliveryUnitID = projectDetail.PMSDeliveryUnit;
            bool Billable = projectDetail.PMSProjectBillableStatus;
            int? DeliveryTeamID = projectDetail.PMSDeliveryTeam;
            int? userId = projectDetail.userId;
            string loggedInUserEmployeeCode = projectDetail.loggedInUserEmployeeCode;
            bool status;
            bool resetStatus = false;
            bool isProjectNameExist = false;
            List<PMSProjectDetailsViewModel> projectNameExist = new List<PMSProjectDetailsViewModel>();
            List<PMSProjectDetailsViewModel> projectCodeExist = new List<PMSProjectDetailsViewModel>();

            var project = dbContext.GetAllPMSProjectDetails_SP();
            if (project != null)
            {
                projectNameExist = (from a in project
                                    where a.ProjectName == projectDetail.ProjectName
                                    select new PMSProjectDetailsViewModel
                                      {
                                          ProjectName = a.ProjectName
                                      }).ToList();
            }

            ObjectParameter ProjID = new ObjectParameter("ProjID", typeof(int));
            ObjectParameter Result = new ObjectParameter("Result", typeof(int));
            ObjectParameter NewProjectCode = new ObjectParameter("GeneratedProjectCode", typeof(string));
            ObjectParameter ResetResult = new ObjectParameter("ResetResult", typeof(int));

            if (projectDetail.ProjectID == 0)
            {
                dbContext.GenerateProjectCode_SP(projectDetail.PMSBusinessGroup, projectDetail.PMSOrganizationUnit, NewProjectCode);
                ProjectCode = Convert.ToString(NewProjectCode.Value);

                var projectDetails = dbContext.GetAllPMSProjectDetails_SP();
                projectCodeExist = (from code in projectDetails
                                    where code.ProjectCode == ProjectCode
                                    select new PMSProjectDetailsViewModel
                                    {
                                        ProjectCode = code.ProjectCode
                                    }).ToList();
                if (projectCodeExist.Count > 0)
                {
                    dbContext.GenerateProjectCode_SP(projectDetail.PMSBusinessGroup, projectDetail.PMSOrganizationUnit, NewProjectCode);
                    ProjectCode = Convert.ToString(NewProjectCode.Value);
                }
            }
            if (projectDetail.ProjectID != 0 && projectDetail.ProjectID != null)
            {
                ProjectID = projectDetail.ProjectID;

                dbContext.AddUpdateProjectDetails_SP(ProjectID, AbbreviatedName, ProjectCode, ProjectName, ProjectGroupID, Description, ProjectStatus, ProjectCurrencyID, CommercialDetailsID,
                    PracticeID, StartDate, EndDate, WorkHours, DurationDays, BusinessGroupID, CustomerID, OrganizationUnitID, LifeCycleID, DeliveryUnitID, Billable, DeliveryTeamID, "UPDATE", loggedInUserEmployeeCode, Result, ProjID);
            }
            else if (projectDetail.ProjectID == 0 && projectNameExist.Count == 0)
            {
                dbContext.AddUpdateProjectDetails_SP(ProjectID, AbbreviatedName, ProjectCode, ProjectName, ProjectGroupID, Description, ProjectStatus, ProjectCurrencyID, CommercialDetailsID,
                   PracticeID, StartDate, EndDate, WorkHours, DurationDays, BusinessGroupID, CustomerID, OrganizationUnitID, LifeCycleID, DeliveryUnitID, Billable, DeliveryTeamID, "INSERT", loggedInUserEmployeeCode, Result, ProjID);
            }
            else
            {
                status = false;
                ProjectID = projectDetail.ProjectID;
                isProjectNameExist = true;
                return new Tuple<bool, int?, bool, bool>(status, ProjectID, isProjectNameExist, resetStatus);
            }

            status = Convert.ToBoolean(Result.Value);
            int projectID = 0;
            if (projectDetail.ProjectID == 0)
            {
                projectID = Convert.ToInt32(ProjID.Value);
            }
            else
            {
                projectID = Convert.ToInt32(projectDetail.ProjectID);
            }
            if (status == true && projectDetail.PMSProjectEndDate > projectDetail.OriginalDateTime)
            {
                dbContext.ResetResourceAllocationFromHistory_SP(projectDetail.ProjectID, projectDetail.OriginalDateTime, projectDetail.PMSProjectEndDate, ResetResult);
                resetStatus = Convert.ToBoolean(Result.Value);
            }
            return new Tuple<bool, int?, bool, bool>(status, projectID, isProjectNameExist, resetStatus);
        }

        public Tuple<bool, int?, bool> SavePMSProjectDetailForRevision(PMSProjectDetailsViewModel projectDetail, int? approvalStatusId)
        {
            int? ProjectID = 0;
            string AbbreviatedName = projectDetail.AbbreviatedName;
            string ProjectCode = projectDetail.ProjectCode;
            string ProjectName = projectDetail.ProjectName;
            int? ProjectGroupID = projectDetail.PMSProjectGroup;
            string Description = projectDetail.PMSProjectDescription;
            string ProjectStatus = projectDetail.PMSProjectStatus;
            int? ProjectCurrencyID = projectDetail.PMSProjectCurrency;
            int? CommercialDetailsID = projectDetail.PMSCommercialDetailsType;
            int? PracticeID = projectDetail.PMSPractice;
            int? DurationDays = projectDetail.PMSProjectDurationDays;
            int? BusinessGroupID = projectDetail.PMSBusinessGroup;
            string WorkHours = projectDetail.PMSProjectWorkHours;
            DateTime? StartDate = projectDetail.PMSProjectStartDate;
            DateTime? EndDate = projectDetail.PMSProjectEndDate;
            int? CustomerID = projectDetail.PMSCustomer;
            int? OrganizationUnitID = projectDetail.PMSOrganizationUnit;
            int? LifeCycleID = projectDetail.PMSLifeCycle;
            int? DeliveryUnitID = projectDetail.PMSDeliveryUnit;
            bool Billable = projectDetail.PMSProjectBillableStatus;
            int? DeliveryTeamID = projectDetail.PMSDeliveryTeam;
            int? userId = projectDetail.userId;
            string loggedInUserEmployeeCode = projectDetail.loggedInUserEmployeeCode;
            bool status;
            bool isProjectNameExist = false;
            List<PMSProjectDetailsViewModel> projectNameExist = new List<PMSProjectDetailsViewModel>();
            if (approvalStatusId != 2)
            {
                var project = dbContext.GetAllPMSProjectDetails_SP();
                if (project != null)
                {
                    projectNameExist = (from a in project
                                        where a.ProjectName == projectDetail.ProjectName
                                        select new PMSProjectDetailsViewModel
                                        {
                                            ProjectName = a.ProjectName
                                        }).ToList();
                }
            }
            ObjectParameter ProjID = new ObjectParameter("ProjID", typeof(int));
            ObjectParameter Result = new ObjectParameter("Result", typeof(int));

            if (projectDetail.ProjectID != 0 && projectDetail.ProjectID != null)
            {
                ProjectID = projectDetail.ProjectID;

                dbContext.AddUpdateProjectDetails_SP(ProjectID, AbbreviatedName, ProjectCode, ProjectName, ProjectGroupID, Description, ProjectStatus, ProjectCurrencyID, CommercialDetailsID,
                    PracticeID, StartDate, EndDate, WorkHours, DurationDays, BusinessGroupID, CustomerID, OrganizationUnitID, LifeCycleID, DeliveryUnitID, Billable, DeliveryTeamID, "UPDATE", loggedInUserEmployeeCode, Result, ProjID);
            }
            else if (projectDetail.ProjectID == 0 && projectNameExist.Count == 0)
            {
                dbContext.AddUpdateProjectDetails_SP(ProjectID, AbbreviatedName, ProjectCode, ProjectName, ProjectGroupID, Description, ProjectStatus, ProjectCurrencyID, CommercialDetailsID,
                   PracticeID, StartDate, EndDate, WorkHours, DurationDays, BusinessGroupID, CustomerID, OrganizationUnitID, LifeCycleID, DeliveryUnitID, Billable, DeliveryTeamID, "INSERT", loggedInUserEmployeeCode, Result, ProjID);
            }
            else
            {
                status = false;
                ProjectID = projectDetail.ProjectID;
                isProjectNameExist = true;
                return new Tuple<bool, int?, bool>(status, ProjectID, isProjectNameExist);
            }

            status = Convert.ToBoolean(Result.Value);
            int projectID = 0;
            if (projectDetail.ProjectID == 0)
            {
                projectID = Convert.ToInt32(ProjID.Value);
            }
            else
            {
                projectID = Convert.ToInt32(projectDetail.ProjectID);
            }

            return new Tuple<bool, int?, bool>(status, projectID, isProjectNameExist);
        }

        public bool DeleteCustomerDetails(int CustomerID)
        {
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            dbContext.DeleteCustomerDetails_SP(CustomerID, Output);
            bool status = Convert.ToBoolean(Output.Value);
            return status;
        }

        public bool DeleteCustomerConDetails(int CustomerID)
        {
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            dbContext.DeleteCustomerContactDetails_SP(CustomerID, Output);
            bool status = Convert.ToBoolean(Output.Value);
            return status;
        }

        //public List<PMSProjectDetailsViewModel> GetProjectNamesListByApprovalStatus(int? approvalStatusID)
        //{
        //    approvalStatusID = 2;
        //    List<PMSProjectDetailsViewModel> projectNames = new List<PMSProjectDetailsViewModel>();
        //    try
        //    {
        //        projectNames = (from project in dbContext.tbl_PM_Project
        //                        where project.ApprovalStatusID == approvalStatusID
        //                        select new PMSProjectDetailsViewModel
        //                        {
        //                            ProjectID = project.ProjectID,
        //                            ProjectName = project.ProjectName,
        //                        }).ToList();

        //        return projectNames.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool SendForApprovalPMSProjectDetails(int projectId, int LoggedInEmployeeId)
        {
            ObjectParameter Result = new ObjectParameter("Result", typeof(int));

            dbContext.ProjectCreationSendForApproval_sp(projectId, "SendForApproval", null, LoggedInEmployeeId, null, Result);
            bool status = Convert.ToBoolean(Result.Value);
            return status;
        }

        public bool ApproveRejectPMSProjectDetail(int projectId, string btnClick, string userName, int LoggedInEmployeeId, string RejectedComments)
        {
            ObjectParameter Result = new ObjectParameter("Result", typeof(int));
            string isClicked = string.Empty;
            if (btnClick == "Approve")
                isClicked = "Approve";
            else
                isClicked = "Reject";

            dbContext.ProjectCreationSendForApproval_sp(projectId, isClicked, userName, LoggedInEmployeeId, RejectedComments, Result);
            bool status = Convert.ToBoolean(Result.Value);
            return status;
        }

        #region Customer Contract Details

        public CustomerContract GetCustomerContractDetails(int ContractID)
        {
            try
            {
                var ContractMaster = dbContext.GetCustomerContractDetails_sp();
                CustomerContract _ContractMaster = (from contract in ContractMaster
                                                    where contract.ContractID == ContractID
                                                    select new CustomerContract
                                                    {
                                                        ContractID = contract.ContractID,
                                                        ContractType = contract.ContractTypeID,
                                                        ContractSummary = contract.ContractSummary,
                                                        ContractDetails = contract.InvoiceingMilestones,
                                                        CommencementDate = contract.ContractStartDate,
                                                        ContractValidityDate = contract.ContractEndDate,
                                                        ContractSigningDate = contract.ContractSummaryDate
                                                    }).FirstOrDefault();
                return _ContractMaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ContractTypes> GetContractTypeList()
        {
            try
            {
                var ContractTypeList = dbContext.GetCustomerContractType_sp();
                List<ContractTypes> _ContractTypeList = (from contractType in ContractTypeList
                                                         select new ContractTypes
                                                         {
                                                             ContractTypeID = contractType.ContractTypeID,
                                                             ContractTypeName = contractType.ContractTypeName
                                                         }).ToList();
                return _ContractTypeList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public v_tbl_PM_ContractMaster GetSelectedCustomerDetails(int? Customerid)
        {
            try
            {
                v_tbl_PM_ContractMaster CustDetails = dbContext.v_tbl_PM_ContractMaster.Where(ed => ed.CustomerID == Customerid).FirstOrDefault();
                return CustDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ContractFileDetails> CustomerContractFileRecord(int ContractID, int page, int rows, out int totalCount)
        {
            List<ContractFileDetails> ContractFileRecords = new List<ContractFileDetails>();
            var ContractFilesDetails = dbContext.GetCustomerContractFileDetails_sp();
            try
            {
                ContractFileRecords = (from contract in ContractFilesDetails
                                       where contract.ContractID == ContractID
                                       select new ContractFileDetails
                                       {
                                           ContractID = ContractID,
                                           ContractAttachmentID = contract.ContractAttachmentID,
                                           FileName = contract.OriginalFileName,
                                           FileUpload = null,
                                           Description = contract.Description,
                                           AttachedBy = contract.AttachedBy,
                                           AttachedDate = contract.AttachedOn,
                                           IsFileExists = null
                                       }).ToList();

                for (int i = 0; i < ContractFileRecords.Count; i++)
                {
                    string[] FileExtention = ContractFileRecords[i].FileName.Split('.');
                    string contentType = "application/" + FileExtention[1];
                    string uploadsPath = (System.Configuration.ConfigurationManager.AppSettings["UploadContractFileLocation"]);
                    uploadsPath = Path.Combine(uploadsPath, (ContractFileRecords[i].ContractID).ToString());
                    string Filepath = Path.Combine(uploadsPath, ContractFileRecords[i].FileName);

                    if (!File.Exists(Filepath))
                    {
                        ContractFileRecords[i].IsFileExists = false;
                    }
                    else
                    {
                        ContractFileRecords[i].IsFileExists = true;
                    }
                }

                totalCount = ContractFileRecords.Count();
                //return ContractFileRecords.Skip((page - 1) * rows).Take(rows).ToList();
                return ContractFileRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerContract> CustomerContractDetailRecord(int? CustomerID, int page, int rows, out int totalCount)
        {
            List<CustomerContract> CustomerContractRecords = new List<CustomerContract>();
            try
            {
                var contractDetails = dbContext.v_tbl_PM_ContractMaster;
                CustomerContractRecords = (from contract in contractDetails
                                           where contract.CustomerID == CustomerID
                                           orderby contract.ContractSummaryDate descending
                                           select new CustomerContract
                                           {
                                               CustomerID = CustomerID,
                                               ContractID = contract.ContractID,
                                               CustomerName = contract.CustomerName,
                                               ContractType = contract.ContractTypeID,
                                               ContractTypeName = contract.ContractTypeName,
                                               ContractSummary = contract.ContractSummary,
                                               ContractDetails = contract.InvoiceingMilestones,
                                               CommencementDate = contract.ContractStartDate,
                                               ContractValidityDate = contract.ContractEndDate,
                                               ContractSigningDate = contract.ContractSummaryDate
                                           }).ToList();

                totalCount = CustomerContractRecords.Count();
                return CustomerContractRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SEMResponse SaveCustomerContractDetails(CustomerContract contractDetails)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                int? ContractID = contractDetails.ContractID;
                int CustomerID = Convert.ToInt32(contractDetails.CustomerID);
                int ContractTypeID = Convert.ToInt32(contractDetails.ContractType);
                string ContractSummary = contractDetails.ContractSummary;
                string ContractDetails = contractDetails.ContractDetails;
                DateTime? CommencementDate = contractDetails.CommencementDate;
                DateTime? ContractSigningDate = contractDetails.ContractSigningDate;
                DateTime? ContractValidityDate = contractDetails.ContractValidityDate;
                string CreatedBy = contractDetails.UserName;
                DateTime CreatedDate = DateTime.Now;
                string Operation = "";
                if (contractDetails.ContractID == 0)
                    Operation = "INSERT";
                else
                    Operation = "UPDATE";

                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                ObjectParameter InsertedContractID = new ObjectParameter("InsertedContractID", typeof(int));
                dbContext.AddUpdateCustomerContractDetails_sp(ContractID, CustomerID, ContractTypeID, ContractSummary, ContractDetails, CommencementDate, ContractValidityDate, ContractSigningDate, CreatedBy, CreatedDate, Operation, Output, InsertedContractID);
                response.status = Convert.ToBoolean(Output.Value);
                if (Operation == "INSERT")
                {
                    response.nextContractID = Convert.ToInt32(InsertedContractID.Value);
                }
                else
                {
                    response.nextContractID = contractDetails.ContractID;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteCustomerContractRecord(int CustomerContractID)
        {
            try
            {
                bool status = false;
                int? ContractID = CustomerContractID;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                dbContext.DeleteCustomerContractDetails_sp(ContractID, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteCustomerContractFileRecord(int ContractAttachmentId)
        {
            try
            {
                bool status = false;
                int ContractAttachmentID = ContractAttachmentId;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                dbContext.DeleteCustomerContractFileDetails_sp(ContractAttachmentID, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteProjectReviewerDetails(string[] SelectedProjectReviewerId)
        {
            try
            {
                bool status = false;
                //int ProjReviewerID = ProjectReviewerId;
                //ObjectParameter Output = new ObjectParameter("Result", typeof(string));

                //dbContext.DeleteProjectReviewerDetails_sp(ProjReviewerID, Output);
                //if (Output.Value.ToString() == "Success")
                //    return status = true;
                //else
                //    return status = false;

                for (int i = 0; i < SelectedProjectReviewerId.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(string));
                    //int ModuleID = Convert.ToInt32(SelectedModuleId[i]);
                    int ProjReviewerID = Convert.ToInt32(SelectedProjectReviewerId[i]);
                    dbContext.DeleteProjectReviewerDetails_sp(ProjReviewerID, Output);
                    if (Output.Value.ToString() == "Success")
                        status = true;
                    else
                        status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveContractFileDetails(ContractFileDetails model)
        {
            try
            {
                var ContractFiles = dbContext.GetCustomerContractFileDetails_sp();
                ContractFileDetails ContractFileDetails = (from file in ContractFiles
                                                           where file.ContractAttachmentID == model.ContractAttachmentID
                                                           select new ContractFileDetails
                                                           {
                                                               ContractAttachmentID = file.ContractAttachmentID,
                                                               FileName = file.OriginalFileName
                                                           }).FirstOrDefault();

                int ContractAttachmentID = model.ContractAttachmentID;
                int ContractID = model.ContractID;
                string AttachedBy = model.EmployeeName;
                DateTime AttachedDate = DateTime.Now;
                string Description = "";
                if (!string.IsNullOrEmpty(model.Description))
                    Description = model.Description.Trim();
                else
                    Description = model.Description;
                string FileName = "";
                if (model.FileName == "" || model.FileName == null)
                    FileName = ContractFileDetails.FileName;
                else
                    FileName = model.FileName;
                string Operation = "";
                if (ContractFileDetails != null)
                    Operation = "UPDATE";
                else
                    Operation = "INSERT";

                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                dbContext.AddUpdateContractFileDetails_sp(ContractAttachmentID, ContractID, AttachedBy, AttachedDate, Description, FileName, Operation, Output);

                bool status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SearchedUserDetails GetEmployeeDetails(int EmployeeID)
        {
            string userName = string.Empty;
            var employeeDetailsList = dbContextHRMS.GetEmployeeDetailsHRMS_sp(EmployeeID, userName);
            SearchedUserDetails employeeDetail = (from e in employeeDetailsList.ToList()
                                                  where e.EmployeeID == EmployeeID
                                                  select new SearchedUserDetails
                                                  {
                                                      EmployeeFullName = e.EmployeeName,
                                                      EmployeeId = EmployeeID,
                                                      EmployeeCode = e.EmployeeCode,
                                                      UserName = e.USERNAME
                                                  }).FirstOrDefault();
            return employeeDetail;
        }

        #endregion Customer Contract Details

        public List<ProjectAppList> GetAPProvedProjectList()
        {
            List<ProjectAppList> projectNames = new List<ProjectAppList>();

            var projectList = dbContext.GetApprovedProjectList_SP();

            projectNames = (from s in projectList
                            orderby s.ProjectName ascending
                            select new ProjectAppList
                            {
                                Projectids = s.ApprovalStatusID,
                                ProjectName = s.ProjectName,
                                projectIdList = s.ProjectID
                            }).ToList();

            return projectNames.ToList();
        }

        public List<ManagePhasesModel> GetManagePhaseSelectedProJectDetails(int projectID, string searchText, int page, int rows, out int totalCount)
        {
            List<ManagePhasesModel> ProjectRecords = new List<ManagePhasesModel>();

            try
            {
                var customerList = dbContext.GetManagePhaseDetails_SP(projectID);
                var ResposibleList = dbContext.GetResponsiblePersonPhase_SP(projectID, 0);

                var EmployeeList = (from s in ResposibleList
                                    select new ManagePhasesModel
                                    {
                                        ResponsiblePerson = s.EmployeeID,
                                        ResponsiblePersonGridName = s.EmployeeName
                                    }).ToList();

                ProjectRecords = (from s in customerList
                                  orderby s.ordernumber ascending
                                  select new ManagePhasesModel
                                  {
                                      ProjectPhaseId = s.ProjectPhaseID,
                                      ProjectId = s.ProjectID,
                                      OrderNumber = s.ordernumber,
                                      Phases = s.Phase,
                                      StartDate = s.EstimatedStartDate,
                                      EndDate = s.EstimatedEndDate,
                                      WorkHours = s.EstimatedEfforts,
                                      PeakTeamSize = s.PlannedResources,
                                      ResponsiblePersonGridName = (from relation in EmployeeList
                                                                   where relation.ResponsiblePerson == s.ResponsiblePerson
                                                                   select relation.ResponsiblePersonGridName).FirstOrDefault(),
                                      Currentphase = s.CurrentPhase,
                                      PercentageEfforts = s.PercentEfforts
                                  }).ToList();

                totalCount = ProjectRecords.Count();
                return ProjectRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tbl_PM_Project GetProjectDetails(int? projectId)
        {
            try
            {
                tbl_PM_Project CustDetails = dbContext.tbl_PM_Project.Where(ed => ed.ProjectID == projectId).FirstOrDefault();
                return CustDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_PM_AuditTrail getAudioTrailDetails(int projectId, int revisionStatusId)
        {
            try
            {
                tbl_PM_AuditTrail trailDetails = dbContext.tbl_PM_AuditTrail.Where(x => x.ProjectID == projectId && x.RevisionStatusID == revisionStatusId).OrderByDescending(x => x.LogID).FirstOrDefault();
                return trailDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetBusinessDays(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                int days;

                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                dbContext.GetBusinessDaysCount_SP(StartDate, EndDate, Result);
                days = Convert.ToInt32(Result.Value);

                return days;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ManagePhasesModel GetManagePhaseProJectDetails(int ProjectPhaseId, int PhaseID)
        {
            List<ManagePhasesModel> ProjectRecords = new List<ManagePhasesModel>();
            ManagePhasesModel SinglphaseRecord = new ManagePhasesModel();
            try
            {
                var customerList = dbContext.GetManagePhaseDetailsFoProject_SP(ProjectPhaseId, PhaseID);

                ProjectRecords = (from s in customerList
                                  orderby s.ordernumber descending
                                  select new ManagePhasesModel
                                  {
                                      ProjectPhaseId = s.ProjectPhaseID,
                                      ProjectId = s.ProjectID,
                                      OrderNumber = s.ordernumber,
                                      Phases = s.Phase,
                                      StartDate = s.EstimatedStartDate,
                                      EndDate = s.EstimatedEndDate,
                                      WorkHours = s.EstimatedEfforts,
                                      PeakTeamSize = s.PlannedResources,
                                      ResponsiblePerson = s.ResponsiblePerson,
                                      Currentphase = s.CurrentPhase,
                                      PercentageEfforts = s.PercentEfforts
                                  }).ToList();

                foreach (var item in ProjectRecords)
                {
                    SinglphaseRecord.Phases = item.Phases;
                    SinglphaseRecord.ProjectPhaseId = item.ProjectPhaseId;
                    SinglphaseRecord.ProjectId = item.ProjectId;
                    SinglphaseRecord.OrderNumber = item.OrderNumber;
                    SinglphaseRecord.StartDate = item.StartDate;
                    SinglphaseRecord.EndDate = item.EndDate;
                    SinglphaseRecord.WorkHours = item.WorkHours;
                    SinglphaseRecord.PeakTeamSize = item.PeakTeamSize;
                    SinglphaseRecord.ResponsiblePerson = item.ResponsiblePerson;
                    SinglphaseRecord.Currentphase = item.Currentphase;
                    SinglphaseRecord.PercentageEfforts = item.PercentageEfforts;
                }
                return SinglphaseRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ResponsiblePesons> ResponsiblrPersonsList(int PhaseID, int ProjectPhaseId)
        {
            List<ResponsiblePesons> projectNames = new List<ResponsiblePesons>();

            var projectList = dbContext.GetResponsiblePersonPhase_SP(PhaseID, ProjectPhaseId);

            projectNames = (from s in projectList
                            select new ResponsiblePesons
                            {
                                PersoneID = s.EmployeeID,
                                PersonName = s.EmployeeName
                            }).ToList();

            return projectNames.ToList();
        }

        //public tbl_PM_Employee GetEmployeeDetailsByUserName(string userName)
        //{
        //    //var employeeDetails = dbContext.GetEmployeeDetailsByUserName_sp(userName);
        //    return employeeDetails;
        //}

        #region Manage Modules

        public List<ProjectAppList> GetLoggedUserProjectList(string UserName, string TextLink, int employeeCode)
        {
            if (TextLink == "IRApproval")
            {
                var approvedProjectListsIR = dbContext.GetApproverList_SP(TextLink, employeeCode);
                List<ProjectAppList> IRApproverList = (from project in approvedProjectListsIR
                                                       orderby project.ProjectName ascending
                                                       select new ProjectAppList
                                                       {
                                                           Projectids = project.ProjectId,
                                                           ProjectName = project.ProjectName
                                                       }).ToList();
                return IRApproverList;
            }
            else if (TextLink == "FinanceApproval")
            {
                var approvedProjectListsFinance = dbContext.GetApproverList_SP(TextLink, employeeCode);
                List<ProjectAppList> FinanceApproverList = (from project in approvedProjectListsFinance
                                                            orderby project.ProjectName ascending
                                                            select new ProjectAppList
                                                            {
                                                                Projectids = project.ProjectId,
                                                                ProjectName = project.ProjectName
                                                            }).ToList();
                return FinanceApproverList;
            }
            else if (TextLink == "ActiveProjects")
            {
                if (HttpContext.Current.User.IsInRole("RMG") || HttpContext.Current.User.IsInRole("PMO"))
                {
                    var approvedProjectListsFinance = dbContext.GetProjectListForRMG_SP(employeeCode);
                    List<ProjectAppList> FinanceApproverList = (from project in approvedProjectListsFinance
                                                                orderby project.projectName ascending
                                                                select new ProjectAppList
                                                                {
                                                                    Projectids = project.ProjectID,
                                                                    ProjectName = project.projectName.Trim()
                                                                }).ToList();
                    return FinanceApproverList;
                }
                else
                {
                    var approvedProjectListsFinance = dbContext.GetActiveProjectList_SP(employeeCode);
                    List<ProjectAppList> FinanceApproverList = (from project in approvedProjectListsFinance
                                                                orderby project.projectName ascending
                                                                select new ProjectAppList
                                                                {
                                                                    Projectids = project.projectId,
                                                                    ProjectName = project.projectName
                                                                }).ToList();
                    return FinanceApproverList;
                }
            }
            else
            {
                //var approvedProjectLists = dbContext.GetApprovedProjectList_SP();
                //List<ProjectAppList> loggedUserProjectList = (from project in approvedProjectLists
                //                                              orderby project.ProjectName ascending
                //                                              where project.CreatedBy == UserName
                //                                              select new ProjectAppList
                //                                              {
                //                                                  Projectids = project.ProjectID,
                //                                                  ProjectName = project.ProjectName
                //                                              }).ToList();
                var approvedProjectLists = dbContext.GetActiveProjectList_SP(employeeCode);
                List<ProjectAppList> loggedUserProjectList = (from project in approvedProjectLists
                                                              orderby project.projectName ascending
                                                              select new ProjectAppList
                                                              {
                                                                  Projectids = project.projectId,
                                                                  ProjectName = project.projectName
                                                              }).ToList();

                return loggedUserProjectList;
            }
        }

        public List<ModuleComplexityList> GetModuleComplexityList()
        {
            var complexityLists = dbContext.usp_Sel_Module_Complexity();
            List<ModuleComplexityList> moduleComplexityList = (from complexity in complexityLists
                                                               select new ModuleComplexityList
                                                               {
                                                                   ComplexityID = complexity.ComplexityId,
                                                                   ComplexityName = complexity.Complexity
                                                               }).ToList();
            return moduleComplexityList;
        }

        public List<AddManageModules> ProjectModuleDetailRecord(int projectID, int page, int rows, out int totalCount)
        {
            try
            {
                List<AddManageModules> moduleRecords = new List<AddManageModules>();
                //var customerList = dbContext.GetManagePhaseDetails_SP(projectID);
                var moduleDetails = dbContext.GetProjectModuleDetails_sp();

                moduleRecords = (from module in moduleDetails
                                 where module.ProjectID == projectID
                                 select new AddManageModules
                                 {
                                     ModuleID = module.ModuleID,
                                     ProjectID = module.ProjectID,
                                     ModuleName = module.ModuleName,
                                     ModuleDescription = module.ModuleDescription,
                                     ModuleStartDate = module.ActualStartDate,
                                     ModuleEndDate = module.ActualEndDate,
                                     Complexity = module.ComplexityName,
                                     WorkHours = module.EstimatedEfforts,
                                     HiddenComplexityID = module.ComplexityID
                                 }).ToList();

                totalCount = moduleRecords.Count();
                //return moduleRecords.Skip((page - 1) * rows).Take(rows).ToList();
                return moduleRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SEMResponse SaveProjectModuleRecord(AddManageModules model, int ComplexityID, string LoggedUserName, int ProjectId, string SelectedModuleName)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                response.isModuleNameExist = false;
                response.status = false;
                var moduleRecords = dbContext.GetProjectModuleDetails_sp();
                var moduleDetails = (from m in moduleRecords
                                     where m.ModuleName.ToLower() == model.ModuleName.ToLower() && m.ProjectID == model.ProjectID
                                     select new AddManageModules
                                     {
                                         ModuleName = m.ModuleName,
                                         ProjectID = m.ProjectID
                                     }).ToList();
                if (moduleDetails.Count == 0 || (model.ModuleName == SelectedModuleName && model.ModuleID > 0))
                {
                    int ModuleID = model.ModuleID;
                    int ProjectID = ProjectId;
                    string ModuleName = model.ModuleName;
                    string ModuleDescription = model.ModuleDescription;
                    DateTime? ModuleStartDate = model.ModuleStartDate;
                    DateTime? ModuleEndDate = model.ModuleEndDate;
                    int? Complexity = ComplexityID;
                    double? WorkHours = model.WorkHours;
                    DateTime CreatedDate = DateTime.Now;

                    string Operation = "";
                    if (model.ModuleID == 0)
                        Operation = "INSERT";
                    else
                        Operation = "UPDATE";

                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                    dbContext.AddUpdateProjectModuleDetails_sp(ModuleID, ProjectID, ModuleName, ModuleDescription, ModuleStartDate, ModuleEndDate, Complexity, WorkHours, CreatedDate, LoggedUserName, Operation, Output);
                    response.status = Convert.ToBoolean(Output.Value);
                    return response;
                }
                else
                {
                    response.isModuleNameExist = true;
                    return response;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddManageModules GetSelectedProjectRecord(int ProjectID)
        {
            var projectDetails = dbContext.GetApprovedProjectList_SP();
            AddManageModules projectReocrd = (from p in projectDetails
                                              where p.ProjectID == ProjectID
                                              select new AddManageModules
                                              {
                                                  ProjectStartDate = p.ActualStartDate,
                                                  ProjectEndDate = p.ActualEndDate
                                              }).FirstOrDefault();
            return projectReocrd;
        }

        public bool DeleteModuleRecord(string[] SelectedModuleId)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedModuleId.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    int ModuleID = Convert.ToInt32(SelectedModuleId[i]);
                    dbContext.DeleteProjectModuleDetails_sp(ModuleID, Output);
                    status = Convert.ToBoolean(Output.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Manage Modules

        public List<PhasesViewModel> GetResourceDetailsProJectDetails(int? PhaseID, int page, int rows, out int totalCount)
        {
            List<PhasesViewModel> ProjectRecords = new List<PhasesViewModel>();

            try
            {
                var customerList = dbContext.GetResourceDetailsForPhaseDetails(PhaseID);

                ProjectRecords = (from s in customerList
                                  select new PhasesViewModel
                                  {
                                      ResponsiblePerson = s.Resource,
                                      StartDate = s.Start_Date,
                                      EndDate = s.End_Date,
                                      WorkHours = s.Work_Hrs_,
                                      ActualHrs = s.Actual_Work_Hrs_
                                  }).ToList();

                totalCount = ProjectRecords.Count();
                return ProjectRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsByUserName(string userName)
        {
            var employeeDetails = dbContext.GetEmployeeDetailsByUserName_sp(userName);
            HRMS_tbl_PM_Employee empDetails = new HRMS_tbl_PM_Employee();
            foreach (var item in employeeDetails)
            {
                empDetails.UserName = item.UserName;
                empDetails.EmployeeID = item.EmployeeID;
                empDetails.EmailID = item.EmailID;
            }
            return empDetails;
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsByEmployeeCode(string EmployeeCode)
        {
            var employeeDetails = dbContext.GetEmployeeDetailsByEmployeeCode_sp(EmployeeCode);
            HRMS_tbl_PM_Employee empDetails = new HRMS_tbl_PM_Employee();
            foreach (var item in employeeDetails)
            {
                empDetails.UserName = item.UserName;
                empDetails.EmployeeID = item.EmployeeID;
                empDetails.EmailID = item.EmailID;
                empDetails.EmployeeName = item.EmployeeName;
            }
            return empDetails;
        }

        //public tbl_PM_Employee GetEmployeeIDFromEmployeeCode(string EmployeeCode)
        //{
        //    try
        //    {
        //        tbl_PM_Employee employeedetails = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == EmployeeCode).FirstOrDefault();
        //        return employeedetails;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public List<PhaseManagementDetails> PhaseManagementDetails(int page, int rows, out int totalCount)
        {
            List<PhaseManagementDetails> PhaseRecords = new List<PhaseManagementDetails>();
            try
            {
                var phaseList = dbContext.GetPhaseManagementData_sp();
                PhaseRecords = (from s in phaseList
                                select new PhaseManagementDetails
                                {
                                    PhaseID = s.PhaseID,
                                    PhaseDescription = s.Phase
                                }).ToList();
                totalCount = PhaseRecords.Count();
                return PhaseRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletePhaseDescription(string[] PhaseID)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < PhaseID.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    int ConvertedPhaseID = Convert.ToInt32(PhaseID[i]);
                    dbContext.DeletePhaseManagementData_sp(ConvertedPhaseID, Output);
                    status = Convert.ToBoolean(Output.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<bool, bool> UpdatePhaseManagementData(PhaseManagementDetails model, string oldPhaseDescription)
        {
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            bool status = false;
            bool IsExists = false;
            if (model.PhaseID != null)
            {
                int PhaseID = model.PhaseID;
                string PhaseDescription = model.PhaseDescription;
                var phasedetails = dbContext.GetPhaseManagementData_sp();
                List<PhaseManagementDetails> IsDescriptionExists = (from p in phasedetails
                                                                    where p.Phase == model.PhaseDescription
                                                                    select new PhaseManagementDetails
                                                                    {
                                                                        PhaseDescription = p.Phase
                                                                    }
                                                                      ).ToList();
                if (IsDescriptionExists.Count == 0 || model.PhaseDescription == oldPhaseDescription)
                {
                    dbContext.UpdatePhaseManagementData_sp(PhaseID, PhaseDescription, Output);
                    status = Convert.ToBoolean(Output.Value);
                    IsExists = false;
                }
                else
                {
                    status = false;
                    IsExists = true;
                }
            }
            return new Tuple<bool, bool>(status, IsExists);
        }

        public List<PMSProjectDetailsViewModel> GetProjectHistoryDetails(int projectId, string btnClick, int page, int rows, out int totalCount)
        {
            List<PMSProjectDetailsViewModel> ProjectRecords = new List<PMSProjectDetailsViewModel>();

            try
            {
                var historyList = dbContext.GetProjectHistoryDetails_sp(projectId, btnClick);

                ProjectRecords = (from item in historyList
                                  select new PMSProjectDetailsViewModel
                                  {
                                      ProjectID = item.projectid,
                                      AuditId = item.logid,
                                      FieldName = item.fieldname,
                                      OldValueProjectHistory = item.oldvalue,
                                      NewValueProjectHistory = item.value,
                                      ApproverDescription = item.Comments,
                                      ApprovalStatus = item.approvalstatus,
                                      ApprovedBy = item.employeename,
                                      ApprovedOn = item.date
                                  }).ToList();

                totalCount = ProjectRecords.Count();
                return ProjectRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IBPhaseList> IBManagesPhaseList()
        {
            List<IBPhaseList> projectNames = new List<IBPhaseList>();

            var projectList = dbContext.GetManage_IB_Phases(0);

            projectNames = (from s in projectList
                            select new IBPhaseList
                            {
                                PhaseID = s.PhaseID,
                                PhaseName = s.Phase
                            }).ToList();

            return projectNames.ToList();
        }

        public PhasesViewModel IBManagesPhaseDateList(int ProjectId)
        {
            List<PhasesViewModel> ProjectRecords = new List<PhasesViewModel>();
            PhasesViewModel DateVales = new PhasesViewModel();
            var projectList = dbContext.GetIBPhaseStartEndTime_SP(ProjectId);
            ProjectRecords = (from s in projectList
                              select new PhasesViewModel
                              {
                                  StartDate = s.ActualStartDate,
                                  EndDate = s.ActualEndDate,
                              }).ToList();

            foreach (var item in ProjectRecords)
            {
                DateVales.EndDate = item.EndDate;
                DateVales.StartDate = item.StartDate;
            }

            return DateVales;
        }

        public List<PhasesViewModel> IBManagesPhaseOrderNumersList(int ProjectId)
        {
            List<PhasesViewModel> ProjectRecords = new List<PhasesViewModel>();
            PhasesViewModel DateVales = new PhasesViewModel();
            var projectList = dbContext.GetIBPhaseOrderNumbers_SP(ProjectId);
            ProjectRecords = (from s in projectList
                              select new PhasesViewModel
                              {
                                  OrderNumber = s.ordernumber
                              }).ToList();

            return ProjectRecords;
        }

        public List<PMSProjectDetailsViewModel> GetProjectNamesListByApprovalStatus(int? approvalStatusID)
        {
            approvalStatusID = 2;
            List<PMSProjectDetailsViewModel> projectNames = new List<PMSProjectDetailsViewModel>();
            try
            {
                projectNames = (from project in dbContext.tbl_PM_Project
                                where project.ApprovalStatusID == approvalStatusID
                                select new PMSProjectDetailsViewModel
                                {
                                    ProjectID = project.ProjectID,
                                    ProjectName = project.ProjectName,
                                }).ToList();

                return projectNames.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ManageSubProjectsModel> LoadSubProjectGrid(int? ProjectID, int page, int rows, string employeeCode, out int totalCount)
        {
            List<ManageSubProjectsModel> subProjectRecords = new List<ManageSubProjectsModel>();
            try
            {
                var subProjectList = dbContext.GetSubProjectDetails_SP();

                subProjectRecords = (from m in subProjectList
                                     where m.ProjectId == ProjectID
                                     select new ManageSubProjectsModel
                                     {
                                         ProjectID = m.ProjectId,
                                         SubProjectName = m.SubProjectName,
                                         SubProjectId = m.SubProjectId,
                                         Description = m.Description,
                                         WorkHours = m.EstimatedEfforts,
                                         ResponsiblePerson = m.ResponsiblePersonName,
                                         StartDate = m.ActualStartDate,
                                         EndDate = m.ActualEndDate,
                                         HiddenResponsiblePerson = m.ResponsiblePerson,
                                         loggedInUserEmployeeCode = employeeCode
                                     }).ToList();

                totalCount = subProjectRecords.Count();
                return subProjectRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, int, bool> SaveSubProjectDetails(ManageSubProjectsModel SubProjectDetails, int ProjectID, string ResponsiblePerson, string loggedInUserEmployeeCode)
        {
            int? SubProjectId = 0;
            int subProjectID = 0;
            bool status = false;

            bool isSubProjectNameExist = false;

            var subProjectDtls = dbContext.GetSubProjectDetails_SP();

            List<ManageSubProjectsModel> subprojectNameExist = (from a in subProjectDtls
                                                                where (a.SubProjectName == SubProjectDetails.SubProjectName && a.ProjectId == ProjectID)
                                                                select new ManageSubProjectsModel
                                                               {
                                                                   SubProjectId = a.SubProjectId
                                                               }).ToList();

            string SubProjectName = SubProjectDetails.SubProjectName;
            string Description = SubProjectDetails.Description;
            Double? workHours = SubProjectDetails.WorkHours;
            int? responsiblePerson;
            if (ResponsiblePerson != "")
            {
                responsiblePerson = Convert.ToInt32(ResponsiblePerson);
            }
            else
            {
                responsiblePerson = null;
            }
            int? ProjectId = SubProjectDetails.ProjectID;
            ProjectId = ProjectID;
            //string loggedInUserEmployeeCode = SubProjectDetails.loggedInUserEmployeeCode;
            DateTime? StartDate = SubProjectDetails.StartDate;
            DateTime? EndDate = SubProjectDetails.EndDate;

            ObjectParameter ResultID = new ObjectParameter("ResultID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Output", typeof(int));

            if (SubProjectDetails.SubProjectId != 0 && SubProjectDetails.SubProjectId != null)
            {
                SubProjectId = SubProjectDetails.SubProjectId;

                dbContext.AddUpdateSubProjectDetails_SP(SubProjectId, ProjectId, SubProjectName, Description, workHours, responsiblePerson, StartDate, EndDate, "UPDATE", loggedInUserEmployeeCode, Output, ResultID);
            }
            else if ((SubProjectDetails.SubProjectId == 0 || SubProjectDetails.SubProjectId == null) && subprojectNameExist.Count == 0)
            {
                dbContext.AddUpdateSubProjectDetails_SP(SubProjectId, ProjectId, SubProjectName, Description, workHours, responsiblePerson, StartDate, EndDate, "INSERT", loggedInUserEmployeeCode, Output, ResultID);
            }
            else
            {
                status = false;
                SubProjectId = SubProjectDetails.SubProjectId;
                isSubProjectNameExist = true;
                return new Tuple<bool, int, bool>(status, subProjectID, isSubProjectNameExist);
            }

            status = Convert.ToBoolean(Output.Value);

            if (SubProjectDetails.SubProjectId == 0)
            {
                subProjectID = Convert.ToInt32(ResultID.Value);
            }
            else
            {
                subProjectID = Convert.ToInt32(SubProjectDetails.SubProjectId);
            }

            return new Tuple<bool, int, bool>(status, subProjectID, isSubProjectNameExist);
        }

        public bool DeleteSubProjectDetails(string[] subProjectID)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < subProjectID.Length; i++)
                {
                    ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                    int SubProjectId = Convert.ToInt32(subProjectID[i]);
                    dbContext.DeleteSubProjectDetails_SP(SubProjectId, Result);
                    status = Convert.ToBoolean(Result.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<bool, int, bool> SavePhaseIBMangDetail(PhasesViewModel custdetail)
        {
            bool status = false;
            bool isPhaseExist = false;
            int ProjectPhaseID = 0;
            var PhaseList = IBManagesPhaseList();
            int? ProjectID = custdetail.ProjectId;
            string Phase = (from relation in PhaseList
                            where relation.PhaseID == custdetail.IBPhaseManageId
                            select relation.PhaseName).FirstOrDefault();
            DateTime? EstimatedStartDate = custdetail.StartDate;
            DateTime? EstimatedEndDate = custdetail.EndDate;
            int? PlannedResources = custdetail.PeakTeamSize;

            float? PercentEfforts = custdetail.PercentageEfforts;
            bool CurrentPhase = custdetail.Currentphase;
            float? EstimatedEfforts = custdetail.PercentageEfforts;
            int? ResponsiblePerson = custdetail.ResponsiblePerId;
            int? ordernumber = custdetail.OrderNumber;

            ObjectParameter CustID = new ObjectParameter("CustID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            if (custdetail.ProjectPhaseId != 0)
            {
                ProjectPhaseID = custdetail.ProjectPhaseId;

                dbContext.AddUpdateIBPhaseDetails_SP(ProjectID, ProjectPhaseID, Phase, EstimatedStartDate, EstimatedEndDate, PlannedResources, PercentEfforts, CurrentPhase, EstimatedEfforts, ResponsiblePerson, ordernumber, "UPDATE", Output, CustID);
                status = Convert.ToBoolean(Output.Value);
            }
            else if (custdetail.ProjectPhaseId == 0)
            {
                tbl_IB_Project_Phases phaseDetails = dbContext.tbl_IB_Project_Phases.Where(x => x.ProjectID == ProjectID && x.Phase == Phase).FirstOrDefault();
                if (phaseDetails != null)
                {
                    isPhaseExist = true;
                }
                else
                {
                    dbContext.AddUpdateIBPhaseDetails_SP(ProjectID, ProjectPhaseID, Phase, EstimatedStartDate, EstimatedEndDate, PlannedResources, PercentEfforts, CurrentPhase, EstimatedEfforts, ResponsiblePerson, ordernumber, "INSERT", Output, CustID);
                    status = Convert.ToBoolean(Output.Value);
                }
            }

            if (custdetail.ProjectPhaseId == 0)
            {
                ProjectPhaseID = Convert.ToInt32(CustID.Value);
            }
            else
            {
                ProjectPhaseID = custdetail.ProjectPhaseId;
            }

            return new Tuple<bool, int, bool>(status, ProjectPhaseID, isPhaseExist);
        }

        public bool DeleteIDGridPhaseDetails(string[] ProjectPhaseId, int ProjectID)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < ProjectPhaseId.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    int ConvertedProjectPhaseID = Convert.ToInt32(ProjectPhaseId[i]);
                    dbContext.DeleteIBPhaseManagementDetails_sp(ProjectID, ConvertedProjectPhaseID, Output);
                    status = Convert.ToBoolean(Output.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PMSProjectDetailsViewModel> GetProjectApprovalDetails(int projectId, int page, int rows, out int totalCount)
        {
            List<PMSProjectDetailsViewModel> ProjectApprovalRecords = new List<PMSProjectDetailsViewModel>();

            try
            {
                var historyList = dbContext.GetProjectApprovalDetails_sp(projectId);

                ProjectApprovalRecords = (from item in historyList.ToList()
                                          select new PMSProjectDetailsViewModel
                                          {
                                              ProjectID = item.projectid,
                                              AuditId = item.logid,
                                              ApprovedRejectedOn = item.modifiedOn,
                                              ApprovedRejectedBy = item.employeename,
                                              ApprovalStatus = item.approvalstatus,
                                              ApproverDescription = item.Comments
                                          }).Skip((page - 1) * rows).Take(rows).ToList();

                totalCount = ProjectApprovalRecords.Count();
                return ProjectApprovalRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PracticeDetails> GetPracticeList()
        {
            List<PracticeDetails> PracticeDetailsRecords = new List<PracticeDetails>();
            try
            {
                var practiceList = dbContext.GetPracticeList_sp();
                PracticeDetailsRecords = (from p in practiceList
                                          select new PracticeDetails
                                          {
                                              PracticeID = p.TypeID,
                                              PracticeName = p.ProjectType
                                          }).ToList();
                return PracticeDetailsRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PhasesPracticeMapping> LoadPhasesPracticeMapping(int practiceID)
        {
            List<PhasesPracticeMapping> PhasesPracticeMappingRecords = new List<PhasesPracticeMapping>();
            try
            {
                var phasepracticeMapping = dbContext.GetPhasesPracticeMapping_sp(practiceID);
                PhasesPracticeMappingRecords = (from ppm in phasepracticeMapping
                                                select new PhasesPracticeMapping
                                                {
                                                    PhaseID = ppm.PhaseID,
                                                    PhaseName = ppm.Phase,
                                                    PercentageEfforts = ppm.PercentageEfforts,
                                                    OrderNumber = ppm.ordernumber,
                                                    IsSelected = ppm.IsSelected
                                                }).ToList();
                return PhasesPracticeMappingRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        # region milestone

        public bool DeleteMilestoneDetails(string[] MilestoneID)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < MilestoneID.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    int ConvertedMilestoneID = Convert.ToInt32(MilestoneID[i]);
                    dbContext.DeleteMilestoneDetails_SP(ConvertedMilestoneID, Output);
                    status = Convert.ToBoolean(Output.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<bool, int?, bool, bool> SaveMilestoneDetails(ManageMilestonesModel MilestoneDetails, int ProjectID, string ResponsiblePerson, string MilestoneStatus)
        {
            int? MilestoneId = 0;
            int milestoneID = 0;
            bool status = false;
            //MilestoneId = MilestoneID;
            bool isMilestoneNameExist = false;
            bool isValidmilestoneStatus = true;

            ManageMilestonesModel model = new ManageMilestonesModel();
            model.TaskClosureComplitionList = new List<TaskClosureComplition>();
            model.TaskClosureVoidList = new List<TaskClosureVoid>();

            if (MilestoneDetails.MilestoneID != 0 && MilestoneDetails.MilestoneID != null)
            {
                MilestoneId = MilestoneDetails.MilestoneID;
            }
            var milestoneDtls = dbContext.GeMilestoneDetails_SP();
            List<ManageMilestonesModel> milestoneNameExist = (from a in milestoneDtls
                                                              where (a.MileStone == MilestoneDetails.MilestoneName && a.ProjectID == ProjectID)
                                                              select new ManageMilestonesModel
                                                              {
                                                                  MilestoneID = a.MileStoneID
                                                              }).ToList();

            if (MilestoneDetails.MilestoneStatus == "Milestone closed")
            {
                model.TaskClosureComplitionList = LoadTaskClosureComplition(ProjectID, MilestoneId);
                model.TaskClosureVoidList = LoadTaskClosureVoid(ProjectID, MilestoneId);

                if (model.TaskClosureComplitionList.Count > 0 || model.TaskClosureVoidList.Count > 0)
                {
                    isValidmilestoneStatus = false;
                    return new Tuple<bool, int?, bool, bool>(status, MilestoneId, isMilestoneNameExist, isValidmilestoneStatus);
                }
            }

            string MilestoneName = MilestoneDetails.MilestoneName;
            string MilestoneDescription = MilestoneDetails.MilestoneDescription;
            string milestoneStatus = MilestoneStatus;

            int? responsiblePerson;
            if (ResponsiblePerson != "")
            {
                responsiblePerson = Convert.ToInt32(ResponsiblePerson);
            }
            else
            {
                responsiblePerson = null;
            }
            int? ProjectId = MilestoneDetails.ProjectID;
            ProjectId = ProjectID;
            DateTime? StartDate = MilestoneDetails.StartDate;
            DateTime? EndDate = MilestoneDetails.EndDate;

            ObjectParameter MID = new ObjectParameter("MID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));

            if (MilestoneDetails.MilestoneID != 0 && MilestoneDetails.MilestoneID != null)
            {
                MilestoneId = MilestoneDetails.MilestoneID;

                dbContext.AddUpdateMilestones_SP(MilestoneId, ProjectId, MilestoneName, MilestoneDescription, milestoneStatus, responsiblePerson, StartDate, EndDate, "UPDATE", Output, MID);
            }
            else if ((MilestoneDetails.MilestoneID == 0 || MilestoneDetails.MilestoneID == null) && milestoneNameExist.Count == 0)
            {
                dbContext.AddUpdateMilestones_SP(MilestoneId, ProjectId, MilestoneName, MilestoneDescription, milestoneStatus, responsiblePerson, StartDate, EndDate, "INSERT", Output, MID);
            }
            else
            {
                status = false;
                MilestoneId = MilestoneDetails.MilestoneID;
                isMilestoneNameExist = true;
                return new Tuple<bool, int?, bool, bool>(status, milestoneID, isMilestoneNameExist, isValidmilestoneStatus);
            }

            status = Convert.ToBoolean(Output.Value);

            if (MilestoneDetails.MilestoneID == 0)
            {
                milestoneID = Convert.ToInt32(MID.Value);
            }
            else
            {
                milestoneID = Convert.ToInt32(MilestoneDetails.MilestoneID);
            }

            return new Tuple<bool, int?, bool, bool>(status, milestoneID, isMilestoneNameExist, isValidmilestoneStatus);
        }

        public List<ManageMilestonesModel> LoadMilestoneGrid(int? ProjectID, int page, int rows, out int totalCount)
        {
            List<ManageMilestonesModel> MilestoneRecords = new List<ManageMilestonesModel>();
            try
            {
                var milestoneList = dbContext.GeMilestoneDetails_SP();

                MilestoneRecords = (from m in milestoneList
                                    where m.ProjectID == ProjectID
                                    select new ManageMilestonesModel
                                    {
                                        ProjectID = m.ProjectID,
                                        MilestoneName = m.MileStone,
                                        MilestoneID = m.MileStoneID,
                                        MilestoneDescription = m.Comments,
                                        MilestoneStatus = m.MilestoneStatus,
                                        HiddenResponsiblePerson = m.ResponsiblePerson,
                                        StartDate = m.PlannedCompletionDate,
                                        EndDate = m.ActualCompletionDate,
                                        ResponsiblePerson = m.EmployeeName
                                    }).ToList();

                totalCount = MilestoneRecords.Count();
                //return MilestoneRecords.Skip((page - 1) * rows).Take(rows).ToList();
                return MilestoneRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TaskClosureComplition> LoadTaskClosureComplition(int ProjectID, int? MilestoneID)
        {
            //ProjectID = 390;
            //MilestoneID = 4892;

            List<TaskClosureComplition> taskClosure = new List<TaskClosureComplition>();
            var taskClosureList = dbContext.usp_Sel_tbl_PM_TasksForCompletion_MileStoneClosure(ProjectID, MilestoneID);

            taskClosure = (from s in taskClosureList
                           select new TaskClosureComplition
                           {
                               TaskName = s.TaskName,
                               TaskID = s.TaskID,
                               ActualPercentComplete = s.ActualPercentComplete,
                               ActualWork = s.ActualWork,
                               ActualStartDate = s.ActualStartDate,
                               ResponsiblePerson = s.EmployeeName,
                               EndDate = s.EndDate,
                               MileStoneID = s.MileStoneID,
                               StartDate = s.StartDate,
                               PlannedWork = s.PlannedWork,
                               ProjectId = s.ProjectId,
                               TaskClosureComplitionChecked = false
                           }).ToList();

            return taskClosure.ToList();
        }

        public List<TaskClosureVoid> LoadTaskClosureVoid(int ProjectID, int? MilestoneID)
        {
            //ProjectID = 390;
            //MilestoneID = 4892;

            List<TaskClosureVoid> taskClosure = new List<TaskClosureVoid>();
            var taskClosureList = dbContext.usp_Sel_tbl_PM_TasksForVoiding_MileStoneClosure(ProjectID, MilestoneID);

            taskClosure = (from s in taskClosureList
                           select new TaskClosureVoid
                           {
                               TaskName = s.TaskName,
                               TaskID = s.TaskID,
                               ActualPercentComplete = s.ActualPercentComplete,
                               ActualWork = s.ActualWork,
                               ActualStartDate = s.ActualStartDate,
                               ResponsiblePerson = s.EmployeeName,
                               EndDate = s.EndDate,
                               MileStoneID = MilestoneID,
                               StartDate = s.StartDate,
                               PlannedWork = s.PlannedWork,
                               ProjectId = ProjectID,
                               TaskClosureVoidChecked = false
                           }).ToList();

            return taskClosure.ToList();
        }

        public List<ManageMilestonesModel> GetMilestoneStatusList()
        {
            List<ManageMilestonesModel> MilestoneStatusList = new List<ManageMilestonesModel>();
            try
            {
                var milestoneStatus = dbContext.GetMilestoneStatusList_SP();

                MilestoneStatusList = (from m in milestoneStatus
                                       select new ManageMilestonesModel
                                       {
                                           MilestoneStatus = m.MilestoneStatus,
                                       }).ToList();
                return MilestoneStatusList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, bool> CloseTasks(string[] TaskID, int Flag, int ProjectID, int? MilestoneID)
        {
            try
            {
                bool status = false;
                bool IsMilestoneClosed = true;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                ObjectParameter TID = new ObjectParameter("TID", typeof(int));
                for (int i = 0; i < TaskID.Length; i++)
                {
                    int ConvertedTaskID = Convert.ToInt32(TaskID[i]);
                    dbContext.usp_Upd_AssignedTasks_Updation_ProjectClosure(Flag, ProjectID, ConvertedTaskID, Output);
                    //dbContext.UpdateTaskStatus_SP(ConvertedTaskID, Output, TID);
                    //status = Convert.ToBoolean(Output.Value);
                    //status = true;
                }
                status = Convert.ToBoolean(Output.Value);

                ManageMilestonesModel model = new ManageMilestonesModel();
                SemDAL dal = new SemDAL();

                // model.TaskClosureComplitionList = new List<TaskClosureComplition>();
                var ComplitionList = dal.LoadTaskClosureComplition(ProjectID, MilestoneID);

                //model.TaskClosureVoidList = new List<TaskClosureVoid>();
                var voidList = dal.LoadTaskClosureVoid(ProjectID, MilestoneID);

                int voidListCount = voidList.Count;
                int ComplitionListCount = ComplitionList.Count;

                if (voidListCount == 0 && ComplitionListCount == 0)
                {
                    dbContext.usp_Upd_tbl_PM_MileStone_CloseMileStone(MilestoneID);
                    IsMilestoneClosed = true;
                }
                else
                {
                    IsMilestoneClosed = false;
                }

                return new Tuple<bool, bool>(status, IsMilestoneClosed);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        //public List<SEMEmployeeListDetails> GetEmployeeList()
        //{
        //    List<SEMEmployeeListDetails> employeelist = new List<SEMEmployeeListDetails>();
        //    try
        //    {
        //        var list = dbContext.GetEmployeeList_SP();

        //        employeelist = (from employee in list
        //                        select new SEMEmployeeListDetails
        //                        {
        //                            EmployeeId = employee.Employeeid,
        //                            EmployeeName = employee.EmployeeName.Trim(),
        //                            EmployeeCode = employee.EmployeeCode

        //                        }).OrderBy(x => x.EmployeeName).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return employeelist;
        //}

        public List<SEMEmployeeListDetails> GetEmployeeList(int? ProjectID)
        {
            List<SEMEmployeeListDetails> employeelist = new List<SEMEmployeeListDetails>();
            try
            {
                //var list = dbContext.GetEmployeeList_SP();
                var list = dbContext.GetCurrentUsersForProject(ProjectID, null, null);

                employeelist = (from employee in list
                                select new SEMEmployeeListDetails
                                {
                                    EmployeeId = employee.EmployeeID,
                                    EmployeeName = employee.EmployeeName.Trim(),
                                    EmployeeCode = employee.employeecode
                                }).OrderBy(x => x.EmployeeName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeelist;
        }

        public bool SaveDocumentFileDetails(DocumentFileDetails model)
        {
            try
            {
                var DocumentFiles = dbContext.GetProjectDocumentsFileDetails();
                DocumentFileDetails DocumentFileDetail = (from file in DocumentFiles
                                                          where file.DocumentID == model.DocumentAttachmentID
                                                          select new DocumentFileDetails
                                                          {
                                                              DocumentAttachmentID = file.DocumentID
                                                          }).FirstOrDefault();

                int DocumentAttachmentID = model.DocumentAttachmentID;
                int ProjectId = model.ProjectId;
                string uploadedBy = model.LoggedInUser;
                DateTime uploadOn = DateTime.Now;
                string Details = model.Details;
                string DocName = model.DocName;
                string Operation = "";
                string DocPath = model.DocPath;
                float FileSize = model.FileSize;
                int categoryId = Convert.ToInt16(model.CategoryId);
                int subCategoryId = Convert.ToInt32(model.SubCategoryId);
                if (DocumentFileDetail != null)
                    Operation = "UPDATE";
                else
                    Operation = "INSERT";

                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                dbContext.AddUpdateDocumentFileDetails_sp(DocumentAttachmentID, ProjectId, uploadedBy, uploadOn, FileSize, categoryId, subCategoryId, DocPath, Details, DocName, Operation, Output);

                bool status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DocumentCategoryDetails> GetCategoryList()
        {
            List<DocumentCategoryDetails> categorylist = new List<DocumentCategoryDetails>();
            try
            {
                var list = dbContext.GetPMSProjectDocumentCategory_SP();
                categorylist = (from item in list
                                select new DocumentCategoryDetails
                                {
                                    DocumentCategoryId = item.Categoryid,
                                    DocumentCategory = item.category.Trim()
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return categorylist;
        }

        public List<DocumentFileDetails> ProjectDocumentFileRecord(int projectId, string searchtext, int? documentCategoryId, int page, int rows, out int totalCount)
        {
            List<DocumentFileDetails> documentFileRecords = new List<DocumentFileDetails>();
            var DocumentFilesDetails = dbContext.GetProjectDocumentsFileDetailsWithCategory_sp();
            try
            {
                documentFileRecords = (from documents in DocumentFilesDetails
                                       where documents.ProjectId == projectId && documents.docname.ToLower().Contains(searchtext.ToLower()) && documents.categoryId == (documentCategoryId == 0 || documentCategoryId == null ? documents.categoryId : documentCategoryId)
                                       select new DocumentFileDetails
                                       {
                                           ProjectId = projectId,
                                           DocumentAttachmentID = documents.documentId,
                                           DocName = documents.docname,
                                           CategoryId = documents.categoryId,
                                           Category = documents.categoryName,
                                           SubCategoryId = documents.SubCategoryID,
                                           SubCategory = documents.SubCategory,
                                           UploadedBy = documents.UploadedByName,
                                           UploadedOn = documents.uploadedDate,
                                           Details = documents.details
                                       }).ToList();

                totalCount = documentFileRecords.Count();
                //  return documentFileRecords.Skip((page - 1) * rows).Take(rows).ToList();
                return documentFileRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DirectoryName GetProjectDocumentsFileDetailsById(int docId)
        {
            //List<DocumentFileDetails> docFileDetails = new List<DocumentFileDetails>();
            DirectoryName docpath = new DirectoryName();
            DocumentFileDetails docFileDetails = new DocumentFileDetails();
            try
            {
                var docfileRecord = dbContext.GetProjectDocumentsFileDetailsById(docId);

                if (docFileDetails != null)
                {
                    docFileDetails = (from doc in docfileRecord
                                      select new DocumentFileDetails
                                      {
                                          DocName = doc.FileName,
                                          DocPath = doc.DirectoryName
                                      }).FirstOrDefault();
                }
                docpath.DirectoryNames = docFileDetails.DocPath;
                docpath.FileName = docFileDetails.DocName;
                return docpath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectDetails> GetProjectAllocationDetails(string empCode)
        {
            List<ProjectDetails> categorylist = new List<ProjectDetails>();
            try
            {
                //var list = dbContext.GetProjectAllocationDetails_sp(empCode); changed to only active projectlist
                if (HttpContext.Current.User.IsInRole("RMG") || HttpContext.Current.User.IsInRole("PMO"))
                {
                    var list = dbContext.GetProjectListForRMG_SP(Convert.ToInt32(empCode));
                    categorylist = (from item in list
                                    select new ProjectDetails
                                    {
                                        ProjectId = item.ProjectID,
                                        ProjectName = item.projectName.Trim()
                                    }).ToList();
                }
                else
                {
                    var list = dbContext.GetActiveProjectList_SP(Convert.ToInt32(empCode));
                    categorylist = (from item in list
                                    select new ProjectDetails
                                    {
                                        ProjectId = item.projectId,
                                        ProjectName = item.projectName.Trim()
                                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return categorylist;
        }

        public bool DeleteProjectDocumentFileRecord(int DocumentAttachmentID)
        {
            try
            {
                bool status = false;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                dbContext.DeleteProjectContactFileDetails_sp(DocumentAttachmentID, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CustomerDetails GetCustomerDetailsForProjectMail(int customerId)
        {
            CustomerDetails customerList = new CustomerDetails();
            try
            {
                var list = dbContext.GetCustomerName_sp(customerId);
                customerList = (from item in list
                                select new CustomerDetails
                                {
                                    CustomerId = item.customer,
                                    CustomerName = item.customername.Trim()
                                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return customerList;
        }

        public CustomerContract GetLatestContractDetails(int CustomerId)
        {
            try
            {
                var latestContract = dbContext.GetLatestCustomerContractDetails_sp(CustomerId);
                CustomerContract ContractDetails = (from contract in latestContract
                                                    select new CustomerContract
                                                    {
                                                        ContractID = contract.ContractID,
                                                        ContractType = contract.ContractTypeID,
                                                        ContractTypeName = contract.ContractTypeName,
                                                        ContractSigningDate = contract.ContractSigningDate,
                                                        ContractValidityDate = contract.ContractValidityDate,
                                                        CommencementDate = contract.CommencementDate,
                                                        ContractSummary = contract.ContractSummary,
                                                        ContractDetails = contract.ContractDetails,
                                                        CustomerName = contract.CustomerName
                                                    }).FirstOrDefault();
                return ContractDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddUpdateProjectTypePhaseData(string[] allFinalValues)
        {
            try
            {
                bool status = false;
                int counter = 0;
                for (int i = 0; i < allFinalValues.Length / 5; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    dbContext.usp_AddUpdateProjectTypePhaseData(Convert.ToInt32(allFinalValues[counter]), Convert.ToInt32(allFinalValues[counter + 1]), Convert.ToInt32(allFinalValues[counter + 2]), Convert.ToDouble(allFinalValues[counter + 3]), Convert.ToDouble(allFinalValues[counter + 4]), Output);
                    status = Convert.ToBoolean(Output.Value);
                    counter = counter + 5;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ResourceAllocationDetailsList> GetEmployeeList1()
        {
            List<ResourceAllocationDetailsList> employeelist = new List<ResourceAllocationDetailsList>();
            try
            {
                var list = dbContext.GetEmployeeList_SP();
                employeelist = (from employee in list
                                select new ResourceAllocationDetailsList
                                {
                                    Employeeid = employee.Employeeid,
                                    EmployeeName = employee.EmployeeName.Trim(),
                                    EmployeeCode = employee.EmployeeCode
                                }).OrderBy(x => x.EmployeeName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeelist;
        }

        public string GetMaxRoleForUser(string userName)
        {
            try
            {
                string[] userRoles = Roles.GetRolesForUser(userName);
                string roleToPass = null;
                int no = 0;
                int[] arr = new int[userRoles.Length];
                int p = 0;
                foreach (string role in userRoles)
                {
                    if (role == "Admin" || role == "RMG")
                    {
                        no = (int)EmployeeRolesOrderSem.Admin;
                    }
                    else
                    {
                        if (role == "PMS_DT")
                        {
                            no = (int)EmployeeRolesOrderSem.PMS_DT;
                        }
                        else
                        {
                            if (role == "PMS_DU")
                            {
                                no = (int)EmployeeRolesOrderSem.PMS_DU;
                            }
                            else
                            {
                                if (role == "Manager")
                                {
                                    no = (int)EmployeeRolesOrderSem.Manager;
                                }
                            }
                        }
                    }

                    if (no != 0)
                    {
                        arr[p] = no;
                        p++;
                        no = 0;
                    }
                }

                int maxVal = arr[0];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] != 0)
                    {
                        if (arr[i] < maxVal)
                            maxVal = arr[i];
                    }
                }

                if (maxVal == (int)EmployeeRolesOrderSem.Admin)
                {
                    roleToPass = "Admin";
                }
                else
                {
                    if (maxVal == (int)EmployeeRolesOrderSem.PMS_DU)
                    {
                        roleToPass = "PMS_DU";
                    }
                    else
                    {
                        if (maxVal == (int)EmployeeRolesOrderSem.PMS_DT)
                        {
                            roleToPass = "PMS_DT";
                        }
                        else
                        {
                            if (maxVal == (int)EmployeeRolesOrderSem.Manager)
                            {
                                roleToPass = "Manager";
                            }
                        }
                    }
                }
                return roleToPass;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<HRMS.Models.RMGViewPostModel.ProjectAppListApproved> GetResourceAllocationProjectList(int ProjectStatus, int employeeId, string roleToPass, string empRole)
        {
            List<HRMS.Models.RMGViewPostModel.ProjectAppListApproved> projectNames = new List<HRMS.Models.RMGViewPostModel.ProjectAppListApproved>();

            var projectList = dbContext.GetProjectStatusDetails(ProjectStatus, employeeId, roleToPass, empRole);

            projectNames = (from s in projectList
                            select new HRMS.Models.RMGViewPostModel.ProjectAppListApproved
                            {
                                Projectids = s.ProjectID,
                                ProjectName = s.ProjectName
                            }).OrderBy(x => x.ProjectName).ToList();

            return projectNames.ToList();
        }

        public List<RMGViewPostModel> GetResouceAllocGridsDetails(int EmployeeId, int projectID, string searchText, DateTime? AsOnDate, int page, int rows, out int totalCount, string GridName, int? ResourcePoolId, int? EmployeeForProject)
        {
            List<RMGViewPostModel> ProjectRecords = new List<RMGViewPostModel>();

            try
            {
                if (EmployeeId != 0 && GridName == "MyCurrentAllocation")
                {
                    var customerList = dbContext.SearchCurrentUsersForProject_SP(EmployeeId);
                    var EmployeeList = (from s in customerList
                                        select new RMGViewPostModel
                                        {
                                            ProjectName = s.ProjectName,
                                            HelpDeskTicketID = s.HelpdeskTicketID,
                                            ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                            EmployeeId = s.EmployeeID,
                                            EmployeeCode = s.employeecode,
                                            EmployeeName = s.EmployeeName,
                                            ReportingTo = s.reportingToName,
                                            ResourcePool = s.ResourcePoolName,
                                            Designation = s.DesignationName,
                                            ProjectRole = s.DesignationName,
                                            ResourceType = s.ResourceStatus,
                                            EmploymentStatus = s.EmploymentStatus,
                                            AllocationStartDate1 = s.StartDate,
                                            AllocationEndDate1 = s.EndDate,
                                            Allocated = Convert.ToDecimal(s.AllocatedPercentage),
                                            ReleaseDate = s.ReleaseEndDate,
                                            ProjectEndAppraisalForm = s.EmployeeName,
                                            RMGComments = s.Comments,
                                            ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                            // ProjectStartDate=s.
                                        }).ToList();

                    ProjectRecords = (from s in EmployeeList
                                      orderby s.EmployeeCode descending
                                      select new RMGViewPostModel
                                      {
                                          ProjectName = s.ProjectName,
                                          HelpDeskTicketID = s.HelpDeskTicketID,
                                          ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                          EmployeeCode = s.EmployeeCode,
                                          EmployeeName = s.EmployeeName,
                                          ReportingTo = s.ReportingTo,
                                          ResourcePool = s.ResourcePool,
                                          Designation = s.Designation,
                                          ProjectRole = s.Designation,
                                          ResourceType = s.ResourceType,
                                          EmploymentStatus = s.EmploymentStatus,
                                          AllocationStartDate1 = s.AllocationStartDate1,
                                          AllocationEndDate1 = s.AllocationEndDate1,
                                          Allocated = s.Allocated,
                                          ReleaseDate = s.ReleaseDate,
                                          ProjectEndAppraisalForm = s.EmployeeName,
                                          RMGComments = s.RMGComments,
                                          ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                      }).ToList();
                }

                if (EmployeeId != 0 && GridName == "MyYHistoryAllocation")
                {
                    var customerList = dbContext.SearchResourceHis_SP(EmployeeId);
                    var EmployeeList = (from s in customerList
                                        select new RMGViewPostModel
                                        {
                                            ProjectName = s.ProjectName,
                                            HelpDeskTicketID = s.HelpdeskTicketID,
                                            EmployeeCode = s.employeecode,
                                            EmployeeId = s.EmployeeID,
                                            EmployeeName = s.EmployeeName,
                                            ReportingTo = s.ResourcePoolName,
                                            ResourcePool = s.ResourcePoolName,
                                            Designation = s.DesignationName,
                                            ProjectRole = s.DesignationName,
                                            ResourceType = s.ResourceStatus,
                                            EmploymentStatus = s.EmploymentStatus,
                                            AllocationStartDate = s.StartDate,
                                            AllocationEndDate = s.EndDate,
                                            Allocated = Convert.ToDecimal(s.AllocatedPercentage),
                                            ReleaseDate = s.EndDate,
                                            ProjectEndAppraisalForm = s.EmployeeName,
                                            RMGComments = s.Comments,
                                            ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                        }).ToList();

                    ProjectRecords = (from s in EmployeeList
                                      orderby s.EmployeeCode descending
                                      select new RMGViewPostModel
                                      {
                                          ProjectName = s.ProjectName,
                                          HelpDeskTicketID = s.HelpDeskTicketID,
                                          EmployeeCode = s.EmployeeCode,
                                          EmployeeName = s.EmployeeName,
                                          ReportingTo = s.ReportingTo,
                                          ResourcePool = s.ResourcePool,
                                          Designation = s.Designation,
                                          ProjectRole = s.Designation,
                                          ResourceType = s.ResourceType,
                                          EmploymentStatus = s.EmploymentStatus,
                                          AllocationStartDate = s.AllocationStartDate,
                                          AllocationEndDate = s.AllocationEndDate,
                                          Allocated = s.Allocated,
                                          ReleaseDate = s.ReleaseDate,
                                          ProjectEndAppraisalForm = s.EmployeeName,
                                          RMGComments = s.RMGComments,
                                          ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                      }).ToList();
                }

                if (GridName == "Current")
                {
                    var customerList = dbContext.GetCurrentUsersForProject(projectID, ResourcePoolId, EmployeeForProject).AsEnumerable();
                    var EmployeeList = (from s in customerList
                                        select new RMGViewPostModel
                                            {
                                                HelpDeskTicketID = s.HelpdeskTicketID,
                                                ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                                EmployeeCode = s.employeecode,
                                                EmployeeId = s.EmployeeID,
                                                EmployeeName = s.EmployeeName,
                                                ProjectID = s.ProjectID,
                                                ProjectName = s.ProjectName,
                                                ReportingTo = s.reportingToName,
                                                ResourcePool = s.ResourcePoolName,
                                                Designation = s.DesignationName,
                                                ProjectRole = s.RoleDescription,
                                                ResourceType = s.ResourceStatus,
                                                EmploymentStatus = s.EmploymentStatus,
                                                AllocationStartDate = s.StartDate,
                                                AllocationEndDate = s.EndDate,
                                                Allocated = Convert.ToDecimal(s.AllocatedPercentage),
                                                ReleaseDate = s.ActualEndDate,
                                                ProjectEndAppraisalForm = s.EmployeeName,
                                                RMGComments = s.Comments,
                                                ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID,
                                                ProjectSkillMatrixStausID = s.ProjectSkillMatrixStausID
                                            }).ToList();

                    ProjectRecords = (from s in EmployeeList
                                      orderby s.EmployeeCode descending
                                      select new RMGViewPostModel
                                      {
                                          HelpDeskTicketID = s.HelpDeskTicketID,
                                          ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                          EmployeeCode = s.EmployeeCode,
                                          EmployeeId = s.EmployeeId,
                                          EmployeeName = s.EmployeeName,
                                          ProjectID = s.ProjectID,
                                          ProjectName = s.ProjectName,
                                          ReportingTo = s.ReportingTo,
                                          ResourcePool = s.ResourcePool,
                                          Designation = s.Designation,
                                          ProjectRole = s.ProjectRole,
                                          ResourceType = s.ResourceType,
                                          EmploymentStatus = s.EmploymentStatus,
                                          AllocationStartDate = s.AllocationStartDate,
                                          AllocationEndDate = s.AllocationEndDate,
                                          Allocated = s.Allocated,
                                          ReleaseDate = s.ReleaseDate,
                                          ProjectEndAppraisalForm = s.EmployeeName,
                                          RMGComments = s.RMGComments,
                                          ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID,
                                          ProjectSkillMatrixStausID = s.ProjectSkillMatrixStausID
                                      }).ToList();
                }
                if (GridName == "History")
                {
                    var customerList = dbContext.GetHistoryForProject(projectID, ResourcePoolId, EmployeeForProject);
                    var EmployeeList = (from s in customerList
                                        select new RMGViewPostModel
                                            {
                                                HelpDeskTicketID = s.HelpdeskTicketID,
                                                ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                                EmployeeCode = s.employeecode,
                                                EmployeeId = s.EmployeeID,
                                                EmployeeName = s.EmployeeName,
                                                ReportingTo = s.ResourcePoolName,
                                                ResourcePool = s.ResourcePoolName,
                                                Designation = s.DesignationName,
                                                ProjectRole = s.RoleDescription,
                                                ResourceType = s.ResourceStatus,
                                                EmploymentStatus = s.EmploymentStatus,
                                                AllocationStartDate = s.StartDate,
                                                AllocationEndDate = s.EndDate,
                                                Allocated = Convert.ToDecimal(s.AllocatedPercentage),
                                                // ReleaseDate = s.EndDate ,
                                                ReleaseDate = s.ActualEndDate,
                                                ProjectEndAppraisalForm = s.EmployeeName,
                                                RMGComments = s.Comments,
                                                ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                            }).ToList();
                    ProjectRecords = (from s in EmployeeList
                                      orderby s.EmployeeCode descending
                                      select new RMGViewPostModel
                                      {
                                          HelpDeskTicketID = s.HelpDeskTicketID,
                                          ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                          EmployeeCode = s.EmployeeCode,
                                          EmployeeId = s.EmployeeId,
                                          EmployeeName = s.EmployeeName,
                                          ReportingTo = s.EmployeeName,
                                          ResourcePool = s.ResourcePool,
                                          Designation = s.Designation,
                                          ProjectRole = s.ProjectRole,
                                          ResourceType = s.ResourceType,
                                          EmploymentStatus = s.EmploymentStatus,
                                          AllocationStartDate = s.AllocationStartDate,
                                          AllocationEndDate = s.AllocationEndDate,
                                          Allocated = s.Allocated,
                                          ReleaseDate = s.ReleaseDate,
                                          ProjectEndAppraisalForm = s.EmployeeName,
                                          RMGComments = s.RMGComments,
                                          ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                      }).ToList();
                }

                //Code commented by nikhil
                if (GridName == "Bench" && projectID != 0)
                {
                    var benchList = dbContext.GetUnallocatedResource_sp(AsOnDate, null);
                    if (EmployeeId != 0)
                        benchList = dbContext.GetUnallocatedResource_sp(AsOnDate, EmployeeId);
                    ProjectRecords = (from s in benchList
                                      select new RMGViewPostModel
                                          {
                                              SearchEmployeeCode = s.EmployeeCode,
                                              EmployeeName = s.EmployeeName,
                                              PrimarySkills = s.Resource_Pool,
                                              ReportingTo = s.ReportingTo,
                                              DesignationName = s.ORGROLE,
                                              Allocated = Convert.ToDecimal(s.percentage),
                                              PresentAbsent = s.Present_Absent,
                                              UnallocatedFrom = Convert.ToString(s.unallocatedfrom)
                                          }).ToList();
                }
                //if (GridName != "Bench")
                //{
                //    for (int i = 0; i < ProjectRecords.Count; i++)
                //    {
                //        int userId = Convert.ToInt32(ProjectRecords[i].EmployeeCode);
                //        var ReportingMgrname = dbContext.SearchCurrentUsersForProject(userId);
                //        ProjectRecords[i].ReportingTo = (from relation in ReportingMgrname
                //                                         select relation.ReportingTo).FirstOrDefault();
                //    }
                //}
                totalCount = ProjectRecords.Count();
                return ProjectRecords.ToList();
                //return ProjectRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EmployeeListDetails> GetEmployeeListAllocatedToProject(int? ProjectID)
        {
            try
            {
                List<EmployeeListDetails> EmployeeList = new List<EmployeeListDetails>();
                var employees = dbContext.GetCurrentUsersForProjectAndProjectReviewers();
                EmployeeList = (from e in employees
                                select new EmployeeListDetails
                                {
                                    EmployeeId = e.EmployeeID,
                                    EmployeeName = e.EmployeeName
                                }).ToList();
                return EmployeeList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EmployeeListDetails> GetEmployeeListAllocatedToProject1(string SearchText, int pageNo = 1, int pageSize = 20)
        {
            try
            {
                List<EmployeeListDetails> EmployeeList = new List<EmployeeListDetails>();
                var employees = dbContext.GetCurrentUsersForProjectAndProjectReviewers();
                EmployeeList = (from e in employees
                                where e.EmployeeName.ToUpper().Contains(SearchText.ToUpper())
                                select new EmployeeListDetails
                                {
                                    EmployeeId = e.EmployeeID,
                                    EmployeeName = e.EmployeeName
                                }).ToList();
                return EmployeeList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EmployeeListDetails> GetEmployeeListForReportingTo(int? ProjectId)
        {
            try
            {
                List<EmployeeListDetails> EmployeeList = new List<EmployeeListDetails>();
                int resourcePoolId = 0;
                var employees = dbContext.GetCurrentUsersForProjectAndProjectReviewers();
                EmployeeList = (from e in employees
                                select new EmployeeListDetails
                                {
                                    EmployeeId = e.EmployeeID,
                                    EmployeeName = e.EmployeeName
                                }).ToList();
                return EmployeeList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Role> getEmployeeRoles()
        {
            try
            {
                List<Role> EmployeeRoleLists = new List<Role>();
                var EmployeeRoles = dbContext.GetEmployeeRoleList_SP();
                EmployeeRoleLists = (from roles in EmployeeRoles
                                     orderby roles.roleDescription ascending
                                     select new Role
                                     {
                                         RoleID = roles.roleid,
                                         RoleDescription = roles.roleDescription
                                     }).ToList();
                return EmployeeRoleLists;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ResourseType> GetResourceTypes()
        {
            try
            {
                List<ResourseType> resourceType = new List<ResourseType>();
                var resourcetypes = dbContext.GetResourceType_SP();
                resourceType = (from r in resourcetypes
                                select new ResourseType
                                {
                                    ResourseTypeID = r.ResourceStatusID,
                                    ResourseTypeDescription = r.ResourceStatus
                                }).ToList();
                return resourceType;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SaveAddEditResources SaveAddEditResource(AddEdirResourseModel model)
        {
            try
            {
                bool status;
                SaveAddEditResources resource = new SaveAddEditResources();
                int EmployeeID = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeName == model.EmployeeName && x.Status == false).FirstOrDefault().EmployeeID;
                var allocationPercentage = dbContext.GetEmpResourceAllocationPercentage_SP(EmployeeID, model.AllocationStartDate, model.AllocationEndDate, Convert.ToString(model.ProjectEmployeeRoleID));
                List<AddEdirResourseModel> totalallocationPercentage = (from r in allocationPercentage
                                                                        select new AddEdirResourseModel
                                                                        {
                                                                            totalAllocation = r.AllocatedPercentageSUM
                                                                        }).ToList();

                foreach (var item in totalallocationPercentage)
                {
                    model.totalAllocation = item.totalAllocation;
                }
                if (model.totalAllocation >= 100.00 && model.ButtonClick == "NewAllocation")
                {
                    resource.ErrorMessage = "Employee is already 100% allocated";
                    resource.CanAllocationDone = false;
                    resource.isAllocationDone = false;
                }
                else
                {
                    if (model.totalAllocation == null)
                        model.totalAllocation = 0;
                    double? currentAllocationSum = model.totalAllocation + model.AllocatedPercentage;
                    if (currentAllocationSum > 100 && model.ButtonClick == "NewAllocation")
                    {
                        double? value;
                        if (currentAllocationSum > model.AllocatedPercentage)
                            value = model.totalAllocation - 100;
                        else
                            value = 100 - model.totalAllocation;
                        string FinalValue = value.HasValue ? value.Value.ToString() : "0";
                        if (FinalValue.Contains("-") == true)
                        {
                            //FinalValue = value.ToString();
                            FinalValue = FinalValue.Replace("-", string.Empty);
                        }
                        resource.ErrorMessage = "You have only " + FinalValue + " % allocation of this Employee";
                        resource.CanAllocationDone = false;
                        resource.isAllocationDone = false;
                    }
                    else
                    {
                        bool canResourceAllocate = true;
                        int loggedInEmployeeID = this.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
                        string userName = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeID == loggedInEmployeeID).FirstOrDefault().UserName;
                        ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                        string buttonClick = string.Empty;
                        if (model.ButtonClick == "NewAllocation")
                            buttonClick = "Add";
                        else
                        {
                            buttonClick = "Update";
                            // tbl_PM_ProjectEmployeeRole allocationDetails = dbContext.tbl_PM_ProjectEmployeeRole.Where(x => x.HelpdeskTicketID == model.HelpdeskTicketID).FirstOrDefault();
                            tbl_PM_ProjectEmployeeRole allocationDetails = (from x in dbContext.tbl_PM_ProjectEmployeeRole
                                                                            where x.EmployeeID == model.EmployeeId && x.ProjectID == model.ProjectID
                                                                            orderby x.CreatedDate descending
                                                                            select x).FirstOrDefault();
                            if (allocationDetails != null)
                            {
                                model.ProjectEmployeeRoleID = allocationDetails.ProjectEmployeeRoleID;
                            }
                            else
                            {
                                resource.ErrorMessage = "This resource Allocation will not be updated as he is not yet allocated on this project.";
                                resource.CanAllocationDone = false;
                                resource.isAllocationDone = false;
                                canResourceAllocate = false;
                            }
                        }
                        if (canResourceAllocate == true)
                        {
                            tbl_PM_Project projectDetails = dbContext.tbl_PM_Project.Where(x => x.ProjectID == model.ProjectID).FirstOrDefault();
                            if (projectDetails != null)
                                if (projectDetails.ActualEndDate != null)
                                {
                                    if (model.AllocationStartDate < projectDetails.ActualStartDate || model.AllocationEndDate > projectDetails.ActualEndDate)
                                    {
                                        resource.ErrorMessage = "Please Select Allocation Start Date or allocation End Date in between Project StartDate And Project End Date.";
                                        resource.CanAllocationDone = false;
                                        resource.isAllocationDone = false;
                                    }
                                    else
                                    {
                                        bool isAllocate = true;
                                        if (buttonClick == "Add")
                                        {
                                            var NumberOfRows = dbContext.GetEmpResourceAllocationDetails_SP(EmployeeID, model.AllocationStartDate, model.AllocationEndDate, model.ProjectID);
                                            var count = NumberOfRows.AsEnumerable().ToList();
                                            int NumberOfrows = 0;
                                            foreach (var item in count)
                                            {
                                                NumberOfrows = item.NumberOfRow.HasValue ? item.NumberOfRow.Value : 0;
                                            }
                                            if (NumberOfrows > 0)
                                            {
                                                resource.ErrorMessage = "This resource is already allocated on this project for selected period.";
                                                resource.CanAllocationDone = false;
                                                resource.isAllocationDone = false;
                                                isAllocate = false;
                                            }
                                        }
                                        if (isAllocate == true)
                                        {
                                            dbContext.AddUpdateResource_SP(model.HelpdeskTicketID, model.ProjectID, EmployeeID, model.ReportingTo, model.ProjectRole, model.ResourceType, model.AllocationStartDate, model.AllocationEndDate, Convert.ToDecimal(model.AllocatedPercentage), model.Comments, buttonClick, userName, model.ProjectEmployeeRoleID, Output);
                                            status = Convert.ToBoolean(Output.Value);
                                            if (status == true)
                                            {
                                                if (buttonClick == "Add")
                                                    resource.ErrorMessage = "Resource added successfully.";
                                                else
                                                    resource.ErrorMessage = "Resource updated successfully.";
                                                resource.CanAllocationDone = true;
                                                resource.isAllocationDone = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool isAllocate = true;
                                    if (buttonClick == "Add")
                                    {
                                        var NumberOfRows = dbContext.GetEmpResourceAllocationDetails_SP(EmployeeID, model.AllocationStartDate, model.AllocationEndDate, model.ProjectID);
                                        var count = NumberOfRows.AsEnumerable().ToList();
                                        int NumberOfrows = 0;
                                        foreach (var item in count)
                                        {
                                            NumberOfrows = item.NumberOfRow.HasValue ? item.NumberOfRow.Value : 0;
                                        }
                                        if (NumberOfrows > 0)
                                        {
                                            resource.ErrorMessage = "This resource is already allocated on this project for selected period.";
                                            resource.CanAllocationDone = false;
                                            resource.isAllocationDone = false;
                                            isAllocate = false;
                                        }
                                    }
                                    if (isAllocate == true)
                                    {
                                        dbContext.AddUpdateResource_SP(model.HelpdeskTicketID, model.ProjectID, EmployeeID, model.ReportingTo, model.ProjectRole, model.ResourceType, model.AllocationStartDate, model.AllocationEndDate, Convert.ToDecimal(model.AllocatedPercentage), model.Comments, buttonClick, userName, model.ProjectEmployeeRoleID, Output);
                                        status = Convert.ToBoolean(Output.Value);
                                        if (status == true)
                                        {
                                            if (buttonClick == "Add")
                                                resource.ErrorMessage = "Resource added successfully.";
                                            else
                                                resource.ErrorMessage = "Resource updated successfully.";
                                            resource.CanAllocationDone = true;
                                            resource.isAllocationDone = true;
                                        }
                                    }
                                }
                        }
                    }
                }
                return resource;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public tbl_PM_ProjectEmployeeRole getEmployeeRoleDetails(int HelpDeskTicketID)
        {
            try
            {
                tbl_PM_ProjectEmployeeRole allocationDetails = dbContext.tbl_PM_ProjectEmployeeRole.Where(x => x.HelpdeskTicketID == HelpDeskTicketID).FirstOrDefault();
                return allocationDetails;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProjectEndAppraisalParameters> GetProjectEndAppraisalParametersList()
        {
            try
            {
                List<ProjectEndAppraisalParameters> para = new List<ProjectEndAppraisalParameters>();
                var param = dbContext.GetProjectEndAppraisalParameters_SP();
                para = (from p in param
                        select new ProjectEndAppraisalParameters
                        {
                            ProjectEndAppraisalParameterID = p.ProjectEndAppraisalParameterID,
                            ProjectEndAppraisalParameter = p.ProjectEndAppraisalParameter
                        }).ToList();

                return para;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ParameterRating GetMinMaxRating()
        {
            try
            {
                HRMSDBEntities db = new HRMSDBEntities();
                List<tbl_Appraisal_RatingMaster> upAppraisalRatingMasters = db.tbl_Appraisal_RatingMaster.OrderByDescending(x => x.Percentage).ToList();
                ParameterRating upraisalRating = new ParameterRating();
                if (upAppraisalRatingMasters.Count > 0)
                {
                    upraisalRating.MaxValue = Convert.ToInt32(upAppraisalRatingMasters[0].Percentage);
                    upraisalRating.MinValue = Convert.ToInt32(upAppraisalRatingMasters[upAppraisalRatingMasters.Count - 1].Percentage);
                }
                return upraisalRating;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<ProjectEndAppraisalParameters> GetProjectEndAppraisalParameters(int? EmployeeID, int? ProjectID, int? ProjectEndAppraisalStatusID)
        {
            List<ProjectEndAppraisalParameters> ParameterList = GetProjectEndAppraisalParametersList();
            List<ProjectEndAppraisalParameters> modelList = new List<ProjectEndAppraisalParameters>();

            foreach (var item in ParameterList)
            {
                ProjectEndAppraisalParameters parm = new ProjectEndAppraisalParameters();

                var endAppraisalDetails = dbContext.GetProjectEndAppraisalFormDetails_SP(EmployeeID, ProjectID, item.ProjectEndAppraisalParameterID);

                var details = (from r in endAppraisalDetails
                               select r).ToList();
                if (details.Count > 0)
                {
                    foreach (var d in details)
                    {
                        parm.ProjectEndAppraisalParameterID = d.ProjectEndAppraisalParameterID;
                        parm.ProjectEndAppraisalParameter = item.ProjectEndAppraisalParameter;
                        parm.ProjectEndAppraisalParameterRemarks = d.ProjectEndAppraisalParameterRemarks;
                        parm.ProjectEndAppraisalParameterRating = d.ProjectEndAppraisalParameterRating;
                        parm.ProjectEndAppraisalFormID = d.ProjectEndAppraisalFormID;
                        parm.ProjectLead = d.ProjectLead;
                    }
                }
                else
                {
                    parm.ProjectEndAppraisalParameterID = item.ProjectEndAppraisalParameterID;
                    parm.ProjectEndAppraisalParameter = item.ProjectEndAppraisalParameter;
                }
                modelList.Add(parm);
            }

            return modelList.ToList();
        }

        public bool SaveProjectEndAppraisalFormDetails(List<ProjectEndAppraisalParameters> model)
        {
            try
            {
                bool status = false;
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                if (model != null)
                {
                    int count = model.Count;

                    for (int m = 0; m < count; m++)
                    {
                        int FormID = model[m].ProjectEndAppraisalFormID;
                        //-----------------------
                        int ProjectEndAppraisalFormID = model[m].ProjectEndAppraisalFormID;
                        int EmployeeID = Convert.ToInt32(model[m].EmployeeID);
                        int ProjectID = Convert.ToInt32(model[m].ProjectID);
                        int ProjectEndAppraisalParameterID = model[m].ProjectEndAppraisalParameterID;
                        int ProjectEndAppraisalStatus = Convert.ToInt32(model[m].ProjectEndAppraisalFormStatus);
                        int ProjectEndAppraisalParameterRating = Convert.ToInt32(model[m].ProjectEndAppraisalParameterRating);
                        string ProjectEndAppraisalParameterRemarks = model[m].ProjectEndAppraisalParameterRemarks;
                        string ProjectLead = model[m].ProjectLead;
                        string LoggedinUserEmployeeCode = model[m].LoggedinUserEmployeeCode;
                        string Type;
                        string State = model[m].State;
                        DateTime? ReleaseDate = null;
                        int ProjectEmployeeRoleID = model[m].ProjectEmployeeRoleID;
                        if (State == "Submit")
                        {
                            //if (model[m].Releasedate != null)
                            //{
                            //    ReleaseDate = model[m].Releasedate;
                            //}

                            dbContext.UpdateProjectEmployeeRoleDetails_SP(ProjectEmployeeRoleID, ReleaseDate, ProjectEndAppraisalStatus, State);
                        }
                        //------------------
                        //tbl_PM_ProjectEndAppraisalFormDetails details = dbContext.tbl_PM_ProjectEndAppraisalFormDetails.Where(x => x.ProjectEndAppraisalFormID == FormID).FirstOrDefault();
                        var data = dbContext.GetProjectEndAppraisalFormDetails_SP(EmployeeID, ProjectID, ProjectEndAppraisalParameterID);
                        var details = (from r in data
                                       select r).ToList();
                        if (details.Count == 0)
                        {
                            Type = "ADD";
                            dbContext.AddUpdateProjectEndAppraisalDetails_SP(ProjectEndAppraisalFormID, EmployeeID, ProjectID, ProjectEndAppraisalParameterID, ProjectEndAppraisalStatus, ProjectEndAppraisalParameterRating,
                                ProjectEndAppraisalParameterRemarks, ProjectLead, LoggedinUserEmployeeCode, Type, State, Result);
                        }
                        else
                        {
                            Type = "UPDATE";
                            dbContext.AddUpdateProjectEndAppraisalDetails_SP(ProjectEndAppraisalFormID, EmployeeID, ProjectID, ProjectEndAppraisalParameterID, ProjectEndAppraisalStatus, ProjectEndAppraisalParameterRating,
                               ProjectEndAppraisalParameterRemarks, ProjectLead, LoggedinUserEmployeeCode, Type, State, Result);
                        }
                    }
                }
                status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AddEdirResourseModel> LoadReallocationeGridFromProjectID(int? ProjectID, int page, int rows, out int totalCount)
        {
            List<AddEdirResourseModel> reallocationRecords = new List<AddEdirResourseModel>();
            try
            {
                var reallocationEmployeeList = dbContext.SearchResourceAllocation_SP(ProjectID);

                reallocationRecords = (from r in reallocationEmployeeList
                                       select new AddEdirResourseModel
                                       {
                                           ProjectID = r.ProjectID,
                                           ProjectEmployeeRoleID = r.ProjectEmployeeRoleID,
                                           HelpdeskTicketID = r.HelpdeskTicketID.HasValue ? r.HelpdeskTicketID.Value : 0,
                                           ProjectCode = r.ProjectCode,
                                           ProjectName = r.ProjectName,
                                           EmployeeId = r.EmployeeID,
                                           EmployeeCode = r.Employeecode,
                                           EmployeeName = r.EmployeeName,
                                           Skills = r.Skills,
                                           DesignationName = r.DesignationName,
                                           ResourceType = r.ResourceType,
                                           EmployementStatus = r.EmploymentStatus,
                                           AllocationStartDate = Convert.ToDateTime(r.StartDate),
                                           AllocationEndDate = Convert.ToDateTime(r.EndDate),
                                           AllocatedPercentage = r.AllocatedPercentage,
                                           Comments = r.Comments,
                                           ProjectStartDate = r.projectStartDate,
                                           ProjectEndDate = r.projectEndDate
                                       }).ToList();

                totalCount = reallocationRecords.Count();
                return reallocationRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AddEdirResourseModel> LoadReallocationDetails(int? ProjectID)
        {
            List<AddEdirResourseModel> reallocationRecords = new List<AddEdirResourseModel>();
            try
            {
                var reallocationEmployeeList = dbContext.SearchResourceAllocation_SP(ProjectID);

                reallocationRecords = (from r in reallocationEmployeeList
                                       select new AddEdirResourseModel
                                       {
                                           ProjectID = r.ProjectID,
                                           ProjectEmployeeRoleID = r.ProjectEmployeeRoleID,
                                           ProjectCode = r.ProjectCode,
                                           ProjectName = r.ProjectName,
                                           EmployeeId = r.EmployeeID,
                                           EmployeeCode = r.Employeecode,
                                           EmployeeName = r.EmployeeName,
                                           Skills = r.Skills,
                                           DesignationName = r.DesignationName,
                                           ResourceType = r.ResourceType,
                                           EmployementStatus = r.EmploymentStatus,
                                           AllocationStartDate = Convert.ToDateTime(r.StartDate),
                                           AllocationEndDate = Convert.ToDateTime(r.EndDate),
                                           AllocatedPercentage = r.AllocatedPercentage,
                                           Comments = r.Comments,
                                           ProjectStartDate = r.projectStartDate,
                                           ProjectEndDate = r.projectEndDate
                                       }).ToList();

                return reallocationRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AddEdirResourseModel> LoadCurrentReallocationeGridFromEmployeeID(int EmployeeID, int page, int rows, out int totalCount)
        {
            List<AddEdirResourseModel> reallocationRecords = new List<AddEdirResourseModel>();
            try
            {
                var reallocationEmployeeList = dbContext.SearchCurrentUsersForProject_SP(EmployeeID);

                reallocationRecords = (from s in reallocationEmployeeList
                                       select new AddEdirResourseModel
                                       {
                                           HelpdeskTicketID = s.HelpdeskTicketID.HasValue ? s.HelpdeskTicketID.Value : 0,
                                           EmployeeId = Convert.ToInt32(s.employeecode),
                                           EmployeeName = s.EmployeeName,
                                           ReportingTo = s.ResourcePoolName,
                                           ResourcePoolName = s.ResourcePoolName,
                                           DesignationName = s.DesignationName,
                                           ProjectRole = s.DesignationName,
                                           ResourceType = s.ResourceStatus,
                                           EmployementStatus = s.EmploymentStatus,
                                           AllocationStartDate = Convert.ToDateTime(s.StartDate),
                                           AllocationEndDate = Convert.ToDateTime(s.EndDate),
                                           AllocatedPercentage = s.AllocatedPercentage,
                                           ReleaseDate = Convert.ToDateTime(s.EndDate),
                                           //ProjectEndAppraisalForm = s.EmployeeName,
                                           ProjectName = s.ProjectName,
                                           Comments = s.Comments
                                       }).ToList();

                totalCount = reallocationRecords.Count();
                return reallocationRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AddEdirResourseModel> LoadCurrentReallocationeHistoryGridFromEmployeeID(int EmployeeID, int page, int rows, out int totalCount)
        {
            List<AddEdirResourseModel> reallocationRecords = new List<AddEdirResourseModel>();
            try
            {
                var reallocationEmployeeList = dbContext.SearchResourceHis_SP(EmployeeID);

                reallocationRecords = (from s in reallocationEmployeeList
                                       select new AddEdirResourseModel
                                       {
                                           HelpdeskTicketID = s.HelpdeskTicketID.HasValue ? s.HelpdeskTicketID.Value : 0,
                                           EmployeeId = Convert.ToInt32(s.employeecode),
                                           EmployeeName = s.EmployeeName,
                                           ReportingTo = s.ResourcePoolName,
                                           ResourcePoolName = s.ResourcePoolName,
                                           DesignationName = s.DesignationName,
                                           ProjectRole = s.DesignationName,
                                           ResourceType = s.ResourceStatus,
                                           EmployementStatus = s.EmploymentStatus,
                                           AllocationStartDate = Convert.ToDateTime(s.StartDate),
                                           AllocationEndDate = Convert.ToDateTime(s.EndDate),
                                           AllocatedPercentage = s.AllocatedPercentage,
                                           ReleaseDate = Convert.ToDateTime(s.EndDate),
                                           //ProjectEndAppraisalForm = s.EmployeeName,
                                           ProjectName = s.ProjectName,
                                           Comments = s.Comments
                                       }).ToList();

                totalCount = reallocationRecords.Count();
                return reallocationRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveEmployeeReportingTo(string ProjectEmployeeRoleId, int ReportingTo)
        {
            bool isAdded = false;
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            int ProjectEmployeeRoleID = Convert.ToInt32(ProjectEmployeeRoleId);
            dbContext.UpdateEmployeeResorceReportingTo_SP(ProjectEmployeeRoleID, ReportingTo, Output);
            isAdded = Convert.ToBoolean(Output.Value);
            if (isAdded == true)
                isAdded = true;
            else
                isAdded = false;
            return isAdded;
        }

        public List<HRMS.Models.RMGViewPostModel.ResourcePoolList> GetResourcePoolList()
        {
            List<HRMS.Models.RMGViewPostModel.ResourcePoolList> ResourcePool = new List<HRMS.Models.RMGViewPostModel.ResourcePoolList>();
            try
            {
                var resourcePoolList = dbContext.sp_GetResourcePool();

                ResourcePool = (from s in resourcePoolList
                                select new HRMS.Models.RMGViewPostModel.ResourcePoolList
                                {
                                    ResourcePoolID = s.ResourcePoolID,
                                    ResourcePoolName = s.ResourcePoolName
                                }).ToList();

                return ResourcePool;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveEmployeeRole(RMGViewPostModel model, int RoleId, int ReportingTo)
        {
            tbl_PM_ProjectEmployeeRole emp = dbContext.tbl_PM_ProjectEmployeeRole.Where(ed => ed.ProjectEmployeeRoleID == model.ProjectEmployeeRoleID).FirstOrDefault();
            dbContext.UpdateEmployeeResorceAllocationDetails_SP(model.ProjectEmployeeRoleID, model.ReleaseDate, ReportingTo, RoleId);
            return true;
        }

        public bool UpdateReallocationOfResource(AddEdirResourseModel model)
        {
            try
            {
                bool isAdded = false;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                if (model.AllocationEndDate < model.AllocationOldEndDate || model.AllocationStartDate > model.AllocationEndDate || model.AllocationEndDate > model.ProjectEndDate)
                    isAdded = false;
                else
                {
                    dbContext.ReallocateResource_SP(model.ProjectEmployeeRoleID, model.AllocationEndDate, model.AllocationOldEndDate, model.HelpdeskTicketID, Output);
                    isAdded = Convert.ToBoolean(Output.Value);
                }
                return isAdded;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<int> SaveBulkAllocationRecords(List<AddEdirResourseModel> model)
        {
            try
            {
                string ProjectEmployeeRoleID = string.Empty;
                DateTime? EndDate = null;
                string EmployeeID = string.Empty;
                string allocationEndDate = string.Empty;
                string BulkReallocationDate = string.Empty;
                string allocationPercentage = string.Empty;
                List<int> NotReallocatedEmployees = new List<int>();
                int? HelpdeskIssueID = null;
                foreach (var item in model)
                {
                    if (item.BulkReallocationDate >= DateTime.Now && this.CheckBulkAllocation(item.ProjectID, item.BulkReallocationDate) == true && item.AllocationStartDate <= item.BulkReallocationDate)
                    {
                        ProjectEmployeeRoleID = ProjectEmployeeRoleID + "," + item.ProjectEmployeeRoleID;
                        EndDate = item.BulkReallocationDate;
                        EmployeeID = EmployeeID + "," + item.EmployeeId;
                        BulkReallocationDate = BulkReallocationDate + "," + item.BulkReallocationDate;
                        allocationPercentage = allocationPercentage + "," + item.AllocatedPercentage;
                        HelpdeskIssueID = item.HelpdeskTicketID;
                    }
                    else
                    {
                        NotReallocatedEmployees.Add(item.EmployeeId);
                    }
                }
                ProjectEmployeeRoleID = ProjectEmployeeRoleID.TrimStart(new char[] { ',' });
                EmployeeID = EmployeeID.TrimStart(new char[] { ',' });
                BulkReallocationDate = BulkReallocationDate.TrimStart(new char[] { ',' });
                allocationPercentage = allocationPercentage.TrimStart(new char[] { ',' });
                var BulkAllocation = dbContext.SimultaneousReallocateResource_SP(ProjectEmployeeRoleID, EndDate, EmployeeID, BulkReallocationDate, allocationPercentage, HelpdeskIssueID);
                List<AddEdirResourseModel> NotReallocatedEmployeeList = (from r in BulkAllocation
                                                                         select new AddEdirResourseModel
                                                                         {
                                                                             EmployeeId = Convert.ToInt32(r.NotAllowedEmployeeID)
                                                                         }).ToList();
                //idhar hai  na helpdesk id pass karna pdega. n waha jake save karna pdega.
                foreach (var item in NotReallocatedEmployeeList)
                {
                    NotReallocatedEmployees.Add(item.EmployeeId);
                }

                //foreach (var item in model)
                //{
                //    foreach (var Employee in NotReallocatedEmployees)
                //    {
                //        if (Employee == item.EmployeeId)
                //        {
                //        }
                //    }
                //}

                return NotReallocatedEmployees;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckBulkAllocation(int? projectID, DateTime? BulkReallocationDate)
        {
            bool status;
            tbl_PM_Project projectDetails = dbContext.tbl_PM_Project.Where(x => x.ProjectID == projectID).FirstOrDefault();
            if (projectDetails.ActualEndDate != null)
            {
                if (projectDetails.ActualEndDate >= BulkReallocationDate)
                    status = true;
                else
                    status = false;
            }
            else
            {
                status = true;
            }
            return status;
        }

        public tbl_PM_ProjectEmployeeRole GetProjectEmployeeRoleAllocationDetails(int ProjectEmployeeRoleID)
        {
            try
            {
                tbl_PM_ProjectEmployeeRole Details = dbContext.tbl_PM_ProjectEmployeeRole.Where(ed => ed.ProjectEmployeeRoleID == ProjectEmployeeRoleID).FirstOrDefault();
                return Details;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_PM_Employee_SEM GetEmployeeDetailsFromEmployeeID(int employeeId)
        {
            try
            {
                tbl_PM_Employee_SEM EmpDetails = dbContext.tbl_PM_Employee_SEM.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetEmployeeID(string employeeCode)
        {
            try
            {
                int employeeID = (from e in dbContext.tbl_PM_Employee_SEM
                                  where e.EmployeeCode == employeeCode
                                  select e.EmployeeID).FirstOrDefault();
                return employeeID;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PM_ProjectEmployeeRole GetResorcetDetails(int? employeeId, int? projectId)
        {
            try
            {
                tbl_PM_ProjectEmployeeRole CustDetails = dbContext.tbl_PM_ProjectEmployeeRole.Where(ed => ed.EmployeeID == employeeId && ed.ProjectID == projectId).OrderByDescending(ed => ed.ModifiedDate).FirstOrDefault();
                return CustDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_PM_Project GetResorcetProjectDetails(int? ProjectId)
        {
            try
            {
                tbl_PM_Project CustDetails = dbContext.tbl_PM_Project.Where(ed => ed.ProjectID == ProjectId).FirstOrDefault();
                return CustDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetAutoTriggerMailDetailsForResource()
        {
            try
            {
                string constring = GetADOConnectionString();
                SqlConnection con = new SqlConnection(constring);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetMailDetails";//write sp name here
                cmd.Connection = con;
                // string records = "Select PRE.projectId,proj.projectName,PRE.employeeId,convert(varchar(12), PRE.expectedEndDate, 101) AllocationEndDate,empEmployee.employeename EmpName,empEmployee.emailId EmpEMailId,empManager.employeename ManagerName,empManager.emailId ManagerEmailId from tbl_PM_ProjectEmployeeRole PRE left join Tbl_PM_Project proj on proj.projectId = PRE.projectId left join tbl_pm_employee empEmployee on empEmployee.employeeid = PRE.employeeId left join tbl_pm_employee empManager on empManager.employeeid = empEmployee.reportingTo where PRE.expectedEndDate = '" + sevenDaysBefore + "'";

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dsResourceDetails = new DataSet();
                da.Fill(dsResourceDetails);
                return dsResourceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetHelpDeskDetailsFromHelpDeskIssueID(int HelpDeskTicketIssueID)
        {
            try
            {
                string constring = GetADOConnectionString();
                SqlConnection con = new SqlConnection(constring);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetHelpDeskIssueDetailsForResourceAllocation_SP";//write sp name here
                cmd.Connection = con;
                // string records = "Select PRE.projectId,proj.projectName,PRE.employeeId,convert(varchar(12), PRE.expectedEndDate, 101) AllocationEndDate,empEmployee.employeename EmpName,empEmployee.emailId EmpEMailId,empManager.employeename ManagerName,empManager.emailId ManagerEmailId from tbl_PM_ProjectEmployeeRole PRE left join Tbl_PM_Project proj on proj.projectId = PRE.projectId left join tbl_pm_employee empEmployee on empEmployee.employeeid = PRE.employeeId left join tbl_pm_employee empManager on empManager.employeeid = empEmployee.reportingTo where PRE.expectedEndDate = '" + sevenDaysBefore + "'";

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dsResourceDetails = new DataSet();
                da.Fill(dsResourceDetails);
                return dsResourceDetails;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetADOConnectionString()
        {
            WSEMDBEntities ctx = new WSEMDBEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;
            return adoConnStr;
        }

        public List<PMSProjectDetailsViewModel> ProjectIRApproverDetailsRecord(int ProjectID, int page, int rows, out int totalCount)
        {
            try
            {
                List<PMSProjectDetailsViewModel> ProjectIRApproverDetails = new List<PMSProjectDetailsViewModel>();

                var ProjectDetails = dbContext.GetProjectIRApproverDetails_sp(ProjectID);
                //if (ProjectReviewerDetail.Count() > 0)
                //{
                ProjectIRApproverDetails = (from reviewerdetails in ProjectDetails
                                            select new PMSProjectDetailsViewModel
                                            {
                                                ProjectIRApproverId = reviewerdetails.Id,
                                                IRApproverProjectID = reviewerdetails.ProjectId,
                                                IRApproverEmployeeId = reviewerdetails.ApproverID,
                                                IRApproverEmployeeName = reviewerdetails.EmployeeName,
                                                IRApproverRoleDescription = reviewerdetails.RoleDescription
                                            }).ToList();
                //}
                totalCount = ProjectIRApproverDetails.Count();
                return ProjectIRApproverDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveProjectIRApproverDetail(PMSProjectDetailsViewModel model)
        {
            try
            {
                int projectId = model.IRApproverProjectID.HasValue ? model.IRApproverProjectID.Value : 0;
                int employeeId = Convert.ToInt32(model.IRApproverEmployeeId);
                int? roleId = model.IRApproverRoleId;
                int projReviewerId = 0;
                ObjectParameter ProjectIRApproverId = new ObjectParameter("ProjectIRApproverId", typeof(int));
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));

                if (model.ProjectIRApproverId != 0)
                {
                    //Customer = CusContantDetails.CustomerIds;
                    //dbContext.AddUpdateCustomerContacts_SP(Customer, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "UPDATE", Output, CustID);
                    projReviewerId = model.ProjectIRApproverId;
                    dbContext.AddUpdateIRApproverDetails_SP(projReviewerId, projectId, employeeId, roleId, "UPDATE", Result, ProjectIRApproverId);
                }
                else
                {
                    dbContext.AddUpdateIRApproverDetails_SP(projReviewerId, projectId, employeeId, roleId, "INSERT", Result, ProjectIRApproverId);
                    //dbContext.AddUpdateCustomerContacts_SP(Cusid, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "INSERT", Output, CustID);
                }
                bool status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteProjectIRApproverDetails(string[] SelectedProjectIRApproverId)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedProjectIRApproverId.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(string));
                    //int ModuleID = Convert.ToInt32(SelectedModuleId[i]);
                    int ProjIRApproverID = Convert.ToInt32(SelectedProjectIRApproverId[i]);
                    dbContext.DeleteProjectIRApproverDetails_sp(ProjIRApproverID, Output);
                    if (Output.Value.ToString() == "Success")
                        status = true;
                    else
                        status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PMSProjectDetailsViewModel> projectIRFinanceApproverDetails(int ProjectID, int page, int rows, out int totalCount)
        {
            try
            {
                List<PMSProjectDetailsViewModel> projectIRFinanceApproverDetails = new List<PMSProjectDetailsViewModel>();

                var ProjectDetails = dbContext.GetProjectIRFinanceApproverDetails_sp(ProjectID);
                //if (ProjectReviewerDetail.Count() > 0)
                //{
                projectIRFinanceApproverDetails = (from reviewerdetails in ProjectDetails
                                                   select new PMSProjectDetailsViewModel
                                                   {
                                                       ProjectIRFinanceApproverId = reviewerdetails.Id,
                                                       IRFinanceApproverProjectID = reviewerdetails.ProjectId,
                                                       IRFinanceApproverEmployeeId = reviewerdetails.FinanceApproverID,
                                                       IRFinanceApproverEmployeeName = reviewerdetails.EmployeeName,
                                                       IRFinanceApproverRoleDescription = reviewerdetails.RoleDescription
                                                   }).ToList();
                //}
                totalCount = projectIRFinanceApproverDetails.Count();
                return projectIRFinanceApproverDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveProjectIRFinanceApproverDetail(PMSProjectDetailsViewModel model)
        {
            try
            {
                int projectId = model.IRFinanceApproverProjectID.HasValue ? model.IRFinanceApproverProjectID.Value : 0;
                int employeeId = Convert.ToInt32(model.IRFinanceApproverEmployeeId);
                int? roleId = model.IRFinanceApproverRoleId;
                int projReviewerId = 0;
                ObjectParameter ProjectIRFinanceApproverId = new ObjectParameter("ProjectIRFinanceApproverId", typeof(int));
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));

                if (model.ProjectIRFinanceApproverId != 0)
                {
                    //Customer = CusContantDetails.CustomerIds;
                    //dbContext.AddUpdateCustomerContacts_SP(Customer, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "UPDATE", Output, CustID);
                    projReviewerId = model.ProjectIRFinanceApproverId;
                    dbContext.AddUpdateIRFinanceApproverDetails_SP(projReviewerId, projectId, employeeId, roleId, "UPDATE", Result, ProjectIRFinanceApproverId);
                }
                else
                {
                    dbContext.AddUpdateIRFinanceApproverDetails_SP(projReviewerId, projectId, employeeId, roleId, "INSERT", Result, ProjectIRFinanceApproverId);
                    //dbContext.AddUpdateCustomerContacts_SP(Cusid, CustomerName, ContactPerson, Mobile, EMailID, OnlineContact, Position, Fax, Phone, "INSERT", Output, CustID);
                }
                bool status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteProjectIRFinanceApproverDetails(string[] SelectedProjectIRFinanceApproverId)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedProjectIRFinanceApproverId.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(string));
                    //int ModuleID = Convert.ToInt32(SelectedModuleId[i]);
                    int ProjIRApproverID = Convert.ToInt32(SelectedProjectIRFinanceApproverId[i]);
                    dbContext.DeleteProjectIRFinanceApproverDetails_sp(ProjIRApproverID, Output);
                    if (Output.Value.ToString() == "Success")
                        status = true;
                    else
                        status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetSemEmployeeId(int Employeecode)
        {
            int EmployeeId = 0;
            var EmployeDetails = dbContext.GetSEMEmployeeId(Employeecode);
            List<RMGViewPostModel> Id = new List<RMGViewPostModel>();
            Id = (from m in EmployeDetails
                  select new RMGViewPostModel
                  {
                      EmployeeId = m.EmployeeID
                  }).ToList();
            EmployeeId = Convert.ToInt32(Id[0].EmployeeId);
            return EmployeeId;
        }

        public List<DateTime> getHolidayDateList()
        {
            List<DateTime> DateList = new List<DateTime>();

            var CurrentYearDates = dbContext.GetCurrentYearHolidayDates();
            List<PMSProjectDetailsViewModel> dates = new List<PMSProjectDetailsViewModel>();
            dates = (from m in CurrentYearDates
                     select new PMSProjectDetailsViewModel
                     {
                         Holidaydate = m.Holidaydate
                     }).ToList();

            foreach (var item in dates)
            {
                DateList.Add(item.Holidaydate);
            }

            return DateList;
        }

        public SearchedUserDetails GetEmployeeDetails(string UserName)
        {
            var employeeDetailsList = dbContext.GetEmployeeDetails_SP();
            SearchedUserDetails employeeDetail = (from e in employeeDetailsList
                                                  where e.UserName == UserName
                                                  select new SearchedUserDetails
                                                  {
                                                      EmployeeFullName = e.EmployeeName,
                                                      EmployeeId = e.EmployeeID,
                                                      EmployeeCode = e.EmployeeCode,
                                                      UserName = e.UserName,
                                                      EmployeeEmailId = e.EmailID
                                                  }).FirstOrDefault();
            return employeeDetail;
        }

        public bool AllocatedResourceOnProject(PMSProjectDetailsViewModel projectDetail)
        {
            bool status = false;
            //string[] roles = Roles.GetRolesForUser(Membership.GetUser().UserName);
            //if (roles.Contains("RMG"))
            //{
            //    status = true;
            //}
            //else
            //{
            //    if (projectDetail.PMSProjectStartDate >= DateTime.Now)
            //    {
            //        //This Tbl_Pm_Employee Details from WSEMDEMo DB
            tbl_PM_Project projectDetails = new tbl_PM_Project();
            projectDetails = GetPMSProjectDetails(projectDetail.ProjectID);
            SearchedUserDetails employeeDetail = GetEmployeeDetails(projectDetails.CreatedBy);

            string[] role = Roles.GetRolesForUser(employeeDetail.EmployeeCode);
            if (role.Contains("RMG"))
            {
                status = true;
            }
            else
            {
                //if (projectDetail.PMSProjectStartDate >= DateTime.Now)
                //{
                int j = 0;
                //This Tbl_Pm_Employee Details from HRMSDBEntities DB
                InvoiceDAL InvDal = new InvoiceDAL();
                SearchedUserDetails employeeDetailHrms = InvDal.GetEmployeeDetails(projectDetails.CreatedBy);
                int RolID = 0;
                int? ReportTo = 0;
                if (employeeDetailHrms != null)
                {
                    RolID = getRoleIdByDesription(employeeDetailHrms.Describtion);
                    ReportTo = employeeDetailHrms.repotingTo;
                }
                int? EmployeeID = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == employeeDetail.EmployeeCode).FirstOrDefault().EmployeeID;
                var allocationPercentage = dbContext.GetEmpResourceAllocationPercentage_SP(EmployeeID, projectDetail.PMSProjectStartDate, projectDetail.PMSProjectEndDate, Convert.ToString(RolID));
                List<AddEdirResourseModel> totalallocationPercentage = (from r in allocationPercentage
                                                                        select new AddEdirResourseModel
                                                                        {
                                                                            totalAllocation = r.AllocatedPercentageSUM
                                                                        }).ToList();
                List<RMGViewPostModel> ProjectRecords = new List<RMGViewPostModel>();

                int? ProjectID = projectDetail.ProjectID;
                DateTime? StartDate = projectDetail.PMSProjectStartDate;
                DateTime? EndDate = projectDetail.PMSProjectEndDate;
                double? AllocatedPercentage = 0;
                double? CurrentAllocatedPercentage = 0;
                AllocatedPercentage = Convert.ToDouble(totalallocationPercentage[0].totalAllocation);
                if (Convert.ToInt32(AllocatedPercentage) < 0 || AllocatedPercentage == 100.00)
                {
                    var MyCurrentallocatedProject = dbContext.SearchCurrentUsersForProject_SP(EmployeeID);
                    var EmployeeList = (from s in MyCurrentallocatedProject
                                        select new RMGViewPostModel
                                        {
                                            HelpDeskTicketID = s.HelpdeskTicketID,
                                            ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                            EmployeeId = s.EmployeeID,
                                            EmployeeCode = s.employeecode,
                                            EmployeeName = s.EmployeeName,
                                            ReportingTo = s.ResourcePoolName,
                                            ResourcePool = s.ResourcePoolName,
                                            Designation = s.DesignationName,
                                            ProjectRole = s.DesignationName,
                                            ResourceType = s.ResourceStatus,
                                            EmploymentStatus = s.EmploymentStatus,
                                            AllocationStartDate = s.StartDate,
                                            AllocationEndDate = s.EndDate,
                                            Allocated = Convert.ToDecimal(s.AllocatedPercentage),
                                            ReleaseDate = s.EndDate,
                                            ProjectEndAppraisalForm = s.EmployeeName,
                                            RMGComments = s.Comments,
                                            ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                        }).ToList();

                    ProjectRecords = (from s in EmployeeList
                                      orderby s.EmployeeCode descending
                                      select new RMGViewPostModel
                                      {
                                          HelpDeskTicketID = s.HelpDeskTicketID,
                                          ProjectEmployeeRoleID = s.ProjectEmployeeRoleID,
                                          EmployeeCode = s.EmployeeCode,
                                          EmployeeName = s.EmployeeName,
                                          ReportingTo = s.ReportingTo,
                                          ResourcePool = s.ResourcePool,
                                          Designation = s.Designation,
                                          ProjectRole = s.Designation,
                                          ResourceType = s.ResourceType,
                                          EmploymentStatus = s.EmploymentStatus,
                                          AllocationStartDate = s.AllocationStartDate,
                                          AllocationEndDate = s.AllocationEndDate,
                                          Allocated = s.Allocated,
                                          ReleaseDate = s.ReleaseDate,
                                          ProjectEndAppraisalForm = s.EmployeeName,
                                          RMGComments = s.RMGComments,
                                          ProjectEndAppraisalStausID = s.ProjectEndAppraisalStausID
                                      }).ToList();

                    for (int i = 0; i < ProjectRecords.Count; i++)
                    {
                        if (ProjectRecords[i].Allocated != 0)
                        {
                            j = i;
                            AllocatedPercentage = Convert.ToDouble(ProjectRecords[i].Allocated);
                            break;
                        }
                    }
                    AllocatedPercentage = AllocatedPercentage - 1; // Update the Previous allocation - 1 If 100% allocation (it become 99% allocated to previous project)
                    CurrentAllocatedPercentage = 1; // and set Current Project allocation 1%
                }
                else if (AllocatedPercentage == 0)
                {
                    CurrentAllocatedPercentage = 100; // for Current Project allocation
                }
                else if (AllocatedPercentage != 100)
                {
                    CurrentAllocatedPercentage = 100 - AllocatedPercentage; // for Current Project allocation
                }
                int? RoleID = RolID;

                // int? HelpDeskID = Convert.ToInt32(ProjectRecords[0].HelpDeskTicketID);

                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                if (projectDetail.ProjectID != 0)
                {
                    if (CurrentAllocatedPercentage == 1)
                    {
                        //int ProjectEmployeeID = EmployeeID.HasValue ? EmployeeID.Value : 0;
                        //dbContext.UpdateProjectAllocation_SP(ProjectEmployeeID, AllocatedPercentage, Result);
                        //bool UpdateStatus = Convert.ToBoolean(Result.Value);
                        dbContext.AllocateResourceAfterProjectApproved_SP(ProjectID, null, EmployeeID, RoleID, StartDate, EndDate, "T", CurrentAllocatedPercentage, DateTime.Now, ReportTo, false, false, "INSERT", Result);
                        bool insertStatus = Convert.ToBoolean(Result.Value);
                        //if (UpdateStatus == true && insertStatus == true)
                        if (insertStatus == true)
                            status = true;
                        else
                            status = false;
                        //Update Previous allocation
                    }
                    else
                    {
                        dbContext.AllocateResourceAfterProjectApproved_SP(ProjectID, null, EmployeeID, RoleID, StartDate, EndDate, "T", CurrentAllocatedPercentage, DateTime.Now, ReportTo, false, false, "INSERT", Result);
                        status = Convert.ToBoolean(Result.Value);
                    }
                }
                //}
                //else
                //{
                //    status = true;
                //}
            }

            //    }
            //    else
            //    {
            //        status = true;
            //    }
            //}

            return status;
        }

        public ResourseType GetResourceTypesByResourceId(int projectRole)
        {
            try
            {
                ResourseType resourceType = new ResourseType();
                var resourceTypelist = dbContext.GetResourceTypeByResourceId_SP(projectRole);
                resourceType = (from r in resourceTypelist
                                select new ResourseType
                                {
                                    ResourseTypeID = r.ResourceStatusID,
                                    ResourseTypeDescription = r.ResourceStatus
                                }).FirstOrDefault();
                return resourceType;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Role GetEmployeeRolesByRoleId(int roleId)
        {
            try
            {
                Role EmployeeRoleLists = new Role();
                var EmployeeRoles = dbContext.GetEmployeeRoleListByRoleId_SP(roleId);
                EmployeeRoleLists = (from roles in EmployeeRoles
                                     select new Role
                                     {
                                         RoleID = roles.roleid,
                                         RoleDescription = roles.roleDescription
                                     }).FirstOrDefault();
                return EmployeeRoleLists;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SearchedUserDetails GetEmployeeDetailsByName(string EmployeeName)
        {
            var employeeDetailsList = dbContext.GetEmployeeDetails_SP();
            SearchedUserDetails employeeDetail = (from e in employeeDetailsList
                                                  where e.EmployeeName == EmployeeName
                                                  select new SearchedUserDetails
                                                  {
                                                      EmployeeFullName = e.EmployeeName,
                                                      EmployeeId = e.EmployeeID,
                                                      EmployeeCode = e.EmployeeCode,
                                                      UserName = e.UserName,
                                                      EmployeeEmailId = e.EmailID
                                                  }).FirstOrDefault();
            return employeeDetail;
        }

        public List<DocumentSubCategoryDetails> GetSubCategory(int categoryId)
        {
            try
            {
                List<DocumentSubCategoryDetails> subcategoryList = new List<DocumentSubCategoryDetails>();
                var subcategorys = dbContext.GetSubcategories(categoryId);
                subcategoryList = (from e in subcategorys
                                   select new DocumentSubCategoryDetails
                                   {
                                       DocumentSubCategoryId = e.SubCategoryID,
                                       DocumentSubCategory = e.SubCategory
                                   }).ToList();
                return subcategoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmployeeIdFromEmployeeCodeSEM(int EmpCode)
        {
            try
            {
                var EmpCode1 = Convert.ToString(EmpCode);
                var SEMEmpId = (from SEMEmpDtls in dbContext.tbl_PM_Employee_SEM
                                where SEMEmpDtls.EmployeeCode == EmpCode1
                                select SEMEmpDtls.EmployeeID).FirstOrDefault();
                return SEMEmpId.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TimesheetModel.TaskList> getTaskName(int? ProjectID, int? EmployeeId)
        {
            List<TimesheetModel.TaskList> taskname = new List<TimesheetModel.TaskList>();
            var tasknames = dbContext.GetTaskName_SP(ProjectID, EmployeeId);
            taskname = (from r in tasknames
                        select new TimesheetModel.TaskList
                        {
                            ProjectTaskTypeId = r.ProjectTaskTypeId,
                            TaskName = r.TaskName,
                            ProjectID = r.ProjectId,
                            AssignedTo = r.AssignedTo,
                            AvgUnitTime = r.AvgUnitTime,
                            Description = r.Description
                        }).ToList();
            return taskname;
        }

        public DirectoryName GetDirectiveName(int? CategoryId, int? SubCategoryId, string mode)
        {
            try
            {
                DirectoryName directive = new DirectoryName();
                var directiveName = dbContext.GetDirectiveName(CategoryId, SubCategoryId, mode);
                directive = (from e in directiveName
                             select new DirectoryName
                             {
                                 DirectoryNames = e.DirectoryName
                             }).FirstOrDefault();
                return directive;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectAppList> ProjectNameForProject(string searchText, int employeeCode, int pageNo = 1, int pageSize = 10)
        {
            if (HttpContext.Current.User.IsInRole("RMG") || HttpContext.Current.User.IsInRole("PMO"))
            {
                var approvedProjectListsFinance = dbContext.GetProjectListForRMG_SP(employeeCode);
                List<ProjectAppList> projectList = (from project in approvedProjectListsFinance
                                                    where (project.projectName.ToLower().Contains(searchText.ToLower()))
                                                    orderby project.projectName ascending
                                                    select new ProjectAppList
                                                    {
                                                        Projectids = project.ProjectID,
                                                        ProjectName = project.projectName.Trim(),
                                                        ProjectStatus = project.ProjectStatus
                                                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                return projectList;
            }
            else
            {
                var approvedProjectListsFinance = dbContext.GetActiveProjectList_SP(employeeCode);
                List<ProjectAppList> projectList = (from project in approvedProjectListsFinance
                                                    where (project.projectName.ToLower().Contains(searchText.ToLower()))
                                                    orderby project.projectName ascending
                                                    select new ProjectAppList
                                                    {
                                                        Projectids = project.projectId,
                                                        ProjectName = project.projectName,
                                                        ProjectStatus = project.ProjectStatus
                                                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                return projectList;
            }
        }

        public bool ReleaseResource(int EmployeeId, int ProjectEmployeeRoleID, int Projectid)
        {
            dbContext.ReleaseResource_SP(ProjectEmployeeRoleID);
            return true;
        }
    }
}