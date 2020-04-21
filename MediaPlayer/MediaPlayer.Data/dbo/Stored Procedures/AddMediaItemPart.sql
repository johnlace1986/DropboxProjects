CREATE PROCEDURE [AddMediaItemPart]
(
	@Location AS VARCHAR(255),
	@MediaItemId AS BIGINT,
	@MediaItemType AS SMALLINT,
	@Size AS BIGINT,
	@Duration AS INT,
	@Index AS SMALLINT
)
AS
BEGIN	
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @MediaItemType)
	BEGIN
		RAISERROR('Specified media item ID does not exist', 11, 1)
		RETURN
	END
	
	IF EXISTS (SELECT 1 FROM [MediaItemPart] WHERE [Location] = @Location)
	BEGIN
		RAISERROR('Specified location already exists', 11, 1)
		RETURN
	END
	
	INSERT INTO [MediaItemPart]
	(
		[Location],
		[MediaItemId],
		[MediaItemType],
		[Size],
		[Duration],
		[Index]
	)
	VALUES
	(
		@Location,
		@MediaItemId,
		@MediaItemType,
		@Size,
		@Duration,
		@Index
	)		
END