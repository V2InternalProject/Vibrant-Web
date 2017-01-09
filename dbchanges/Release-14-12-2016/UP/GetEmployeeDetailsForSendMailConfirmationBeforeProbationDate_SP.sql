USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[GetEmployeeDetailsForSendMailConfirmationBeforeProbationDate_SP]    Script Date: 11/23/2016 11:39:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetEmployeeDetailsForSendMailConfirmationBeforeProbationDate_SP]
	-- Add the parameters for the stored procedure here
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT he.EmployeeName AS EmployeeName,
	he1.EmployeeName AS EmployeeName1,
	he.EmailID AS EmailId,
	he1.EmailID AS EmailId1,
	he.Probation_Review_Date 

	FROM 
	HRMS_tbl_PM_Employee he JOIN 
    HRMS_tbl_PM_Employee he1 ON (he.ReportingTo=he1.EmployeeID)

   WHERE he.EmployeeID NOT IN (SELECT EmployeeID FROM tbl_CF_Confirmation CF
   WHERE he.EmployeeID=CF.EmployeeID) AND he.Status=0 AND he.EmployeeStatusMasterID=1 AND he.ConfirmationDate IS NULL AND he.ConfirmationStatus IS NULL AND he.EmployeeStatusID=5 AND 
   (he.Probation_Review_Date = (select dateadd(day, 3, CAST(getdate() as date))) OR he.Probation_Review_Date=CAST(getdate() as date))
END

GO


