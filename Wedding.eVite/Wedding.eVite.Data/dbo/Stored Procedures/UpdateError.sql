CREATE PROCEDURE [dbo].[UpdateError]
(
	@ErrorId AS INT
	,@Name AS VARCHAR(500)
	,@DateThrown AS DATETIME
	,@OuterErrorId AS INT
	,@Message AS VARCHAR(1000)
	,@StackTrace AS VARCHAR(4000)
)
AS
BEGIN
	UPDATE
		[Error]
	SET
		[Name] = @Name
		,[DateThrown] = @DateThrown
		,[OuterErrorId] = @OuterErrorId
		,[Message] = @Message
		,[StackTrace] = @StackTrace
	WHERE
		[Id] = @ErrorId
END