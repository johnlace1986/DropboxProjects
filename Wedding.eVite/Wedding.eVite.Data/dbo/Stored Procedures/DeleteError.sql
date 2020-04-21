CREATE PROCEDURE [dbo].[DeleteError]
(
	@ErrorId AS INT
)
AS
BEGIN
	WITH errors AS
	(
		SELECT
			[E].[Id]
			,[E].[OuterErrorId]
		FROM
			[Error] AS [E]
		WHERE
			[E].[Id] = @ErrorId

		UNION ALL
		SELECT
			[E].[Id]
			,[E].[OuterErrorId]
		FROM
			[Error] AS [E]
		INNER JOIN
			errors as [subErrors] ON subErrors.id = [E].[OuterErrorId]
	)

	DELETE FROM [Error] WHERE [Id] IN (SELECT id FROM errors)
END