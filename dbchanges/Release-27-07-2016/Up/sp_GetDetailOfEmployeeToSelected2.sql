USE [V2Intranet]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDetailOfEmployeeToSelected2]    Script Date: 8/10/2016 11:58:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--USE [HelpDesk]  
--GO  
--/****** Object:  StoredProcedure [dbo].[sp_GetDetailOfEmployeeToSelected1]    Script Date: 03/07/2013 12:18:22 ******/  
--SET ANSI_NULLS ON  
--GO  
--SET QUOTED_IDENTIFIER ON  
--GO  
ALTER  procedure [dbo].[sp_GetDetailOfEmployeeToSelected2] --[sp_GetDetailOfEmployeeToSelected2] 1008  
@EmployeeId int  
  
as    
 /* select *  
 from tblCategoryMaster x,  
 (select CategoryID,isAdmin   
 from tblHelpdeskEmployeeRoles   
 where EmployeeID= @SuperAdminEmployeeId AND IsAdmin=1) y            
 where x.isActive=1  
 and x.CategoryID *= y.CategoryID*/  
create table #temp5 (CategoryID int,Category varchar(50) ,isActive bit,IsAdmin bit )


insert into #temp5
Select b.CategoryID,b.Category,b.isActive,a.IsAdmin  from tblHelpdeskEmployeeRoles a LEFT JOIN  (Select CategoryID,Category,isActive from tblCategoryMaster where CategoryID  in (  
select CategoryID from tblHelpdeskEmployeeRoles --where IsAdmin =1  
) and isActive =1)b  ON b.CategoryID = a.CategoryID
where a.EmployeeID=@EmployeeId  
 and b.isActive =1  
Select a.CategoryID,a.Category,a.isActive ,a.CategoryID ,b.IsAdmin from #temp5 b right join  tblCategoryMaster a on (b.CategoryID=a.CategoryID) where a.isActive=1

 declare @testcategoryID1 int  
 declare getcategoryid  
 cursor for   
    select distinct(x.CategoryId)   
    from tblCategoryMaster x
    WHERE   
    --y.EmployeeID = @SuperAdminEmployeeId and  
 --x.CategoryID= y.CategoryID  
--AND  y.IsAdmin=1  
    x.IsActive=1  
 open getcategoryid  
 Fetch Next from getcategoryid into @testcategoryID1  
 while @@FETCH_STATUS=0  
 BEGIN   
 Select   
 distinct(b.Subcategory ), b.SubCategoryID  
 from   
 tblSubCategoryAssignment a,  
 tblSubCategoryMaster b   
 where  a.EmployeeID=@EmployeeId  and  
  b.CategoryID = @testcategoryID1  and b.IsActive=1  
 and b.SubCategoryID =a.SubCategoryID  
 Fetch Next from getcategoryid into @testcategoryID1  
 end  
 close getcategoryid  
 deallocate getcategoryid  
  drop table #temp5
  
  
  
  
  

