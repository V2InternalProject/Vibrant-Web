USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[GetPM_DMforMailalert]    Script Date: 11/23/2016 11:42:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPM_DMforMailalert]
	-- Add the parameters for the stored procedure here
	@ProjectID int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select r.EmployeeID,he.EmployeeCode,he.EmailID,ds.DesignationName,he.EmployeeName from tbl_PM_ProjectEmployeeRole r 
join tbl_pm_employee te on (r.EmployeeID=te.EmployeeID)
join tbl_PM_Project pr on (r.ProjectID=pr.ProjectID)
join HRMS_tbl_PM_Employee he on (he.EmployeeCode=te.EmployeeCode)
join HRMS_tbl_PM_DesignationMaster ds on (he.DesignationID=ds.DesignationID)
where 
r.ProjectID=@ProjectID and ds.DesignationID in (20,66) and he.Status=0 and he.EmployeeStatusMasterID=1
END

GO


