CREATE PROCEDURE [dbo].[GetRootFolderTagsByPriority]
(
	@RootFolderPriority AS SMALLINT,
	@MediaItemType AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [RootFolder] WHERE [Priority] = @RootFolderPriority AND [MediaItemType] = @MediaItemType)
	BEGIN
		RAISERROR('Specified root folder ID does not exist', 11, 1)
		RETURN
	END
	
	SELECT
		[RootFolderPriority],
		[MediaItemType],
		[Tag]
	FROM
		[RootFolderTag]
	WHERE
		[RootFolderPriority] = @RootFolderPriority
	AND
		[MediaItemType] = @MediaItemType
	ORDER BY
		[Tag]
END