USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetSelectedIssueForSuperAdmin]    Script Date: 6/13/2016 12:42:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 --dbo.sp_GetSelectedIssueForSuperAdmin   21091,2565

ALTER PROCEDURE [dbo].[sp_GetSelectedIssueForSuperAdmin]  
  
@IssueAssignmentID int,  
  
@UserID int  
  
AS  
  
  
  
--select DISTINCT i1.IssueAssignmentID, i1.EmployeeID, i1.ReportIssueID, r1.StatusID,  i1.Cause, i1.Fix, convert(varchar(12), r1.ReportIssueDate, 101) as ReportIssueDate,  
  
--convert(varchar(12), r1.ReportCloseDate, 101) as ReportCloseDate, r1.Name, r1.ProblemSeverityID, ps1.ProblemSeverity, r1.ProblemPriorityID,c1.Category  
  
------, p1.ProblemPriority  
  
--, r1.Description, r1.SubCategoryID, s1.SubCategory ,r1.Comments,r1.DescriptionAndComments   
  
--from tblReportIssue r1, tblIssueAssignment i1, tblProblemSeverityMaster ps1, tblSubCategoryMaster s1,tblCategoryMaster c1  
  
-----, tblProblemPriorityMaster p1  
  
--where r1.ReportIssueID = i1.ReportIssueID  
  
------ and r1.ProblemPriorityID = p1.ProblemPriorityID  
  
--and r1.ProblemSeverityID = ps1.ProblemSeverityID  
  
--and r1.SubCategoryID = s1.SubCategoryID  
  
----and i1.IssueAssignmentID = @IssueAssignmentID  
  
--and r1.ReportIssueID =  @IssueAssignmentID --actually @IssueAssignmentID is ReportIssueID  
  
--and s1.CategoryID= c1.CategoryID  
  
--Order by i1.IssueAssignmentID desc  
  
  
  
  
  
select DISTINCT i1.IssueAssignmentID, i1.EmployeeID,r1.ReportingToId, r1.ReportIssueID, s.pk_statusid as StatusID,s.StatusDesc as CurrentStatus , i1.Cause, i1.Fix, convert(varchar(12), r1.ReportIssueDate, 101) as ReportIssueDate, r1.ReportIssueDate as ReportIssueDateTime,
  
convert(varchar(12), r1.ReportCloseDate, 101) as ReportCloseDate,r1.PhoneExt, r1.SeatingLocation, r1.Name, r1.ProblemSeverityID, ps1.ProblemSeverity, r1.ProblemPriorityID,c1.Category  
  
----, p1.ProblemPriority  
  
, r1.Description,r1.TypeID,requestType as Type, r1.SubCategoryID, s1.SubCategory ,r1.Comments,r1.DescriptionAndComments  
  
,proj.projectName,empRole.RoleDescription,empName.employeename,rscPool.ResourcePoolName,  
  
 r1.WorkHours,convert(varchar(12), r1.FromDate, 101) as FromDate,convert(varchar(12), r1.ToDate, 101) as ToDate,r1.NumberOfResources,  
  
 convert(varchar(12),isnull(pmProject.actualStartDate,pmProject.ExpectedStartDate), 101) as actualStartDate,  
  
 convert(varchar(12),isnull( pmProject.actualEndDate,pmProject.ExpectedEndDate), 101) as actualEndDate,  
  
 ReportEmp.employeeid,r1.projectnameid  , ReportEmp.employeeid as ReportingToID1
  
from tblReportIssue r1 inner join  
  
tblProblemSeverityMaster ps1 on r1.ProblemSeverityID = ps1.ProblemSeverityID inner join  
  
 tblSubCategoryMaster s1 on r1.SubCategoryID = s1.SubCategoryID inner join  
  
 tblCategoryMaster c1 on s1.CategoryID= c1.CategoryID left join tblIssueAssignment i1 on r1.ReportIssueID = i1.ReportIssueID left join  
  
 RequestType rt on r1.typeid = rt.typeid left join  
  
 helpdeskstatus s on s.pk_statusid = r1.statusid  
  
 left join v2intranet.[dbo].tbl_pm_project proj on proj.projectId = r1.projectNameId  
  
 left join v2intranet.[dbo].tbl_pm_role empRole on empRole.RoleId = r1.projectroleId  
  
 left join HRMS_tbl_PM_Employee empName on empName.employeeid = r1.reportingToId  
  
 left join tbl_PM_ResourcePoolMaster rscPool on rscPool.resourcepoolId = r1.resourcepoolId  
  
 left join v2intranet.[dbo].[tbl_PM_Project] pmProject on pmProject.projectId = r1.projectNameId  
  
 left join v2intranet.dbo.tbl_pm_employee ReportEmp on ReportEmp.username = r1.name  
  
---, tblProblemPriorityMaster p1  
  
where   
  
---- and r1.ProblemPriorityID = p1.ProblemPriorityID  
  
--and   
  
--and  
  
--and i1.IssueAssignmentID = @IssueAssignmentID  
  
 r1.ReportIssueID =  @IssueAssignmentID --actually @IssueAssignmentID is ReportIssueID  
  
  
Order by i1.IssueAssignmentID desc  
  
  
  
select EmailID,UserName from HRMS_tbl_pm_Employee where EmployeeCode = @UserID  
  
--select * from tblReportIssue  
  
select top 1 v.Emp_User_Name  as Emp_User_Name from tblIssueAssignment a ,vwPMSEmployee v   where  
  
 v.Emp_User_Name = a.EmployeeID and a.statusid =5 and  
  
 a.ReportIssueID = @IssueAssignmentID  
  
  
GO