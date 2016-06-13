USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[sp_InsertIssueDetails]    Script Date: 6/13/2016 12:39:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[sp_InsertIssueDetails]         
(
	 @Name varchar(100), --commented on july 17        
	 @EmailID varchar(100),
	 @CCEmailID varchar(100),
	 @PhoneExtension varchar(100),
	 @SeatingLocation varchar(100),
	 @SubCategoryID int,
	 @SeverityID int,
	 @UploadedFileName varchar(100),
	 @UploadedFileExtension varchar(50),
	 @Description varchar(1000),
	 @StatusID int,
	 @TypeID int,
	 --Added By Nikhil For PMS Category  
	 --Start  
	 @ProjectNameId int,
	 @ProjectRoleId int,
	 @WorkHours int,
	 @FromDate datetime = null,
	 @ToDate datetime = null,
	 @NumberOfResources int,
	 @ResourcePoolId int,
	 @ReportingToId int
	 --end  
	 --Adding now on july 17 by puja        
	 --@NameProvided varchar(100)        
)

AS

BEGIN TRANSACTION T1
	DECLARE @ReportIssueID INT
	DECLARE @intErrorCode INT

	SET @ReportIssueID = (SELECT ISNUll(max(ReportIssueID),0) from tblReportIssue) + 1
	INSERT INTO tblReportIssue          
	(
		Name,MailId, CCMailID, PhoneExt, SeatingLocation, SubCategoryID, ProblemSeverityID, Description,TypeID,  
		StatusID, ReportIssueDate,ProjectNameId,ProjectRoleId,WorkHours,FromDate,ToDate,NumberOfResources,ResourcePoolId,ReportingToId
	)
	VALUES
	(
		@Name,@EmailID, @CCEmailID, @PhoneExtension, @SeatingLocation, @SubCategoryID, @SeverityID, @Description,@TypeID,  
		@StatusID, getdate(),@ProjectNameId,@ProjectRoleId,@WorkHours,@FromDate,@ToDate,@NumberOfResources,@ResourcePoolId,@ReportingToId
	)

	SELECT @intErrorCode = @@ERROR
    IF (@intErrorCode <> 0) GOTO PROBLEM

	--Update the uploaded file to the blReportIssueID, to the same record, which is added above        
	IF(@UploadedFileName != '0' AND @UploadedFileExtension != '0')        
	BEGIN        
		--UPDATE tblReportIssue SET FileName = @UploadedFileName+'_'+convert(varchar,@ReportIssueID)+@UploadedFileExtension WHERE ReportIssueID = @ReportIssueID   
		INSERT INTO [dbo].[tblFileInfo]  
		(reportIssueID,FileName)values  
		(@reportIssueID,@UploadedFileName+'_'+convert(varchar,@ReportIssueID)+@UploadedFileExtension)

		SELECT @intErrorCode = @@ERROR
		IF (@intErrorCode <> 0) GOTO PROBLEM
	END

	IF(@ReportIssueID = (select ISNUll(max(ReportIssueID),0) from tblReportIssue))   
	BEGIN  
		select ISNUll(max(ReportIssueID ),0)as IssueId from tblReportIssue

		SELECT @intErrorCode = @@ERROR
		IF (@intErrorCode <> 0) GOTO PROBLEM
	END
COMMIT TRANSACTION T1

PROBLEM:
IF (@intErrorCode <> 0) 
BEGIN
    ROLLBACK TRANSACTION T1
END

GO