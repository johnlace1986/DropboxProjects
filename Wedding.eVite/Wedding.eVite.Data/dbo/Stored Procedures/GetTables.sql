CREATE PROCEDURE [GetTables]
AS
BEGIN
	SELECT
		[Id] AS [TableId]
	FROM
		[Table]
	ORDER BY
		[Id]
END