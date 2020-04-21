CREATE PROCEDURE [dbo].[AddRootFolder]
(
	@MediaItemType AS SMALLINT,
	@Path AS VARCHAR(255)
)
AS
BEGIN
	
	IF EXISTS (SELECT 1 FROM [RootFolder] WHERE [MediaItemType] = @MediaItemType AND [Path] = @Path)
	BEGIN
		RAISERROR('Root folder already exists with specified path', 11, 1)
		RETURN
	END
	
	--get priority
	DECLARE @Priority SMALLINT
	SELECT
		@Priority = COUNT(1)
	FROM
		[RootFolder]
	WHERE
		[MediaItemType] = @MediaItemType
	
	--insert into table
	INSERT INTO [RootFolder]
	(
		[MediaItemType],
		[Priority],
		[Path]
	)
	VALUES
	(
		@MediaItemType,
		@Priority,
		@Path
	)
	
	--select priority
	SELECT @Priority AS [NewPriority]
END