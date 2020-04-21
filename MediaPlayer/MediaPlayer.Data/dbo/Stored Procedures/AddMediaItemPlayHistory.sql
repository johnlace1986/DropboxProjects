CREATE PROCEDURE [AddMediaItemPlayHistory]
(
	@MediaItemId AS BIGINT,
	@MediaItemType AS SMALLINT,
	@DatePlayed AS DATETIME
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @MediaItemType)
	BEGIN
		RAISERROR('Specified media item ID does not exist', 11, 1)
		RETURN
	END
	
	IF EXISTS (SELECT 1 FROM [MediaItemPlayHistory] WHERE [MediaItemId] = @MediaItemId AND [MediaItemType] = @MediaItemType AND [DatePlayed] = @DatePlayed)
	BEGIN
		RAISERROR('Specified date played already exists in media item', 11, 1)
		RETURN
	END
	
	INSERT INTO [MediaItemPlayHistory]
	(
		[MediaItemId],
		[MediaItemType],
		[DatePlayed]
	)
	VALUES
	(
		@MediaItemId,
		@MediaItemType,
		@DatePlayed
	)
END