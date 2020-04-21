CREATE PROCEDURE [dbo].[UpdateVideo]
(
	@MediaItemId AS BIGINT,
	@Type AS SMALLINT,
	@Name AS VARCHAR(255),
	@IsHidden AS BIT,
	@DateCreated AS DATETIME,
	@DateModified AS DATETIME,
	@Genre AS VARCHAR(50),
	@Program AS VARCHAR(50),
	@Series AS SMALLINT,
	@Episode AS SMALLINT,
	@NumberOfEpisodes AS SMALLINT,
	@UserName AS VARCHAR(255)
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [MediaItem] WHERE [Id] = @MediaItemId AND [Type] = @Type)
	BEGIN
		RAISERROR('Specified video ID does not exist', 11, 1)
		RETURN
	END
	
	IF NOT EXISTS (SELECT 1 FROM [Video] WHERE [MediaItemId] = @MediaItemId AND [MediaItemType] = @Type)
	BEGIN
		RAISERROR('Specified video ID does not exist', 11, 1)
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
		[Video]
	SET
		[Program] = @Program,
		[Series] = @Series,
		[Episode] = @Episode,
		[NumberOfEpisodes] = @NumberOfEpisodes
	WHERE
		[MediaItemId] = @MediaItemId
	AND
		[MediaItemType] = @Type
END