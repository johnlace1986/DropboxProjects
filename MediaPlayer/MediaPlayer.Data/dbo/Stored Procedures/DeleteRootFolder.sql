CREATE PROCEDURE [dbo].[DeleteRootFolder]
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
		
	--delete root folder
	DELETE FROM
		[RootFolder]
	WHERE
		[MediaItemType] = @MediaItemType
	AND
		[Priority] = @Priority
		
	--update priorities of remaining root folders
	UPDATE
		[RootFolder]
	SET
		[Priority] = [Priority] - 1
	WHERE
		[MediaItemType] = @MediaItemType
	AND
		[Priority] > @Priority
END