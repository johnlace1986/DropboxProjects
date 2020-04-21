CREATE PROCEDURE [AddMediaItemTag]
(
	@MediaItemId AS BIGINT,
	@MediaItemType AS SMALLINT,
	@Tag AS VARCHAR(50)
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @MediaItemType)
	BEGIN
		RAISERROR('Specified media item ID does not exist', 11, 1)
		RETURN
	END
	
	IF EXISTS (SELECT 1 FROM [MediaItemTag] WHERE [MediaItemId] = @MediaItemId AND [MediaItemType] = @MediaItemType AND [Tag] = @Tag)
	BEGIN
		RAISERROR('Specified tag already exists', 11, 1)
		RETURN
	END
	
	INSERT INTO [MediaItemTag]
	(
		[MediaItemId],
		[MediaItemType],
		[Tag]
	)
	VALUES
	(
		@MediaItemId,
		@MediaItemType,
		@Tag
	)
END