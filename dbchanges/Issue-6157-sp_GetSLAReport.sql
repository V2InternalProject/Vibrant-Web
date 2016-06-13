USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetSLAReport]    Script Date: 6/13/2016 12:40:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--EXEC sp_GetResolutionTimeDetails 0, -1,'1 January 2013','1 March 2008',2006

ALTER PROCEDURE [dbo].[sp_GetSLAReport] 

@DepartmentID INT,
@FromDate DATETIME,
@ToDate DATETIME

AS
 
BEGIN
	SELECT DISTINCT r.ReportIssueID as IssueID, r.Description as HelpDeskName, st.SubCategory as Category, r.Name as Requester, Category Department, StatusDesc as Status,

	CASE -- To check column IssueType
	WHEN r.typeid = '1' THEN 'Request'
	WHEN r.typeid = '2' THEN 'Issue'
	ELSE 'Not Set' END as Type,

	c.ProblemSeverity as ProblemSeverity, isnull(b.EmployeeName,'') as AssignedTo, r.ReportIssueDate as SubmittedDate,

	CASE -- To check the issue resolution date
	WHEN a.IssueResolvedDate='1900-01-01 00:00:00.000' THEN '' 
	ELSE a.IssueResolvedDate END as ResolvedOn,

	CASE -- To check the SLA Met or Not Met as per the PK_Statusid and IssueHealth
	WHEN (PK_Statusid=3 or PK_Statusid=9) THEN (CASE WHEN (a.IssueHealth = 1 /*1 for Green*/ OR a.IssueHealth = 2 /*2 for Amber*/ ) THEN 'MET' ELSE 'NOT MET' END)
	WHEN (PK_Statusid=1) THEN 'NOT ACKNOWLEDGED'
	WHEN (PK_Statusid=4) THEN 'NOT MET'
	--WHEN (PK_Statusid=2 or PK_Statusid=4 or PK_Statusid=5 or PK_Statusid=6 or PK_Statusid=7 or PK_Statusid=9) THEN 'NOT MET'
	--For all the other PK_Statusid 2, 4, 5, 6, 7 and 9 it will be 'NOT MET'
	ELSE 'NOT OCCURED'
	END as ResolutionHealth,
	 
	DATENAME(MONTH,r.ReportIssueDate) as [Month],DATENAME(YEAR,r.ReportIssueDate) as [Year] 
	FROM tblReportIssue r 
	left join vwResolutionsTimeDetailsNew a on  r.ReportIssueId = a.ReportIssueId
	left join tblHelpdeskEmployee b on  b.EmployeeID = a.AssignEmployeeID
	left join tblProblemSeverityMaster c on  r.ProblemSeverityID =  c.ProblemSeverityID
	left join helpdeskStatus s on s.pk_StatusId=r.Statusid
	left join tblSubCategoryMaster st on r.Subcategoryid = st.Subcategoryid
	left join tblCategoryMaster t on t.CategoryId= st.Categoryid
	  
	--	 comparing only date not time 
	WHERE t.Categoryid = @DepartmentID and (DATEADD(dd, 0, DATEDIFF(dd, 0, r.ReportIssueDate)) between 
	DATEADD(dd, 0, DATEDIFF(dd, 0,@FromDate)) and  DATEADD(dd, 0, DATEDIFF(dd, 0, @ToDate)))  AND
	s.PK_StatusId not in (8)
	ORDER BY r.ReportIssueID DESC
END

GO