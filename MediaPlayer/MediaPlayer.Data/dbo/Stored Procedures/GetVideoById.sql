CREATE PROCEDURE [dbo].[GetVideoById]
(
	@VideoId AS BIGINT
)
AS
BEGIN

	DECLARE @Type SMALLINT
	SET @Type = 0

	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE ([Id] = @VideoId AND [Type] = @Type))
	BEGIN
		RAISERROR('Specified video ID does not exist', 11, 1)
		RETURN
	END
	
	SELECT
		[MediaItem].[Id],
		[MediaItem].[Type],
		[MediaItem].[Name],
		[MediaItem].[IsHidden],
		[MediaItem].[DateCreated],
		[MediaItem].[DateModified],
		[MediaItem].[Genre],
		[MediaItem].[UserName],
		[Video].[Program],
		[Video].[Series],
		[Video].[Episode],
		[Video].[NumberOfEpisodes]
	FROM
		[MediaItem]
	INNER JOIN
		[Video]
	ON
		[MediaItem].[Id] = [Video].[MediaItemId]
	AND
		[MediaItem].[Type] = [Video].[MediaItemType]
	WHERE
		[MediaItem].[Id] = @VideoId
	AND
		[MediaItem].[Type] = @Type
END