CREATE PROCEDURE [UpdateRootFolder]
(
	@MediaItemType AS SMALLINT,
	@Priority AS SMALLINT,
	@Path AS VARCHAR(255)
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [RootFolder] WHERE [MediaItemType] = @MediaItemType AND [Priority] = @Priority)
	BEGIN
		RAISERROR('Root folder does not exist with specified priority', 11, 1)
		RETURN
	END
	
	UPDATE
		[RootFolder]
	SET
		[Path] = @Path
	WHERE
		[MediaItemType] = @MediaItemType
	AND
		[Priority] = @Priority
END