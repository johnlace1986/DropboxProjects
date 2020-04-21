CREATE PROCEDURE [GetErrorByOuterErrorId]
(
	@OuterErrorId AS INT
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
		[OuterErrorId] = @OuterErrorId
	ORDER BY
		[DateThrown] DESC
END