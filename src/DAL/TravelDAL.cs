using HRMS.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;
using V2.CommonServices.Exceptions;

namespace HRMS.DAL
{
    public class TravelDAL
    {
        public HRMS_tbl_PM_Employee GetEmployeePersonalDetails(int employeeId)
        {
            try
            {
                HRMSDBEntities dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee PersonalDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                return PersonalDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmployeeUserName(int employeeId)
        {
            string UserName = string.Empty;
            try
            {
                HRMSDBEntities dbContext = new HRMSDBEntities();
                var username = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                UserName = username.UserName;
            }
            catch (Exception)
            {
                throw;
            }
            return UserName;
        }

        public int GetEmployeeStatusMasterID(int EmployeeID)
        {
            try
            {
                HRMSDBEntities dbContext = new HRMSDBEntities();
                var employeeStatus = dbContext.HRMS_tbl_PM_Employee.Where(e => e.EmployeeID == EmployeeID).FirstOrDefault();
                if (employeeStatus != null)
                {
                    int EmployeeStatusMasterID = employeeStatus.EmployeeStatusMasterID.HasValue ? employeeStatus.EmployeeStatusMasterID.Value : 0;
                    return EmployeeStatusMasterID;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<tbl_PM_EmployeeEmergencyContact> GetEmployeeEmergencyContactDetails(int employeeId)
        {
            try
            {
                HRMSDBEntities dbContext = new HRMSDBEntities();
                List<tbl_PM_EmployeeEmergencyContact> EmployeeContactDetailsList = (from contacts in dbContext.tbl_PM_EmployeeEmergencyContact
                                                                                    where contacts.EmployeeID == employeeId
                                                                                    select contacts).ToList();
                return EmployeeContactDetailsList.OrderBy(x => x.EmployeeEmergencyContactID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmergencyContactViewModel> GetEmployeeEmergencyContactDetails(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                HRMSDBEntities dbContext = new HRMSDBEntities();
                var EmergencyContactList = (from contact in dbContext.tbl_PM_EmployeeEmergencyContact
                                            where contact.EmployeeID == employeeId
                                            orderby contact.EmployeeID descending
                                            select new EmergencyContactViewModel
                                            {
                                                EmployeeId = contact.EmployeeID,
                                                EmployeeEmergencyContactId = contact.EmployeeEmergencyContactID,
                                                Name = contact.Name,
                                                EmailId = contact.EmailID,
                                                ContactNo = contact.ContactNo,
                                                EmgAddress = contact.Address,
                                                Relation = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                                            where relation.UniqueID == contact.RelationTypeID
                                                            select relation.RelationType).FirstOrDefault(),
                                                uniqueID = contact.RelationTypeID,
                                            }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from contact in dbContext.tbl_PM_EmployeeEmergencyContact
                              where contact.EmployeeID == employeeId
                              select contact.EmployeeID).Count();
                return EmergencyContactList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HRMSDBEntities dbContext = new HRMSDBEntities();

        public bool SaveAccomodationFormDetails(AccomodationViewModel empAccomodation, string decryptedTravelId)
        {
            bool isAdded = false;
            int Travelid = Convert.ToInt32(decryptedTravelId);
            tbl_HR_Travel_EmployeeTravelRequirement emp = dbContext.tbl_HR_Travel_EmployeeTravelRequirement.Where(ed => ed.TravelId == Travelid).FirstOrDefault();
            if (emp == null || emp.TravelRequirementsID <= 0)
            {
                tbl_HR_Travel_EmployeeTravelRequirement accomodation = new tbl_HR_Travel_EmployeeTravelRequirement();

                accomodation.TravelId = Travelid;
                accomodation.AccomodationMasterID = empAccomodation.AccomodationNeeded;
                accomodation.AirportPickupMasterID = empAccomodation.AirportPickUpNeeded;
                accomodation.LaptopMasterID = empAccomodation.LaptopNeeded;
                accomodation.HardDriveMasterID = empAccomodation.HardDriveNeeded;
                accomodation.UsbDriveMasterID = empAccomodation.USBDriveNeeded;
                accomodation.EmployeeSoftwaresRequirement = empAccomodation.SoftwaresNeeded;
                accomodation.RequirementAdditionalInformation = empAccomodation.AdditionalInformation;

                dbContext.tbl_HR_Travel_EmployeeTravelRequirement.AddObject(accomodation);
            }
            else
            {
                emp.AccomodationMasterID = empAccomodation.AccomodationNeeded;
                emp.AirportPickupMasterID = empAccomodation.AirportPickUpNeeded;
                emp.LaptopMasterID = empAccomodation.LaptopNeeded;
                emp.HardDriveMasterID = empAccomodation.HardDriveNeeded;
                emp.UsbDriveMasterID = empAccomodation.USBDriveNeeded;
                emp.EmployeeSoftwaresRequirement = empAccomodation.SoftwaresNeeded;
                emp.RequirementAdditionalInformation = empAccomodation.AdditionalInformation;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public int getTravelID(int employeeID)
        {
            try
            {
                int travelID = 0;
                Tbl_HR_Travel _HRTravel = dbContext.Tbl_HR_Travel.Where(x => x.EmployeeId == employeeID).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (_HRTravel != null)
                {
                    return travelID = _HRTravel.TravelId;
                }
                else
                {
                    return travelID = 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool SubmitTravelApprovalFormOrganizationHeads(AccomodationViewModel empTravel, int loginEmpId, int travelid, string userrole, string UserRolesOrgan, string UserRolesOrganSubmited)
        {
            bool isAdded = false;
            try
            {
                Tbl_HR_Travel latestTravelDetail = (from travel in dbContext.Tbl_HR_Travel
                                                    where travel.TravelId == travelid
                                                    orderby travel.CreatedDate descending
                                                    select travel).FirstOrDefault();

                int employeeId = Convert.ToInt32(latestTravelDetail.EmployeeId);

                tbl_HR_TravelStageEvent emp = new tbl_HR_TravelStageEvent();

                bool result;
                if (latestTravelDetail.TRFNo.Contains("."))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                if (result == false && loginEmpId == employeeId && (UserRolesOrgan == "Delivery Manager" || UserRolesOrgan == "Account Owners" || UserRolesOrgan == "Group Head"))
                {
                    emp.TravelId = travelid;
                    emp.EventDateTime = DateTime.Now;
                    emp.Action = "Approved";
                    emp.FromStageID = latestTravelDetail.StageID;
                    emp.ToStageID = latestTravelDetail.StageID + 1;
                    emp.UserID = loginEmpId;
                    dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                    dbContext.SaveChanges();
                    latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                }
                else if (result == true && loginEmpId == employeeId && (UserRolesOrgan == "Delivery Manager" || UserRolesOrgan == "Account Owners" || UserRolesOrgan == "Group Head"))
                {
                    emp.TravelId = travelid;
                    emp.EventDateTime = DateTime.Now;
                    emp.Action = "Approved";
                    emp.FromStageID = latestTravelDetail.StageID;
                    emp.ToStageID = latestTravelDetail.StageID + 1;
                    emp.UserID = loginEmpId;
                    dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                    dbContext.SaveChanges();
                    latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                }
                else if (result == true && loginEmpId == employeeId && UserRolesOrgan == "Management")
                {
                    emp.TravelId = travelid;
                    emp.EventDateTime = DateTime.Now;
                    emp.Action = "Approved";
                    emp.FromStageID = latestTravelDetail.StageID;
                    emp.ToStageID = 3;
                    emp.UserID = loginEmpId;
                    dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                    dbContext.SaveChanges();
                    latestTravelDetail.StageID = 3;
                }
                else if (result == false && loginEmpId == employeeId && UserRolesOrgan == "Management")
                {
                    emp.TravelId = travelid;
                    emp.EventDateTime = DateTime.Now;
                    emp.Action = "Approved";
                    emp.FromStageID = latestTravelDetail.StageID;
                    emp.ToStageID = 3;
                    emp.UserID = loginEmpId;
                    dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                    dbContext.SaveChanges();
                    latestTravelDetail.StageID = 3;
                }
                else
                {
                    if (userrole == "TravelApprover" && result == true && loginEmpId != employeeId && (UserRolesOrganSubmited == "Delivery Manager" || UserRolesOrganSubmited == "Account Owners" || UserRolesOrganSubmited == "Group Head"))
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = 3;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = 3;
                    }
                    if (userrole == "TravelApprover" && result == false && loginEmpId != employeeId && (UserRolesOrganSubmited == "Delivery Manager" || UserRolesOrganSubmited == "Account Owners" || UserRolesOrganSubmited == "Group Head"))
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = 3;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = 3;
                    }
                    if (userrole == "TravelApprover" && result == true && loginEmpId != employeeId && UserRolesOrganSubmited == "Management")
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = 3;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = 3;
                    }
                    if (userrole == "TravelApprover" && result == false && loginEmpId != employeeId && UserRolesOrganSubmited == "Management")
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = 3;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = 3;
                    }
                    if (loginEmpId != employeeId && result == false && userrole != "GroupHead" && userrole != "TravelApprover" && (UserRolesOrganSubmited == "Delivery Manager" || UserRolesOrganSubmited == "Account Owners" || UserRolesOrganSubmited == "Group Head"))
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = latestTravelDetail.StageID + 1;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                    }
                    if (loginEmpId != employeeId && result == true && userrole != "GroupHead" && userrole != "TravelApprover" && (UserRolesOrganSubmited == "Delivery Manager" || UserRolesOrganSubmited == "Account Owners" || UserRolesOrganSubmited == "Group Head"))
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = latestTravelDetail.StageID + 1;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                    }

                    if (loginEmpId != employeeId && result == false && userrole != "GroupHead" && userrole != "TravelApprover" && UserRolesOrganSubmited == "Management")
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = latestTravelDetail.StageID + 1;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                    }
                    if (loginEmpId != employeeId && result == true && userrole != "GroupHead" && userrole != "TravelApprover" && UserRolesOrganSubmited == "Management")
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = latestTravelDetail.StageID + 1;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                    }
                }

                dbContext.SaveChanges();

                isAdded = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        public List<ClientTravelAccomodationList> clientTravelYesNoList()
        {
            List<ClientTravelAccomodationList> clientAccomodation = new List<ClientTravelAccomodationList>();
            try
            {
                clientAccomodation = (from client in dbContext.Tbl_HR_Client_ReimbursementStatusMaster
                                      select new ClientTravelAccomodationList
                                      {
                                          ClientTraveAccomodationlsId = client.ID,
                                          ClientTraveAccomodationlsValue = client.Description
                                      }).ToList();
                return clientAccomodation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckForTheLogged(int employeeId, string decryptedTravelId)
        {
            var travelid = Convert.ToInt32(decryptedTravelId);
            var userExist = (from travel in dbContext.Tbl_HR_Travel
                             where travel.EmployeeId == employeeId
                             && travel.TravelId == travelid
                             select travel).FirstOrDefault();
            if (userExist != null)
                return true; //means the user has logged in himself
            else
                return false; //some other user has logged in
        }

        public bool SaveClientDetailForm(ClientViewModel clientDetails, string decryptedTravelId, int LocationId, int ClientId)
        {
            bool isAdded = false;
            bool isClientExist = false;
            int Travelid = Convert.ToInt32(decryptedTravelId);

            tbl_HR_Travel_ClientInformation emp = dbContext.tbl_HR_Travel_ClientInformation.Where(ed => ed.ClientId == clientDetails.ClientId).FirstOrDefault();

            if (emp == null || emp.ClientId <= 0)
            {
                List<tbl_HR_Travel_ClientInformation> info = dbContext.tbl_HR_Travel_ClientInformation.Where(ed => ed.TravelId == Travelid).ToList();
                foreach (tbl_HR_Travel_ClientInformation d in info)
                {
                    if (d.ClientName == ClientId)
                    {
                        isClientExist = true;
                    }
                }
                if (isClientExist == false)
                {
                    tbl_HR_Travel_ClientInformation clientInfo = new tbl_HR_Travel_ClientInformation();
                    if (LocationId == 4)
                    {
                        clientInfo.TravelId = Travelid;
                        clientInfo.ClientName = ClientId;
                        clientInfo.ClientContact = clientDetails.ClientContact;
                        clientInfo.ClientContactNumber = clientDetails.ClientContactNumber;
                        clientInfo.ClientAddress = clientDetails.ClientAddress;
                        clientInfo.ClientEmailID = clientDetails.ClientEmailId;
                        clientInfo.ClientVisitPurpose = clientDetails.PurposeOfVisit;
                        clientInfo.TravellingLocation = LocationId;
                        clientInfo.ClientLetterName = clientDetails.ClientInviteLetterName;
                        clientInfo.UploadPath = clientDetails.ClientIviteLetterFilePath;
                        clientInfo.ClientCreatedDate = DateTime.Now;
                        clientInfo.ClientProspectName = null;
                    }
                    else if (LocationId == 5)
                    {
                        clientInfo.TravelId = Travelid;
                        clientInfo.ClientProspectName = clientDetails.ProspectName;
                        clientInfo.ClientName = null;
                        clientInfo.ClientContact = null;
                        clientInfo.ClientContactNumber = null;
                        clientInfo.ClientAddress = null;
                        clientInfo.ClientEmailID = null;
                        clientInfo.ClientVisitPurpose = clientDetails.PurposeOfVisit;
                        clientInfo.TravellingLocation = LocationId;
                        clientInfo.ClientLetterName = null;
                        clientInfo.UploadPath = null;
                        clientInfo.ClientCreatedDate = DateTime.Now;
                    }
                    else
                    {
                        clientInfo.TravelId = Travelid;
                        clientInfo.ClientName = null;
                        clientInfo.ClientContact = null;
                        clientInfo.ClientContactNumber = null;
                        clientInfo.ClientAddress = null;
                        clientInfo.ClientEmailID = null;
                        clientInfo.ClientVisitPurpose = clientDetails.PurposeOfVisit;
                        clientInfo.TravellingLocation = LocationId;
                        clientInfo.ClientLetterName = null;
                        clientInfo.UploadPath = null;
                        clientInfo.ClientProspectName = null;
                        clientInfo.ClientCreatedDate = DateTime.Now;
                    }
                    dbContext.tbl_HR_Travel_ClientInformation.AddObject(clientInfo);
                }
                else
                {
                    return isAdded;
                }
            }
            else
            {
                if (LocationId == 4)
                {
                    emp.ClientName = ClientId;
                    emp.ClientContact = clientDetails.ClientContact;
                    emp.ClientContactNumber = clientDetails.ClientContactNumber;
                    emp.ClientAddress = clientDetails.ClientAddress;
                    emp.ClientEmailID = clientDetails.ClientEmailId;
                    emp.ClientVisitPurpose = clientDetails.PurposeOfVisit;
                    emp.TravellingLocation = LocationId;
                    if (!string.IsNullOrEmpty(clientDetails.ClientInviteLetterName))
                        emp.ClientLetterName = clientDetails.ClientInviteLetterName;
                    else
                        emp.ClientLetterName = emp.ClientLetterName;
                    if (!string.IsNullOrEmpty(clientDetails.ClientIviteLetterFilePath))
                        emp.UploadPath = clientDetails.ClientIviteLetterFilePath;
                    else
                        emp.UploadPath = emp.UploadPath;
                    emp.ClientModifiedDate = DateTime.Now;
                    emp.ClientProspectName = null;
                }
                else if (LocationId == 5)
                {
                    emp.ClientProspectName = clientDetails.ProspectName;
                    emp.ClientName = null;
                    emp.ClientContact = null;
                    emp.ClientContactNumber = null;
                    emp.ClientAddress = null;
                    emp.ClientEmailID = null;
                    emp.ClientVisitPurpose = clientDetails.PurposeOfVisit;
                    emp.TravellingLocation = LocationId;
                    emp.ClientLetterName = null;
                    emp.UploadPath = null;
                    emp.ClientCreatedDate = DateTime.Now;
                }
                else
                {
                    emp.ClientName = null;
                    emp.ClientContact = null;
                    emp.ClientContactNumber = null;
                    emp.ClientAddress = null;
                    emp.ClientEmailID = null;
                    emp.ClientVisitPurpose = clientDetails.PurposeOfVisit;
                    emp.TravellingLocation = LocationId;
                    emp.ClientLetterName = null;
                    emp.UploadPath = null;
                    emp.ClientModifiedDate = DateTime.Now;
                    emp.ClientProspectName = null;
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveClientUploadDetails(ClientViewModel model, int TravelId)
        {
            try
            {
                bool isAdded = false;
                tbl_HR_Travel_ClientUploadInformation clientDetails = dbContext.tbl_HR_Travel_ClientUploadInformation.Where(c => c.ClientId == model.ClientId && c.TravelId == TravelId).FirstOrDefault();
                if (clientDetails == null)
                {
                    tbl_HR_Travel_ClientUploadInformation clientUpload = new tbl_HR_Travel_ClientUploadInformation();
                    clientUpload.TravelId = TravelId;
                    clientUpload.ClientId = model.ClientId;
                    clientUpload.FileName = model.ClientInviteLetterName;
                    clientUpload.FilePath = model.ClientIviteLetterFilePath;
                    dbContext.tbl_HR_Travel_ClientUploadInformation.AddObject(clientUpload);
                    isAdded = true;
                }
                else
                {
                    clientDetails.TravelId = TravelId;
                    clientDetails.ClientId = model.ClientId;
                    clientDetails.FileName = model.ClientInviteLetterName;
                    clientDetails.FilePath = model.ClientIviteLetterFilePath;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClientViewModel> TravelDetailRecord(int decryptedTravelId, int page, int rows, out int totalCount)
        {
            List<ClientViewModel> clientRecords = new List<ClientViewModel>();
            try
            {
                clientRecords = (from client in dbContext.tbl_HR_Travel_ClientInformation
                                 join location in dbContext.tbl_HR_Travel_TravellingLocation on client.TravellingLocation equals location.TravelingLocationId into loc
                                 from locations in loc.DefaultIfEmpty()
                                 //join projectname in dbContext.tbl_Client_ProjectNamesMaster on client.ClientName equals projectname.ProjectNameID into proj
                                 //join projectname in dbSEMContext.tbl_PM_Customer on client.ClientName equals projectname.Customer into proj
                                 //from project in proj.DefaultIfEmpty()
                                 where client.TravelId == decryptedTravelId
                                 select new ClientViewModel
                                  {
                                      ClientId = client.ClientId,
                                      //ClientName = project.CustomerName,
                                      //ProjectNameId = project.Customer,
                                      ClientNameId = client.ClientName,
                                      ClientEmailId = client.ClientEmailID,
                                      ClientAddress = client.ClientAddress,
                                      ClientContactNumber = client.ClientContactNumber,
                                      ClientContact = client.ClientContact,
                                      PurposeOfVisit = client.ClientVisitPurpose,
                                      TravellingLocName = locations.TravellingLocationName,
                                      TravellingLocNameHidden = locations.TravellingLocationName,
                                      TravellingLocId = locations.TravelingLocationId,
                                      ClientInviteLetterName = client.ClientLetterName,
                                      ClientInviteLetterNameUpload = null,
                                      ClientIviteLetterFilePath = client.UploadPath,
                                      TravelId = client.TravelId,
                                      ProspectName = client.ClientProspectName
                                  }).ToList();

                foreach (var i in clientRecords)
                {
                    i.ProjectNameId = (from d in dbSEMContext.tbl_PM_Customer
                                       where d.Customer == i.ClientNameId
                                       select d.Customer).FirstOrDefault();

                    i.ClientName = (from f in dbSEMContext.tbl_PM_Customer
                                    where f.Customer == i.ClientNameId
                                    select f.CustomerName).FirstOrDefault();
                }

                totalCount = clientRecords.Count();
                return clientRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletedClientDetails(int clientID)
        {
            bool isDeleted = false;
            tbl_HR_Travel_ClientInformation clientInfo = dbContext.tbl_HR_Travel_ClientInformation.Where(cd => cd.ClientId == clientID).FirstOrDefault();
            if (clientID != null && clientID > 0)
            {
                dbContext.DeleteObject(clientInfo);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        private PMS3_HRMSDBEntities dbpms3Context = new PMS3_HRMSDBEntities();
        private WSEMDBEntities dbSEMContext = new WSEMDBEntities();
        private List<string> columnNameList = new List<string>();
        private List<string> FieldLabelList = new List<string>();
        private List<string> NewValueList = new List<string>();
        private V2toolsDBEntities dbv2toolsContext = new V2toolsDBEntities();
        private bool MemberInactive;

        // code added by Jitu-----Start

        public List<tbl_HR_VisaDetailsTravel> GetTravelVisaDetails(int travelId)
        {
            List<tbl_HR_VisaDetailsTravel> TravelVisaDetailsList = new List<tbl_HR_VisaDetailsTravel>();

            try
            {
                dbContext = new HRMSDBEntities();
                TravelVisaDetailsList = (from visadetails in dbContext.tbl_HR_VisaDetailsTravel
                                         where visadetails.ID == travelId
                                         select visadetails).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return TravelVisaDetailsList.OrderBy(x => x.ID).ToList();
        }

        public List<tbl_HR_TravelLocalConveyanceDetails> GetTravelConyanacesDetails(int travelId)
        {
            List<tbl_HR_TravelLocalConveyanceDetails> ConvaynanceDetailsList = new List<tbl_HR_TravelLocalConveyanceDetails>();

            try
            {
                dbContext = new HRMSDBEntities();
                ConvaynanceDetailsList = (from Convaydetails in dbContext.tbl_HR_TravelLocalConveyanceDetails
                                          where Convaydetails.TravelID == travelId
                                          select Convaydetails).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return ConvaynanceDetailsList.OrderBy(x => x.LocalConveyanceID).ToList();
        }

        public List<VisaViewModel> GetTravelVisaDetails(int travelId, int page, int rows, out int totalCount, string countryId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                Tbl_HR_Travel travelDetails = dbContext.Tbl_HR_Travel.Where(x => x.TravelId == travelId).FirstOrDefault();
                List<VisaViewModel> TravelVisaDetails = new List<VisaViewModel>();

                if (countryId == "0")
                {
                    TravelVisaDetails = (from visaDetails in dbContext.tbl_HR_VisaDetailsTravel
                                         where visaDetails.ID == travelId && visaDetails.CountryID == travelDetails.TravelToCountry
                                         orderby visaDetails.ID descending
                                         select new VisaViewModel
                                         {
                                             ID = visaDetails.ID,
                                             VisaTravelID = visaDetails.VisaTravelID,
                                             EmployeeId = dbContext.Tbl_HR_Travel.Where(x => x.TravelId == travelId).FirstOrDefault().EmployeeId,
                                             EmployeeVisaID = visaDetails.VisaTravelID,
                                             //IsValidVisa = visaDetails.IsValid.HasValue ? visaDetails.IsValid.Value : false,
                                             ValidTill = visaDetails.ToDate,
                                             SelectedCountryId = visaDetails.CountryID.HasValue ? visaDetails.CountryID.Value : 0,
                                             CountryID = visaDetails.CountryID.HasValue ? visaDetails.CountryID.Value : 0,
                                             CountryName = dbContext.tbl_PM_CountryMaster.Where(m => m.CountryID == visaDetails.CountryID).Select(m => m.CountryName).FirstOrDefault(),
                                             VisaTypeID = visaDetails.VisaTypeID.HasValue ? visaDetails.VisaTypeID.Value : 0,
                                             VisaTypeName = dbContext.tbl_PM_VisaTypeMaster.Where(m => m.VisaTypeID == visaDetails.VisaTypeID).Select(m => m.VisaType).FirstOrDefault(),
                                             IsVisaExpired = visaDetails.ToDate < DateTime.Now ? true : false,
                                             ToDate = visaDetails.ToDate,
                                             Decription = visaDetails.Decription,
                                             VisaFileName = visaDetails.VisaFileName,
                                             VisaFileNameUpload = null,
                                             VisaFilePath = visaDetails.VisaFilePath,
                                             StageID = travelDetails.StageID
                                         }).Skip((page - 1) * rows).Take(rows).ToList();
                }
                else
                {
                    int CountryId = Convert.ToInt32(countryId);

                    TravelVisaDetails = (from visaDetails in dbContext.tbl_HR_VisaDetailsTravel
                                         where visaDetails.ID == travelId && visaDetails.CountryID == CountryId
                                         orderby visaDetails.ID descending
                                         select new VisaViewModel
                                         {
                                             ID = visaDetails.ID,
                                             VisaTravelID = visaDetails.VisaTravelID,
                                             EmployeeId = dbContext.Tbl_HR_Travel.Where(x => x.TravelId == travelId).FirstOrDefault().EmployeeId,
                                             EmployeeVisaID = visaDetails.VisaTravelID,
                                             //IsValidVisa = visaDetails.IsValid.HasValue ? visaDetails.IsValid.Value : false,
                                             ValidTill = visaDetails.ToDate,
                                             SelectedCountryId = visaDetails.CountryID.HasValue ? visaDetails.CountryID.Value : 0,
                                             CountryID = CountryId,
                                             CountryName = dbContext.tbl_PM_CountryMaster.Where(m => m.CountryID == CountryId).Select(m => m.CountryName).FirstOrDefault(),
                                             VisaTypeID = visaDetails.VisaTypeID.HasValue ? visaDetails.VisaTypeID.Value : 0,
                                             VisaTypeName = dbContext.tbl_PM_VisaTypeMaster.Where(m => m.VisaTypeID == visaDetails.VisaTypeID).Select(m => m.VisaType).FirstOrDefault(),
                                             IsVisaExpired = visaDetails.ToDate < DateTime.Now ? true : false,
                                             ToDate = visaDetails.ToDate,
                                             Decription = visaDetails.Decription,
                                             VisaAddedStatus = visaDetails.IsAddedByAdmin,
                                             VisaFileName = visaDetails.VisaFileName,
                                             VisaFilePath = visaDetails.VisaFilePath,
                                             StageID = travelDetails.StageID
                                         }).Skip((page - 1) * rows).Take(rows).ToList();
                }

                totalCount = TravelVisaDetails.Count;

                return TravelVisaDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VisaViewModel GetTravelVisaShowHistory(int VisaTravelID)
        {
            try
            {
                VisaViewModel showHistory = new VisaViewModel();

                showHistory = (from empVisa in dbContext.tbl_HR_VisaDetailsTravel
                               where empVisa.VisaTravelID == VisaTravelID
                               select new VisaViewModel
                               {
                                   VisaFileName = empVisa.VisaFileName,
                                   VisaFilePath = empVisa.VisaFilePath,
                                   CreatedDate = empVisa.CreatedDate,
                                   VisaTravelID = empVisa.VisaTravelID,
                                   EmployeeId = empVisa.ID
                               }).FirstOrDefault();
                return showHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ConveyanceAdminViewModel> GetConvaynancesDetails(int travelId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ConveyanceAdminViewModel> TravelConvaynaceDetails = (from Convaynancedetails in dbContext.tbl_HR_TravelLocalConveyanceDetails
                                                                          join visatype in dbContext.tbl_HR_Travel_JourneyMode on Convaynancedetails.ConveyanceType equals visatype.JourneyModeID
                                                                          where Convaynancedetails.TravelID == travelId
                                                                          orderby Convaynancedetails.TravelID descending
                                                                          select new ConveyanceAdminViewModel
                                                                         {
                                                                             LocalConveyanceID = Convaynancedetails.LocalConveyanceID,
                                                                             TravelID = Convaynancedetails.TravelID,
                                                                             CityName = Convaynancedetails.City,
                                                                             ConveyanceType = visatype.JourneyModeID,
                                                                             TravelDetails = Convaynancedetails.TravelDetails,
                                                                             ConvayName = visatype.JourneyModeDescription,
                                                                             ConvayNameHidden = visatype.JourneyModeDescription,
                                                                             FromDate = Convaynancedetails.FromDate,
                                                                             AdditionalInformation = Convaynancedetails.AdditionalInformation,
                                                                             FromAddress = Convaynancedetails.Fromaddress,
                                                                             ToAddress = Convaynancedetails.Toaddress,
                                                                             ReservationNumber = Convaynancedetails.ReservationNumber,
                                                                             TravelingFrom = Convaynancedetails.TravelingFrom,
                                                                             HotelName = Convaynancedetails.HotelName,
                                                                             AirportName = Convaynancedetails.AirportName,
                                                                             AirporttoHotel = Convaynancedetails.AirporttoHotel
                                                                         }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from visadetails in dbContext.tbl_HR_TravelLocalConveyanceDetails
                              where visadetails.LocalConveyanceID == travelId
                              select visadetails.LocalConveyanceID).Count();

                return TravelConvaynaceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveTravelVisaDetails(VisaViewModel travelvisadetails, string decryptedTravelId, int? SelectedCountryID, int? SelectedVisaTypeID)
        {
            bool isAdded = false;
            int travelid = Convert.ToInt32(decryptedTravelId);

            tbl_HR_VisaDetailsTravel visa = dbContext.tbl_HR_VisaDetailsTravel.Where(ed => ed.VisaTravelID == travelvisadetails.VisaTravelID).FirstOrDefault();
            if (visa == null || visa.ID <= 0)
            {
                tbl_HR_VisaDetailsTravel visadetails = new tbl_HR_VisaDetailsTravel();
                visadetails.ID = travelid;
                visadetails.CountryID = SelectedCountryID;
                visadetails.VisaTypeID = SelectedVisaTypeID;
                visadetails.ToDate = travelvisadetails.ToDate;
                visadetails.Decription = travelvisadetails.Decription;
                visadetails.IsAddedByAdmin = 1;
                //if (isNoFileUploaded == false)
                //{
                visadetails.VisaFileName = travelvisadetails.VisaFileName;
                visadetails.VisaFilePath = travelvisadetails.VisaFilePath;
                //}
                visadetails.CreatedDate = DateTime.Now;
                dbContext.tbl_HR_VisaDetailsTravel.AddObject(visadetails);
            }
            else
            {
                if (travelvisadetails.FromDate == null && travelvisadetails.ToDate == null)
                {
                    visa.Decription = travelvisadetails.Decription;
                }
                else
                {
                    visa.ID = travelid;
                    visa.CountryID = SelectedCountryID;
                    visa.VisaTypeID = SelectedVisaTypeID;
                    visa.FromDate = travelvisadetails.FromDate;
                    visa.ToDate = travelvisadetails.ToDate;
                    visa.Decription = travelvisadetails.Decription;
                    visa.AdditionalInfo = travelvisadetails.AdditionalInfo;
                    //if (isNoFileUploaded == false)
                    //{
                    if (!string.IsNullOrEmpty(travelvisadetails.VisaFileName))
                        visa.VisaFileName = travelvisadetails.VisaFileName;
                    else
                        visa.VisaFileName = visa.VisaFileName;
                    if (!string.IsNullOrEmpty(travelvisadetails.VisaFilePath))
                        visa.VisaFilePath = travelvisadetails.VisaFilePath;
                    else
                        visa.VisaFilePath = visa.VisaFilePath;
                    //}
                    visa.ModifiedDate = DateTime.Now;
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveVisaUploadDetails(VisaViewModel model, int TravelId)
        {
            try
            {
                bool isAdded = false;
                tbl_HR_VisaUploadDetailsTravel visaDetails = dbContext.tbl_HR_VisaUploadDetailsTravel.Where(v => v.VisaID == model.VisaTravelID && v.TravelId == TravelId).FirstOrDefault();
                if (visaDetails == null)
                {
                    tbl_HR_VisaUploadDetailsTravel visaUpload = new tbl_HR_VisaUploadDetailsTravel();
                    visaUpload.TravelId = TravelId;
                    visaUpload.VisaID = model.VisaTravelID;
                    visaUpload.FileName = model.VisaFileName;
                    visaUpload.FilePath = model.VisaFilePath;
                    dbContext.tbl_HR_VisaUploadDetailsTravel.AddObject(visaUpload);
                    isAdded = true;
                }
                else
                {
                    visaDetails.TravelId = TravelId;
                    visaDetails.VisaID = model.VisaTravelID;
                    visaDetails.FileName = model.VisaFileName;
                    visaDetails.FilePath = model.VisaFilePath;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveConavanaceDetails(ConveyanceAdminViewModel travelConvaydetails, int travelid, int ConveyanceTypeId, string HotelToAirport)
        {
            bool isAdded = false;

            tbl_HR_TravelLocalConveyanceDetails Convay = dbContext.tbl_HR_TravelLocalConveyanceDetails.Where(ed => ed.LocalConveyanceID == travelConvaydetails.LocalConveyanceID).FirstOrDefault();
            if (Convay == null || Convay.LocalConveyanceID <= 0)
            {
                tbl_HR_TravelLocalConveyanceDetails Convayetails = new tbl_HR_TravelLocalConveyanceDetails();
                Convayetails.TravelID = travelid;
                Convayetails.ConveyanceType = ConveyanceTypeId;
                Convayetails.City = Convert.ToString(travelConvaydetails.City);
                Convayetails.TravelDetails = travelConvaydetails.TravelDetails;
                Convayetails.FromDate = travelConvaydetails.FromDate;
                Convayetails.Fromaddress = travelConvaydetails.FromAddress;
                Convayetails.Toaddress = travelConvaydetails.ToAddress;
                Convayetails.ReservationNumber = travelConvaydetails.ReservationNumber;
                if (HotelToAirport == "True")
                {
                    Convayetails.TravelingFrom = "Airport to Hotel";
                    Convayetails.AirporttoHotel = 1;
                }
                if (HotelToAirport == "False")
                {
                    Convayetails.TravelingFrom = "Hotel to Airport";
                    Convayetails.AirporttoHotel = 2;
                }
                Convayetails.AirportName = travelConvaydetails.AirportName;
                Convayetails.HotelName = travelConvaydetails.HotelName;

                dbContext.tbl_HR_TravelLocalConveyanceDetails.AddObject(Convayetails);
            }
            else
            {
                Convay.TravelID = travelid;
                Convay.ConveyanceType = ConveyanceTypeId;
                Convay.City = Convert.ToString(travelConvaydetails.City);
                Convay.TravelDetails = travelConvaydetails.TravelDetails;
                Convay.FromDate = travelConvaydetails.FromDate;
                Convay.Fromaddress = travelConvaydetails.FromAddress;
                Convay.Toaddress = travelConvaydetails.ToAddress;
                if (ConveyanceTypeId == 7)
                    Convay.ReservationNumber = travelConvaydetails.ReservationNumber;
                else
                    Convay.ReservationNumber = "";

                if (HotelToAirport == "True")
                {
                    Convay.TravelingFrom = "Airport to Hotel";
                    Convay.AirporttoHotel = 1;
                }
                else
                    Convay.HoteltoAirport = 0;

                if (HotelToAirport == "False")
                {
                    Convay.TravelingFrom = "Hotel to Airport";
                    Convay.AirporttoHotel = 2;
                }
                else
                    Convay.HoteltoAirport = 0;
                Convay.AirportName = travelConvaydetails.AirportName;
                Convay.HotelName = travelConvaydetails.HotelName;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteTravelVisaDetailsInfo(int travelID, int ID)
        {
            bool isDeleted = false;
            tbl_HR_VisaDetailsTravel TravelID = dbContext.tbl_HR_VisaDetailsTravel.Where(cd => cd.VisaTravelID == travelID && cd.ID == ID).FirstOrDefault();
            if (TravelID != null)
            {
                dbContext.DeleteObject(TravelID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool DeleteConvaynanceDetailsInfo(int ConvaynacetravelId, int travelId)
        {
            bool isDeleted = false;
            tbl_HR_TravelLocalConveyanceDetails TravelID = dbContext.tbl_HR_TravelLocalConveyanceDetails.Where(cd => cd.TravelID == travelId && cd.LocalConveyanceID == ConvaynacetravelId).FirstOrDefault();
            if (TravelID != null)
            {
                dbContext.DeleteObject(TravelID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public string GetcountyName(int? countryId)
        {
            var countryname = from country in dbContext.tbl_PM_CountryMaster
                              where country.CountryID == countryId
                              select country.CountryName;

            return countryname.ToString();
        }

        public int GetCityName(int? countryId)
        {
            int id = Convert.ToInt32(countryId);
            var countryname = dbContext.tbl_TMP_City.Where(cd => cd.CityID == id).FirstOrDefault();
            int cityid = countryname.CityID;

            return cityid;
        }

        public int GetConvayancedType(int? ConvaynancetypeId)
        {
            int converyid = 0;
            int cId = Convert.ToInt32(ConvaynancetypeId);
            var ConvaynanceTypeName = dbContext.tbl_HR_Travel_JourneyMode.Where(cd => cd.JourneyModeID == cId).FirstOrDefault();
            if (ConvaynanceTypeName != null)
                converyid = ConvaynanceTypeName.JourneyModeID;

            return converyid;
        }

        public string GetVisaType(int visatypeId)
        {
            var VisaTypeName = from visatype in dbContext.tbl_PM_VisaTypeMaster
                               where visatype.VisaTypeID == visatypeId
                               select visatype.VisaType;

            return VisaTypeName.ToString();
        }

        public List<Country> GetCountryDetails()
        {
            List<Country> conutries = new List<Country>();
            try
            {
                conutries = (from country in dbContext.tbl_PM_CountryMaster
                             orderby country.CountryName
                             select new Country
                             {
                                 CountryID = country.CountryID,
                                 CountryName = country.CountryName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        public List<CityT> GetCityDetails()
        {
            List<CityT> conutries = new List<CityT>();
            try
            {
                conutries = (from country in dbContext.tbl_TMP_City
                             orderby country.CityName
                             select new CityT
                             {
                                 CityID = country.CityID,
                                 CityName = country.CityName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        public List<VisaType> GetVisaTypeDetails()
        {
            List<VisaType> VisaType = new List<VisaType>();
            try
            {
                VisaType = (from visa in dbContext.tbl_PM_VisaTypeMaster
                            orderby visa.VisaType
                            select new VisaType
                            {
                                VisaTypeID = visa.VisaTypeID,
                                VisaTypeName = visa.VisaType
                            }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return VisaType;
        }

        public List<ConveyType> GetConaveyanceTypeDetails()
        {
            List<ConveyType> ConvaynaceType = new List<ConveyType>();
            try
            {
                ConvaynaceType = (from convay in dbContext.tbl_HR_Travel_JourneyMode
                                  orderby convay.JourneyModeDescription
                                  select new ConveyType
                                  {
                                      ConvayListID = convay.JourneyModeID,
                                      ConvayListName = convay.JourneyModeDescription
                                  }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return ConvaynaceType;
        }

        public List<tbl_HR_TravelOtherRequirement> GetTravelOtherRequirementDetails(int travelId)
        {
            List<tbl_HR_TravelOtherRequirement> TravelOtherRequirementDetailsList = new List<tbl_HR_TravelOtherRequirement>();

            try
            {
                dbContext = new HRMSDBEntities();
                TravelOtherRequirementDetailsList = (from otherdetails in dbContext.tbl_HR_TravelOtherRequirement
                                                     where otherdetails.ID == travelId
                                                     select otherdetails).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return TravelOtherRequirementDetailsList.OrderBy(x => x.ID).ToList();
        }

        public List<OtherAdminViewModel> GetTravelOtherRequirementDetails(int travelId, int page, int rows, out int totalCount)
        {
            try
            {
                List<EmployeeAcceptance> Acceptance = new List<EmployeeAcceptance>
               {
                   new EmployeeAcceptance   { AcceptanceID="",  ReceivedByEmployee=""},
                   new EmployeeAcceptance   { AcceptanceID="1",  ReceivedByEmployee="Yes"},
                   new EmployeeAcceptance{ AcceptanceID="2", ReceivedByEmployee="No"}
               };

                List<TCurrencyList> ListCur = new List<TCurrencyList>
            {
                new TCurrencyList {CurrencyID = 0, CurrencyName = ""},
            };

                ListCur.AddRange(GetCurrencyList());

                List<PaymentmodeList> PaymodeCash = new List<PaymentmodeList>
            {
                new PaymentmodeList {PaymentModeid = 0, PaymentmodeName = ""},
                new PaymentmodeList {PaymentModeid = 1, PaymentmodeName = "Cash"},
            };
                List<PaymentmodeList> PaymodeCard = new List<PaymentmodeList>
            {
                new PaymentmodeList {PaymentModeid = 0, PaymentmodeName = ""},
                new PaymentmodeList {PaymentModeid = 1, PaymentmodeName = "Card"},
            };

                dbContext = new HRMSDBEntities();
                List<OtherAdminViewModel> TravelOtherrRequirementDetails = (from otherrequirementType in dbContext.tbl_HR_TravelOtherRequirement.ToList()

                                                                            join acc in Acceptance on otherrequirementType.ReceivedByEmployee equals acc.AcceptanceID
                                                                            join cor in ListCur on otherrequirementType.Currency equals cor.CurrencyID
                                                                            join paymode in PaymodeCash on otherrequirementType.Cash equals paymode.PaymentModeid
                                                                            join paymodecard in PaymodeCard on otherrequirementType.Card equals paymodecard.PaymentModeid
                                                                            join Othertype in dbContext.tbl_HR_TravelOtherRequirementType on otherrequirementType.TypeID equals Othertype.RequrementTypeID
                                                                            where otherrequirementType.ID == travelId
                                                                            orderby otherrequirementType.ID descending
                                                                            select new OtherAdminViewModel
                                                                            {
                                                                                ID = otherrequirementType.ID,
                                                                                RequirementID = otherrequirementType.RequirementID,
                                                                                RequrementTypeID = Othertype.RequrementTypeID,
                                                                                Description = Othertype.Description,
                                                                                Miscdetails = otherrequirementType.Details,
                                                                                FileName = otherrequirementType.FileName,
                                                                                FilePath = otherrequirementType.FilePath,
                                                                                AcceptanceID = acc.AcceptanceID,
                                                                                ReceivedByEmployee = acc.ReceivedByEmployee,
                                                                                Advacesamount = otherrequirementType.Amount,
                                                                                CurrnyName = cor.CurrencyName,
                                                                                CurrencyID = cor.CurrencyID,
                                                                                PaymentMode = paymode.PaymentmodeName + "  " + paymodecard.PaymentmodeName,
                                                                                CardDetails = otherrequirementType.CardDetails,
                                                                                AmountOnCard = otherrequirementType.AmountOnCard,
                                                                                InsuranceFromDate = otherrequirementType.MInsuranceStartDate,
                                                                                InsuranceToDate = otherrequirementType.MInsuranceEndDate
                                                                            }).Skip((page - 1) * rows).Take(rows).ToList();
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                for (int i = 0; i < TravelOtherrRequirementDetails.Count; i++)
                {
                    string payMode = TravelOtherrRequirementDetails[i].PaymentMode.TrimEnd();
                    string[] payModeTypes = payMode.Split(symbols);
                    if (payModeTypes.Length == 3)
                    {
                        if (payModeTypes[0].Contains(PaymodeCash[1].PaymentmodeName) && payModeTypes[2].Contains(PaymodeCard[1].PaymentmodeName))
                            TravelOtherrRequirementDetails[i].PaymentMode = payModeTypes[0] + ", " + payModeTypes[2];
                    }
                }

                totalCount = (from otherdetails in dbContext.tbl_HR_TravelOtherRequirement
                              where otherdetails.ID == travelId
                              select otherdetails.ID).Count();

                return TravelOtherrRequirementDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveTravelOtherRequirementDetails(OtherAdminViewModel travelOtherRequirementdetails, int travelid)
        {
            bool isAdded = false;

            tbl_HR_TravelOtherRequirement otherdetilsa = dbContext.tbl_HR_TravelOtherRequirement.Where(ed => ed.RequirementID == travelOtherRequirementdetails.RequirementID).FirstOrDefault();
            if (otherdetilsa == null || otherdetilsa.ID <= 0)
            {
                tbl_HR_TravelOtherRequirement travelotherdetails = new tbl_HR_TravelOtherRequirement();
                travelotherdetails.RequirementID = Convert.ToInt32(travelOtherRequirementdetails.RequirementID);
                travelotherdetails.ID = travelid;
                travelotherdetails.Details = travelOtherRequirementdetails.Miscdetails;
                travelotherdetails.FileName = travelOtherRequirementdetails.FileName;
                travelotherdetails.FilePath = travelOtherRequirementdetails.FilePath;
                travelotherdetails.TypeID = travelOtherRequirementdetails.RequrementTypeID;
                travelotherdetails.MInsuranceStartDate = travelOtherRequirementdetails.InsuranceFromDate;
                travelotherdetails.MInsuranceEndDate = travelOtherRequirementdetails.InsuranceToDate;

                if (travelOtherRequirementdetails.CurrencyID != null)
                {
                    travelotherdetails.Currency = travelOtherRequirementdetails.CurrencyID;
                }
                else
                    travelotherdetails.Currency = 0;

                if (travelOtherRequirementdetails.AcceptanceID != null)
                {
                    travelotherdetails.ReceivedByEmployee = travelOtherRequirementdetails.AcceptanceID;
                }
                else
                    travelotherdetails.ReceivedByEmployee = "";

                if (travelOtherRequirementdetails.cash == true)
                {
                    travelotherdetails.Amount = travelOtherRequirementdetails.Advacesamount;
                    travelotherdetails.Cash = Convert.ToInt32(travelOtherRequirementdetails.cash);
                }
                else
                {
                    travelotherdetails.Amount = null;
                    travelotherdetails.Cash = 0;
                }
                if (travelOtherRequirementdetails.card == true)
                {
                    travelotherdetails.AmountOnCard = travelOtherRequirementdetails.AmountOnCard;
                    travelotherdetails.CardDetails = travelOtherRequirementdetails.CardDetails;
                    travelotherdetails.Card = Convert.ToInt32(travelOtherRequirementdetails.card);
                }
                else
                {
                    travelotherdetails.AmountOnCard = null;
                    travelotherdetails.Card = 0;
                    travelotherdetails.CardDetails = null;
                }

                dbContext.tbl_HR_TravelOtherRequirement.AddObject(travelotherdetails);
            }
            else
            {
                otherdetilsa.ID = travelid;
                otherdetilsa.RequirementID = travelOtherRequirementdetails.RequirementID;
                otherdetilsa.Details = travelOtherRequirementdetails.Miscdetails;

                if (otherdetilsa.MInsuranceStartDate != null)
                    otherdetilsa.MInsuranceStartDate = travelOtherRequirementdetails.InsuranceFromDate;

                if (otherdetilsa.MInsuranceEndDate != null)
                    otherdetilsa.MInsuranceEndDate = travelOtherRequirementdetails.InsuranceToDate;

                if (travelOtherRequirementdetails.FileName != null && travelOtherRequirementdetails.FilePath != null)
                {
                    otherdetilsa.FileName = travelOtherRequirementdetails.FileName;
                    otherdetilsa.FilePath = travelOtherRequirementdetails.FilePath;
                }
                if (travelOtherRequirementdetails.AcceptanceID != null)
                {
                    otherdetilsa.ReceivedByEmployee = travelOtherRequirementdetails.AcceptanceID;
                }
                else
                    otherdetilsa.ReceivedByEmployee = "";

                if (travelOtherRequirementdetails.CurrencyID != null && travelOtherRequirementdetails.RequrementTypeID == 4)
                {
                    otherdetilsa.Currency = travelOtherRequirementdetails.CurrencyID;
                }
                else
                    otherdetilsa.Currency = 0;

                if (travelOtherRequirementdetails.RequrementTypeID == 4)
                    otherdetilsa.Amount = travelOtherRequirementdetails.Advacesamount;
                else
                    otherdetilsa.Amount = null;

                if (travelOtherRequirementdetails.cash == true)
                {
                    otherdetilsa.Amount = travelOtherRequirementdetails.Advacesamount;
                    otherdetilsa.Cash = Convert.ToInt32(travelOtherRequirementdetails.cash);
                }
                else
                {
                    otherdetilsa.Amount = null;
                    otherdetilsa.Cash = 0;
                }
                if (travelOtherRequirementdetails.card == true)
                {
                    otherdetilsa.AmountOnCard = travelOtherRequirementdetails.AmountOnCard;
                    otherdetilsa.CardDetails = travelOtherRequirementdetails.CardDetails;
                    otherdetilsa.Card = Convert.ToInt32(travelOtherRequirementdetails.card);
                }
                else
                {
                    otherdetilsa.AmountOnCard = null;
                    otherdetilsa.Card = 0;
                    otherdetilsa.CardDetails = null;
                }

                otherdetilsa.TypeID = travelOtherRequirementdetails.RequrementTypeID;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteTravelOtherRequirementDetailsInfo(int travelID, int ID)
        {
            bool isDeleted = false;
            tbl_HR_TravelOtherRequirement TravelID = dbContext.tbl_HR_TravelOtherRequirement.Where(cd => cd.RequirementID == travelID && cd.ID == ID).FirstOrDefault();
            if (TravelID != null)
            {
                dbContext.DeleteObject(TravelID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public string GetRequirementTypeName(int TypeID)
        {
            var typename = from type in dbContext.tbl_HR_TravelOtherRequirementType
                           where type.RequrementTypeID == TypeID
                           select type.Description;

            return typename.ToString();
        }

        public List<RequirementType> GetRequiremetTypeDetails()
        {
            List<RequirementType> conutries = new List<RequirementType>();
            try
            {
                conutries = (from type in dbContext.tbl_HR_TravelOtherRequirementType
                             orderby type.Description
                             select new RequirementType
                             {
                                 RequrementTypeID = type.RequrementTypeID,
                                 Description = type.Description
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        public List<EmployeeAcceptance> GetAcceptanceStatus()
        {
            try
            {
                List<EmployeeAcceptance> Acceptance = new List<EmployeeAcceptance>
               {
                   new EmployeeAcceptance   { AcceptanceID="1",  ReceivedByEmployee="Yes"},
                   new EmployeeAcceptance{ AcceptanceID="2", ReceivedByEmployee="No"}
               };

                return Acceptance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_TravelAccomodationDetails GetAccomodationAdminDetails(int TravelId)
        {
            try
            {
                tbl_HR_TravelAccomodationDetails travelDetails = dbContext.tbl_HR_TravelAccomodationDetails.Where(travel => travel.TravelId == TravelId).FirstOrDefault();
                return travelDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_VisaUploadDetailsTravel GetVisaUploadDetails(int? TravelId, int? VisaId)
        {
            try
            {
                tbl_HR_VisaUploadDetailsTravel visaDetails = dbContext.tbl_HR_VisaUploadDetailsTravel.Where(v => v.TravelId == TravelId && v.VisaID == VisaId).FirstOrDefault();
                return visaDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_TravelAccomodationUploadDetails GetAccomodationAdminUploadDetails(int? TravelId, int? AccomodationID)
        {
            try
            {
                tbl_HR_TravelAccomodationUploadDetails accomodationDetails = dbContext.tbl_HR_TravelAccomodationUploadDetails.Where(accomodation => accomodation.TravelId == TravelId && accomodation.AccomodationId == AccomodationID).FirstOrDefault();
                return accomodationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_Travel_ClientUploadInformation GetClientUploadDetails(int? TravelId, int? ClientId)
        {
            try
            {
                tbl_HR_Travel_ClientUploadInformation clientDetails = dbContext.tbl_HR_Travel_ClientUploadInformation.Where(c => c.TravelId == TravelId && c.ClientId == ClientId).FirstOrDefault();
                return clientDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_TravelJourneyUploadDetails GetJourneyUploadDetails(int? TravelId, int? JourneyID)
        {
            try
            {
                Tbl_HR_TravelJourneyUploadDetails journeyDetails = dbContext.Tbl_HR_TravelJourneyUploadDetails.Where(j => j.TravelId == TravelId && j.JourneyId == JourneyID).FirstOrDefault();
                return journeyDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAccoDetailsAdditionalInformation(int TravelId)
        {
            try
            {
                string additionalInfo = (from addInfo in dbContext.Tbl_HR_Travel
                                         where addInfo.TravelId == TravelId
                                         select addInfo.AccoDetailsAdditionalInformation).FirstOrDefault();
                return additionalInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AccomodationAdmin> GetAccomodationAdminGrid(int page, int rows, int travelid, out int totalCount)
        {
            try
            {
                List<AccomodationAdmin> accomodationAdminList = (from accomodation in dbContext.tbl_HR_TravelAccomodationDetails
                                                                 where accomodation.TravelId == travelid
                                                                 select new AccomodationAdmin
                                                                 {
                                                                     AccomodationID = accomodation.AccommodationID,
                                                                     TravelId = travelid,
                                                                     HotelName = accomodation.HotelName,
                                                                     HotelAddress = accomodation.HotelAddress,
                                                                     HotelContactNumber = accomodation.HotelContactNumber,
                                                                     BookingFromDate = accomodation.BookingFromDate,
                                                                     BookingToDate = accomodation.BookingToDate,
                                                                     FileName = accomodation.FileName,
                                                                     FilePath = accomodation.FilePath,
                                                                     FileUpload = null,
                                                                     AdditionalDetails = accomodation.AdditionalDetails
                                                                 }).ToList();
                totalCount = accomodationAdminList.Count();

                return accomodationAdminList.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddAdminAccomodationDetails(AccomodationAdmin model, int travelid)
        {
            try
            {
                bool isAdded = false;
                tbl_HR_TravelAccomodationDetails AdminAccomodationDetails = dbContext.tbl_HR_TravelAccomodationDetails.Where(a => a.AccommodationID == model.AccomodationID).FirstOrDefault();
                if (AdminAccomodationDetails == null)
                {
                    tbl_HR_TravelAccomodationDetails AdminAccomodation = new tbl_HR_TravelAccomodationDetails();
                    AdminAccomodation.TravelId = travelid;
                    AdminAccomodation.HotelName = model.HotelName.Trim();
                    AdminAccomodation.HotelAddress = model.HotelAddress.Trim();
                    AdminAccomodation.HotelContactNumber = model.HotelContactNumber.Trim();
                    AdminAccomodation.BookingFromDate = model.BookingFromDate;
                    AdminAccomodation.BookingToDate = model.BookingToDate;

                    //if (model.FileName != null && model.FileName != "")
                    //    AdminAccomodation.FileName = model.FileName.Trim();
                    //else
                    //    AdminAccomodation.FileName = "";

                    //if (model.FilePath != null && model.FilePath != "")
                    //    AdminAccomodation.FilePath = model.FilePath.Trim();
                    //else
                    //    AdminAccomodation.FilePath = "";
                    ////
                    if (!string.IsNullOrEmpty(model.FileName))
                        AdminAccomodation.FileName = model.FileName.Trim();
                    else
                        //AdminAccomodation.FileName = AdminAccomodationDetails.FileName;
                        AdminAccomodation.FileName = String.Empty;

                    if (!string.IsNullOrEmpty(model.FileName) && !string.IsNullOrEmpty(model.FilePath))
                        AdminAccomodation.FilePath = model.FilePath.Trim();
                    else
                        //AdminAccomodation.FilePath = AdminAccomodationDetails.FilePath;
                        AdminAccomodation.FilePath = String.Empty;

                    if (model.AdditionalDetails != null && model.AdditionalDetails != "")
                        AdminAccomodation.AdditionalDetails = model.AdditionalDetails.Trim();
                    else
                        AdminAccomodation.AdditionalDetails = model.AdditionalDetails;
                    AdminAccomodation.CreatedDate = DateTime.Now;
                    dbContext.tbl_HR_TravelAccomodationDetails.AddObject(AdminAccomodation);
                    isAdded = true;
                }
                else
                {
                    AdminAccomodationDetails.TravelId = travelid;
                    AdminAccomodationDetails.HotelName = model.HotelName.Trim();
                    AdminAccomodationDetails.HotelAddress = model.HotelAddress.Trim();
                    AdminAccomodationDetails.HotelContactNumber = model.HotelContactNumber.Trim();
                    AdminAccomodationDetails.BookingFromDate = model.BookingFromDate;
                    AdminAccomodationDetails.BookingToDate = model.BookingToDate;

                    //if (model.FileName != null && model.FileName != "")
                    //    AdminAccomodationDetails.FileName = model.FileName.Trim();
                    //else
                    //    AdminAccomodationDetails.FileName = "";

                    //if (model.FilePath != null && model.FilePath != "")
                    //    AdminAccomodationDetails.FilePath = model.FilePath.Trim();
                    //else
                    //    AdminAccomodationDetails.FilePath = "";
                    if (!string.IsNullOrEmpty(model.FileName))
                        AdminAccomodationDetails.FileName = model.FileName.Trim();
                    else
                        AdminAccomodationDetails.FileName = AdminAccomodationDetails.FileName;

                    if (!string.IsNullOrEmpty(model.FileName) && !string.IsNullOrEmpty(model.FilePath))
                        AdminAccomodationDetails.FilePath = model.FilePath.Trim();
                    else
                        AdminAccomodationDetails.FilePath = AdminAccomodationDetails.FilePath;

                    if (model.AdditionalDetails != null && model.AdditionalDetails != "")
                        AdminAccomodationDetails.AdditionalDetails = model.AdditionalDetails.Trim();
                    else
                        AdminAccomodationDetails.AdditionalDetails = model.AdditionalDetails;
                    AdminAccomodationDetails.ModifiedDate = DateTime.Now;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddAdminAccomodationUploadDetails(AccomodationAdmin model)
        {
            try
            {
                bool isAdded = false;
                tbl_HR_TravelAccomodationUploadDetails AdminAccomodationUploadDetails = dbContext.tbl_HR_TravelAccomodationUploadDetails.Where(a => a.AccomodationId == model.AccomodationID && a.TravelId == model.TravelId).FirstOrDefault();
                if (AdminAccomodationUploadDetails == null)
                {
                    tbl_HR_TravelAccomodationUploadDetails AdminAccomodationUpload = new tbl_HR_TravelAccomodationUploadDetails();
                    AdminAccomodationUpload.TravelId = model.TravelId;
                    AdminAccomodationUpload.AccomodationId = model.AccomodationID;
                    AdminAccomodationUpload.FileName = model.FileName;
                    AdminAccomodationUpload.FilePath = model.FilePath;
                    dbContext.tbl_HR_TravelAccomodationUploadDetails.AddObject(AdminAccomodationUpload);
                    isAdded = true;
                }
                else
                {
                    AdminAccomodationUploadDetails.TravelId = model.TravelId;
                    AdminAccomodationUploadDetails.AccomodationId = model.AccomodationID;
                    AdminAccomodationUploadDetails.FileName = model.FileName;
                    AdminAccomodationUploadDetails.FilePath = model.FilePath;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAdminAccomodationDetails(int AccomodationID, int TravelID)
        {
            try
            {
                bool isDeleted = false;
                tbl_HR_TravelAccomodationDetails accomodationDetails = dbContext.tbl_HR_TravelAccomodationDetails.Where(ad => ad.AccommodationID == AccomodationID && ad.TravelId == TravelID).FirstOrDefault();
                if (accomodationDetails != null)
                {
                    dbContext.tbl_HR_TravelAccomodationDetails.DeleteObject(accomodationDetails);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAdminAccomodationUploadRecord(int AccomodationID, int TravelID)
        {
            try
            {
                bool isDeleted = false;
                tbl_HR_TravelAccomodationUploadDetails accomodationDetails = dbContext.tbl_HR_TravelAccomodationUploadDetails.Where(ad => ad.AccomodationId == AccomodationID && ad.TravelId == TravelID).FirstOrDefault();
                if (accomodationDetails != null)
                {
                    dbContext.tbl_HR_TravelAccomodationUploadDetails.DeleteObject(accomodationDetails);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteVisaUploadRecord(int TravelID, int VisaId)
        {
            try
            {
                bool isDeleted = false;
                tbl_HR_VisaUploadDetailsTravel visaDetails = dbContext.tbl_HR_VisaUploadDetailsTravel.Where(v => v.TravelId == TravelID && v.VisaID == VisaId).FirstOrDefault();
                if (visaDetails != null)
                {
                    dbContext.tbl_HR_VisaUploadDetailsTravel.DeleteObject(visaDetails);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteJourneyUploadRecord(int TravelID, int JourneyId)
        {
            try
            {
                bool isDeleted = false;
                Tbl_HR_TravelJourneyUploadDetails journeyDetails = dbContext.Tbl_HR_TravelJourneyUploadDetails.Where(j => j.TravelId == TravelID && j.JourneyId == JourneyId).FirstOrDefault();
                if (journeyDetails != null)
                {
                    dbContext.Tbl_HR_TravelJourneyUploadDetails.DeleteObject(journeyDetails);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteClientUploadRecord(int TravelID, int ClientId)
        {
            try
            {
                bool isDeleted = false;
                tbl_HR_Travel_ClientUploadInformation clientDetails = dbContext.tbl_HR_Travel_ClientUploadInformation.Where(c => c.TravelId == TravelID && c.ClientId == ClientId).FirstOrDefault();
                if (clientDetails != null)
                {
                    dbContext.tbl_HR_Travel_ClientUploadInformation.DeleteObject(clientDetails);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveClientForm(ClientViewModel clientDetails, string decryptedTravelId)
        {
            bool isAdded = false;
            int travelid = Convert.ToInt32(decryptedTravelId);

            Tbl_HR_Travel emp = dbContext.Tbl_HR_Travel.Where(ed => ed.TravelId == travelid).FirstOrDefault();
            if (emp != null || emp.TravelId >= 0)
            {
                emp.ClientAdditionalInformation = clientDetails.AdditionalInfo;
                dbContext.SaveChanges();
                isAdded = true;
            }
            return isAdded;
        }

        public PassportViewModel getPassportDetails(int employeeID)
        {
            try
            {
                PassportViewModel model = (from employee in dbContext.HRMS_tbl_PM_Employee
                                           where employee.EmployeeID == employeeID
                                           select new PassportViewModel
                                           {
                                               PassportNumber = employee.PassportNumber,
                                               SonofWifeOfDaughterof = employee.PP_RelativeName,
                                               DateOfIssue = employee.PP_DateOfIssue,
                                               PlaceOfIssue = employee.PP_PlaceOfIssue,
                                               DateOfExpiry = employee.PP_ExpiryDate,
                                               NumberOfPagesLeft = employee.NoofPagesLeft,
                                               FullNameAsInPassport = employee.PP_FullName
                                           }).FirstOrDefault();
                return model;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool SavePassportDetails(PassportViewModel model)
        {
            try
            {
                bool status = false;
                tbl_HR_Travel_PassportDocument _Travel_PassportDocument = new tbl_HR_Travel_PassportDocument();
                _Travel_PassportDocument.TravelID = model.TravelID;
                _Travel_PassportDocument.PassportFileName = model.PassportFileName;
                _Travel_PassportDocument.PassportFilePath = model.PassportFilePath;
                _Travel_PassportDocument.CreatedDate = DateTime.Now;
                dbContext.tbl_HR_Travel_PassportDocument.AddObject(_Travel_PassportDocument);
                dbContext.SaveChanges();
                status = true;
                return status;
            }
            catch
            {
                throw;
            }
        }

        public List<PassportViewModel> GetPassportDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            List<PassportViewModel> passportDetails = (from p in dbContext.Tbl_PM_EmployeePassport
                                                       where p.EmployeeID == EmployeeID
                                                       select new PassportViewModel
                                                       {
                                                           PassportFileName = p.PassportFileName,
                                                           EmployeeID = p.EmployeeID,
                                                           DocumentID = p.DocumentID
                                                       }).ToList();
            totalCount = passportDetails.Count();
            return passportDetails.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public tbl_PM_GroupMaster GetDeliveryTeamName(int groupId)
        {
            try
            {
                tbl_PM_GroupMaster empDetails = dbContext.tbl_PM_GroupMaster.Where(ed => ed.GroupID == groupId).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_Travel GetTravelDetails(int travelId)
        {
            try
            {
                Tbl_HR_Travel empDetails = dbContext.Tbl_HR_Travel.Where(ed => ed.TravelId == travelId).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TravelTypeList> GetTravelTypes()
        {
            List<TravelTypeList> traveltype = new List<TravelTypeList>();
            try
            {
                traveltype = (from types in dbContext.tbl_HR_Travel_Type
                              orderby types.TravelTypeDescription
                              select new TravelTypeList
                              {
                                  TravelTypeId = types.TravelTypeID,
                                  TravelTypes = types.TravelTypeDescription
                              }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return traveltype;
        }

        //public List<ProjectNameList> GetProjectList()
        //{
        //    List<ProjectNameList> projectlist = new List<ProjectNameList>();
        //    try
        //    {
        //        projectlist = (from types in dbContext.tbl_Client_ProjectNamesMaster
        //                       orderby types.ProjectName
        //                       select new ProjectNameList
        //                       {
        //                           ProjectNameID = types.ProjectNameID,
        //                           ProjectName = types.ProjectName
        //                       }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return projectlist;
        //}

        public List<ProjectNameList> GetProjectList()
        {
            List<ProjectNameList> projectlist = new List<ProjectNameList>();
            try
            {
                projectlist = (from types in dbSEMContext.tbl_PM_Customer
                               orderby types.CustomerName
                               select new ProjectNameList
                               {
                                   ProjectNameID = types.Customer,
                                   ProjectName = types.CustomerName
                               }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return projectlist;
        }

        public tbl_PM_Location getOrganizationUnitDetails(int locationId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_Location pm_location = dbContext.tbl_PM_Location.Where(x => x.LocationID == locationId).FirstOrDefault();
                return pm_location;
            }
            catch
            {
                throw;
            }
        }

        public string GetNextTRFNo()
        {
            try
            {
                List<string> Trfno = new List<string>();
                Trfno = (from d in dbContext.Tbl_HR_Travel
                         select d.TRFNo).ToList();
                List<decimal> IDs = new List<decimal>();
                string maxval = "";
                if (Trfno.Count != 0)
                {
                    foreach (var item in Trfno)
                    {
                        IDs.Add(Convert.ToDecimal(item));
                    }

                    var result = (from m in IDs
                                  select m).Max();
                    maxval = Convert.ToString(result);
                }
                else
                {
                    maxval = "";
                }
                return maxval;
            }
            catch
            {
                throw;
            }
        }

        public tbl_HR_Travel_EmployeeTravelRequirement GetAccomodationDetails(int travelID)
        {
            try
            {
                tbl_HR_Travel_EmployeeTravelRequirement empDetails = dbContext.tbl_HR_Travel_EmployeeTravelRequirement.Where(ed => ed.TravelId == travelID).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RetriveTravelID SaveRquestDeatilsForm(TravelViewModel Traveldetailsmodel, int employeeId)
        {
            RetriveTravelID RetriveTravelID = new RetriveTravelID();

            Tbl_HR_Travel emp = dbContext.Tbl_HR_Travel.Where(ed => ed.TravelId == Traveldetailsmodel.TravelId).FirstOrDefault();

            bool TrfNostatatus = GetTRFNoIsValide(Traveldetailsmodel.TravelTRFNo);
            string LatestTrfNo = string.Empty;
            int TrfLatest;
            if (TrfNostatatus == true)
            {
                TrfLatest = Convert.ToInt32(Traveldetailsmodel.TravelTRFNo) + 1;
                LatestTrfNo = Convert.ToString(TrfLatest);
            }
            else
            {
                LatestTrfNo = Traveldetailsmodel.TravelTRFNo;
            }

            if (emp == null || emp.TravelId <= 0)
            {
                Tbl_HR_Travel expense = new Tbl_HR_Travel();
                expense.EmployeeId = Traveldetailsmodel.TravelEmployeeId;
                expense.TRFNo = LatestTrfNo;
                expense.RequestDate = Traveldetailsmodel.RequestDate;
                expense.ProjectName = Traveldetailsmodel.ProjectName;
                //expense.GroupHeadId = Traveldetailsmodel.GroupheadApprover;
                expense.CreatedDate = DateTime.Now;
                expense.ProjectManagerId = Traveldetailsmodel.ProjectManagerApprover;
                expense.AdminApproverId = Traveldetailsmodel.AdminApprover;
                expense.TravelTypeId = Traveldetailsmodel.TravelType;
                expense.TravelToCountry = Convert.ToInt32(Traveldetailsmodel.TravelToCountry);
                expense.TravelStartDate = Traveldetailsmodel.TravelStartDate;
                expense.TravelEndDate = Traveldetailsmodel.TravelEndDate;
                expense.ExpenseReimbursedByClient = Traveldetailsmodel.ExpenseReimbursedByClient;
                expense.AdditionalInfo = Traveldetailsmodel.AdditionalInfo;
                expense.StageID = 0;
                dbContext.Tbl_HR_Travel.AddObject(expense);

                dbContext.SaveChanges();
                Tbl_HR_Travel TravelDetails = dbContext.Tbl_HR_Travel.OrderByDescending(x => x.TravelId).FirstOrDefault();
                RetriveTravelID.TravelID = TravelDetails.TravelId;
                RetriveTravelID.EmployeeID = TravelDetails.EmployeeId.HasValue ? TravelDetails.EmployeeId.Value : 0;

                RetriveTravelID.IsAdded = true;
            }
            else
            {
                if (emp.StageID == 3 && Traveldetailsmodel.TravelStartDate != null && Traveldetailsmodel.TravelEndDate != null)
                {
                    if (Traveldetailsmodel.TravelExtensionEndDate != null)
                    {
                        emp.TravelExtensionEndDate = Traveldetailsmodel.TravelExtensionEndDate;
                    }
                    else
                    {
                        emp.TravelStartDate = Traveldetailsmodel.TravelStartDate;
                        emp.TravelEndDate = Traveldetailsmodel.TravelEndDate;
                    }
                    if (emp.StageID == 3 && Traveldetailsmodel.ContactNoAbroad != null)
                    {
                        emp.ContactNoAbroad = Traveldetailsmodel.ContactNoAbroad;
                    }
                    RetriveTravelID.TravelID = emp.TravelId;
                    RetriveTravelID.EmployeeID = emp.EmployeeId.HasValue ? emp.EmployeeId.Value : 0;
                    RetriveTravelID.IsAdded = true;
                    dbContext.SaveChanges();
                }
                else
                {
                    if (emp.StageID == 4)
                    {
                        emp.ContactNoAbroad = Traveldetailsmodel.ContactNoAbroad;
                        dbContext.SaveChanges();
                        RetriveTravelID.IsAdded = true;
                    }
                    else if (/*emp.GroupHeadId == employeeId ||*/ emp.ProjectManagerId == employeeId)
                    {
                        RetriveTravelID.TravelID = emp.TravelId;
                        RetriveTravelID.EmployeeID = emp.EmployeeId.HasValue ? emp.EmployeeId.Value : 0;
                        RetriveTravelID.IsAdded = true;
                    }
                    else
                    {
                        emp.EmployeeId = Traveldetailsmodel.TravelEmployeeId;
                        emp.TRFNo = Traveldetailsmodel.TravelTRFNo;
                        emp.RequestDate = Traveldetailsmodel.RequestDate;
                        emp.ProjectName = Traveldetailsmodel.ProjectName;
                        //emp.GroupHeadId = Traveldetailsmodel.GroupheadApprover;
                        emp.CreatedDate = DateTime.Now;
                        emp.ProjectManagerId = Traveldetailsmodel.ProjectManagerApprover;
                        emp.AdminApproverId = Traveldetailsmodel.AdminApprover;
                        emp.TravelTypeId = Traveldetailsmodel.TravelType;
                        emp.TravelToCountry = Convert.ToInt32(Traveldetailsmodel.TravelToCountry);
                        emp.TravelStartDate = Traveldetailsmodel.TravelStartDate;
                        emp.TravelEndDate = Traveldetailsmodel.TravelEndDate;
                        emp.ExpenseReimbursedByClient = Traveldetailsmodel.ExpenseReimbursedByClient;
                        emp.AdditionalInfo = Traveldetailsmodel.AdditionalInfo;
                        dbContext.SaveChanges();
                        RetriveTravelID.TravelID = emp.TravelId;
                        RetriveTravelID.EmployeeID = emp.EmployeeId.HasValue ? emp.EmployeeId.Value : 0;
                        RetriveTravelID.IsAdded = true;
                    }
                }
            }

            return RetriveTravelID;
        }

        public List<tbl_PM_EmployeeVisaDetails> GetTravelVisaDetailsFromVibrantWeb(int? employeeId)
        {
            List<tbl_PM_EmployeeVisaDetails> tbl_PM_EmployeeVisaDetails = new List<tbl_PM_EmployeeVisaDetails>();
            return tbl_PM_EmployeeVisaDetails = dbContext.tbl_PM_EmployeeVisaDetails.Where(x => x.EmployeeID == employeeId).ToList();
        }

        public List<tbl_HR_VisaDetailsTravel> GetTravelVisaDetail(int travelId)
        {
            List<tbl_HR_VisaDetailsTravel> tbl_HR_VisaDetailsTravel = new List<tbl_HR_VisaDetailsTravel>();
            return tbl_HR_VisaDetailsTravel = dbContext.tbl_HR_VisaDetailsTravel.Where(x => x.ID == travelId).ToList();
        }

        public bool GetTRFNoIsValide(string TRFNO)
        {
            try
            {
                bool status;
                var expensedetails = (from e in dbContext.Tbl_HR_Travel where e.TRFNo == TRFNO select e).FirstOrDefault();
                if (expensedetails != null)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void saveTravelVisaDetail(Tbl_HR_Travel travelDetails)
        {
            List<tbl_PM_EmployeeVisaDetails> employeeVisaDetails = this.GetTravelVisaDetailsFromVibrantWeb(travelDetails.EmployeeId);
            tbl_HR_VisaDetailsTravel updateVisaDetails = new tbl_HR_VisaDetailsTravel();

            if (employeeVisaDetails != null)
            {
                foreach (var employeeVisa in employeeVisaDetails)
                {
                    updateVisaDetails = (from t in dbContext.tbl_HR_VisaDetailsTravel
                                         where t.ID == travelDetails.TravelId && t.EmployeeVisaID == employeeVisa.EmployeeVisaID && t.CountryID == employeeVisa.CountryID
                                         select t).FirstOrDefault();

                    if (updateVisaDetails != null && travelDetails.StageID == 0)
                    {
                        updateVisaDetails.ID = travelDetails.TravelId;
                        updateVisaDetails.VisaTypeID = employeeVisa.VisaTypeID;
                        updateVisaDetails.CountryID = employeeVisa.CountryID;
                        updateVisaDetails.FromDate = employeeVisa.ValidFrom;
                        updateVisaDetails.ToDate = employeeVisa.ValidUpto;
                        updateVisaDetails.CreatedDate = employeeVisa.CreatedDate;
                        updateVisaDetails.VisaFileName = employeeVisa.VisaFileName;
                        updateVisaDetails.VisaFilePath = employeeVisa.VisaFilePath;
                    }
                    else if (updateVisaDetails == null && travelDetails.StageID == 0)
                    {
                        tbl_HR_VisaDetailsTravel VisaDetail = new tbl_HR_VisaDetailsTravel();
                        VisaDetail.ID = travelDetails.TravelId;
                        VisaDetail.VisaTypeID = employeeVisa.VisaTypeID;
                        VisaDetail.CountryID = employeeVisa.CountryID;
                        VisaDetail.FromDate = employeeVisa.ValidFrom;
                        VisaDetail.ToDate = employeeVisa.ValidUpto;
                        VisaDetail.CreatedDate = employeeVisa.CreatedDate;
                        VisaDetail.VisaFileName = employeeVisa.VisaFileName;
                        VisaDetail.VisaFilePath = employeeVisa.VisaFilePath;
                        VisaDetail.EmployeeVisaID = employeeVisa.EmployeeVisaID;
                        dbContext.tbl_HR_VisaDetailsTravel.AddObject(VisaDetail);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public List<ClientTravelList> clientYesNoList()
        {
            List<ClientTravelList> clientReimbursement = new List<ClientTravelList>();
            try
            {
                clientReimbursement = (from client in dbContext.Tbl_HR_Client_ReimbursementStatusMaster
                                       select new ClientTravelList
                                       {
                                           ClientTravelsId = client.ID,
                                           ClientTravelsValue = client.Description
                                       }).ToList();
                return clientReimbursement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PassportViewModel getEmployeeDetailsFromtravelID(int TravelID)
        {
            try
            {
                PassportViewModel details = (from employee in dbContext.HRMS_tbl_PM_Employee
                                             join passport in dbContext.Tbl_HR_Travel on employee.EmployeeID equals passport.EmployeeId
                                             where passport.TravelId == TravelID
                                             select new PassportViewModel
                                             {
                                                 EmployeeID = employee.EmployeeID,
                                                 EmployeeCode = employee.EmployeeCode
                                             }).FirstOrDefault();
                return details;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PassportViewModel GetPassportShowHistory(int EmployeeID, int DocumentID)
        {
            try
            {
                PassportViewModel showHistory = new PassportViewModel();
                showHistory = (from passport in dbContext.Tbl_PM_EmployeePassport
                               where passport.DocumentID == DocumentID && passport.EmployeeID == EmployeeID
                               select new PassportViewModel
                               {
                                   PassportFileName = passport.PassportFileName,
                                   CreatedDate = passport.CreatedDate,
                                   PassportFilePath = passport.PassportFilePath,
                                   DocumentID = passport.DocumentID,
                                   EmployeeID = passport.EmployeeID
                               }).FirstOrDefault();
                return showHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OtherAdminViewModel GetOtherRequireDetailsShowHistory(int? TravelID, int RequirementID)
        {
            try
            {
                OtherAdminViewModel otherdetailshistory = new OtherAdminViewModel();
                otherdetailshistory = (from otherdetails in dbContext.tbl_HR_TravelOtherRequirement
                                       where otherdetails.RequirementID == RequirementID && otherdetails.ID == TravelID
                                       select new OtherAdminViewModel
                               {
                                   FileName = otherdetails.FileName,
                                   FilePath = otherdetails.FilePath,
                                   ID = otherdetails.ID,
                                   RequirementID = otherdetails.RequirementID
                               }).FirstOrDefault();
                return otherdetailshistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeletePassportDetails(int DocumentID)
        {
            try
            {
                bool isDeleted = false;
                tbl_HR_Travel_PassportDocument _PassportDocument = dbContext.tbl_HR_Travel_PassportDocument.Where(p => p.DocumentID == DocumentID).FirstOrDefault();
                if (_PassportDocument != null)
                {
                    dbContext.DeleteObject(_PassportDocument);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<JourneyList> GetJourneyList(int page, int rows, int TravelID, string TRFNo, out int totalCount)
        {
            try
            {
                List<JourneyList> JourneyList = (from journey in dbContext.Tbl_HR_TravelJourneyDetails
                                                 join mode in dbContext.tbl_HR_Travel_JourneyMode
                                                 on journey.JourneyMode equals mode.JourneyModeID
                                                 where journey.TravelId == TravelID && journey.TRFNo == TRFNo
                                                 select new JourneyList
                                                 {
                                                     JourneyID = journey.Id,
                                                     TravelID = journey.TravelId,
                                                     FromPlace = journey.FromPlace,
                                                     ToPlace = journey.ToPlace,
                                                     JourneyDate = journey.JourneyDate,
                                                     JourneyMode = mode.JourneyModeDescription,
                                                     JourneyModeHidden = mode.JourneyModeDescription,
                                                     JourneyModeID = mode.JourneyModeID,
                                                     AdditionalInformation = journey.AdditionalInformation,
                                                     JourneyModeDetails = journey.JourneyModeDetail,
                                                     JourneyFeedback = journey.JourneyFeedback,
                                                     TicketName = journey.TicketName,
                                                     TicketNameUpload = null,
                                                     JourneyFilePath = journey.TicketName,
                                                     TRFNo = journey.TRFNo
                                                 }).ToList();
                totalCount = JourneyList.Count();
                return JourneyList.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmergencyContactViewModel> GetRelationList()
        {
            List<EmergencyContactViewModel> model = new List<EmergencyContactViewModel>();
            try
            {
                model = (from bt in dbContext.tbl_PM_EmployeeRelationType
                         select new EmergencyContactViewModel
                         {
                             uniqueID = bt.UniqueID,
                             Relation = bt.RelationType
                         }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model.OrderBy(x => x.Relation).ToList();
        }

        public bool SaveAdminAccoAdditionalInfo(int travelid, string AdditionalInformation)
        {
            try
            {
                bool isAdded = false;
                Tbl_HR_Travel additionalInfo = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                if (additionalInfo != null)
                {
                    if (AdditionalInformation != null && AdditionalInformation != "")
                        additionalInfo.AccoDetailsAdditionalInformation = AdditionalInformation.Trim();
                    else
                        additionalInfo.AccoDetailsAdditionalInformation = AdditionalInformation;
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_Travel GetTravelDetailsfromTravelID(int TravelID)
        {
            try
            {
                Tbl_HR_Travel travelDetails = dbContext.Tbl_HR_Travel.Where(t => t.TravelId == TravelID).FirstOrDefault();
                return travelDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveTravelContact(ContactViewModel model, int travelid, int ContStageid)
        {
            try
            {
                bool isAdded = false;
                var travelContact = dbContext.Tbl_HR_TravelContact.Where(x => x.TravelId == travelid).FirstOrDefault();

                if (travelContact == null)
                {
                    Tbl_HR_TravelContact contact = new Tbl_HR_TravelContact();
                    contact.TravelId = travelid;
                    contact.ContactNoIndia = model.ContactNoIndia;
                    contact.PersonalEmailId = model.userPersonalEmailId;
                    if (model.ContactNoAbroad != null && model.ContactNoAbroad != "")
                        contact.ContactNoAbroad = model.ContactNoAbroad;
                    contact.CreatedDate = DateTime.Now.Date;
                    dbContext.Tbl_HR_TravelContact.AddObject(contact);
                }
                else
                {
                    if (ContStageid == 3)
                    {
                        travelContact.ContactNoAbroad = model.ContactNoAbroad;
                    }
                    else
                    {
                        travelContact.TravelId = travelid;
                        travelContact.PersonalEmailId = model.userPersonalEmailId;
                        travelContact.ContactNoIndia = model.ContactNoIndia;
                        travelContact.ContactNoAbroad = model.ContactNoAbroad;
                        travelContact.ModifiedDate = DateTime.Now.Date;
                    }
                }
                dbContext.SaveChanges();
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveExtFormWithAllFormDeatils(string NewTRFNo, int Travelid, DateTime? Startdate, DateTime? enddate, string AddInfo, TravelViewModel model)
        {
            bool isAdded = false;

            var travelExtCheck = dbContext.Tbl_HR_Travel.Where(x => x.TRFNo == NewTRFNo).FirstOrDefault();

            try
            {
                if (travelExtCheck == null)
                {
                    #region Travel Extension form Details Move

                    Tbl_HR_Travel _tbl_HR_Travel = (from e in dbContext.Tbl_HR_Travel
                                                    where e.TravelId == Travelid
                                                    select e).FirstOrDefault();

                    if (_tbl_HR_Travel != null)
                    {
                        Tbl_HR_Travel Obj_tbl_HR_Travel = new Tbl_HR_Travel();
                        Obj_tbl_HR_Travel = _tbl_HR_Travel;
                        _tbl_HR_Travel.TRFNo = NewTRFNo;
                        _tbl_HR_Travel.TravelExtexsionStartDate = Startdate;
                        _tbl_HR_Travel.TravelExtensionEndDate = enddate;
                        _tbl_HR_Travel.TravelExtDetails = AddInfo;

                        _tbl_HR_Travel.RequestDate = model.RequestDate;
                        _tbl_HR_Travel.ProjectName = model.ProjectName;
                        _tbl_HR_Travel.GroupHeadId = model.GroupheadApprover;
                        _tbl_HR_Travel.CreatedDate = DateTime.Now;
                        _tbl_HR_Travel.ProjectManagerId = model.ProjectManagerApprover;
                        _tbl_HR_Travel.AdminApproverId = model.AdminApprover;
                        _tbl_HR_Travel.TravelTypeId = model.TravelType;
                        _tbl_HR_Travel.TravelToCountry = Convert.ToInt32(model.TravelToCountry);
                        _tbl_HR_Travel.TravelStartDate = _tbl_HR_Travel.TravelStartDate;
                        _tbl_HR_Travel.TravelEndDate = _tbl_HR_Travel.TravelEndDate;
                        _tbl_HR_Travel.ExpenseReimbursedByClient = model.ExpenseReimbursedByClient;
                        _tbl_HR_Travel.AdditionalInfo = model.AdditionalInfo;
                        _tbl_HR_Travel.ImmigrationDate = model.ImmigrationDate;

                        _tbl_HR_Travel.ConvaynaceAdditionalInfo = null;
                        _tbl_HR_Travel.AccoDetailsAdditionalInformation = null;
                        _tbl_HR_Travel.StageID = 0;

                        dbContext.ObjectStateManager.ChangeObjectState(Obj_tbl_HR_Travel, System.Data.EntityState.Added);
                        dbContext.Tbl_HR_Travel.AddObject(Obj_tbl_HR_Travel);
                        dbContext.SaveChanges();

                        int NewTravelID = dbContext.Tbl_HR_Travel.Where(x => x.TRFNo == NewTRFNo).FirstOrDefault().TravelId;
                        List<Tbl_HR_TravelJourneyDetails> travel = dbContext.Tbl_HR_TravelJourneyDetails.Where(x => x.TRFNo == NewTRFNo).ToList();
                        foreach (var item in travel)
                        {
                            item.TravelId = NewTravelID;
                        }
                    }

                    #endregion Travel Extension form Details Move

                    Tbl_HR_Travel ExtTravelid = GetTravelidExtensionform(NewTRFNo);

                    #region Travel Passport Details Move

                    tbl_HR_TravelPassportDetails _tbl_HR_Passport = (from e in dbContext.tbl_HR_TravelPassportDetails
                                                                     where e.TravelID == Travelid
                                                                     select e).FirstOrDefault();

                    if (_tbl_HR_Passport != null)
                    {
                        tbl_HR_TravelPassportDetails Obj_tbl_HR_Passport = new tbl_HR_TravelPassportDetails();
                        Obj_tbl_HR_Passport = _tbl_HR_Passport;
                        _tbl_HR_Passport.TravelID = ExtTravelid.TravelId;
                        dbContext.ObjectStateManager.ChangeObjectState(Obj_tbl_HR_Passport, System.Data.EntityState.Added);
                        dbContext.tbl_HR_TravelPassportDetails.AddObject(Obj_tbl_HR_Passport);
                    }

                    #endregion Travel Passport Details Move

                    #region Travel Passport Document Details Move

                    List<tbl_HR_Travel_PassportDocument> _tbl_HR_PassportDocument = (from e in dbContext.tbl_HR_Travel_PassportDocument
                                                                                     where e.TravelID == Travelid
                                                                                     select e).ToList();
                    if (_tbl_HR_PassportDocument != null)
                    {
                        List<tbl_HR_Travel_PassportDocument> Obj_tbl_HR_PassportDoc = new List<tbl_HR_Travel_PassportDocument>();
                        foreach (var i in _tbl_HR_PassportDocument)
                        {
                            i.TravelID = ExtTravelid.TravelId;
                        }
                        Obj_tbl_HR_PassportDoc = _tbl_HR_PassportDocument;
                        tbl_HR_Travel_PassportDocument Obj_tbl_HR_PassportDocnew = new tbl_HR_Travel_PassportDocument();
                        foreach (tbl_HR_Travel_PassportDocument add in Obj_tbl_HR_PassportDoc)
                        {
                            dbContext.ObjectStateManager.ChangeObjectState(add, System.Data.EntityState.Added);
                            dbContext.tbl_HR_Travel_PassportDocument.AddObject(add);
                        }
                    }

                    #endregion Travel Passport Document Details Move

                    #region Travel ClientInfo Details Move

                    List<tbl_HR_Travel_ClientInformation> _tbl_HR_Clentinfo = (from e in dbContext.tbl_HR_Travel_ClientInformation
                                                                               where e.TravelId == Travelid
                                                                               select e).ToList();

                    if (_tbl_HR_Clentinfo != null)
                    {
                        List<tbl_HR_Travel_ClientInformation> Obj_tbl_HR_ClientInfo = new List<tbl_HR_Travel_ClientInformation>();
                        Obj_tbl_HR_ClientInfo = _tbl_HR_Clentinfo;
                        foreach (var i in _tbl_HR_Clentinfo)
                        {
                            i.TravelId = ExtTravelid.TravelId;
                        }
                        foreach (tbl_HR_Travel_ClientInformation item in Obj_tbl_HR_ClientInfo)
                        {
                            dbContext.ObjectStateManager.ChangeObjectState(item, System.Data.EntityState.Added);
                            dbContext.tbl_HR_Travel_ClientInformation.AddObject(item);
                        }
                    }

                    #endregion Travel ClientInfo Details Move

                    #region Travel Contact Move

                    List<Tbl_HR_TravelContact> _tbl_HR_Contact = (from e in dbContext.Tbl_HR_TravelContact
                                                                  where e.TravelId == Travelid
                                                                  select e).ToList();

                    if (_tbl_HR_Contact != null)
                    {
                        List<Tbl_HR_TravelContact> Obj_tbl_HR_Contact = new List<Tbl_HR_TravelContact>();
                        Obj_tbl_HR_Contact = _tbl_HR_Contact;
                        foreach (var i in _tbl_HR_Contact)
                        {
                            i.TravelId = ExtTravelid.TravelId;
                        }

                        foreach (Tbl_HR_TravelContact item in Obj_tbl_HR_Contact)
                        {
                            dbContext.ObjectStateManager.ChangeObjectState(item, System.Data.EntityState.Added);
                            dbContext.Tbl_HR_TravelContact.AddObject(item);
                        }
                    }

                    #endregion Travel Contact Move

                    #region Travel Contact Details Move

                    List<Tbl_HR_TravelContactDetails> _tbl_HR_ContactDetails = (from e in dbContext.Tbl_HR_TravelContactDetails
                                                                                where e.TravelId == Travelid
                                                                                select e).ToList();

                    if (_tbl_HR_ContactDetails != null)
                    {
                        List<Tbl_HR_TravelContactDetails> Obj_tbl_HR_ContactDeatils = new List<Tbl_HR_TravelContactDetails>();
                        Obj_tbl_HR_ContactDeatils = _tbl_HR_ContactDetails;
                        foreach (var i in _tbl_HR_ContactDetails)
                        {
                            i.TravelId = ExtTravelid.TravelId;
                        }
                        foreach (Tbl_HR_TravelContactDetails item in Obj_tbl_HR_ContactDeatils)
                        {
                            dbContext.ObjectStateManager.ChangeObjectState(item, System.Data.EntityState.Added);
                            dbContext.Tbl_HR_TravelContactDetails.AddObject(item);
                        }
                    }

                    #endregion Travel Contact Details Move

                    #region Travel Visa  Move

                    List<tbl_HR_VisaDetailsTravel> _tbl_HR_VisaDeatails = (from e in dbContext.tbl_HR_VisaDetailsTravel
                                                                           where e.ID == Travelid
                                                                           select e).ToList();

                    if (_tbl_HR_VisaDeatails != null)
                    {
                        List<tbl_HR_VisaDetailsTravel> Obj_tbl_HR_VisaDeatails = new List<tbl_HR_VisaDetailsTravel>();

                        Obj_tbl_HR_VisaDeatails = _tbl_HR_VisaDeatails;
                        foreach (var i in _tbl_HR_VisaDeatails)
                        {
                            i.ID = ExtTravelid.TravelId;
                        }
                        foreach (tbl_HR_VisaDetailsTravel item in Obj_tbl_HR_VisaDeatails)
                        {
                            dbContext.ObjectStateManager.ChangeObjectState(item, System.Data.EntityState.Added);
                            dbContext.tbl_HR_VisaDetailsTravel.AddObject(item);
                        }
                    }

                    #endregion Travel Visa  Move
                }
                else
                {
                    travelExtCheck.RequestDate = model.RequestDate;
                    travelExtCheck.ProjectName = model.ProjectName;
                    travelExtCheck.GroupHeadId = model.GroupheadApprover;
                    travelExtCheck.CreatedDate = DateTime.Now;
                    travelExtCheck.ProjectManagerId = model.ProjectManagerApprover;
                    travelExtCheck.AdminApproverId = model.AdminApprover;
                    travelExtCheck.TravelTypeId = model.TravelType;
                    travelExtCheck.TravelToCountry = Convert.ToInt32(model.TravelToCountry);
                    travelExtCheck.TravelStartDate = model.TravelStartDate;
                    travelExtCheck.TravelEndDate = model.TravelEndDate;
                    travelExtCheck.ExpenseReimbursedByClient = model.ExpenseReimbursedByClient;
                    travelExtCheck.AdditionalInfo = model.AdditionalInfo;
                    travelExtCheck.ImmigrationDate = model.ImmigrationDate;
                    travelExtCheck.ContactNoAbroad = model.ContactNoAbroad;
                    if (model.TravelExtensionEndDate != null)
                    {
                        travelExtCheck.TravelExtensionEndDate = model.TravelExtensionEndDate;
                    }
                    travelExtCheck.TravelExtexsionStartDate = model.TravelExtexsionStartDate;
                    travelExtCheck.TravelExtDetails = model.TravelExtDetails;
                }
                dbContext.SaveChanges();

                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmployeeTrfNo(string decryptedTravelId)
        {
            try
            {
                int travelid = Convert.ToInt32(decryptedTravelId);
                Tbl_HR_Travel record = dbContext.Tbl_HR_Travel.Where(ed => ed.TravelId == travelid).FirstOrDefault();
                return record.TRFNo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetNewExtensionTRFNo(string trfNo)
        {
            try
            {
                decimal Trfno = Convert.ToDecimal(trfNo);
                decimal newTrf = decimal.Add(Trfno, 0.1M);
                return newTrf.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TravelShowStatus> GetShowStatusResult(int page, int rows, int travelID, out int totalCount)
        {
            try
            {
                List<TravelShowStatus> FinalResult = new List<TravelShowStatus>();
                List<TravelShowStatus> result = new List<TravelShowStatus>();
                TravelShowStatus secondresult = new TravelShowStatus();
                string ApproverName = string.Empty;
                string ApproverNameFinal = string.Empty;
                var travelDetails = (from e in dbContext.Tbl_HR_Travel where e.TravelId == travelID select e).FirstOrDefault();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                if (travelDetails.StageID == 0)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(travelDetails.EmployeeId.HasValue ? travelDetails.EmployeeId.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                if (travelDetails.StageID == 1)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(travelDetails.ProjectManagerId.HasValue ? travelDetails.ProjectManagerId.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                //else if (travelDetails.StageID == 2)
                //{
                //    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(travelDetails.GroupHeadId.HasValue ? travelDetails.GroupHeadId.Value : 0);
                //    ApproverNameFinal = EmpDetails.EmployeeName;
                //}
                else if (travelDetails.StageID == 3)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(travelDetails.AdminApproverId.HasValue ? travelDetails.AdminApproverId.Value : 0);
                    if (EmpDetails != null)
                        ApproverNameFinal = EmpDetails.EmployeeName;
                    else
                        ApproverNameFinal = "Pending for Admin to take Action";
                }

                result = (from events in dbContext.tbl_HR_TravelStageEvent
                          join employee in dbContext.HRMS_tbl_PM_Employee on events.UserID equals employee.EmployeeID into expenseemployee
                          from exevent in expenseemployee.DefaultIfEmpty()
                          join expense in dbContext.Tbl_HR_Travel on events.TravelId equals expense.TravelId into expenseEvent
                          from exStageEvent in expenseEvent.DefaultIfEmpty()
                          join stages in dbContext.tbl_HR_Travel_TravelStages on events.Action == "Approve" ? events.ToStageID : (events.FromStageID + 1) equals stages.StageID into stage
                          from eventstage in stage.DefaultIfEmpty()
                          join employee in dbContext.HRMS_tbl_PM_Employee on exStageEvent.EmployeeId equals employee.EmployeeID into employeeexpenseevent
                          from employeeexpense in employeeexpenseevent.DefaultIfEmpty()
                          where exStageEvent.TravelId == travelID
                          orderby events.TravelStatgeID ascending
                          select new TravelShowStatus
                          {
                              TravelShowstatusAction = events.Action,
                              TravelShowstatusActor = exevent.EmployeeName,
                              TravelShowstatusCurrentStage = eventstage.TravelStage,
                              TravelShowstatusStageID = events.FromStageID,
                              TravelShowstatusEmployeeCode = employeeexpense.EmployeeCode,
                              TravelShowstatusEmployeeId = exevent.EmployeeID,
                              TravelShowstatusEmployeeName = employeeexpense.EmployeeName,
                              TravelShowstatusTime = events.EventDateTime,
                              TravelShowstatusComments = events.Action == "Rejected" ? events.Comments : ""
                          }).ToList();

                if (result.Any())
                    FinalResult.AddRange(result);

                if (travelDetails.StageID != 4)
                {
                    string msgToDisplay = "";
                    if (travelDetails.StageID == 3)
                    {
                        msgToDisplay = "Pending for Admin to take Action";
                    }
                    else
                    {
                        msgToDisplay = "Waiting for " + ApproverNameFinal + " to take Action";
                    }
                    secondresult = (from ex in dbContext.Tbl_HR_Travel
                                    join s in dbContext.tbl_HR_Travel_TravelStages on (ex.StageID + 1) equals s.StageID into stage
                                    from EStage in stage.DefaultIfEmpty()
                                    where ex.TravelId == travelID
                                    select new TravelShowStatus
                                    {
                                        TravelShowstatusCurrentStage = EStage.TravelStage,
                                        showStatus = msgToDisplay
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }
                else if (travelDetails.StageID == 4)
                {
                    secondresult = (from ex in dbContext.Tbl_HR_Travel
                                    join s in dbContext.tbl_HR_Travel_TravelStages on (ex.StageID + 1) equals s.StageID into stage
                                    from EStage in stage.DefaultIfEmpty()
                                    where ex.TravelId == travelID
                                    select new TravelShowStatus
                                    {
                                        TravelShowstatusCurrentStage = EStage.TravelStage
                                    }).FirstOrDefault();
                    FinalResult.Add(secondresult);
                }
                totalCount = FinalResult.Count;
                return FinalResult.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveCommentDetails(int travelid, string comments, string CommentType)
        {
            bool status = false;
            try
            {
                Tbl_HR_Travel emp = dbContext.Tbl_HR_Travel.Where(ed => ed.TravelId == travelid).FirstOrDefault();
                if (emp != null)
                {
                    if (CommentType == "rejected")
                    {
                        emp.RejectComment = comments;
                        dbContext.SaveChanges();
                    }

                    if (CommentType == "canceled")
                    {
                        emp.CancelComment = comments;
                        dbContext.SaveChanges();
                    }
                }

                status = true;
            }
            catch
            {
                throw;
            }
            return status;
        }

        public bool RejectTravelApprovalForm(AccomodationViewModel empTravel, int employeeId)
        {
            bool isDeleted = false;
            Tbl_HR_Travel travelInfo = dbContext.Tbl_HR_Travel.Where(cd => cd.TravelId == empTravel.TravelId).FirstOrDefault();
            if (travelInfo != null && travelInfo.TravelId > 0)
            {
                travelInfo.StageID = 0;
                tbl_HR_TravelStageEvent emp = new tbl_HR_TravelStageEvent();
                emp.TravelId = empTravel.TravelId;
                emp.EventDateTime = DateTime.Now;
                emp.Action = "Rejected";
                emp.FromStageID = empTravel.StageID;
                emp.ToStageID = 0;
                emp.UserID = employeeId;
                emp.Comments = empTravel.Comments;
                dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public TravelStatus getTravelDetails(int travelID)
        {
            try
            {
                TravelStatus travelDetails = new TravelStatus();
                travelDetails = (from travel in dbContext.Tbl_HR_Travel
                                 join employee in dbContext.HRMS_tbl_PM_Employee on travel.EmployeeId equals employee.EmployeeID into exemployee
                                 from travelemployee in exemployee.DefaultIfEmpty()
                                 where travel.TravelId == travelID
                                 select new TravelStatus
                                 {
                                     TravelId = travel.TravelId,
                                     EmployeeId = travel.EmployeeId,
                                     Employeename = travelemployee.EmployeeName,
                                     StageId = travel.StageID,
                                     ProjectName = travel.ProjectName,
                                     ProjectManagerApprover = travel.ProjectManagerId,
                                     GroupHeadApprover = travel.GroupHeadId,
                                     AdminApprover = travel.AdminApproverId
                                 }).FirstOrDefault();
                return travelDetails;
            }
            catch
            {
                throw;
            }
        }

        public bool DeletedAllTravelDetails(int TravelID, string employeeId)
        {
            bool isDeleted = false;
            Tbl_HR_Travel DelFromTraveltbl = dbContext.Tbl_HR_Travel.Where(cd => cd.TravelId == TravelID).FirstOrDefault();
            if (TravelID != null && TravelID > 0)
            {
                if (DelFromTraveltbl != null)
                {
                    DelFromTraveltbl.IsCancelled = true;
                }

                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public EmployeeDetailsViewModel getEmployeeDetailsForTravel(int? employeeID)
        {
            try
            {
                EmployeeDetailsViewModel employeeDetails = new EmployeeDetailsViewModel();
                employeeDetails = (from e in dbContext.HRMS_tbl_PM_Employee
                                   join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into role
                                   from ERole in role.DefaultIfEmpty()
                                   where e.EmployeeID == employeeID
                                   select new EmployeeDetailsViewModel
                                   {
                                       OrgRoleDescription = ERole.RoleDescription,
                                       EmployeeId = e.EmployeeID,
                                       EmployeeCode = e.EmployeeCode,
                                       EmployeeName = e.EmployeeName,
                                       EmailID = e.EmailID
                                   }).FirstOrDefault();
                return employeeDetails;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveTravelContactDetails(List<tbl_PM_EmployeeEmergencyContact> list, int travelID)
        {
            bool isAdded = false;
            try
            {
                foreach (var item in list)
                {
                    Tbl_HR_TravelContactDetails Obj_tbl_HR_ContactDetails = new Tbl_HR_TravelContactDetails();
                    Obj_tbl_HR_ContactDetails.EmerContactId = item.EmployeeEmergencyContactID;
                    Obj_tbl_HR_ContactDetails.Name = item.Name;
                    Obj_tbl_HR_ContactDetails.TravelId = travelID;
                    Obj_tbl_HR_ContactDetails.Relationship = item.RelationTypeID;
                    Obj_tbl_HR_ContactDetails.Address = item.Address;
                    Obj_tbl_HR_ContactDetails.ContactNo = item.ContactNo;
                    Obj_tbl_HR_ContactDetails.EmailId = item.EmailID;
                    Obj_tbl_HR_ContactDetails.CreatedBy = item.EmployeeID;
                    Obj_tbl_HR_ContactDetails.CreatedDate = DateTime.Now;
                    Obj_tbl_HR_ContactDetails.IsAddedFromVB = true;
                    dbContext.Tbl_HR_TravelContactDetails.AddObject(Obj_tbl_HR_ContactDetails);
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TravelStatus> GetInboxListTravelDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<TravelStatus> mainResult = new List<TravelStatus>();
            List<TravelStatus> employeeresult = new List<TravelStatus>();
            List<TravelStatus> PriApproverCheck = new List<TravelStatus>();
            List<TravelStatus> SecApproverCheck = new List<TravelStatus>();
            List<TravelStatus> FinApproverCheck = new List<TravelStatus>();

            List<TravelStatus> ListForStageId = new List<TravelStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };
                EmployeeDAL empdal = new EmployeeDAL();

                HRMS_tbl_PM_Employee employeeDetails = empdal.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                #region Employee Inbox Section

                employeeresult = (from E in dbContext.Tbl_HR_Travel
                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID into exp
                                  from ex in exp.DefaultIfEmpty()
                                  join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID into st
                                  from extstage in st.DefaultIfEmpty()
                                  where E.EmployeeId == employeeId && E.StageID == 0 && (E.IsCancelled == false || E.IsCancelled == null)
                                  && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                       && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                  join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                                  select new TravelStatus
                                  {
                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                                      ReportingTo = ex.ReportingTo,
                                      TravelId = E.TravelId,
                                      StageId = E.StageID,
                                      TravelStageOrder = E.StageID,
                                      stageName = extstage.TravelStage,
                                      EmployeeId = E.EmployeeId,
                                      Employeename = ex.EmployeeName,
                                      TravelRequestNumber = E.TRFNo
                                  }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                #endregion Employee Inbox Section

                #region For Project Manager Approver Inbox Section

                PriApproverCheck = (from E in dbContext.Tbl_HR_Travel
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID
                                    join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID
                                    where E.ProjectManagerId == employeeId && E.StageID == 1 && (E.IsCancelled == false || E.IsCancelled == null)
                                     && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                                    select new TravelStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                                        ReportingTo = emp.ReportingTo,
                                        TravelId = E.TravelId,
                                        StageId = E.StageID,
                                        TravelStageOrder = E.StageID,
                                        stageName = s.TravelStage,
                                        EmployeeId = E.EmployeeId,
                                        Employeename = emp.EmployeeName,
                                        TravelRequestNumber = E.TRFNo
                                    }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                #endregion For Project Manager Approver Inbox Section

                //#region Secondary approver Inbox Section
                //SecApproverCheck = (from E in dbContext.Tbl_HR_Travel
                //                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID
                //                    join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID
                //                    where E.GroupHeadId == employeeId && E.StageID == 2 && (E.IsCancelled == false || E.IsCancelled == null)
                //                     && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                //                         && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                //                    join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                //                    select new TravelStatus
                //                    {
                //                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                //                        ReportingTo = emp.ReportingTo,
                //                        TravelId = E.TravelId,
                //                        StageId = E.StageID,
                //                        TravelStageOrder = E.StageID,
                //                        stageName = s.TravelStage,
                //                        EmployeeId = E.EmployeeId,
                //                        Employeename = emp.EmployeeName,
                //                        TravelRequestNumber = E.TRFNo

                //                    }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                //#endregion

                #region Finance Approval stage Inbox Section

                FinApproverCheck = (from E in dbContext.Tbl_HR_Travel
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID
                                    join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID
                                    where LogeedInEmRoles.Contains(UserRoles.TravelAdmin) && E.StageID == 3 && (E.IsCancelled == false || E.IsCancelled == null)
                                     && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                                    select new TravelStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                                        ReportingTo = emp.ReportingTo,
                                        TravelId = E.TravelId,
                                        StageId = E.StageID,
                                        TravelStageOrder = E.StageID,
                                        stageName = s.TravelStage,
                                        EmployeeId = E.EmployeeId,
                                        Employeename = emp.EmployeeName,
                                        TravelRequestNumber = E.TRFNo
                                    }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                #endregion Finance Approval stage Inbox Section

                // admin condition change : E.AdminApproverId == employeeId && this remove from where clause
                mainResult = employeeresult.Union(employeeresult).Union(PriApproverCheck).Union(SecApproverCheck).Union(FinApproverCheck).ToList();

                var distinctItems = mainResult.GroupBy(x => x.TravelId).Select(y => y.First()).ToList();
                totalCount = distinctItems.Count;
                //return distinctItems.Skip((page - 1) * rows).Take(rows).ToList();
                return distinctItems.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TravelStatus> GetWatchListTravelDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<TravelStatus> mainResult = new List<TravelStatus>();
            List<TravelStatus> employeeresult = new List<TravelStatus>();
            List<TravelStatus> PriApproverCheck = new List<TravelStatus>();
            List<TravelStatus> SecApproverCheck = new List<TravelStatus>();
            List<TravelStatus> FinApproverCheck = new List<TravelStatus>();
            List<TravelStatus> mainResultFinal = new List<TravelStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };
                EmployeeDAL empdal = new EmployeeDAL();

                HRMS_tbl_PM_Employee employeeDetails = empdal.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                #region Employee Watchlist Section

                employeeresult = (from E in dbContext.Tbl_HR_Travel
                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID
                                  join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID
                                  where E.EmployeeId == employeeId && E.StageID != 0 && (E.IsCancelled == false || E.IsCancelled == null) && (((E.StageID == 1) && (E.ProjectManagerId != E.EmployeeId)) || ((E.StageID == 2) && (E.GroupHeadId != E.EmployeeId)) || (E.StageID == 3) || (E.StageID == 4))
                                  && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                       && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                  join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                                  select new TravelStatus
                                  {
                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                                      ReportingTo = emp.ReportingTo,
                                      TravelId = E.TravelId,
                                      StageId = E.StageID,
                                      TravelStageOrder = E.StageID,
                                      stageName = s.TravelStage,
                                      EmployeeId = E.EmployeeId,
                                      Employeename = emp.EmployeeName,
                                      TravelRequestNumber = E.TRFNo
                                  }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                #endregion Employee Watchlist Section

                #region For Primary Approver Watchlist Section

                PriApproverCheck = (from E in dbContext.Tbl_HR_Travel
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID into st
                                    from extstage in st.DefaultIfEmpty()
                                    where E.ProjectManagerId == employeeId && (E.StageID != 1 && (E.StageID == 0 || E.StageID == 2 || E.StageID == 4 || (E.StageID == 3 && !LogeedInEmRoles.Contains(UserRoles.TravelAdmin)))) && (E.IsCancelled == false || E.IsCancelled == null)
                                     && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                                    select new TravelStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                                        ReportingTo = ex.ReportingTo,
                                        TravelId = E.TravelId,
                                        StageId = E.StageID,
                                        TravelStageOrder = E.StageID,
                                        stageName = extstage.TravelStage,
                                        EmployeeId = E.EmployeeId,
                                        Employeename = ex.EmployeeName,
                                        TravelRequestNumber = E.TRFNo
                                    }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                //if (LogeedInEmRoles.Contains(UserRoles.TravelAdmin) && LogeedInEmRoles.Contains(UserRoles.TravelApprover))
                //{
                //    List<tbl_HR_TravelStageEvent> LatestEntry = new List<tbl_HR_TravelStageEvent>();
                //    if (PriApproverCheck != null)
                //    {
                //        foreach (var item in PriApproverCheck.ToList())
                //        {
                //            LatestEntry = (from empInfo in dbContext.tbl_HR_TravelStageEvent
                //                           where empInfo.TravelId == item.TravelId
                //                           orderby empInfo.EventDateTime descending
                //                           select empInfo).ToList();

                //            if (LatestEntry.Count > 0)
                //            {
                //                foreach (var y in LatestEntry)
                //                {
                //                    if (y.FromStageID == 0 && y.ToStageID == 1)
                //                    {
                //                        if (y.UserID == item.EmployeeId)
                //                        {
                //                            PriApproverCheck.Remove(item);
                //                        }

                //                    }
                //                }

                //            }
                //            else
                //            {
                //                continue;
                //            }
                //        }

                //    }

                //}

                #endregion For Primary Approver Watchlist Section

                //#region Secondary approver Watchlist Section
                //SecApproverCheck = (from E in dbContext.Tbl_HR_Travel
                //                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID into exp
                //                    from ex in exp.DefaultIfEmpty()
                //                    join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID into st
                //                    from extstage in st.DefaultIfEmpty()
                //                    where E.GroupHeadId == employeeId && (E.StageID != 2) && (E.IsCancelled == false || E.IsCancelled == null)
                //                    && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                //                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                //                    join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                //                    select new TravelStatus
                //                    {
                //                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                //                        ReportingTo = ex.ReportingTo,
                //                        TravelId = E.TravelId,
                //                        StageId = E.StageID,
                //                        TravelStageOrder = E.StageID,
                //                        stageName = extstage.TravelStage,
                //                        EmployeeId = E.EmployeeId,
                //                        Employeename = ex.EmployeeName,
                //                        TravelRequestNumber = E.TRFNo

                //                    }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                //#endregion

                #region Finance Approval stage Watchlist Section

                FinApproverCheck = (from E in dbContext.Tbl_HR_Travel
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID + 1 equals s.StageID into st
                                    from extstage in st.DefaultIfEmpty()
                                    where LogeedInEmRoles.Contains(UserRoles.TravelAdmin) && (E.StageID != 3) && (E.IsCancelled == false || E.IsCancelled == null) && (E.ProjectManagerId != employeeId || E.ProjectManagerId == null)
                                    && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_HR_TravelStageEvent on E.TravelId equals ese.TravelId into eventStageRecord
                                    select new TravelStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty,
                                        ReportingTo = ex.ReportingTo,
                                        TravelId = E.TravelId,
                                        StageId = E.StageID,
                                        TravelStageOrder = E.StageID,
                                        stageName = extstage.TravelStage,
                                        EmployeeId = E.EmployeeId,
                                        Employeename = ex.EmployeeName,
                                        TravelRequestNumber = E.TRFNo
                                    }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();

                #endregion Finance Approval stage Watchlist Section

                // remove admin approve code : E.AdminApproverId == employeeId &&
                mainResult =
                    employeeresult.Union(employeeresult)
                        .Union(PriApproverCheck)
                        .Union(SecApproverCheck)
                        .Union(FinApproverCheck)
                        .ToList();

                foreach (var item in mainResult)
                {
                    if (LogeedInEmRoles.Contains(UserRoles.TravelAdmin) && (LogeedInEmRoles.Contains(UserRoles.Management) || LogeedInEmRoles.Contains(UserRoles.GroupHead) || LogeedInEmRoles.Contains(UserRoles.TravelApprover)) && item.StageId == 3)
                    {
                        continue;
                    }
                    else
                        mainResultFinal.Add(item);
                }

                var distinctItems = mainResultFinal.GroupBy(x => x.TravelId).Select(y => y.First()).ToList();
                totalCount = distinctItems.Count;
                //return distinctItems.Skip((page - 1) * rows).Take(rows).ToList();
                return distinctItems.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveJourneyDetails(JourneyList model, int TravelId, string TRFNO, string JourneyModeId)
        {
            try
            {
                bool isAdded = false;

                int Travelid = TravelId;

                var travelJourney = dbContext.Tbl_HR_TravelJourneyDetails.Where(x => x.Id == model.JourneyID).FirstOrDefault();
                Tbl_HR_Travel GetStageid = GetTravelDetails(Travelid);
                int? stageid = GetStageid.StageID;

                if (travelJourney == null)
                {
                    Tbl_HR_TravelJourneyDetails journey = new Tbl_HR_TravelJourneyDetails();
                    journey.TravelId = Travelid;
                    journey.FromPlace = model.FromPlace;
                    journey.ToPlace = model.ToPlace;
                    journey.JourneyMode = Convert.ToInt32(JourneyModeId);
                    journey.JourneyDate = model.JourneyDate;
                    journey.AdditionalInformation = model.AdditionalInformation;
                    journey.JourneyModeDetail = model.JourneyModeDetails;
                    journey.JourneyFeedback = model.JourneyFeedback;
                    journey.CreatedDate = DateTime.Now.Date;
                    journey.TicketName = model.TicketName;
                    journey.UploadPath = model.JourneyFilePath;
                    journey.TRFNo = TRFNO;
                    dbContext.Tbl_HR_TravelJourneyDetails.AddObject(journey);
                }
                else
                {
                    if (stageid == 4)
                    {
                        travelJourney.JourneyFeedback = model.JourneyFeedback;
                    }
                    else
                    {
                        if (model.JourneyDate == null || model.ToPlace == null)
                        {
                            travelJourney.JourneyModeDetail = model.JourneyModeDetails;
                            if (!string.IsNullOrEmpty(model.TicketName))
                                travelJourney.TicketName = model.TicketName.Trim();
                            else
                                travelJourney.TicketName = travelJourney.TicketName;
                            if (!string.IsNullOrEmpty(model.JourneyFilePath))
                                travelJourney.UploadPath = model.JourneyFilePath.Trim();
                            else
                                travelJourney.UploadPath = travelJourney.UploadPath;
                        }
                        else
                        {
                            travelJourney.TravelId = Travelid;
                            travelJourney.FromPlace = model.FromPlace;
                            travelJourney.ToPlace = model.ToPlace;
                            travelJourney.JourneyMode = Convert.ToInt32(JourneyModeId);
                            travelJourney.JourneyDate = model.JourneyDate;
                            travelJourney.AdditionalInformation = model.AdditionalInformation;
                            travelJourney.JourneyModeDetail = model.JourneyModeDetails;
                            travelJourney.JourneyFeedback = model.JourneyFeedback;
                            if (!string.IsNullOrEmpty(model.TicketName))
                                travelJourney.TicketName = model.TicketName.Trim();
                            else
                                travelJourney.TicketName = travelJourney.TicketName;
                            travelJourney.TRFNo = TRFNO;
                            if (!string.IsNullOrEmpty(model.JourneyFilePath))
                                travelJourney.UploadPath = model.JourneyFilePath.Trim();
                            else
                                travelJourney.UploadPath = travelJourney.UploadPath;
                        }
                    }
                }
                dbContext.SaveChanges();
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveJourneyUploadDetails(JourneyViewModel model, int TravelId)
        {
            try
            {
                bool isAdded = false;
                Tbl_HR_TravelJourneyUploadDetails journeyDetails = dbContext.Tbl_HR_TravelJourneyUploadDetails.Where(j => j.JourneyId == model.JourneyDetail.JourneyID && j.TravelId == TravelId).FirstOrDefault();
                if (journeyDetails == null)
                {
                    Tbl_HR_TravelJourneyUploadDetails journeyUpload = new Tbl_HR_TravelJourneyUploadDetails();
                    journeyUpload.TravelId = TravelId;
                    journeyUpload.JourneyId = model.JourneyDetail.JourneyID;
                    journeyUpload.FileName = model.JourneyDetail.TicketName;
                    journeyUpload.FilePath = model.JourneyDetail.JourneyFilePath;
                    dbContext.Tbl_HR_TravelJourneyUploadDetails.AddObject(journeyUpload);
                    isAdded = true;
                }
                else
                {
                    journeyDetails.TravelId = TravelId;
                    journeyDetails.JourneyId = model.JourneyDetail.JourneyID;
                    journeyDetails.FileName = model.JourneyDetail.TicketName;
                    journeyDetails.FilePath = model.JourneyDetail.JourneyFilePath;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TravelEmergencyContactViewModel> GetTravelEmergencyContactDetails(int travelId, int page, int rows, out int totalCount)
        {
            try
            {
                HRMSDBEntities dbContext = new HRMSDBEntities();
                var EmergencyContactList = (from contact in dbContext.Tbl_HR_TravelContactDetails
                                            where contact.TravelId == travelId
                                            orderby contact.TravelId descending
                                            select new TravelEmergencyContactViewModel
                                            {
                                                TravelId = contact.TravelId,
                                                EmployeeEmergencyContactId = contact.Id,
                                                Name = contact.Name,
                                                EmailId = contact.EmailId,
                                                ContactNo = contact.ContactNo,
                                                EmgAddress = contact.Address,
                                                Relation = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                                            where relation.UniqueID == contact.Relationship
                                                            select relation.RelationType).FirstOrDefault(),
                                                uniqueID = contact.Relationship,
                                                IsAddedFromVB = contact.IsAddedFromVB.HasValue ? contact.IsAddedFromVB.Value : false
                                            }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from contact in dbContext.Tbl_HR_TravelContactDetails
                              where contact.TravelId == travelId
                              select contact.TravelId).Count();
                return EmergencyContactList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddTravelEmergencyContactDetails(TravelEmergencyContactViewModel emergencyContact, int UniqueID, int TravelId, string decryptedTravelId)
        {
            try
            {
                bool isAdded = false;
                int Travelid = Convert.ToInt32(decryptedTravelId);

                Tbl_HR_TravelContactDetails contDetails = dbContext.Tbl_HR_TravelContactDetails.Where(ed => ed.Id == emergencyContact.EmployeeEmergencyContactId).FirstOrDefault();
                if (contDetails == null || contDetails.Id <= 0)
                {
                    Tbl_HR_TravelContactDetails contCtls = new Tbl_HR_TravelContactDetails();
                    contCtls.TravelId = TravelId;

                    contCtls.Name = emergencyContact.Name.Trim();

                    if (emergencyContact.EmgAddress != null && emergencyContact.EmgAddress != "")
                        contCtls.Address = emergencyContact.EmgAddress.Trim();
                    else
                        contCtls.Address = emergencyContact.EmgAddress;

                    if (emergencyContact.EmailId != null && emergencyContact.EmailId != "")
                        contCtls.EmailId = emergencyContact.EmailId.Trim();
                    else
                        contCtls.EmailId = emergencyContact.EmailId;

                    contCtls.ContactNo = emergencyContact.ContactNo.Trim();
                    contCtls.Relationship = UniqueID;
                    contCtls.IsAddedFromVB = false;
                    dbContext.Tbl_HR_TravelContactDetails.AddObject(contCtls);
                    dbContext.SaveChanges();
                }
                else
                {
                    contDetails.TravelId = TravelId;
                    contDetails.Name = emergencyContact.Name.Trim();
                    if (emergencyContact.EmgAddress != null && emergencyContact.EmgAddress != "")
                        contDetails.Address = emergencyContact.EmgAddress.Trim();
                    else
                        contDetails.Address = emergencyContact.EmgAddress;

                    if (emergencyContact.EmailId != null && emergencyContact.EmailId != "")
                        contDetails.EmailId = emergencyContact.EmailId.Trim();
                    else
                        contDetails.EmailId = emergencyContact.EmailId;
                    contDetails.ContactNo = emergencyContact.ContactNo.Trim();
                    contDetails.Relationship = UniqueID;
                    contDetails.IsAddedFromVB = false;
                    dbContext.SaveChanges();
                }

                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_TravelContact GetTravelContact(int travelId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                Tbl_HR_TravelContact contact = dbContext.Tbl_HR_TravelContact.Where(ed => ed.TravelId == travelId).FirstOrDefault();
                return contact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveTravelContact(HRMS_tbl_PM_Employee objContactDetails, int travelId)
        {
            try
            {
                bool isSaved = false;
                if (objContactDetails != null)
                {
                    Tbl_HR_TravelContact contactDetails = new Tbl_HR_TravelContact();
                    contactDetails.TravelId = travelId;
                    contactDetails.PersonalEmailId = objContactDetails.EmailID1;
                    contactDetails.ContactNoIndia = objContactDetails.MobileNumber;
                    dbContext.Tbl_HR_TravelContact.AddObject(contactDetails);
                    dbContext.SaveChanges();
                    isSaved = true;
                }
                return isSaved;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteTravelContactDetails(int contactId, int travelId)
        {
            try
            {
                bool isDeleted = false;
                Tbl_HR_TravelContactDetails contDetails = dbContext.Tbl_HR_TravelContactDetails.Where(ed => ed.Id == contactId && ed.TravelId == travelId).FirstOrDefault();

                if (contDetails != null && contDetails.Id > 0)
                {
                    dbContext.Tbl_HR_TravelContactDetails.DeleteObject(contDetails);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_TravelContactDetails GetTravelContactDetailsForEmergncyContactId(int emerContactId, int travelId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                Tbl_HR_TravelContactDetails contact = dbContext.Tbl_HR_TravelContactDetails.Where(ed => ed.EmerContactId == emerContactId && ed.TravelId == travelId).FirstOrDefault();
                return contact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveConveyAdditionalInfo(int TravelID, string addiconveyInfo)
        {
            try
            {
                bool isAdded = false;
                Tbl_HR_Travel additionalInfo = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == TravelID).FirstOrDefault();
                if (additionalInfo != null)
                {
                    if (addiconveyInfo != null && addiconveyInfo != "")
                        additionalInfo.ConvaynaceAdditionalInfo = addiconveyInfo.Trim();
                    else
                        additionalInfo.ConvaynaceAdditionalInfo = addiconveyInfo;
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteJourneyDetails(int JourneyId)
        {
            bool isDeleted = false;
            Tbl_HR_TravelJourneyDetails journeyInfo = dbContext.Tbl_HR_TravelJourneyDetails.Where(jr => jr.Id == JourneyId).FirstOrDefault();
            if (JourneyId != null && JourneyId > 0)
            {
                dbContext.DeleteObject(journeyInfo);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<JourneyModeList> GetJourneyModeList()
        {
            List<JourneyModeList> model = new List<JourneyModeList>();
            try
            {
                model = (from bt in dbContext.tbl_HR_Travel_JourneyMode
                         select new JourneyModeList
                         {
                             JourneyModeId = bt.JourneyModeID,
                             JourneyModeDescription = bt.JourneyModeDescription
                         }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model.OrderBy(x => x.JourneyModeId).ToList();
        }

        public List<TravelStatus> GetExtensionListTravelDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<TravelStatus> mainResult = new List<TravelStatus>();
            List<TravelStatus> employeeresult = new List<TravelStatus>();
            List<TravelStatus> FinApproverCheck = new List<TravelStatus>();

            List<TravelStatus> SameRecords = new List<TravelStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };
                EmployeeDAL empdal = new EmployeeDAL();

                HRMS_tbl_PM_Employee employeeDetails = empdal.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                #region Finance Approval stage Extension Section

                if (/*LogeedInEmRoles.Contains("Group Head") ||*/ LogeedInEmRoles.Contains("Travel Approver"))
                {
                    FinApproverCheck = (from E in dbContext.Tbl_HR_Travel
                                        join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID into exp
                                        from ex in exp.DefaultIfEmpty()
                                        join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID equals s.StageID into st
                                        from extstage in st.DefaultIfEmpty()
                                        where E.StageID == 4 && (E.IsCancelled == false || E.IsCancelled == null)
                                        && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                       && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                        select new TravelStatus
                                        {
                                            ReportingTo = ex.ReportingTo,
                                            TravelId = E.TravelId,
                                            StageId = E.StageID,
                                            TravelStageOrder = E.StageID,
                                            stageName = extstage.TravelStage,
                                            EmployeeId = E.EmployeeId,
                                            Employeename = ex.EmployeeName,
                                            TravelRequestNumber = E.TRFNo
                                        }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();
                }
                else
                {
                    FinApproverCheck = (from E in dbContext.Tbl_HR_Travel
                                        join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeId equals emp.EmployeeID into exp
                                        from ex in exp.DefaultIfEmpty()
                                        join s in dbContext.tbl_HR_Travel_TravelStages on E.StageID equals s.StageID into st
                                        from extstage in st.DefaultIfEmpty()
                                        where E.StageID == 4 && (E.IsCancelled == false || E.IsCancelled == null) && E.EmployeeId == employeeId
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                        select new TravelStatus
                                        {
                                            ReportingTo = ex.ReportingTo,
                                            TravelId = E.TravelId,
                                            StageId = E.StageID,
                                            TravelStageOrder = E.StageID,
                                            stageName = extstage.TravelStage,
                                            EmployeeId = E.EmployeeId,
                                            Employeename = ex.EmployeeName,
                                            TravelRequestNumber = E.TRFNo
                                        }).Distinct().OrderByDescending(exid => exid.TravelId).ToList();
                }

                #endregion Finance Approval stage Extension Section

                mainResult = employeeresult.Union(FinApproverCheck).ToList();

                var distinctItems = mainResult.GroupBy(x => x.TravelId).Select(y => y.First()).ToList();

                totalCount = distinctItems.Count;
                //return distinctItems.Skip((page - 1) * rows).Take(rows).ToList();
                return distinctItems.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<JourneyList> GetJourneyShowHistory(int TravelID, int DocumentID)
        {
            try
            {
                List<JourneyList> showHistory = new List<JourneyList>();
                showHistory = (from journey in dbContext.Tbl_HR_TravelJourneyDetails
                               where journey.Id == DocumentID && journey.TravelId == TravelID
                               select new JourneyList
                               {
                                   TravelID = journey.TravelId,
                                   JourneyID = journey.Id,
                                   TicketName = journey.TicketName,
                                   JourneyFilePath = journey.UploadPath
                               }).ToList();
                return showHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClientViewModel> GetClientShowHistory(int TravelID, int DocumentID)
        {
            try
            {
                List<ClientViewModel> showHistory = new List<ClientViewModel>();
                showHistory = (from client in dbContext.tbl_HR_Travel_ClientInformation
                               where client.ClientId == DocumentID && client.TravelId == TravelID
                               select new ClientViewModel
                               {
                                   TravelId = client.TravelId,
                                   ClientId = client.ClientId,
                                   ClientInviteLetterName = client.ClientLetterName,
                                   ClientIviteLetterFilePath = client.UploadPath
                               }).ToList();
                return showHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_Travel GetTravelidExtensionform(string TRFNO)
        {
            try
            {
                Tbl_HR_Travel Exttravelid = dbContext.Tbl_HR_Travel.Where(ed => ed.TRFNo == TRFNO).FirstOrDefault();
                return Exttravelid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RejectTravelApprovalFormAdmin(AccomodationViewModel model, int loginEmpId, int travelid)
        {
            bool isDeleted = false;
            Tbl_HR_Travel travelInfo = dbContext.Tbl_HR_Travel.Where(cd => cd.TravelId == travelid).FirstOrDefault();
            if (travelInfo != null && travelInfo.TravelId > 0)
            {
                travelInfo.StageID = 0;
                tbl_HR_TravelStageEvent emp = new tbl_HR_TravelStageEvent();
                emp.TravelId = model.TravelId;
                emp.EventDateTime = DateTime.Now;
                emp.Action = "Rejected";
                emp.FromStageID = travelInfo.StageID;
                emp.ToStageID = 0;
                emp.UserID = loginEmpId;
                emp.Comments = model.Comments;
                dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<bool> GetAllGridStatus(int travelid, int CountryId, int TravelType, string TravelTRFNo)
        {
            try
            {
                bool ClientG = false;
                bool PassG = false;
                bool ContactG = false;
                bool Contact1G = false;
                bool VisaG = false;
                bool JourneyG = false;

                List<bool> Statuslist = new List<bool>();
                Tbl_HR_Travel gridInfo = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                if (gridInfo != null)
                {
                    tbl_HR_Travel_ClientInformation Clientgrid = dbContext.tbl_HR_Travel_ClientInformation.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                    if (Clientgrid == null)
                    {
                        ClientG = true;
                    }
                    int? employeeID = dbContext.Tbl_HR_Travel.Where(x => x.TravelId == travelid).FirstOrDefault().EmployeeId;
                    Tbl_PM_EmployeePassport Passportgrid = dbContext.Tbl_PM_EmployeePassport.Where(travel => travel.EmployeeID == employeeID).FirstOrDefault();
                    if (Passportgrid == null)
                    {
                        PassG = true;
                    }
                    Tbl_HR_TravelContact Contactgrid = dbContext.Tbl_HR_TravelContact.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                    if (Contactgrid == null)
                    {
                        ContactG = true;
                    }
                    Tbl_HR_TravelContactDetails Contactsecondgrid = dbContext.Tbl_HR_TravelContactDetails.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                    if (Contactsecondgrid == null)
                    {
                        Contact1G = true;
                    }
                    tbl_HR_VisaDetailsTravel Visagrid = null;
                    List<tbl_HR_VisaDetailsTravel> visalist = new List<tbl_HR_VisaDetailsTravel>();
                    if (TravelType == 1)
                    {
                        Visagrid = dbContext.tbl_HR_VisaDetailsTravel.Where(travel => travel.ID == travelid && travel.CountryID == gridInfo.TravelToCountry).FirstOrDefault();
                        visalist = dbContext.tbl_HR_VisaDetailsTravel.Where(travel => travel.ID == travelid && travel.CountryID == gridInfo.TravelToCountry).ToList();
                    }
                    else
                    {
                        Visagrid = dbContext.tbl_HR_VisaDetailsTravel.Where(travel => travel.ID == travelid && travel.CountryID == CountryId).FirstOrDefault();
                        visalist = dbContext.tbl_HR_VisaDetailsTravel.Where(travel => travel.ID == travelid && travel.CountryID == CountryId).ToList();
                    }

                    int counter = 0;
                    foreach (var item in visalist)
                    {
                        if (item.ToDate < DateTime.Now)
                            counter = counter + 1;
                    }

                    if (Visagrid == null || counter > 0)
                    {
                        VisaG = true;
                    }
                    else
                        VisaG = false;

                    Tbl_HR_TravelJourneyDetails Journeygrid = dbContext.Tbl_HR_TravelJourneyDetails.Where(travel => travel.TravelId == travelid && travel.TRFNo == TravelTRFNo).FirstOrDefault();
                    if (Journeygrid == null)
                    {
                        JourneyG = true;
                    }

                    Statuslist.Add(ClientG);
                    Statuslist.Add(PassG);
                    Statuslist.Add(ContactG);
                    Statuslist.Add(Contact1G);
                    Statuslist.Add(VisaG);
                    Statuslist.Add(JourneyG);
                }
                return Statuslist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CountryDetailsList> GetTravelCountryDetails()
        {
            List<CountryDetailsList> conutries = new List<CountryDetailsList>();
            try
            {
                conutries = (from country in dbContext.tbl_PM_CountryMaster
                             orderby country.CountryName
                             select new CountryDetailsList
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

        public bool CheckExtensionStageId(string TrfNo)
        {
            bool isDeleted = false;
            TravelDAL dal = new TravelDAL();
            var TRFNoAuto = dal.GetNewExtensionTRFNo(TrfNo);
            bool statustrfno = dal.GetTRFNoIsValide(TRFNoAuto);
            string NewExtTRFNo = string.Empty;
            string trfNofinal = string.Empty;
            while (statustrfno == true)
            {
                TRFNoAuto = dal.GetNewExtensionTRFNo(TRFNoAuto);
                bool finalstatus = dal.GetTRFNoIsValide(TRFNoAuto);
                if (finalstatus == false)
                {
                    statustrfno = false;
                }
            }

            decimal Trfno = Convert.ToDecimal(TRFNoAuto);
            decimal newTrf = decimal.Subtract(Trfno, 0.1M);

            string CheckTrfNo = string.Empty;
            trfNofinal = newTrf.ToString();
            if (trfNofinal.Contains(".0"))
            {
                trfNofinal = Math.Round(newTrf).ToString();
            }
            else
            {
                trfNofinal = newTrf.ToString();
            }

            Tbl_HR_Travel Extenstionid = dbContext.Tbl_HR_Travel.Where(jr => jr.TRFNo == trfNofinal).FirstOrDefault();
            if (Extenstionid != null)
            {
                if (Extenstionid.StageID == 4 || Extenstionid.IsCancelled == true)
                {
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        public bool CheckAdminConveyanceUploadStatus(int travelid)
        {
            try
            {
                bool Status = false;
                Tbl_HR_Travel gridInfo = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                if (gridInfo != null)
                {
                    List<Tbl_HR_TravelJourneyDetails> _tbl_HR_VisaDeatails = (from e in dbContext.Tbl_HR_TravelJourneyDetails
                                                                              where e.TravelId == travelid
                                                                              select e).ToList();

                    foreach (var item in _tbl_HR_VisaDeatails)
                    {
                        if (item.JourneyModeDetail == null || (item.TicketName == null && item.JourneyMode != 5 && item.JourneyMode != 6 && item.JourneyMode != 7 && item.JourneyMode != 8))
                        {
                            Status = true;
                            break;
                        }
                    }
                }
                return Status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckValidationAccTabDeatails(int travelid)
        {
            try
            {
                bool Status = false;
                Tbl_HR_Travel accomodationdetails = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                tbl_HR_Travel_EmployeeTravelRequirement AccDetails = dbContext.tbl_HR_Travel_EmployeeTravelRequirement.Where(ed => ed.TravelId == travelid).FirstOrDefault();
                if (AccDetails != null)
                {
                    if (accomodationdetails.TRFNo.Contains("."))
                    {
                        if (AccDetails.AccomodationMasterID == null || AccDetails.AirportPickupMasterID == null)
                        {
                            Status = true;
                        }
                    }
                    else
                    {
                        if (AccDetails.AccomodationMasterID == null || AccDetails.AirportPickupMasterID == null || AccDetails.LaptopMasterID == null || AccDetails.HardDriveMasterID == null || AccDetails.UsbDriveMasterID == null || AccDetails.EmployeeSoftwaresRequirement == null)
                        {
                            Status = true;
                        }
                    }
                }
                else
                {
                    Status = true;
                }

                return Status;

                //|| AccDetails.ShuttleMasterID == null
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<bool> GetAllAdminStatusGrid(int travelid)
        {
            try
            {
                bool AccomodationG = false;
                bool LocalConveyG = false;
                bool MiscellG = false;

                List<bool> Statuslist = new List<bool>();
                Tbl_HR_Travel gridInfo = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                if (gridInfo != null)
                {
                    tbl_HR_TravelAccomodationDetails Accomodationgrid = dbContext.tbl_HR_TravelAccomodationDetails.Where(travel => travel.TravelId == travelid).FirstOrDefault();
                    if (Accomodationgrid == null)
                    {
                        AccomodationG = true;
                    }
                    tbl_HR_TravelLocalConveyanceDetails Localgrid = dbContext.tbl_HR_TravelLocalConveyanceDetails.Where(travel => travel.TravelID == travelid).FirstOrDefault();
                    if (Localgrid == null)
                    {
                        LocalConveyG = true;
                    }
                    tbl_HR_TravelOtherRequirement MiscleGrid = dbContext.tbl_HR_TravelOtherRequirement.Where(travel => travel.ID == travelid).FirstOrDefault();
                    if (MiscleGrid == null)
                    {
                        MiscellG = true;
                    }

                    Statuslist.Add(AccomodationG);
                    Statuslist.Add(LocalConveyG);
                    Statuslist.Add(MiscellG);
                }
                return Statuslist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string getUserOrganisationRole(int employeeID)
        {
            string Userrole = string.Empty;
            try
            {
                EmployeeDetailsViewModel employeeDetails = new EmployeeDetailsViewModel();
                employeeDetails = (from e in dbContext.HRMS_tbl_PM_Employee
                                   join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into role
                                   from ERole in role.DefaultIfEmpty()
                                   where e.EmployeeID == employeeID
                                   select new EmployeeDetailsViewModel
                                   {
                                       OrgRoleDescription = ERole.RoleDescription,
                                   }).FirstOrDefault();

                Userrole = employeeDetails.OrgRoleDescription;
            }
            catch (Exception)
            {
                throw;
            }
            return Userrole;
        }

        public bool SubmitTravelApprovalForm(AccomodationViewModel empTravel, int loginEmpId, int travelid, string userrole)
        {
            bool isAdded = false;
            try
            {
                Tbl_HR_Travel latestTravelDetail = (from travel in dbContext.Tbl_HR_Travel
                                                    where travel.TravelId == travelid
                                                    orderby travel.CreatedDate descending
                                                    select travel).FirstOrDefault();

                int employeeId = Convert.ToInt32(latestTravelDetail.EmployeeId);

                tbl_HR_TravelStageEvent emp = new tbl_HR_TravelStageEvent();

                bool result;
                if (latestTravelDetail.TRFNo.Contains("."))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                if (result == false)
                {
                    emp.TravelId = travelid;
                    emp.EventDateTime = DateTime.Now;
                    emp.Action = "Approved";
                    emp.FromStageID = latestTravelDetail.StageID;
                    if (emp.FromStageID == 1)
                        emp.ToStageID = 3;
                    else
                        emp.ToStageID = latestTravelDetail.StageID + 1;
                    emp.UserID = loginEmpId;
                    dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                    dbContext.SaveChanges();
                    if (emp.FromStageID == 1)
                        latestTravelDetail.StageID = 3;
                    else
                        latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                }
                else
                {
                    if (result == true && loginEmpId == employeeId)
                    {
                        emp.TravelId = travelid;
                        emp.EventDateTime = DateTime.Now;
                        emp.Action = "Approved";
                        emp.FromStageID = latestTravelDetail.StageID;
                        emp.ToStageID = latestTravelDetail.StageID + 1;
                        emp.UserID = loginEmpId;
                        dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        dbContext.SaveChanges();
                        latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                    }
                    else
                    {
                        if (userrole == "TravelApprover" && result == true)
                        {
                            emp.TravelId = travelid;
                            emp.EventDateTime = DateTime.Now;
                            emp.Action = "Approved";
                            emp.FromStageID = latestTravelDetail.StageID;
                            emp.ToStageID = 3;
                            emp.UserID = loginEmpId;
                            dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                            dbContext.SaveChanges();
                            latestTravelDetail.StageID = 3;
                        }
                        //if (userrole == "GroupHead" && result == true)
                        //{
                        //    emp.TravelId = travelid;
                        //    emp.EventDateTime = DateTime.Now;
                        //    emp.Action = "Approved";
                        //    emp.FromStageID = latestTravelDetail.StageID;
                        //    emp.ToStageID = 3;
                        //    emp.UserID = loginEmpId;
                        //    dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                        //    dbContext.SaveChanges();
                        //    latestTravelDetail.StageID = 3;
                        //}
                        if (result == true && userrole != "GroupHead" && userrole != "TravelApprover")
                        {
                            emp.TravelId = travelid;
                            emp.EventDateTime = DateTime.Now;
                            emp.Action = "Approved";
                            emp.FromStageID = latestTravelDetail.StageID;
                            emp.ToStageID = latestTravelDetail.StageID + 1;
                            emp.UserID = loginEmpId;
                            dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                            dbContext.SaveChanges();
                            latestTravelDetail.StageID = latestTravelDetail.StageID + 1;
                        }
                    }
                }
                dbContext.SaveChanges();

                isAdded = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        public string GetEmployeeCode(int employeeid)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                string employeeID = (from e in dbContext.HRMS_tbl_PM_Employee
                                     where e.EmployeeID == employeeid
                                     select e.EmployeeCode).FirstOrDefault();
                return employeeID;
            }
            catch
            {
                throw;
            }
        }

        public bool RejectTravelApprovalFormAdminForOrgMem(AccomodationViewModel model, int loginEmpId, int travelid, string userrole, string UserRolesOrganSubmited)
        {
            bool isDeleted = false;
            tbl_HR_TravelStageEvent emp = new tbl_HR_TravelStageEvent();
            Tbl_HR_Travel travelInfo = dbContext.Tbl_HR_Travel.Where(cd => cd.TravelId == travelid).FirstOrDefault();
            int employeeId = Convert.ToInt32(travelInfo.EmployeeId);

            if (userrole == "TravelApprover" && loginEmpId != employeeId && (UserRolesOrganSubmited == "Delivery Manager" || UserRolesOrganSubmited == "Account Owners" || UserRolesOrganSubmited == "Group Head"))
            {
                travelInfo.StageID = 0;
                emp.TravelId = model.TravelId;
                emp.EventDateTime = DateTime.Now;
                emp.Action = "Rejected";
                emp.FromStageID = model.StageID;
                emp.ToStageID = 0;
                emp.UserID = loginEmpId;
                emp.Comments = model.Comments;
                dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                dbContext.SaveChanges();
                isDeleted = true;
            }

            if (loginEmpId != employeeId && userrole != "TravelApprover" && (UserRolesOrganSubmited == "Delivery Manager" || UserRolesOrganSubmited == "Account Owners" || UserRolesOrganSubmited == "Group Head"))
            {
                travelInfo.StageID = 0;
                emp.TravelId = model.TravelId;
                emp.EventDateTime = DateTime.Now;
                emp.Action = "Rejected";
                emp.FromStageID = model.StageID;
                emp.ToStageID = 0;
                emp.UserID = loginEmpId;
                emp.Comments = model.Comments;
                dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                dbContext.SaveChanges();
                isDeleted = true;
            }

            if (loginEmpId != employeeId && userrole != "GroupHead" && userrole != "TravelApprover" && UserRolesOrganSubmited == "Management")
            {
                travelInfo.StageID = 0;
                emp.TravelId = model.TravelId;
                emp.EventDateTime = DateTime.Now;
                emp.Action = "Rejected";
                emp.FromStageID = model.StageID;
                emp.ToStageID = 0;
                emp.UserID = loginEmpId;
                emp.Comments = model.Comments;
                dbContext.tbl_HR_TravelStageEvent.AddObject(emp);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        //public List<TCurrencyList> GetCurrencyList()
        //{
        //    List<TCurrencyList> ListCur = new List<TCurrencyList>
        //    {
        //        new TCurrencyList {CurrencyID = 1, CurrencyName = "Rs."},
        //        new TCurrencyList {CurrencyID = 2, CurrencyName = "$"},
        //    };

        //    return ListCur;

        //}

        public List<TCurrencyList> GetCurrencyList()
        {
            List<TCurrencyList> ListCur = new List<TCurrencyList>();

            ListCur = (from country in dbContext.tbl_HR_TravelCurrencyMaster
                       orderby country.CurrencyName
                       select new TCurrencyList
                       {
                           CurrencyID = country.CurrencyID,
                           CurrencyName = country.CurrencyName
                       }).ToList();
            return ListCur;
        }

        public List<FieldChildDetails> GetFieldChildDetailsList(string field)
        {
            try
            {
                List<FieldChildDetails> childDetails = new List<FieldChildDetails>();
                if (field == "Business Group")
                {
                    List<FieldChildDetails> child = (from l in dbContext.tbl_CNF_BusinessGroups
                                                     select new FieldChildDetails
                                                     {
                                                         Id = l.BusinessGroupID,
                                                         Description = l.BusinessGroup
                                                     }).ToList();

                    return child;
                }
                else
                {
                    if (field == "Organization Unit")
                    {
                        List<FieldChildDetails> child = (from l in dbContext.tbl_PM_Location
                                                         where l.LocationID <= 5
                                                         select new FieldChildDetails
                                                         {
                                                             Id = l.LocationID,
                                                             Description = l.Location
                                                         }).ToList();

                        return child;
                    }
                    else
                    {
                        if (field == "Stage Name")
                        {
                            List<FieldChildDetails> child = (from expenseStage in dbContext.tbl_HR_Travel_TravelStages
                                                             select new FieldChildDetails
                                                             {
                                                                 Id = expenseStage.StageID,
                                                                 Description = expenseStage.TravelStageDescription
                                                             }).ToList();

                            return child;
                        }
                        else
                        {
                            return childDetails;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TravelReportViewModel> GetTravelReportForEmployee(int TravelId)
        {
            dbContext = new HRMSDBEntities();

            TravelReportViewModel TravelReportList = (from a in dbContext.Tbl_HR_Travel
                                                      join e in dbContext.HRMS_tbl_PM_Employee on a.EmployeeId equals e.EmployeeID into emp
                                                      from empList in emp.DefaultIfEmpty()
                                                      join q in dbContext.HRMS_tbl_PM_Employee on a.ProjectManagerId equals q.EmployeeID into mgr
                                                      from MgrList in mgr.DefaultIfEmpty()
                                                      join s in dbContext.Tbl_HR_TravelContact on a.TravelId equals s.TravelId into stage
                                                      from stageList in stage.DefaultIfEmpty()
                                                      join d in dbContext.tbl_HR_VisaDetailsTravel on a.TravelId equals d.ID into Visa
                                                      from VisaList in Visa.DefaultIfEmpty()
                                                      join g in dbContext.tbl_PM_GroupMaster on empList.GroupID equals g.GroupID into empGroup
                                                      from groupList in empGroup.DefaultIfEmpty()
                                                      join y in dbContext.tbl_HR_TravelAccomodationDetails on a.TravelId equals y.TravelId into Accmo
                                                      from AccmoList in Accmo.DefaultIfEmpty()
                                                      join p in dbContext.tbl_HR_TravelLocalConveyanceDetails on a.TravelId equals p.TravelID into Covey
                                                      from ConveyList in Covey.DefaultIfEmpty()
                                                      join m in dbContext.tbl_HR_Travel_ClientInformation on a.TravelId equals m.TravelId into Client
                                                      from ClientList in Client.DefaultIfEmpty()
                                                      join k in dbContext.Tbl_HR_TravelJourneyDetails on a.TravelId equals k.TravelId into Journey
                                                      from JourneyreList in Journey.DefaultIfEmpty()
                                                      //join o in dbContext.tbl_HR_TravelOtherRequirement on a.TravelId equals o.ID into other
                                                      //from otherList in other.DefaultIfEmpty()
                                                      where a.TravelId == TravelId
                                                      select new TravelReportViewModel
                                                           {
                                                               travelid = a.TravelId,
                                                               TRFNO = a.TRFNo,
                                                               EmployeeID = a.EmployeeId,
                                                               EmployeeCode = empList.EmployeeCode,
                                                               EmployeeName = empList.EmployeeName,
                                                               Group = groupList.GroupName,
                                                               ReportingManager = MgrList.EmployeeName,
                                                               DepartFromBaseLocation = a.TravelStartDate,
                                                               ArrivalDestination = a.TravelStartDate,
                                                               DepartFromDestination = a.TravelEndDate,
                                                               ArrivalBaseDestination = a.TravelEndDate,
                                                               TavelContactNo = a.ContactNoAbroad,
                                                               HotelAddress = AccmoList.HotelAddress,
                                                               HotelRoomNo = AccmoList.HotelName,
                                                               ClientName = ClientList.ClientName,
                                                               clientAddress = ClientList.ClientAddress,
                                                               ClientContactPerson = ClientList.ClientContact,
                                                               ClientContactNo = ClientList.ClientContactNumber,
                                                               VisaValiditydate = VisaList.Decription,
                                                               //InsurenceDetails = otherList.Details,
                                                               TravelStatus = null,
                                                               Comments = null,
                                                               TicketName = JourneyreList.TicketName,
                                                               TicketAttachment = JourneyreList.UploadPath,
                                                               //InsurenceAttachment = null
                                                           }).FirstOrDefault();

            List<TravelReportViewModel> test = new List<TravelReportViewModel>();
            test.Add(TravelReportList);
            return test;

            //return TravelReportList;
        }

        public List<OtherAdminViewModel> GetOtherRequirementDetailsList(int TravelId)
        {
            List<OtherAdminViewModel> OtherReq = (from otherdetails in dbContext.tbl_HR_TravelOtherRequirement
                                                  where otherdetails.ID == TravelId
                                                  select new OtherAdminViewModel
                                          {
                                              FileName = otherdetails.FileName,
                                              FilePath = otherdetails.FilePath,
                                              ID = otherdetails.ID,
                                              RequirementID = otherdetails.RequirementID,
                                              InsurenceDetails = otherdetails.Details
                                          }).ToList();
            return OtherReq.ToList();
        }

        public List<ClientViewModel> GetClientContactList(int TravelId)
        {
            List<ClientViewModel> ClientList2 = (from client in dbContext.tbl_HR_Travel_ClientInformation
                                                 join location in dbContext.tbl_HR_Travel_TravellingLocation on client.TravellingLocation equals location.TravelingLocationId into loc
                                                 from locations in loc.DefaultIfEmpty()
                                                 //join projectname in dbContext.tbl_Client_ProjectNamesMaster on client.ClientName equals projectname.ProjectNameID into proj
                                                 //join projectname in ClientList1 on client.ClientName equals projectname.Customer into proj
                                                 //from project in proj.DefaultIfEmpty()
                                                 where client.TravelId == TravelId
                                                 select new ClientViewModel
                                                 {
                                                     ClientId = client.ClientId,
                                                     //ClientName = project.CustomerName,
                                                     //ProjectNameId = project.Customer,
                                                     ClientNameId = client.ClientName,
                                                     ClientEmailId = client.ClientEmailID,
                                                     ClientAddress = client.ClientAddress,
                                                     ClientContactNumber = client.ClientContactNumber,
                                                     ClientContact = client.ClientContact,
                                                     PurposeOfVisit = client.ClientVisitPurpose,
                                                     TravellingLocName = locations.TravellingLocationName,
                                                     TravellingLocId = locations.TravelingLocationId,
                                                     ClientInviteLetterName = client.ClientLetterName,
                                                     ClientIviteLetterFilePath = client.UploadPath,
                                                     TravelId = client.TravelId,
                                                     ProspectName = client.ClientProspectName
                                                 }).ToList();

            foreach (var i in ClientList2)
            {
                i.ProjectNameId = (from d in dbSEMContext.tbl_PM_Customer
                                   where d.Customer == i.ClientNameId
                                   select d.Customer).FirstOrDefault();

                i.ClientName = (from f in dbSEMContext.tbl_PM_Customer
                                where f.Customer == i.ClientNameId
                                select f.CustomerName).FirstOrDefault();
            }
            return ClientList2.ToList();
        }

        public List<AccomodationAdmin> GetAccomodationDetailsList(int TravelId)
        {
            List<AccomodationAdmin> AccoList = (from acco in dbContext.tbl_HR_TravelAccomodationDetails
                                                where acco.TravelId == TravelId
                                                select new AccomodationAdmin
                                                {
                                                    HotelAddress = acco.HotelAddress,
                                                    HotelContactNumber = acco.HotelName,
                                                    TravelId = TravelId
                                                }).ToList();
            return AccoList.ToList();
        }

        public List<JourneyList> GetJourneyDetailsList(int TravelId)
        {
            List<JourneyList> JourneyList = (from jour in dbContext.Tbl_HR_TravelJourneyDetails
                                             where jour.TravelId == TravelId
                                             select new JourneyList
                                                   {
                                                       TicketName = jour.TicketName,
                                                       JourneyFilePath = jour.UploadPath,
                                                       TravelID = TravelId
                                                   }).ToList();
            return JourneyList.ToList();
        }

        public List<ConveyanceAdminViewModel> GetTravelToHotelList(int TravelId)
        {
            List<ConveyanceAdminViewModel> ConveyList = (from Convaynancedetails in dbContext.tbl_HR_TravelLocalConveyanceDetails
                                                         join visatype in dbContext.tbl_HR_Travel_JourneyMode on Convaynancedetails.ConveyanceType equals visatype.JourneyModeID
                                                         where Convaynancedetails.TravelID == TravelId
                                                         select new ConveyanceAdminViewModel
                                                         {
                                                             ConvayName = visatype.JourneyModeDescription,
                                                             TravelDetails = Convaynancedetails.TravelDetails,
                                                             TravelID = TravelId
                                                         }).ToList();

            return ConveyList.ToList();
        }

        public List<EmergencyContactViewModel> GetAllEmergencyContacts(int TravelId)
        {
            List<EmergencyContactViewModel> EmergencyContactList = (from contact in dbContext.Tbl_HR_TravelContactDetails
                                                                    where contact.TravelId == TravelId
                                                                    orderby contact.TravelId descending
                                                                    select new EmergencyContactViewModel
                                                    {
                                                        EmployeeEmergencyContactId = contact.Id,
                                                        Name = contact.Name,
                                                        EmailId = contact.EmailId,
                                                        ContactNo = contact.ContactNo,
                                                        EmgAddress = contact.Address,
                                                        Relation = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                                                    where relation.UniqueID == contact.Relationship
                                                                    select relation.RelationType).FirstOrDefault(),
                                                        uniqueID = contact.Relationship,
                                                        IsAddedFromVB = contact.IsAddedFromVB.HasValue ? contact.IsAddedFromVB.Value : false,
                                                        TravelID = TravelId
                                                    }).ToList();

            return EmergencyContactList.ToList();
        }

        public List<TravellingLocationList> GetTravellingLocation()
        {
            List<TravellingLocationList> travellinglocation = new List<TravellingLocationList>();
            try
            {
                travellinglocation = (from client in dbContext.tbl_HR_Travel_TravellingLocation
                                      select new TravellingLocationList
                                      {
                                          TravellingLocationId = client.TravelingLocationId,
                                          TravellingLocationName = client.TravellingLocationName
                                      }).ToList();
                return travellinglocation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GetTravelID> GetTravelIDs()
        {
            try
            {
                List<GetTravelID> employeeresult = (from E in dbContext.Tbl_HR_Travel
                                                    where E.StageID == 4
                                                    select new GetTravelID
                                                    {
                                                        StageId = E.StageID,
                                                        TravelId = E.TravelId
                                                    }).ToList();

                return employeeresult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AccomodationAdmin GetAdminAccomodationShowHistory(int? TravelID, int AccomodationID)
        {
            try
            {
                AccomodationAdmin otherdetailshistory = new AccomodationAdmin();
                otherdetailshistory = (from otherdetails in dbContext.tbl_HR_TravelAccomodationDetails
                                       where otherdetails.AccommodationID == AccomodationID && otherdetails.TravelId == TravelID
                                       select new AccomodationAdmin
                                       {
                                           FileName = otherdetails.FileName,
                                           FilePath = otherdetails.FilePath,
                                           TravelId = otherdetails.TravelId,
                                           AccomodationID = otherdetails.AccommodationID
                                       }).FirstOrDefault();
                return otherdetailshistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CheckPassportValid checkValidEmployeeVisaDetail(int employeeId, int countryId)
        {
            tbl_PM_EmployeeVisaDetails tbl_PM_EmployeeVisaDetails = new tbl_PM_EmployeeVisaDetails();
            CheckPassportValid CheckPassportValid = new CheckPassportValid();

            int isVisaRequired = CheckCountryRequiresVisa(countryId);
            if (isVisaRequired != 0)
            {
                CheckPassportValid.IsVisaRequired = false;
                CheckPassportValid.IsVisaExist = false;
                CheckPassportValid.IsVisaValid = false;
            }
            else
            {
                tbl_PM_EmployeeVisaDetails = dbContext.tbl_PM_EmployeeVisaDetails.Where(x => x.EmployeeID == employeeId && x.CountryID == countryId).FirstOrDefault();
                if (tbl_PM_EmployeeVisaDetails != null)
                {
                    CheckPassportValid.IsVisaValid = tbl_PM_EmployeeVisaDetails.ValidUpto < DateTime.Now ? false : true;
                    CheckPassportValid.IsVisaExist = true;
                    CheckPassportValid.IsVisaRequired = true;
                }
                else
                {
                    CheckPassportValid.IsVisaRequired = true;
                    CheckPassportValid.IsVisaExist = false;
                    CheckPassportValid.IsVisaValid = false;
                }
            }
            return CheckPassportValid;
        }

        public int CheckCountryRequiresVisa(int countryId)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            SqlParameter[] objParam = new SqlParameter[1];

            objParam[0] = new SqlParameter("@CountryID", SqlDbType.Int);
            objParam[0].Value = countryId;

            try
            {
                var count = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "sp_CheckCountryRequiresVisa", objParam);
                return count != null ? Convert.ToInt32(count) : 0;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}