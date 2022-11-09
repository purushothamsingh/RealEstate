DECLARE @UserID as int
SET @UserID = (select ID from Db_Registers where UserName='santo')
delete from Users where Username='santo'
delete from PropertyTypes where LastUpdatedBy=@UserID
delete from FurnishingTypes where LastUpdatedBy=@UserID
delete from Cities where LastUpdatedBy=@UserID
delete from Properties where PostedBy=@UserId