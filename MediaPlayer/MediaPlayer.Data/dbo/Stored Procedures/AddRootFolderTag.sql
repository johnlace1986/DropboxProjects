CREATE PROCEDURE [dbo].[AddRootFolderTag]
(
	@RootFolderPriority AS SMALLINT,
	@MediaItemType AS SMALLINT,
	@Tag AS VARCHAR(50)
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM [RootFolder] WHERE [Priority] = @RootFolderPriority AND [MediaItemType] = @MediaItemType)
	BEGIN
		RAISERROR('Specified root folder ID does not exist', 11, 1)
		RETURN
	END
	
	IF EXISTS (SELECT 1 FROM [RootFolderTag] WHERE [RootFolderPriority] = @RootFolderPriority AND [MediaItemType] = @MediaItemType AND [Tag] = @Tag)
	BEGIN
		RAISERROR('Specified tag already exists', 11, 1)
		RETURN
	END
	
	INSERT INTO [RootFolderTag]
	(
		[RootFolderPriority],
		[MediaItemType],
		[Tag]
	)
	VALUES
	(
		@RootFolderPriority,
		@MediaItemType,
		@Tag
	)
END