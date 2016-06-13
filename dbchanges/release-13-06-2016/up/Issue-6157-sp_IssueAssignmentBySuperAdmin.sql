USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[sp_IssueAssignmentBySuperAdmin]    Script Date: 6/13/2016 12:43:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


 --exec sp_IssueAssignmentBySuperAdmin 5,0,21240,'test','te',2036,120,1,9,null,null,'6/9/2016 2:58:35 PM',0
 
ALTER    PROCEDURE [dbo].[sp_IssueAssignmentBySuperAdmin]  
@StatusID int,
@IssueAssignmentID int,
@ReportIssueID int,
@Cause varchar(255) = null,
@Fix varchar(255) = null,
@EmployeeID int = null ,
@SubCategory int,
@TypeID int,
@ProblemSeverity int,
@WorkHours int,
@FromDate datetime = null,
@ToDate datetime = null,
@IssueReportDateTime datetime = null,
@NumberOfResources int
 AS 
  
Begin  
 
	DECLARE @MaxIssueAssignmentID int  
create table #temp (Subcategoryid int) 
insert into #temp select Subcategoryid from tblSubCategoryMaster where categoryid=11

create table #temp2 (IssueName varchar(100),IssueDescription varchar(1500))
insert into #temp2 select ReportIssueID,Description from tblReportIssue where Reportissueid=@ReportIssueID
	--If the Status ID is 2(Resolved), then Update the tblIssueAssignment with IssueResolved Date.  
	if @StatusID=3  
		Begin  
		
			Insert into tblIssueAssignment(EmployeeID, ReportIssueID,cause,fix, StatusID, IssuedAssignmentDate, IssueResolvedDate)   
			VALUES(@EmployeeID,  @ReportIssueID,@Cause,@Fix, @StatusID, @IssueReportDateTime, getdate())  

			SET @MaxIssueAssignmentID = (SELECT MAX(IssueAssignmentID) FROM tblIssueAssignment ia WHERE ia.ReportIssueID = @ReportIssueID)  
			------------------------------------------------------------------------------------  
			-------------------------------****IMPORTANT NOTE****-------------------------------  
			  
			----This Stored procedure has to be executed each time the issue is resolved.--  
			----Set the identity first and then pass the same to the stored procedure.(eg. @UpdatedIssueAssignmentID in this case)--  
			EXEC dbo.sp_UpdateResolutionTimeandHealth_New @MaxIssueAssignmentID, @ReportIssueID  
			  
			-------------------------------*********************--------------------------------  
			------------------------------------------------------------------------------------  
			  
			Update tblReportIssue  
			Set StatusID = @StatusID,TypeID=@TypeID,ProblemSeverityID=@ProblemSeverity,ReportCloseDate=getdate(),  
			workHours=@WorkHours,fromdate =@FromDate,todate=@ToDate,numberofresources =@NumberOfResources  
			  where ReportIssueID = @ReportIssueID  
  
		end  
		
-- if Issue is Assigned to particular user (Status ID is 5 [AssignedTo]) then new task is generated against that user.
	else if( @StatusID=5 and (@SubCategory =(select Subcategoryid from #temp where Subcategoryid=@SubCategory )))

		begin

			Declare @defaultStatusID int
			set @defaultStatusID=(Select LookUpTypeId from [tbl_PM_LookUp] where dataTypeValue = 'TaskStatus' and Value = 'Open')

			

			 Insert into tbl_PM_ProjectTask(TaskName,StartDate,EndDate,PlannedHours,[Status],[Description],AssignedTo,CreatedDate,ModifiedDate)        
				Values(('HelpDesk Issue No.: '+ (select IssueName from #temp2)),GETDATE(),DATEADD(day,2,GETDATE()),120.00,@defaultStatusID,(select IssueDescription from #temp2),@EmployeeID,GETDATE(),GETDATE()) 
			

		end   
	else 
  
		begin  
					  
			Insert into tblIssueAssignment(EmployeeID, ReportIssueID,cause,fix, StatusID , IssuedAssignmentDate)   
			VALUES(@EmployeeID,  @ReportIssueID,@Cause,@Fix, @StatusID , @IssueReportDateTime) 
			
			Update tblReportIssue  
			Set StatusID = @StatusID ,TypeID=@TypeID,ProblemSeverityID=@ProblemSeverity,subCategoryId=@SubCategory,  
			workHours=@WorkHours,fromdate =@FromDate,todate=@ToDate,numberofresources =@NumberOfResources  
			  where ReportIssueID = @ReportIssueID   
			  
		end  
				    
drop table #temp 
drop table #temp2    
END  
  
  

GO