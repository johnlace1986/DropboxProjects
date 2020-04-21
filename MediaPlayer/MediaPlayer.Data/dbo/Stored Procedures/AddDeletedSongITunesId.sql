CREATE PROCEDURE [AddDeletedSongITunesId]
(
	@iTunesId AS SMALLINT
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [DeletedSongITunesId] WHERE [iTunesId] = @iTunesId)
	BEGIN
		RAISERROR('Specified iTunes ID already exists', 11, 1)
		RETURN
	END
	
	INSERT INTO [DeletedSongITunesId]
	(
		[iTunesId]
	)
	VALUES
	(
		@iTunesId
	)
END