using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class ReportDAL
    {
        private WSEMDBEntities dbContext = new WSEMDBEntities();

        public List<Report> GetReportList(int? EmployeeID)
        {
            try
            {
                var reportDetails = dbContext.v2_getReportList(EmployeeID);
                List<Report> ReportList = new List<Report>();
                ReportList = (from m in reportDetails
                              select new Report
                                   {
                                       ReportID = m.reportID,
                                       ReportName = m.reportName,
                                       ReportDescription = m.reportDescription,
                                       ReportFileName = m.rptFileName
                                   }).ToList();
                return ReportList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReportParamterMaster> GetReportData(int ReportID, int employeeID)
        {
            try
            {
                var reportDetails = dbContext.v2_getReportParamMaster(ReportID, employeeID);
                List<ReportParamterMaster> ReportParamList = new List<ReportParamterMaster>();
                ReportParamList = (from m in reportDetails
                                   select new ReportParamterMaster
                              {
                                  ReportParamID = m.reportParamId,
                                  ReportID = m.reportId,
                                  ReportParamName = m.reportParamName.TrimStart('@'),
                                  ReportParamType = m.reportParamType,
                                  ReportParamDefaultValue = m.reportParamDefaultValue,
                                  ReportParamDescription = m.reportParamDescription,
                                  DropDownList = GetDropDownDataIfAvailable(dbContext, m.reportParamSP, employeeID, null, ReportID, m.reportParamName.TrimStart('@')),
                                  CascadeDD = m.CascadeDD,
                                  All = m.All,
                                  Team = m.Team
                              }).ToList();
                return ReportParamList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownValues> GetDropDownDataIfAvailable(WSEMDBEntities context, string procName, int? employeeID, string optParm1, int? ReportID, string reportParamName)
        {
            List<DropDownValues> values = new List<DropDownValues>();
            var reportDetailsTeam = dbContext.v2_getReportParamMaster(ReportID, employeeID);
            var reportDetailsAll = dbContext.v2_getReportParamMaster(ReportID, employeeID);
            List<string> strTeam = reportDetailsTeam.Where(t => t.Team != null && t.reportParamName == reportParamName).Select(t => t.Team).ToList();
            List<string> strAll = reportDetailsAll.Where(t => t.All != null && t.reportParamName == reportParamName).Select(t => t.All).ToList();
            string[] team = { };
            string[] all = { };
            foreach (var item in strTeam)
            {
                if (item != null)
                    team = item.Split(',');
            }
            foreach (var item in strAll)
            {
                if (item != null)
                    all = item.Split(',');
            }
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            if (procName != null)
            {
                var DropDownValues = dbContext.GetDropDownFillValues(procName, employeeID, optParm1, null, null, null, null);
                List<DropDownValues> DropDownList = new List<DropDownValues>();
                DropDownList = (from m in DropDownValues
                                select new DropDownValues
                              {
                                  Key = Convert.ToString(m.Key),
                                  Value = m.Value
                              }).ToList();

                DropDownValues selfValue = new DropDownValues()
                {
                    Key = "-1",
                    Value = "Self"
                };

                DropDownValues allValue = new DropDownValues()
                {
                    Key = "-3",
                    Value = "All"
                };

                DropDownValues teamValue = new DropDownValues()
                {
                    Key = "-2",
                    Value = "Team"
                };

                if (strTeam.Count != 0 || strAll.Count != 0)
                    DropDownList.Insert(0, selfValue);

                DropDownList.RemoveAll(x => x.Key == Membership.GetUser().UserName);

                if (CheckIfValueExist(all, role))
                {
                    if (DropDownList.Contains(selfValue))
                    {
                        DropDownList.Insert(1, allValue);
                    }
                    else
                    {
                        DropDownList.Insert(0, allValue);
                    }
                }

                if (CheckIfValueExist(team, role))
                {
                    if (DropDownList.Contains(selfValue) && DropDownList.Contains(allValue))
                    {
                        DropDownList.Insert(2, teamValue);
                    }
                    else if (DropDownList.Contains(selfValue) && !DropDownList.Contains(allValue))
                    {
                        DropDownList.Insert(1, teamValue);
                    }
                    else if (!DropDownList.Contains(selfValue) && DropDownList.Contains(allValue))
                    {
                        DropDownList.Insert(1, teamValue);
                    }
                    else if (!DropDownList.Contains(selfValue) && !DropDownList.Contains(allValue))
                    {
                        DropDownList.Insert(0, teamValue);
                    }
                }

                values = DropDownList.ToList();
            }
            return values;
        }

        private bool CheckIfValueExist(string[] from, string[] into)
        {
            foreach (var f in from)
            {
                foreach (var i in into)
                {
                    if (f == i)
                        return true;
                }
            }
            return false;
        }

        public Tuple<bool, Guid> SaveFormData(List<ControlsList> Controls)
        {
            try
            {
                bool isInserted = false;
                Guid newGuid = Guid.NewGuid();
                var typeId = 0;
                foreach (var item in Controls)
                {
                    typeId = Convert.ToInt32(item.TypeId);
                    break;
                }
                List<tbl_PM_reportParamMaster> details = dbContext.tbl_PM_reportParamMaster.Where(x => x.reportId == typeId).ToList();
                foreach (var item in details)
                {
                    temp_reportParamMaster ReportDeatils = new temp_reportParamMaster();
                    ReportDeatils.reportSessionId = newGuid;
                    ReportDeatils.reportParamId = item.reportParamId;
                    ReportDeatils.reportId = item.reportId;
                    ReportDeatils.reportParamName = item.reportParamName;
                    ReportDeatils.reportParamType = item.reportParamType;
                    foreach (var items in Controls)
                    {
                        if (items.id == item.reportParamName)
                            ReportDeatils.reportParamValue = items.value;
                    }
                    ReportDeatils.reportParamDescription = item.reportParamDescription;
                    ReportDeatils.reportParamSP = item.reportParamSP;
                    ReportDeatils.createdDate = DateTime.Now;
                    dbContext.temp_reportParamMaster.AddObject(ReportDeatils);
                    isInserted = true;
                }
                dbContext.SaveChanges();
                return new Tuple<bool, Guid>(isInserted, newGuid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportSchema GetReportSchema(Guid reportSessionID, int reportID)
        {
            var reportSchema = new ReportSchema();
            ObjectParameter reportName = new ObjectParameter("reportName", typeof(string));
            var reportParameters = dbContext.GetReportParameters(reportSessionID, reportID, reportName);
            List<SqlParameter> paramList = new List<SqlParameter>();
            var report = dbContext.tbl_PM_reportMaster.Where(t => t.reportID == reportID).FirstOrDefault();

            foreach (var reportParam in reportParameters)
            {
                var param = new SqlParameter("@" + reportParam.reportParamName, reportParam.inputType);
                param.Value = reportParam.reportParamValue;
                if (reportParam.reportParamName == "LoggedInUser" || reportParam.reportParamName == "LoggedInEmployee")
                    param.Value = Membership.GetUser().UserName;

                paramList.Add(param);
            }

            reportSchema.Name = report.reportName;
            reportSchema.ProcName = report.rptSPName;
            reportSchema.FileName = report.rptFileName;
            reportSchema.ProcInputItemList = paramList;

            return reportSchema;
        }

        public ReportMaster GetReportMasterData(int reportID)
        {
            ReportMaster reportMaster = new ReportMaster();
            var tmpReportData = dbContext.GetReportMasterData(reportID);
            reportMaster = (from m in tmpReportData
                            select new ReportMaster
                            {
                                ReportID = m.reportID,
                                ReportDatasetName = m.rptDataSetName
                            }).FirstOrDefault();
            return reportMaster;
        }
    }
}