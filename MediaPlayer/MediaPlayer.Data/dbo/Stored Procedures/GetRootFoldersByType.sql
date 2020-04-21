CREATE PROCEDURE [GetRootFoldersByType]
(
	@MediaItemType AS SMALLINT
)
AS
BEGIN
	SELECT
		[Priority],
		[MediaItemType],
		[Path]
	FROM
		[RootFolder]
	WHERE
		[MediaItemType] = @MediaItemType
	ORDER BY
		[Priority]
END