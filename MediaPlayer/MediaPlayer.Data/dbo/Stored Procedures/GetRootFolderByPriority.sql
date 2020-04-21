CREATE PROCEDURE [dbo].[GetRootFolderByPriority]
(
	@MediaItemType AS SMALLINT,
	@Priority AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [RootFolder] WHERE [MediaItemType] = @MediaItemType AND [Priority] = @Priority)
	BEGIN
		RAISERROR('Root folder does not exist with specified priority', 11, 1)
		RETURN
	END
	
	SELECT
		[MediaItemType],
		[Priority],
		[Path]
	FROM
		[RootFolder]
	WHERE
		[MediaItemType] = @MediaItemType
	AND
		[Priority] = @Priority
END