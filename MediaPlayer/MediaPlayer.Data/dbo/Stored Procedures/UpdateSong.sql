CREATE PROCEDURE [dbo].[UpdateSong]
(
	@MediaItemId AS BIGINT,
	@Type AS SMALLINT,
	@Name AS VARCHAR(255),
	@IsHidden AS BIT,
	@DateCreated AS DATETIME,
	@DateModified AS DATETIME,
	@Genre AS VARCHAR(50),
	@Artist AS VARCHAR(50),
	@Album AS VARCHAR(50),
	@DiskNumber AS SMALLINT,
	@NumberOfDisks AS SMALLINT,
	@TrackNumber AS SMALLINT,
	@NumberOfTracks AS SMALLINT,
	@Year AS SMALLINT,
	@iTunesId AS SMALLINT,
	@UserName AS VARCHAR(255)
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @Type)
	BEGIN
		RAISERROR('Specified Song ID does not exist', 11, 1)
		RETURN
	END
	
	IF NOT EXISTS (SELECT 1 FROM [Song] WHERE [MediaItemId] = @MediaItemId AND [MediaItemType] = @Type)
	BEGIN
		RAISERROR('Specified Song ID does not exist', 11, 1)
		RETURN
	END
	
	UPDATE
		[MediaItem]
	SET
		[Name] = @Name,
		[Genre] = @Genre,
		[IsHidden] = @IsHidden,
		[DateCreated] = @DateCreated,
		[DateModified] = @DateModified,
		[UserName] = @UserName
	WHERE
		[Id] = @MediaItemId
	AND
		[Type] = @Type
	
	UPDATE
		[Song]
	SET
		[Artist] = @Artist,
		[Album] = @Album,
		[DiskNumber] = @DiskNumber,
		[NumberOfDisks] = @NumberOfDisks,
		[TrackNumber] = @TrackNumber,
		[NumberOfTracks] = @NumberOfTracks,
		[Year] = @Year,
		[iTunesId] = @iTunesId
	WHERE
		[MediaItemId] = @MediaItemId
	AND
		[MediaItemType] = @Type
END