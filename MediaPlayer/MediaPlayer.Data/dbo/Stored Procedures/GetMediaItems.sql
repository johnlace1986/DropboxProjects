CREATE PROCEDURE [dbo].[GetMediaItems]
AS
BEGIN
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
		[Video].[NumberOfEpisodes],
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
	LEFT OUTER JOIN
		[Song]
	ON
		[MediaItem].[Id] = [Song].[MediaItemId]
	AND
		[MediaItem].[Type] = [Song].[MediaItemType]
	LEFT OUTER JOIN
		[Video]
	ON
		[MediaItem].[Id] = [Video].[MediaItemId]
	AND
		[MediaItem].[Type] = [Video].[MediaItemType]
END