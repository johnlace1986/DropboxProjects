CREATE PROCEDURE [dbo].[AddSong]
(
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
	--generate new ID number
	DECLARE @TypeString VARCHAR(50)
	SET @TypeString = CONVERT(VARCHAR(50), @Type)
	
	DECLARE @TypeWhereClause VARCHAR(50)
	SET @TypeWhereClause = '[Type] = ' + @TypeString
	
	DECLARE @NewId BIGINT
	EXEC [GenerateIdNumber]
		@TableName = 'MediaItem',
		@IdColumnName = '[Id]',
		@WhereClause = @TypeWhereClause,
		@NewId = @NewId OUT

	INSERT INTO [MediaItem]
	(
		[Id],
		[Type],
		[Name],
		[Genre],
		[IsHidden],
		[DateCreated],
		[DateModified],
		[UserName]
	)
	VALUES
	(
		@NewId,
		@Type,
		@Name,
		@Genre,
		@IsHidden,
		@DateCreated,
		@DateModified,
		@UserName
	)
	
	INSERT INTO [Song]
	(
		[MediaItemId],
		[MediaItemType],
		[Artist],
		[Album],
		[DiskNumber],
		[NumberOfDisks],
		[TrackNumber],
		[NumberOfTracks],
		[Year],
		[iTunesId]
	)
	VALUES
	(
		@NewId,
		@Type,
		@Artist,
		@Album,
		@DiskNumber,
		@NumberOfDisks,
		@TrackNumber,
		@NumberOfTracks,
		@Year,
		@iTunesId
	)
	
	SELECT @NewId AS [NewId]
END