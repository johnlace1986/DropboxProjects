CREATE PROCEDURE [DeleteDeletedSongITunesId]
(
	@iTunesId AS SMALLINT
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [DeletedSongITunesId] WHERE [iTunesId] = @iTunesId)
	BEGIN
		RAISERROR('Specified iTunes ID does not exist', 11, 1)
		RETURN
	END
	
	DELETE FROM
		[DeletedSongITunesId]
	WHERE
		[iTunesId] = @iTunesId
END