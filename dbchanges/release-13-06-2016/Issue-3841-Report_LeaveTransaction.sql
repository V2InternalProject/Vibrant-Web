USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[Report_LeaveTransaction]    Script Date: 6/13/2016 12:51:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Rahul Ramachandran
-- Create date: 4 Feb 2016
-- Description:	Location wise holiday inclusion.
-- =============================================

ALTER  PROCEDURE [dbo].[Report_LeaveTransaction] --Report_LeaveTransaction 4091,-3,'2016-01-01 00:00:00.000','2016-06-01 00:00:00.000',-3,4091

-- Add the parameters for the stored procedure here
@EmployeeCode int,
@LeaveTypeID int = Null ,
@FromDate Datetime = Null,
@ToDate datetime = Null,
@ShiftID int,
@LoggedInEmployee int 
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @EmployeeDetail TABLE(UserId INT,EmployeeName VARCHAR(max))

	IF(@EmployeeCode=-2)
		INSERT INTO @EmployeeDetail EXEC GetHierarchWiseEmployeeId @LoggedInEmployee

	Declare @ToDate1 varchar(30),@FromDate1 varchar(30)

	set @FromDate1 = convert(nvarchar(12),@FromDate,101) + ' 00:00:00:000'
	set @ToDate1 = convert(nvarchar(12),@ToDate,101) + ' 23:59:59:997'
	
	SELECT LT.LeaveTransactionID, e.UserID, e.EmployeeName,[OrbitPhase2]..shiftmaster.Description as ShiftName, 
	convert(varchar,[OrbitPhase2]..CompensationDetails.AppliedFor,101)as AppliedFor,convert(varchar,LT.TransactionDate,101)  as tdate, LT.Description,
	(case  when Quantity > 0 then cast(Quantity as varchar(10)) +   '  Cr' 	else  cast( Quantity*(-1) as varchar(10) ) +  '  Dr' end )as Quantity1,
	Quantity, LT.LeaveType,	DBO.GetEmployeeLeavebalance_2(LT.userid,LT.Transactiondate) Currentbal into #temp
	FROM [OrbitPhase2]..LeaveTransaction LT 
		LEFT JOIN [OrbitPhase2]..CompensationDetails on LT.CompensationID = [OrbitPhase2]..CompensationDetails.CompensationID
		RIGHT OUTER JOIN  [OrbitPhase2]..vwEmployee_Master e on e.userid = LT.UserID 
		INNER JOIN [OrbitPhase2]..vwEmployee_Master e1 ON e.ReportingTo = e1.EmployeeID
		left join [OrbitPhase2]..ShiftMaster on [OrbitPhase2]..ShiftMaster.shiftid = e.shiftid 
	where e.IsDeleted = 0 AND LT.Quantity !=0  AND (LT.Transactiondate > '01/01/2014 00:00:00.000' 
	AND LT.LeaveTransactionID not in 
	(select LeaveTransactionID from  OrbitPhase2..LeaveTransaction t
		where (t.quantity <0 AND convert(nvarchar(12),t.TransactionDate,101) in 
		(
			SELECT CONVERT(NVARCHAR(12),HolidayDate,101) FROM OrbitPhase2..HolidayMaster WHERE OfficeLocation IS NOT NULL AND
			CHARINDEX
			(
				','+ CAST (officeLocation as varchar)+',',
				CASE 
					WHEN (SELECT OfficeLocation FROM HRMS_tbl_PM_Employee WHERE EmployeeCode=e.UserId)=2 
					THEN ',2,9,11,'
					WHEN (SELECT OfficeLocation FROM HRMS_tbl_PM_Employee WHERE EmployeeCode=e.UserId)=3
					THEN ',3,9,11,' 
					ELSE ',9,10,'
				END
			)>0

		)))
	)
	AND 
	(
		(@EmployeeCode>0  AND e.UserId=@EmployeeCode) OR	
		(@EmployeeCode=-1 AND e.UserId=@LoggedInEmployee) OR    
		(@EmployeeCode=-2 AND e.UserId IN (SELECT UserId FROM @EmployeeDetail)) OR 
		(@EmployeeCode=-3 AND 1=1 ) 
	)
	AND ((@ShiftID<0 and 1=1) or (@ShiftID>0 and  e.shiftid = @ShiftID))
	AND ((@LeaveTypeID<0 and 1=1) or(@LeaveTypeID>0 and LT.LeaveType=@LeaveTypeID))
	order by e.userid , convert(varchar,LT.TransactionDate,102) desc
	
	select * from #temp 
	where cast(tdate as datetime) between cast(@FromDate1 as datetime) and cast(@ToDate1 as datetime) 
	order by cast(tdate as datetime) 
	
	drop table #temp
END

GO