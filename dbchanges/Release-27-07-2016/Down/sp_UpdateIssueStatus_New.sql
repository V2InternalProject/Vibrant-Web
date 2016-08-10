USE [V2Intranet]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateIssueStatus_New]    Script Date: 7/29/2016 5:51:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_UpdateIssueStatus_New] --[sp_UpdateIssueStatus_New] 'testMessage2',46,25957,1,'Anushree Tirwadkar:'
(
	@comments varchar(1000),
	@EmployeeId varchar(100),
	@SubCategoryID int,
	@reportIssueID int,
	@StatusID int,
	@counter int
	--@UserName varchar(200)
)
AS
--Declate @date DateTime
--set @date = 
--DECLARE @EmployeeID varchar(100)
--BEGIN 
--SET @EmployeeID= (select 
--top 1 a.EmployeeId
--from 
--tblIssueAssignment a,tblReportIssue b 
--where 
--a.ReportIssueID =b.ReportIssueID 
--and b.SubCategoryID = @SubCategoryID
--and b.ReportIssueID = @reportIssueID 
--order by IssuedAssignmentDate DESC)
--END


if(@counter = '1')
begin
IF @StatusID <> '4'
  BEGIN


   UPDATE tblReportIssue SET DescriptionAndComments = COALESCE(DescriptionAndComments,'') + @comments, StatusID = '4'
   WHERE ReportIssueID = @reportIssueID
   declare @employeeid1 int
   set @employeeid1 =( Select employeeid from tblIssueAssignment  WHERE ReportIssueID = @reportIssueID and StatusID = 3)
   



   INSERT INTO tblIssueAssignment(EmployeeID, ReportIssueID, StatusID, IssuedAssignmentDate)
   VALUES(@employeeid1, @reportIssueID, '4', getdate())
  END
 ELSE 
 BEGIN
   UPDATE tblReportIssue SET DescriptionAndComments =  COALESCE(DescriptionAndComments,'') + @comments
   WHERE ReportIssueID = @reportIssueID
  END
  end
  
  else
   if(@counter ='2')
   begin
   
   IF @StatusID <> '8' 
  BEGIN
   UPDATE tblReportIssue SET DescriptionAndComments = COALESCE(DescriptionAndComments,'') + @comments, StatusID = '9'
   WHERE ReportIssueID = @reportIssueID

   INSERT INTO tblIssueAssignment(EmployeeID, ReportIssueID, StatusID, IssuedAssignmentDate)
   VALUES(@EmployeeId, @reportIssueID, '9', getdate())
  END
  
 ELSE 
 BEGIN
   UPDATE tblReportIssue SET DescriptionAndComments =  COALESCE(DescriptionAndComments,'') + @comments
   WHERE ReportIssueID = @reportIssueID
  END
   
   end
   
   
   else
      if(@counter ='3')
      
      begin
         UPDATE tblReportIssue SET DescriptionAndComments = COALESCE(DescriptionAndComments,'') + @comments, StatusID = @StatusID
   WHERE ReportIssueID = @reportIssueID

   --INSERT INTO tblIssueAssignment(EmployeeID, ReportIssueID, StatusID, IssuedAssignmentDate)
   --VALUES(@EmployeeId, @reportIssueID, @StatusID, getdate())
      end
      

--' ' + description +
--+ CHAR(13)+CHAR(10)  + Convert(varchar(30),getdate(),100) + ' ' + @UserName +

 if @StatusID = 9
	 begin
		declare @IssueResolvedDate datetime, @IssuedAssignmentDate datetime
		set @IssuedAssignmentDate = ( Select top 1 IssuedAssignmentDate  from tblIssueAssignment  WHERE ReportIssueID = @reportIssueID ORDER BY IssueAssignmentID  DESC )
		
	
		set @IssueResolvedDate =  ISNULL(( Select IssueResolvedDate  from tblIssueAssignment  WHERE ReportIssueID = @reportIssueID and StatusID = 3),@IssuedAssignmentDate) 
		 UPDATE tblIssueAssignment SET IssueResolvedDate =  @IssueResolvedDate WHERE ReportIssueID = @reportIssueID and StatusID = @StatusID
 

  
	 end