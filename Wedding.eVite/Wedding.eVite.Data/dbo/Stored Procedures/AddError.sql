CREATE PROCEDURE [dbo].[AddError]
(
	@Name AS VARCHAR(500)
	,@DateThrown AS DATETIME
	,@OuterErrorId AS INT
	,@Message AS VARCHAR(1000)
	,@StackTrace AS VARCHAR(4000)
)
AS
BEGIN
	INSERT INTO [Error]
	(
		[Name]
		,[DateThrown]
		,[OuterErrorId]
		,[Message]
		,[StackTrace]
	)
	VALUES
	(
		@Name
		,@DateThrown
		,@OuterErrorId
		,@Message
		,@StackTrace
	)

	DECLARE @ErrorId INT
	SET @ErrorId = SCOPE_IDENTITY()

	SELECT @ErrorId AS [NewErrorId]
END