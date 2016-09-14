select EmployeeStatusID,ConfirmationDate,* from HRMS_tbl_PM_Employee where EmployeeCode in (2052
,2053
,2054
,2001
,2038
,2051
) order by EmployeeName


update HRMS_tbl_PM_Employee
SET EmployeeStatusID=1
where EmployeeCode in (2052
,2053
,2054
,2001
,2038
,2051)

update HRMS_tbl_PM_Employee
SET ConfirmationDate=HRMS_tbl_PM_Employee.JoiningDate
where EmployeeCode in (
2052
,2053
,2054
,2001
,2038
,2051)