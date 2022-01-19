-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertTransaction] 
	@UserId int,
	@Date datetime,
	@Mount decimal(18,2),
	@Note nchar(1000) = null,
	@CategoryId int,
	@CountId int,
	@TransactionTypeId int,
	@Condition bit

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    insert into [Transaction](UserId, DateTransaction,CountId,CategoryId, Mount, Note, TransactionTypeId, Condition)
	values(
		@UserId,
		@Date,	
		@CountId,		
		@CategoryId,
		abs(@Mount),
		@Note,
		@TransactionTypeId,
		@Condition 
	)

	UPDATE Counts
	SET Balance += @Mount
	Where Id = @CountId
	and Condition = @Condition
	SELECT SCOPE_IDENTITY();
END
