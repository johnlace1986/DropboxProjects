CREATE PROCEDURE [GetDeletedSongITunesIds]
AS
BEGIN
	SELECT
		[iTunesId]
	FROM	
		[DeletedSongITunesId]
	ORDER BY
		[iTunesId]
END