--Select * from tbl_HR_Expense where ExpenseID=1760

--UPDATE tbl_HR_Expense
--SET StageID=4
--where ExpenseID=1760


--INSERT into Tbl_HR_ExpenseStageEvent(ExpenseID,EventDatatime,Action,FromStageId,ToStageId,UserId,Comments)
--Values(1760,GETDATE(),'Closed',2,4,'Closed as per discussion with Finance team')