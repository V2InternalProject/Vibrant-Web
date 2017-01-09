select * from HRMS_EmailTemplates where EmailTemplateId=96

Insert into HRMS_EmailTemplates(EmailTemplateName,EmailSubject,EmailBody) Values ('Confirmation Process Initiation Befor Probation','Confirmation Process Initiation For ##employeename##','Hello ##reportingmanage##,<br>This is to notify you that the confirmation process for ##employeename## has been initiated on ##probationdate## in VibrantWeb.<br>Kindly fill the confirmation form on ##probationdate##.<br><br>Regards,<br>##logged in user##')
