USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[GetMailDetails]    Script Date: 11/23/2016 11:40:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Nikhil Kandalkar>
-- Create date: <15/4/2014>
-- Description:	<To Get Details for auto trigger mail of resource allocation>
-- =============================================
ALTER PROCEDURE [dbo].[GetMailDetails]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- get auto generated mail details
Select PRE.projectId,proj.projectName,PRE.employeeId,convert(varchar(12), PRE.expectedEndDate, 101) AllocationEndDate,
	empEmployee.employeename EmpName,empEmployee.emailId EmpEMailId,her.employeename ManagerName,her.emailId ManagerEmailId from
	 tbl_PM_ProjectEmployeeRole PRE 
	 left join Tbl_PM_Project proj on proj.projectId = PRE.projectId 
	 left join tbl_pm_employee empEmployee on empEmployee.employeeid = PRE.employeeId 
	 left join HRMS_tbl_PM_Employee he on (empEmployee.EmployeeCode=he.EmployeeCode)
	 left join HRMS_tbl_PM_Employee her on (he.ReportingTo=her.EmployeeID)
	 left join tbl_pm_employee empManager on empManager.employeeid = empEmployee.reportingTo 
	where (
	  PRE.expectedEndDate = (select dateadd(day, 15, CAST(getdate() as date)))
	 OR PRE.expectedEndDate =(select dateadd(day, 0, CAST(getdate() as date))) 
	OR PRE.expectedEndDate = (select dateadd(day, 3, CAST(getdate() as date)))
	OR PRE.expectedEndDate=(select dateadd(day, 2, CAST(getdate() as date)))
	OR PRE.expectedEndDate=(select dateadd(day, 1, CAST(getdate() as date)))
	) and he.Status=0 and he.EmployeeStatusMasterID=1 --PRE.expectedEndDate = convert(date,dateadd(day,7,getdate())) and he.Status=0 and he.EmployeeStatusMasterID=1 
END

GO


