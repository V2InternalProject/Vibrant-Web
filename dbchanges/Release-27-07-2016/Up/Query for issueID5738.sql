update tbl_HR_ExitInstance
set stageID=4
where ExitInstanceId=2554

update Tbl_HR_ExitStageEvent
set ToStageId=4
where ExitInstanceId=2554 and Id=6024



update tbl_HR_ExitInstance
set stageID=5
where ExitInstanceId=2554


update Tbl_HR_ExitStageEvent
set ToStageId=5
where ExitInstanceId=2554 and Id=7435