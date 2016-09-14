--select * from tbl_HR_Expense where ExpenseID in (
--1693,
--1811,
--1836
--)

--Update tbl_HR_Expense
--SET StageID=4
--Where ExpenseID in (
--1693,
--1811,
--1836
--) 

--INSERT into Tbl_HR_ExpenseStageEvent(ExpenseID,EventDatatime,Action,FromStageId,ToStageId,UserId,Comments)
--Values(ExpenseID,GETDATE(),'Closed',1,4,'Closed as per discussion with Finance team')