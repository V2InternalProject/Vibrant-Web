USE [V2Intranet]
GO

/****** Object:  Trigger [dbo].[Trg_ConfirmationMngrUpdate]    Script Date: 7/12/2016 3:02:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Rahul Ramachandran
-- Create date: 01st July 2016
-- Description:	Update the reporting Manager in Confirmation Table according to the reporting to in Employee Table.
-- =============================================
CREATE TRIGGER [dbo].[Trg_ConfirmationMngrUpdate]
       ON [dbo].[HRMS_tbl_PM_Employee]
AFTER UPDATE
AS
BEGIN
       SET NOCOUNT ON;
 
       DECLARE @EmployeeID INT
       DECLARE @ReportingTo INT
 
       
	   SELECT @EmployeeID=inserted.EmployeeID
	   FROM inserted    
	   SELECT @ReportingTo = inserted.ReportingTo  
       FROM INSERTED
		print @EmployeeID
		print @ReportingTo
       IF UPDATE(reportingto)
       BEGIN
	   print 'x'
                Update tbl_CF_Confirmation
				set ReportingManager=@ReportingTo
				where employeeid=@EmployeeID
	  print 'y'
       END 
END
GO


