CREATE PROCEDURE [dbo].[DeleteMediaItem]
(
	@MediaItemId AS BIGINT,
	@Type AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @Type)
	BEGIN
		RAISERROR('Specified media item ID does not exist', 11, 1)
		RETURN
	END
	
	DELETE FROM
		[MediaItem]
	WHERE
		[Id] = @MediaItemId
	AND
		[Type] = @Type
END