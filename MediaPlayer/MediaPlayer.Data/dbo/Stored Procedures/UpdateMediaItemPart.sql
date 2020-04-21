CREATE PROCEDURE [dbo].[UpdateMediaItemPart]
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

	IF NOT EXISTS (SELECT 1 FROM [MediaItemPart] WHERE [Location] = @Location)
	BEGIN
		RAISERROR('Specified location does not exist', 11, 1)
		RETURN
	END

	IF EXISTS (SELECT 1 FROM [MediaItemPart] WHERE [Location] = @Location AND NOT ([MediaItemId] = @MediaItemId AND [MediaItemType] = @MediaItemType))
	BEGIN
		RAISERROR('Specified location already exists', 11, 1)
		RETURN
	END
	
	UPDATE
		[MediaItemPart]
	SET
		[Size] = @Size,
		[Duration] = @Duration
	WHERE
		[Location] = @Location
END