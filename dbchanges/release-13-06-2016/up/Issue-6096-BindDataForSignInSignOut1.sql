USE [OrbitPhase2]
GO

/****** Object:  StoredProcedure [dbo].[BindDataForSignInSignOut1]    Script Date: 6/13/2016 1:24:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[BindDataForSignInSignOut1]   -- exec BindDataForSignInSignOut1 1195,'date','desc'
@EmployeeID int	,@ColumnName nvarchar(20), @SortOrder nvarchar(10)
AS
Declare @Initial nvarchar(2000)
Declare @Append nvarchar(1000)
Declare @Final nvarchar(3000)
declare @ShiftName varchar(50)
DECLARE @Location tinyint
BEGIN
if(@ColumnName = 'TotalHoursWorked') 
Set @ColumnName = 'Hours'

Set @Initial = 'SELECT  SignInSignOutID, case when signintime is not null then convert(varchar, a.SignInTime,102)  else convert(varchar, a.SignOutTime,102) end as [date]
       ,convert(varchar, a.SignInTime,108)as SignInTime
      ,convert(varchar, a.SignOutTime,108) as SignOutTime
      ,a.TotalHoursWorked
      ,convert(varchar(10),(a.IsSignInManual | a.IsSignOutManual)) as Mode      
      ,a.SignInComment
      ,a.SignOutComment      
      ,b.statusName as Status
      ,a.IsBulk
      ,c.EmployeeName 
      ,a.ApproverComments 
      ,datediff(minute,SignInTime,SignOutTime) as Hours
        FROM SignInSignOut a left join StatusMaster b on a.statusID = b.statusID 
  left join vwEmployee_Master c on a.ApproverID = c.UserID
  where  (a.UserID = '+cast(@EmployeeID as varchar(10))+')' 
   
      
  

set @Append = 'order by ' + @ColumnName + ' '  + @SortOrder
Set @Final = @Initial+@Append

--print @Final
exec sp_executeSQL @Final

--select HolidayDate from HolidayMaster
SELECT @Location=OfficeLocation FROM V2Intranet.dbo.HRMS_tbl_PM_Employee WHERE EmployeeCode= @EmployeeID
IF(@Location=2)
	BEGIN
		SELECT  HolidayDate   FROM HolidayMaster
		where  officeLocation in (2,11,9)
	END
	ELSE IF(@Location=3)
	BEGIN
		SELECT HolidayDate  FROM HolidayMaster
		 where	 officeLocation in (3,11,9)
	END
	ELSE
	BEGIN
		SELECT  HolidayDate   FROM HolidayMaster
			where 	 officeLocation in (9,10)
	END

SELECT [ConfigItemID]
      ,[ConfigItemName]
      ,[ConfigItemValue]
      ,[ConfigItemDescription]
  FROM [ConfigItem] where ConfigItemName  = 'Freezing Date'

SELECT @ShiftName=  r.RoleName
    FROM   V2Tools.dbo.aspnet_Roles r, V2Tools.dbo.aspnet_UsersInRoles ur,V2Tools.dbo.aspnet_Users u
    WHERE  r.RoleId = ur.RoleId  AND u.Username = @EmployeeID and u.userid = ur.userid and r.RoleName='Shift'
    ORDER BY r.RoleName

select isnull(@ShiftName,'General') as ShiftName

select HolidayDate from HolidayMaster where isholidayforshift=1



END

GO