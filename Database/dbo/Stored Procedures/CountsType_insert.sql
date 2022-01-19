
CREATE PROCEDURE [dbo].[CountsType_insert]
	@name nvarchar(50),
	@userId int
as
BEGIN
	SET NOCOUNT ON;
	declare @order int 
	select @order = coalesce(max(OrderType), 0) + 1
	from CountsType
	where UserId = @userId

	Insert into CountsType(Name,UserId,OrderType) values(@name, @userId, @order)
	Select SCOPE_IDENTITY()
END
