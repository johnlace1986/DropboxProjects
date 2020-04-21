CREATE PROCEDURE [GetFileTypes]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[MediaItemType]
	FROM
		[FileType]
	ORDER BY
		[Name]
END