CREATE PROCEDURE [dbo].[GetErrors]
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
		[OuterErrorId] IS NULL
	ORDER BY
		[DateThrown] DESC
END