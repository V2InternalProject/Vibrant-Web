USE [V2Intranet]
GO

/****** Object:  StoredProcedure [dbo].[GetProjectEndRecords]    Script Date: 1/9/2017 2:12:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[GetProjectEndRecords]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from tbl_pm_project where
 ActualEndDate=(select dateadd(day, 3, CAST(getdate() as date))) OR 
 ActualEndDate =(select dateadd(day, 0, CAST(getdate() as date))) 
END

GO


