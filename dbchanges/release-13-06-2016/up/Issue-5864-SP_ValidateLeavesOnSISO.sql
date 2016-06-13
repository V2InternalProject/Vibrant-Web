USE [OrbitPhase2]
GO

/****** Object:  StoredProcedure [dbo].[SP_ValidateLeavesOnSISO]    Script Date: 6/13/2016 1:04:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Rahul Ramachandran
-- Create date: 09th June 2016
-- Description:	To validate leaves on the day when user is present(called while uploading the biometric excel).
-- =============================================
CREATE PROCEDURE [dbo].[SP_ValidateLeavesOnSISO] 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @MyCursor CURSOR
	DECLARE @EmployeeCode int,@today date;
	set dateformat dmy		
	SELECT TOP 1 @today=date from TempAttendance
	set dateformat mdy
	print @today

	SET @MyCursor = CURSOR FAST_FORWARD
	FOR	
	SELECT UserID from OrbitPhase2..LeaveTransaction WHERE TransactionDate=@today
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @EmployeeCode
	WHILE @@FETCH_STATUS = 0
	BEGIN	
		IF EXISTS(SELECT * FROM OrbitPhase2..SignInSignOut WHERE (CONVERT(varchar(10), SignInTime,101)=CONVERT(varchar(10), @today,101) OR CONVERT(varchar(10), SignOutTime,101)=CONVERT(varchar(10), @today,101)) AND UserID=@EmployeeCode)
		BEGIN
		print @EmployeeCode
			UPDATE OrbitPhase2..LeaveTransaction
			SET Quantity=0
			WHERE UserID=@EmployeeCode AND TransactionDate=@today

			UPDATE OrbitPhase2..LeaveDetails
			SET TotalLeaveDays=TotalLeaveDays-1 
			WHERE CONVERT(varchar(10),@today,101)>=CONVERT(VARCHAR(10),LeaveDateFrom,101) AND CONVERT(varchar(10),@today,101)<=CONVERT(VARCHAR(10),LeaveDateTo,101)
			AND userid=@EmployeeCode AND StatusID=2
		END

		FETCH NEXT FROM @MyCursor
		INTO @EmployeeCode
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor

END

GO