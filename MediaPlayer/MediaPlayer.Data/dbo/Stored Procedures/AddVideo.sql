CREATE PROCEDURE [dbo].[AddVideo]
(
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
	
	INSERT INTO [Video]
	(
		[MediaItemId],
		[MediaItemType],
		[Program],
		[Series],
		[Episode],
		[NumberOfEpisodes]
	)
	VALUES
	(
		@NewId,
		@Type,
		@Program,
		@Series,
		@Episode,
		@NumberOfEpisodes
	)
	
	SELECT @NewId AS [NewId]
END