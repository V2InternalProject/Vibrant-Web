USE [V2Intranet]
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerEndRecords]    Script Date: 1/10/2017 5:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mahesh Farad
-- Create date: 10th jan 2017 
-- Description:	To Get CustomerEndRecord for Alert
-- =============================================
ALTER PROCEDURE  [dbo].[GetCustomerEndRecords]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from tbl_PM_Customer where
 ContractValidityDate=(select dateadd(day, 7, CAST(getdate() as date))) OR 
 ContractValidityDate =(select dateadd(day, 0, CAST(getdate() as date))) 
END