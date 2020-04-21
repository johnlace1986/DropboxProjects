CREATE PROCEDURE [dbo].[GetSongById]
(
	@SongId AS BIGINT
)
AS
BEGIN

	DECLARE @Type SMALLINT
	SET @Type = 1

	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE ([Id] = @SongId AND [Type] = @Type))
	BEGIN
		RAISERROR('Specified Song ID does not exist', 11, 1)
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
		[Song].[Artist],
		[Song].[Album],
		[Song].[DiskNumber],
		[Song].[NumberOfDisks],
		[Song].[TrackNumber],
		[Song].[NumberOfTracks],
		[Song].[Year],
		[Song].[iTunesId]
	FROM
		[MediaItem]
	INNER JOIN
		[Song]
	ON
		[MediaItem].[Id] = [Song].[MediaItemId]
	AND
		[MediaItem].[Type] = [Song].[MediaItemType]
	WHERE
		[MediaItem].[Id] = @SongId
	AND
		[MediaItem].[Type] = @Type
END