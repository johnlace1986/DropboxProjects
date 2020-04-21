CREATE PROCEDURE [dbo].[GetErrorById]
(
	@ErrorId AS INT
)
AS
BEGIN
	SELECT
		[Id] AS [ErrorId]
		,[Name]
		,[DateThrown]
		,[OuterErrorId]
		,[Message]
		,[StackTrace]
	FROM
		[Error]
	WHERE
		[Id] = @ErrorId
END